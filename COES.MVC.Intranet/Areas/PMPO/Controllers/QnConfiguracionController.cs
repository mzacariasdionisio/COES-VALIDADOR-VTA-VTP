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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class QnConfiguracionController : BaseController
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
        /// Index Estaciones Hidrológicas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            QnConfiguracionModel model = new QnConfiguracionModel();

            try
            {                
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.UsarLayoutModulo = false;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }
            return View(model);
        }

        /// <summary>
        /// Devuelve el html del listado de estaciones hidrologicas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEstacionesHidrologicas()
        {
            QnConfiguracionModel model = new QnConfiguracionModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");                
                model.HtmlListadoEstacionesH = pmpoServicio.HtmlListadoEstacionHidro(url, tienePermiso);
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
        /// Actualiza el campo orden de las estaciones cuando se reordena el listado
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromPosition"></param>
        /// <param name="toPosition"></param>
        /// <param name="direction"></param>
        public void UpdateOrder(int id, int fromPosition, int toPosition, string direction)
        {
            pmpoServicio.ActualizarOrdenEstaciones(fromPosition, toPosition, direction);                        
        }

        /// <summary>
        /// Obtiene datos generales al mostrar pestaña Detalles
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="pmehcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int pmehcodi)
        {
            QnConfiguracionModel model = new QnConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Accion = accion;

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones

                if (accion == ConstantesPMPO.AccionCrear) 
                {                                        
                    model.EstacionH = new PmoEstacionhDTO();
                    model.EsNuevo =  true;
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        PmoEstacionhDTO estacionConsulta = pmpoServicio.GetByIdPmoEstacionh(pmehcodi);                        
                        model.EstacionH = estacionConsulta;                        
                    }
                }

                model.ListaSddp = pmpoServicio.ListarPuntosSDDP();
                
                model.Resultado = "1";
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
        /// Obtiene descripcion de la estacion
        /// </summary>
        /// <param name="sddpcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDescripcion(int accion, int sddpcodi, int pmehcodi)
        {
            QnConfiguracionModel model = new QnConfiguracionModel();
            string desc = "";
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (accion == ConstantesPMPO.AccionCrear)
                {
                    List<PmoSddpCodigoDTO> lst = pmpoServicio.ListarPuntosSDDP();
                    var obj = lst.Find(x => x.Sddpcodi == sddpcodi);
                    desc = obj != null ? obj.DescripcionSDDP.Trim() : "";
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        PmoEstacionhDTO estacionConsulta = pmpoServicio.GetByIdPmoEstacionh(pmehcodi);
                        desc = estacionConsulta.Pmehdesc;
                    }
                }

                model.Descripcion = desc;
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
        /// Obtiene datos ptos de medicion al mostrar pestaña Detalles 
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="pmehcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarDataPtos(int accion, int pmehcodi)
        {
            QnConfiguracionModel model = new QnConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);                                

                if (accion == ConstantesPMPO.AccionCrear)
                {
                    model.ListaPtosMedicion = pmpoServicio.ListarPuntosGenerales( accion, pmehcodi);                    
                    model.ListaPtosXEstacion = pmpoServicio.ListarPtoXEstacionHidro(accion, pmehcodi);
                }
                else
                {
                    if (accion == ConstantesPMPO.AccionEditar || accion == ConstantesPMPO.AccionVerDetalles)
                    {
                        model.ListaPtosMedicion = pmpoServicio.ListarPuntosGenerales(accion, pmehcodi);
                        model.ListaPtosXEstacion = pmpoServicio.ListarPtoXEstacionHidro(accion, pmehcodi);
                    }
                }

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
        public JsonResult RegistrarEstacionHidrologica(int accion, int ptomedicodi, string referencia, string descripcion, string integrante, List<PuntosSDDP> listaPtosPorEstacion)
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
                    string strValidacionEH = "";
                    strValidacionEH = pmpoServicio.ValidarEstacionHidrologicaRepetida(ptomedicodi);//Completar Rosmel
                    if (strValidacionEH != "") throw new ArgumentException(strValidacionEH);
                    
                }

                PmoEstacionhDTO objEstacionH = new PmoEstacionhDTO();
                objEstacionH.Pmehdesc = descripcion;
                objEstacionH.Pmehreferencia = referencia;
                objEstacionH.Sddpcodi = ptomedicodi;

                pmpoServicio.EnviarDatosEstacionHidro(ptomedicodi, referencia, descripcion, integrante, listaPtosPorEstacion, base.UserName);


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
        /// Devuelve el html de la ventana de versiones de cierta estacion
        /// </summary>
        /// <param name="pmhecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(int pmhecodi)
        {
            QnConfiguracionModel model = new QnConfiguracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = pmpoServicio.GenerarHtmlVersionesEstacion(url, pmhecodi, tienePermiso);  // implementar Rosmel                
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
        /// Da de baja una estación hidrológica
        /// </summary>
        /// <param name="pmhecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarEstacionHidrologica(int pmhecodi)
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.EliminarEstacionHidrologica(pmhecodi, base.UserName);

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
        /// Genera el archivo dat a exportar
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoSalida()
        {
            ParametrosFechasModel model = new ParametrosFechasModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                pmpoServicio.GenerarArchivosEstacionHidro(ruta, out string nameFile);                

                pmpoServicio.GenerarArchivosEstacionHidroZip(ruta, out string nameFile2);
                
                model.Resultado = nameFile2;
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

            string fullPath = ruta + "PMPO\\CAUDALES\\" +  nombreArchivo;

            return File(fullPath, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar extension
        }
        #endregion
    }
}