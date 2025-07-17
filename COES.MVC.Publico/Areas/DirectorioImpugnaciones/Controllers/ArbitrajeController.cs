using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DirectorioImpugnaciones.Controllers
{
    public class ArbitrajeController : Controller
    {        
        public ActionResult LaudosArbitrales()
        {
            return View();
        }
       
        public ActionResult Solicitudes()
        {
            return View();
        }
    }
}
