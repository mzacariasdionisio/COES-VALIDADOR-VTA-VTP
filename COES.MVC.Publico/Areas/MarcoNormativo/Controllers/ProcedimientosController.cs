using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MarcoNormativo.Controllers
{
    public class ProcedimientosController : Controller
    {
        //
        // GET: /MarcoNormativo/Procedimientos/

        public ActionResult Administrativos()
        {
            return View();
        }

        public ActionResult Tecnicos()
        {
            return View();
        }

        public ActionResult InformacionGeneral()
        {
            return View();
        }


    }
}
