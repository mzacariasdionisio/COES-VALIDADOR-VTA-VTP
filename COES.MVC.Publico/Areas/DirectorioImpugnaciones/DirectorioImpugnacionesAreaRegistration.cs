using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones
{
    public class DirectorioImpugnacionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DirectorioImpugnaciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DirectorioImpugnaciones_default",
                "DirectorioImpugnaciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
