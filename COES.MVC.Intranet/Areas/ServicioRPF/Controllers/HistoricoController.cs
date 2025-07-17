using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class HistoricoController : BaseController
    {
        /// <summary>
        /// Carga de archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Ruta = base.PathFiles;
            return View();
        }

    }
}
