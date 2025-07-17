using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo
{
    public class ServicioRPFNuevoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ServicioRPFNuevo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ServicioRPFNuevo_default",
                "ServicioRPFNuevo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
