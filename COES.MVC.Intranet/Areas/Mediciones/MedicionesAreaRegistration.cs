using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones
{
    public class MedicionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mediciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Mediciones_default",
                "Mediciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
