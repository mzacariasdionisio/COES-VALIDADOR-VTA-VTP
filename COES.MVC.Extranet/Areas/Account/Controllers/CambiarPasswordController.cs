using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Extranet.SeguridadServicio;
using COES.MVC.Extranet.Helper;

namespace COES.MVC.Extranet.Areas.Account.Controllers
{
    public class CambiarPasswordController : Controller
    {
        SeguridadServicio.SeguridadServicioClient servicio = new SeguridadServicio.SeguridadServicioClient();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Cambiar(string passwordActual, string passwordNueva)
        {
            try
            {
                string userLogin = string.Empty;
                if (Session[DatosSesion.SesionUsuario] != null)
                    userLogin = ((UserDTO)Session[DatosSesion.SesionUsuario]).UserLogin;
                else if (!string.IsNullOrEmpty(User.Identity.Name)) 
                    userLogin = User.Identity.Name;
                int resultado = this.servicio.CambiarClaveUsuario(userLogin, passwordActual, passwordNueva);
                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
