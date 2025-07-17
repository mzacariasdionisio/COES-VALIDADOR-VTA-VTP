using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Controllers
{
    public class ContactoController : Controller
    {
        //
        // GET: /Contacto/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Muestra el listado de anexos
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Anexos()
        {
            return PartialView();
        }

    }
}
