using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Manttos
{
    public class ManttosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manttos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manttos_default",
                "Manttos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}