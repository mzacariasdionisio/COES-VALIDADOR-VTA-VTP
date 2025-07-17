using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class AgendaController : BaseController
    {

        /// <summary>
        /// Usuario que inicia sesion
        /// </summary>
        public string UserName
        {
            get
            {
                return (Session[DatosSesion.SesionUsuario] != null) ?
                    ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin : string.Empty;
            }
        }

        /// <summary>
        /// Vista azure
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int tipo)
        {
            if (!string.IsNullOrEmpty(this.UserName))
            {
                string user = (new SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(this.UserName);
                ViewBag.Usuario = user;
                ViewBag.Tipo = tipo;
                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

    }
}
