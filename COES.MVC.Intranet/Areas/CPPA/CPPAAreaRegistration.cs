using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CPPA
{
    public class CPPAAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CPPA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CPPA_default",
                "CPPA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
