using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme
{
    public class PotenciaFirmeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PotenciaFirme";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PotenciaFirme_default",
                "PotenciaFirme/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
