using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SharePoint
{
    public class SharePointAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SharePoint";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SharePoint_default",
                "SharePoint/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
