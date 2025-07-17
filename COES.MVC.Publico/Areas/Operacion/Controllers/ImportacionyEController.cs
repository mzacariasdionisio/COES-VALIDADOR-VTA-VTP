using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class ImportacionyEController : Controller
    {
        //
        // GET: /Operacion/ImportacionyE/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CapacidadImport()
        {
            return View();
        }

        public ActionResult ExcedentesExp()
        {
            return View();
        }
    }
}
