using COES.MVC.Publico.Areas.MercadoMayorista.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.MercadoMayorista.Controllers
{
    public class VisualizacionDatosController : Controller
    {
        //
        // GET: /MercadoMayorista/VisualizacionDatos/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Indicadores()
        {
            return View();
        }

        public ActionResult Informacion()
        {
            return View();
        }

        public ActionResult Porcentaje() 
        {
            return View();
        }

        public ActionResult Valorizacion() 
        {
            return View();
        }

        public ActionResult ListadoParticipantes()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Listado(int tipo)
        {
            ListaParticipantesModel model = new ListaParticipantesModel();
            model.Listado = (new RegistroIntegrantesAppServicio()).ObtenerAgentesParticipantes(tipo);
            return PartialView(model);
        }
    }
}
