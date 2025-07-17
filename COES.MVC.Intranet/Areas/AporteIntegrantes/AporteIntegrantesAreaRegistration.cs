using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.AporteIntegrantes
{
    public class AporteIntegrantesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AporteIntegrantes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AporteIntegrantes_default",
                "AporteIntegrantes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
