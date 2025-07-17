using COES.MVC.Extranet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Controllers
{
    public class FrecuenciaController : Controller
    {
        public ActionResult Consulta()
        {
            FrecuenciaModel model = new FrecuenciaModel();

            //model.ListAnios.Add(DateTime.Now.Year - 1);
            //model.ListAnios.Add(DateTime.Now.Year);

            //for(int i = 1; )

            return View();
        }
    }
}
