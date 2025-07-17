using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using System.IO;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class FactorUtilizacionController : BaseController
    {
        private readonly CostoOportunidadAppServicio costoOpServicio = new CostoOportunidadAppServicio();
        private readonly ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient coService = new ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FactorUtilizacionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
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

        #region Métodos 

        /// <summary>
        /// Index Factor de Utilizacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FactorUtilizacionModel model = new FactorUtilizacionModel();

            model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

            model.FechaIni= DateTime.Now.AddDays(-15).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaProcesoManual = DateTime.Now.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
            

            return View(model);
        }

        /// <summary>
        /// Devuelve el listado de factores de utilización en formato html
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarFactoresUtilizacion(string fecIni, string fecFin)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();
            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");

                DateTime fechaIni = DateTime.ParseExact(fecIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fecFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                model.Resultado = costoOpServicio.GenerarHtmlFactoresUtilizacion(url, fechaIni, fechaFin, out bool hayDiasConErrres);
                model.MostrarBtnRT = hayDiasConErrres;
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
        /// Reprocesa para todos los dias con errores
        /// </summary>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarTodos(string fecIni, string fecFin)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaIni = DateTime.ParseExact(fecIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fecFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //costoOpServicio.ReprocesarCalculoTodos(fechaIni, fechaFin, base.UserName);
                coService.ReprocesarCalculoTodos(fechaIni, fechaFin, base.UserName);

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
        /// Reprocesa calculo para cierto dia
        /// </summary>
        /// <param name="prodiacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarCalculo(int prodiacodi)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                CoProcesoDiarioDTO objProcesoD = costoOpServicio.GetByIdCoProcesoDiario(prodiacodi);
                //costoOpServicio.EjecutarProcesoDiario(objProcesoD.Prodiafecha.Value.Date, ConstantesCostoOportunidad.TipoReproceso, base.UserName);
                int hayProcesoEnCurso = coService.EjecutarProcesoDiario(objProcesoD.Prodiafecha.Value.Date, ConstantesCostoOportunidad.TipoReproceso, base.UserName);
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
        /// Reprocesa calculo para cierto dia
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarManualmente(string fecha)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<CoVersionDTO> lstVersionesPorMes = costoOpServicio.ObtenerVersionesPorMes(fechaConsulta.Month, fechaConsulta.Year);
                
                if (lstVersionesPorMes.Any())
                {
                    List<CoVersionDTO> lstVersionesConPeriodoActivo = lstVersionesPorMes.Where(x => x.Coperestado == "A").ToList();
                    if (lstVersionesConPeriodoActivo.Any())
                    {
                        List<CoVersionDTO> lstVersionesConPeriodoYEstadoActivo = lstVersionesConPeriodoActivo.Where(x => x.Coverestado == "A").OrderByDescending(x => x.Covercodi).ToList();

                        if (lstVersionesConPeriodoYEstadoActivo.Any())
                        {
                            CoVersionDTO ultimaVersionActiva = lstVersionesConPeriodoYEstadoActivo.First();

                            //Verifico que la fecha este en el rango de la version
                            DateTime rangoIniVersionActiva = ultimaVersionActiva.Coverfecinicio.Value;
                            DateTime rangoFinVersionActiva = ultimaVersionActiva.Coverfecfin.Value;

                            int r1 = DateTime.Compare(rangoIniVersionActiva, fechaConsulta);
                            int r2 = DateTime.Compare(fechaConsulta, rangoFinVersionActiva);

                            //si esta en el rango
                            if(r1 <= 0 && r2 <= 0)
                            {
                                //costoOpServicio.EjecutarProcesoDiario(fechaConsulta.Date, ConstantesCostoOportunidad.TipoManual, base.UserName);
                                int hayProcesoEnCurso = coService.EjecutarProcesoDiario(fechaConsulta.Date, ConstantesCostoOportunidad.TipoManual, base.UserName);

                                if (hayProcesoEnCurso == 1)
                                {
                                    model.Resultado = "2";
                                }
                                else
                                {
                                    model.Resultado = "1";
                                }
                            }
                            else
                            {
                                //si no esta en el rango, verifico que haya una version donde si este
                                List<CoVersionDTO> lstVersionesEstadoCerrado = lstVersionesConPeriodoActivo.Where(x => x.Coverestado == "C" && x.Coverfecinicio.Value.Date <= fechaConsulta.Date && x.Coverfecfin.Value.Date >= fechaConsulta.Date).OrderByDescending(x => x.Covercodi).ToList();

                                if (lstVersionesEstadoCerrado.Any())
                                {
                                    model.Mensaje = "La fecha seleccionada se encuentra dentro de una versión cerrada.";
                                    model.Resultado = "-2";
                                }
                                else
                                {
                                    model.Mensaje = "La fecha seleccionada se encuentra fuera del rango de la última versión vigente del periodo.";
                                    model.Resultado = "-2";
                                }
                            }
                        }
                        else
                        {
                            model.Mensaje = "No existe ninguna versión activa para la fecha seleccionada.";
                            model.Resultado = "-2";
                        }
                    }
                    else
                    {
                        model.Mensaje = "No existe periodo activo para la fecha seleccionada.";
                        model.Resultado = "-2";
                    }
                }
                else
                {
                    model.Mensaje = "No existen versiones registradas para la fecha seleccionada.";
                    model.Resultado = "-2";
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
        /// Muestra reporte de errores
        /// </summary>
        /// <param name="prodiacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarReporteError(int prodiacodi)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                CoProcesoDiarioDTO procDiario = costoOpServicio.GetByIdCoProcesoDiario(prodiacodi);
                string nameFile = string.Format("ReporteErrores_FactoresUtilizacion_{0}{1}{2}.xlsx", string.Format("{0:D2}", procDiario.Prodiafecha.Value.Day), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Month), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Year));

                costoOpServicio.GenerarReporteDeErrores(ruta, prodiacodi, nameFile);
                model.HtmlErrores = costoOpServicio.GenerarHtmlReporteDeErrores( prodiacodi);
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
        /// Genera reporte de resultados
        /// </summary>
        /// <param name="prodiacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteResultados(int prodiacodi)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                CoProcesoDiarioDTO procDiario = costoOpServicio.GetByIdCoProcesoDiario(prodiacodi);
                string nameFile = string.Format("FactoresUtilizacion_{0}{1}{2}.xlsx", string.Format("{0:D2}", procDiario.Prodiafecha.Value.Day), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Month), string.Format("{0:D2}", procDiario.Prodiafecha.Value.Year));

                costoOpServicio.GenerarReporteDeResultados(ruta, procDiario, nameFile);
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
        /// Exporta informacion a un archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            System.IO.File.Delete(ruta + nombreArchivo);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Reemplaza los valores de factorres de utilizacion manualmente
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProcesarReemplazo(FormCollection formCollection)
        {
            FactorUtilizacionModel model = new FactorUtilizacionModel();
            try
            {
                base.ValidarSesionJsonResult();

                var fileName = formCollection["name"];
                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                if (stremExcel != null)
                {
                    List<FactorUtilizacionExcel> listaDataExcel = new List<FactorUtilizacionExcel>();
                    List<FactorUtilizacionErrorExcel> listaDataExcelErrores = new List<FactorUtilizacionErrorExcel>();
                    
                    try
                    {
                        this.costoOpServicio.ObtenerDataExcel(stremExcel,  User.Identity.Name, out listaDataExcel, out listaDataExcelErrores);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(NameController, ex);
                        if(ex.Message.Contains("mal posicionada"))
                            throw new Exception("El contenido del archivo Excel no es correcto. " + ex.Message + ".");
                        else
                            throw new Exception("Contenido del archivo incorrecto. El archivo solo debe contener una tabla con 3 columnas en el siguiente orden: Fecha y Hora, valor Alpha, valor Beta");
                    }

                    //Guardar en BD si el archivo está correcto
                    if (listaDataExcel.Any() && listaDataExcelErrores.Count == 0)
                    {                        
                        costoOpServicio.ReemplazarValoresFUManualmente(listaDataExcel, User.Identity.Name);                        
                        model.Resultado = "1";
                    }

                    if (listaDataExcelErrores.Any())
                        model.Resultado = this.costoOpServicio.ObtenerTablaErroresHtml(listaDataExcelErrores);
                }
            }
            catch (Exception ex)
            {
                model.Mensaje = "Error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}