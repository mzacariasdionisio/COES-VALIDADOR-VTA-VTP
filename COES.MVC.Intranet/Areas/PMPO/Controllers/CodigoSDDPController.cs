using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PMPO.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PMPO;
using log4net;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO.Controllers
{
    /// <summary>
    /// Clase controladora de los Codigos SDDP
    /// </summary>
    public class CodigoSDDPController : BaseController
    {
        readonly ProgramacionAppServicio pmpo = new ProgramacionAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SDDPModel model = new SDDPModel();
            model.ListaTipoSDDP = pmpo.ListPmoSddpTipos();

            return View(model);
        }

        [HttpPost]
        public JsonResult ListaCodigo(int tsddpcodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                model.ListaCodigoSDDP = pmpo.ListarCodigoSDDP(tsddpcodi.ToString());
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
        /// Grabar nuevo codigo
        /// </summary>
        public JsonResult GuardarCodigo(PmoSddpCodigoDTO obj)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (obj.Grupocodi.GetValueOrDefault(0) <= 0) obj.Grupocodi = null;
                if (obj.Tptomedicodi.GetValueOrDefault(0) <= 0) obj.Tptomedicodi = null;
                if (obj.Ptomedicodi.GetValueOrDefault(0) <= 0) obj.Ptomedicodi = null;
                if (obj.Equicodi.GetValueOrDefault(0) <= 0) obj.Equicodi = null;

                pmpo.GuardarCodigoSDDP(obj, base.UserName);

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
        /// Grabar nueva correlacion
        /// </summary>
        public JsonResult ObtenerCodigo(int sddpcodi)
        {
            SDDPModel model = new SDDPModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.CodigoSDDP = pmpo.GetByIdPmoSddpCodigo(sddpcodi);
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
    }
}