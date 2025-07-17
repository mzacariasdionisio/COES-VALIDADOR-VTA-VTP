using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Yupana.Models;
using COES.Base.Core;
using COES.MVC.Intranet.Areas.Yupana.Helper;
using COES.Servicios.Aplicacion.Yupana;
using COES.MVC.Intranet.Helper;
using System.Globalization;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Yupana.Controllers
{
    public class ResultadoController : Controller
    {
        //
        // GET: /Yupana/Resultado/
        YupanaAppServicio servicio = new YupanaAppServicio();
        public ActionResult Index()
        {
            ResultadoModel model = new ResultadoModel();
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaVariable = FuncionHelper.GetListaVariablesOut();
            model.ListaEcuacion = FuncionHelper.GetListaEcuacionesOutGams();
            model.ListaCosto = FuncionHelper.GetListaCostosOutGams();
            return View(model);
        }

        /// <summary>
        /// Vista para mostrar el listado de VAriables de Resultado en Web
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int topcodi, short srestcodi, string fecha, int orientacion, int costo)
        {
            DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            ResultadoModel model = new ResultadoModel();
            if (costo == 1)
                model.Resultado = servicio.GenerarReporteTablaCosto(topcodi);
            else
                model.Resultado = (orientacion == 1) ? servicio.GenerarReporteResultadoHorizontal(topcodi, srestcodi, dfecha) : servicio.GenerarReporteResultadoTransversal(topcodi, srestcodi, dfecha);
            return PartialView(model);
        }

        /// <summary>
        /// Genera archivo excel del listado de resultado para descarga
        /// </summary>
        /// <param name="topcodi"></param>
        /// <param name="srestcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraReporteResultadoXls(int topcodi, short srestcodi, string fecha)
        {
            int indicador = 1;
            try
            {
                DateTime dfecha = fecha != null && fecha.Trim() != string.Empty ? DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteYupana;
                string archivReporte = ruta + YupanaArchivos.ReporteSalida;
                CpTopologiaDTO topologia = servicio.GetTopologia(topcodi);
                DateTime finicio = topologia.Topfecha.AddDays(topologia.Topinicio);
                DateTime ffin = topologia.Topfecha.AddDays(topologia.Topinicio + topologia.Topdiasproc);
                servicio.GenerarExcelListadoSalida(archivReporte, topcodi, srestcodi, dfecha);
            }
            catch (Exception ex)
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general a formato excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporteSalida()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteYupana;
            string nombreArchivo = YupanaArchivos.ReporteSalida;
            string fullPath = ruta + YupanaArchivos.ReporteSalida;
            return File(fullPath, YupanaArchivos.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Genera el listado de escenario para filto popup en web
        /// </summary>
        /// <param name="fechaini"></param>
        /// <param name="fechafin"></param>
        /// <param name="idTipo"></param>
        /// <param name="nombre"></param>
        /// <param name="topcodi"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaEscenario(string fechaini, string fechafin, short idTipo, string nombre, int topcodi, int busqueda)
        {
            ResultadoModel model = new ResultadoModel();
            switch (busqueda)
            {
                case 0:
                    DateTime dfechaini = fechaini != null && fechaini.Trim() != string.Empty ? DateTime.ParseExact(fechaini, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                    DateTime dfechafin = fechafin != null && fechafin.Trim() != string.Empty ? DateTime.ParseExact(fechafin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
                    model.ListaTopologia = servicio.ListaTopologia(dfechaini, dfechafin, idTipo);
                    break;
                case 1:
                    model.ListaTopologia = servicio.ListaTopologiaXNombre(nombre);
                    break;
                case 2:
                    var topologia = servicio.GetTopologia(topcodi);
                    model.ListaTopologia = new List<CpTopologiaDTO>();
                    if (topologia != null)
                        model.ListaTopologia.Add(topologia);
                    break;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Genera archivo de reporte RSF para descarga
        /// </summary>
        /// <param name="topcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraReporteRsfXls(int topcodi)
        {
            int indicador = 1;
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteYupana;
                string archivReporte = ruta + YupanaArchivos.ReporteRsf;
                CpTopologiaDTO topologia = servicio.GetTopologia(topcodi);
                DateTime finicio = topologia.Topfecha.AddDays(topologia.Topinicio);
                DateTime ffin = topologia.Topfecha.AddDays(topologia.Topinicio + topologia.Topdiasproc);
                servicio.GeneraReporteRSF(archivReporte, topcodi, finicio, ffin);
            }
            catch (Exception ex)
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general a formato excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporteRsf()
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioReporteYupana;
            string nombreArchivo = YupanaArchivos.ReporteRsf;
            string fullPath = ruta + YupanaArchivos.ReporteRsf;
            return File(fullPath, YupanaArchivos.AppExcel, nombreArchivo);
        }

    }
}
