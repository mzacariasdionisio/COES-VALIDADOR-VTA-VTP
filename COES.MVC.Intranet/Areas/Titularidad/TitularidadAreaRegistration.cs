using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Titularidad
{
    public class TitularidadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Titularidad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Titularidad_default",
                "Titularidad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
