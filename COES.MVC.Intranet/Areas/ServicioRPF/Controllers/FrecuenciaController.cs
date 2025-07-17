using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Helper;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.ServicioRPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class FrecuenciaController : BaseController
    {
        /// <summary>
        /// Instancia de clase para acceso a datos
        /// </summary>
        RpfAppServicio servicio = new RpfAppServicio();

        /// <summary>
        /// Muestra la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ServicioModel model = new ServicioModel();
            model.FechaConsulta = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Muestra el listado de gps
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fecha)
        {
            ServicioModel model = new ServicioModel();
            DateTime fechaConsulta = DateTime.Now.AddDays(-1);

            if (!string.IsNullOrEmpty(fecha))
            {
                fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            model.ListaGPS = this.servicio.ObtenerConsultaGPS(fechaConsulta);

            if (base.IdArea == 1 || base.IdArea == 17 || base.IdArea == 8)
            {
                model.IndicadorExportar = true;
            }
            return PartialView(model);
        }

        /// Permite generar el archivo de frecuencias al segundo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fecha, int gpscodi)
        {
            int indicador = 1;

            try
            {
                if (!string.IsNullOrEmpty(fecha))
                {
                    DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    List<ServicioGps> list = this.servicio.ObtenerConsultaFrecuencia(fechaConsulta, gpscodi);
                    ExcelDocument.GenerarArchivoFrecuencias(list);
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + NombreArchivo.ReporteFrecuenciasRPF;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteFrecuenciasRPF);
        }



        /// Permite generar el archivo de frecuencias al segundo
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarMacro(string fecha, int gpscodi, string nombre)
        {
            int indicador = 1;

            try
            {
                if (!string.IsNullOrEmpty(fecha))
                {
                    string file = string.Format(NombreArchivo.ReporteEvaFrecuenciaXLS, nombre);
                    DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    List<ServicioGps> list = this.servicio.ObtenerConsultaFrecuencia(fechaConsulta, gpscodi);
                    ExcelDocument.GenerarArchivoEvaFrecuencias(list, file, fechaConsulta);
                }
            }
            catch(Exception ex)
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarMacro(string nombre)
        {
            string file = string.Format(NombreArchivo.ReporteEvaFrecuenciaXLS, nombre);
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, file);
        }


        /// <summary>
        /// Permite completar o reemplazar los datos de un gps
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="gpsorigen"></param>
        /// <param name="gpsdestino"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reemplazar(string fecha, int gpsorigen, int gpsdestino)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                this.servicio.ReemplazarFrecuencias(fechaConsulta, gpsorigen, gpsdestino);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite completar los datos de la frecuencia de san juan
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Completar(string fecha)
        {
            try
            {
                DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int resultado = this.servicio.CompletarFrecuenciaSanJuan(fechaConsulta);

                return Json(resultado);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
