using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaPO
{
    public class DemandaPOAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DemandaPO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DemandaPO_default",
                "DemandaPO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}