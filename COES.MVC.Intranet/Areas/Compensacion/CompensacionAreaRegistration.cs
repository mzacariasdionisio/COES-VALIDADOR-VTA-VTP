using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Compensacion
{
    public class CompensacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Compensacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Compensacion_default",
                "Compensacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
