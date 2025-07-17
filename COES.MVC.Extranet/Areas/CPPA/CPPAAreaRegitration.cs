using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CPPA
{
    public class CPPAAreaRegitration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CPPA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CPPA_default",
                "CPPA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}