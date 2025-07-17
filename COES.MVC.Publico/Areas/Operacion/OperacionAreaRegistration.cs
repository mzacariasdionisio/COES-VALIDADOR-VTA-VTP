using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion
{
    public class OperacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Operacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Operacion_default",
                "Operacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
