using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using System.Linq;
using log4net;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.IEOD;
using System.Configuration;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class ConsultaMedidoresAppServicio : AppServicioBase
    {
        ExcelPackage xlPackage = null;

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaMedidoresAppServicio));

        /// <summary>
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1 && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListaTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia()
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1 && x.Fenergcodi != 0).ToList();
        }

        /// <summary>
        /// Permite listar las empresas
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ListaEmpresa()
        {
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasSEIN();
        }

        /// <summary>
        /// Permite obtener las empresa por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipo(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return (new IEODAppServicio()).ListarEmpresasTienenCentralGenxTipoEmpresa(tiposEmpresa);
        }

        public List<SiEmpresaDTO> ListarEmpresasRsfBorne(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return (new IEODAppServicio()).ListarEmpresasRsfBorne(tiposEmpresa);
        }        

        /// <summary>
        /// Permite obtener las empresas por tipo del sin
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipoGeneralSein(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasPorTipoSEIN(tiposEmpresa);
        }

        ///// <summary>
        ///// Permite obtener las empresa por tipo
        ///// </summary>
        ///// <returns></returns>
        //public List<SiEmpresaDTO> ObteneEmpresasPorTipoGeneral(string tiposEmpresa)
        //{
        //    if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
        //    return FactorySic.GetSiEmpresaRepository().ObtenerEmpresasPorTipo(tiposEmpresa);
        //}

        public string GetFuenteSSAA(string tiposGeneracion)
        {
            string[] codigos = tiposGeneracion.Split(ConstantesAppServicio.CaracterComa);
            string fuentes = string.Empty;

            foreach (string item in codigos)
            {
                int id = int.Parse(item);
                if (id == 1)
                {
                    if (!string.IsNullOrEmpty(fuentes)) fuentes = fuentes + ConstantesAppServicio.CaracterComa.ToString() + 20.ToString();
                    else fuentes = fuentes + 20.ToString();
                }
                else if (id == 2)
                {
                    if (!string.IsNullOrEmpty(fuentes)) fuentes = fuentes + ConstantesAppServicio.CaracterComa.ToString() + 21.ToString();
                    else fuentes = fuentes + 21.ToString();
                }
                else if (id == 3)
                {
                    if (!string.IsNullOrEmpty(fuentes)) fuentes = fuentes + ConstantesAppServicio.CaracterComa.ToString() + 22.ToString();
                    else fuentes = fuentes + 22.ToString();
                }
                else if (id == 4)
                {
                    if (!string.IsNullOrEmpty(fuentes)) fuentes = fuentes + ConstantesAppServicio.CaracterComa.ToString() + 23.ToString();
                    else fuentes = fuentes + 23.ToString();
                }
            }

            if (string.IsNullOrEmpty(fuentes)) fuentes = ConstantesAppServicio.ParametroDefecto;

            return fuentes;
        }

        /// <summary>
        /// Permite obtener la lista de la consulta
        /// </summary>       
        public List<MeMedicion96DTO> ObtenerConsultaMedidores(DateTime fecInicio, DateTime fecFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central, int parametro, int nroPagina, int nroRegistros, out List<MeMedicion96DTO> sumatoria)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);

            if(parametro == 4 || parametro == 2)
            {
                lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracionPrueba"]);
            }

            List<MeMedicion96DTO> entitys = new List<MeMedicion96DTO>();
            sumatoria = new List<MeMedicion96DTO>();

            if (string.IsNullOrEmpty(empresas))
                return entitys;

            int tipoInformacion = 0;

            if (parametro == ConstantesMedicion.TipoPotenciaActiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaActiva;
                entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiTodos);
                sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiDefault);
                sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiDefault);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactivaCapacitiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiCapacitiva);
                sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiCapacitiva);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactivaInductiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiInductiva);
                sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiInductiva);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaActivaSSAA)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaActiva;
                string fuentes = this.GetFuenteSSAA(tiposGeneracion);

                entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, nroPagina, nroRegistros, ConstantesMedidores.TptoMedicodiTodos);
                sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
            }

            if (sumatoria != null && sumatoria.Count > 0 && sumatoria.Count == 3)
            {
                switch (parametro)
                {
                    case ConstantesMedicion.TipoPotenciaReactiva:
                        sumatoria[0].Gruponomb = "TOTAL ENERGÍA REACTIVA (MVarh)";
                        sumatoria[1].Gruponomb = "TOTAL POTENCIA REACTIVA MÁXIMA (MVAR)";
                        sumatoria[2].Gruponomb = "TOTAL POTENCIA REACTIVA MÍNIMA (MVAR)";
                        break;
                    case ConstantesMedicion.TipoPotenciaReactivaCapacitiva:
                        sumatoria[0].Gruponomb = "TOTAL ENERGÍA REACTIVA CAPACITIVA (MVarh)";
                        sumatoria[1].Gruponomb = "TOTAL POTENCIA REACTIVA CAPACITIVA MÁXIMA (MVAR)";
                        sumatoria[2].Gruponomb = "TOTAL POTENCIA REACTIVA CAPACITIVA MÍNIMA (MVAR)";
                        break;
                    case ConstantesMedicion.TipoPotenciaReactivaInductiva:
                        sumatoria[0].Gruponomb = "TOTAL ENERGÍA REACTIVA INDUCTIVA (MVarh)";
                        sumatoria[1].Gruponomb = "TOTAL POTENCIA REACTIVA INDUCTIVA MÁXIMA (MVAR)";
                        sumatoria[2].Gruponomb = "TOTAL POTENCIA REACTIVA INDUCTIVA MÍNIMA (MVAR)";
                        break;
                }
            }

            entitys = entitys.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();

            return entitys;
        }      

        public int ObtenerNroRegistroConsultaMedidores(DateTime fecInicio, DateTime fecFin)
        {
            int count;

            count = FactorySic.GetMeMedicion48Repository().ObtenerNroElementosConsultaMedidores(fecInicio, fecFin);

            return count;
        }

        public void GenerarArchivoExportacionGeneracionNoCoes(DateTime fecInicio, DateTime fecFin, string path, string file)
        {
            try
            {
                #region Acceso a datos

                int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);

                List<MeMedicion48DTO> listActiva = new List<MeMedicion48DTO>();

                listActiva = FactorySic.GetMeMedicion48Repository().ObtenerConsultaMedidores(fecInicio, fecFin);                

                listActiva = listActiva.OrderBy(x => x.Medifecha).ThenBy(x => x.Emprnomb).ToList();

                #endregion

                #region Generación de Archivo

                file = path + file;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }
                using (xlPackage = new ExcelPackage(newFile))
                {
                    if (listActiva != null)
                    {
                        this.CreaHojaHorizontalGeneracionNoCoes("Generación no Coes", listActiva, fecInicio.ToString("dd/MM/yyyy"), fecFin.ToString("dd/MM/yyyy"), "Generación No Coes");
                    }
                    xlPackage.Save();
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        protected void CreaHojaHorizontalGeneracionNoCoes(string hojaName, List<MeMedicion48DTO> list, string fechaInicio, string fechaFin, string titulo)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hojaName);

            if (ws != null)
            {
                ws.Cells[5, 2].Value = titulo;

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Desde:";
                ws.Cells[7, 3].Value = fechaInicio;
                ws.Cells[8, 2].Value = "Hasta:";
                ws.Cells[8, 3].Value = fechaFin;

                ws.Cells[10, 2].Value = "FECHA";
                ws.Cells[10, 3].Value = "EMPRESA";
                ws.Cells[10, 4].Value = "GRUPO";
                ws.Cells[10, 5].Value = "PUNTO MEDICIÓN";               

                string header = string.Empty;              

                DateTime now = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                for (int i = 1; i <= 48; i++)
                {
                    DateTime fecColumna = now.AddMinutes(30 * i);
                    ws.Cells[10, 5 + i].Value = fecColumna.ToString("HH:mm");
                }

                rg = ws.Cells[10, 2, 10, 53];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                int row = 11;

                foreach (MeMedicion48DTO item in list)
                {
                    ws.Cells[row, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");
                    ws.Cells[row, 3].Value = item.Emprnomb;
                    ws.Cells[row, 4].Value = item.Gruponomb;
                    ws.Cells[row, 5].Value = item.Ptomedicodi;

                    for (int k = 1; k <= 48; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            ws.Cells[row, 5 + k].Value = Convert.ToDecimal(resultado);
                        }
                    }
                    
                    rg = ws.Cells[row, 2, row, 53];

                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    row++;
                }

                ws.Column(2).Width = 20;

                ws.Column(2).Style.Numberformat.Format = "dd/MM/yyyy";

                for (int t = 5; t <= 53; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }                

                rg = ws.Cells[row + 4, 2, row + 8, 3];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;          
                rg.Style.Font.Italic = true;

                rg = ws.Cells[1, 3, row + 2, 103];
                rg.AutoFitColumns();

                ws.View.FreezePanes(11, 8);               

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }
        }

       public List<MeMedicion48DTO> ObtenerConsultaMedidoresGeneracionNoCoes(DateTime fecInicio, DateTime fecFin, int nroPagina, int nroRegistros)
        {                     
            List<MeMedicion48DTO> entitys = new List<MeMedicion48DTO>();
                  

            entitys = FactorySic.GetMeMedicion48Repository().ObtenerConsultaMedidores(fecInicio, fecFin,nroPagina, nroRegistros);                

            entitys = entitys.OrderBy(x => x.Medifecha).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();

            return entitys;
        }

        /// <summary>
        /// Permite obtener la consulta de medidores de distribucion
        /// </summary>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="empresas"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroRegistros"></param>
        /// <param name="sumatoria"></param>
        /// <returns></returns>
        public List<MeMedicion96DTO> ObtenerConsultaMedDistribucion(DateTime fecInicio, DateTime fecFin, string empresas, int nroPagina,
            int nroRegistros, out List<MeMedicion96DTO> sumatoria)
        {

            List<MeMedicion96DTO> entitys = FactorySic.GetMeMedicion96Repository().ObtenerConsultaMedDistribucion(empresas,
                fecInicio, fecFin, nroPagina, nroRegistros);

            sumatoria = FactorySic.GetMeMedicion96Repository().ObtenerTotalConsultaMedDistribucion(empresas, fecInicio, fecFin);
            return entitys;
        }

        /// <summary>
        /// Obtiene el número de registros de la consulta
        /// </summary>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public int ObtenerNroRegistroConsultaMedidores(DateTime fecInicio, DateTime fecFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central, int parametro)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            int count = 0;

            if (string.IsNullOrEmpty(empresas))
                return count;

            int tipoInformacion = 0;

            int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);
            if (parametro == 2 || parametro == 4)
            {
                lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracionPrueba"]);
            }

            if (parametro == ConstantesMedicion.TipoPotenciaActiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaActiva;
                count = FactorySic.GetMeMedicion96Repository().ObtenerNroElementosConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                count = FactorySic.GetMeMedicion96Repository().ObtenerNroElementosConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiDefault);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactivaCapacitiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                count = FactorySic.GetMeMedicion96Repository().ObtenerNroElementosConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiCapacitiva);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaReactivaInductiva)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaReactiva;
                count = FactorySic.GetMeMedicion96Repository().ObtenerNroElementosConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiInductiva);
            }
            if (parametro == ConstantesMedicion.TipoPotenciaActivaSSAA)
            {
                tipoInformacion = ConstantesMedicion.IdTipoInfoPotenciaActiva;
                string fuentes = this.GetFuenteSSAA(tiposGeneracion);

                count = FactorySic.GetMeMedicion96Repository().ObtenerNroElementosServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, tipoInformacion
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
            }

            return count;
        }

        /// <summary>
        /// Permite obtener la consulta de medidores de distribucion
        /// </summary>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="empresas"></param>
        /// <returns></returns>
        public int ObtenerNroRegistrosMedDistribucion(DateTime fecInicio, DateTime fecFin, string empresas)
        {
            return FactorySic.GetMeMedicion96Repository().ObtenerNroRegistrosMedDistribucion(empresas, fecInicio, fecFin);
        }



        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        public void GenerarArchivoExportacion(DateTime fecInicio, DateTime fecFin, string tiposEmpresa, string empresas,
            string tiposGeneracion, int central, string parametros, string path, string file, int formato, bool flag)
        {
            try
            {
                #region Acceso a datos

                if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
                {
                    List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                    empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
                }

                int lectcodi = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["IdLecturaMedidorGeneracion"]);

                string fuentes = this.GetFuenteSSAA(tiposGeneracion);

                List<MeMedicion96DTO> listActiva = new List<MeMedicion96DTO>();
                List<MeMedicion96DTO> listReactiva = new List<MeMedicion96DTO>();
                List<MeMedicion96DTO> listReactivaCapacitiva = new List<MeMedicion96DTO>();
                List<MeMedicion96DTO> listReactivaInductiva = new List<MeMedicion96DTO>();
                List<MeMedicion96DTO> listServiciosAuxiliares = new List<MeMedicion96DTO>();

                List<MeMedicion96DTO> listCSV = null;

                string[] lecturas = string.IsNullOrEmpty(parametros) ? new string[0] : parametros.Split(ConstantesAppServicio.CaracterComa);

                if (string.IsNullOrEmpty(parametros))
                {
                    listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);

                    listReactiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiDefault);

                    listReactivaCapacitiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiCapacitiva);

                    listReactivaInductiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiInductiva);

                    listServiciosAuxiliares = FactorySic.GetMeMedicion96Repository().ObtenerExportacionServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                    , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                    , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                }
                else
                {
                    foreach (string item in lecturas)
                    {
                        int id = int.Parse(item);

                        if (formato == 1 || formato == 2)
                        {
                            if (id == ConstantesMedicion.TipoPotenciaActiva)
                            {
                                listActiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactiva)
                            {
                                listReactiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiDefault);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactivaCapacitiva)
                            {
                                listReactivaCapacitiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiCapacitiva);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactivaInductiva)
                            {
                                listReactivaInductiva = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiInductiva);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaActivaSSAA)
                            {
                                listServiciosAuxiliares = FactorySic.GetMeMedicion96Repository().ObtenerExportacionServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                            }
                        }
                        if (formato == 3)
                        {
                            if (id == ConstantesMedicion.TipoPotenciaActiva)
                            {
                                listCSV = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactiva)
                            {
                                listCSV = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiDefault);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactivaCapacitiva)
                            {
                                listCSV = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiCapacitiva);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaReactivaInductiva)
                            {
                                listCSV = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedidores(fecInicio, fecFin, central, tiposGeneracion, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaReactiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiInductiva);
                            }
                            if (id == ConstantesMedicion.TipoPotenciaActivaSSAA)
                            {
                                listCSV = FactorySic.GetMeMedicion96Repository().ObtenerExportacionServiciosAuxiliares(fecInicio, fecFin, central, fuentes, empresas
                                , ConstantesMedicion.IdFamiliaSSAA, ConstantesMedicion.GrupoNoIntegrante, lectcodi, ConstantesMedicion.IdTipoInfoPotenciaActiva
                                , ConstantesMedicion.TipoGenerrer, ConstantesMedidores.TptoMedicodiTodos);
                            }
                        }
                    }
                }

                listActiva = listActiva.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
                listReactiva = listReactiva.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
                listReactivaCapacitiva = listReactivaCapacitiva.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
                listReactivaInductiva = listReactivaInductiva.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();
                listServiciosAuxiliares = listServiciosAuxiliares.OrderBy(x => x.Medifecha.Value).ThenBy(x => x.Emprnomb).ThenBy(x => x.Central).ThenBy(x => x.Equinomb).ToList();

                #endregion

                #region Generación de Archivo

                file = path + file;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }


                if (formato == 1)
                {
                    using (xlPackage = new ExcelPackage(newFile))
                    {
                        if (listActiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaActiva.ToString()))
                        {
                            this.CreaHojaHorizontal("ACTIVA (MW)", listActiva, fecInicio.ToString("dd/MM/yyyy"),
                                fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA ACTIVA (MW)", 1, flag);
                        }
                        if (listReactiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactiva.ToString()))
                        {
                            this.CreaHojaHorizontal("REACTIVA (MVAR)", listReactiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA (MVAR)", 5, flag);
                        }
                        if (listReactivaCapacitiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactivaCapacitiva.ToString()))
                        {
                            this.CreaHojaHorizontal("REACTIVA CAPACITIVA (MVAR)", listReactivaCapacitiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA CAPACITIVA (MVAR)", 2, flag);
                        }
                        if (listReactivaInductiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactivaInductiva.ToString()))
                        {
                            this.CreaHojaHorizontal("REACTIVA INDUCTIVA (MVAR)", listReactivaInductiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA INDUCTIVA (MVAR)", 4, flag);
                        }
                        if (listServiciosAuxiliares != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaActivaSSAA.ToString()))
                        {
                            this.CreaHojaHorizontal("SERV. AUX. (MW)", listServiciosAuxiliares, fecInicio.ToString("dd/MM/yyyy"),
                                fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE LOS SERVICIOS AUXILIARES (MW)", 3, flag);
                        }
                        xlPackage.Save();
                    }

                }

                if (formato == 2)
                {
                    using (xlPackage = new ExcelPackage(newFile))
                    {
                        if (listActiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaActiva.ToString()))
                        {
                            this.CreaHojaVertical("ACTIVA (MW)", listActiva, fecInicio.ToString("dd/MM/yyyy"),
                                fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA ACTIVA (MW)", 1, flag);
                        }
                        if (listReactiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactiva.ToString()))
                        {
                            this.CreaHojaVertical("REACTIVA (MVAR)", listReactiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA (MVAR)", 5, flag);
                        }
                        if (listReactivaCapacitiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactivaCapacitiva.ToString()))
                        {
                            this.CreaHojaVertical("REACTIVA CAPACITIVA (MVAR)", listReactivaCapacitiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA CAPACITIVA (MVAR)", 2, flag);
                        }
                        if (listReactivaInductiva != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaReactivaInductiva.ToString()))
                        {
                            this.CreaHojaVertical("REACTIVA INDUCTIVA (MVAR)", listReactivaInductiva, fecInicio.ToString("dd/MM/yyyy"),
                                    fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE POTENCIA REACTIVA INDUCTIVA (MVAR)", 4, flag);
                        }
                        if (listServiciosAuxiliares != null && lecturas.Contains(ConstantesMedicion.TipoPotenciaActivaSSAA.ToString()))
                        {
                            this.CreaHojaVertical("SERV. AUX. (MW)", listServiciosAuxiliares, fecInicio.ToString("dd/MM/yyyy"),
                                fecFin.ToString("dd/MM/yyyy"), "REGISTROS DE MEDIDORES EN BORNES DE GENERACIÓN CADA 15 MINUTOS DE LOS SERVICIOS AUXILIARES (MW)", 3, flag);
                        }
                        xlPackage.Save();
                    }
                }

                if (formato == 3)
                {
                    if (listCSV != null)
                    {
                        this.CreaFormatoCSV(listCSV, fecInicio, fecFin, file);
                    }
                }


                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        /// <summary>
        /// Permite generar el reporte de medidores de distribución
        /// </summary>
        /// <param name="fecInicio"></param>
        /// <param name="fecFin"></param>
        /// <param name="empresas"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <param name="formato"></param>
        public void GenerarArchivoExportacionDistribucion(DateTime fecInicio, DateTime fecFin, string empresas,
            string path, string file, int formato, int frecuencia)
        {
            try
            {
                List<MeMedicion96DTO> list = FactorySic.GetMeMedicion96Repository().ObtenerExportacionConsultaMedDistribucion(empresas, fecInicio, fecFin);
                file = path + file;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                if (formato == 1)
                {
                    using (xlPackage = new ExcelPackage(newFile))
                    {
                        if (list != null)
                        {
                            this.CreaHojaHorizontalMedDistribucion(list, fecInicio.ToString("dd/MM/yyyy"), fecFin.ToString("dd/MM/yyyy"), frecuencia);
                        }

                        xlPackage.Save();
                    }

                }

                if (formato == 2)
                {
                    using (xlPackage = new ExcelPackage(newFile))
                    {
                        if (list != null)
                        {
                            this.CreaHojaVerticalMedDistribucion(list, fecInicio.ToString("dd/MM/yyyy"), fecFin.ToString("dd/MM/yyyy"), frecuencia);
                        }
                        xlPackage.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



        #region Creación de hojas excel

        #region Modificacion PR15 - 30/11/2017
        /// <summary>
        /// Crea la hoja con los datos a exportar
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        protected void CreaHojaHorizontal(string hojaName, List<MeMedicion96DTO> list, string fechaInicio, string fechaFin, string titulo, int tipo, bool flag)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hojaName);

            if (ws != null)
            {
                ws.Cells[5, 2].Value = titulo;

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Desde:";
                ws.Cells[7, 3].Value = fechaInicio;
                ws.Cells[8, 2].Value = "Hasta:";
                ws.Cells[8, 3].Value = fechaFin;

                ws.Cells[10, 2].Value = "FECHA";
                ws.Cells[10, 3].Value = "PUNTO MEDICIÓN";
                ws.Cells[10, 4].Value = "EMPRESA";
                ws.Cells[10, 5].Value = "CENTRAL";
                ws.Cells[10, 6].Value = "UNIDAD";

                string header = string.Empty;
                if (tipo == 1) header = "TOTAL ENERGIA ACTIVA  (MWh)";
                if (tipo == 5) header = "TOTAL ENERGÍA REACTIVA (MVarh)";
                if (tipo == 2) header = "TOTAL ENERGÍA REACTIVA CAPACITIVA (MVarh)";
                if (tipo == 4) header = "TOTAL ENERGÍA REACTIVA INDUCTIVA (MVarh)";
                if (tipo == 3) header = "TOTAL SERVICIOS AUXILIARES (MWh)";

                ws.Cells[10, 7].Value = header;

                DateTime now = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                for (int i = 1; i <= 96; i++)
                {
                    DateTime fecColumna = now.AddMinutes(15 * i);
                    ws.Cells[10, 7 + i].Value = fecColumna.ToString("HH:mm");
                }

                rg = ws.Cells[10, 2, 10, 103];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                int row = 11;

                foreach (MeMedicion96DTO item in list)
                {
                    ws.Cells[row, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");
                    ws.Cells[row, 3].Value = item.Ptomedicodi;
                    ws.Cells[row, 4].Value = item.Emprnomb;
                    ws.Cells[row, 5].Value = item.Central;
                    ws.Cells[row, 6].Value = item.Equinomb;

                    ws.Cells[row, 7].Formula = "=SUM(" + this.ObtenerColumnNumber(row, 8) + ":" + this.ObtenerColumnNumber(row, 103) + ")/4";

                    for (int k = 1; k <= 96; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            ws.Cells[row, 7 + k].Value = Convert.ToDecimal(resultado);
                        }
                    }

                    rg = ws.Cells[row, 2, row, 103];

                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    row++;
                }

                ws.Column(2).Width = 20;

                ws.Column(2).Style.Numberformat.Format = "dd/MM/yyyy";

                for (int t = 7; t <= 103; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }

                string footer1 = string.Empty;
                string footer2 = string.Empty;
                string footer3 = string.Empty;

                if (tipo == 1)
                {
                    footer1 = "TOTAL ENERGÍA (MWh)";
                    footer2 = "TOTAL POTENCIA MÁXIMA (MW)";
                    footer3 = "TOTAL POTENCIA MÍNIMA (MW)";
                }
                if (tipo == 5)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA (MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA MÍNIMA (MVAR)";
                }
                if (tipo == 2)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA CAPACITIVA(MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA CAPACITIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA CAPACITIVA MÍNIMA (MVAR)";
                }
                if (tipo == 4)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA (MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA INDUCTIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA INDUCTIVA MÍNIMA (MVAR)";
                }
                if (tipo == 3)
                {
                    footer1 = "TOTAL ENERGÍA SERV. AUX. (MWh)";
                    footer2 = "TOTAL POTENCIA MÁXIMA SERV. AUX. (MW)";
                    footer3 = "TOTAL POTENCIA MÍNIMA SERV. AUX. (MW)";
                }

                ws.Cells[row, 2].Value = footer1;
                rg = ws.Cells[row, 2, row, 6];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[row + 1, 2].Value = footer2;
                rg = ws.Cells[row + 1, 2, row + 1, 6];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[row + 2, 2].Value = footer3;
                rg = ws.Cells[row + 2, 2, row + 2, 6];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rg = ws.Cells[row, 2, row + 2, 103];
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3493D1"));

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                ws.Cells[row, 7].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 7) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 7) + ")";

                for (int k = 1; k <= 96; k++)
                {
                    ws.Cells[row, 7 + k].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 7 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 7 + k) + ")/4";
                    ws.Cells[row + 1, 7 + k].Formula = "=MAX(" + this.ObtenerColumnNumber(11, 7 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 7 + k) + ")";
                    ws.Cells[row + 2, 7 + k].Formula = "=MIN(" + this.ObtenerColumnNumber(11, 7 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 7 + k) + ")";
                }

                ws.Cells[row + 1, 7].Formula = "=MAX(" + this.ObtenerColumnNumber(row + 1, 8) + ":" + this.ObtenerColumnNumber(row + 1, 103) + ")";
                ws.Cells[row + 2, 7].Formula = "=MIN(" + this.ObtenerColumnNumber(row + 2, 8) + ":" + this.ObtenerColumnNumber(row + 2, 103) + ")";

                ws.Cells[1, 1].Value = "0";
                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.White);

                ws.Cells[row + 4, 2].Value = "Leyenda";

                if (flag)
                {
                    ws.Cells[row + 6, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[row + 6, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    ws.Cells[row + 6, 3].Value = "Registros negativos";
                    ws.Cells[row + 8, 2].Value = "(*) Incluye a las centrales eléctricas RER No Adjudicadas (C.T. Maple Etanol y C.H. Pías I).";
                }
                else
                {
                    ws.Cells[row + 6, 2].Value = "(*) Incluye a las centrales eléctricas RER No Adjudicadas (C.T. Maple Etanol y C.H. Pías I).";

                }

                rg = ws.Cells[row + 4, 2, row + 8, 3];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Italic = true;

                rg = ws.Cells[1, 3, row + 2, 103];
                rg.AutoFitColumns();

                ws.View.FreezePanes(11, 8);


                if (flag)
                {

                    var cellAddress = new ExcelAddress(1, 1, list.Count + 20, 103);
                    var cf = ws.ConditionalFormatting.AddLessThan(cellAddress);
                    cf.Formula = "0";
                    cf.Style.Fill.BackgroundColor.Color = Color.Red;
                }

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }
        }

        /// <summary>
        /// Permite crear la hoja con los datos verticales
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="titulo"></param>
        protected void CreaHojaVertical(string hojaName, List<MeMedicion96DTO> list, string fechaInicio, string fechaFin, string titulo, int tipo, bool flag)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(hojaName);

            if (ws != null)
            {
                ws.Cells[5, 2].Value = titulo;

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Desde:";
                ws.Cells[7, 3].Value = fechaInicio;
                ws.Cells[8, 2].Value = "Hasta:";
                ws.Cells[8, 3].Value = fechaFin;

                int row = 10;
                int column = 4;

                ws.Cells[row, column - 2].Value = "FECHA/HORA";
                ws.Cells[row, column - 2, row + 3, column - 2].Merge = true;
                ws.Cells[row, column - 1].Value = "TOTAL";
                ws.Cells[row, column - 1, row + 2, column - 1].Merge = true;

                string header = string.Empty;
                if (tipo == 1) header = "MW";
                if (tipo == 2) header = "MVAR";
                if (tipo == 3) header = "MW";

                ws.Cells[row + 3, column - 1].Value = header;

                var listEmpresas = list.Select(m => new { m.Emprcodi, m.Emprnomb }).Distinct().OrderBy(x => x.Emprnomb).ToList();

                DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int dias = (int)(fecFin.Subtract(fecha).TotalDays);

                foreach (var empresa in listEmpresas)
                {
                    var listCentrales = list.Where(x => x.Emprcodi == empresa.Emprcodi).Select(m => new { m.Equipadre, m.Central, m.Emprcodi }).Distinct().ToList();

                    int countCentral = 0;
                    foreach (var central in listCentrales)
                    {
                        var listEquipos = (central.Equipadre != 0) ? list.Where(x => x.Equipadre == central.Equipadre && x.Emprcodi == central.Emprcodi).
                            Select(m => new { m.Equicodi, m.Equinomb, m.Ptomedicodi }).Distinct().ToList() :
                        list.Where(x => x.Central == central.Central && x.Emprcodi == central.Emprcodi).
                            Select(m => new { m.Equicodi, m.Equinomb, m.Ptomedicodi }).Distinct().ToList();

                        int count = 0;
                        foreach (var equipo in listEquipos)
                        {
                            ws.Cells[row, column].Value = equipo.Ptomedicodi;
                            ws.Cells[row + 3, column].Value = equipo.Equinomb.Trim();
                            column++;
                            count++;
                            countCentral++;

                            List<MeMedicion96DTO> listDatos = list.Where(x => x.Emprcodi == empresa.Emprcodi && x.Ptomedicodi == equipo.Ptomedicodi).OrderBy(x => x.Medifecha).ToList();
                            int nroDias = (int)fecFin.Subtract(fecha).TotalDays;
                            int fila = 14;

                            for (int i = 0; i < nroDias + 1; i++)
                            {
                                DateTime fechaConsulta = fecha.AddDays(i);

                                List<MeMedicion96DTO> subList = listDatos.Where(x => (DateTime)x.Medifecha == fechaConsulta).ToList();

                                if (subList.Count > 0)
                                {
                                    foreach (MeMedicion96DTO dato in subList)
                                    {
                                        for (int j = 1; j <= 96; j++)
                                        {
                                            object resultado = dato.GetType().GetProperty("H" + j).GetValue(dato, null);
                                            if (resultado != null)
                                            {
                                                ws.Cells[fila, column - 1].Value = Convert.ToDecimal(resultado);
                                            }

                                            fila++;
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 1; j <= 96; j++)
                                    {
                                        fila++;
                                    }
                                }
                            }
                        }

                        ws.Cells[row + 2, column - count].Value = central.Central.Trim();
                        ws.Cells[row + 2, column - count, row + 2, column - 1].Merge = true;
                    }

                    ws.Cells[row + 1, column - countCentral].Value = empresa.Emprnomb.Trim();
                    ws.Cells[row + 1, column - countCentral, row + 1, column - 1].Merge = true;
                }

                int k = row + 4;
                int t = 2;
                for (int i = 0; i < dias + 1; i++)
                {
                    for (int j = 1; j <= 96; j++)
                    {
                        ws.Cells[k, t].Value = fecha.AddDays(i).AddMinutes(j * 15).ToString("dd/MM/yyyy HH:mm");
                        ws.Cells[k, t + 1].Formula = "=SUM(" + this.ObtenerColumnNumber(k, t + 2) + ":" + this.ObtenerColumnNumber(k, column - 1) + ")";
                        k++;
                    }
                }

                rg = ws.Cells[row, 2, row + 3, column - 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Gray);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Gray);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Gray);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Gray);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[row + 4, 2, k - 1, column - 1];

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;


                string footer1 = string.Empty;
                string footer2 = string.Empty;
                string footer3 = string.Empty;

                if (tipo == 1)
                {
                    footer1 = "TOTAL ENERGÍA (MWh)";
                    footer2 = "TOTAL POTENCIA MÁXIMA (MW)";
                    footer3 = "TOTAL POTENCIA MÍNIMA (MW)";
                }
                if (tipo == 5)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA (MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA MÍNIMA (MVAR)";
                }
                if (tipo == 2)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA CAPACITIVA(MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA CAPACITIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA CAPACITIVA MÍNIMA (MVAR)";
                }
                if (tipo == 4)
                {
                    footer1 = "TOTAL ENERGÍA REACTIVA (MVarh)";
                    footer2 = "TOTAL POTENCIA REACTIVA INDUCTIVA MÁXIMA (MVAR)";
                    footer3 = "TOTAL POTENCIA REACTIVA INDUCTIVA MÍNIMA (MVAR)";
                }
                if (tipo == 3)
                {
                    footer1 = "TOTAL ENERGÍA SERV. AUX. (MWh)";
                    footer2 = "TOTAL POTENCIA MÁXIMA SERV. AUX. (MW)";
                    footer3 = "TOTAL POTENCIA MÍNIMA SERV. AUX. (MW)";
                }

                ws.Cells[k, 2].Value = footer1;
                ws.Cells[k + 1, 2].Value = footer2;
                ws.Cells[k + 2, 2].Value = footer3;

                for (int l = 3; l < column; l++)
                {
                    ws.Cells[k, l].Formula = "=SUM(" + this.ObtenerColumnNumber(row + 4, l) + ":" + this.ObtenerColumnNumber(k - 1, l) + ")/4";
                    ws.Cells[k + 1, l].Formula = "=MAX(" + this.ObtenerColumnNumber(row + 4, l) + ":" + this.ObtenerColumnNumber(k - 1, l) + ")";
                    ws.Cells[k + 2, l].Formula = "=MIN(" + this.ObtenerColumnNumber(row + 4, l) + ":" + this.ObtenerColumnNumber(k - 1, l) + ")";
                }

                rg = ws.Cells[14, 3, k + 2, column];
                rg.Style.Numberformat.Format = "#,##0.000";

                rg = ws.Cells[k, 2, k + 2, column - 1];
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3493D1"));

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Bold = true;

                ws.Column(2).Width = 25;
                rg = ws.Cells[1, 3, k + 2, column];
                rg.AutoFitColumns();

                ws.Cells[1, 1].Value = "0";
                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.White);

                ws.Cells[k + 4, 2].Value = "Leyenda";

                if (flag)
                {
                    ws.Cells[k + 6, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[k + 6, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    ws.Cells[k + 6, 3].Value = "Registros negativos";
                    ws.Cells[k + 8, 2].Value = "(*) Incluye a las centrales eléctricas RER No Adjudicadas (C.T. Maple Etanol y C.H. Pías I).";
                }
                else
                {
                    ws.Cells[k + 6, 2].Value = "(*) Incluye a las centrales eléctricas RER No Adjudicadas (C.T. Maple Etanol y C.H. Pías I).";
                }

                rg = ws.Cells[k + 4, 2, k + 8, 3];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Italic = true;

                ws.View.FreezePanes(14, 4);

                if (flag)
                {
                    var cellAddress = new ExcelAddress(1, 1, k + 2, column);
                    var cf = ws.ConditionalFormatting.AddLessThan(cellAddress);
                    cf.Formula = "0";
                    cf.Style.Fill.BackgroundColor.Color = Color.Red;
                }

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }
        #endregion

        /// <summary>
        /// Permite crear el formato csv
        /// </summary>
        /// <param name="list"></param>
        protected void CreaFormatoCSV(List<MeMedicion96DTO> list, DateTime fechaInicio, DateTime fechaFin, string fileName)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            {
                var listCentrales = list.Select(m => new { m.Ptomedicodi, m.Ptomedielenomb, m.Emprnomb, m.Emprcodi}).Distinct().OrderBy(x => x.Emprnomb).ToList();

                int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays + 1;
                List<List<MeMedicion96DTO>> totales = new List<List<MeMedicion96DTO>>();

                string header = "fechahora ";

                foreach (var item in listCentrales)
                {
                    List<MeMedicion96DTO> listDatos = list.Where(x => x.Ptomedicodi == item.Ptomedicodi && x.Emprcodi == item.Emprcodi).ToList().OrderBy(p => p.Medifecha).ToList();

                    if (listDatos.Count > 0)
                    {
                        DateTime fecha = (DateTime)listDatos[0].Medifecha;

                        int diferencia = (int)fecha.Subtract(fechaInicio).TotalDays;

                        for (int i = 0; i < diferencia; i++)
                        {
                            listDatos.Insert(0, new MeMedicion96DTO());
                        }
                    }

                    totales.Add(listDatos);
                    header = header + ", " + item.Emprnomb.Trim() + " -" + ((item.Ptomedielenomb != null) ? item.Ptomedielenomb.Trim() : "");
                }

                file.WriteLine(header);

                for (int i = 0; i < dias; i++)
                {
                    for (int j = 1; j <= 96; j++)
                    {
                        string linea = fechaInicio.AddDays(i).AddMinutes(j * 15).ToString("dd/MM/yyyy HH:mm");

                        for (int k = 0; k < listCentrales.Count; k++)
                        {
                            if (totales[k].Count > i)
                            {
                                MeMedicion96DTO entidad = totales[k][i];

                                object val = entidad.GetType().GetProperty("H" + j).GetValue(entidad, null);

                                if (val != null)
                                {
                                    linea = linea + ", " + val.ToString();
                                }
                                else
                                {
                                    linea = linea + "," + " ";
                                }
                            }
                            else
                            {
                                linea = linea + ", " + " ";
                            }
                        }

                        file.WriteLine(linea);
                    }
                }
            }
        }


        /// <summary>
        /// Crea la hoja con los datos a exportar
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        protected void CreaHojaHorizontalMedDistribucion(List<MeMedicion96DTO> list, string fechaInicio, string fechaFin, int frecuencia)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DATOS");

            if (ws != null)
            {
                ws.Cells[5, 2].Value = "REGISTRO MEDIDORES DE DISTRIBUCIÓN";

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Desde:";
                ws.Cells[7, 3].Value = fechaInicio;
                ws.Cells[8, 2].Value = "Hasta:";
                ws.Cells[8, 3].Value = fechaFin;

                ws.Cells[10, 2].Value = "FECHA";
                ws.Cells[10, 3].Value = "PUNTO MEDICIÓN";
                ws.Cells[10, 4].Value = "EMPRESA";
                ws.Cells[10, 5].Value = "ÁREA OPERATIVA";
                ws.Cells[10, 6].Value = "TENSIÓN BARRA (KV)";
                ws.Cells[10, 7].Value = "SUBESTACIÓN";
                ws.Cells[10, 8].Value = "EQUIPO";
                ws.Cells[10, 9].Value = "TOTAL";


                DateTime now = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int maxColumnas = 0;
                if (frecuencia == 1)
                {
                    for (int i = 1; i <= 96; i++)
                    {
                        DateTime fecColumna = now.AddMinutes(15 * i);
                        ws.Cells[10, 9 + i].Value = fecColumna.ToString("HH:mm");
                    }
                    maxColumnas = 105;
                }
                else if (frecuencia == 0)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        DateTime fecColumna = now.AddMinutes(30 * i);
                        ws.Cells[10, 9 + i].Value = fecColumna.ToString("HH:mm");
                    }
                    maxColumnas = 57;
                }

                rg = ws.Cells[10, 2, 10, maxColumnas];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                int row = 11;

                foreach (MeMedicion96DTO item in list)
                {
                    ws.Cells[row, 2].Value = ((DateTime)item.Medifecha).ToString("dd/MM/yyyy");
                    ws.Cells[row, 3].Value = item.Ptomedicodi;
                    ws.Cells[row, 4].Value = item.Emprnomb;
                    ws.Cells[row, 5].Value = (item.Areaoperativa != null) ? item.Areaoperativa : string.Empty;
                    ws.Cells[row, 6].Value = (item.Equitension != null) ? (item.Equitension).ToString() : string.Empty;
                    ws.Cells[row, 7].Value = item.Areanomb;
                    ws.Cells[row, 8].Value = item.Equinomb;
                    ws.Cells[row, 9].Formula = "=SUM(" + this.ObtenerColumnNumber(row, 10) + ":" + this.ObtenerColumnNumber(row, 105) + ")";

                    for (int k = 1; k <= 96; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            if (frecuencia == 1)
                            {
                                ws.Cells[row, 9 + k].Value = Convert.ToDecimal(resultado);
                            }
                            else if (frecuencia == 0 && k % 2 == 0)
                            {
                                ws.Cells[row, 9 + k / 2].Value = Convert.ToDecimal(resultado);
                            }
                        }
                    }

                    rg = ws.Cells[row, 2, row, maxColumnas];

                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(Color.Black);
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(Color.Black);
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(Color.Black);
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                    rg.Style.Font.Size = 10;

                    row++;
                }

                ws.Column(2).Width = 20;
                ws.Column(2).Style.Numberformat.Format = "dd/MM/yyyy";

                for (int t = 9; t <= maxColumnas; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }


                ws.Cells[row, 2].Value = "TOTAL";
                rg = ws.Cells[row, 2, row, 8];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                rg = ws.Cells[row, 2, row, maxColumnas];
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3493D1"));

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                ws.Cells[row, 9].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 9) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 9) + ")";

                if (frecuencia == 1)
                {
                    for (int k = 1; k <= 96; k++)
                    {
                        ws.Cells[row, 9 + k].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 9 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 9 + k) + ")";
                    }
                }
                else if (frecuencia == 0)
                {
                    for (int k = 1; k <= 48; k++)
                    {
                        ws.Cells[row, 9 + k].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 9 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 9 + k) + ")";
                    }
                }


                rg = ws.Cells[row + 4, 2, row + 8, 3];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Italic = true;
                rg = ws.Cells[1, 3, row + 2, maxColumnas];
                rg.AutoFitColumns();
                ws.View.FreezePanes(11, 10);

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);

            }
        }

        /// <summary>
        /// Permite crear la hoja con los datos verticales
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="titulo"></param>
        protected void CreaHojaVerticalMedDistribucion(List<MeMedicion96DTO> list, string fechaInicio, string fechaFin, int frecuencia)
        {
            ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DATOS");

            if (ws != null)
            {
                ws.Cells[5, 2].Value = "REGISTRO DE MEDIDORES DE DISTRIBUCIÓN";

                ExcelRange rg = ws.Cells[5, 2, 5, 2];
                rg.Style.Font.Size = 13;
                rg.Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Desde:";
                ws.Cells[7, 3].Value = fechaInicio;
                ws.Cells[8, 2].Value = "Hasta:";
                ws.Cells[8, 3].Value = fechaFin;

                int row = 10;
                int column = 4;

                ws.Cells[row, column - 2].Value = "FECHA/HORA";
                ws.Cells[row, column - 2, row + 5, column - 2].Merge = true;
                ws.Cells[row, column - 1].Value = "TOTAL";
                ws.Cells[row, column - 1, row + 5, column - 1].Merge = true;


                DateTime fecha = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                int dias = (int)(fecFin.Subtract(fecha).TotalDays);


                var listPuntos = list.Select(m => new
                {
                    m.Ptomedicodi,
                    m.Equicodi,
                    m.Equinomb,
                    m.Equitension,
                    m.Areanomb,
                    m.Areaoperativa,
                    m.Emprcodi,
                    m.Emprnomb
                }).Distinct().ToList();

                foreach (var equipo in listPuntos)
                {
                    ws.Cells[row, column].Value = equipo.Ptomedicodi;
                    ws.Cells[row + 1, column].Value = equipo.Emprnomb;
                    ws.Cells[row + 2, column].Value = equipo.Areaoperativa;
                    ws.Cells[row + 3, column].Value = (equipo.Equitension != null) ? equipo.Equitension : null;
                    ws.Cells[row + 4, column].Value = equipo.Areanomb;
                    ws.Cells[row + 5, column].Value = equipo.Equinomb;
                    column++;

                    List<MeMedicion96DTO> listDatos = list.Where(x => x.Ptomedicodi == equipo.Ptomedicodi).OrderBy(x => x.Medifecha).ToList();
                    int nroDias = (int)fecFin.Subtract(fecha).TotalDays;
                    int fila = 16;

                    for (int i = 0; i < nroDias + 1; i++)
                    {
                        DateTime fechaConsulta = fecha.AddDays(i);

                        List<MeMedicion96DTO> subList = listDatos.Where(x => (DateTime)x.Medifecha == fechaConsulta).ToList();

                        if (subList.Count > 0)
                        {
                            foreach (MeMedicion96DTO dato in subList)
                            {
                                for (int j = 1; j <= 96; j++)
                                {
                                    object resultado = dato.GetType().GetProperty("H" + j).GetValue(dato, null);

                                    if (frecuencia == 1)
                                    {
                                        if (resultado != null)
                                        {
                                            ws.Cells[fila, column - 1].Value = Convert.ToDecimal(resultado);
                                        }

                                        fila++;
                                    }
                                    else if (frecuencia == 0 && j % 2 == 0)
                                    {
                                        if (resultado != null)
                                        {
                                            ws.Cells[fila, column - 1].Value = Convert.ToDecimal(resultado);
                                        }

                                        fila++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (frecuencia == 1)
                            {
                                for (int j = 1; j <= 96; j++)
                                {
                                    fila++;
                                }
                            }
                            else if (frecuencia == 0)
                            {
                                for (int j = 1; j <= 48; j++)
                                {
                                    fila++;
                                }
                            }
                        }
                    }
                }


                int k = row + 6;
                int t = 2;
                for (int i = 0; i < dias + 1; i++)
                {
                    if (frecuencia == 1)
                    {
                        for (int j = 1; j <= 96; j++)
                        {
                            ws.Cells[k, t].Value = fecha.AddDays(i).AddMinutes(j * 15).ToString("dd/MM/yyyy HH:mm");
                            ws.Cells[k, t + 1].Formula = "=SUM(" + this.ObtenerColumnNumber(k, t + 2) + ":" + this.ObtenerColumnNumber(k, column - 1) + ")";
                            k++;
                        }
                    }
                    else
                    {
                        for (int j = 1; j <= 48; j++)
                        {
                            ws.Cells[k, t].Value = fecha.AddDays(i).AddMinutes(j * 30).ToString("dd/MM/yyyy HH:mm");
                            ws.Cells[k, t + 1].Formula = "=SUM(" + this.ObtenerColumnNumber(k, t + 2) + ":" + this.ObtenerColumnNumber(k, column - 1) + ")";
                            k++;
                        }
                    }
                }

                rg = ws.Cells[row, 2, row + 5, column - 1];
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Gray);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Gray);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Gray);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Gray);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;

                rg = ws.Cells[row + 6, 2, k - 1, column - 1];

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;

                ws.Cells[k, 2].Value = "TOTAL";

                for (int l = 3; l < column; l++)
                {
                    ws.Cells[k, l].Formula = "=SUM(" + this.ObtenerColumnNumber(row + 6, l) + ":" + this.ObtenerColumnNumber(k - 1, l) + ")";
                }

                rg = ws.Cells[14, 3, k + 2, column];
                rg.Style.Numberformat.Format = "#,##0.000";

                rg = ws.Cells[k, 2, k, column - 1];
                rg.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rg.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#3493D1"));

                rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Left.Color.SetColor(Color.Black);
                rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Right.Color.SetColor(Color.Black);
                rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Top.Color.SetColor(Color.Black);
                rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rg.Style.Border.Bottom.Color.SetColor(Color.Black);
                rg.Style.Font.Size = 10;
                rg.Style.Font.Color.SetColor(Color.White);
                rg.Style.Font.Bold = true;

                ws.Column(2).Width = 25;
                rg = ws.Cells[1, 3, k, column];
                rg.AutoFitColumns();

                ws.Cells[1, 1].Value = "0";
                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.White);
                rg = ws.Cells[k + 4, 2, k + 8, 3];
                rg.Style.Font.Size = 10;
                rg.Style.Font.Bold = true;
                rg.Style.Font.Color.SetColor(Color.Black);
                rg.Style.Font.Italic = true;

                ws.View.FreezePanes(16, 4);

                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("ff", img);
                picture.From.Column = 1;
                picture.From.Row = 1;
                picture.To.Column = 2;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
            }
        }


        /// <summary>
        /// Retorna el nombre de columna
        /// </summary>
        /// <param name="nroColumn"></param>
        /// <returns></returns>
        protected string ObtenerColumnNumber(int nroFila, int nroColumn)
        {
            int div = nroColumn;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter + nroFila;
        }


        /// <summary>
        /// Permite obtener el reporte de cumplimiento
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="formatCodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ObtenerReporteCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<SiEmpresaDTO> listEmpresas = (new GeneralAppServicio()).ListarEmpresasPorTipo(2.ToString());
            List<int> idsEmpresas = empresas.Split(',').Select(int.Parse).ToList();
            List<SiEmpresaDTO> listEmpresasFiltro = listEmpresas.Where(x => idsEmpresas.Any(y => x.Emprcodi == y)).ToList();
            List<MeEnvioDTO> entitys = FactorySic.GetMeEnvioRepository().ObtenerReporteEnvioCumplimiento(empresas, formatCodi, fechaInicio, fechaFin);

            List<MeEnvioDTO> resultado = new List<MeEnvioDTO>();
            List<SiEmpresaDTO> listCargaron = listEmpresasFiltro.Where(x => entitys.Any(y => x.Emprcodi == y.Emprcodi)).Distinct().ToList();

            foreach (SiEmpresaDTO empresa in listCargaron)
            {
                MeEnvioDTO envio = entitys.Where(x => x.Emprcodi == empresa.Emprcodi).OrderByDescending(x => x.Enviofecha).FirstOrDefault();

                if (envio != null)
                {
                    envio.Indcumplimiento = 1;
                    resultado.Add(envio);
                }
            }

            List<SiEmpresaDTO> listNoCargaron = listEmpresasFiltro.Where(x => !entitys.Any(y => x.Emprcodi == y.Emprcodi)).Distinct().ToList();

            foreach (SiEmpresaDTO empresa in listNoCargaron)
            {
                MeEnvioDTO envio = new MeEnvioDTO
                {
                    Emprnomb = empresa.Emprnomb,
                    Indcumplimiento = 0
                };

                resultado.Add(envio);
            }

            return resultado;
        }

        /// <summary>
        /// Permite listar el reporte de envios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="formatCodi"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeEnvioDTO> ObtenerEnviosCumplimiento(int idEmpresa, int formatCodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeEnvioDTO> list = FactorySic.GetMeEnvioRepository().ObtenerReporteEnvioCumplimiento(idEmpresa.ToString(), formatCodi, fechaInicio, fechaFin);

            foreach (MeEnvioDTO item in list)
            {
                item.Indcumplimiento = 1;
            }

            return list;
        }

        /// <summary>
        /// Permite obtener las empresas a validar
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public List<MdValidacionDTO> ObtenerValidacionMes(DateTime fecha)
        {
            return FactorySic.GetMdValidacionRepository().GetByCriteria(fecha);
        }

        #endregion


        #region GESPROTEC-20241031

        public List<SiEmpresaDTO> ListObtenerEmpresaSEINProtecciones()
        {
            return FactorySic.GetSiEmpresaRepository().ListObtenerEmpresaSEINProtecciones();
        }
        #endregion
    }


}
