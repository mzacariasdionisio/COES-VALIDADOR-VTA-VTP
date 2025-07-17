using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.InformacionAgentes
{
    public class InformacionAgentesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InformacionAgentes";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InformacionAgentes_default",
                "InformacionAgentes/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
