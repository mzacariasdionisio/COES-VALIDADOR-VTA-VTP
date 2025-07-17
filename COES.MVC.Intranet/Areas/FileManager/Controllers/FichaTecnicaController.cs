using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.FileManager.Controllers
{
    public class FichaTecnicaController : BaseController
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
        public ActionResult FileAdmin()
        {
            if (!string.IsNullOrEmpty(this.UserName))
            {
                string user = (new SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(this.UserName);
                ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppFichaTecnica];
                ViewBag.Usuario = user;
                return View();
            }
            else            
            {
                return RedirectToLogin();
            }
        }

    }
}
