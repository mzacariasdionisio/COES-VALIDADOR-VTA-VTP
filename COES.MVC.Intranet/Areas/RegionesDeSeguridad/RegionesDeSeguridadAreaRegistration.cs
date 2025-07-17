using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RegionesDeSeguridad
{
    public class RegionesDeSeguridadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RegionesDeSeguridad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RegionesDeSeguridad_default",
                "RegionesDeSeguridad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}