using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Campanias
{
    public class CampaniasAreaRegistration:AreaRegistration
    {

        public override string AreaName
        {
            get
            {
                return "Campanias";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Campanias_default",
                "Campanias/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}