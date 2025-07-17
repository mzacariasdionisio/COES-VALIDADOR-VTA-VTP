using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo
{
    public class CortoPlazoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CortoPlazo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CortoPlazo_default",
                "CortoPlazo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
