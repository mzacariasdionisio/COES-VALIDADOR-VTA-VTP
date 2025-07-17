using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Combustibles
{
    public class CombustiblesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Combustibles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Combustibles_default",
                "Combustibles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
