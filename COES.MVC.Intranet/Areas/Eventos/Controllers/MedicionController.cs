using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Eventos.Controllers
{
    public class MedicionController : Controller
    {
        //
        // GET: /Eventos/Medicion/

        public ActionResult Download()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Download(FormCollection form)
        {
            return View();
        }

    }
}
