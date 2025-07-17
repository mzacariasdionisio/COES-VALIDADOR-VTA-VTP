using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Sorteo
{
    public class SorteoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sorteo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sorteo_default",
                "Sorteo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}