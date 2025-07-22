using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP
{
    public class ValidacionVTEAVTPRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ValidacionVTEAVTP";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ValidacionVTEAVTP_default",
                "ValidacionVTEAVTP/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Controllers" }
            );
        }
    }
}