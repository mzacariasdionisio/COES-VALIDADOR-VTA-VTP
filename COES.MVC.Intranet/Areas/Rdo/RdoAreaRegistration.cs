using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Rdo
{
    public class RdoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Rdo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Rdo_default",
                "Rdo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
