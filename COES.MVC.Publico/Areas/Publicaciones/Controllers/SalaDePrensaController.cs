using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Publico.Models;
using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Helper;


namespace COES.MVC.Publico.Areas.Publicaciones.Controllers
{
    public class SalaDePrensaController : Controller
    {
        // GET: SalaDePrensa
        //public ActionResult Index()
        //{
        //    return View();
        //}

        /// <summary>
        /// Instancia de la clase servicio correspondiente
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        public ActionResult SalaPrensa()
        {
            HomeModel model = new HomeModel();
            model.ListaEventos = this.servicio.ListarResumenEventosWeb(DateTime.Today).OrderByDescending(x => (DateTime)x.Evenini).ToList();
            var ListComuni = new List<WbComunicadosDTO>();
            ListComuni = this.servicio.ListarComunicados().Where(x => x.Comtipo == "S" && x.Comfechaini <=DateTime.Today && x.Comfechafin >= DateTime.Today).OrderByDescending(x => x.Comcodi).ToList();

            model.ListaComunicado = ListComuni.Take(30).ToList();
            model.ListaBanner = (new COES.Storage.App.Servicio.Portal()).ObtenerBannerPortal();
            ViewBag.IndicadorAviso = Constantes.SI;
            return View(model);
        }

        public ActionResult AppMovil()
        {
            return View();
        }



    }
}