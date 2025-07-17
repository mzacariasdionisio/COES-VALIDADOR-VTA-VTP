using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MarcoNormativo
{
    public class MarcoNormativoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MarcoNormativo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MarcoNormativo_default",
                "MarcoNormativo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
