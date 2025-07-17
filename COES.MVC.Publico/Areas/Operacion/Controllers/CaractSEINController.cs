using COES.MVC.Publico.Areas.Operacion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class CaractSEINController : Controller
    {
        //
        // GET: /Operacion/CaractSEIN/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaseDatos()
        {
            return View();
        }

        public ActionResult DiagramaUnifilar()
        {
            return View();
        }
        
        public ActionResult MapaSEIN()
        {
            return View();
        }

        public ActionResult DespachoProgramadoDiario()
        {
            return View();
        }

        public ActionResult DespachoProgramadoSemanal()
        {
            return View();
        }

        public ActionResult ConsultaFrecuenciaDiariaNTCSE()
        {
            return View();
        }

        public ActionResult ConsultaMantenimientos()
        {
            return View();
        }

        public ActionResult CostoVariables()
        {
            return View();
        }

        public ActionResult DespachoEjecutadoDiario()
        {
            return View();
        }

        public ActionResult MedidoresGeneracion()
        {
            return View();
        }

        public ActionResult PotenciaMediaHorariaIndisponible()
        {
            return View();
        }

        public PartialViewResult Menu()
        {
            return PartialView();
        }
    }
}
