using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IND
{
    public class INDAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IND";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IND_default",
                "IND/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}