using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CostoOportunidad
{
    public class CostoOportunidadAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CostoOportunidad";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CostoOportunidad_default",
                "CostoOportunidad/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
