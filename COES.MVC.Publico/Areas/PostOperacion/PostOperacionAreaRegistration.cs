using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion
{
    public class PostOperacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PostOperacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PostOperacion_default",
                "PostOperacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
