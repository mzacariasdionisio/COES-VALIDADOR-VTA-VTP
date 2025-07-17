using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante
{
    public class RegistroIntegranteAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RegistroIntegrante";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "RegistroIntegrante_default",
                "RegistroIntegrante/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}