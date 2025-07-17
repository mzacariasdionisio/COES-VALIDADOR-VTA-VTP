using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Hidrologia
{
    public class HidrologiaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Hidrologia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Hidrologia_default",
                "Hidrologia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
