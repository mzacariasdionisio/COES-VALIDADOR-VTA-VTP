using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento
{
    public class CalculoResarcimientoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CalculoResarcimiento";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CalculoResarcimiento_default",
                "CalculoResarcimiento/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}