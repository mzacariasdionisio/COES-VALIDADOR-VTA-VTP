using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RechazoCarga
{
    public class RechazoCargaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RechazoCarga";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RechazoCarga_default",
                "RechazoCarga/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
