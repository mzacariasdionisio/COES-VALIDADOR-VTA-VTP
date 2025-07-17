using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Valorizacion.Controllers
{
    public class RegistroController : Controller
    {
        /// <summary>
        /// Llama a la página principal del listado de revisiones
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Paginado(string fecha)
        {
            return PartialView();
        }
 
    }
}
