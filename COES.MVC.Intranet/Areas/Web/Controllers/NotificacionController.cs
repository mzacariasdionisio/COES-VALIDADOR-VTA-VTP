using COES.Dominio.DTO.Sic;

using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Web.Controllers
{
    public class NotificacionController : BaseController
    {

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();

        //
        // GET: /Web/Notificacion/

        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Permite listar los registros de las notificaciones
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string titulo, string fechaInicio, string fechaFin)
        {
            WbNotificacionDTO model = new WbNotificacionDTO();

            DateTime? fecInicio = (!string.IsNullOrEmpty(fechaInicio)) ? (DateTime?)DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : null;
            DateTime? fecFin = (!string.IsNullOrEmpty(fechaFin)) ? (DateTime?)DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : null;
                       
            ViewBag.Listado = this.servicio.ListNotificaciones(titulo, fecInicio, fecFin);

            return PartialView();
        }

        /// <summary>
        /// Permite editar la notificacion
        /// </summary>
        /// <param name="idNotificacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Editar(int notiCodi)
        {
            var model = this.servicio.GetByIdNotificacion(notiCodi);
            model.FechaEjecucion = (notiCodi != 0) ? model.NotiEjecucion.ToString(Constantes.FormatoFechaHora) : string.Empty;

            return PartialView(model);
        }

        /// <summary>
        /// Permite eliminar la relación de equivalencia
        /// </summary>
        /// <param name="idRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Eliminar(int notiCodi)
        {
            try
            {
                this.servicio.DeleteNotificacion(notiCodi);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite grabar una notificacion
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grabar(WbNotificacionDTO model)
        {
            try
            {
                model.NotiUsuCreacion = base.UserName;
                model.NotiUsuModificacion = base.UserName;
                model.NotiEjecucion = DateTime.ParseExact(model.FechaEjecucion, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                int result = this.servicio.SaveWbNotificacion(model);
         
                return Json(result);

            }
            catch
            {
                return Json(-1);
            }
        }

    }
}
