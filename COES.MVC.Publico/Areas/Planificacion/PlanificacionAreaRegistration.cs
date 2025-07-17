using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Planificacion
{
    public class PlanificacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Planificacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Planificacion_default",
                "Planificacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
