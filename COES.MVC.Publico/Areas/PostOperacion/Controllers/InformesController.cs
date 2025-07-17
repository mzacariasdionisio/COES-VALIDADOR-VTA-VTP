using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion.Controllers
{
    public class InformesController : Controller
    {
        //
        // GET: /PostOperacion/Informes/

        public ActionResult AnalisisEconomicoDespacho()
        {
            return View();
        }

        public ActionResult EvaluacionAnual()
        {
            return View();
        }

        public ActionResult EvaluacionMensual()
        {
            return View();
        }

        public ActionResult EvaluacionSemanal()
        {
            return View();
        }

        public ActionResult ReservaRotanteRPF()
        {
            return View();
        }

        public ActionResult ReservaRotanteRSF()
        {
            return View();
        }

        public ActionResult UnidadesGeneracionPruebas()
        { 
            return View();
        }

        public ActionResult MagEnergiaDejadaInyectar()
        {
            return View();
        }

        public ActionResult ListadoURS()
        {
            return View();
        }

        /// Permite mostrar el mensaje
        /// </summary>
        /// <returns></returns>
        public ActionResult CompensacionConfiabilidad()
        {
            return View();
        }
    }
}
