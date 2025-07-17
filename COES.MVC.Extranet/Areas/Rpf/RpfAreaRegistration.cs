using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Rpf
{
    public class RpfAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Rpf";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Rpf_default",
                "Rpf/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
