using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ConsumoCombustible
{
    public class ConsumoCombustibleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ConsumoCombustible";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ConsumoCombustible_default",
                "ConsumoCombustible/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
