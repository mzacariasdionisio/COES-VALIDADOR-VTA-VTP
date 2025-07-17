﻿using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Migraciones
{
    public class MigracionesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Migraciones";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Migraciones_default",
                "Migraciones/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
