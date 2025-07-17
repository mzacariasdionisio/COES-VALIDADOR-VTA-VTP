using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.ReportePotencia
{
    public class ReportePotenciaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ReportePotencia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReportePotencia_default",
                "ReportePotencia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
