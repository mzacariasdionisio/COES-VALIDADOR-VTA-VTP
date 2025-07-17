using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class SimulacionController : BaseController
    {
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();
        ServiceReferenceYupanaContinuo.YupanaContinuoServicioClient cliente = new ServiceReferenceYupanaContinuo.YupanaContinuoServicioClient();

        private bool UsarCliente = ConfigurationManager.AppSettings[ConstantesYupanaContinuo.UsarWebServiceYupanaContinuo] == "S";

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

        public SimulacionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Index simulación
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SimulacionModel model = new SimulacionModel();

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //si existe un arbol en ejecucion
            CpArbolContinuoDTO objUltimoArbol = yupanaServicio.GetUltimoArbol();
            bool existeArbolEnEjec = objUltimoArbol != null;
                        //&& (ConstantesYupanaContinuo.EstadoArbolEnEjecucion == objUltimoArbol.Cparbestado || ConstantesYupanaContinuo.EstadoArbolNoIniciado == objUltimoArbol.Cparbestado);

            DateTime fechaActual = DateTime.Today;
            if (existeArbolEnEjec) fechaActual = objUltimoArbol.Cparbfecha;
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            //topologia seleccionada
            model.ListaTopologia = yupanaServicio.ListarTopologiaXFecha(fechaActual);
            model.CodigoTopologiaMostrado = model.ListaTopologia.Any() ? model.ListaTopologia.First().Topcodi : 0;
            if (existeArbolEnEjec) model.CodigoTopologiaMostrado = objUltimoArbol.Topcodi;

            //arbol seleccionado
            model.ListaTag = yupanaServicio.GetByCriteriaCpArbolContinuos(model.CodigoTopologiaMostrado);
            model.CodigoArbolMostrado = 0;
            if (existeArbolEnEjec) model.CodigoArbolMostrado = objUltimoArbol.Cparbcodi;

            return View(model);
        }

        /// <summary>
        /// Devuelve lista de identificadores de árbol para cierta fecha
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaIdentificadores(string fechaConsulta)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                model.ListaTopologia = yupanaServicio.ListarTopologiaXFecha(fecha);

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
        /// Devuelve lista de tags de árbol para cierta fecha e identificador
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="identificadorConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaTags(string fechaConsulta, int topcodi)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                model.ListaTag = yupanaServicio.GetByCriteriaCpArbolContinuos(topcodi);

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
        /// Inicia la simulación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IniciarSimulacion()
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaHora = DateTime.Today;
                //fechaHora = new DateTime(2021, 12, 15);

                string fecha = fechaHora.ToString(ConstantesAppServicio.FormatoFecha);
                int hora = fechaHora.Hour;
                string usuario = base.UserName;

                //Validación
                yupanaServicio.ValidarPrevioSimulacion(ConstantesYupanaContinuo.SimulacionManual, fecha, hora, out DateTime fechaHora2, out int topcodi);

                if (UsarCliente)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => cliente.EjecutarYupanaContinuoManual(fecha, hora, usuario));
                }
                else
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => yupanaServicio.EjecutarSimulacionManualXFechaYHora(fecha, hora, usuario));
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Muestra el grafico de un árbol
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <param name="identificadorConsulta"></param>
        /// <param name="tagConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarArbol(string fechaConsulta, int cparbcodi)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                if (cparbcodi > 0)
                {
                    model.LstDatosNodos = yupanaServicio.ObtenerContenidoNodos(cparbcodi, out CpArbolContinuoDTO arbolConsultado);

                    model.EsUltimoTag = yupanaServicio.VerificarUltimoTag(cparbcodi);
                    model.PorcentajeNodosSimulados = arbolConsultado.Cparbporcentaje;
                    model.MensajeProceso = arbolConsultado.Cparbmsjproceso ?? "";
                    model.IdentificadorArbolCreado = arbolConsultado.Cparbidentificador;
                    model.TagArbolCreado = arbolConsultado.Cparbtag;
                    model.CodigoArbolMostrado = cparbcodi;
                    model.Resultado = "1";
                }
                else
                {
                    model.LstDatosNodos = new List<EstructuraNodo>();
                    model.Resultado = "2";
                }

                if (UsarCliente)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => cliente.VerificarEstadoYupanaContinuo());
                }
                else
                {
                    HostingEnvironment.QueueBackgroundWorkItem(token => yupanaServicio.VerificarPlazosEjecucion());
                }
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Descarga un archivo .rar con las salidas de cada simulacion por nodo
        /// </summary>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public JsonResult DescargarArchSalida(string numeroNodoClikeado, int arbolcodi)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (arbolcodi > 0)
                {
                    yupanaServicio.GenerarArchivosSalidaPorNodo(ruta, arbolcodi, numeroNodoClikeado, out string nameFile);
                    model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "No se encuentra el árbol.";
                    model.Resultado = "-1";
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
        /// Descarga un archivo .rar con las salidas de todo un tag
        /// </summary>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public JsonResult DescargarArchivosTag(int arbolcodi)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (arbolcodi > 0)
                {
                    yupanaServicio.GenerarArchivosSalidaPorTag(ruta, arbolcodi, out string nameFile);
                    model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "No se encuentra el árbol.";
                    model.Resultado = "-1";
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
        /// Guarda la informacion de los insumos usados en la simulación del escenario elegido
        /// </summary>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public JsonResult GuardarDataNodo(string numeroNodoClikeado, int arbolcodi)
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int idTopocodi = yupanaServicio.GuardarNodoEnTopologia(numeroNodoClikeado, arbolcodi);
                model.Resultado = "1";

                if (idTopocodi == 0)
                {
                    model.Resultado = "-1";
                    model.Mensaje = "No se puedo guardar escenario.";
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
        /// Guarda la informacion de los insumos usados en la simulación del escenario elegido
        /// </summary>
        /// <param name="numeroNodoClikeado"></param>
        /// <param name="arbolcodi"></param>
        /// <returns></returns>
        public JsonResult FinalizarEjecucionGams()
        {
            SimulacionModel model = new SimulacionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                if (UsarCliente)
                {
                    cliente.TerminarEjecucionGams();
                }
                else
                {
                    yupanaServicio.FinalizarEjecucionArbol();
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
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

    }
}