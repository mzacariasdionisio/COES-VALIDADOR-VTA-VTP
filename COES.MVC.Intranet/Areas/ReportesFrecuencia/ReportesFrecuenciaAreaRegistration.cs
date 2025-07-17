using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia
{
    public class ReportesFrecuenciaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ReportesFrecuencia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReportesFrecuencia_default",
                "ReportesFrecuencia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
