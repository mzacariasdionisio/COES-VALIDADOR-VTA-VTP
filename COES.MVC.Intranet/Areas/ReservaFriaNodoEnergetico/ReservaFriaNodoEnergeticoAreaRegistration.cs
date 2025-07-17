using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReservaFriaNodoEnergetico
{
    public class ReservaFriaNodoEnergeticoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ReservaFriaNodoEnergetico";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ReservaFriaNodoEnergetico_default",
                "ReservaFriaNodoEnergetico/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
