using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PrimasRER
{
    public class PrimasRERAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PrimasRER";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PrimasRER_default",
                "PrimasRER/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
