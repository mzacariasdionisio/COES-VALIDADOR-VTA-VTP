using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MercadoMayorista
{
    public class MercadoMayoristaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MercadoMayorista";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MercadoMayorista_default",
                "MercadoMayorista/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
