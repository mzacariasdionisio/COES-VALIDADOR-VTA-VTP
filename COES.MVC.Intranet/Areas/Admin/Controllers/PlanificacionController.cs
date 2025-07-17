using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Admin.Helper;
using COES.MVC.Intranet.Areas.Admin.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    public class PlanificacionController : BaseController
    {
        /// <summary>
        /// Instancia de la clase de servicio1
        /// </summary>
        GeneralAppServicio servicio = new GeneralAppServicio();

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PlanificacionModel model = new PlanificacionModel();
            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);            
            return View(model);
        }

        /// <summary>
        /// Inicio de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Listado(string fechaInicio, string fechaFin)
        {
            PlanificacionModel model = new PlanificacionModel();

            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.ListaRegistro = this.servicio.ObtenerReporteDescarga(fecInicio, fecFin);
            
            return View(model);
        }
        
    }
}
