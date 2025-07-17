using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.ValorizacionDiaria
{
    public class ValorizacionDiariaAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ValorizacionDiaria";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ValorizacionDiaria_default",
                "ValorizacionDiaria/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
