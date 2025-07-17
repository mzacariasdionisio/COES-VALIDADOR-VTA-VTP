using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Coordinacion
{
    public class CoordinacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Coordinacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Coordinacion_default",
                "Coordinacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
