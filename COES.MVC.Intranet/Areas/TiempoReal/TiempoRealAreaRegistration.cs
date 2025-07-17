using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.TiempoReal
{
    public class TiempoRealAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TiempoReal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TiempoReal_default",
                "TiempoReal/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
