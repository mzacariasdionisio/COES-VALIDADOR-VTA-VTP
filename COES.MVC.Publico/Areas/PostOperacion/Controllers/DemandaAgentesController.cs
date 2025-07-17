using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.PostOperacion.Controllers
{
    public class DemandaAgentesController : Controller
    {
        //
        // GET: /PostOperacion/DemandaAgentes/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Distribuidores()
        {
            return View();
        }
        public ActionResult SEIN()
        {
            return View();
        }
        public ActionResult UsuariosLibres()
        {
            return View();
        }
    }
}
