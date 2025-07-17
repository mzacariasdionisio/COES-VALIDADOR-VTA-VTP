using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPF
{
    public class ServicioRPFAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ServicioRPF";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ServicioRPF_default",
                "ServicioRPF/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
