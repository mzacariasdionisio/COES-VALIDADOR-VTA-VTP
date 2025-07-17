using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class EstudiosController : Controller
    {
        //
        // GET: /Operacion/Estudios/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Comercializacion()
        {
            return View();
        }

        public ActionResult CostosVariables()
        {
            return View();
        }

        public ActionResult CostosVariablesNoCom()
        {
            return View();
        }

        public ActionResult Hidrologia()
        {
            return View();
        }
        public ActionResult HomologacionModelos()
        {
            return View();
        }
        public ActionResult OperacionSEIN()
        {
            return View();
        }
        public ActionResult PotenciaEfectiva()
        {
            return View();
        }
        public ActionResult PotenciaMinima()
        {
            return View();
        }
    }
}
