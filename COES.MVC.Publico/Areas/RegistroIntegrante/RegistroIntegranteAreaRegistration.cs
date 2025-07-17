using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.RegistroIntegrante
{
    public class RegistroIntegranteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RegistroIntegrante";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RegistroIntegrante_default",
                "RegistroIntegrante/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
