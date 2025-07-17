using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.YupanaContinuo
{
    public class YupanaContinuoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "YupanaContinuo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "YupanaContinuo_default",
                "YupanaContinuo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
