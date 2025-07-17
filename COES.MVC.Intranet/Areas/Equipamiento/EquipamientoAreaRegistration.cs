using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento
{
    public class EquipamientoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Equipamiento";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Equipamiento_default",
                "Equipamiento/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
