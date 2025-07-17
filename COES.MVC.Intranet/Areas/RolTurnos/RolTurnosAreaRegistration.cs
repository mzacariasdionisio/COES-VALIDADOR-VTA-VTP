using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RolTurnos
{
    public class RolTurnosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RolTurnos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RolTurnos_default",
                "RolTurnos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
