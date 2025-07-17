using COES.Storage.App.Metadata.Entidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Controllers
{
    public class SiteMapController : Controller
    {
       
        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //List<WbMenuDTO> list = (new COES.Storage.App.Servicio.Portal()).ObtenerMenuPortal();
            //string menu = Helper.Helper.ObtenerTreeOpciones(list, string.Empty);
            //ViewBag.Menu = menu;
            return View();
        }

        /// <summary>
        /// Permite mostrar el menu de la aplicacion
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            List<WbMenuDTO> list = (new COES.Storage.App.Servicio.Portal()).ObtenerMenuPortal();
            string menu = Helper.Helper.ObtenerNewMenuSalaPrensa(list, string.Empty);
            ViewBag.Menu = menu;
            return PartialView();
        }

    }
}
