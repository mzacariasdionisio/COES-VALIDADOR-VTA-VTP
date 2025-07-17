using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.ReportesMedicion;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace COES.Servicios.Aplicacion.Mediciones
{
    public class EjecutadoAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia del libro excel a generar
        /// </summary>
        ExcelPackage xlPackage = null;

        HorasOperacionAppServicio servHO = new HorasOperacionAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConsultaMedidoresAppServicio));

        #region Métodos Tabla ME_DESPACHO_PRODGEN

        /// <summary>
        /// Inserta un registro de la tabla ME_DESPACHO_PRODGEN
        /// </summary>
        public void SaveMeDespachoProdgen(MeDespachoProdgenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetMeDespachoProdgenRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_DESPACHO_PRODGEN
        /// </summary>
        public void UpdateMeDespachoProdgen(MeDespachoProdgenDTO entity)
        {
            try
            {
                FactorySic.GetMeDespachoProdgenRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_DESPACHO_PRODGEN
        /// </summary>
        public void DeleteMeDespachoProdgen(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetMeDespachoProdgenRepository().Delete(tipoDato, fechaIni, fechaFin, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_DESPACHO_PRODGEN
        /// </summary>
        public MeDespachoProdgenDTO GetByIdMeDespachoProdgen(int dpgencodi)
        {
            return FactorySic.GetMeDespachoProdgenRepository().GetById(dpgencodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_DESPACHO_PRODGEN
        /// </summary>
        public List<MeDespachoProdgenDTO> ListMeDespachoProdgens(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagRER)
        {
            return FactorySic.GetMeDespachoProdgenRepository().ListResumen(tipoDato, fechaIni, fechaFin, flagRER);
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeDespachoProdgen
        /// </summary>
        public List<MeDespachoProdgenDTO> GetByCriteriaMeDespachoProdgens(int tipoDato, DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER)
        {
            return FactorySic.GetMeDespachoProdgenRepository().GetByCriteria(tipoDato, fechaIni, fechaFin, flagIntegrante, flagRER);
        }

        #endregion

        #region Métodos Tabla ME_DESPACHO_RESUMEN

        /// <summary>
        /// Inserta un registro de la tabla ME_DESPACHO_RESUMEN
        /// </summary>
        public void SaveMeDespachoResumen(MeDespachoResumenDTO entity, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetMeDespachoResumenRepository().Save(entity, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla ME_DESPACHO_RESUMEN
        /// </summary>
        public void UpdateMeDespachoResumen(MeDespachoResumenDTO entity)
        {
            try
            {
                FactorySic.GetMeDespachoResumenRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla ME_DESPACHO_RESUMEN
        /// </summary>
        public void DeleteMeDespachoResumen(int tipoDato, DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            try
            {
                FactorySic.GetMeDespachoResumenRepository().Delete(tipoDato, fechaIni, fechaFin, conn, tran);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla ME_DESPACHO_RESUMEN
        /// </summary>
        public MeDespachoResumenDTO GetByIdMeDespachoResumen(int dregencodi)
        {
            return FactorySic.GetMeDespachoResumenRepository().GetById(dregencodi);
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla ME_DESPACHO_RESUMEN
        /// </summary>
        public List<MeDespachoResumenDTO> ListMeDespachoResumens()
        {
            return FactorySic.GetMeDespachoResumenRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeDespachoResumen
        /// </summary>
        public List<MeDespachoResumenDTO> GetByCriteriaMeDespachoResumens(int tipoDato, DateTime fechaIni, DateTime fechaFin)
        {
            return FactorySic.GetMeDespachoResumenRepository().GetByCriteria(tipoDato, fechaIni, fechaFin);
        }

        #endregion

        /// <summary>
        /// Permite listar los tipos de empresas
        /// </summary>
        /// <returns></returns>
        public List<SiTipoempresaDTO> ListaTipoEmpresas()
        {
            return FactorySic.GetSiTipoempresaRepository().List();
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
        /// Lista los tipos de generación
        /// </summary>
        /// <returns></returns>
        public List<SiTipogeneracionDTO> ListaTipoGeneracion()
        {
            return FactorySic.GetSiTipogeneracionRepository().GetByCriteria().Where(x => x.Tgenercodi != -1
                && x.Tgenercodi != 0 && x.Tgenercodi != 5).ToList();
        }

        /// <summary>
        /// Listado de registros de la tabla SI_Fuenteenergia
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListarFuenteEnergia()
        {
            List<SiFuenteenergiaDTO> lista = FactorySic.GetSiFuenteenergiaRepository().List();

            foreach (var reg in lista)
            {
                switch (reg.Fenergcodi)
                {
                    case ConstantesPR5ReportesServicio.FenergcodiAgua:
                        reg.Fenergorden = 1;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiGas:
                        reg.Fenergorden = 2;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiCarbon:
                        reg.Fenergorden = 3;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiDiesel:
                        reg.Fenergorden = 4;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiR500:
                        reg.Grupocomb = ConstantesPR5ReportesServicio.GrupocombR500;
                        reg.Fenergorden = 5;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiR6:
                        reg.Grupocomb = ConstantesPR5ReportesServicio.GrupocombR6;
                        reg.Fenergorden = 6;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiBagazo:
                        reg.Fenergorden = 7;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiBiogas:
                        reg.Fenergorden = 8;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiSolar:
                        reg.Fenergorden = 9;
                        break;
                    case ConstantesPR5ReportesServicio.FenergcodiEolica:
                        reg.Fenergorden = 10;
                        break;
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite obtener las empresa por tipo
        /// </summary>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObteneEmpresasPorTipo(string tiposEmpresa)
        {
            if (string.IsNullOrEmpty(tiposEmpresa)) tiposEmpresa = ConstantesAppServicio.ParametroDefecto;
            return FactorySic.GetSiEmpresaRepository().GetByCriteria(tiposEmpresa);
        }


        #region Métodos de consulta ejecutado diario

        /// <summary>
        /// Permite obtener el número de registros de la consulta del despacho ejecutado diario
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <returns></returns>
        public int ObtenerNroRegistrosConsulta(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa,
            string empresas, string tiposGeneracion)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            return FactorySic.GetMeMedicion48Repository().ObtenerNroRegistrosEjecutado(fechaInicial, fechaFinal,
                empresas, tiposEmpresa);
        }

        /// <summary>
        /// Permite obtener la consulta del despacho ejecutado diario
        /// </summary>       
        public List<MeMedicion48DTO> ObtenerConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa,
            string empresas, string tiposGeneracion, int nroPagina, int nroRegistros, out List<MeMedicion48DTO> sumatoria)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            List<MeMedicion48DTO> entitys = FactorySic.GetMeMedicion48Repository().ObtenerConsultaEjecutado(fechaInicial,
                fechaFinal, empresas, tiposGeneracion, nroPagina, nroRegistros);
            sumatoria = FactorySic.GetMeMedicion48Repository().ObtenerTotalConsultaEjecutado(fechaInicial, fechaFinal,
                empresas, tiposGeneracion);

            return entitys;
        }

        /// <summary>
        /// Permite generar el archivo del reporte
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="path"></param>
        /// <param name="file"></param>
        public void GenerarArchivoExportacion(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string path, string file)
        {
            try
            {
                if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
                {
                    List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                    empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
                }

                List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerExportacionConsultaEjecutado(fechaInicial,
                    fechaFinal, empresas, tiposGeneracion);

                file = path + file;
                FileInfo newFile = new FileInfo(file);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(file);
                }

                using (this.xlPackage = new ExcelPackage(newFile))
                {
                    this.CreaHojaHorizontal("ACTIVA (MW)", list, fechaInicial.ToString("dd/MM/yyyy"),
                            fechaFinal.ToString("dd/MM/yyyy"), "DESPACHO EJECUTADO DIARIO");

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea la hoja con los datos a exportar
        /// </summary>
        /// <param name="hojaName"></param>
        /// <param name="list"></param>
        protected void CreaHojaHorizontal(string hojaName, List<MeMedicion48DTO> list, string fechaInicio, string fechaFin, string titulo)
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
                ws.Cells[10, 7].Value = "TOTAL ENERGIA ACTIVA  (MWh)";

                DateTime now = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                for (int i = 1; i <= 48; i++)
                {
                    DateTime fecColumna = now.AddMinutes(30 * i);
                    ws.Cells[10, 7 + i].Value = fecColumna.ToString("HH:mm");
                }

                rg = ws.Cells[10, 2, 10, 55];
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
                    ws.Cells[row, 3].Value = item.Ptomedicodi;
                    ws.Cells[row, 4].Value = item.Emprnomb;
                    ws.Cells[row, 5].Value = item.Central;
                    ws.Cells[row, 6].Value = item.Equinomb;

                    ws.Cells[row, 7].Formula = "=SUM(" + this.ObtenerColumnNumber(row, 8) + ":" + this.ObtenerColumnNumber(row, 55) + ")/4";

                    for (int k = 1; k <= 48; k++)
                    {
                        object resultado = item.GetType().GetProperty("H" + k).GetValue(item, null);
                        if (resultado != null)
                        {
                            ws.Cells[row, 7 + k].Value = Convert.ToDecimal(resultado);
                        }
                    }

                    rg = ws.Cells[row, 2, row, 55];
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

                for (int t = 7; t <= 55; t++)
                {
                    ws.Column(t).Style.Numberformat.Format = "#,##0.000";
                }

                ws.Cells[row, 2].Value = "TOTAL ENERGÍA (MWh)";
                rg = ws.Cells[row, 2, row, 6];
                rg.Merge = true;
                rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rg = ws.Cells[row, 2, row, 55];
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

                for (int k = 1; k <= 48; k++)
                {
                    ws.Cells[row, 7 + k].Formula = "=SUM(" + this.ObtenerColumnNumber(11, 7 + k) + ":" + this.ObtenerColumnNumber(11 + list.Count - 1, 7 + k) + ")/2";
                }


                rg = ws.Cells[1, 3, row + 3, 55];
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

        /// <summary>
        /// Retorna el nombre de columna
        /// </summary>
        /// <param name="nroColumn"></param>
        /// <returns></returns>
        protected string ObtenerColumnNumber(int nroFila, int nroColumn)
        {
            string[] columns = {"A","B", "C", "D", "E", "F", "G", "H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",
                               "AA","AB","AC","AD","AE","AF","AG","AH","AI","AJ","AK","AL","AM","AN","AO","AP","AQ","AR","AS","AT","AU","AV","AW","AX","AY","AZ",
                               "BA","BB","BC","BD","BE","BF","BG","BH","BI","BJ","BK","BL","BM","BN","BO","BP","BQ","BR","BS","BT","BU","BV","BW","BX","BY","BZ",
                               "CA","CB","CC","CD","CE","CF","CG","CH","CI","CJ","CK","CL","CM","CN","CO","CP","CQ","CR","CS","CT","CU","CV","CW","CX","CY","CZ",
                               "DA","DB","DC","DD","DE","DF","DG","DH","DI","DJ","DK","DL","DM","DN","DO","DP","DQ","DR","DS","DT","DU","DV","DW","DX","DY","DZ",
                               "EA","EB","EC","ED","EE","EF","EG","EH","EI","EJ","EK","EL","EM","EN","EO","EP","EQ","ER","ES","ET","EU","EV","EW","EX","EY","EZ",
                               "FA","FB","FC","FD","FE","FF","FG","FH","FI","FJ","FK","FL","FM","FN","FO","FP","FQ","FR","FS","FT","FU","FV","FW","FX","FY","FZ",
                               "GA","GB","GC","GD","GE","GF","GG","GH","GI","GJ","GK","GL","GM","GN","GO","GP","GQ","GR","GS","GT","GU","GV","GW","GX","GY","GZ",
                               "HA","HB","HC","HD","HE","HF","HG","HH","HI","HJ","HK","HL","HM","HN","HO","HP","HQ","HR","HS","HT","HU","HV","HW","HX","HY","HZ",
                               "IA","IB","IC","ID","IE","IF","IG","IH","II","IJ","IK","IL","IM","IN","IO","IP","IQ","IR","IS","IT","IU","IV","IW","IX","IY","IZ"
                               };

            int index = nroColumn - 1;

            return columns[index] + nroFila;
        }

        #endregion

        #region Métodos Consolidado Ejecutado Mensual

        public List<MedicionReporteDTO> ObtenerReporteConsolidado(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion)
        {
            #region Parametros

            string fuentesEnergia = ConstantesAppServicio.ParametroDefecto;

            if (!string.IsNullOrEmpty(tiposGeneracion))
            {
                if (tiposGeneracion != ConstantesAppServicio.ParametroDefecto)
                {
                    List<SiFuenteenergiaDTO> listFuente = this.ListaFuenteEnergia(0);
                    List<int> tipos = (tiposGeneracion.Split(ConstantesAppServicio.CaracterComa)).Select(n => int.Parse(n)).ToList();
                    List<int> ids = listFuente.Where(x => tipos.Any(y => x.Tgenercodi == y)).Select(x => x.Fenergcodi).ToList();
                    fuentesEnergia = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), ids);

                    if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = ConstantesAppServicio.ParametroNoExiste;
                }
            }

            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            #endregion

            List<MedicionReporteDTO> resultado = new List<MedicionReporteDTO>();

            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerConsolidadoEjecutado(fechaInicial, fechaFinal,
                empresas, fuentesEnergia);

            var listEmpresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().OrderBy(x => x.Emprnomb).ToList();
            var listCentrales = list.Select(x => new { x.Emprcodi, x.Central }).Distinct().ToList();
            var listFechas = list.Select(x => new { ((DateTime)x.Medifecha).Year, ((DateTime)x.Medifecha).Month }).Distinct().ToList();

            var listEquipos = list.Select(x => new
            {
                x.Emprcodi,
                x.Central,
                x.Equicodi,
                x.Equinomb,
                x.Tgenernomb,
                x.Tgenercodi,
                x.Fenergcodi,
                x.Fenergnomb
            }).Distinct().ToList();

            foreach (var itemEmpresa in listEmpresas)
            {
                var subCentral = listCentrales.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).ToList();

                foreach (var itemCentral in subCentral)
                {
                    var subEquipo = listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi).ToList();

                    foreach (var itemEquipo in subEquipo)
                    {
                        foreach (var itemFecha in listFechas)
                        {
                            List<MeMedicion48DTO> subList = list.Where(x => x.Equicodi == itemEquipo.Equicodi &&
                                x.Medifecha.Year == itemFecha.Year && x.Medifecha.Month == itemFecha.Month).ToList();

                            decimal valor = this.ObtenerTotalizadoPorEquipo(subList);

                            MedicionReporteDTO resultadoEquipo = new MedicionReporteDTO();

                            resultadoEquipo.Emprnomb = itemEmpresa.Emprnomb;
                            resultadoEquipo.Central = itemCentral.Central;
                            resultadoEquipo.Unidad = itemEquipo.Equinomb;
                            resultadoEquipo.Fenergcodi = itemEquipo.Fenergcodi;
                            resultadoEquipo.Fenergnomb = itemEquipo.Fenergnomb;
                            resultadoEquipo.Tgenercodi = itemEquipo.Tgenercodi;
                            resultadoEquipo.Tgenernomb = itemEquipo.Tgenernomb;
                            resultadoEquipo.Anio = itemFecha.Year;
                            resultadoEquipo.Mes = itemFecha.Month;
                            resultadoEquipo.EmprCodi = itemEmpresa.Emprcodi;
                            resultadoEquipo.EquiCodi = itemEquipo.Equicodi;
                            resultadoEquipo.ValorAcumulado = valor;

                            resultado.Add(resultadoEquipo);
                        }
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el valor total de un equipo por mes
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public decimal ObtenerTotalizadoPorEquipo(List<MeMedicion48DTO> list)
        {
            List<MeMedicion48DTO> agrupado = list;

            var lista = (from t in agrupado
                         group t by new { t.Medifecha }
                             into destino
                         select new MeMedicion48DTO()
                         {
                             Medifecha = destino.Key.Medifecha,
                             H1 = destino.Sum(t => t.H1),
                             H2 = destino.Sum(t => t.H2),
                             H3 = destino.Sum(t => t.H3),
                             H4 = destino.Sum(t => t.H4),
                             H5 = destino.Sum(t => t.H5),
                             H6 = destino.Sum(t => t.H6),
                             H7 = destino.Sum(t => t.H7),
                             H8 = destino.Sum(t => t.H8),
                             H9 = destino.Sum(t => t.H9),
                             H10 = destino.Sum(t => t.H10),
                             H11 = destino.Sum(t => t.H11),
                             H12 = destino.Sum(t => t.H12),
                             H13 = destino.Sum(t => t.H13),
                             H14 = destino.Sum(t => t.H14),
                             H15 = destino.Sum(t => t.H15),
                             H16 = destino.Sum(t => t.H16),
                             H17 = destino.Sum(t => t.H17),
                             H18 = destino.Sum(t => t.H18),
                             H19 = destino.Sum(t => t.H19),
                             H20 = destino.Sum(t => t.H20),
                             H21 = destino.Sum(t => t.H21),
                             H22 = destino.Sum(t => t.H22),
                             H23 = destino.Sum(t => t.H23),
                             H24 = destino.Sum(t => t.H24),
                             H25 = destino.Sum(t => t.H25),
                             H26 = destino.Sum(t => t.H26),
                             H27 = destino.Sum(t => t.H27),
                             H28 = destino.Sum(t => t.H28),
                             H29 = destino.Sum(t => t.H29),
                             H30 = destino.Sum(t => t.H30),
                             H31 = destino.Sum(t => t.H31),
                             H32 = destino.Sum(t => t.H32),
                             H33 = destino.Sum(t => t.H33),
                             H34 = destino.Sum(t => t.H34),
                             H35 = destino.Sum(t => t.H35),
                             H36 = destino.Sum(t => t.H36),
                             H37 = destino.Sum(t => t.H37),
                             H38 = destino.Sum(t => t.H38),
                             H39 = destino.Sum(t => t.H39),
                             H40 = destino.Sum(t => t.H40),
                             H41 = destino.Sum(t => t.H41),
                             H42 = destino.Sum(t => t.H42),
                             H43 = destino.Sum(t => t.H43),
                             H44 = destino.Sum(t => t.H44),
                             H45 = destino.Sum(t => t.H45),
                             H46 = destino.Sum(t => t.H46),
                             H47 = destino.Sum(t => t.H47),
                             H48 = destino.Sum(t => t.H48)
                         }).ToList();

            decimal suma = 0;
            foreach (MeMedicion48DTO item in lista)
            {
                for (int i = 1; i <= 48; i++)
                {
                    var valor = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(item, null);
                    suma = suma + (decimal)valor;
                }
            }

            return suma;
        }

        /// <summary>
        /// Lista las fuentes de energía
        /// </summary>
        /// <returns></returns>
        public List<SiFuenteenergiaDTO> ListaFuenteEnergia(int idTipoGeneracion)
        {
            return FactorySic.GetSiFuenteenergiaRepository().GetByCriteria().Where(x => x.Fenergcodi != -1
                && x.Fenergcodi != 0 && (x.Tgenercodi == idTipoGeneracion || idTipoGeneracion == 0)).ToList();
        }

        #endregion


        /// <summary>
        /// Permite obtener la consulta de CMgReal por área
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<ReporteCMGRealDTO> ObtenerConsultaCMgRealPorArea(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReporteCMGRealDTO> resultado = new List<ReporteCMGRealDTO>();

            List<MeMedicion48DTO> list = FactorySic.GetMeMedicion48Repository().ObtenerConsultaCMgRealPorArea(fechaInicio, fechaFin);
            List<int> puntos = new List<int> { 1197, 1230, 1231 };
            int dias = (int)fechaFin.Subtract(fechaInicio).TotalDays;

            for (int d = 0; d <= dias; d++)
            {
                DateTime fechaConsulta = fechaInicio.AddDays(d);

                List<MeMedicion48DTO> subList = list.Where(x => x.Medifecha.Year == fechaConsulta.Year && x.Medifecha.Month == fechaConsulta.Month
                    && x.Medifecha.Day == fechaConsulta.Day).ToList();

                MeMedicion48DTO itemCentro = subList.Where(x => x.Ptomedicodi == 1197).FirstOrDefault();
                MeMedicion48DTO itemNorte = subList.Where(x => x.Ptomedicodi == 1230).FirstOrDefault();
                MeMedicion48DTO itemSur = subList.Where(x => x.Ptomedicodi == 1231).FirstOrDefault();

                for (int i = 1; i <= 48; i++)
                {
                    ReporteCMGRealDTO itemResultado = new ReporteCMGRealDTO();
                    itemResultado.Fecha = fechaConsulta.AddMinutes(i * 30);
                    if (itemCentro != null) itemResultado.Centro = (decimal)itemCentro.GetType().GetProperty(
                        ConstantesMedicion.CaracterH + i).GetValue(itemCentro, null);
                    if (itemNorte != null) itemResultado.Norte = (decimal)itemNorte.GetType().GetProperty(
                        ConstantesMedicion.CaracterH + i).GetValue(itemNorte, null);
                    if (itemSur != null) itemResultado.Sur = (decimal)itemSur.GetType().GetProperty(
                        ConstantesMedicion.CaracterH + i).GetValue(itemSur, null);
                    resultado.Add(itemResultado);
                }
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener el resultado de la consulta de cmg por area
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerConsultaCMgReal(DateTime fechaInicio, DateTime fechaFin)
        {
            return FactorySic.GetMeMedicion48Repository().ObtenerConsultaCMgRealPorArea(fechaInicio, fechaFin);
        }

        #region Maxima Demanda - Despacho

        /// <summary>
        /// Obtener la data con el valor de Meditotal correcto
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaMDDataGeneracion"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataMDGeneracionFromConsolidado48(DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> listaMDDataGeneracion)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();

            if (listaMDDataGeneracion.Count > 0)
            {
                var regPrimer = listaMDDataGeneracion.First();

                for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
                {
                    var listaMedicionFuente = listaMDDataGeneracion.Where(x => x.Medifecha.Date == f).ToList();

                    MeMedicion48DTO reg = new MeMedicion48DTO();
                    reg.Medifecha = f;
                    reg.Tipoinfocodi = regPrimer.Tipoinfocodi;
                    reg.Lectcodi = regPrimer.Lectcodi;

                    foreach (var regtmp in listaMedicionFuente)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty("H" + (i)).GetValue(regtmp, null);
                            decimal? valorAcum = (decimal?)reg.GetType().GetProperty("H" + (i)).GetValue(reg, null);
                            if (valorNuevo != null)
                            {
                                valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                                reg.GetType().GetProperty("H" + (i)).SetValue(reg, valorAcum);
                            }
                        }
                    }

                    listaFinal.Add(reg);
                }

                decimal? valorH = null;
                decimal total = 0;
                List<decimal> listaH = new List<decimal>();

                foreach (var reg in listaFinal)
                {
                    listaH = new List<decimal>();
                    total = 0;
                    for (int h = 1; h <= 48; h++)
                    {
                        valorH = (decimal?)reg.GetType().GetProperty("H" + h).GetValue(reg, null);
                        if (valorH != null)
                        {
                            listaH.Add(valorH.Value);
                        }
                    }

                    if (listaH.Count > 0)
                    {
                        total = listaH.Sum(x => x);
                    }

                    reg.Meditotal = total;
                }
            }

            return listaFinal;
        }

        /// <summary>
        /// Data total del SEIN - Cada 30min
        /// </summary>
        /// <param name="listaMDDataGeneracion"></param>
        /// <param name="listaMDDataInterconexion48"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataMDTotalSEIN48(List<MeMedicion48DTO> listaMDDataGeneracion, List<MeMedicion48DTO> listaMDDataInterconexion48)
        {
            List<MeMedicion48DTO> listaMedicion = new List<MeMedicion48DTO>();
            List<decimal> listaH = new List<decimal>();
            foreach (var reg in listaMDDataGeneracion)
            {
                MeMedicion48DTO regTot = new MeMedicion48DTO();
                regTot.Medifecha = reg.Medifecha;
                regTot.Tipoinfocodi = reg.Tipoinfocodi;
                regTot.Lectcodi = reg.Lectcodi;
                regTot.Tipoptomedicodi = reg.Tipoptomedicodi;
                regTot.Emprcodi = reg.Emprcodi;
                regTot.Emprnomb = reg.Emprnomb;
                regTot.Tgenercodi = reg.Tgenercodi;

                var regTIE = listaMDDataInterconexion48.ToList().FirstOrDefault(x => x.Medifecha == reg.Medifecha) ?? new MeMedicion48DTO();

                listaH = new List<decimal>();
                for (int i = 1; i <= 48; i++)
                {
                    decimal valorTie = ((decimal?)regTIE.GetType().GetProperty("H" + i.ToString()).GetValue(regTIE, null)).GetValueOrDefault(0);
                    decimal newval = ((decimal?)reg.GetType().GetProperty("H" + i.ToString()).GetValue(reg, null)).GetValueOrDefault(0) - valorTie;
                    regTot.GetType().GetProperty("H" + i.ToString()).SetValue(regTot, newval);
                    listaH.Add(newval);

                }
                regTot.Meditotal = listaH.Sum(x => x);
                listaMedicion.Add(regTot);
            }

            return listaMedicion;
        }

        public List<MeMedicion48DTO> ListaDataMDTotalSEINXPto48(List<MeMedicion48DTO> listaMDDataGeneracion)
        {

            List<MeMedicion48DTO> listAreaXDia = (from t in listaMDDataGeneracion
                                                  group t by new { t.Ptomedicodi, t.Medifecha }
                                                        into destino
                                                  select new MeMedicion48DTO()
                                                  {
                                                      Ptomedicodi = destino.Key.Ptomedicodi,
                                                      Medifecha = destino.Key.Medifecha,
                                                      H1 = destino.Sum(t => t.H1),
                                                      H2 = destino.Sum(t => t.H2),
                                                      H3 = destino.Sum(t => t.H3),
                                                      H4 = destino.Sum(t => t.H4),
                                                      H5 = destino.Sum(t => t.H5),
                                                      H6 = destino.Sum(t => t.H6),
                                                      H7 = destino.Sum(t => t.H7),
                                                      H8 = destino.Sum(t => t.H8),
                                                      H9 = destino.Sum(t => t.H9),
                                                      H10 = destino.Sum(t => t.H10),

                                                      H11 = destino.Sum(t => t.H11),
                                                      H12 = destino.Sum(t => t.H12),
                                                      H13 = destino.Sum(t => t.H13),
                                                      H14 = destino.Sum(t => t.H14),
                                                      H15 = destino.Sum(t => t.H15),
                                                      H16 = destino.Sum(t => t.H16),
                                                      H17 = destino.Sum(t => t.H17),
                                                      H18 = destino.Sum(t => t.H18),
                                                      H19 = destino.Sum(t => t.H19),
                                                      H20 = destino.Sum(t => t.H20),

                                                      H21 = destino.Sum(t => t.H21),
                                                      H22 = destino.Sum(t => t.H22),
                                                      H23 = destino.Sum(t => t.H23),
                                                      H24 = destino.Sum(t => t.H24),
                                                      H25 = destino.Sum(t => t.H25),
                                                      H26 = destino.Sum(t => t.H26),
                                                      H27 = destino.Sum(t => t.H27),
                                                      H28 = destino.Sum(t => t.H28),
                                                      H29 = destino.Sum(t => t.H29),
                                                      H30 = destino.Sum(t => t.H30),

                                                      H31 = destino.Sum(t => t.H31),
                                                      H32 = destino.Sum(t => t.H32),
                                                      H33 = destino.Sum(t => t.H33),
                                                      H34 = destino.Sum(t => t.H34),
                                                      H35 = destino.Sum(t => t.H35),
                                                      H36 = destino.Sum(t => t.H36),
                                                      H37 = destino.Sum(t => t.H37),
                                                      H38 = destino.Sum(t => t.H38),
                                                      H39 = destino.Sum(t => t.H39),
                                                      H40 = destino.Sum(t => t.H40),

                                                      H41 = destino.Sum(t => t.H41),
                                                      H42 = destino.Sum(t => t.H42),
                                                      H43 = destino.Sum(t => t.H43),
                                                      H44 = destino.Sum(t => t.H44),
                                                      H45 = destino.Sum(t => t.H45),
                                                      H46 = destino.Sum(t => t.H46),
                                                      H47 = destino.Sum(t => t.H47),
                                                      H48 = destino.Sum(t => t.H48)
                                                  }).ToList();

            List<decimal> listaH;
            decimal total; decimal? valorH;
            foreach (var reg in listAreaXDia)
            {
                listaH = new List<decimal>();
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valorH = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(reg, null);
                    if (valorH != null)
                    {
                        listaH.Add(valorH.Value);
                    }
                }

                if (listaH.Count > 0)
                {
                    total = listaH.Sum(x => x);
                }

                reg.Meditotal = total;
            }

            return listAreaXDia;
        }

        /// <summary>
        /// Data total del SEIN - Cada 1h
        /// </summary>
        /// <param name="listaMDDataGeneracion"></param>
        /// <returns></returns>
        public List<MeMedicion24DTO> ListaDataMDTotalSEIN24(List<MeMedicion48DTO> listaMDDataGeneracion)
        {
            List<MeMedicion24DTO> listaMedicion = new List<MeMedicion24DTO>();
            List<decimal> listaH = new List<decimal>();
            foreach (var reg in listaMDDataGeneracion)
            {
                MeMedicion24DTO regTot = new MeMedicion24DTO();
                regTot.Medifecha = reg.Medifecha;
                regTot.Tipoinfocodi = reg.Tipoinfocodi;
                regTot.Lectcodi = reg.Lectcodi;
                regTot.Tipoptomedicodi = reg.Tipoptomedicodi;
                regTot.Emprcodi = reg.Emprcodi;
                regTot.Emprnomb = reg.Emprnomb;
                regTot.Tgenercodi = reg.Tgenercodi;
                regTot.Tgenernomb = reg.Tgenernomb;
                regTot.Ptomedicodi = reg.Ptomedicodi;

                listaH = new List<decimal>();
                for (int i = 1; i <= 24; i++)
                {
                    decimal newval1 = ((decimal?)reg.GetType().GetProperty("H" + (i * 2 - 1).ToString()).GetValue(reg, null)).GetValueOrDefault(0);
                    decimal newval2 = ((decimal?)reg.GetType().GetProperty("H" + (i * 2).ToString()).GetValue(reg, null)).GetValueOrDefault(0);
                    decimal newval = (newval1 + newval2) / 2;
                    regTot.GetType().GetProperty("H" + i.ToString()).SetValue(regTot, newval);
                    listaH.Add(newval);

                }
                regTot.Meditotal = listaH.Sum(x => x);
                listaMedicion.Add(regTot);
            }

            return listaMedicion;
        }

        /// <summary>
        /// Data total del SEIN - Día
        /// </summary>
        /// <param name="listaMDDataGeneracion"></param>
        /// <returns></returns>
        public List<MeMedicion1DTO> ListaDataMDTotalSEIN1(List<MeMedicion24DTO> listaMDDataGeneracion)
        {
            List<MeMedicion1DTO> listaMedicion = new List<MeMedicion1DTO>();
            List<decimal> listaH = new List<decimal>();
            foreach (var reg in listaMDDataGeneracion)
            {
                MeMedicion1DTO regTot = new MeMedicion1DTO();
                regTot.Medifecha = reg.Medifecha;
                regTot.Tipoinfocodi = reg.Tipoinfocodi;
                regTot.Lectcodi = reg.Lectcodi;
                regTot.Tipoptomedicodi = reg.Tipoptomedicodi;
                regTot.Emprcodi = reg.Emprcodi;
                regTot.Emprnomb = reg.Emprnomb;
                regTot.H1 = reg.Meditotal;
                regTot.Ptomedicodi = reg.Ptomedicodi;

                listaMedicion.Add(regTot);
            }

            return listaMedicion;
        }

        /// <summary>
        /// Obtener día y hora de la máxima demanda menos 30minutos
        /// Si la maxima hora es las H48, devolvería la fecha del dia siguiente por tanto se le resta 30min para que devuelva el mismo día,
        /// posteiormente para efectos de reporte se le suma 15min, pero para consulta de data se toma el Medifecha.Date
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="bloquePeriodo"></param>
        /// <param name="listaMedicionTotal"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <param name="fechaMD48"></param>
        /// <param name="fechaDia48"></param>
        /// <param name="hMax48"></param>
        public void GetDiaMaximaDemandaFromDataMD48(DateTime fechaInicio, DateTime fechaFin, int bloquePeriodo, 
            List<MeMedicion48DTO> listaMedicionTotal, List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario, 
            out DateTime fechaMD48, out DateTime fechaDia48, out int hMax48)
        {
            List<MeMedicion48DTO> listaMedicion = listaMedicionTotal.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date).ToList();

            decimal maximoValor = 0;
            DateTime maximoValorDia = fechaInicio.Date;
            int maximoValorColumna = 1;

            if (listaMedicion.Count() != 0)
            {
                for (var i = 0; i < listaMedicion.Count(); i++)
                {
                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(bloquePeriodo, listaMedicion[i].Medifecha, new List<MeMedicion48DTO>() { listaMedicion[i] }, listaRangoNormaHP, listaBloqueHorario,
                                                        out decimal valorDia, out int hDia, out DateTime fechaHoraDia);

                    if (valorDia > maximoValor)
                    {
                        maximoValor = valorDia;
                        maximoValorDia = listaMedicion[i].Medifecha;
                        maximoValorColumna = hDia;
                    }
                }
            }

            //salidas
            fechaDia48 = maximoValorDia;
            fechaMD48= maximoValorDia.AddMinutes(maximoValorColumna * 30);
            hMax48 = maximoValorColumna;
        }

        /// <summary>
        /// /// Obtener día y hora de la máxima demanda menos 60minutos 
        /// Si la maxima hora es las H24, devolvería la fecha del dia siguiente por tanto se le resta 60min para que devuelva el mismo día,
        /// posteiormente para efectos de reporte se le suma 60min, pero para consulta de data se toma el Medifecha.Date
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="bloquePeriodo"></param>
        /// <param name="listaMedicionTotal"></param>
        /// <param name="listaRangoNormaHP"></param>
        /// <param name="listaBloqueHorario"></param>
        /// <returns></returns>
        public DateTime GetDiaMaximaDemandaFromDataMD24(DateTime fechaInicio, DateTime fechaFin, int bloquePeriodo
            , List<MeMedicion24DTO> listaMedicionTotal, List<SiParametroValorDTO> listaRangoNormaHP, List<SiParametroValorDTO> listaBloqueHorario)
        {
            List<MeMedicion24DTO> listaMedicion = listaMedicionTotal.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date).ToList();

            decimal maximoValor = 0;
            DateTime maximoValorDia = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);
            int maximoValorColumna = 1;

            if (listaMedicion.Count() != 0)
            {
                for (var i = 0; i < listaMedicion.Count(); i++)
                {
                    MedidoresHelper.ObtenerValorHXPeriodoDemandaM24(bloquePeriodo, listaMedicion[i].Medifecha, new List<MeMedicion24DTO>() { listaMedicion[i] }, listaRangoNormaHP, listaBloqueHorario,
                                                        out decimal valorDia, out int hDia, out DateTime fechaHoraDia);

                    if (valorDia > maximoValor)
                    {
                        maximoValor = valorDia;
                        maximoValorDia = listaMedicion[i].Medifecha;
                        maximoValorColumna = hDia;
                    }
                }
            }

            //Si la maxima hora es las H24, devolvería la fecha del dia siguiente por tanto se le resta 60min para que devuelva el mismo día,
            //posteiormente para efectos de reporte se le suma 60min, pero para consulta de data se toma el Medifecha.Date
            return maximoValorDia.AddMinutes((maximoValorColumna - 1) * 60);
        }

        /// <summary>
        /// Obtener día de la máxima demanda segun data de entrada, Demanda Horaria
        /// </summary>
        /// <param name="listaMedicion"></param>
        /// <returns></returns>
        public DateTime GetDiaMaximaDemandaFromDataMD1(DateTime fechaInicio, DateTime fechaFin, List<MeMedicion1DTO> listaMedicionTotal)
        {
            List<MeMedicion1DTO> listaMedicion = listaMedicionTotal.Where(x => x.Medifecha >= fechaInicio.Date && x.Medifecha <= fechaFin.Date).ToList();

            decimal maximoValor = 0;
            decimal valorH = 0;
            DateTime maximoValorDia = new DateTime(fechaInicio.Year, fechaInicio.Month, 1);

            if (listaMedicion.Count() != 0)
            {
                for (var i = 0; i < listaMedicion.Count(); i++)
                {
                    valorH = listaMedicion[i].H1.GetValueOrDefault(0);

                    if (valorH > maximoValor)
                    {
                        maximoValor = valorH;
                        maximoValorDia = listaMedicion[i].Medifecha;
                    }
                }
            }

            return maximoValorDia;
        }

        /// <summary>
        /// Data detallada cada 30min de Despacho
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <param name="tipoRecurso"></param>
        /// <param name="usoHOP"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataMDGeneracionConsolidado48(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int estadoValidacion, string tipoRecurso, bool hayCruceHOP, int lectcodi)
        {
            return this.ListaDataGeneracion48(fechaIni, fechaFin, tipoCentral, tipoGeneracion, idEmpresa, estadoValidacion, tipoRecurso, hayCruceHOP, ConstantesMedicion.IdTipoInfoPotenciaActiva, lectcodi);
        }

        /// <summary>
        /// Listar data de medicion48
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="tipoGeneracion"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estadoValidacion"></param>
        /// <param name="tipoRecurso"></param>
        /// <param name="hayCruceHOP"></param>
        /// <param name="tipoinfocodi"></param>
        /// <param name="lectcodi"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataGeneracion48(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int estadoValidacion, string tipoRecurso, bool hayCruceHOP, int tipoinfocodi, int lectcodi)
        {
            //Coes, no Coes
            List<PrGrupodatDTO> listaOperacionCoes = FactorySic.GetPrGrupodatRepository().ListarHistoricoValores(ConstantesPR5ReportesServicio.PropGrupoOperacionCoes.ToString(), -1)
                                                        .Where(x => x.Deleted == 0).OrderByDescending(x => x.Fechadat).ToList();

            //Medicion
            List<MeMedicion48DTO> listaM48Rango = FactorySic.GetMeMedicion48Repository().GetConsolidadoMaximaDemanda48SinGrupoIntegrante(tipoCentral
                    , tipoGeneracion, fechaIni, fechaFin, idEmpresa
                    , lectcodi, tipoinfocodi, ConstantesMedicion.IdTptomedicodiTodos);

            //Determinar que el dato es integrante o no del COES para ese dia 
            foreach (var reg48 in listaM48Rango)
            {
                reg48.Grupointegrante = ReporteMedidoresAppServicio.SetValorCentralIntegrante(reg48.Grupocodi, reg48.Medifecha, reg48.Grupointegrante, listaOperacionCoes);
            }

            if (tipoCentral == ConstantesMedicion.IdTipogrupoCOES)
                listaM48Rango = listaM48Rango.Where(x => x.Grupointegrante == ConstantesAppServicio.SI).ToList();
            if (tipoCentral == ConstantesMedicion.IdTipogrupoNoIntegrante)
                listaM48Rango = listaM48Rango.Where(x => x.Grupointegrante != ConstantesAppServicio.SI).ToList();

            //Cruce con Horas de Operación
            List<MeMedicion48DTO> listaTotal;
            if (hayCruceHOP)
            {
                listaTotal = new List<MeMedicion48DTO>();

                string strFechaCruceHO = System.Configuration.ConfigurationManager.AppSettings["FechaCruceHoraOperacion"];
                DateTime fechaCruceHO = DateTime.ParseExact(strFechaCruceHO, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                #region cruce para obtener datos por fuente de energia

                DateTime? fechaIniSinCruceHO = null;
                DateTime? fechaFinSinCruceHO = null;
                DateTime? fechaIniConCruceHO = null;
                DateTime? fechaFinConCruceHO = null;
                if (fechaIni <= fechaCruceHO && fechaCruceHO <= fechaFin)
                {
                    fechaIniSinCruceHO = fechaIni;
                    fechaFinSinCruceHO = fechaCruceHO.AddDays(-1);
                    fechaIniConCruceHO = fechaCruceHO;
                    fechaFinConCruceHO = fechaFin;
                }
                else
                {
                    if (fechaCruceHO <= fechaIni)
                    {
                        fechaIniConCruceHO = fechaIni;
                        fechaFinConCruceHO = fechaFin;
                    }
                    else
                    {
                        fechaIniSinCruceHO = fechaIni;
                        fechaFinSinCruceHO = fechaFin;
                    }
                }

                List<MeMedicion48DTO> listaMedicionSinCruce = new List<MeMedicion48DTO>();
                List<MeMedicion48DTO> listaMedicionConCruce = new List<MeMedicion48DTO>();

                if (fechaIniSinCruceHO != null && fechaFinSinCruceHO != null)
                {
                    listaMedicionSinCruce = listaM48Rango.Where(x => x.Medifecha >= fechaIniSinCruceHO && x.Medifecha <= fechaFinSinCruceHO).ToList();
                }
                if (fechaIniConCruceHO != null && fechaFinConCruceHO != null)
                {
                    listaMedicionConCruce = listaM48Rango.Where(x => x.Medifecha >= fechaIniConCruceHO && x.Medifecha <= fechaFinConCruceHO).ToList();
                    List<EveHoraoperacionDTO> listaHOPTotal = this.servHO.ListarHorasOperacionByCriteria(fechaIniConCruceHO.Value, fechaFinConCruceHO.Value.AddDays(1), ConstantesHorasOperacion.ParamEmpresaTodos, ConstantesHorasOperacion.ParamCentralTodos, ConstantesHorasOperacion.TipoListadoTodo);
                    listaMedicionConCruce = this.ListarData48CruceHorasOperacion(fechaIniConCruceHO.Value, fechaFinConCruceHO.Value, listaMedicionConCruce, listaHOPTotal, false, false);
                }

                #endregion

                listaTotal.AddRange(listaMedicionSinCruce);
                listaTotal.AddRange(listaMedicionConCruce);
            }
            else 
            {
                //sin cruce
                listaTotal = listaM48Rango;
            }

            //Filtrar Fuente de energía
            if (tipoRecurso != ConstantesMedicion.IdTipoRecursoTodos.ToString())
            {
                int[] result = tipoRecurso.Split(',').Select(x => int.Parse(x)).ToArray();
                listaTotal = listaTotal.Where(x => result.Contains(x.Fenergcodi)).ToList();
            }

            //Generar total
            decimal? valorH = null;
            decimal total = 0;
            List<decimal> listaH = new List<decimal>();

            foreach (var reg in listaTotal)
            {
                listaH = new List<decimal>();
                total = 0;
                for (int h = 1; h <= 48; h++)
                {
                    valorH = (decimal?)reg.GetType().GetProperty("H" + h).GetValue(reg, null);
                    if (valorH != null)
                    {
                        listaH.Add(valorH.Value);
                    }
                }

                if (listaH.Count > 0)
                {
                    total = listaH.Sum(x => x);
                }

                reg.Meditotal = total;
            }

            return listaTotal;
        }

        /// <summary>
        /// Listar Data de Medidores de Generación con Cruce de Horas de Operación
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaMedicionFuenteTotal"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarData48CruceHorasOperacion(DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> listaMedicionFuenteTotal, List<EveHoraoperacionDTO> listaHOPTotal
              , bool estaCompletadoHO, bool soloIncluirSiTieneHO)
        {
            List<MeMedicion48DTO> listaMedicionHOP = new List<MeMedicion48DTO>();

            List<SiFuenteenergiaDTO> listaFuenteEnergia = this.ListarFuenteEnergia();
            List<SiTipogeneracionDTO> listaTipoGeneracion = this.ListaTipoGeneracion();
            List<PrGrupoDTO> listaGrupoGeneracion = this.servHO.ListarAllGrupoGeneracion(fechaIni, "'S', 'N'");
            List<PrGrupoDTO> listaModoOpTotal = this.servHO.ListarModoOperacionXCentralYEmpresa(-2, -2);
            List<PrGrupoDTO> listaUnidadTermo = this.servHO.ListarAllUnidadTermoelectrica();

            if (!estaCompletadoHO)
                listaHOPTotal = this.servHO.CompletarListaHoraOperacionTermo(listaHOPTotal);

            for (DateTime f = fechaIni.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                var listaMedicionFuente = listaMedicionFuenteTotal.Where(x => x.Medifecha.Date == f).ToList();

                var listaHO30min = this.servHO.ListarHO30min(listaHOPTotal, f);
                List<EveHoraoperacionDTO> listaHOModo = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoModo).ToList();
                List<EveHoraoperacionDTO> listaHOUnidad = listaHO30min.Where(x => x.Hophorini.Value.Date == f && x.FlagTipoHo == ConstantesHorasOperacion.FlagTipoHoUnidad).ToList();

                foreach (var m48 in listaMedicionFuente)
                {
                    if (m48.Emprcodi == 8) 
                    { }
                    List<MeMedicion48DTO> listaMedicionHOPEquipo = new List<MeMedicion48DTO>();

                    var equipo = listaUnidadTermo.Find(x => x.Grupocodi == m48.Grupocodi);
                    int? equicodi = equipo != null ? equipo.Equicodi : -1;
                    bool esUnidadEspecial = equipo != null ? equipo.FlagModoEspecial == ConstantesHorasOperacion.FlagModoEspecial : false;

                    MeMedicion48DTO mHora = new MeMedicion48DTO();
                    mHora.Medifecha = f;
                    mHora.Grupocodi = m48.Grupocodi;
                    mHora.Gruponomb = m48.Gruponomb;
                    mHora.Grupocomb = m48.Grupocomb;
                    mHora.Grupopadre = m48.Grupopadre;
                    mHora.Central = m48.Central;
                    mHora.Emprcodi = m48.Emprcodi;
                    mHora.Emprnomb = m48.Emprnomb;
                    mHora.Osinergcodi = m48.Osinergcodi;

                    string msjValidacionHO = string.Empty;

                    //Determinar los grupos por cada media hora
                    for (int h = 1; h <= 48; h++)
                    {
                        DateTime fi = f.AddMinutes(h * 30);

                        PrGrupoDTO grupo = null;
                        int grupocodi, fenergcodi;
                        string grupocomb = null;

                        if (ConstantesPR5ReportesServicio.TgenercodiTermo == m48.Tgenercodi && !esUnidadEspecial)
                        {
                            EveHoraoperacionDTO hopunidad = listaHOUnidad.Find(x => x.Equicodi == equicodi && x.HoraIni48 <= fi && fi <= x.HoraFin48);
                            EveHoraoperacionDTO hop = hopunidad != null ? listaHOModo.Find(x => x.Hopcodi == hopunidad.Hopcodipadre.GetValueOrDefault(-2)) : null;
                            if (hop != null)
                            {
                                grupocodi = hop.Grupocodi.Value;
                                grupo = listaModoOpTotal.Find(x => x.Grupocodi == hop.Grupocodi);
                                mHora.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mHora, (decimal)hop.Grupocodi);
                            }
                            else
                            {
                                if (ConstantesMedicion.IdTipogrupoNoIntegrante == m48.Tipogrupocodi) //caso C.T. Tablazo
                                {
                                    grupocodi = m48.Grupocodi;
                                    grupo = listaGrupoGeneracion.Find(x => x.Grupocodi == m48.Grupocodi);
                                    mHora.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mHora, (decimal)m48.Grupocodi);
                                }
                            }
                        }
                        else
                        {
                            grupocodi = m48.Grupocodi;
                            grupo = listaGrupoGeneracion.Find(x => x.Grupocodi == m48.Grupocodi);
                            mHora.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(mHora, (decimal)m48.Grupocodi);
                        }

                        decimal? valor = (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null);
                        if (grupo == null)
                        {
                            if (valor.GetValueOrDefault(0) > 0)
                            {
                                msjValidacionHO += m48.Emprnomb + "," + m48.Central + "," + m48.Equinomb + "=> Fecha-Hora=" + fi.ToString(ConstantesAppServicio.FormatoFechaHora) + ", MW=" + valor.Value + "<br/>";
                            }
                            //si el grupo no tiene hora de operación se le asigna la fuente de energía de su unidad
                            fenergcodi = m48.Fenergcodi;

                            //si no tiene hora de operación pero sí data en M96 se le colocará 0 si el flag es obligatori tener HO
                            if (soloIncluirSiTieneHO)
                                valor = 0;
                        }
                        else
                        {
                            fenergcodi = grupo.Fenergcodi.GetValueOrDefault(-1);
                            grupocomb = grupo.Grupocomb;
                            //if (grupocomb == "R500" || grupocomb == "R6")//Verificacion de unidades de Residual
                            //{ }
                        }

                        //if (fenergcodi == 4 && grupocomb == null) //Verificacion de unidades de Residual
                        //{ }

                        var obj48Fenerg = listaMedicionHOPEquipo.Find(x => x.Fenergcodi == fenergcodi);
                        var objFenerg = listaFuenteEnergia.Find(x => x.Fenergcodi == fenergcodi);
                        var objTgener = listaTipoGeneracion.Find(x => x.Tgenercodi == objFenerg.Tgenercodi);

                        if (obj48Fenerg == null)
                        {
                            MeMedicion48DTO reg = new MeMedicion48DTO();
                            reg.Medifecha = m48.Medifecha;
                            reg.Lectcodi = m48.Lectcodi;
                            reg.Tipoinfocodi = m48.Tipoinfocodi;
                            reg.Tipoptomedicodi = m48.Tipoptomedicodi;
                            reg.Fenergcodi = objFenerg.Fenergcodi;
                            reg.Fenergnomb = objFenerg.Fenergnomb;
                            reg.Fenercolor = objFenerg.Fenergcolor;
                            reg.Grupocomb = grupocomb;
                            reg.Tgenercodi = objTgener != null ? objTgener.Tgenercodi : 0;
                            reg.Tgenernomb = objTgener != null ? objTgener.Tgenernomb : "";
                            reg.Emprnomb = m48.Emprnomb;
                            reg.Emprcodi = m48.Emprcodi;
                            reg.Central = m48.Central;
                            reg.Equipadre = m48.Equipadre;
                            reg.Equinomb = m48.Equinomb;
                            reg.Equicodi = m48.Equicodi;
                            reg.Grupocodi = m48.Grupocodi;
                            reg.Ptomedicodi = m48.Ptomedicodi;
                            reg.Tipogrupocodi = m48.Tipogrupocodi;
                            reg.Tipogenerrer = m48.Tipogenerrer;
                            reg.Grupointegrante = m48.Grupointegrante;
                            reg.Grupotipocogen = m48.Grupotipocogen;
                            reg.Osinergcodi = m48.Osinergcodi;

                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(reg, valor);

                            listaMedicionHOPEquipo.Add(reg);
                        }
                        else
                        {
                            obj48Fenerg.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).SetValue(obj48Fenerg, valor);
                        }
                    }

                    //Generar registros pero diferenciados por tipo recurso
                    var listaTipoRecXEquipo = listaMedicionHOPEquipo.Select(x => new { x.Fenergcodi, x.Fenergnomb }).Distinct().OrderBy(x => x.Fenergcodi);

                    foreach (var tipoRec in listaTipoRecXEquipo)
                    {
                        List<MeMedicion48DTO> listaTmp = listaMedicionHOPEquipo.Where(x => x.Fenergcodi == tipoRec.Fenergcodi).ToList();
                        MeMedicion48DTO regPrimer = listaTmp.First();
                        MeMedicion48DTO reg = new MeMedicion48DTO();
                        reg.Medifecha = regPrimer.Medifecha;
                        reg.Lectcodi = regPrimer.Lectcodi;
                        reg.Tipoinfocodi = regPrimer.Tipoinfocodi;
                        reg.Tipoptomedicodi = regPrimer.Tipoptomedicodi;
                        reg.Ptomedicodi = regPrimer.Ptomedicodi;
                        reg.Fenergcodi = regPrimer.Fenergcodi;
                        reg.Fenergnomb = regPrimer.Fenergnomb;
                        reg.Fenercolor = regPrimer.Fenercolor;
                        reg.Grupocomb = regPrimer.Grupocomb;
                        reg.Tgenercodi = regPrimer.Tgenercodi;
                        reg.Tgenernomb = regPrimer.Tgenernomb;
                        reg.Emprnomb = regPrimer.Emprnomb;
                        reg.Emprcodi = regPrimer.Emprcodi;
                        reg.Central = regPrimer.Central;
                        reg.Equipadre = regPrimer.Equipadre;
                        reg.Equinomb = regPrimer.Equinomb;
                        reg.Equicodi = regPrimer.Equicodi;
                        reg.Grupocodi = m48.Grupocodi;
                        reg.Tipogrupocodi = regPrimer.Tipogrupocodi;
                        reg.Tipogenerrer = regPrimer.Tipogenerrer;
                        reg.Grupointegrante = regPrimer.Grupointegrante;
                        reg.Grupotipocogen = regPrimer.Grupotipocogen;
                        reg.Osinergcodi = regPrimer.Osinergcodi;

                        foreach (var regtmp in listaTmp)
                        {
                            for (int i = 1; i <= 48; i++)
                            {
                                decimal? valorNuevo = (decimal?)regtmp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(regtmp, null);
                                decimal? valorAcum = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(reg, null);
                                if (valorNuevo != null)
                                {
                                    valorAcum = valorAcum.GetValueOrDefault(0) + valorNuevo;
                                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).SetValue(reg, valorAcum);
                                }
                            }
                        }

                        //Totalizado
                        decimal? totalXTmp = 0;
                        for (int i = 1; i <= 48; i++)
                        {
                            decimal? valorAcum = (decimal?)reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i)).GetValue(reg, null);
                            totalXTmp += valorAcum.GetValueOrDefault(0);
                        }
                        reg.Meditotal = totalXTmp;
                        reg.MensajeValidacion = msjValidacionHO;

                        listaMedicionHOP.Add(reg);
                    }
                }
            }

            return listaMedicionHOP;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <param name="listCuadros"></param>
        /// <param name="datosReporte"></param>
        /// <param name="reporteFE"></param>
        /// <param name="reporteEmpresas"></param>
        /// <param name="reporteTG"></param>
        public void ObtenerReporteDespacho(DateTime fechaInicial, DateTime fechaFinal, string tiposEmpresa, string empresas,
                   string tiposGeneracion, string fuentesEnergia, int central, out List<MedicionReporteDTO> listCuadros,
                   out MedicionReporteDTO datosReporte, out List<MedicionReporteDTO> reporteFE, out List<MeMedicion48DTO> reporteEmpresas, out List<MedicionReporteDTO> reporteTG, int lectcodi)
        {
            if (tiposEmpresa != ConstantesAppServicio.ParametroDefecto && empresas == ConstantesAppServicio.ParametroDefecto)
            {
                List<int> idsEmpresas = this.ObteneEmpresasPorTipo(tiposEmpresa).Select(x => x.Emprcodi).ToList();
                empresas = string.Join<int>(ConstantesAppServicio.CaracterComa.ToString(), idsEmpresas);
            }

            List<MeMedicion48DTO> list = this.ListaDataMDGeneracionConsolidado48(fechaInicial, fechaFinal, central, tiposGeneracion, empresas, ConstanteValidacion.EstadoTodos, fuentesEnergia, false, lectcodi);

            //Data Generación
            List<MeMedicion48DTO> listaDemandaGen = this.ListaDataMDGeneracionFromConsolidado48(fechaInicial, fechaFinal, list);

            //Data Interconexion
            List<MeMedicion48DTO> listaInterconexion = this.ListaDataMDInterconexion48(fechaInicial, fechaFinal);

            //Data Total
            List<MeMedicion48DTO> listDemanda = this.ListaDataMDTotalSEIN48(listaDemandaGen, listaInterconexion);
            MedicionReporteDTO umbrales = this.ObtenerParametros(listDemanda, listaInterconexion);

            datosReporte = umbrales;

            var listEmpresas = list.Select(x => new { x.Emprcodi, x.Emprnomb }).Distinct().OrderBy(x => x.Emprnomb).ToList();
            var listCentrales = list.Select(x => new { x.Emprcodi, x.Central }).Distinct().ToList();
            var listEquipos = list.Select(x => new
            {
                x.Emprcodi,
                x.Central,
                x.Equicodi,
                x.Equinomb,
                x.Tgenernomb,
                x.Tgenercodi,
                x.Fenergcodi,
                x.Fenergnomb
            }).Distinct().ToList();

            #region Armado de cuadros

            List<MedicionReporteDTO> resultado = new List<MedicionReporteDTO>();

            int i = 1;
            decimal sumTotal = 0;
            decimal maxTotal = 0;
            decimal minTotal = 0;

            decimal solarTotal = 0;
            decimal eolicaTotal = 0;
            decimal hidraulicaTotal = 0;
            decimal termicoTotal = 0;

            foreach (var itemEmpresa in listEmpresas)
            {
                var subCentral = listCentrales.Where(x => x.Emprcodi == itemEmpresa.Emprcodi).ToList();

                decimal sumEmpresa = 0;
                decimal maxEmpresa = 0;
                decimal minEmpresa = 0;

                decimal solarEmpresa = 0;
                decimal eolicaEmpresa = 0;
                decimal hidraulicaEmpresa = 0;
                decimal termicoEmpresa = 0;

                foreach (var itemCentral in subCentral)
                {

                    var subEquipo =
                        listEquipos.Where(x => x.Central == itemCentral.Central && x.Emprcodi == itemCentral.Emprcodi).ToList();


                    foreach (var itemEquipo in subEquipo)
                    {
                        var listaTipoRecXEquipo = list.Where(x => x.Equicodi == itemEquipo.Equicodi).Select(x => new { x.Fenergcodi, x.Fenergnomb }).Distinct().OrderBy(x => x.Fenergcodi);

                        foreach (var tipoRec in listaTipoRecXEquipo)
                        {
                            MedicionReporteDTO resultadoEquipo = new MedicionReporteDTO();

                            resultadoEquipo.NroItem = i;
                            resultadoEquipo.Emprnomb = itemEmpresa.Emprnomb;
                            resultadoEquipo.Central = itemCentral.Central;
                            resultadoEquipo.Unidad = itemEquipo.Equinomb;
                            resultadoEquipo.Fenergcodi = tipoRec.Fenergcodi;
                            resultadoEquipo.Fenergnomb = tipoRec.Fenergnomb;
                            resultadoEquipo.Tgenercodi = itemEquipo.Tgenercodi;
                            resultadoEquipo.Tgenernomb = itemEquipo.Tgenernomb;
                            resultadoEquipo.IndicadorTotal = false;
                            resultadoEquipo.IndicadorTotalGeneral = false;

                            MeMedicion48DTO unidadMaxima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Fenergcodi == tipoRec.Fenergcodi && x.Medifecha == umbrales.FechaMaximaDemanda).FirstOrDefault();
                            MeMedicion48DTO unidadMinima = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Fenergcodi == tipoRec.Fenergcodi && x.Medifecha == umbrales.FechaMinimaDemanda).FirstOrDefault();

                            decimal energiaUnidad = 0;
                            decimal maximaUnidad = 0;
                            decimal minimaUnidad = 0;

                            object subTotal = list.Where(x => x.Equicodi == itemEquipo.Equicodi && x.Fenergcodi == tipoRec.Fenergcodi && x.Emprcodi == itemEquipo.Emprcodi).Sum(x => x.Meditotal);
                            if (subTotal != null)
                            {
                                energiaUnidad = energiaUnidad + Convert.ToDecimal(subTotal);
                            }

                            maximaUnidad = unidadMaxima != null ? ((decimal?)unidadMaxima.GetType().GetProperty(ConstantesAppServicio.CaracterH + umbrales.HoraMaximaDemanda).GetValue(unidadMaxima, null)).GetValueOrDefault(0) : 0;
                            minimaUnidad = unidadMinima != null ? ((decimal?)unidadMinima.GetType().GetProperty(ConstantesAppServicio.CaracterH + umbrales.HoraMinimaDemanda).GetValue(unidadMinima, null)).GetValueOrDefault(0) : 0;

                            resultadoEquipo.Total = (energiaUnidad != 0) ? energiaUnidad / 4 : 0;
                            resultadoEquipo.MaximaDemanda = maximaUnidad;
                            resultadoEquipo.MinimaDemanda = minimaUnidad;

                            sumEmpresa = sumEmpresa + resultadoEquipo.Total;
                            maxEmpresa = maxEmpresa + resultadoEquipo.MaximaDemanda;
                            minEmpresa = minEmpresa + resultadoEquipo.MinimaDemanda;

                            if (itemEquipo.Tgenercodi == 1)
                            {
                                resultadoEquipo.Hidraulico = resultadoEquipo.Total;
                                hidraulicaEmpresa = hidraulicaEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 2)
                            {
                                resultadoEquipo.Termico = resultadoEquipo.Total;
                                termicoEmpresa = termicoEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 3)
                            {
                                resultadoEquipo.Solar = resultadoEquipo.Total;
                                solarEmpresa = solarEmpresa + resultadoEquipo.Total;
                            }
                            else if (itemEquipo.Tgenercodi == 4)
                            {
                                resultadoEquipo.Eolico = resultadoEquipo.Total;
                                eolicaEmpresa = eolicaEmpresa + resultadoEquipo.Total;
                            }

                            resultado.Add(resultadoEquipo);
                            i++;
                        }
                    }
                }

                sumTotal = sumTotal + sumEmpresa;
                maxTotal = maxTotal + maxEmpresa;
                minTotal = minTotal + minEmpresa;

                solarTotal = solarTotal + solarEmpresa;
                termicoTotal = termicoTotal + termicoEmpresa;
                hidraulicaTotal = hidraulicaTotal + hidraulicaEmpresa;
                eolicaTotal = eolicaTotal + eolicaEmpresa;


                MedicionReporteDTO resultadoEmpresa = new MedicionReporteDTO();
                resultadoEmpresa.IndicadorTotal = true;
                resultadoEmpresa.Emprnomb = itemEmpresa.Emprnomb;
                resultadoEmpresa.Total = sumEmpresa;
                resultadoEmpresa.MaximaDemanda = maxEmpresa;
                resultadoEmpresa.MinimaDemanda = minEmpresa;
                resultadoEmpresa.Solar = solarEmpresa;
                resultadoEmpresa.Eolico = eolicaEmpresa;
                resultadoEmpresa.Termico = termicoEmpresa;
                resultadoEmpresa.Hidraulico = hidraulicaEmpresa;
                resultado.Add(resultadoEmpresa);
            }

            resultado = resultado.OrderBy(x => x.Emprnomb).ThenBy(x => x.IndicadorTotal).ThenBy(x => x.Central).ThenBy(x => x.Unidad).ThenBy(x => x.Fenergnomb).ToList();

            MedicionReporteDTO resultadoTotal = new MedicionReporteDTO();
            resultadoTotal.IndicadorTotalGeneral = true;
            resultadoTotal.Total = sumTotal;
            resultadoTotal.MaximaDemanda = maxTotal;
            resultadoTotal.MinimaDemanda = minTotal;
            resultadoTotal.Solar = solarTotal;
            resultadoTotal.Hidraulico = hidraulicaTotal;
            resultadoTotal.Termico = termicoTotal;
            resultadoTotal.Eolico = eolicaTotal;
            resultado.Add(resultadoTotal);

            listCuadros = resultado;

            #endregion

            List<MeMedicion48DTO> listTipoRecurso = list.Where(x => x.Medifecha == umbrales.FechaMaximaDemanda).ToList();
            int indice = umbrales.HoraMaximaDemanda;

            List<MeMedicion48DTO> listFuenteGeneracion = new List<MeMedicion48DTO>();

            #region Fuente Generacion

            listFuenteGeneracion = (from t in listTipoRecurso
                                    group t by new { t.Fenergcodi, t.Fenergnomb }
                                        into destino
                                    select new MeMedicion48DTO()
                                    {
                                        Fenergcodi = destino.Key.Fenergcodi,
                                        Fenergnomb = destino.Key.Fenergnomb,
                                        H1 = destino.Sum(t => t.H1),
                                        H2 = destino.Sum(t => t.H2),
                                        H3 = destino.Sum(t => t.H3),
                                        H4 = destino.Sum(t => t.H4),
                                        H5 = destino.Sum(t => t.H5),
                                        H6 = destino.Sum(t => t.H6),
                                        H7 = destino.Sum(t => t.H7),
                                        H8 = destino.Sum(t => t.H8),
                                        H9 = destino.Sum(t => t.H9),
                                        H10 = destino.Sum(t => t.H10),

                                        H11 = destino.Sum(t => t.H11),
                                        H12 = destino.Sum(t => t.H12),
                                        H13 = destino.Sum(t => t.H13),
                                        H14 = destino.Sum(t => t.H14),
                                        H15 = destino.Sum(t => t.H15),
                                        H16 = destino.Sum(t => t.H16),
                                        H17 = destino.Sum(t => t.H17),
                                        H18 = destino.Sum(t => t.H18),
                                        H19 = destino.Sum(t => t.H19),
                                        H20 = destino.Sum(t => t.H20),

                                        H21 = destino.Sum(t => t.H21),
                                        H22 = destino.Sum(t => t.H22),
                                        H23 = destino.Sum(t => t.H23),
                                        H24 = destino.Sum(t => t.H24),
                                        H25 = destino.Sum(t => t.H25),
                                        H26 = destino.Sum(t => t.H26),
                                        H27 = destino.Sum(t => t.H27),
                                        H28 = destino.Sum(t => t.H28),
                                        H29 = destino.Sum(t => t.H29),
                                        H30 = destino.Sum(t => t.H30),

                                        H31 = destino.Sum(t => t.H31),
                                        H32 = destino.Sum(t => t.H32),
                                        H33 = destino.Sum(t => t.H33),
                                        H34 = destino.Sum(t => t.H34),
                                        H35 = destino.Sum(t => t.H35),
                                        H36 = destino.Sum(t => t.H36),
                                        H37 = destino.Sum(t => t.H37),
                                        H38 = destino.Sum(t => t.H38),
                                        H39 = destino.Sum(t => t.H39),
                                        H40 = destino.Sum(t => t.H40),

                                        H41 = destino.Sum(t => t.H41),
                                        H42 = destino.Sum(t => t.H42),
                                        H43 = destino.Sum(t => t.H43),
                                        H44 = destino.Sum(t => t.H44),
                                        H45 = destino.Sum(t => t.H45),
                                        H46 = destino.Sum(t => t.H46),
                                        H47 = destino.Sum(t => t.H47),
                                        H48 = destino.Sum(t => t.H48)
                                    }).ToList();

            #endregion

            List<MedicionReporteDTO> resultadoFE = new List<MedicionReporteDTO>();

            decimal totalMDFuenteEnergia = 0;
            decimal totalEnergiaFuenteEnergia = 0;

            foreach (MeMedicion48DTO item in listFuenteGeneracion)
            {
                MedicionReporteDTO itemFE = new MedicionReporteDTO();
                itemFE.Fenergnomb = item.Fenergnomb;
                itemFE.Fenergcodi = item.Fenergcodi;

                if (indice >= 1 && indice <= 48)
                {
                    object result = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + indice).GetValue(item, null);
                    itemFE.MDFuenteEnergia = (result != null) ? Convert.ToDecimal(result) : 0;
                    totalMDFuenteEnergia = totalMDFuenteEnergia + itemFE.MDFuenteEnergia;
                }

                object totFuente = list.Where(x => x.Fenergcodi == item.Fenergcodi).Sum(x => x.Meditotal);

                if (totFuente != null)
                {
                    itemFE.EnergiaFuenteEnergia = Convert.ToDecimal(totFuente) / 4.0M;
                    totalEnergiaFuenteEnergia = totalEnergiaFuenteEnergia + itemFE.EnergiaFuenteEnergia;
                }

                resultadoFE.Add(itemFE);
            }

            List<SiFuenteenergiaDTO> listTotFuente = this.ListaFuenteEnergia(0);
            List<SiFuenteenergiaDTO> listTotFuenteFalta = listTotFuente.Where(x => !listFuenteGeneracion.Any(y => y.Fenergcodi == x.Fenergcodi)).ToList();

            foreach (SiFuenteenergiaDTO item in listTotFuenteFalta)
            {
                MedicionReporteDTO itemFE = new MedicionReporteDTO();
                itemFE.Fenergnomb = item.Fenergnomb;
                itemFE.Fenergcodi = item.Fenergcodi;
                itemFE.MDFuenteEnergia = 0;
                object totFuente = list.Where(x => x.Fenergcodi == item.Fenergcodi).Sum(x => x.Meditotal);
                if (totFuente != null)
                {
                    itemFE.EnergiaFuenteEnergia = Convert.ToDecimal(totFuente) / 4.0M;
                    totalEnergiaFuenteEnergia = totalEnergiaFuenteEnergia + itemFE.EnergiaFuenteEnergia;
                }
                resultadoFE.Add(itemFE);
            }

            resultadoFE = resultadoFE.OrderBy(x => x.Fenergnomb).ToList();

            MedicionReporteDTO finalFE = new MedicionReporteDTO();
            finalFE.EnergiaFuenteEnergia = totalEnergiaFuenteEnergia;
            finalFE.MDFuenteEnergia = totalMDFuenteEnergia;
            finalFE.IndicadorTotal = true;
            resultadoFE.Add(finalFE);

            reporteFE = resultadoFE;

            List<MeMedicion48DTO> listEmpresa = new List<MeMedicion48DTO>();
            listEmpresa = (from t in list
                           group t by new { t.Emprcodi, t.Emprnomb, t.Tgenercodi, t.Tgenernomb }
                               into destino
                           select new MeMedicion48DTO()
                           {
                               Emprcodi = destino.Key.Emprcodi,
                               Emprnomb = destino.Key.Emprnomb,
                               Tgenercodi = destino.Key.Tgenercodi,
                               Tgenernomb = destino.Key.Tgenernomb,
                               Meditotal = destino.Sum(t => t.Meditotal) / 4.0M
                           }).ToList();

            reporteEmpresas = listEmpresa;

            List<MeMedicion48DTO> listTipoGeneracion = new List<MeMedicion48DTO>();

            listTipoGeneracion = (from t in listTipoRecurso
                                  group t by new { t.Tgenercodi, t.Tgenernomb }
                                      into destino
                                  select new MeMedicion48DTO()
                                  {
                                      Tgenercodi = destino.Key.Tgenercodi,
                                      Tgenernomb = destino.Key.Tgenernomb,
                                      H1 = destino.Sum(t => t.H1),
                                      H2 = destino.Sum(t => t.H2),
                                      H3 = destino.Sum(t => t.H3),
                                      H4 = destino.Sum(t => t.H4),
                                      H5 = destino.Sum(t => t.H5),
                                      H6 = destino.Sum(t => t.H6),
                                      H7 = destino.Sum(t => t.H7),
                                      H8 = destino.Sum(t => t.H8),
                                      H9 = destino.Sum(t => t.H9),
                                      H10 = destino.Sum(t => t.H10),

                                      H11 = destino.Sum(t => t.H11),
                                      H12 = destino.Sum(t => t.H12),
                                      H13 = destino.Sum(t => t.H13),
                                      H14 = destino.Sum(t => t.H14),
                                      H15 = destino.Sum(t => t.H15),
                                      H16 = destino.Sum(t => t.H16),
                                      H17 = destino.Sum(t => t.H17),
                                      H18 = destino.Sum(t => t.H18),
                                      H19 = destino.Sum(t => t.H19),
                                      H20 = destino.Sum(t => t.H20),

                                      H21 = destino.Sum(t => t.H21),
                                      H22 = destino.Sum(t => t.H22),
                                      H23 = destino.Sum(t => t.H23),
                                      H24 = destino.Sum(t => t.H24),
                                      H25 = destino.Sum(t => t.H25),
                                      H26 = destino.Sum(t => t.H26),
                                      H27 = destino.Sum(t => t.H27),
                                      H28 = destino.Sum(t => t.H28),
                                      H29 = destino.Sum(t => t.H29),
                                      H30 = destino.Sum(t => t.H30),

                                      H31 = destino.Sum(t => t.H31),
                                      H32 = destino.Sum(t => t.H32),
                                      H33 = destino.Sum(t => t.H33),
                                      H34 = destino.Sum(t => t.H34),
                                      H35 = destino.Sum(t => t.H35),
                                      H36 = destino.Sum(t => t.H36),
                                      H37 = destino.Sum(t => t.H37),
                                      H38 = destino.Sum(t => t.H38),
                                      H39 = destino.Sum(t => t.H39),
                                      H40 = destino.Sum(t => t.H40),

                                      H41 = destino.Sum(t => t.H41),
                                      H42 = destino.Sum(t => t.H42),
                                      H43 = destino.Sum(t => t.H43),
                                      H44 = destino.Sum(t => t.H44),
                                      H45 = destino.Sum(t => t.H45),
                                      H46 = destino.Sum(t => t.H46),
                                      H47 = destino.Sum(t => t.H47),
                                      H48 = destino.Sum(t => t.H48)
                                  }).ToList();


            List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

            foreach (MeMedicion48DTO item in listTipoGeneracion)
            {
                MedicionReporteDTO itemTG = new MedicionReporteDTO();
                itemTG.Tgenernomb = item.Tgenernomb;

                if (indice >= 1 && indice <= 48)
                {
                    object result = item.GetType().GetProperty(ConstantesAppServicio.CaracterH + indice).GetValue(item, null);
                    itemTG.MDFuenteEnergia = (result != null) ? Convert.ToDecimal(result) : 0;
                }

                resultadoTG.Add(itemTG);
            }


            reporteTG = resultadoTG;
        }

        /// <summary>
        /// Permite obtener la máxima y mínima demanda
        /// </summary>
        /// <param name="list"></param>
        /// <param name="listaInterconexion"></param>
        /// <returns></returns>
        public MedicionReporteDTO ObtenerParametros(List<MeMedicion48DTO> list, List<MeMedicion48DTO> listaInterconexion)
        {
            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaBloqueHorario = servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            MedicionReporteDTO entity = new MedicionReporteDTO();

            decimal max = decimal.MinValue;
            decimal min = decimal.MaxValue;
            DateTime fechaMax = DateTime.Now;
            int horaMax = 0;
            DateTime fechaMin = DateTime.Now;
            int horaMin = 0;

            MeMedicion48DTO maxima = new MeMedicion48DTO();
            int index = 0;//reg de m48 con max demanda

            foreach (MeMedicion48DTO item in list)
            {
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoMaximaTodoDia, item.Medifecha, new List<MeMedicion48DTO>() { item }, null, null,
                                                    out decimal valorMaxDia, out int hMaxDia, out DateTime fechaHoraMaxDia);

                MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoMinimaTodoDia, item.Medifecha, new List<MeMedicion48DTO>() { item }, null, null,
                                                    out decimal valorMinDia, out int hMinDia, out DateTime fechaHoraMinDia);

                if (valorMaxDia > max)
                {
                    max = valorMaxDia;
                    fechaMax = item.Medifecha;
                    horaMax = hMaxDia;
                    maxima = item;
                }

                if (valorMinDia < min)
                {
                    min = valorMinDia;
                    fechaMin = item.Medifecha;
                    horaMin = hMinDia;
                }
            }

            entity.MaximaDemanda = max;
            entity.FechaMaximaDemanda = fechaMax;
            entity.HoraMaximaDemanda = horaMax;
            entity.MinimaDemanda = min;
            entity.FechaMinimaDemanda = fechaMin;
            entity.HoraMinimaDemanda = horaMin;

            entity.MaximaDemandaHora = fechaMax.AddMinutes(horaMax * 30);
            entity.MinimaDemandaHora = fechaMin.AddMinutes(horaMin * 30);

            if (index < list.Count)
            {
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoPeriodoBloqueMinima, maxima.Medifecha, new List<MeMedicion48DTO>() { maxima }, null, listaBloqueHorario,
                                                            out decimal valorDminima, out int horaDminima, out DateTime fechaDminima);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoPeriodoBloqueMedia, maxima.Medifecha, new List<MeMedicion48DTO>() { maxima }, null, listaBloqueHorario,
                                                            out decimal valorDmedia, out int horaDmedia, out DateTime fechaDmedia);
                MedidoresHelper.ObtenerValorHXPeriodoDemandaM48(ConstantesRepMaxDemanda.TipoPeriodoBloqueMaxima, maxima.Medifecha, new List<MeMedicion48DTO>() { maxima }, null, listaBloqueHorario,
                                                            out decimal valorDmaxima, out int horaDmaxima, out DateTime fechaDaxima);

                MeMedicion48DTO dmdInterconexionDia = new MeMedicion48DTO();
                dmdInterconexionDia = listaInterconexion.Where(x => x.Medifecha.Date == fechaMax.Date).FirstOrDefault();

                entity.BloqueMaximaDemanda = valorDmaxima;
                entity.BloqueMediaDemanda = valorDmedia;
                entity.BloqueMinimaDemanda = valorDminima;
                entity.BloqueMaximaHora = horaDmaxima;
                entity.BloqueMediaHora = horaDmedia;
                entity.BloqueMinimaHora = horaDminima;
                entity.BloqueMaximaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + horaDmaxima.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                entity.BloqueMediaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + horaDmedia.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;
                entity.BloqueMinimaInterconexion = (dmdInterconexionDia != null) ? ((decimal?)dmdInterconexionDia.GetType().GetProperty("H" + horaDminima.ToString()).GetValue(dmdInterconexionDia, null)).GetValueOrDefault(0) : 0M;

                entity.HoraBloqueMaxima = fechaMax.AddMinutes(30 * horaDmaxima);
                entity.HoraBloqueMedia = fechaMax.AddMinutes(30 * horaDmedia);
                entity.HoraBloqueMinima = fechaMax.AddMinutes(30 * horaDminima);

            }

            return entity;
        }

        #endregion

        #region Maxima Demanda - Flujo Transmision

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataMDInterconexion48(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaInterconexion = this.ObtenerDataHistoricaInterconexion(fechaInicio, fechaFin);

            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var listaDay = listaInterconexion.Where(x => x.Medifecha.Date == day).ToList();

                var regDefault = listaDay.Where(x => x.Tipoptomedicodi == -1).FirstOrDefault();
                var regExpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                var regImpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();

                if (regDefault != null)
                {
                    lista.Add(regDefault);
                }
                else
                {
                    var reg = new MeMedicion48DTO();
                    reg.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                    reg.Lectcodi = ConstantesPR5ReportesServicio.LectcodiFlujoPotencia;
                    reg.Medifecha = day;

                    decimal total = 0M;
                    if (regExpMw != null && regExpMw != null)
                    {
                        for (int i = 1; i <= 48; i++)
                        {
                            var valorEpxMWh = (decimal?)regExpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regExpMw, null);
                            var valorImpMWh = (decimal?)regImpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regImpMw, null);
                            decimal valor = (decimal)(valorEpxMWh.GetValueOrDefault(0) - valorImpMWh.GetValueOrDefault(0));

                            reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                            total += valor;
                        }
                    }

                    reg.Meditotal = total;

                    lista.Add(reg);
                }
            }

            return lista;
        }

        public void ListaFlujo30minInterconexion48(int fuenteDato, DateTime fechaInicio, DateTime fechaFin, out List<MeMedicion48DTO> listaTotal,
                        out List<MeMedicion48DTO> listaTotalExp, out List<MeMedicion48DTO> listaTotalImp)
        {
            listaTotal = new List<MeMedicion48DTO>();
            listaTotalExp = new List<MeMedicion48DTO>();
            listaTotalImp = new List<MeMedicion48DTO>();

            var listaInterconexion = new List<MeMedicion48DTO>();
            switch (fuenteDato)
            {
                case ConstantesInterconexiones.FuenteTIEFlujoNewAnexoA:
                    listaInterconexion = this.ObtenerDataNuevoFlujoInterconexion(fechaInicio, fechaFin);
                    break;
                case ConstantesInterconexiones.FuenteTIEFlujoOldDesktop:
                    listaInterconexion =  this.ObtenerDataHistoricaInterconexion(fechaInicio, fechaFin);
                    break;
            }

            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var reg = new MeMedicion48DTO();
                reg.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg.Lectcodi = ConstantesPR5ReportesServicio.LectcodiFlujoPotencia;
                reg.Medifecha = day;
                listaTotal.Add(reg);

                var reg2 = new MeMedicion48DTO();
                reg2.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg2.Lectcodi = ConstantesPR5ReportesServicio.LectcodiFlujoPotencia;
                reg2.Medifecha = day;
                listaTotalExp.Add(reg2);

                var reg3 = new MeMedicion48DTO();
                reg3.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg3.Lectcodi = ConstantesPR5ReportesServicio.LectcodiFlujoPotencia;
                reg3.Medifecha = day;
                listaTotalImp.Add(reg3);

                //REPARTIR
                var listaDay = listaInterconexion.Where(x => x.Medifecha.Date == day).ToList();

                var regDefault = listaDay.Where(x => x.Tipoptomedicodi == -1).FirstOrDefault();
                var regExpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh).FirstOrDefault();
                var regImpMw = listaDay.Where(x => x.Tipoptomedicodi == ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh).FirstOrDefault();

                if (regDefault == null) regDefault = new MeMedicion48DTO();
                if (regExpMw == null) regExpMw = new MeMedicion48DTO();
                if (regImpMw == null) regImpMw = new MeMedicion48DTO();

                decimal total = 0M;
                decimal totalExp = 0M;
                decimal totalImp = 0M;
                for (int i = 1; i <= 48; i++)
                {
                    //Convertir MWh a MW  (convertir hora a media hora)
                    decimal valorEpxMW = (decimal?)regExpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regExpMw, null) ?? 0;
                    decimal valorImpMW = (decimal?)regImpMw.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regImpMw, null) ?? 0;
                    decimal valor = valorEpxMW - valorImpMW;

                    reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                    reg2.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg2, valorEpxMW);
                    reg3.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg3, valorImpMW);
                    total += valor;
                    totalExp += valorEpxMW;
                    totalImp += valorImpMW;
                }

                reg.Meditotal = total;
                reg2.Meditotal = totalExp;
                reg3.Meditotal = totalImp;
            }
        }

        /// <summary>
        /// Lista de datos de Flujo de Potencia de la Aplicativo SGOCOES Desktop
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDataHistoricaInterconexion(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaFinalTmp = new List<MeMedicion48DTO>();

            List<MeMedicion48DTO> listaOld = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(ConstantesPR5ReportesServicio.LectcodiEjecutadoPorAreaClienteDesktop.ToString(), 
                                            fechaInicio, fechaFin, ConstantesPR5ReportesServicio.PtomedicodiEcuador.ToString());

            List<MeMedicion48DTO> listaOldDay = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listDay = new List<MeMedicion48DTO>();

            for (DateTime f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                if (f.Date == (new DateTime(2019, 7, 17)).Date)
                { }
                listaOldDay = listaOld.Where(x => x.Medifecha == f).ToList();

                listaFinalTmp = new List<MeMedicion48DTO>();

                MeMedicion48DTO m = null;
                listDay = listaOldDay.Where(x => x.Tipoinfocodi == ConstantesTipoInformacion.TipoinfoMW).ToList();

                //agregar Exportacion
                MeMedicion48DTO mexp = new MeMedicion48DTO();
                mexp.Medifecha = f;
                mexp.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                mexp.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;

                MeMedicion48DTO mimp = new MeMedicion48DTO();
                mimp.Medifecha = f;
                mimp.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                mimp.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;

                foreach (var pto in listDay)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valorMW = ((decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null)).GetValueOrDefault(0);
                        decimal valorimp = 0;
                        decimal valorexp = 0;

                        if (valorMW >= 0)
                        {
                            valorexp += valorMW;
                        }
                        else
                        {
                            valorimp += valorMW * -1;
                        }

                        mimp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(mimp, valorimp);
                        mexp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(mexp, valorexp);
                    }
                }

                listaFinalTmp.Add(mexp);
                listaFinalTmp.Add(mimp);

                listaFinal.AddRange(listaFinalTmp);
            }

            return listaFinal;
        }

        /// <summary>
        /// Lista de datos de Flujo de Potencia de la Aplicativo IEOD Web
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ObtenerDataNuevoFlujoInterconexion(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> listaFinal = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaFinalTmp = new List<MeMedicion48DTO>();

            //Obtener datos de intercambios internacionales
            List<int> listaPtomedicodi = FactorySic.GetMeReporptomedRepository().GetByCriteria(ConstantesPR5ReportesServicio.ReporcodiFlujoLineaTIEAnexoA, -1).Select(x => x.Ptomedicodi).Distinct().ToList();
            List<MeMedicion48DTO> listaNuevo = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(ConstantesPR5ReportesServicio.LectcodiFlujoPotencia.ToString(),
                                        fechaInicio, fechaFin, string.Join(",", listaPtomedicodi), ConstantesTipoInformacion.TipoinfoMW.ToString());

            for (DateTime f = fechaInicio.Date; f <= fechaFin.Date; f = f.AddDays(1))
            {
                List<MeMedicion48DTO> listDay = listaNuevo.Where(x => x.Medifecha == f).ToList();

                listaFinalTmp = new List<MeMedicion48DTO>();

                MeMedicion48DTO m = null;

                //agregar Exportacion
                MeMedicion48DTO mexp = new MeMedicion48DTO();
                mexp.Medifecha = f;
                mexp.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                mexp.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiExportacionMwh;

                MeMedicion48DTO mimp = new MeMedicion48DTO();
                mimp.Medifecha = f;
                mimp.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                mimp.Tipoptomedicodi = ConstantesInterconexiones.IdTipoPtomedicodiImportacionMwh;

                foreach (var pto in listDay)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        decimal valorMW = ((decimal?)pto.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(pto, null)).GetValueOrDefault(0);
                        decimal valorimp = 0;
                        decimal valorexp = 0;

                        if (valorMW >= 0)
                        {
                            valorexp += valorMW;
                        }
                        else
                        {
                            valorimp += valorMW * -1;
                        }

                        mimp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(mimp, valorimp);
                        mexp.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(mexp, valorexp);
                    }
                }

                listaFinalTmp.Add(mexp);
                listaFinalTmp.Add(mimp);

                listaFinal.AddRange(listaFinalTmp);
            }

            return listaFinal;
        }

        /// <summary>
        /// Demanda Programada Ecuador
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListaDataDemandaProgramadaInterconexion48(DateTime fechaInicio, DateTime fechaFin)
        {
            List<MeMedicion48DTO> lista = new List<MeMedicion48DTO>();
            List<MeMedicion48DTO> listaInterconexion = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(
                ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario.ToString(), fechaInicio, fechaFin, ConstantesPR5ReportesServicio.PtomedicodiDemandaEcuador)
                .Where(x => x.Tipoinfocodi == ConstantesPR5ReportesServicio.TipoinfoMWDemanda).ToList();

            for (DateTime day = fechaInicio.Date; day <= fechaFin.Date; day = day.AddDays(1))
            {
                var regDay = listaInterconexion.Find(x => x.Medifecha.Date == day);

                var reg = new MeMedicion48DTO();
                reg.Tipoinfocodi = ConstantesTipoInformacion.TipoinfoMW;
                reg.Lectcodi = ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario;
                reg.Medifecha = day;

                decimal total = 0M;
                if (regDay != null)
                {
                    for (int i = 1; i <= 48; i++)
                    {
                        var valor = (decimal?)regDay.GetType().GetProperty(ConstantesAppServicio.CaracterH + i).GetValue(regDay, null);
                        reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + (i).ToString()).SetValue(reg, valor);
                        total += valor.GetValueOrDefault(0);
                    }
                }

                reg.Meditotal = total;

                lista.Add(reg);
            }

            return lista;
        }

        #endregion

        #region Demanda por Áreas
        
        public void ListaDemandaArea30min(int fuenteDato, DateTime fechaInicio, DateTime fechaFin, List<MeMedicion48DTO> listaM48RangoEjec, List<MeMedicion48DTO> listaM48Flujo, 
                                out List<MeMedicion48DTO> listaAreaTotal)
        {
            listaAreaTotal = new List<MeMedicion48DTO>();

            switch (fuenteDato)
            {
                case ConstantesInterconexiones.FuenteTIEFlujoNewAnexoA:
                    listaAreaTotal = this.ObtenerDataDemandaXAreaNuevoIEOD(fechaInicio, fechaFin, listaM48RangoEjec, listaM48Flujo);
                    break;
                case ConstantesInterconexiones.FuenteTIEFlujoOldDesktop:
                    listaAreaTotal = this.ObtenerDataDemandaXAreaHistorico(fechaInicio, fechaFin);
                    break;
            }
        }
         
        private List<MeMedicion48DTO> ObtenerDataDemandaXAreaNuevoIEOD(DateTime fechaIni, DateTime fechaFin, List<MeMedicion48DTO> listaM48RangoEjec, List<MeMedicion48DTO> listaM48Flujo)
        {
            FormatoReporteAppServicio servFormatoRep = new FormatoReporteAppServicio();
            List<MeReporptomedDTO> listaPtoArea = servFormatoRep.GetListaAreaOperativa();
            int ptomedicodiNorte = listaPtoArea.Find(x => x.Repptoorden == 1).Ptomedicodi;
            int ptomedicodiCentro = listaPtoArea.Find(x => x.Repptoorden == 2).Ptomedicodi;
            int ptomedicodiSur = listaPtoArea.Find(x => x.Repptoorden == 3).Ptomedicodi;

            List<MeMedicion48DTO> listaM48Rango = new List<MeMedicion48DTO>();
            listaM48Rango.AddRange(listaM48RangoEjec);
            listaM48Rango.AddRange(listaM48Flujo);

            List<MePtomedicionDTO> listaPto = new List<MePtomedicionDTO>();
            List<MeMedicion48DTO> listArea = servFormatoRep.GetListaDataM48FromMeReporptomed(ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto,
                            fechaIni, fechaFin, listaPtoArea, true, 1, new List<int> { ConstantesPR5ReportesServicio.PtomedicodiZorritos }, listaM48Rango, ref listaPto);
            listArea = ListaDataMDTotalSEINXPto48(listArea);

            //setear area operativa
            foreach (var item in listArea)
            {
                if (item.Ptomedicodi == ptomedicodiNorte) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaNorte;
                if (item.Ptomedicodi == ptomedicodiCentro) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaCentro;
                if (item.Ptomedicodi == ptomedicodiSur) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaSur;
            }

            return listArea;
        }

        private List<MeMedicion48DTO> ObtenerDataDemandaXAreaHistorico(DateTime fechaIni, DateTime fechaFin)
        {
            string ptomedicodis = string.Format("{0},{1},{2}", ConstantesPR5ReportesServicio.PtomedicodiAreaNorte, ConstantesPR5ReportesServicio.PtomedicodiAreaCentro, ConstantesPR5ReportesServicio.PtomedicodiAreaSur);

            List<MeMedicion48DTO> listaData = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro(ConstantesPR5ReportesServicio.LectcodiEjecutadoPorAreaClienteDesktop.ToString(),
                                        fechaIni, fechaFin, ptomedicodis, ConstantesTipoInformacion.TipoinfoMW.ToString());
            
            //setear area operativa
            foreach (var item in listaData)
            {
                if (item.Ptomedicodi == ConstantesPR5ReportesServicio.PtomedicodiAreaNorte) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaNorte;
                if (item.Ptomedicodi == ConstantesPR5ReportesServicio.PtomedicodiAreaCentro) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaCentro;
                if (item.Ptomedicodi == ConstantesPR5ReportesServicio.PtomedicodiAreaSur) item.AreaOperativa = ConstantesPR5ReportesServicio.AreaSur;
            }

            return listaData;
        }

        #endregion

        #region Dashboard IEOD

        /// <summary>
        /// Se obtiene los datos de produccion de la energia por tipo de generacion
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public List<MeMedicion48DTO> ListarProduccionxTipoGeneracionxFecha(DateTime fechaini, DateTime fechaFin)
        {
            List<MeMedicion48DTO> list = this.ListaDataMDGeneracionConsolidado48(fechaini, fechaFin, ConstantesMedicion.IdTipogrupoCOES, ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos, ConstantesMedicion.IdTipoRecursoTodos.ToString(), true, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);

            List<MeMedicion48DTO> listTipoGeneracion = (from t in list
                                                        group t by new { t.Medifecha, t.Tgenercodi, t.Tgenernomb }
                                                        into destino
                                                        select new MeMedicion48DTO()
                                                        {
                                                            Medifecha = destino.Key.Medifecha,
                                                            Tgenercodi = destino.Key.Tgenercodi,
                                                            Tgenernomb = destino.Key.Tgenernomb,
                                                            Meditotal = destino.Sum(t => t.Meditotal),
                                                            H1 = destino.Sum(t => t.H1),
                                                            H2 = destino.Sum(t => t.H2),
                                                            H3 = destino.Sum(t => t.H3),
                                                            H4 = destino.Sum(t => t.H4),
                                                            H5 = destino.Sum(t => t.H5),
                                                            H6 = destino.Sum(t => t.H6),
                                                            H7 = destino.Sum(t => t.H7),
                                                            H8 = destino.Sum(t => t.H8),
                                                            H9 = destino.Sum(t => t.H9),
                                                            H10 = destino.Sum(t => t.H10),

                                                            H11 = destino.Sum(t => t.H11),
                                                            H12 = destino.Sum(t => t.H12),
                                                            H13 = destino.Sum(t => t.H13),
                                                            H14 = destino.Sum(t => t.H14),
                                                            H15 = destino.Sum(t => t.H15),
                                                            H16 = destino.Sum(t => t.H16),
                                                            H17 = destino.Sum(t => t.H17),
                                                            H18 = destino.Sum(t => t.H18),
                                                            H19 = destino.Sum(t => t.H19),
                                                            H20 = destino.Sum(t => t.H20),

                                                            H21 = destino.Sum(t => t.H21),
                                                            H22 = destino.Sum(t => t.H22),
                                                            H23 = destino.Sum(t => t.H23),
                                                            H24 = destino.Sum(t => t.H24),
                                                            H25 = destino.Sum(t => t.H25),
                                                            H26 = destino.Sum(t => t.H26),
                                                            H27 = destino.Sum(t => t.H27),
                                                            H28 = destino.Sum(t => t.H28),
                                                            H29 = destino.Sum(t => t.H29),
                                                            H30 = destino.Sum(t => t.H30),

                                                            H31 = destino.Sum(t => t.H31),
                                                            H32 = destino.Sum(t => t.H32),
                                                            H33 = destino.Sum(t => t.H33),
                                                            H34 = destino.Sum(t => t.H34),
                                                            H35 = destino.Sum(t => t.H35),
                                                            H36 = destino.Sum(t => t.H36),
                                                            H37 = destino.Sum(t => t.H37),
                                                            H38 = destino.Sum(t => t.H38),
                                                            H39 = destino.Sum(t => t.H39),
                                                            H40 = destino.Sum(t => t.H40),

                                                            H41 = destino.Sum(t => t.H41),
                                                            H42 = destino.Sum(t => t.H42),
                                                            H43 = destino.Sum(t => t.H43),
                                                            H44 = destino.Sum(t => t.H44),
                                                            H45 = destino.Sum(t => t.H45),
                                                            H46 = destino.Sum(t => t.H46),
                                                            H47 = destino.Sum(t => t.H47),
                                                            H48 = destino.Sum(t => t.H48)
                                                        }).ToList();

            return listTipoGeneracion;
        }

        #endregion

        #region REQ 2023-002106 Reporte de Reserva fría en la Operación del corto y largo plazo

        /// <summary>
        /// Reporte de despacho
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="rutaArchivo"></param>
        public void GenerarReporteDespachoYReservaFria(DateTime fechaInicial, DateTime fechaFinal, string rutaArchivo)
        {
            //Obtener datos
            EjecutadoRptDespachoYRfria objRpt = ListarDataReporteDespachoYReservaFria(fechaInicial, fechaFinal);

            //Generar Excel
            FileInfo newFile = new FileInfo(rutaArchivo);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(rutaArchivo);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("DESPACHO_EJECTUDO");
                ws = xlPackage.Workbook.Worksheets["DESPACHO_EJECTUDO"];

                GeneraRptDespachoRegistradoGruposDespacho(ws, fechaInicial, fechaFinal, objRpt);

                xlPackage.Save();
            }
        }

        private EjecutadoRptDespachoYRfria ListarDataReporteDespachoYReservaFria(DateTime fechaInicial, DateTime fechaFinal)
        {
            #region Despacho

            //Data 30 minutos Ejecutado de grupos despacho de Centrales Integrantes
            List<MeMedicion48DTO> listaMe48 = ListaDataGeneracion48(fechaInicial, fechaFinal, ConstantesMedicion.IdTipogrupoTodos
                , ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesAppServicio.ParametroDefecto, ConstanteValidacion.EstadoTodos
                , ConstantesMedicion.IdTipoRecursoTodos.ToString(), false
                , ConstantesMedicion.TipoPotenciaActiva, Int32.Parse(ConstantesAppServicio.LectcodiEjecutadoHisto));

            //Lista de puntos de medición
            List<MePtomedicionDTO> listaPto = FactorySic.GetMePtomedicionRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto
                                        , ConstantesAppServicio.OriglectcodiDespachomediahora.ToString(), ConstantesAppServicio.ParametroDefecto);
            listaPto = listaPto.Where(x => x.Ptomediestado != ConstantesAppServicio.Anulado && x.Ptomedicodi > 0).OrderBy(x => x.Ptomediestado).ToList();

            //Lista de grupos
            List<PrGrupoDTO> listaGrupo = FactorySic.GetPrGrupoRepository().List();
            listaGrupo = listaGrupo.Where(x => x.GrupoEstado != ConstantesAppServicio.Anulado).ToList();
            foreach (var reg in listaGrupo)
            {
                reg.Gruponomb = (reg.Gruponomb ?? "").Trim();
                reg.Grupoabrev = (reg.Grupoabrev ?? "").Trim();
                reg.Grupoorden = reg.Grupoorden != null ? reg.Grupoorden.Value : 9999999;
            }

            //Lista de empresas
            List<SiEmpresaDTO> listaEmpresa = FactorySic.GetSiEmpresaRepository().GetByCriteria(ConstantesAppServicio.ParametroDefecto).Where(x => x.Emprcodi > 0).ToList();
            foreach (var reg in listaEmpresa)
            {
                reg.Emprorden = reg.Emprorden != null ? reg.Emprorden.Value : 9999999;
            }

            //Objetos para reporte
            List<PrGrupoDTO> listaGrupoData = new List<PrGrupoDTO>();

            List<int> lGrupocodi = listaMe48.Select(x => x.Grupocodi).Distinct().ToList();
            foreach (var grupocodi in lGrupocodi)
            {
                var objGrupo = listaGrupo.Find(x => x.Grupocodi == grupocodi);

                var objPto = listaPto.Find(x => x.Grupocodi == grupocodi);
                objGrupo.Ptomedicodi = (objPto?.Ptomedicodi) ?? 0;

                var objEmp = listaEmpresa.Find(x => x.Emprcodi == objGrupo.Emprcodi);
                objGrupo.Emprnomb = objEmp.Emprnomb;
                objGrupo.Emprorden = objEmp.Emprorden;

                //total
                objGrupo.Potencia = listaMe48.Where(x => x.Ptomedicodi == objGrupo.Ptomedicodi).Sum(x => x.Meditotal ?? 0) / 2.0m;

                listaGrupoData.Add(objGrupo);
            }
            listaGrupoData = listaGrupoData.OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ThenBy(x => x.Grupoorden).ThenBy(x => x.Grupoorden).ToList();

            List<SiEmpresaDTO> listaEmpresaData = new List<SiEmpresaDTO>();
            List<int> lEmprcodi = listaGrupoData.Select(x => x.Emprcodi ?? 0).Distinct().ToList();
            foreach (var emprcodi in lEmprcodi)
            {
                var objEmp = listaEmpresa.Find(x => x.Emprcodi == emprcodi);
                listaEmpresaData.Add(objEmp);
            }

            listaEmpresaData = listaEmpresaData.OrderBy(x => x.Emprorden).ThenBy(x => x.Emprnomb).ToList();

            //Totalizar
            List<MeMedicion48DTO> listaMe48TotalXDia = new List<MeMedicion48DTO>();
            decimal? valor;
            decimal total;
            for (var day = fechaInicial.Date; day.Date <= fechaFinal.Date; day = day.AddDays(1))
            {
                List<MeMedicion48DTO> listaMe48XDia = listaMe48.Where(x => x.Medifecha == day).ToList();
                MeMedicion48DTO objTotal = new MeMedicion48DTO();
                objTotal.Medifecha = day;

                for (int h = 1; h <= 48; h++)
                {
                    total = ((decimal?)objTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objTotal, null)).GetValueOrDefault(0);
                    foreach (var m48 in listaMe48XDia)
                    {
                        valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                        total += valor.GetValueOrDefault(0);
                    }

                    objTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + (h).ToString()).SetValue(objTotal, total);
                }

                listaMe48TotalXDia.Add(objTotal);
            }

            decimal energiaTotal = listaGrupoData.Sum(x => x.Potencia ?? 0);

            #endregion

            #region Reserva Fría

            string ptomedicodisRfria = "3200,3201,3202,3203";
            List<MeMedicion48DTO> listaMe48Rfria = FactorySic.GetMeMedicion48Repository().ListarMeMedicion48ByFiltro("29", fechaInicial, fechaFinal, ptomedicodisRfria)
                                    .Where(x => x.Tipoinfocodi == ConstantesMedicion.TipoPotenciaActiva).ToList();

            List<MePtomedicionDTO> listaPtoRfria = FactorySic.GetMePtomedicionRepository().List(ptomedicodisRfria, ConstantesAppServicio.ParametroDefecto);

            List<PrGrupoDTO> listaGrupoRfria = listaPtoRfria.Select(x => new PrGrupoDTO()
            {
                Grupocodi = 0,
                Ptomedicodi = x.Ptomedicodi,
                Gruponomb = x.Ptomedielenomb
            }).ToList();
            foreach (var objGrupo in listaGrupoRfria)
            {
                //total
                objGrupo.Potencia = listaMe48Rfria.Where(x => x.Ptomedicodi == objGrupo.Ptomedicodi).Sum(x => x.Meditotal ?? 0) / 2.0m;
            }

            #endregion

       
            //Salidas
            EjecutadoRptDespachoYRfria objRpt = new EjecutadoRptDespachoYRfria()
            {
                ListaMe48 = listaMe48,
                ListaGrupo = listaGrupoData,
                ListaEmpresa = listaEmpresaData,
                ListaMe48TotalXDia = listaMe48TotalXDia,
                EnergiaTotal = energiaTotal,
                ListaMe48Rfria = listaMe48Rfria,
                ListaGrupoRfria = listaGrupoRfria
            };

            return objRpt;
        }

        private void GeneraRptDespachoRegistradoGruposDespacho(ExcelWorksheet ws, DateTime fechaInicial, DateTime fechaFinal, EjecutadoRptDespachoYRfria objRpt)
        {
            int row = 4;
            int col = 1;

            #region Despacho - Cabecera 

            int rowIniPto = 1;

            int colIniTitulo = col;
            int rowIniTitulo = row;
            int colFinTitulo = col + 2 + objRpt.ListaGrupo.Count + 1 - 1;
            ws.Cells[rowIniTitulo, colIniTitulo].Value = "POTENCIA ACTIVA EJECUTADA DE LAS UNIDADES DE GENERACIÓN DEL SEIN (MW)";
            UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo, "Centro", "Izquierda", "#FFFFFF", "#528DD5", "Arial", 18, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colIniTitulo, rowIniTitulo, colFinTitulo, "#FFFFFF");

            int colIniFecha = col;
            int rowIniFecha = rowIniTitulo + 1;
            int rowFinFecha = rowIniFecha + 2 - 1;
            ws.Cells[rowIniFecha, colIniFecha].Value = "FECHA";
            UtilExcel.SetFormatoCelda(ws, rowIniFecha, colIniFecha, rowFinFecha, colIniFecha, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniFecha, colIniFecha, rowFinFecha, colIniFecha);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniFecha, colIniFecha, rowFinFecha, colIniFecha, "#FFFFFF");

            int colIniHora = colIniFecha + 1;
            int rowIniHora = rowIniFecha;
            int rowFinHora = rowFinFecha;
            ws.Cells[rowIniHora, colIniHora].Value = "HORA";
            UtilExcel.SetFormatoCelda(ws, rowIniHora, colIniHora, rowFinHora, colIniHora, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniHora, colIniHora, rowFinHora, colIniHora);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniHora, colIniHora, rowFinHora, colIniHora, "#FFFFFF");

            int rowIniEmp = rowIniFecha;
            int colIniEmp = colIniHora + 1;
            int colFinEmp = colIniEmp;

            int rowIniGrupoDespacho = rowIniEmp + 1;
            int colIniGrupoDespacho = colIniHora + 1;
            int colFinGrupoDespacho = colIniGrupoDespacho;

            int totalDia = (int)(fechaFinal - fechaInicial).TotalDays + 1;
            int rowIniData = rowIniGrupoDespacho + 1;
            int rowFinData = rowIniData + totalDia * 48 - 1;
            int rowEjec = rowFinData + 1;

            for (int i = 0; i < objRpt.ListaEmpresa.Count; i++)
            {
                //Empresa
                var objEmp = objRpt.ListaEmpresa[i];
                int totalGrXemp = objRpt.ListaGrupo.Where(x => x.Emprcodi == objEmp.Emprcodi).Count();

                colFinEmp = colIniEmp + totalGrXemp - 1;
                ws.Cells[rowIniEmp, colIniEmp].Value = objEmp.Emprnomb.Trim();
                UtilExcel.SetFormatoCelda(ws, rowIniEmp, colIniEmp, rowIniEmp, colFinEmp, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 9, true, true);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniEmp, colIniEmp, rowIniEmp, colFinEmp);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIniEmp, colIniEmp, rowIniEmp, colFinEmp, "#FFFFFF");

                colIniEmp = colFinEmp + 1;

                //Grupos Despacho
                var listaGrupoDespachoXEmp = objRpt.ListaGrupo.Where(x => x.Emprcodi == objEmp.Emprcodi).ToList();

                for (int k = 0; k < listaGrupoDespachoXEmp.Count; k++)
                {
                    var objGrupoDespacho = listaGrupoDespachoXEmp[k];

                    colFinGrupoDespacho = colIniGrupoDespacho;
                    ws.Cells[rowIniGrupoDespacho, colIniGrupoDespacho].Value = objGrupoDespacho.Gruponomb;
                    UtilExcel.SetFormatoCelda(ws, rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 8, true, true);
                    UtilExcel.CeldasExcelAgrupar(ws, rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho);
                    UtilExcel.BorderCeldasLineaDelgada(ws, rowIniGrupoDespacho, colIniGrupoDespacho, rowIniGrupoDespacho, colIniGrupoDespacho, "#FFFFFF");

                    //codigo punto
                    ws.Cells[rowIniPto, colIniGrupoDespacho].Value = objGrupoDespacho.Ptomedicodi; //oculto
                    UtilExcel.CeldasExcelColorTexto(ws, rowIniPto, colIniGrupoDespacho, rowIniPto, colIniGrupoDespacho, "#FFFFFF");
                    UtilExcel.CeldasExcelColorFondo(ws, rowIniPto, colIniGrupoDespacho, rowIniPto, colIniGrupoDespacho, "#FFFFFF");

                    //MWh total por grupo
                    ws.Cells[rowEjec, colIniGrupoDespacho].Value = objGrupoDespacho.Potencia;

                    colIniGrupoDespacho = colFinGrupoDespacho + 1;
                }
            }

            int colIniTotal = colFinTitulo;
            int colFinTotal = colIniTotal;
            int rowIniTotal = rowIniEmp;
            int rowFinTotal = rowIniGrupoDespacho;

            //Descripcion Total - medida
            ws.Cells[rowIniTotal, colIniTotal].Value = "TOTAL GENERACIÓN COES";
            UtilExcel.SetFormatoCelda(ws, rowIniTotal, colIniTotal, rowIniTotal, colFinTotal, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTotal, colIniTotal, rowIniTotal, colFinTotal);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTotal, colIniTotal, rowIniTotal, colFinTotal, "#FFFFFF");

            ws.Cells[rowFinTotal, colIniTotal].Value = "MW";
            UtilExcel.SetFormatoCelda(ws, rowFinTotal, colIniTotal, rowFinTotal, colFinTotal, "Centro", "Centro", "#FFFFFF", "#528DD5", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowFinTotal, colIniTotal, rowFinTotal, colFinTotal);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowFinTotal, colIniTotal, rowFinTotal, colFinTotal, "#FFFFFF");

            #endregion

            #region Despacho - Cuerpo

            row = rowIniData;

            decimal? valor;

            int colData = colIniHora;
            for (var day = fechaInicial.Date; day.Date <= fechaFinal.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                List<MeMedicion48DTO> listaMe48XDia = objRpt.ListaMe48.Where(x => x.Medifecha == day).ToList();
                MeMedicion48DTO objTotal = objRpt.ListaMe48TotalXDia.Find(x => x.Medifecha == day);

                for (int h = 1; h <= 48; h++)
                {
                    ws.Row(row).Height = 24;

                    //Fecha
                    ws.Cells[row, colIniFecha].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);

                    colData = colIniHora;
                    //Hora
                    ws.Cells[row, colIniHora].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);

                    colData++;

                    foreach (var objGr in objRpt.ListaGrupo)
                    {
                        MeMedicion48DTO m48 = listaMe48XDia.Find(x => x.Ptomedicodi == objGr.Ptomedicodi);
                        valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                        ws.Cells[row, colData].Value = valor;
                        colData++;
                    }

                    valor = objTotal != null ? (decimal?)objTotal.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(objTotal, null) : null;
                    ws.Cells[row, colIniTotal].Value = valor;
                    colData++;


                    horas = horas.AddMinutes(30);
                    row++;
                }
            }

            ws.Cells[rowEjec, colIniFecha].Value = "EJEC";
            ws.Cells[rowEjec, colIniHora].Value = "EJEC";
            UtilExcel.SetFormatoCelda(ws, rowEjec, colIniFecha, rowEjec, colIniHora, "Centro", "Centro", "#000000", "#D6DCE4", "Arial", 12, true, true);
            UtilExcel.SetFormatoCelda(ws, rowEjec, colIniHora + 1, rowEjec, colIniTotal, "Centro", "Centro", "#000000", "#D6DCE4", "Arial", 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEjec, colIniFecha, rowEjec, colIniTotal, "#4472C4", true);
            ws.Cells[rowEjec, colIniTotal].Value = objRpt.EnergiaTotal;

            //Formatear data
            int colFinData = colIniTotal;
            using (var range = ws.Cells[rowIniData, colIniHora + 1, rowFinData + 1, colFinData])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Numberformat.Format = "#,##0.00";
                range.Style.Font.Size = 10;
            }

            //mostrar lineas horas
            for (int c = colIniFecha; c <= colFinData; c++)
            {
                for (int f = rowIniData; f <= rowFinData; f += 8)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                    ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                }
            }

            UtilExcel.CeldasExcelEnNegrita(ws, rowIniData, colIniTotal, rowFinData, colIniTotal);

            //Formato de Filas y columnas
            for (int columna = colIniHora + 1; columna <= colFinData; columna++)
                ws.Column(columna).Width = 20;

            ws.Column(colIniFecha).Width = 12;
            ws.Column(colIniHora).Width = 10;
            ws.Row(rowIniEmp).Height = 40;
            ws.Row(rowIniGrupoDespacho).Height = 40;
            ws.Row(rowEjec).Height = 25;

            #endregion

            #region Reserva fría - Cabecera 

            int colIniTituloRfria = colIniTotal + 2;
            int colFinTituloRfria = colIniTituloRfria + 2 + objRpt.ListaGrupoRfria.Count - 1;
            ws.Cells[rowIniTitulo, colIniTituloRfria].Value = "RESERVA FRÍA";
            UtilExcel.SetFormatoCelda(ws, rowIniTitulo, colIniTituloRfria, rowIniTitulo, colFinTituloRfria, "Centro", "Izquierda", "#FFFFFF", "#8497B0", "Arial", 18, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniTitulo, colIniTituloRfria, rowIniTitulo, colFinTituloRfria);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniTitulo, colIniTituloRfria, rowIniTitulo, colFinTituloRfria, "#FFFFFF");

            int colIniFechaRfria = colIniTotal + 2;
            ws.Cells[rowIniFecha, colIniFechaRfria].Value = "FECHA";
            UtilExcel.SetFormatoCelda(ws, rowIniFecha, colIniFechaRfria, rowFinFecha, colIniFechaRfria, "Centro", "Centro", "#FFFFFF", "#8497B0", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniFecha, colIniFechaRfria, rowFinFecha, colIniFechaRfria);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniFecha, colIniFechaRfria, rowFinFecha, colIniFechaRfria, "#FFFFFF");

            int colIniHoraRfria = colIniFechaRfria + 1;
            ws.Cells[rowIniHora, colIniHoraRfria].Value = "HORA";
            UtilExcel.SetFormatoCelda(ws, rowIniHora, colIniHoraRfria, rowFinHora, colIniHoraRfria, "Centro", "Centro", "#FFFFFF", "#8497B0", "Arial", 8, true, true);
            UtilExcel.CeldasExcelAgrupar(ws, rowIniHora, colIniHoraRfria, rowFinHora, colIniHoraRfria);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowIniHora, colIniHoraRfria, rowFinHora, colIniHoraRfria, "#FFFFFF");

            int rowIniRfria = rowIniFecha;
            int rowFinRfria = rowIniRfria + 1;
            int colIniRfria = colIniHoraRfria + 1;
            int colFinRfria = colIniRfria;

            for (int k = 0; k < objRpt.ListaGrupoRfria.Count; k++)
            {
                var objGrupoRfria = objRpt.ListaGrupoRfria[k];

                ws.Cells[rowIniRfria, colFinRfria].Value = objGrupoRfria.Gruponomb;
                UtilExcel.SetFormatoCelda(ws, rowIniRfria, colFinRfria, rowFinRfria, colFinRfria, "Centro", "Centro", "#FFFFFF", "#8497B0", "Arial", 9, true, true);
                UtilExcel.CeldasExcelAgrupar(ws, rowIniRfria, colFinRfria, rowFinRfria, colFinRfria);
                UtilExcel.BorderCeldasLineaDelgada(ws, rowIniRfria, colFinRfria, rowFinRfria, colFinRfria, "#FFFFFF");

                //codigo punto
                ws.Cells[rowIniPto, colFinRfria].Value = objGrupoRfria.Ptomedicodi; //oculto
                UtilExcel.CeldasExcelColorTexto(ws, rowIniPto, colFinRfria, rowIniPto, colFinRfria, "#FFFFFF");
                UtilExcel.CeldasExcelColorFondo(ws, rowIniPto, colFinRfria, rowIniPto, colFinRfria, "#FFFFFF");

                //MWh total por grupo
                ws.Cells[rowEjec, colFinRfria].Value = objGrupoRfria.Potencia;

                colFinRfria = colFinRfria + 1;
            }

            #endregion

            #region Reserva fría - Cuerpo

            row = rowIniData;

            colData = colIniHoraRfria;
            for (var day = fechaInicial.Date; day.Date <= fechaFinal.Date; day = day.AddDays(1))
            {
                //HORA
                DateTime horas = day.AddMinutes(30);

                List<MeMedicion48DTO> listaMe48XDia = objRpt.ListaMe48Rfria.Where(x => x.Medifecha == day).ToList();

                for (int h = 1; h <= 48; h++)
                {
                    ws.Row(row).Height = 24;

                    //Fecha
                    ws.Cells[row, colIniFechaRfria].Value = horas.ToString(ConstantesAppServicio.FormatoFecha);

                    colData = colIniHoraRfria;
                    //Hora
                    ws.Cells[row, colIniHoraRfria].Value = horas.ToString(ConstantesAppServicio.FormatoOnlyHora);

                    colData++;

                    foreach (var objGr in objRpt.ListaGrupoRfria)
                    {
                        MeMedicion48DTO m48 = listaMe48XDia.Find(x => x.Ptomedicodi == objGr.Ptomedicodi);
                        valor = m48 != null ? (decimal?)m48.GetType().GetProperty(ConstantesAppServicio.CaracterH + h).GetValue(m48, null) : null;
                        ws.Cells[row, colData].Value = valor;
                        colData++;
                    }

                    horas = horas.AddMinutes(30);
                    row++;
                }
            }

            ws.Cells[rowEjec, colIniFechaRfria].Value = "EJEC";
            ws.Cells[rowEjec, colIniHoraRfria].Value = "EJEC";
            UtilExcel.SetFormatoCelda(ws, rowEjec, colIniFechaRfria, rowEjec, colIniHoraRfria, "Centro", "Centro", "#000000", "#D6DCE4", "Arial", 12, true, true);
            UtilExcel.SetFormatoCelda(ws, rowEjec, colIniHoraRfria + 1, rowEjec, colFinTituloRfria, "Centro", "Centro", "#000000", "#D6DCE4", "Arial", 12, true);
            UtilExcel.BorderCeldasLineaDelgada(ws, rowEjec, colIniFechaRfria, rowEjec, colFinTituloRfria, "#4472C4", true);

            //Formatear data
            colFinData = colIniTotal;
            using (var range = ws.Cells[rowIniData, colIniHoraRfria + 1, rowFinData + 1, colFinTituloRfria])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.Numberformat.Format = "#,##0.00";
                range.Style.Font.Size = 10;
            }

            //mostrar lineas horas
            for (int c = colIniFechaRfria; c <= colFinTituloRfria; c++)
            {
                for (int f = rowIniData; f <= rowFinData; f += 8)
                {
                    ws.Cells[f, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c].Style.Border.Top.Color.SetColor(Color.Blue);

                    ws.Cells[f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f + 8 - 1, c].Style.Border.Bottom.Color.SetColor(Color.Blue);

                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Fill.BackgroundColor.SetColor(Color.White);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Left.Color.SetColor(Color.Blue);
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    ws.Cells[f, c, f + 8 - 1, c].Style.Border.Right.Color.SetColor(Color.Blue);
                }
            }

            //Formato de Filas y columnas
            for (int columna = colIniHoraRfria + 1; columna <= colFinTituloRfria; columna++)
                ws.Column(columna).Width = 20;

            ws.Column(colIniFechaRfria).Width = 12;
            ws.Column(colIniHoraRfria).Width = 10;

            #endregion

            ws.View.FreezePanes(rowFinFecha + 1, colIniHora + 1);
            ws.View.ZoomScale = 70;

            //Todo el excel con Font Arial
            var allCells = ws.Cells[1, 1, ws.Dimension.End.Row, ws.Dimension.End.Column];
            var cellFont = allCells.Style.Font;
            cellFont.Name = "Arial";

            //No mostrar lineas
            ws.View.ShowGridLines = false;
        }


        #endregion

        #region Informes SGI (Carga masiva despacho resumido)

        public void GuardarDespachoDiarioProdGen(DateTime fechaIni, DateTime fechaFin, int tipoDespachoEjec, int tipoDespachoProg)
        {
            //Insumo despacho ejecutado (con cruce de horas de operación)
            List<MeMedicion48DTO> listaM48RangoEjec = ListaDataMDGeneracionConsolidado48(fechaIni, fechaFin, ConstantesMedicion.IdTipogrupoCOES,
                        ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos,
                        ConstantesMedicion.IdTipoRecursoTodos.ToString(), true, ConstantesPR5ReportesServicio.LectDespachoEjecutadoHisto);
            List<MeMedicion48DTO> listaDemandaGen48XDiaEjec = ListaDataMDGeneracionFromConsolidado48(fechaIni, fechaFin, listaM48RangoEjec);

            //Insumo despacho programado (sin cruce de horas de operación)
            List<MeMedicion48DTO> listaM48RangoProg = ListaDataMDGeneracionConsolidado48(fechaIni, fechaFin, ConstantesMedicion.IdTipogrupoCOES,
                        ConstantesMedicion.IdTipoGeneracionTodos.ToString(), ConstantesMedicion.IdEmpresaTodos.ToString(), ConstanteValidacion.EstadoTodos,
                        ConstantesMedicion.IdTipoRecursoTodos.ToString(), false, ConstantesPR5ReportesServicio.LectDespachoProgramadoDiario);
            List<MeMedicion48DTO> listaDemandaGen48XDiaProg = ListaDataMDGeneracionFromConsolidado48(fechaIni, fechaFin, listaM48RangoProg);

            //Obtener detalle del despacho
            List<MeDespachoProdgenDTO> listaDespachoEjecDiaXGrupo = listaM48RangoEjec
                        .GroupBy(x => new { x.Medifecha, x.Grupocodi, x.Fenergcodi })
                        .Select(x => new MeDespachoProdgenDTO()
                        {
                            Dpgentipo = tipoDespachoEjec,
                            Dpgenfecha = x.Key.Medifecha,
                            Dpgenvalor = x.Sum(y => y.Meditotal ?? 0),
                            Dpgenintegrante = x.First().Grupointegrante,
                            Dpgenrer = x.First().Tipogenerrer,
                            Emprcodi = x.First().Emprcodi,
                            Equipadre = x.First().Equipadre,
                            Grupocodi = x.Key.Grupocodi,
                            Fenergcodi = x.Key.Fenergcodi,
                            Tgenercodi = x.First().Tgenercodi,
                            Ctgdetcodi = x.First().Ctgdetcodi > 0 ? (int?)x.First().Ctgdetcodi : null
                        }).ToList();

            List<MeDespachoProdgenDTO> listaDespachoProgDiaXGrupo = new List<MeDespachoProdgenDTO>();
            if (tipoDespachoProg > 0)
            {
                listaDespachoProgDiaXGrupo = listaM48RangoProg
                            .GroupBy(x => new { x.Medifecha, x.Grupocodi, x.Tgenercodi })
                            .Select(x => new MeDespachoProdgenDTO()
                            {
                                Dpgentipo = tipoDespachoProg,
                                Dpgenfecha = x.Key.Medifecha,
                                Dpgenvalor = x.Sum(y => y.Meditotal ?? 0),
                                Dpgenintegrante = x.First().Grupointegrante,
                                Dpgenrer = x.First().Tipogenerrer,
                                Emprcodi = x.First().Emprcodi,
                                Equipadre = x.First().Equipadre,
                                Grupocodi = x.Key.Grupocodi,
                                Fenergcodi = -1, //en programado no se puede determinar la fuente de combustible
                                Tgenercodi = x.First().Tgenercodi,
                                Ctgdetcodi = x.First().Ctgdetcodi > 0 ? (int?)x.First().Ctgdetcodi : null
                            }).ToList();
            }

            //resumen de datos de centrales integrantes COES
            List<MeDespachoResumenDTO> listaTotalEjec = listaDemandaGen48XDiaEjec.GroupBy(x => x.Medifecha)
                        .Select(x => new MeDespachoResumenDTO()
                        {
                            Dregentipo = tipoDespachoEjec,
                            Dregenfecha = x.Key,
                            Dregentotalsein = x.Sum(y => y.Meditotal ?? 0)
                        }).ToList();

            List<MeDespachoResumenDTO> listaTotalProg = new List<MeDespachoResumenDTO>();
            if (tipoDespachoProg > 0)
            {
                listaTotalProg = listaM48RangoProg.GroupBy(x => x.Medifecha)
                        .Select(x => new MeDespachoResumenDTO()
                        {
                            Dregentipo = tipoDespachoProg,
                            Dregenfecha = x.Key,
                            Dregentotalsein = x.Sum(y => y.Meditotal ?? 0)
                        }).ToList();
            }

            //Flujos de lineas
            List<MeMedicion48DTO> listaM48Flujo = FactorySic.GetMeMedicion48Repository().GetByCriteria(fechaIni, fechaFin, ConstantesPR5ReportesServicio.LectcodiFlujoPotencia.ToString(),
                                                            ConstantesAppServicio.TipoinfocodiMW, ConstantesAppServicio.ParametroDefecto);

            //Interconexiones Internacionales
            ListaFlujo30minInterconexion48(ConstantesInterconexiones.FuenteTIEFlujoOldDesktop, fechaIni, fechaFin, out List<MeMedicion48DTO> listaInterconexion48,
                        out List<MeMedicion48DTO> listaTotalExp, out List<MeMedicion48DTO> listaTotalImp);

            //Sumatoria de Generación e Intercambios
            List<MeMedicion48DTO> listaMedicionTotal48 = this.ListaDataMDTotalSEIN48(listaDemandaGen48XDiaEjec, listaInterconexion48);
            List<MeMedicion48DTO> listaMedicionTotal48Prog = this.ListaDataMDTotalSEIN48(listaDemandaGen48XDiaProg, new List<MeMedicion48DTO>());

            //Parametros para calculo de MD
            List<SiParametroValorDTO> listaBloqueHorario = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroHPPotenciaActiva);

            //Cálculo por áreas
            ListaDemandaArea30min(ConstantesInterconexiones.FuenteTIEFlujoOldDesktop, fechaIni, fechaFin, listaM48RangoEjec, listaM48Flujo, out List<MeMedicion48DTO> listArea);

            //ejecutados
            foreach (var regDia in listaTotalEjec)
            {
                var listaGenXDia = listaM48RangoEjec.Where(x => x.Medifecha == regDia.Dregenfecha.Date).ToList();
                var listAreaXDia = listArea.Where(x => x.Medifecha == regDia.Dregenfecha.Date).ToList();

                //Total de potencia en el día
                regDia.Dregentotalexp = listaTotalExp.Find(x => x.Medifecha == regDia.Dregenfecha.Date).Meditotal;
                regDia.Dregentotalimp = listaTotalImp.Find(x => x.Medifecha == regDia.Dregenfecha.Date).Meditotal;
                regDia.Dregentotalsein += (regDia.Dregentotalimp.GetValueOrDefault(0) - regDia.Dregentotalexp.GetValueOrDefault(0));

                regDia.Dregentotalnorte = listAreaXDia.Where(x => x.AreaOperativa == ConstantesPR5ReportesServicio.AreaNorte).Sum(x => x.Meditotal ?? 0);
                regDia.Dregentotalcentro = listAreaXDia.Where(x => x.AreaOperativa == ConstantesPR5ReportesServicio.AreaCentro).Sum(x => x.Meditotal ?? 0);
                regDia.Dregentotalsur = listAreaXDia.Where(x => x.AreaOperativa == ConstantesPR5ReportesServicio.AreaSur).Sum(x => x.Meditotal ?? 0);

                //Máxima demanda del SEIN
                this.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoMaximaTodoDia, listaMedicionTotal48, null, null, 
                                                                                out DateTime fechaHoraMDSein, out DateTime fechaDia48S, out int hMax48S);

                regDia.Dregenmdhidro = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiHidro).ToList());
                regDia.Dregenmdtermo = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiTermo).ToList());
                regDia.Dregenmdeolico = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiEolica).ToList());
                regDia.Dregenmdsolar = GetValorH(fechaHoraMDSein, listaGenXDia.Where(x => x.Tgenercodi == ConstantesPR5ReportesServicio.TgenercodiSolar).ToList());

                regDia.Dregenmdhora = fechaHoraMDSein; 
                regDia.Dregenmdsein = GetValorH(fechaHoraMDSein, listaMedicionTotal48);
                regDia.Dregenmdexp = GetValorH(fechaHoraMDSein, listaTotalExp);
                regDia.Dregenmdimp = GetValorH(fechaHoraMDSein, listaTotalImp);

                //Máxima Demanda en Hora Punta
                this.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoHoraPunta, listaMedicionTotal48, null, listaBloqueHorario, 
                                                                    out DateTime fechaHoraHP, out DateTime fechaDiaHP, out int hMaxHP);

                regDia.Dregenhphora = fechaHoraHP;
                regDia.Dregenhpsein = GetValorH(fechaHoraHP, listaMedicionTotal48);
                regDia.Dregenhpexp = GetValorH(fechaHoraHP, listaTotalExp);
                regDia.Dregenhpimp = GetValorH(fechaHoraHP, listaTotalImp);

                //Máxima Demanda en Fuera Hora Punta
                this.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoFueraHoraPunta, listaMedicionTotal48, null, listaBloqueHorario, 
                                                                    out DateTime fechaHoraFHP, out DateTime fechaDiaFHP, out int hMaxFHP);

                regDia.Dregenfhphora = fechaHoraFHP;
                regDia.Dregenfhpsein = GetValorH(fechaHoraFHP, listaMedicionTotal48);
                regDia.Dregenfhpexp = GetValorH(fechaHoraFHP, listaTotalExp);
                regDia.Dregenfhpimp = GetValorH(fechaHoraFHP, listaTotalImp);

                //Máxima demanda sin TTIE
                this.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoMaximaTodoDia, listaDemandaGen48XDiaEjec, null, null, 
                                                                                out DateTime fechaHoraMDSeinNoII, out DateTime fechaDia48NS, out int hMax48NS);

                regDia.Dregenmdnoiihora = fechaHoraMDSeinNoII;
                regDia.Dregenmdnoiisein = GetValorH(fechaHoraMDSeinNoII, listaDemandaGen48XDiaEjec);
            }

            //programados
            foreach (var regDia in listaTotalProg)
            {
                this.GetDiaMaximaDemandaFromDataMD48(regDia.Dregenfecha.Date, regDia.Dregenfecha.Date, ConstantesRepMaxDemanda.TipoMaximaTodoDia, listaMedicionTotal48Prog, null, null, 
                                                                                out DateTime fechaHoraMDSein, out DateTime fechaDia48, out int hMax48);

                regDia.Dregenmdhora = fechaHoraMDSein; 
                regDia.Dregenmdsein = GetValorH(fechaHoraMDSein, listaMedicionTotal48Prog);
            }

            //Guardar en BD
            List<MeDespachoResumenDTO> listaResumen = new List<MeDespachoResumenDTO>();
            listaResumen.AddRange(listaTotalEjec);
            listaResumen.AddRange(listaTotalProg);

            List<MeDespachoProdgenDTO> listaDetGen = new List<MeDespachoProdgenDTO>();
            listaDetGen.AddRange(listaDespachoEjecDiaXGrupo);
            listaDetGen.AddRange(listaDespachoProgDiaXGrupo);

            GuardarTablaMeDespachoTransaccional(tipoDespachoEjec, tipoDespachoProg, fechaIni, fechaFin, listaResumen, listaDetGen);
        }

        public decimal? GetValorH(DateTime fechaHora, List<MeMedicion48DTO> lista)
        {
            DateTime diaConsulta = fechaHora.Date;

            int mediaHora = fechaHora.Hour * 2 + fechaHora.Minute / 30;
            if (mediaHora == 0)
            {
                mediaHora = 48; //las 00:00
                diaConsulta = diaConsulta.AddDays(-1);
            }

            var listaXDia = lista.Where(x => x.Medifecha == diaConsulta.Date);
            if (listaXDia.Any())
            {
                decimal valor = 0;
                foreach (var regDia in listaXDia)
                {
                    decimal valorD = ((decimal?)regDia.GetType().GetProperty(ConstantesAppServicio.CaracterH + mediaHora.ToString()).GetValue(regDia, null)).GetValueOrDefault(0);
                    valor += valorD;
                }

                return valor;
            }

            return null;
        }

        private void GuardarTablaMeDespachoTransaccional(int tipoDespachoEjec, int tipoDespachoProg, DateTime fechaIni, DateTime fechaFin, 
                                                    List<MeDespachoResumenDTO> listaResumen, List<MeDespachoProdgenDTO> listaDetGen)
        {
            IDbConnection conn = null;
            DbTransaction tran = null;

            try
            {
                conn = FactorySic.GetMeDespachoResumenRepository().BeginConnection();
                tran = FactorySic.GetMeDespachoResumenRepository().StartTransaction(conn);

                DeleteMeDespachoResumen(tipoDespachoEjec, fechaIni, fechaFin, conn, tran);
                if (tipoDespachoProg > 0) DeleteMeDespachoResumen(tipoDespachoProg, fechaIni, fechaFin, conn, tran);

                DeleteMeDespachoProdgen(tipoDespachoEjec, fechaIni, fechaFin, conn, tran);
                if (tipoDespachoProg > 0) DeleteMeDespachoProdgen(tipoDespachoProg, fechaIni, fechaFin, conn, tran);

                int correlativoDregencodi = FactorySic.GetMeDespachoResumenRepository().GetMaxId();
                foreach (var reg in listaResumen)
                {
                    reg.Dregencodi = correlativoDregencodi;
                    SaveMeDespachoResumen(reg, conn, tran);
                    correlativoDregencodi++;
                }

                int correlativoDpgencodi = FactorySic.GetMeDespachoProdgenRepository().GetMaxId();
                foreach (var reg in listaDetGen)
                {
                    reg.Dpgencodi = correlativoDpgencodi;
                    SaveMeDespachoProdgen(reg, conn, tran);
                    correlativoDpgencodi++;
                }

                //guardar definitivamente
                tran.Commit();
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new ArgumentException("Ocurrió un error al momento de guardar los datos.");
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        #endregion

    }

    public class EjecutadoRptDespachoYRfria 
    {
        public List<MeMedicion48DTO> ListaMe48 { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<MeMedicion48DTO> ListaMe48TotalXDia { get; set; }
        public decimal EnergiaTotal { get; set; }

        public List<MeMedicion48DTO> ListaMe48Rfria { get; set; }
        public List<PrGrupoDTO> ListaGrupoRfria { get; set; }

    }
}
