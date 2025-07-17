using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Formulas
{
    public class FormulaSCADAAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Formulas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Formulas_default",
                "Formulas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
