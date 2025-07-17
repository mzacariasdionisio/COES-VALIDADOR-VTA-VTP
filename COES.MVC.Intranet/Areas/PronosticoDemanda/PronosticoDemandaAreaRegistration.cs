using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PronosticoDemanda
{
    public class PronosticoDemandaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PronosticoDemanda";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PronosticoDemanda_default",
                "PronosticoDemanda/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}