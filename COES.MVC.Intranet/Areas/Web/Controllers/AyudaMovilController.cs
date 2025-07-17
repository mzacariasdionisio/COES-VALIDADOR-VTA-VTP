using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Web.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class AyudaMovilController : BaseController
    {
        //
        // GET: /Web/AyudaMovil/

        public ActionResult Index()
        {
            return View();
        }
    
        [HttpPost]
        public PartialViewResult Listar()
        {
            AyudaMovilModel model = new AyudaMovilModel();
            model.Listado = (new PortalAppServicio()).ObtenerListaAyudaApp();
            return PartialView(model);
        }

        /// <summary>
        /// Muestra los datos de la ventana
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Editar(int id)
        {
            AyudaMovilModel model = new AyudaMovilModel();
            WbAyudaappDTO ayuda = (new PortalAppServicio()).ObtenerAyudaVentanaApp(id);
            model.Codigo = ayuda.Ayuappcodi;
            model.IdVentana = ayuda.Ayuappcodigoventana;
            model.Indicador = ayuda.Ayuappestado;
            model.NombreVentana = ayuda.Ayuappdescripcionventana;
            model.TextoAyuda = ayuda.Ayuappmensaje;
            model.TextoAyudaEng = ayuda.Ayuappmensajeeng;

            if (string.IsNullOrEmpty(model.TextoAyuda)) model.TextoAyuda = "<span></span>";
            if (string.IsNullOrEmpty(model.TextoAyudaEng)) model.TextoAyudaEng = "<span></span>";

            return Json(model);
        }

        /// <summary>
        /// Permite grabar los datos de la ayuda app movil
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(AyudaMovilModel model)
        {
            WbAyudaappDTO entity = new WbAyudaappDTO();
            entity.Ayuappcodi = model.Codigo;
            entity.Ayuappestado = model.Indicador;
            entity.Ayuappmensaje = model.TextoAyuda;
            entity.Ayuappmensajeeng = model.TextoAyudaEng;
            entity.Ayuappusumodificacion = base.UserName;
            int result = (new PortalAppServicio()).GrabarAyudaVentana(entity);
            return Json(result);
        }
    }
}
