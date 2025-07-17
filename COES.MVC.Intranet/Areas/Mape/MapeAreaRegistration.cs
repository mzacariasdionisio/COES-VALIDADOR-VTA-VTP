using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mape
{
    public class MapeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mape";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mape_default",
                "Mape/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
