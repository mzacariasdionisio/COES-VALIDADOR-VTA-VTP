using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Transferencias
{
    public class TransferenciasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Transferencias";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Transferencias_default",
                "Transferencias/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
