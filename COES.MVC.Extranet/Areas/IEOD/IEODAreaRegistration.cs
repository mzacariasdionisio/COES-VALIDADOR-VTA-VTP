using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD
{
    public class IEODAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IEOD";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IEOD_default",
                "IEOD/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
