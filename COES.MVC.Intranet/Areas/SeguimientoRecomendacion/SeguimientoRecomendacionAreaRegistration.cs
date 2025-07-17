using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion
{
    public class ReservaFriaNodoEnergeticoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SeguimientoRecomendacion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SeguimientoRecomendacion_default",
                "SeguimientoRecomendacion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
