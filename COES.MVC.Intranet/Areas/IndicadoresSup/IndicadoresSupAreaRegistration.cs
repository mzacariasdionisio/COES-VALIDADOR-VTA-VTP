using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IndicadoresSup
{
    public class IndicadoresSupAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IndicadoresSup";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IndicadoresSup_default",
                "IndicadoresSup/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
