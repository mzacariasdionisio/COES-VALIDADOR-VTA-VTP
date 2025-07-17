using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PMPO
{
    public class PMPOAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PMPO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMPO_default",
                "PMPO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
