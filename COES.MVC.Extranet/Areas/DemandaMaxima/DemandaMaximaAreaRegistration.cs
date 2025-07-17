using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.DemandaMaxima
{
    public class DemandaMaximaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DemandaMaxima";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DemandaMaxima_default",
                "DemandaMaxima/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
