using COES.MVC.Intranet.Areas.Demo.Models;
using COES.Servicios.Aplicacion.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Demo.Controllers
{
    public class DemoController : Controller
    {
        DemoAppServicio servicio = new DemoAppServicio();

        //
        // GET: /Demo/Demo/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string nombre)
        {
            DemoModel model = new DemoModel();
            model.ListaPrueba = this.servicio.BuscarPorNombre(nombre);

            return PartialView(model);
        }

        /// <summary>
        /// Planificacion demo
        /// </summary>
        /// <returns></returns>
        public ActionResult Planificacion()
        {
            return View();
        }
    }
}
