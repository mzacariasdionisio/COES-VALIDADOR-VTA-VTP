using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.DemandaBarras
{
    public class DemandaBarrasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DemandaBarras";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DemandaBarras_default",
                "DemandaBarras/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
