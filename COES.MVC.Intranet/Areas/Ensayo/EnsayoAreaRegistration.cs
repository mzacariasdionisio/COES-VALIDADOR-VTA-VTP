using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ensayo
{
    public class EnsayoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ensayo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Ensayo_default",
                "Ensayo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
