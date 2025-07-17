using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen
{
    public class InformeEjecutivoMenAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InformeEjecutivoMen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InformeEjecutivoMen_default",
                "InformeEjecutivoMen/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
