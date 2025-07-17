using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Configuration;
using System.Configuration;

namespace COES.MVC.Intranet.Helper
{
    /// <summary>
    /// Atributo personalizado que modifica la manera como se autorizan los permisos 
    /// </summary>
    public class CustomAuthorize : AuthorizeAttribute
    {
        public CustomAuthorize()
        {

        }

        /// <summary>
        /// Evento de Autorización
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool autorizado = false;


            var accion = filterContext.ActionDescriptor.ActionName;
            var controladora = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name.Trim()))
            {
                string login = HttpContext.Current.User.Identity.Name;
                autorizado = true;
            }
            else
            {
                autorizado = false;
            }

            if (autorizado)
            {
                bool acceso = (new SeguridadServicio.SeguridadServicioClient()).ValidarAccesoOpcion(controladora, accion,
                    Constantes.IdAplicacion, HttpContext.Current.User.Identity.Name.Trim());

                //if (acceso)
                //{
                //    base.OnAuthorization(filterContext);
                //}
                //else
                //{
                //    filterContext.Result = new RedirectResult("http://www.coes.org.pe/AppIntranet/home/autorizacion");
                //}
            }
            else
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["UrlIntranet"] + "home/login");
            }
        }

    }
}