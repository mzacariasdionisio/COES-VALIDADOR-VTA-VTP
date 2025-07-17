using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using COES.Servicios.Aplicacion.PMPO.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ParametrosFechasController : BaseController
    {
        private readonly ProgramacionAppServicio pmpoServicio = new ProgramacionAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametrosFechasController));
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
        /// Index Parámetros Fechas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ParametrosFechasModel model = new ParametrosFechasModel();

            model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaIniRango = DateTime.Now.Year.ToString();
            model.FechaFinRango = (DateTime.Now).AddYears(3).Year.ToString();

            return View(model);
        }

        /// <summary>
        /// Devuelve el html del listado de anios
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarAniosOperativos()
        {
            ParametrosFechasModel model = new ParametrosFechasModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.HtmlListadoAniosOp = pmpoServicio.HtmlListadoAnioOperativo(url, tienePermiso);

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
        /// Deveulve el html de la ventana de versiones de cierto anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int anio)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = pmpoServicio.GenerarHtmlVersionesAnio(url, anio, tienePermiso);
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
        /// Inicia la pestaña Detalles y obtiene datos actualizados del BLOQUE "Fecha Inicio" (anio, fecIni, Dia, FecFin)
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="aniocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int aniocodi)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Accion = accion;

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones

                if (accion == ConstantesPMPO.AccionCrear)
                {
                    int anioActual = DateTime.Now.Year;
                    DateTime fechaIniAnio = pmpoServicio.ObtenerFechaInicioAnho(anioActual);
                    DateTime fechaFinAnio = pmpoServicio.ObtenerFechaFInAnho(anioActual, fechaIniAnio);

                    model.Anio = anioActual.ToString();
                    model.FechaIniAnio = fechaIniAnio.ToString(ConstantesAppServicio.FormatoFecha);
                    model.FechaFinAnio = fechaFinAnio.ToString(ConstantesAppServicio.FormatoFecha);
                    model.DiaNombre = fechaIniAnio.ToString("dddd");
                    model.EsNuevo = accion == ConstantesPMPO.AccionCrear ? true : false;
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        PmoAnioOperativoDTO anioConsulta = pmpoServicio.GetByIdPmoAnioOperativo(aniocodi);
                        model.Anio = anioConsulta.Pmanopanio.Value.ToString();
                        model.FechaIniAnio = anioConsulta.PmanopfeciniDesc;
                        model.FechaFinAnio = anioConsulta.PmanopfecfinDesc;
                        model.DiaNombre = anioConsulta.Pmanopfecini.Value.ToString("dddd");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene datos actualizados del BLOQUE "Fecha Inicio" (anio, fecIni, Dia, FecFin)
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFechaInicialAnio(int anio, int accion, int? aniocodi)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

                if (accion == ConstantesPMPO.AccionVerDetalles)
                {
                    model.TienePermisoNuevo = false; //Desactiva funcionalidades en el handsonTable
                    model.TienePermisoEditar = false;
                }

                PmoAnioOperativoDTO objAnioOperativo;
                if (aniocodi != 0 && aniocodi != null)
                    objAnioOperativo = pmpoServicio.GetByIdPmoAnioOperativo(aniocodi.Value);
                else
                    objAnioOperativo = pmpoServicio.GetByCriteriaPmoAnioOperativos(anio).Find(x => x.Pmanopestado == ConstantesPMPO.EstadoActivo);

                DateTime fechaIniAnio = accion == ConstantesPMPO.AccionEditar ? objAnioOperativo.Pmanopfecini.Value : pmpoServicio.ObtenerFechaInicioAnho(anio);
                DateTime fechaFinAnio = accion == ConstantesPMPO.AccionEditar ? objAnioOperativo.Pmanopfecfin.Value : pmpoServicio.ObtenerFechaFInAnho(anio, fechaIniAnio);

                model.FechaIniAnio = fechaIniAnio.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFinAnio = fechaFinAnio.ToString(ConstantesAppServicio.FormatoFecha);
                model.DiaNombre = fechaIniAnio.ToString("dddd");
                model.EsProcesado = (accion == ConstantesPMPO.AccionEditar && objAnioOperativo.Pmanopprocesado == ConstantesPMPO.EstadoProcesado) ? true : false;
                model.NumVersion = objAnioOperativo != null ? objAnioOperativo.Pmanopnumversion.Value : -1;

                //meses
                List<PmoMesDTO> lstSemanaMes = pmpoServicio.ListarSemanaMesDeAnho(anio, accion, aniocodi);
                model.ListaSemanaMes = lstSemanaMes;

                //feriados
                DateTime FecIni = new DateTime(anio, 1, 1);
                DateTime FecFin = new DateTime(anio, 12, 31);

                List<PmoFeriadoDTO> lstFeriados = pmpoServicio.ListarFeriadosDeAnho(anio, FecIni, FecFin, accion, aniocodi ?? 0);
                model.ListaFeriados = lstFeriados;

                //salida
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
        /// Agrega un feriado en el listado de feriados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="descripcion"></param>
        /// <param name="anio"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="listaFeriadosEnPantalla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AgregarFeriado(string fecha, string descripcion, int anio, List<PmoFeriadoDTO> listaFeriadosEnPantalla)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!tienePermiso) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string fechaIni = "01/01/" + anio;
                string fechaFin = "31/12/" + anio;

                DateTime Fec = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime FecIni = DateTime.ParseExact(fechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime FecFin = DateTime.ParseExact(fechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string msgValidacion = pmpoServicio.ValidarFeriadoIngresado(Fec, FecIni, FecFin, listaFeriadosEnPantalla);
                if (msgValidacion != "") throw new ArgumentException(msgValidacion);

                model.ListaFeriados = pmpoServicio.AumentarFeriadoALista(fecha, descripcion, listaFeriadosEnPantalla);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Actualiza el num Semana de la lista de semanaMes ante algun cambio
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="fechaIniAnio"></param>
        /// <param name="listaSMEnPantalla"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarLstSemanaMes(int anio, string fechaIniAnio, List<PmoMesDTO> listaSMEnPantalla)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime FecIniAnio = DateTime.ParseExact(fechaIniAnio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string strFechaInicioValido = pmpoServicio.ValidarFechIniEnAnioOperativo(anio, FecIniAnio);
                if (strFechaInicioValido != "") throw new Exception(strFechaInicioValido);

                model.ListaSemanaMes = pmpoServicio.ActualizarListaSemanaMes(anio, FecIniAnio, listaSMEnPantalla);
                model.FechaFinRango = pmpoServicio.ObtenerFechaFInAnho(anio, FecIniAnio).ToString(ConstantesAppServicio.FormatoFecha);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ActualizarLstFeriados(int anio, string fechaIniAnio, int? aniocodi, List<PmoFeriadoDTO> listaFeriadoPantalla)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime FecIniAnio = DateTime.ParseExact(fechaIniAnio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string strFechaInicioValido = pmpoServicio.ValidarFechIniEnAnioOperativo(anio, FecIniAnio);
                if (strFechaInicioValido != "") throw new Exception(strFechaInicioValido);

                model.ListaFeriados = pmpoServicio.ActualizarListaFeriados(anio, FecIniAnio, aniocodi, listaFeriadoPantalla);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }


        /// <summary>
        /// Guarda un anio operativo (incluido la lista de semanas/mes y feriados)
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="fechaIniocioAnio"></param>
        /// <param name="descripcion"></param>
        /// <param name="listaInicioSemanaMes"></param>
        /// <param name="listaFeriados"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistrarAnioOperativo(int accion, int anio, string fechaIniocioAnio, string descripcion, List<PmoMesDTO> listaInicioSemanaMes, List<PmoFeriadoDTO> listaFeriados)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //valida que no se Agregue dos veces un mismo anio
                if (accion == ConstantesPMPO.AccionCrear)
                {
                    string strValidacionAnio = pmpoServicio.ValidarAnioOperativo(anio);
                    if (strValidacionAnio != "") throw new ArgumentException(strValidacionAnio);
                    descripcion = "Año registrado";
                }

                DateTime fec = DateTime.ParseExact(fechaIniocioAnio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                pmpoServicio.EnviarDatosAnhoOperativo(anio, fec, descripcion, listaInicioSemanaMes, listaFeriados, base.UserName);


                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Escoge una version como VIGENTE y actualiza sus datos
        /// </summary>
        /// <param name="aniocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarVigencia(int aniocodi)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.AprobarVersionAnioOperativo(aniocodi, base.UserName, out int anio);
                model.Anio = anio.ToString();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el archivo dat a exportar
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="anioIni"></param>
        /// <param name="anioFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoSalida(int tipo, int anioIni, int anioFin)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                pmpoServicio.GenerarArchivoDat(ruta, tipo, anioIni, anioFin, out string nameFile);
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
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            //byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);
            string fullPath = ruta + nombreArchivo;

            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar extension
        }

        #endregion
    }
}