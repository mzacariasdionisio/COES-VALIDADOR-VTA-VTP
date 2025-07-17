using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Planificacion.Controllers
{
    public class InterconexionController : Controller
    {
        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
