using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.RDO
{
    public class RDOAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RDO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RDO_default",
                "RDO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}