using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReportesMedicion
{
    public class ReportesMedicionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ReportesMedicion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReportesMedicion_default",
                "ReportesMedicion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
