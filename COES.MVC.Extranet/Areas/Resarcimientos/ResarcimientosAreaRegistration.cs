using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Resarcimientos
{
    public class ResarcimientosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Resarcimientos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Resarcimientos_default",
                "Resarcimientos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
