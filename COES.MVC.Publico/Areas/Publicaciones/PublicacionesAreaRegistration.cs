using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Publicaciones
{
    public class PublicacionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Publicaciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Publicaciones_default",
                "Publicaciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
