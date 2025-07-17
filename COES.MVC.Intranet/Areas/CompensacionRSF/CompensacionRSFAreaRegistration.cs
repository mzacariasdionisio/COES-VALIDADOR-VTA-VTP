using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CompensacionRSF
{
    public class CompensacionRSFAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CompensacionRSF";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CompensacionRSF_default",
                "CompensacionRSF/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
