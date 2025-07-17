using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Publico.Models;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Publico.Controllers
{
    public class InformacionController : Controller
    {
        //
        // GET: /Organizacion/

        PortalAppServicio servicio = new PortalAppServicio();

        public ActionResult Campania()
        {
            ViewBag.Title = "Información - Campañía";
            return View();
        }
    }
}