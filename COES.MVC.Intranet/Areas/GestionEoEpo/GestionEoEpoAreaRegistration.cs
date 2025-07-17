using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.GestionEoEpo
{
    public class GestionEoEpoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "GestionEoEpo";
            }
        }
         
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "GestionEoEpo_default",
                "GestionEoEpo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
