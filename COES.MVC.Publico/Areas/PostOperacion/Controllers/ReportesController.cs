using COES.MVC.Publico.Areas.PostOperacion.Models;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion.Controllers
{
    public class ReportesController : Controller
    {
        //
        // GET: /PostOperacion/Reportes/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Idcc()
        {
            return View();
        }

        public ActionResult Idcos()
        {
            return View();
        }

        public ActionResult Ieod()
        {
            return View();
        }

        public ActionResult ReporteIndisponibilidad()
        {
            return View();
        }

        public ActionResult ReservaCompensableRPF()
        {
            return View();
        }

        public ActionResult HorasOperación()
        {
            return View();
        }

        /// <summary>
        /// Muestra el reporte de pruebas aleatorias
        /// </summary>
        /// <returns></returns>
        public ActionResult PruebasAleatorias()
        {
            PruebaAleatoriaModel model = new PruebaAleatoriaModel();
            ReportePortalAppServicio servicio = new ReportePortalAppServicio();
            model.ListadoSorteo = servicio.ListaLogSorteo();
            model.ListadoSituacionUnidades = servicio.ObtenerSituacionUnidades();
            model.ListadoMantenimientos = servicio.ObtenerMantenimientos();
            return View(model);
        }

        /// <summary>
        
        
    }
}
