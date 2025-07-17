using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MarcoNormativo.Controllers
{
    public class ProyectosController : Controller
    {
        //
        // GET: /MarcoNormativo/Proyectos/

        public ActionResult ConsultaIntegrantes()
        {
            return View();
        }
      
        public ActionResult RemitidosMEM()
        {
            return View();
        }
        public ActionResult TramiteAprobacion()
        {
            return View();
        }

    }
}
