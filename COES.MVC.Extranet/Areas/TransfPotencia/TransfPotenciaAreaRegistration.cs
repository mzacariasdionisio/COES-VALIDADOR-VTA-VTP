using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.TransfPotencia
{
    public class TransfPotenciaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TransfPotencia";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TransfPotencia_default",
                "TransfPotencia/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
