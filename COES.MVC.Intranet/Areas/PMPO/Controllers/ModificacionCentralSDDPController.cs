
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
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    public class ModificacionCentralSDDPController : BaseController
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
        /// Index Modificación Central SDDP
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? mtopcodi, int? tipoAccion)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ModificacionCentralModel model = new ModificacionCentralModel();
            bool valorLayout = false; 
            try
            {
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                
                model.Topcodi = mtopcodi ?? ConstantesPMPO.IdTopologiaModificacion;
                model.Action = tipoAccion ?? 0;

                string escenario = "";
                if (model.Topcodi > 1)
                {
                    valorLayout = true;
                    var objEscenario = pmpoServicio.GetByIdMpTopologia(model.Topcodi);
                    var nombreEscenario = objEscenario.Mtopnomb;
                    var version = objEscenario.Mtopversion;
                    escenario = ": " + nombreEscenario + " Versión " + version;
                }

                model.UsarLayoutModulo = valorLayout;
                ViewBag.Titulo = "Modificación Configuración Base" + escenario;
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
        /// Devuelve el html del listado de Modifiación Centrales SDDP
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarModificacionCentraleSDDP(int topcodi, int action)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();
            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.HtmlListadoModificacionCentral = pmpoServicio.HtmlListadoModificacionCentral(topcodi, action, url, tienePermiso);
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
        /// Obtiene Carga de pestaña detalle
        /// </summary>
        /// <param name="accion"></param>
        /// <param name="topcodi"></param>
        /// <param name="recurcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargarDetalles(int accion, int topcodi, int recurcodi)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Accion = accion;

                if (accion == ConstantesPMPO.AccionVerDetalles)
                    model.TienePermisoNuevo = false; //Desactiva botones

                DateTime fechaActual = DateTime.Now;
                model.FechaFutura = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);
                //model.ModificacionCentral = pmpoServicio.ObtenerModificacion(topcodi, recurcodi);
                model.ModificacionCentral = new MpTopologiaDTO();
                model.ModificacionCentral.RecursoSddp = new MpRecursoDTO();
                model.ModificacionCentral.RecursoSddp.ListaPropRecursoSddp = new List<MpProprecursoDTO>();
                model.EsNuevo = accion == ConstantesPMPO.AccionCrear? true: false;

                //codigos de las propiedades
                model.propcodiPotencia = ConstantesPMPO.PropPotencia;
                model.propcodiCoefProd = ConstantesPMPO.PropCoefProduccion;
                model.propcodiCaudalMinTur = ConstantesPMPO.PropCaudalMinT;
                model.propcodiCaudalMaxTur = ConstantesPMPO.PropCaudalMaxT;
                model.propcodiICP = ConstantesPMPO.PropFactorIndForzada;
                model.propcodiIH = ConstantesPMPO.PropFactorIndHistorica;
                model.propcodiDefluenciaTotMin = ConstantesPMPO.PropDefluenciaTotMin;
                model.propcodiVolumenMax = ConstantesPMPO.PropVolMax;
                model.propcodiIndicadorEA = ConstantesPMPO.PropIndicadorEA;


                var topcodiForRecurso = topcodi == ConstantesPMPO.IdTopologiaModificacion? ConstantesPMPO.IdTopologiaBase : topcodi;
                model.ListaSddp = pmpoServicio.ListMpRecursos().Where(x=>x.Mtopcodi == topcodiForRecurso && x.Mrecurestado == ConstantesPMPO.EstadoActivo).ToList();

                model.ListaSddp.ForEach(x => pmpoServicio.FormatearRecursoTopBase(x));
                //foreach (var item in model.ListaSddp)
                //{
                //    pmpoServicio.FormatearRecrusoTopBase(item);
                //}

                //if (model.ListaSddp.Count == 0) throw new ArgumentException("No existe recursos para la topología Base");
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
        /// Obtiene el recurso seleccionado
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerRecurso(int topcodi, int recurcodi, int accion)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //if(accion != ConstantesPMPO.AccionCrear)
                //    model.ModificacionCentral = pmpoServicio.ObtenerModificacion(topcodi, recurcodi);
                var topcodiForRecurso = topcodi == ConstantesPMPO.IdTopologiaModificacion ? ConstantesPMPO.IdTopologiaBase : topcodi;
                model.ModificacionCentral = pmpoServicio.ObtenerModificacion(topcodiForRecurso, recurcodi);

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
        /// Guarda o Edita la modifiación de centrales SDDP
        /// </summary>
        /// <param name="dataJson"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RegistarModificacionCentralSDDP(string dataJson, int accion)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                MpTopologiaDTO modifCentralSDDP = serializer.Deserialize<MpTopologiaDTO>(dataJson);

                string strValidacion = "";
                if (accion == ConstantesPMPO.AccionCrear)
                {
                    //VALIDAR DUPLICADOS
                    strValidacion = pmpoServicio.ValidarModificacionCentralSDDPRepetida(modifCentralSDDP);
                    if (strValidacion != "") throw new ArgumentException(strValidacion);

                    pmpoServicio.GuardarModificacionCentralSDDP(modifCentralSDDP, base.UserName);
                }
                if (accion == ConstantesPMPO.AccionEditar)
                {
                    //VALIDAR QUE EXISTA CAMBIOS
                    strValidacion = pmpoServicio.ValidarCambiosModificaciones(modifCentralSDDP);
                    if (strValidacion != "") throw new ArgumentException(strValidacion);
                    //VALIDAR DUPLICADOS
                    strValidacion = pmpoServicio.ValidarModificacionCentralSDDPRepetida(modifCentralSDDP);
                    if (strValidacion != "") throw new ArgumentException(strValidacion);

                    pmpoServicio.ActualizarModificacionCentralSDDP(modifCentralSDDP, base.UserName);
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

        /// <summary>
        /// Da de baja el recurso de modificación
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="recurcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarModificacionCentralSDDP(int topcodi, int recurcodi)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!model.TienePermisoNuevo) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pmpoServicio.EliminarModificacionCentralSDDP(topcodi, recurcodi);

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
        /// Generar Reporte excel 
        /// </summary>
        /// <returns></returns>
        public JsonResult DescargarModificacionCentralSDDP(int topcodi)
        {
            ModificacionCentralModel model = new ModificacionCentralModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string nameFile = pmpoServicio.DescargarModificacionCentralSDDP(topcodi, ruta); //revisar

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
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }
        #endregion
    }
}