using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Combustibles.Controllers
{
    public class ReporteGasController : BaseController
    {
        CombustibleAppServicio servicioCombustible = new CombustibleAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReporteGasController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            CombustibleGasModel model = new CombustibleGasModel();

            return View(model);
        }

        #region reporte histórico de precios, costos y poder calorífico
        public ActionResult IndexHistorico()
        {
            base.ValidarSesionJsonResult();

            CombustibleGasModel model = new CombustibleGasModel();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime(hoy.Year, hoy.Month, 1).ToString(ConstantesAppServicio.FormatoFecha));
            model.FechaFin = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);
            model.ListadoCentrales = servicioCombustible.ObtenerListadoCentralesTermicas(false).DistinctBy(x => x.Equicodi).ToList();
            model.ListadoEstados = servicioCombustible.ListExtEstadoEnvio(ConstantesCombustibles.CombustiblesGaseosos);

            return View(model);

        }

        public ActionResult IndexHistoricoConsulta()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CombustibleGasModel model = new CombustibleGasModel();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime(hoy.Year, hoy.Month, 1).ToString(ConstantesAppServicio.FormatoFecha));
            model.FechaFin = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);
            model.ListadoCentrales = servicioCombustible.ObtenerListadoCentralesTermicas(false).DistinctBy(x => x.Equicodi).ToList();
            model.ListadoEstados = servicioCombustible.ListExtEstadoEnvio(ConstantesCombustibles.CombustiblesGaseosos);

            return View(model);

        }

        [HttpPost]
        public JsonResult ListarCbCentralXEmpresa(List<string> idEmpresa)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            base.ValidarSesionUsuario();

            string listaEmpresas = string.Join(",", idEmpresa.ToArray());
            listaEmpresas = String.IsNullOrEmpty(listaEmpresas) ? "0" : listaEmpresas;
            List<int> listaEmprcodi = listaEmpresas.Split(',').Select(x => int.Parse(x)).ToList();

            model.ListadoCentrales = servicioCombustible.ObtenerListadoCentralesTermicas(false).DistinctBy(x => x.Equicodi).ToList();
            model.ListadoCentrales = model.ListadoCentrales.Where(x => listaEmprcodi.Contains(x.Emprcodi)).ToList();

            return Json(model);
        }

        [HttpPost]
        public JsonResult ListarReporteHistorico(string centrales, int tipoReporte, string finicio, string ffin)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaInicio;
                DateTime fechaFin;

                if (tipoReporte == ConstantesCombustibles.ReporteCV_S || tipoReporte == ConstantesCombustibles.ReporteCV_USD)
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                else
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    //fechaFin = fechaFin.AddMonths(1).AddDays(-1);
                }

                //empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                var html = servicioCombustible.ObteneterReporteHistorico(centrales, tipoReporte, fechaInicio, fechaFin);

                model.Resultado = html;
                //model.Grafico1 = grafico1;
                //model.Grafico2 = grafico2;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpPost]
        public JsonResult GenerarReporteExcelHistorico(string centrales, int tipoReporte, string finicio, string ffin)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaInicio;
                DateTime fechaFin;

                if (tipoReporte == ConstantesCombustibles.ReporteCV_S || tipoReporte == ConstantesCombustibles.ReporteCV_USD)
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                else
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                }

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                //string nameFile = "ReporteCostosVariables.xlsx";

                servicioCombustible.DescargarReportesHistorico(ruta, pathLogo, centrales, tipoReporte, fechaInicio, fechaFin, out string nameFile);
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


        [HttpPost]
        public JsonResult GenerarGrafico(string centrales, int tipoReporte, string finicio, string ffin, int opcion)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();
                DateTime fechaInicio;
                DateTime fechaFin;

                if (tipoReporte == ConstantesCombustibles.ReporteCV_S || tipoReporte == ConstantesCombustibles.ReporteCV_USD)
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                else
                {
                    fechaInicio = DateTime.ParseExact(finicio, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    fechaFin = DateTime.ParseExact(ffin, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                    //fechaFin = fechaFin.AddMonths(1).AddDays(-1);
                }

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                servicioCombustible.ObteneterGraficoHistorico(centrales, tipoReporte, fechaInicio, fechaFin, opcion, out GraficoWeb grafico1, out GraficoWeb grafico2);

                model.Resultado = "1";
                model.Grafico1 = grafico1;
                model.Grafico2 = grafico2;
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

        #region Reporte histórico de envios Extranet

        public ActionResult IndexEnvios()
        {
            base.ValidarSesionJsonResult();

            CombustibleGasModel model = new CombustibleGasModel();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime((hoy.AddMonths(-1)).Year, (hoy.AddMonths(-1)).Month, 1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = hoy.ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);

            return View(model);

        }

        [HttpPost]
        public JsonResult ListarReporteEnvios(string empresas, string finicios, string ffins)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                model.ListadoEnvios = servicioCombustible.ObtenerListadoHistoricoEnviosExtranet(empresas, ConstantesCombustibles.PorDefecto, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesGaseosos);

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

        [HttpPost]
        public JsonResult GenerarReporteEnvios(string empresas, string finicios, string ffins)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "rptEnviosHistoricoExtranet.xlsx";

                servicioCombustible.ExportacionHistoricoExtranet(ruta, pathLogo, empresas, fechaInicio, fechaFin, -1, nameFile);
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
        /// Permite descargar formato3 e informes sustentatorios en archivo comprimido
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public JsonResult ExportarFormato3eInfSusXVersion(int idEnvio, int idVersion)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //DateTime fecha = DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);

                servicioCombustible.ExportarFormato3InfSustXVersion(GetCurrentCarpetaSesion(), idEnvio, idVersion, base.UserName, out string nameFile);
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
        /// exportar comprimido
        /// </summary>
        /// <returns></returns>
        public virtual FileResult ExportarZip()
        {
            string nombreArchivo = Request["file_name"];

            string modulo = ConstantesCombustibles.ModuloArchivosXEnvio;
            string pathDestino = ConstantesCombustibles.FolderRaizPR31Gaseoso + "Temporal_" + modulo + @"/" + ConstantesCombustibles.NombreArchivosZip;
            string pathAlternativo = servicioCombustible.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        #endregion

        #region Reporte Ampliación plazo

        public ActionResult IndexAmpliacion()
        {
            base.ValidarSesionJsonResult();

            CombustibleGasModel model = new CombustibleGasModel();

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime((hoy.AddMonths(-1)).Year, (hoy.AddMonths(-1)).Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.FechaFin = (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);

            return View(model);

        }

        [HttpPost]
        public JsonResult ListarReporteAmpliacion(string empresas, string finicios, string ffins)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio > fechaFin)
                    throw new ArgumentException("La fecha inicio no puede ser mayor a la final.");

                model.ListadoEnvios = servicioCombustible.ObtenerListadoEnvios(empresas, ConstantesCombustibles.PorDefecto, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesGaseosos);

                model.ListadoEnvios = model.ListadoEnvios.Where(x => x.Cbenvfecampl != null).ToList(); // solo los que ampliados

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


        #endregion

        #region reporte por Participante Generador

        public ActionResult IndexParticipante()
        {
            CombustibleGasModel model = new CombustibleGasModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime((hoy.AddMonths(-1)).Year, (hoy.AddMonths(-1)).Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.FechaFin = (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);
            model.ListadoEstados = servicioCombustible.ListExtEstadoEnvio(ConstantesCombustibles.CombustiblesGaseosos);

            return View(model);

        }


        public JsonResult ExportarReporteParticipanteGen(string empresas, string estados, string finicios, string ffins, int tipoArchivo)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName) && tipoArchivo != 2) throw new Exception(Constantes.MensajePermisoNoValido);

                var fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);

                servicioCombustible.ExportarReporteParticipanteGen(GetCurrentCarpetaSesion(), empresas, estados, fechaInicio, fechaFin, tipoArchivo, out string nameFile);
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

        public virtual FileResult ExportarZipParticipanteGen()
        {
            string nombreArchivo = Request["file_name"];

            string modulo = ConstantesCombustibles.ModuloArchivosParticipanteGen;
            string pathDestino = ConstantesCombustibles.FolderRaizPR31Gaseoso + "Temporal_" + modulo + @"/" + ConstantesCombustibles.NombreArchivosZip;
            string pathAlternativo = servicioCombustible.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        #endregion

        private string GetCurrentCarpetaSesion()
        {
            return base.UserEmail;
        }

        #region reporte mensual de costo variable

        /// <summary>
        /// Pagina principal del reporte
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCostoVariable()
        {
            CombustibleGasModel model = new CombustibleGasModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            model.FechaActual = DateTime.Today.ToString(ConstantesAppServicio.FormatoMesAnio);

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime((hoy.AddMonths(-1)).Year, (hoy.AddMonths(-1)).Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.FechaFin = (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);// cambiar por empresas del mes vigencia
            model.MesAnio = EPDate.f_NombreMes(DateTime.Today.Month) + " " + DateTime.Today.Year;

            return View(model);
        }

        /// <summary>
        /// Devuelve los reportes guardados para cierto mes de vigencia
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerReportesGuardados(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                //CUADRO 1
                model.Data = servicioCombustible.ObtenerReporteCVGuardado(ConstantesCombustibles.TipoReporteCVCuadro1, mesDeVigencia, null, out int reportecodiC1, out string nombreReporteC1, out string notasC1, out string filasPintadas1);
                model.ReporteCodiC1 = reportecodiC1;
                model.NombreReporteC1 = nombreReporteC1;
                model.NotaRC1 = notasC1;

                //CUADRO 2
                model.Data2 = servicioCombustible.ObtenerReporteCVGuardado(ConstantesCombustibles.TipoReporteCVCuadro2, mesDeVigencia, null, out int reportecodiC2, out string nombreReporteC2, out string notasC2, out string filasPintadas2);
                model.ReporteCodiC2 = reportecodiC2;
                model.NombreReporteC2 = nombreReporteC2;
                model.NotaRC2 = notasC2;

                //CUADRO 3
                model.Data3 = servicioCombustible.ObtenerReporteCVGuardado(ConstantesCombustibles.TipoReporteCVCuadro3, mesDeVigencia, null, out int reportecodiC3, out string nombreReporteC3, out string notasC3, out string filasPintadas3);
                model.ReporteCodiC3 = reportecodiC3;
                model.NombreReporteC3 = nombreReporteC3;
                model.NotaRC3 = notasC3;
                model.ListaFilasPintar = filasPintadas3;

                //CUADRO 4
                string[][] Data4 = servicioCombustible.ObtenerReporteCVGuardado(ConstantesCombustibles.TipoReporteCVCVC, mesDeVigencia, null, out int reportecodiC4, out string nombreReporteC4, out string notasC4, out string filasPintadas4);
                model.NombreReporteC4 = nombreReporteC4;
                model.MesAnio = EPDate.f_NombreMes(mesDeVigencia.Month) + " " + mesDeVigencia.Year;

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
        /// Obtiene un reporte por codigo
        /// </summary>
        /// <param name="cbrepcodi"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult obtenerReportePorCodigo(int cbrepcodi, int tipoReporte)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime mesDeVigencia = DateTime.Now;

                if (tipoReporte == ConstantesCombustibles.TipoReporteCVCuadro1 || tipoReporte == ConstantesCombustibles.TipoReporteCVCuadro2)
                {
                    model.Data = servicioCombustible.ObtenerReporteCVGuardado(tipoReporte, mesDeVigencia, cbrepcodi, out int reportecodiC1, out string nombreReporteC1, out string notasC1, out string filasPintadas1);
                    model.ReporteCodiC1 = reportecodiC1;
                    model.NombreReporteC1 = nombreReporteC1;
                    model.NotaRC1 = notasC1;
                }
                else
                {
                    if (tipoReporte == ConstantesCombustibles.TipoReporteCVCuadro3)
                    {
                        model.Data = servicioCombustible.ObtenerReporteCVGuardado(tipoReporte, mesDeVigencia, cbrepcodi, out int reportecodiC1, out string nombreReporteC1, out string notasC1, out string filasPintadas1);
                        model.ReporteCodiC1 = reportecodiC1;
                        model.NombreReporteC1 = nombreReporteC1;
                        model.NotaRC1 = notasC1;
                        model.ListaFilasPintar = filasPintadas1;
                    }
                    else
                    {
                        if (tipoReporte == ConstantesCombustibles.TipoReporteCVCVC)
                        {
                            CbReporteDTO reporte = servicioCombustible.GetByIdCbReporte(cbrepcodi);
                            string[][] Data4 = servicioCombustible.ObtenerReporteCVGuardado(ConstantesCombustibles.TipoReporteCVCVC, mesDeVigencia, cbrepcodi, out int reportecodiC4, out string nombreReporteC4, out string notasC4, out string filasPintadas4);
                            model.NombreReporteC1 = nombreReporteC4;
                            model.MesAnio = EPDate.f_NombreMes(reporte.Cbrepmesvigencia.Value.Month) + " " + reporte.Cbrepmesvigencia.Value.Year;
                        }
                    }
                }
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
        /// Procesa el cuadro 1 con la data actual de la BD
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult procesarReporteC1(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                model.Data = servicioCombustible.ObtenerDataReprocesoReporteC1(mesDeVigencia, out string nombreReporteC1, out string notasC1);
                model.ExisteVersion1 = servicioCombustible.VerificarExistenciaVersion1(ConstantesCombustibles.TipoReporteCVCuadro1, mesDeVigencia);
                model.NombreReporteC1 = nombreReporteC1;
                model.NotaRC1 = notasC1;

                if (model.Data != null)
                {

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "2";
                }

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
        /// Procesa el cuadro 2 con la data actual de la BD
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult procesarReporteC2(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                model.Data2 = servicioCombustible.ObtenerDataReprocesoReporteC2(mesDeVigencia, out string nombreReporteC2, out string notasC2);
                model.ExisteVersion1 = servicioCombustible.VerificarExistenciaVersion1(ConstantesCombustibles.TipoReporteCVCuadro2, mesDeVigencia);
                model.NombreReporteC2 = nombreReporteC2;
                model.NotaRC2 = notasC2;

                if (model.Data2 != null)
                {

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "2";
                }

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
        /// Procesa el cuadro 3 con la data actual de la BD
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult procesarReporteC3(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                model.Data3 = servicioCombustible.ObtenerDataReprocesoReporteC3(mesDeVigencia, out string nombreReporteC3, out string notasC3, out string filasPintadas);
                model.ExisteVersion1 = servicioCombustible.VerificarExistenciaVersion1(ConstantesCombustibles.TipoReporteCVCuadro3, mesDeVigencia);
                model.NombreReporteC3 = nombreReporteC3;
                model.NotaRC3 = notasC3;
                model.ListaFilasPintar = filasPintadas;

                if (model.Data3 != null)
                {

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "2";
                }

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
        /// Devuelve el listado de notas para el reporte (si idreporte = 0 (reproceso) entonces devuelve la nota del ultimo reporte guardado, si no hay devuelve vacio)
        /// </summary>
        /// <param name="codigoReporte"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerNota(int codigoReporte, int tipoReporte, string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                model.Data = servicioCombustible.ObtenerDataNota(codigoReporte, tipoReporte, mesDeVigencia);

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
        ///Descarga el reporte de CV para cierto mes de vigencia
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteCV(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                string nameFile = servicioCombustible.ExportarTodosReportesMensualCV(ruta, mesDeVigencia);
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
        /// Guarda reporte mensual de CV para cierto cuadro
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="notas"></param>
        /// <param name="nombReporte"></param>
        /// <param name="tipoReporte"></param>
        /// <param name="strMesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarReporte(string[][] datos, string notas, string nombReporte, int tipoReporte, string strMesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();
                string usuario = base.UserName;
                if (!base.VerificarAccesoAccion(Acciones.Grabar, usuario)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = strMesVigencia.Split('-')[0];
                var anio = strMesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                int cbrepcodi = servicioCombustible.GuardarReporte(usuario, datos, notas, nombReporte, tipoReporte, mesDeVigencia);

                model.IdReporte = cbrepcodi;
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
        /// Devuelve el html del historial de cambios
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerHistorial(int tipoReporte, string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                string url = Url.Content("~/");
                model.HtmlHistorial = servicioCombustible.ObtenerHtmlHistorial(url, tipoReporte, mesDeVigencia);

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
        /// Exporta formato 3 e informes sustentarios
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        public JsonResult ExportarF3InfSustXEmpresas(string empresas, string finicios, string ffins)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                var fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);

                servicioCombustible.ExportarFormato3InfSustXEmpresas(GetCurrentCarpetaSesion(), empresas, fechaInicio, fechaFin, out string nameFile);
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
        /// Muestra las tablas de centrales y de costos variables
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosCarga(string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                string url = Url.Content("~/");
                List<CbReporteCentralDTO> lstCentralesCargaBD = servicioCombustible.ObtenerCentralesCargaBD(mesDeVigencia);
                model.HtmlCentrales = servicioCombustible.ObtenerHtmlCentralesCargaBD(lstCentralesCargaBD);
                model.HtmlListado = servicioCombustible.GenerarHtmlListaCostosVariable();

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
        /// Guarda en BD la informacion de la ventana emergente
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <param name="listaCodicocv"></param>
        /// <param name="lstCentrales"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult cargarABaseDeDatos(string mesVigencia, List<int> listaCodicocv, List<int> lstCentrales)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var mes = mesVigencia.Split('-')[0];
                var anio = mesVigencia.Split('-')[1];
                DateTime mesDeVigencia = new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), 1);

                servicioCombustible.CargarABaseDatos(mesDeVigencia, base.UserName, listaCodicocv, lstCentrales);

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

        #endregion

        #region Reporte de Cumplimiento

        /// <summary>
        /// Pagina principal del reporte
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCumplimiento()
        {
            CombustibleGasModel model = new CombustibleGasModel();

            model.FechaActual = DateTime.Today.ToString(ConstantesAppServicio.FormatoMesAnio);

            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.FechaFin =    (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);

            return View(model);

        }

        /// <summary>
        /// Devuelve el listado de log de los envios por emrpesa y rango
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListadoLogEnvios(string empresas, string rangoIni, string rangoFin)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(rangoIni, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);

                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                List<CumplimientoEmpresa> ListadoLogEnvios = servicioCombustible.ObtenerListadoLogEnviosGaseosos(empresas, fechaInicio, fechaFin);
                model.NumRegistros = ListadoLogEnvios.Count;
                model.HtmlLogEnvios = servicioCombustible.GenerarHtmlLogEnvios(ListadoLogEnvios);
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
        ///Descarga el reporte de Cumplimiento para cierta empresa y rango
        /// </summary>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteCumplimiento(string empresas, string rangoIni, string rangoFin)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaInicio = DateTime.ParseExact(rangoIni, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                List<CumplimientoEmpresa> ListadoLogEnvios = servicioCombustible.ObtenerListadoLogEnviosGaseosos(empresas, fechaInicio, fechaFin);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                string nameFile = servicioCombustible.ExportarReporteCumplimiento(ruta, ListadoLogEnvios, empresas, rangoIni, rangoFin);
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