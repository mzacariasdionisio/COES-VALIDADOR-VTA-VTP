using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Interconexiones
{
    public class InterconexionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Interconexiones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Interconexiones_default",
                "Interconexiones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
