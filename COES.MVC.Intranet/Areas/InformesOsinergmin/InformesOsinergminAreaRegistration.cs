using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin
{
    public class InformesOsinergminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InformesOsinergmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InformesOsinergmin_default",
                "InformesOsinergmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
