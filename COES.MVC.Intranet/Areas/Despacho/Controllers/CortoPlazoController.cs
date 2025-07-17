using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Despacho.Helper;
using COES.MVC.Intranet.Areas.Despacho.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using log4net;

namespace COES.MVC.Intranet.Areas.Despacho.Controllers
{
    public class CortoPlazoController : BaseController
    {        
        /// <summary>
        /// Pagina Inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CortoPlazoModel model = new CortoPlazoModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaRestricciones = (new McpAppServicio()).ObtenerTipoRestricciones();
            return View(model);
        }

        [HttpPost]
        public JsonResult ObtenerEscenarios(string fecha, int tipo)
        {
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                var list = (new McpAppServicio()).ObtenerEscenariosPorDiaConsulta(fechaProceso, tipo).OrderBy(x=>x.Topcodi).ToList();
                return Json(list);
            }
            catch
            {
                return Json(-1);
            }
        }

        [HttpPost]
        public PartialViewResult Listado(int idEscenario, string fecha, int tipoInformacion)
        {
            CortoPlazoModel model = new CortoPlazoModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<CpMedicion48DTO> list = (new McpAppServicio()).ObtenerDatosEscenario(idEscenario, fechaConsulta, tipoInformacion);
            
            model.ListaMedicion = list;
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult Exportar(int idEscenario, string fecha, int tipoInformacion)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<CpMedicion48DTO> list = (new McpAppServicio()).ObtenerDatosEscenario(idEscenario, fechaConsulta, tipoInformacion);
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionDespacho;
                string file = NombreArchivo.ReporteDatosYupana;
                ExcelDocument.GenerarReporteCortoPlazo(list, path, file);
             

                return Json(1);
            }
            catch
            {
                return Json(-11);
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionDespacho + NombreArchivo.ReporteDatosYupana;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteDatosYupana);
        }

        [HttpPost]
        public JsonResult GenerarReporte(string fecha)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);                

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionDespacho;
                string file = NombreArchivo.ReporteResumenYupana;
                int result = (new McpAppServicio()).ReporteResumen(fechaConsulta, path, file);

                return Json(new { Result = 1, Fecha = fechaConsulta.ToString("ddMMyy") });
            }
            catch
            {
                return Json(new { Result = -1, Fecha = string.Empty });
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarReporte(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionDespacho + NombreArchivo.ReporteResumenYupana;
            return File(fullPath, Constantes.AppExcel, string.Format(NombreArchivo.ReporteYupanaDescarga, fecha));
        }
    }
}
