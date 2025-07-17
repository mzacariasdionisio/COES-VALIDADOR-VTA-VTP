using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Subastas
{
    public class SubastasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Subastas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Subastas_default",
                "Subastas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
