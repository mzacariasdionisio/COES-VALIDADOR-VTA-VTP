using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.TramiteVirtual
{
    public class TramiteVirtualAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "TramiteVirtual";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "TramiteVirtual_default",
                "TramiteVirtual/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}