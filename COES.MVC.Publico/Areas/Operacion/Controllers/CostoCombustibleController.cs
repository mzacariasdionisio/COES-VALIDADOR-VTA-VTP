using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Operacion.Helper;
using COES.MVC.Publico.Areas.Operacion.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class CostoCombustibleController : Controller
    {
        /// <summary>
        /// Instancia de la clase aplicación
        /// </summary>
        DespachoAppServicio servicio = new DespachoAppServicio();

        /// <summary>
        /// Primera página del aplicativo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            PrecioCombustibleModel model = new PrecioCombustibleModel();
            model.ListaEmpresas = new List<SiEmpresaDTO>();// this.servicio.ObtenerEmpresasPreciosCombustibles();

            return View(model);
        }

        /// <summary>
        /// Permite listar los precios de combustibles
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(int? idEmpresa)
        {
            PrecioCombustibleModel model = new PrecioCombustibleModel();
            model.ListaFormula = this.servicio.ObtenerReportePrecioCombustible(idEmpresa);

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el reporte precios de combustibles
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int? idEmpresa)
        {
            int result = 1;
            try
            {
                List<PrGrupodatDTO> list = this.servicio.ObtenerReportePrecioCombustible(idEmpresa);
                OperacionHelper.GenerarReportePrecioCombustible(list);                

                result = 1;
            }
            catch
            {
                result = -1;
            }

            return Json(result);
        }

        /// <summary>
        /// Permite exportar los datos de los precios de combustibles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + NombreArchivo.ReportePrecioCombustible;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReportePrecioCombustible);
        }
    }
}
