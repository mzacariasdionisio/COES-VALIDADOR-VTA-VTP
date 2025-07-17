using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DevolucionAporte
{
    public class DevolucacionAporteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DevolucionAporte";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DevolucionAporte_default",
                "DevolucionAporte/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
