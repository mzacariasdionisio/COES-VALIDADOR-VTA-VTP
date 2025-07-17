using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using System;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class GeneralController : BaseController
    {
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            Intervencion model = new Intervencion();
            model.TienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, ConstantesIntervencionesAppServicio.MenuOpcionCodeIntervenciones);

            return View(model);
        }

        /// <summary>
        /// validar sesion antes de ingresar a cada opción de los items
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarModulo()
        {
            InGeneral model = new InGeneral();

            if (base.IsValidSesionView()) model.EsValidoSesion = true;
            model.EsValidoOpcion = true;

            return Json(model);
        }
    }
}