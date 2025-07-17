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
    public class InformacionOperativaController : BaseController
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
                ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppInformacionOperativa];
                ViewBag.Usuario = user;
                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        /// <summary>
        /// Vista local
        /// </summary>
        /// <returns></returns>
        public ActionResult FileCarga()
        {
            if (!string.IsNullOrEmpty(this.UserName))
            {
                string user = (new SeguridadServicio.SeguridadServicioClient()).EncriptarUsuario(this.UserName);
                ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppInformacionOperativa];
                ViewBag.Usuario = user;
                return View();
            }
            else
            {
                return RedirectToLogin();
            }
        }

        public ActionResult InformacionOperacion()
        {
            ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppInformacionOperativa];
            return View();
        }

        public ActionResult InformeSemanal()
        {
            ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppInformacionOperativa];
            return View();
        }


        public ActionResult ContratosGasNatural()
        {
            ViewBag.UrlFileApp = ConfigurationManager.AppSettings[Constantes.UrlFileAppInformacionOperativa];
            return View();
        }

    }
}
