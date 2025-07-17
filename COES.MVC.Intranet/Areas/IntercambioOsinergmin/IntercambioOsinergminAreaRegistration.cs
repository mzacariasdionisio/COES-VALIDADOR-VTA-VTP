using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin
{
    public class IntercambioOsinergminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IntercambioOsinergmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IntercambioOsinergmin_default",
                "IntercambioOsinergmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
