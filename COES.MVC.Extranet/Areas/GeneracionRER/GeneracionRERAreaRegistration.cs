using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.GeneracionRER
{
    public class GeneracionRERAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GeneracionRER";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GeneracionRER_default",
                "GeneracionRER/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}