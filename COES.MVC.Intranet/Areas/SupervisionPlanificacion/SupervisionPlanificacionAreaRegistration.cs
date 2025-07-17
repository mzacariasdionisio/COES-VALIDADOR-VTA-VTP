using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SupervisionPlanificacion
{
    public class SupervisionPlanificacionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SupervisionPlanificacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SupervisionPlanificacion_default",
                "SupervisionPlanificacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
