using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion.Controllers
{
    public class GasNaturalController : Controller
    {
        //
        // GET: /PostOperacion/GasNatural/

        public ActionResult DeclaracionPreciosGas()
        {
            return View();
        }
        public ActionResult PreciosDeclarados()
        {
            return View();
        }

        public ActionResult DocumentosRelacionados()
        {
            return View();
        }
    }
}
