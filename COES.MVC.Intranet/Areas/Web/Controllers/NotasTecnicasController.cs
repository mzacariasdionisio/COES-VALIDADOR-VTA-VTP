using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class NotasTecnicasController : BaseController
    {
        /// <summary>
        /// Pagina inicial del aplicativo
        /// </summary>
        /// <returns></returns>        
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            string user = (new SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(base.UserName);
            ViewBag.Usuario = user;
            return View();
        }

    }
}
