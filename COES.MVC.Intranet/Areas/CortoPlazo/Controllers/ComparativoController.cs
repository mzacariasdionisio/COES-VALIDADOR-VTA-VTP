using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ComparativoController : BaseController
    {
        private readonly ComparativoAppServicio servicio = new ComparativoAppServicio();


        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public ComparativoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Comparar datos HOP vs Despacho

        public ActionResult IndexHOvsDespacho()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ComparativoModel model = new ComparativoModel();

            DateTime fechaConsulta = DateTime.Today;
            model.FechaPeriodo = fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);

            servicio.ListarFiltroHOvsDespacho(fechaConsulta, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, out List<SiEmpresaDTO> listaEmpresa
                                                        , out List<EqEquipoDTO> listaCentral, out List<PrGrupoDTO> listaModo);

            model.ListaEmpresa = listaEmpresa;
            model.ListaCentral = listaCentral;
            model.ListaModo = new List<PrGrupoDTO>();

            return View(model);
        }

        /// <summary>
        /// Carga los filtros de empresa, central y modos
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equipadre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltroHOvsDespacho(string strFecha, int emprcodi, int equipadre)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.ListarFiltroHOvsDespacho(fechaPeriodo, emprcodi.ToString(), equipadre.ToString(), out List<SiEmpresaDTO> listaEmpresa
                                                            , out List<EqEquipoDTO> listaCentral, out List<PrGrupoDTO> listaModo);

                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
                model.ListaModo = listaModo;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el reporte web del comparativo HO vs Despacho
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarComparativoHOvsDespacho(string strFecha, int equipadre, int grupocodi)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaReporte = servicio.GenerarReporteWebHOvsDespacho(ConstantesCortoPlazo.TipoCompHOvsDesp, fechaPeriodo, -1, equipadre, grupocodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera el reporte excel del comparativo HO vs Despacho
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelComparativoHOvsDespacho(string strFecha, int equipadre, int grupocodi)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servicio.GenerarExcelComparativoHOvsDespacho(ruta, ConstantesCortoPlazo.TipoCompHOvsDesp, fechaPeriodo, -1, equipadre, grupocodi, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        #endregion

        #region Comparar datos Generación EMS vs Despacho Ejecutado

        public ActionResult IndexEMSvsDespacho()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ComparativoModel model = new ComparativoModel();

            DateTime fechaConsulta = DateTime.Today;
            model.FechaPeriodo = fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaIni = fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);

            servicio.ListarFiltroEMSvsDespacho(fechaConsulta, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto, out List<SiEmpresaDTO> listaEmpresa
                                                        , out List<PrGrupoDTO> listaCentral, out List<PrGrupoDTO> listaGrupo, out List<string> listaVal);

            model.ListaEmpresa = listaEmpresa;
            model.ListaGrupoCentral = listaCentral;
            model.ListaGrupoDespacho = new List<PrGrupoDTO>();
            model.ListaMensajeValidacion = listaVal;

            return View(model);
        }

        /// <summary>
        /// Carga los filtros de empresa, central y grupos
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equipadre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltroEMSvsDespacho(string strFecha, int emprcodi, int grupopadre)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.ListarFiltroEMSvsDespacho(fechaPeriodo, emprcodi.ToString(), grupopadre.ToString(), out List<SiEmpresaDTO> listaEmpresa
                                                            , out List<PrGrupoDTO> listaCentral, out List<PrGrupoDTO> listaGrupo, out List<string> listaVal);

                model.ListaEmpresa = listaEmpresa;
                model.ListaGrupoCentral = listaCentral;
                model.ListaGrupoDespacho = listaGrupo;

                model.ListaMensajeValidacion = listaVal;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el reporte web del comparativo EMS vs Despacho
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarComparativoEMSvsDespacho(string strFecha, int emprcodi, int grupopadre, int grupocodi)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (grupocodi <= 0)
                    throw new ArgumentException("Debe seleccionar un grupo de despacho.");

                model.ListaReporte = servicio.GenerarReporteWebHOvsDespacho(ConstantesCortoPlazo.TipoCompEMSvsDesp, fechaPeriodo, emprcodi, grupopadre, grupocodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera el reporte excel del comparativo EMS vs Despacho
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelComparativoEMSvsDespacho(string strFecha, int emprcodi, int grupopadre, int grupocodi)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (grupocodi <= 0)
                    throw new ArgumentException("Debe seleccionar un grupo de despacho.");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                servicio.GenerarExcelComparativoHOvsDespacho(ruta, ConstantesCortoPlazo.TipoCompEMSvsDesp, fechaPeriodo, emprcodi, grupopadre, grupocodi, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el reporte excel de diferencias EMS vs Despacho
        /// </summary>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelDiferenciaEMSvsDespacho(string strFechaIni, string strFechaFin)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodoIni = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaPeriodoFin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (!(fechaPeriodoIni <= fechaPeriodoFin && fechaPeriodoIni > fechaPeriodoFin.AddDays(-31)))
                {
                    throw new ArgumentException("Solo puede consultar como maximo un rango de fechas de 31 dias");
                }

                if (fechaPeriodoIni > DateTime.Today) 
                {
                    throw new ArgumentException("Debe seleccionar fechas hasta a la fecha actual.");
                }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servicio.GenerarExcelDiferenciaEMSvsDespacho(ruta, fechaPeriodoIni, fechaPeriodoFin, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #region Comparar Demanda EMS vs Demanda Ejecutada

        /// <summary>
        /// Permite obtener el comparativo Demanda EMS - Demanda Ejecutada
        /// </summary>
        /// <returns></returns>
        public ActionResult DemandaEMS() 
        {
            ComparativoModel model = new ComparativoModel();
            model.ListaBarra = this.servicio.ObtenerBarrasEMS();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaPeriodo = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite obtener la data del comparativo 
        /// </summary>
        /// <param name="barra"></param>       
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComparativoDemandaEMS(int idBarra, string fecha, string option)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteComparativoDemanda;
            bool boolExportar = (option == Constantes.SI) ? true : false;

            return Json(this.servicio.ObtenerComparativoDemanda(idBarra, DateTime.ParseExact(fecha, 
                Constantes.FormatoFecha, CultureInfo.InvariantCulture), boolExportar, path, file));
        }              

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarDemandaEMS(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteComparativoDemanda;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteComparativoDemanda, fecha));            
        }

        /// <summary>
        /// Permite generar el reporte masivo de comparativo Demanda
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDemandaEMSMasivo(string fechaInicio, string fechaFin) 
        {
            DateTime fechaDesde = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaHasta = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteComparativoDemandaMasivo;

            return Json(this.servicio.GenerarMasivoComparativoDemanda(fechaDesde, fechaHasta, path, file));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarDemandaEMSMasivo(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteComparativoDemandaMasivo;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteComparativoDemandaMasivo, fecha));
        }

        #endregion

        #region Comparar CM Aplicativo vs CM Programado vs CI Tiempo Real

        /// <summary>
        /// Permite obtener el comparativo Demanda EMS - Demanda Ejecutada
        /// </summary>
        /// <returns></returns>
        public ActionResult CostosMarginales()
        {
            ComparativoModel model = new ComparativoModel();
            model.ListaBarra = this.servicio.ObtenerBarrasEMS();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);         
            return View(model);
        }

        /// <summary>
        /// Permite obtener la data del comparativo 
        /// </summary>
        /// <param name="barra"></param>       
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComparativoCostosMarginales(int idBarra, string fecha, string option)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteComparativoCostoMarginal;
            bool boolExportar = (option == Constantes.SI) ? true : false;

            return Json(this.servicio.ObtenerComparativoCostosMarginales(idBarra, DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture), boolExportar, path, file));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarCostosMarginales(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteComparativoCostoMarginal;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteComparativoCostoMarginal, fecha));
        }
        #endregion

        #region Comparar registro de congestiones 

        public ActionResult Congestiones()
        {
            ComparativoModel model = new ComparativoModel();          
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);           
            return View(model);
        }

        /// <summary>
        /// Permite obtener la data del comparativo de congestiones
        /// </summary>       
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComparativoCongestiones(string fecha, string option)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteComparativoCongestiones;
            bool boolExportar = (option == Constantes.SI) ? true : false;

            return Json(this.servicio.ObtenerComparativoCongestiones(DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture), boolExportar, path, file));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarComparativoCongestiones(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteComparativoCongestiones;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteComparativoCongestiones, fecha));
        }

        /// <summary>
        /// Descarga del archivo de resultados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoResultado(string fecha, int correlativo)
        {
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

            string path = fechaConsulta.Year + @"\" +
                          fechaConsulta.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                          fechaConsulta.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\Corrida_" + correlativo + @"\";

            List<FileData> files = FileServerScada.ListarArhivos(path, pathTrabajo);

            FileData dat = files.Where(x => x.FileName.Contains("RESULTADO_GAMS_ANALISIS")).FirstOrDefault();

            if (dat != null)
            {
                path = path + dat.FileName;
                return File(FileServerScada.DownloadToArrayByte(path, pathTrabajo), Constantes.AppCSV, dat.FileName);
            }

            return null;
        }

        #endregion

        #region Comparar costos incrementales

        public ActionResult CostosIncrementales()
        {
            ComparativoModel model = new ComparativoModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            CmUmbralComparacionDTO umbral = this.servicio.ObtenerParametros();
            decimal valor = 0;

            if (umbral != null)
            {
                if (umbral.Cmumcoci != null) valor = (decimal)umbral.Cmumcoci;
            }
            model.UmbralCI = valor;
            return View(model);
        }

        /// <summary>
        /// Permite obtener la data del comparativo de congestiones
        /// </summary>       
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ComparativoCostosIncrementales(string fecha, string umbral, int equipo, int hora, string option)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteComparativoCostoIncremental;
            bool boolExportar = (option == Constantes.SI) ? true : false;

            decimal umbralCI = (!string.IsNullOrEmpty(umbral)) ? decimal.Parse(umbral) : 0;

            return Json(this.servicio.ObtenerComparativoCostoIncremental(DateTime.ParseExact(fecha,
                Constantes.FormatoFecha, CultureInfo.InvariantCulture), umbralCI, equipo, hora, boolExportar, path, file));
        }

        /// <summary>
        /// Permite obtener los filtros del comparativo de CI
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFiltrosCI(string fecha)
        {
            return Json(this.servicio.ObtenerFiltrosCI(DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarComparativoCostosIncrementales(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteComparativoCostoIncremental;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteComparativoCostoIncremental, fecha));
        }

        #endregion

        #region Comparar Horas de Operación vs Reserva Secundaria

        public ActionResult IndexHOvsRsvaSec()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ComparativoModel model = new ComparativoModel();

            DateTime fechaConsulta = DateTime.Today;
            model.FechaPeriodo = fechaConsulta.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Genera el reporte web del comparativo Horas de Operación VS Reserva Secundaria
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarComparativoHOvsRsvaSec(string strFecha)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaRsvaSec = servicio.GenerarReporteWebHOvsRsvaSec(fechaPeriodo, out int numInconsistencia);
                model.Resultado = numInconsistencia.ToString();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera el reporte excel del comparativo Horas de Operación VS Reserva Secundaria
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="equipadre"></param>
        /// <param name="grupocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarExcelComparativoHOvsRsvaSec(string strFecha)
        {
            ComparativoModel model = new ComparativoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servicio.GenerarExcelComparativoHOvsRsvaSec(ruta, fechaPeriodo, out string nameFile);

                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

    }
}
