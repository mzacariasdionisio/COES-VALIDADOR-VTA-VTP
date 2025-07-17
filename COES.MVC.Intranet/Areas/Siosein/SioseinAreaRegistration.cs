using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Siosein
{
    public class SioseinAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Siosein";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Siosein_default",
                "Siosein/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
