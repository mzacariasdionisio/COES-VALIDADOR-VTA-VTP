using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Yupana
{
    public class YupanaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Yupana";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Yupana_default",
                "Yupana/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
