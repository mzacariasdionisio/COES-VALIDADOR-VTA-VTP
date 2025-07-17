using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace COES.MVC.Extranet.App_Start
{
    public class ValidarSesionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session[DatosSesion.SesionUsuario] == null)
            {
                string originalUrl = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
                string url = ConfigurationManager.AppSettings[RutaDirectorio.InitialUrl].ToString();
                filterContext.HttpContext.Session["ReturnPage"] = originalUrl;
                filterContext.Result = new RedirectResult(url + Constantes.PaginaLogin);
            }
        }
    }
}