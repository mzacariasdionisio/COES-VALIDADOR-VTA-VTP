using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ReporteSegundosFaltantesController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ActionResult Index()
        {
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            List<ReporteSegundosFaltantesDTO> lista = new List<ReporteSegundosFaltantesDTO>();
            List<EquipoGPSDTO> listaEquipos = new List<EquipoGPSDTO>();

            List<EquipoGPSDTO> listaEquiposSelect = new List<EquipoGPSDTO>();

            DateTime date = DateTime.Now;
            model.FechaInicial = date;
            int intAnio = 0;
            int intMes = 0;
            if (model.FechaInicial.Month == 1)
            {
                intAnio = date.Year - 1;
                intMes = 12;
            }
            else
            {
                intAnio = date.Year;
                intMes = model.FechaInicial.Month - 1;
            }
            model.FechaInicial = new DateTime(intAnio, intMes, 1, 0, 0, 0);
            var fechaFinal = model.FechaInicial.AddMonths(1).AddDays(-1);
            model.FechaFinal = new DateTime(fechaFinal.Year, fechaFinal.Month, fechaFinal.Day, 23, 59, 59);
            model.ListaReporte = lista;
            List<string> listaFechas = new List<string>();
            model.ListaFechas = listaFechas;
            model.ListaEquipos = listaEquipos;

            listaEquiposSelect = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(x=> x.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(listaEquiposSelect, "GPSCODI", "NOMBREEQUIPO");

            List<SelectListItem> listaOficial = new List<SelectListItem>();
            listaOficial.Add(new SelectListItem { Text = "NO", Value = "N" });
            listaOficial.Add(new SelectListItem { Text = "SI", Value = "S" });
            TempData["ListaOficial"] = listaOficial;

            return View(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(ReporteSegundosFaltantesModel model)
        {
            string mensajeError = "";
            List<string>  listaFechas = new List<string>();
            List<EquipoGPSDTO> listaGPS = new List<EquipoGPSDTO>();
            List<ReporteSegundosFaltantesDTO> listaReporteSegundosFaltantes = new List<ReporteSegundosFaltantesDTO>();


            if (DateTime.ParseExact(model.FechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            } else
            {
                ReporteSegundosFaltantesParam param = new ReporteSegundosFaltantesParam();
                param.FechaInicial = model.FechaIni;
                param.FechaFinal = model.FechaFin;
                param.IdGPS = model.IdEquipo;
                param.IndOficial = model.IndOficial;
                //listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteSegundosFaltantes(param);
                listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteTotalSegundosFaltantes(param);
                listaGPS = new EquipoGPSAppServicio().GetListaEquipoGPSPorFiltro(param.IdGPS, param.IndOficial);           

                DateTime start = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(param.FechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                TimeSpan difference = end - start;             
                for (int i = 0; i <= difference.Days; i++)
                {
                    DateTime fechaInicio = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime fechaActual = fechaInicio.AddDays(i);
                    listaFechas.Add(fechaActual.ToString(Constantes.FormatoFecha));
                }
            }

            model.ListaEquipos = listaGPS;
            model.ListaFechas = listaFechas;
            model.ListaReporte = listaReporteSegundosFaltantes;
            return PartialView(model);
        }

        /// <summary>
        /// Genera el reporte excel 
        /// </summary>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="IdEquipo"></param>
        /// <param name="IndOficial"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteXls(string fechaIni, string fechaFin, int IdEquipo, string IndOficial)
        {
            int indicador = 1;
            try
            {
                ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
                model.FechaIni = fechaIni;
                model.FechaFin = fechaFin;
                model.IdEquipo = IdEquipo;
                model.IndOficial = IndOficial;
                string mensajeError = "";
                List<string> listaFechas = new List<string>();
                List<EquipoGPSDTO> listaGPS = new List<EquipoGPSDTO>();
                List<ReporteSegundosFaltantesDTO> listaReporteSegundosFaltantes = new List<ReporteSegundosFaltantesDTO>();

                if (DateTime.ParseExact(model.FechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture) > DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture))
                {
                    mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                    TempData["sMensajeExito"] = mensajeError;
                }
                else
                {
                    ReporteSegundosFaltantesParam param = new ReporteSegundosFaltantesParam();
                    param.FechaInicial = model.FechaIni;
                    param.FechaFinal = model.FechaFin;
                    param.IdGPS = model.IdEquipo;
                    param.IndOficial = model.IndOficial;
                    listaReporteSegundosFaltantes = new ReporteSegundosFaltantesAppServicio().GetReporteTotalSegundosFaltantes(param);
                    listaGPS = new EquipoGPSAppServicio().GetListaEquipoGPSPorFiltro(param.IdGPS, param.IndOficial);

                    DateTime start = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    DateTime end = DateTime.ParseExact(param.FechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                    TimeSpan difference = end - start;
                    for (int i = 0; i <= difference.Days; i++)
                    {
                        DateTime fechaInicio = DateTime.ParseExact(param.FechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        DateTime fechaActual = fechaInicio.AddDays(i);
                        listaFechas.Add(fechaActual.ToString(Constantes.FormatoFecha));
                    }
                }

                model.ListaEquipos = listaGPS;
                model.ListaFechas = listaFechas;
                model.ListaReporte = listaReporteSegundosFaltantes;

                ExcelDocument.GernerarArchivoReporteSegundosFaltantes(model);
                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                throw ex;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Descarga el reporte excel del servidor
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = FormatoReportesFrecuencia .NombreReporteSegundosFaltantes;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesReportesFrecuencia.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);

        }

        
    }
}