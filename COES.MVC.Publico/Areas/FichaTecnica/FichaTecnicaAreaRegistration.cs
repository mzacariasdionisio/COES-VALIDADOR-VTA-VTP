using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.FichaTecnica
{
    public class FichaTecnicaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FichaTecnica";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FichaTecnica_default",
                "FichaTecnica/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
