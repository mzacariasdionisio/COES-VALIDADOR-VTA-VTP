using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SistemasTransmision
{
    public class SistemasTransmisionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SistemasTransmision";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SistemasTransmision_default",
                "SistemasTransmision/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
