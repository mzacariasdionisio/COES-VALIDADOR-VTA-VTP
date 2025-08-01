﻿using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Demo
{
    public class DemoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Demo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Demo_default",
                "Demo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
