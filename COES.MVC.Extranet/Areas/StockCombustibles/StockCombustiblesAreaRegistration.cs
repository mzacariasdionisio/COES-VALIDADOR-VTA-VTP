using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.StockCombustibles
{
    public class StockCombustiblesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "StockCombustibles";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "StockCombustibles_default",
                "StockCombustibles/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
