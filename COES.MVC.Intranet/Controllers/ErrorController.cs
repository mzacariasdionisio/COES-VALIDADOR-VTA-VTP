using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace COES.MVC.Intranet.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Default()
        {
            return View("Error");
        }

        public ActionResult Error404()
        {
            return View("404");
        }
    }
}