using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Equipamiento
{
    public class ProteccionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Proteccion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Proteccion_default",
                "Proteccion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "COES.MVC.Intranet.Areas.Proteccion.Controllers" }
            );
        }
    }
}
