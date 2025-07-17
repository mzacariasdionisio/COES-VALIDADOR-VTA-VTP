using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using System.Reflection;
using log4net;
using COES.MVC.Intranet.Helper;
//using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.MVC.Intranet.Areas.CPPA.Models;
using COES.Dominio.DTO.Transferencias;

using COES.Servicios.Aplicacion.CPPA;
using System.IO;
using COES.MVC.Intranet.Controllers;
using System.Text;
using System.Text.Json;
using Microsoft.Office.Interop.Excel;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.CPPA.Controllers
{
    public class AjustePresupuestalController : BaseController
    {
        #region Declaración de variables
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        private readonly CPPAAppServicio CppaAppServicio = new CPPAAppServicio();

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

        public AjustePresupuestalController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        /// <summary>
        /// Pagina inicial de la modulo.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Obtiene las revisiones en base al filtro específico.
        /// </summary>
        /// <param name="anioFrom"></param>
        /// <param name="anioUntil"></param>
        /// <param name="estados"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult List(int anioFrom, int anioUntil, string estados)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                base.ValidarSesionJsonResult();
                model.ListRevision = CppaAppServicio.ListarRevisiones(anioFrom, anioUntil, ConstantesCPPA.todos, estados);
                model.sResultado = "1";

                model.bNuevo = base.VerificarAccesoAccion(Acciones.Nuevo, User.Identity.Name);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Prepara los datos para crear una nueva Revisión.
        /// </summary>
        /// <returns></returns>
        public ActionResult New()
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                int year = DateTime.Now.Year + 1;
                model.Revision = CppaAppServicio.ObtenerNuevaRevision(year, "A1");
                model.sResultado = "1";

                model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Prepara los datos para crear una nueva Revisión.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetNewRevision(int cpaapanio, string cpaapajuste)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                model.Revision = CppaAppServicio.ObtenerNuevaRevision(cpaapanio, cpaapajuste);
                model.sResultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Prepara los datos para editar una Revisión específica.
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        public ActionResult Edit(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                model.Revision = CppaAppServicio.GetByIdCpaRevision(cparcodi);
                model.sResultado = "1";

                model.bGrabar = base.VerificarAccesoAccion(Acciones.Grabar, User.Identity.Name);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return PartialView(model);
        }


        /// <summary>
        /// Guardar los datos del formulario de Ajuste Presupuestal para crear una nueva Revisión
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Save(CPPAModel model)
        {
            CPPAModel modelNew = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();

                if (model == null || model.Revision == null)
                {
                    modelNew.sResultado = "-1";
                    modelNew.sMensaje = "No se está enviando ningún dato para ser procesado.";
                    return Json(modelNew);
                }

                CppaAppServicio.CrearNuevaRevision(model.Revision, User.Identity.Name);

                modelNew.sResultado = "1";
                return Json(modelNew);
            }
            catch (Exception ex)
            {
                modelNew.sResultado = "-1";
                modelNew.sMensaje = ex.Message;
                modelNew.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(modelNew);
            }
        }

        /// <summary>
        /// Actualizar los datos del formulario de Ajuste Presupuestal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(CPPAModel model)
        {
            CPPAModel modelNew = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();

                if (model == null || model.Revision == null)
                {
                    modelNew.sResultado = "-1";
                    modelNew.sMensaje = "No se está enviando ningún dato para ser procesado.";
                    return Json(modelNew);
                }

                CppaAppServicio.ActualizarEstadoYCMgPMPORevision(model.Revision.Cparcodi, model.Revision.Cparestado, model.Revision.Cparcmpmpo, User.Identity.Name);

                modelNew.sResultado = "1";
                return Json(modelNew);
            }
            catch (Exception ex)
            {
                modelNew.sResultado = "-1";
                modelNew.sMensaje = ex.Message;
                modelNew.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(modelNew);
            }
        }

        /// <summary>
        /// Anula una Revisión
        /// </summary>
        /// <param name="cparcodi"></param>
        /// <returns></returns>
        public JsonResult Annul(int cparcodi)
        {
            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();

                CppaAppServicio.AnularRevision(cparcodi, User.Identity.Name);

                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// Obtiene los datos del histórico de cambios de una revisión
        /// </summary>
        /// <param name="value">Valor de value</param>
        /// <returns></returns>
        public ActionResult ViewLog(int cparcodi)
        {

            CPPAModel model = new CPPAModel();

            try
            {
                base.ValidarSesionUsuario();
                model.Revision = CppaAppServicio.GetByIdCpaRevision(cparcodi);
                model.ListHistorico = CppaAppServicio.ListaHistorico(cparcodi);
                model.sResultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Prepara una vista para editar un nuevo registro
        /// </summary>
        /// <param name="anio">Valor de value</param>
        /// <param name="ajuste">Valor de value</param>
        /// <param name="revision">Valor de value</param>
        /// <param name="idRevision">Valor de value</param>
        /// <returns></returns>
        public ActionResult CopyParameter(string anio, string ajuste, string revision, int idRevision)
        {
            //base.ValidarSesionUsuario();
            CPPAModel model = new CPPAModel();
            model.ListaAnio = CppaAppServicio.ObtenerAnios(out List<CpaRevisionDTO> ListRevision);
            ViewBag.Anio = anio;
            ViewBag.Ajuste = ajuste;
            ViewBag.Revision = revision;
            ViewBag.IdRevision = idRevision;
            ViewBag.ListRevision = Newtonsoft.Json.JsonConvert.SerializeObject(ListRevision);

            return PartialView(model);
        }

        /// <summary>
        /// Permite copiar las relaciones entre empresas, centrales 
        /// y centrales pmpo a otra revision
        /// </summary>
        /// <param name="revisionHasta"></param>
        /// <param name="anioHasta"></param>
        /// <param name="ajusteHasta"></param>
        /// <param name="revisionDesde"></param>
        /// <param name="anioDesde"></param>
        /// <param name="ajusteDesde"></param>
        /// <returns></returns>
        public JsonResult CopiarDatos(int revisionHasta, int anioHasta, string ajusteHasta, int revisionDesde, int anioDesde, string ajusteDesde)
        {
            CPPAModel model = new CPPAModel();
            try
            {
                int nuevoAnioHasta = anioHasta - 1;
                int nuevoAnioDesde = anioDesde - 1;
                object resMessage = CppaAppServicio.CopiarEstructuraIntegrante(revisionHasta, nuevoAnioHasta, ajusteHasta, revisionDesde, nuevoAnioDesde, ajusteDesde, User.Identity.Name);
                model.sMensaje = (string)resMessage.GetType().GetProperty("dataMsg").GetValue(resMessage);
                model.sTipo = (string)resMessage.GetType().GetProperty("typeMsg").GetValue(resMessage);
                model.sResultado = "1";
                return Json(model);
            }
            catch (Exception ex)
            {
                model.sResultado = "-1";
                model.sMensaje = ex.Message;
                model.sDetalle = ex.StackTrace;
                Log.Error(NameController, ex);

                return Json(model);
            }
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesCPPA.ModuloManualUsuario;
            string nombreArchivo = ConstantesCPPA.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesCPPA.FolderRaizCPPAModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}