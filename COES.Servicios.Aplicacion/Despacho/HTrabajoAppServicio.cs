using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Despacho.Helper;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using log4net;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using static COES.Servicios.Aplicacion.Migraciones.Helper.UtilCdispatch;

namespace COES.Servicios.Aplicacion.Despacho
{
    /// <summary>
    /// Clases con métodos del módulo Pronóstico Generación RER
    /// </summary>
    public class HTrabajoAppServicio : AppServicioBase
    {
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        readonly MigracionesAppServicio servMigraciones = new MigracionesAppServicio();

        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HTrabajoAppServicio));

        #region Métodos Tabla HT_CENTRAL_CFG

        /// <summary>
        /// Inserta un registro de la tabla HT_CENTRAL_CFG
        /// </summary>
        public int SaveHtCentralCfg(HtCentralCfgDTO entity)
        {
            try
            {
                return FactorySic.GetHtCentralCfgRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla HT_CENTRAL_CFG
        /// </summary>
        public void UpdateHtCentralCfg(HtCentralCfgDTO entity)
        {
            try
            {
                FactorySic.GetHtCentralCfgRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla HT_CENTRAL_CFG
        /// </summary>
        public void DeleteHtCentralCfg(int htcentcodi)
        {
            try
            {
                FactorySic.GetHtCentralCfgRepository().Delete(htcentcodi);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla HT_CENTRAL_CFG
        /// </summary>
        public HtCentralCfgDTO GetByIdHtCentralCfg(int htcentcodi)
        {
            var reg = FactorySic.GetHtCentralCfgRepository().GetById(htcentcodi);
            var listadetalles = this.ListHtCentralCfgdets().Where(x => x.Htcdetactivo == 1).ToList();// solo activos
            if (reg != null) FormatearConfiguracion(reg, listadetalles);

            return reg;

        }

        /// <summary>
        /// Permite listar todos los registros de la tabla HT_CENTRAL_CFG
        /// </summary>
        public List<HtCentralCfgDTO> ListHtCentralCfgs()
        {
            return FactorySic.GetHtCentralCfgRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla HtCentralCfg
        /// </summary>
        public List<HtCentralCfgDTO> GetByCriteriaHtCentralCfgs()
        {
            return FactorySic.GetHtCentralCfgRepository().GetByCriteria();
        }

        #endregion

        #region Métodos Tabla HT_CENTRAL_CFGDET

        /// <summary>
        /// Inserta un registro de la tabla HT_CENTRAL_CFGDET
        /// </summary>
        public void SaveHtCentralCfgdet(HtCentralCfgdetDTO entity)
        {
            try
            {
                FactorySic.GetHtCentralCfgdetRepository().Save(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza un registro de la tabla HT_CENTRAL_CFGDET
        /// </summary>
        public void UpdateHtCentralCfgdet(HtCentralCfgdetDTO entity)
        {
            try
            {
                FactorySic.GetHtCentralCfgdetRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina un registro de la tabla HT_CENTRAL_CFGDET
        /// </summary>
        public void DeleteHtCentralCfgdet()
        {
            try
            {
                FactorySic.GetHtCentralCfgdetRepository().Delete();
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite obtener un registro de la tabla HT_CENTRAL_CFGDET
        /// </summary>
        public HtCentralCfgdetDTO GetByIdHtCentralCfgdet()
        {
            return FactorySic.GetHtCentralCfgdetRepository().GetById();
        }

        /// <summary>
        /// Permite listar todos los registros de la tabla HT_CENTRAL_CFGDET
        /// </summary>
        public List<HtCentralCfgdetDTO> ListHtCentralCfgdets()
        {
            return FactorySic.GetHtCentralCfgdetRepository().List();
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla HtCentralCfgdet
        /// </summary>
        public List<HtCentralCfgdetDTO> GetByCriteriaHtCentralCfgdets(int htcentcodi)
        {
            return FactorySic.GetHtCentralCfgdetRepository().GetByCriteria(htcentcodi);
        }

        #endregion

        #region Configuracion de Centrales RER

        /// <summary>
        /// Permite obtener el listado de configuración
        /// </summary>
        /// <returns></returns>
        public List<HtCentralCfgDTO> ListarConfiguracionCentralesRER()
        {
            List<HtCentralCfgDTO> listaConfBD = ListHtCentralCfgs().Where(x => x.Htcentactivo == 1).ToList();// solo activos
            //obtener lista detalles
            var listadetalles = this.ListHtCentralCfgdets().Where(x => x.Htcdetactivo == 1).ToList();// solo activos
            //List<HtCentralCfgdetDTO> listaConfDetBD = ListHtCentralCfgdets();

            foreach (var item in listaConfBD)
                this.FormatearConfiguracion(item, listadetalles);

            return listaConfBD.OrderBy(x => x.Famnomb).ThenBy(x => x.Equinomb).ToList();
        }

        /// <summary>
        /// Permite dar formato a la cabecera y sus detalles 
        /// </summary>
        /// <param name="reg"></param>
        private void FormatearConfiguracion(HtCentralCfgDTO reg, List<HtCentralCfgdetDTO> listadetalles)
        {
            if (reg != null)
            {
                //FILTRAR DETALLE
                //var listaConfDetFilter = GetByCriteriaHtCentralCfgdets(reg.Htcentcodi);
                var listaConfDetFilter = listadetalles.Where(x => x.Htcentcodi == reg.Htcentcodi).ToList(); //lista detalles

                reg.NombreElemento = reg.Htcentfuente == ConstantesDespacho.TipoFuenteHtrabajo ? string.Join("|", listaConfDetFilter.Select(x => string.Format("Punto despacho - {0} - {1}", x.Ptomedicodi, x.Ptomedielenomb)))
                    : string.Join("|", listaConfDetFilter.Select(x => string.Format("Canal Scada - {0} - {1}", x.Canalcodi, x.Canalnomb)));
                reg.FactorDesc = string.Join("|", listaConfDetFilter.Select(x => x.Htcdetfactor));

                var central = ListarCentralesConfigReqHT().Find(x => x.Equicodi == reg.Equicodi);
                reg.Famcodi = central.Famcodi.Value;
                reg.Equinomb = central.Equinomb;
                reg.Famnomb = central.Famnomb;
                reg.Famnomb = (reg.Famnomb ?? "").Trim();
                reg.Equinomb = (reg.Equinomb ?? "").Trim();
                reg.FuenteDesc = reg.Htcentfuente == ConstantesDespacho.TipoFuenteHtrabajo ? "Archivo HTrabajo" : "Tiempo Real SP7";
                reg.HtcentfecregistroDesc = reg.Htcentfecregistro != null ? reg.Htcentfecregistro.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : string.Empty;
                reg.UltimaModificacionFechaDesc = reg.Htcentfecmodificacion != null ? reg.Htcentfecmodificacion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2) : reg.HtcentfecregistroDesc;
                reg.UltimaModificacionUsuarioDesc = reg.Htcentusumodificacion != null ? reg.Htcentusumodificacion : reg.Htcentusuregistro;
            }
        }

        /// <summary>
        /// Permite guardar y editar cabecera y sus detalles
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="idCentralCfg"></param>
        /// <param name="tipo"></param>
        /// <param name="listaConfWeb"></param>
        /// <param name="username"></param>
        public void GuardarConfiguracion(int equicodi, int idCentralCfg, int tipo, List<HtCentralCfgdetDTO> listaConfWeb, string username)
        {
            HtCentralCfgDTO objConf = GetByIdHtCentralCfg(idCentralCfg);
            List<HtCentralCfgdetDTO> listaConfBD = GetByCriteriaHtCentralCfgdets(idCentralCfg);

            int codigoCfg = 0;
            if (idCentralCfg > 0)
            {
                objConf = GetByIdHtCentralCfg(idCentralCfg);
                objConf.Htcentfuente = tipo == 1 ? ConstantesDespacho.TipoFuenteHtrabajo : ConstantesDespacho.TipoFuenteScada;
                objConf.Htcentfecmodificacion = DateTime.Now;
                objConf.Htcentusumodificacion = username;
                codigoCfg = objConf.Htcentcodi;
                UpdateHtCentralCfg(objConf);
            }
            else
            {
                objConf = new HtCentralCfgDTO();
                objConf.Equicodi = equicodi;
                objConf.Htcentfuente = tipo == 1 ? ConstantesDespacho.TipoFuenteHtrabajo : ConstantesDespacho.TipoFuenteScada;
                objConf.Htcentfecregistro = DateTime.Now;
                objConf.Htcentusuregistro = username;
                objConf.Htcentactivo = 1;

                codigoCfg = SaveHtCentralCfg(objConf);
            }

            //la configuracion 
            List<HtCentralCfgdetDTO> listaActivo = new List<HtCentralCfgdetDTO>();
            foreach (var reg in listaConfWeb)
            {
                HtCentralCfgdetDTO regBD = new HtCentralCfgdetDTO();
                if (tipo == ConstantesDespacho.TipoHtrabajo)
                    regBD = listaConfBD.Find(x => x.Ptomedicodi == reg.Ptomedicodi);
                else
                    regBD = listaConfBD.Find(x => x.Canalcodi == reg.Canalcodi);

                if (regBD != null)
                {
                    var regClone = (HtCentralCfgdetDTO)regBD.Clone();
                    regClone.Htcdetfactor = reg.Htcdetfactor;
                    regClone.Htcdetactivo = 1;
                    listaActivo.Add(regClone);
                }
                else
                {
                    reg.Htcentcodi = codigoCfg;
                    reg.Ptomedicodi = reg.Ptomedicodi;
                    reg.Canalcodi = reg.Canalcodi;
                    reg.Htcdetfactor = reg.Htcdetfactor;
                    reg.Htcdetactivo = 1;
                    listaActivo.Add(reg);
                }
            }

            //dar de baja lo que esta en bd
            List<HtCentralCfgdetDTO> listaBaja = new List<HtCentralCfgdetDTO>();
            foreach (var regBD in listaConfBD)
            {
                var regClone = (HtCentralCfgdetDTO)regBD.Clone();
                regClone.Htcdetactivo = 0;
                listaBaja.Add(regClone);
            }

            //nuevos registros y dar de baja
            foreach (var reg in listaActivo)
            {
                SaveHtCentralCfgdet(reg);
            }
            foreach (var reg in listaBaja)
            {
                UpdateHtCentralCfgdet(reg);
            }
        }

        /// <summary>
        /// Permite eliminar(baja) una configuración y detalles
        /// </summary>
        /// <param name="htcentcodi"></param>
        /// <param name="username"></param>
        public void EliminarConfiguracion(int htcentcodi, string username)
        {
            HtCentralCfgDTO objConf = new HtCentralCfgDTO();
            List<HtCentralCfgdetDTO> listaConfBD = new List<HtCentralCfgdetDTO>();
            if (htcentcodi > 0)
            {
                // Dar de baja la configuración
                objConf = GetByIdHtCentralCfg(htcentcodi);
                objConf.Htcentfecmodificacion = DateTime.Now;
                objConf.Htcentusumodificacion = username;
                objConf.Htcentactivo = 0; //
                UpdateHtCentralCfg(objConf);


                listaConfBD = GetByCriteriaHtCentralCfgdets(htcentcodi);
                List<HtCentralCfgdetDTO> listaBaja = new List<HtCentralCfgdetDTO>();
                foreach (var regBD in listaConfBD)
                {
                    // Dar de baja el detalle
                    var regClone = (HtCentralCfgdetDTO)regBD.Clone();
                    regClone.Htcdetactivo = 0;
                    listaBaja.Add(regClone);
                }

                foreach (var reg in listaBaja)
                {
                    UpdateHtCentralCfgdet(reg);
                }
            }
        }

        /// <summary>
        /// permite obtener el listado de centrales solares y eólicas
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesConfig()
        {
            List<int> listadoFamiliasCentrales = new List<int>();
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoSolar); // solares
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoEolica); // eolicas
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoGeneradorSolar); // eolicas
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoGeneradorEolico); // eolicas

            List<EqEquipoDTO> listaCentrales = this.ListarEquipoxFamilias(listadoFamiliasCentrales.ToArray()).ToList();
            listaCentrales = listaCentrales.Where(x => x.Equiestado == "A" || x.Equiestado == "F" || x.Equiestado == "P").OrderBy(t => t.Equinomb).ToList();
            return listaCentrales;
        }

        /// <summary>
        /// permite obtener el listado de centrales solares y eólicas
        /// </summary>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarCentralesConfigReqHT()
        {
            List<int> listadoFamiliasCentrales = new List<int>();
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoSolar); // solares
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoEolica); // eolicas
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoGeneradorSolar); // eolicas
            listadoFamiliasCentrales.Add(ConstantesDespacho.TipoGeneradorEolico); // eolicas

            List<EqEquipoDTO> listaCentrales = this.ListarEquipoxFamiliasActivosyProyectos(listadoFamiliasCentrales.ToArray()).ToList();
            return listaCentrales;
        }

        /// <summary>
        /// Permite obtener el punto de medición
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public MePtomedicionDTO GetByIdEquipo(int ptomedicodi)
        {
            return FactorySic.GetMePtomedicionRepository().GetById(ptomedicodi);
        }

        /// <summary>
        /// Permite obtener el canal
        /// </summary>
        /// <param name="canalcodi"></param>
        /// <returns></returns>
        public TrCanalSp7DTO GetByIdCanal(int canalcodi)
        {
            return FactoryScada.GetTrCanalSp7Repository().GetById(canalcodi);
        }

        /// <summary>
        /// Listado de Equipos filtrado por varias familias.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamilias(params int[] iCodFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquipoxFamilias(iCodFamilias);
        }

        /// <summary>
        /// Listado de Equipos filtrado por varias familias.
        /// Datos de Equipo, Familia, Empresa y Area
        /// </summary>
        /// <param name="iCodFamilias">Código de Familias</param>
        /// <returns></returns>
        public List<EqEquipoDTO> ListarEquipoxFamiliasActivosyProyectos(params int[] iCodFamilias)
        {
            return FactorySic.GetEqEquipoRepository().ListarEquipoxFamiliasActivosyProyectos(iCodFamilias);
        }

        #endregion

        #region Procesar archivo de generación RER

        public void ObtenerMediaHoraAProcesar(DateTime fechaSistema, out string fechaWS, out int h)
        {
            SiProcesoDTO objProceso = FactorySic.GetSiProcesoRepository().GetById(ConstantesDespacho.PrcscodiEjecutarCargaFTPPronRER);
            DateTime fechaPosible = fechaSistema.AddMinutes((objProceso.Prscminutoinicio ?? 0) * -1).AddMinutes(+1);

            fechaWS = fechaPosible.Date.ToString(ConstantesAppServicio.FormatoFechaWS);

            int indice = Util.GetPosicionHoraFinal48(fechaPosible)[0];
            h = indice - 1;

            //00:00
            if (h <= 0)
            {
                h = 48;
                fechaWS = fechaPosible.Date.AddDays(-1).ToString(ConstantesAppServicio.FormatoFechaWS);
            }
        }

        /// <summary>
        /// Permite realizar el procesamiento del archivo y convertir a csv
        /// </summary>
        /// <param name="rutaWorkspace"></param>
        /// <param name="archivoHTrabajo"></param>
        /// <param name="fechaMediaHora"></param>
        /// <param name="archivoCsv"></param>
        /// <param name="listaObs"></param>
        /// <returns></returns>
        public void ProcesarArchivoGeneracionRER(string rutaWorkspace, string archivoHTrabajo, string archivoCsv, DateTime fechaDia, int hMax, decimal umbralIni, decimal umbralFin
                                                    , out List<ErrorHtrabajo> listaObs)
        {
            listaObs = new List<ErrorHtrabajo>();

            //convertir htrabajo a lista de memedicion48 
            var listMed48HT = ListarM48FromHtrabajo(rutaWorkspace, archivoHTrabajo, fechaDia);

            //obtener configuracion de centrales
            List<HtCentralCfgDTO> lstCentrales = ListarConfiguracionCentralesRER().OrderBy(x => x.Famnomb).ThenBy(x => x.Equinomb).ToList();
            var listadetalles = this.ListHtCentralCfgdets().Where(x => x.Htcdetactivo == 1).ToList();// solo activos

            //obtener la data de tiempo real
            var lisDetailsCanal = listadetalles.Where(x => x.Canalcodi != null).ToList(); // solo detalles de canales
            List<string> lstCanalcodis = lisDetailsCanal.Select(x => x.Canalcodi.ToString()).ToList();
            string canalcodis = string.Join(",", lstCanalcodis);

            if (canalcodis == "")
                canalcodis = "-1";

            List<MeScadaSp7DTO> listaDataSp7 = GetByCriteriaMeScadaSp7s(fechaDia, fechaDia, canalcodis);

            //convertir tiempo real a lista de memedicion48 
            var listMed48TR = ListarM48FromTiempoReal(listaDataSp7);

            //Generar archivo csv
            string textoArchivo = GenerarContenidoCsv(fechaDia, hMax, listMed48HT, listMed48TR
                                            , lstCentrales, listadetalles, umbralIni, umbralFin, out List<ErrorHtrabajo> listaErrorDatos);
            listaObs.AddRange(listaErrorDatos);

            var resultado = GenerarArchivo(archivoCsv, rutaWorkspace, textoArchivo);
        }

        /// <summary>
        /// Permite leer los datos del archivo Htrabajo_generación xlsm y convertirlo en MeMedicion48DTO
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ListarM48FromHtrabajo(string path, string fileName, DateTime fecha)
        {
            //leer excel
            List<MeMedicion48DTO> listaM48Excel = new List<MeMedicion48DTO>();

            var filePath = path + fileName;

            // Check if the file exists
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                throw new Exception("File " + filePath + " Does Not Exists");
            }

            int indexCodigoMW = 11;

            using (ExcelPackage xlPackage = new ExcelPackage(fi))
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[ConstantesDespacho.HojaActivaExcel];

                // Fetch the WorkSheet size
                ExcelCellAddress startCell = worksheet.Dimension.Start;
                ExcelCellAddress endCell = worksheet.Dimension.End;

                int rowIniPto = 1;
                int colIniPto = 2;
                int rowStarData = 9; // leer data
                int rowEndData = 56; // leer data
                //int colStarData = 2; // leer data
                while (worksheet.Cells[rowIniPto, colIniPto].Value != null) // mientras exista elementos
                {
                    int pto = 0;
                    var codigoPto = worksheet.Cells[rowIniPto, colIniPto].Value.ToString();
                    codigoPto = (codigoPto ?? "").Trim();
                    Int32.TryParse(codigoPto, out pto);
                    if (pto < 0)
                    {
                        colIniPto++;
                        continue;
                    }

                    MeMedicion48DTO entidad = new MeMedicion48DTO();
                    entidad.Medifecha = fecha;
                    entidad.Ptomedicodi = pto;

                    //por cada columna de codigo positivo, traer data de fila 9 a 56
                    int num = 1;
                    for (int row = rowStarData; row <= rowEndData; row++)
                    {
                        var sValorH = string.Empty;
                        if (worksheet.Cells[row, colIniPto].Value != null) sValorH = worksheet.Cells[row, colIniPto].Value.ToString();

                        decimal valorH;
                        sValorH = (sValorH ?? "").Trim();
                        decimal.TryParse(sValorH, out valorH);
                        entidad.GetType().GetProperty("H" + num.ToString()).SetValue(entidad, valorH);

                        num++;
                    }

                    listaM48Excel.Add(entidad);
                    colIniPto++;
                }
            }

            return listaM48Excel;
        }

        /// <summary>
        /// Permite obtener la data para Scada y convertirlo en MeMedicion48DTO
        /// </summary>
        /// <param name="listaDataSp7"></param>
        /// <returns></returns>
        private List<MeMedicion48DTO> ListarM48FromTiempoReal(List<MeScadaSp7DTO> listaDataSp7)
        {
            //var canales = listaDataSp7.GroupBy(x => x.Canalcodi).ToList();
            List<MeMedicion48DTO> listaM48 = new List<MeMedicion48DTO>();

            foreach (var m96 in listaDataSp7)
            {
                MeMedicion48DTO entidad = new MeMedicion48DTO();
                for (int i = 1; i <= 48; i++)
                {
                    //var m96 = listaDataSp7.Find(x => x.Canalcodi == d.Key);
                    decimal? valor = (decimal?)m96.GetType().GetProperty("H" + i * 2).GetValue(m96, null);
                    entidad.Medifecha = m96.Medifecha;
                    entidad.Canalcodi = m96.Canalcodi;
                    entidad.GetType().GetProperty("H" + i.ToString()).SetValue(entidad, valor);
                }
                listaM48.Add(entidad);
            }

            return listaM48;
        }

        /// <summary>
        /// Permite realizar búsquedas en la tabla MeScadaSp7
        /// </summary>
        public List<MeScadaSp7DTO> GetByCriteriaMeScadaSp7s(DateTime fechaIni, DateTime fechaFin, string canalcodi)
        {
            return FactoryScada.GetMeScadaSp7Repository().GetByCriteria(fechaIni, fechaFin, canalcodi);
        }

        /// <summary>
        /// Permite crear el archivo Measurement csv
        /// </summary>
        /// <param name="ruta"></param>
        /// <param name="fechaMediaHora"></param>
        /// <param name="listMed48HT"></param>
        /// <param name="listMed48TR"></param>
        /// <param name="listadetalles"></param>
        /// <param name="nameFile"></param>
        /// <param name="listaErrorDatos"></param>
        /// <returns></returns>
        public string GenerarContenidoCsv(DateTime fechaDia, int hMax, List<MeMedicion48DTO> listMed48HT, List<MeMedicion48DTO> listMed48TR
                                , List<HtCentralCfgDTO> lstCentrales, List<HtCentralCfgdetDTO> listadetalles, decimal umbralIni, decimal umbralFin, out List<ErrorHtrabajo> listaErrorDatos)
        {
            listaErrorDatos = new List<ErrorHtrabajo>();

            string textoArchivo = "";

            //CABECERA 
            string sLineCabecera = string.Empty;
            sLineCabecera += "begin [UTC-5]" + ConstantesDespacho.SeparadorCampo;
            sLineCabecera += "end [UTC-5]" + ConstantesDespacho.SeparadorCampo;

            //foreach (var reg in lstCentrales)
            //{
            //    sLineCabecera += reg.Equinomb + ConstantesDespacho.SeparadorCampo;
            //}

            sLineCabecera += string.Join(",", lstCentrales.Select(x => x.Equinomb + " [MW]"));

            AgregaLinea(ref textoArchivo, sLineCabecera);


            //CUERPO
            var fechaInicial = fechaDia;
            for (int i = 1; i <= hMax; i++)
            {
                string sLine = string.Empty;
                List<string> fila = new List<string>();
                sLine += fechaInicial.ToString(ConstantesBase.FormatFechaFull) + ConstantesDespacho.SeparadorCampo;
                //Agregar media hora
                fechaInicial = fechaInicial.AddMinutes(30);
                sLine += fechaInicial.ToString(ConstantesBase.FormatFechaFull) + ConstantesDespacho.SeparadorCampo;
                foreach (var item in lstCentrales)
                {
                    var listaConfDetFilter = listadetalles.Where(x => x.Htcentcodi == item.Htcentcodi).ToList(); //lista detalles

                    //obtener pto o canal según fuente
                    decimal? total = 0m;
                    bool sumaValida = false;
                    foreach (var det in listaConfDetFilter)
                    {
                        var valor = ObtenerValorNumericoCelda(i, det, item.Htcentfuente, listMed48HT, listMed48TR);
                        if (valor != null)
                        {
                            sumaValida = true;
                            total += valor;
                        }
                    }

                    if (sumaValida) // SI LA SUMA NO ES NULL
                    {
                        if (item.Htcentfuente == ConstantesDespacho.TipoFuenteScada)
                        {
                            //caso especial de SSAA negativos de Scada 
                            if (total > umbralIni && total < 0) total = 0;
                        }

                        if (total < 0m || total > 1000m) // SI ESTA FUERA DE LOS LÍMITES
                        {
                            //ERROR: fuera de límites
                            var error = new ErrorHtrabajo();
                            error.Tipo = "ALERTA";
                            error.Descripcion = "VALOR FUERA DE LOS LÍMITES";
                            error.Valor = total.ToString();
                            error.Fuente = item.Htcentfuente == ConstantesDespacho.TipoFuenteHtrabajo ? "Archivo HTrabajo" : "Tiempo real SP7";
                            error.Central = item.Equinomb;
                            error.MediaHora = fechaInicial.ToString(ConstantesBase.FormatFechaFull);
                            listaErrorDatos.Add(error);
                        }

                        if (total < umbralFin)
                        {
                            decimal valorMenor = 0m;
                            fila.Add(valorMenor.ToString());
                        }
                        else
                            fila.Add(total.ToString());
                    }
                    else //(ES NULL EN BLANCO)
                    {
                        //sLine += string.Empty + ConstantesDespacho.SeparadorCampo;
                        fila.Add(string.Empty);

                        // ERROR: NO ESTÁ EN M48
                        var error = new ErrorHtrabajo();
                        error.Tipo = "ALERTA";
                        error.Descripcion = "no existe en medición48";
                        error.Valor = string.Empty;
                        error.Fuente = item.Htcentfuente == ConstantesDespacho.TipoFuenteHtrabajo ? "Archivo HTrabajo" : "Tiempo real SP7";
                        error.Central = item.Equinomb;
                        error.MediaHora = fechaInicial.ToString(ConstantesBase.FormatFechaFull);
                        listaErrorDatos.Add(error);
                    }
                }

                sLine += string.Join(",", fila);

                AgregaLinea(ref textoArchivo, sLine);
            }

            //ultima fila es linea vacia
            AgregaLinea(ref textoArchivo, string.Empty);

            return textoArchivo;
        }

        /// <summary>
        /// Permite obtener el valor numérico del producto del H Y el factor del punto/canal
        /// </summary>
        /// <param name="h"></param>
        /// <param name="det"></param>
        /// <param name="fuenteDato"></param>
        /// <param name="listMed48HT"></param>
        /// <param name="listMed48TR"></param>
        /// <returns></returns>
        private decimal? ObtenerValorNumericoCelda(int h, HtCentralCfgdetDTO det, string fuenteDato, List<MeMedicion48DTO> listMed48HT, List<MeMedicion48DTO> listMed48TR)
        {
            decimal? resultado = null;

            var m48 = fuenteDato == ConstantesDespacho.TipoFuenteHtrabajo ? listMed48HT.Find(x => x.Ptomedicodi == det.Ptomedicodi)
                : listMed48TR.Find(x => x.Canalcodi == det.Canalcodi);
            if (m48 != null)
            {
                var valor = (decimal?)m48.GetType().GetProperty("H" + h).GetValue(m48, null);
                if (valor != null)
                {
                    resultado = valor.Value * det.Htcdetfactor;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Permite enviar notificación del resultado del proceso
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="listaErr"></param>
        /// <param name="fecha"></param>
        /// <param name="nombreArchivo"></param>
        public void EnviarNotificacionEnvio(int tipo, List<ErrorHtrabajo> listaErr, DateTime fecha, string nombreArchivo = "")
        {
            //plantilla
            int plantcodi = ConstantesDespacho.PlantcodiNotificacion;
            SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

            try
            {
                List<string> listaToEmails = (plantilla.Planticorreos ?? "").Split(';').ToList();
                List<string> ccEmails = (plantilla.PlanticorreosCc ?? "").Split(';').Where(x => x != "").ToList();
                List<string> bccEmails = (plantilla.PlanticorreosBcc ?? "").Split(';').Where(x => x != "").ToList();

                string asunto = "";
                asunto = String.Format(plantilla.Plantasunto, fecha.ToString(ConstantesDespacho.FormatoFechaHora));

                string contenido = this.ObtenerContenidoCorreo(tipo, listaErr, nombreArchivo);
                COES.Base.Tools.Util.SendEmail(listaToEmails, ccEmails, bccEmails, asunto, contenido, plantilla.PlanticorreoFrom);

                //Guardar en SI_CORREO
                //var correo = new SiCorreoDTO();
                //correo.Corrasunto = string.Format(plantilla.Plantasunto, fecha.ToString(ConstantesBase.FormatoFechaHora));
                //correo.Corrcontenido = contenido;
                //correo.Corrfechaenvio = DateTime.Now; ;
                //correo.Corrfrom = TipoPlantillaCorreo.MailFrom;
                //correo.Corrto = string.Join(";", plantilla.Planticorreos);
                //correo.Corrcc = string.Join(";", ccEmails);
                //correo.Corrbcc = string.Join(";", bccEmails);
                //correo.Enviocodi = null;
                //correo.Plantcodi = plantilla.Plantcodi;
                //servCorreo.SaveSiCorreo(correo);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Permite obtener cuerpo del correo para notificación
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="listaErr"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        private string ObtenerContenidoCorreo(int tipo, List<ErrorHtrabajo> listaErr, string nombreArchivo)
        {
            StringBuilder htmlElemento = new StringBuilder();

            string msjResult = string.Empty;
            string tableVisible = string.Empty;
            switch (tipo)
            {
                case 0: // exito sin alertas
                    msjResult = "Se realizó el envío del archivo exitosamente.";
                    tableVisible = "none";
                    break;
                case 1: //Error de Onedrive
                    msjResult = "No se realizó el envío del archivo. No existe archivo " + nombreArchivo + " en el repositorio Onedrive.";
                    tableVisible = "none";
                    break;
                case 2: //exito con alertas
                    msjResult = "Se realizó el envío del archivo exitosamente. Los siguientes datos no estaban en el rango [0, 1000] o no existen datos para la media hora procesada.";
                    tableVisible = "block";

                    listaErr = listaErr.OrderBy(x => x.Central).ThenBy(x => x.MediaHora).ToList();
                    foreach (var item in listaErr)
                    {
                        htmlElemento.Append(String.Format(HtmlNotificacion.HtmlAlertas, item.Central, item.Fuente, item.MediaHora, item.Valor));
                    }
                    break;
                case 3: //Error servidor ftp
                    msjResult = "No se realizó el envío del archivo. No se cargó el archivo " + nombreArchivo + " en el servidor FTP / SFTP.";
                    tableVisible = "none";
                    break;
                case 4: //No se pudo generar el archivo CSV
                    msjResult = "No se realizó el envío del archivo. No se generó archivo " + nombreArchivo + " en el servidor COES.";
                    tableVisible = "none";
                    break;
                case 5: //No se pudo conectar a servidor COES.
                    msjResult = "No se realizó el envío del archivo. El sistema no se pudo conectar al servidor COES.";
                    tableVisible = "none";
                    break;
            }

            String mensaje = String.Format(HtmlNotificacion.HtmlCuerpo, msjResult, tableVisible, htmlElemento.ToString(), HtmlNotificacion.HtmlCss);
            //mensaje = mensaje.Replace("[", "{");
            //mensaje = mensaje.Replace("]", "}");
            return mensaje;
        }

        #region Utilidad

        /// <summary>
        /// Generar el archivo en ruta
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static bool GenerarArchivo(string fileName, string path, string texto)
        {
            try
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    // Reemplazar por FilerServer
                    using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    {
                        file.Write(texto);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                //Guardar log de error
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }
        }

        /// <summary>
        /// Agregar línea
        /// </summary>
        /// <param name="log"></param>
        /// <param name="linea"></param>
        private static void AgregaLinea(ref string log, string linea)
        {
            log += linea + "\r\n";
        }

        /// <summary>
        /// Armar html para las notificaciones
        /// </summary>
        public struct HtmlNotificacion
        {
            public const string HtmlAlertas = @"<tr>
					                 <td class='tdcelda'>{0}</td>
					                 <td class='tdcelda'>{1}</td>
					                 <td class='tdcelda'>{2}</td>
					                 <td class='tdcelda'>{3}</td>
				                 </tr>";

            public const string HtmlCss = @"<style type='text/css'>
                            body{
	                            background-color:#EEF0F2;
	                            top:0;
	                            left:0;
	                            margin:0;
	                            font-family:Arial, Helvetica, sans-serif;
	                            font-size:12px;
	                            color:#333333;
                            }
                            .content{
	                            width:80%;
	                            margin:auto;
                            }

                            .titulo{
	                            font-size:16px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-align:center;
	                            padding:20px;
	                            text-transform:uppercase;
                            }

                            .subtitulo{
	                            font-size:13px;
	                            color:#004080;
	                            font-weight:bold;
	                            text-transform:uppercase;		
                            }

                            .table{
	
	                            margin-bottom:20px;
                            }

                            .trtitulo{
	                            background-color:#506DBE;
	                            color:#fff;
	                            font-weight:bold;
	                            text-align:center;
	                            line-height:20px;
	                            font-size:10px;
	                            text-transform:uppercase;
                            }

                            .tdcelda{
	                            background-color:#fff;
	                            text-align:center;
	                            border:1px solid #DBDCDD;
	                            border-top:1px none;
	                            line-height:18px;
                                font-size:11px;
                            }

                            </style>";

            public const string HtmlCuerpo = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Informe equipamiento</title>
                            {3}
                            </head>

                            <body>

                            <p>Estimados Señores,<br /></p>

                            <p>{0}</p>
                                <table class='content' style='display: {1}'>
	                                <tr>
		                                <td>
			                                <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                                <tr class='trtitulo'>
					                                <td>Central</td>
					                                <td>Fuente</td>
					                                <td>Media Hora</td>
					                                <td>Valor</td>
				                                </tr>
                                                {2}
                                            </table>
                                            <br />
                                            <br />
		                                </td>
	                                </tr>
                                </table>
                            </body>
                            </html>
                            ";

            public const string HtmlAlertas2 = @"<tr>
					                 <td class='tdcelda'>{0}</td>
					                 <td class='tdcelda'>{1}</td>
					                 <td class='tdcelda'>{2}</td>
				                 </tr>";

            public const string HtmlCuerpoAlerta = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                            <html xmlns='http://www.w3.org/1999/xhtml'>
                            <head>
                            <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
                            <title>Alertas:</title>
                            {2}
                            </head>

                            <body>
                                <table class='content' style='display: {0}'>
	                                <tr>
		                                <td>
			                                <table cellspacing='0' cellpadding='0' width='100%' border='0' class='table'>
				                                <tr class='trtitulo'>
					                                <td>Punto medición</td>
					                                <td>Celda</td>
					                                <td>Descripción</td>
				                                </tr>
                                                {1}
                                            </table>
                                            <br />
                                            <br />
		                                </td>
	                                </tr>
                                </table>
                            </body>
                            </html>
                            ";
        }

        #endregion

        #endregion

        #region Cargar archivo Htrabajo en SICOES

        /// <summary>
        /// Obtener Fecha para procesar
        /// </summary>
        /// <param name="fechaSistema"></param>
        /// <param name="fechaWS"></param>
        public void ObtenerFechaAProcesar(DateTime fechaSistema, out string fechaWS)
        {
            //fecha de validación
            DateTime fechaValidacion = DateTime.Now;
            TimeSpan ts = new TimeSpan(0, 30, 0);
            fechaValidacion = fechaValidacion.Date + ts;

            //fecha sistema
            if (fechaSistema.Date == fechaValidacion.Date)
            {
                //si es anterior a las 00:30 entonces cargar los datos del día de ayer sino hoy
                if (fechaSistema < fechaValidacion)
                    fechaWS = fechaSistema.Date.AddDays(-1).ToString(ConstantesAppServicio.FormatoFechaWS);
                else
                    fechaWS = fechaSistema.Date.ToString(ConstantesAppServicio.FormatoFechaWS);
            }
            else
            {
                //carga de fechas anteriores a hoy
                fechaWS = fechaSistema.Date.ToString(ConstantesAppServicio.FormatoFechaWS);
            }
        }

        /// <summary>
        /// Guardar archivo Cdispatch
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="flagCarga"></param>
        /// <param name="path"></param>
        /// <param name="nombreFile"></param>
        /// <param name="listaErrorValidacion"></param>
        public void GuardarCdispatch(int lectcodi, HtFiltro flagCarga, string path, string nombreFile, out List<HtError> listaErrorValidacion)
        {
            listaErrorValidacion = new List<HtError>();
            try
            {
                servMigraciones.LeerFileUpxls(nombreFile, path, lectcodi, flagCarga, true, out List <MeMedicion48DTO> listaMe48, out List<string> listaOK, out List<string> listaError, out List<HtError> listaError2);
                listaErrorValidacion = listaError2;

                if (listaMe48.Any())
                {
                    servMigraciones.LoadDispatchFromHtrabajo(lectcodi, listaMe48, out CDespachoGlobal regCDespacho);

                    servMigraciones.GrabarCDispatch(regCDespacho, "SISTEMA", "SISTEMA", true);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Permite enviar notificación del resultado del proceso
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="listaErr"></param>
        /// <param name="fecha"></param>
        public void EnviarNotificacionCdisptch(int tipo, List<ErrorHtrabajo> listaErr, DateTime fecha)
        {
            //Generar Tupla de Variable y valor
            var mapaVariable = new Dictionary<string, string>();
            mapaVariable[ConstantesDespacho.VariableFechaProceso] = fecha.ToString(ConstantesAppServicio.FormatoFecha);
            mapaVariable[ConstantesDespacho.VariableFechaSistema] = DateTime.Now.ToString(ConstantesAppServicio.FormatoFechaFull);

            string cuerpo = this.ObtenerContenidoAlerta(tipo, listaErr);

            if(tipo != 2) // exito
                mapaVariable[ConstantesDespacho.VariableAlertas] = cuerpo;
            else // error
                mapaVariable[ConstantesDespacho.VariableErrores] = cuerpo;

            try
            {
                //plantilla
                int plantcodi = tipo != 2 ? ConstantesDespacho.PlantcodiNotificacionExito : ConstantesDespacho.PlantcodiNotificacionError;
                SiPlantillacorreoDTO plantilla = servCorreo.GetByIdSiPlantillacorreo(plantcodi);

                string asunto = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantasunto, mapaVariable);
                string contenido = CorreoAppServicio.GetTextoSinVariable(plantilla.Plantcontenido, mapaVariable);
                string from = TipoPlantillaCorreo.MailFrom;
                string to = plantilla.Planticorreos;
                string cc = plantilla.PlanticorreosCc;
                string bcc = plantilla.PlanticorreosBcc;

                List<string> listaTo = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(to);
                List<string> listaCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(cc);
                List<string> listaBCC = CorreoAppServicio.ListarCorreosValidoSegunAmbiente(bcc, true, true);
                string asuntoEmail = CorreoAppServicio.GetTextoAsuntoSegunAmbiente(asunto);
                to = string.Join(";", listaTo);
                cc = string.Join(";", listaCC);
                bcc = string.Join(";", listaBCC);

                if (!string.IsNullOrEmpty(contenido))
                {
                    //Enviar correo
                    COES.Base.Tools.Util.SendEmail(listaTo, listaCC, listaBCC, asunto, contenido, plantilla.PlanticorreoFrom);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
        }

        /// <summary>
        /// Permite obtener el contenido de la alerta
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="listaErr"></param>
        /// <returns></returns>
        private string ObtenerContenidoAlerta(int tipo, List<ErrorHtrabajo> listaErr)
        {
            if (listaErr != null && listaErr.Any())
            {
                StringBuilder htmlElemento = new StringBuilder();
                string tableVisible = string.Empty;

                switch (tipo)
                {
                    case 0: //exito sin alertas
                        tableVisible = "none";
                        break;
                    case 1: //exito con alertas
                        tableVisible = "block";
                        List<ErrorHtrabajo> listaAlertas = new List<ErrorHtrabajo>();
                        foreach (var listaAgrupada in listaErr.GroupBy(x => new { x.Ptomedicion, x.Descripcion }))
                        {
                            var celdas = listaAgrupada.ToList().Select(x => x.Posicion).ToList();
                            var posicion = string.Join(",", celdas);
                            listaAlertas.Add(new ErrorHtrabajo { Ptomedicion = listaAgrupada.Key.Ptomedicion, Posicion = posicion, Descripcion = listaAgrupada.Key.Descripcion });
                        }

                        listaAlertas = listaAlertas.OrderBy(x => x.Ptomedicion).ThenBy(x => x.Descripcion).ToList();
                        foreach (var item in listaAlertas)
                        {
                            htmlElemento.Append(string.Format(HtmlNotificacion.HtmlAlertas2, item.Ptomedicion, item.Posicion, item.Descripcion));
                        }
                        break;
                    case 2: //Error de Onedrive
                        string lstErrorColores = @"";
                        if (listaErr != null)
                        {
                            foreach (var err in listaErr)
                            {
                                lstErrorColores = lstErrorColores + string.Format("- {0} <br />", err.Descripcion);
                            }
                        }
                        return lstErrorColores;
                }

                return string.Format(HtmlNotificacion.HtmlCuerpoAlerta, tableVisible, htmlElemento.ToString(), HtmlNotificacion.HtmlCss);
            }

            return "";
        }

        #endregion
    }

    public class ErrorHtrabajo
    {
        public string Tipo { get; set; }
        public int Numero { get; set; }
        public string Descripcion { get; set; }
        public string Central { get; set; }
        public string MediaHora { get; set; }
        public string Valor { get; set; }
        public string Fuente { get; set; }
        public string Ptomedicion { get; set; }
        public string Posicion { get; set; }
    }
}
