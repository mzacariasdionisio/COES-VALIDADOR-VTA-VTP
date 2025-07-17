using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Intervenciones
{
    public class IntervencionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Intervenciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Intervenciones_default",
                "Intervenciones/{controller}/{action}/{id}",
                new { action = "Programaciones", id = UrlParameter.Optional }
            );
        }
    }
}