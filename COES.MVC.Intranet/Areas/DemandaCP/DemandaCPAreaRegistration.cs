using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaCP
{
    public class DemandaCPAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DemandaCP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DemandaCP_default",
                "DemandaCP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
