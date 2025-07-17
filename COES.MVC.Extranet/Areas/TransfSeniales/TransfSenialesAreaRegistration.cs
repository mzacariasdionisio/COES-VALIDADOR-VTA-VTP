using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.TransfSeniales
{
    public class TransfSenialesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TransfSeniales";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "TransfSeniales_default",
                "TransfSeniales/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
