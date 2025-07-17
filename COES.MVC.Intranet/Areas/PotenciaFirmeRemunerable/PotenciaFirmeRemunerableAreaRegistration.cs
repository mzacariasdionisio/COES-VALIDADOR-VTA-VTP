using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable
{
    public class PotenciaFirmeRemunerableAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PotenciaFirmeRemunerable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PotenciaFirmeRemunerable_default",
                "PotenciaFirmeRemunerable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
        
    }
}