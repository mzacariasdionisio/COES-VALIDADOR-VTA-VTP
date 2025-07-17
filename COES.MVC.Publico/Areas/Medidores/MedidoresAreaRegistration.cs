using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Medidores
{
    public class MedidoresAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Medidores";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Medidores_default",
                "Medidores/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
