using COES.Dominio.DTO.Sic;
using COES.MVC.Publico.Areas.Mediciones.Helpers;
using COES.MVC.Publico.Areas.Mediciones.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Mediciones.Controllers
{
    public class EjecutadoController : Controller
    {
        /// <summary>
        /// Instancia de la clase de aplicacion respectiva
        /// </summary>
        EjecutadoAppServicio servicio = new EjecutadoAppServicio();

        #region Metodos Consulta Ejecutado diario

        /// <summary>
        /// Cpnsulta inicial del formulario
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            EjecutadoModel model = new EjecutadoModel();
                            
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            DateTime fecha = DateTime.Now.AddMonths(-1);
            DateTime fechaInicio = new DateTime(fecha.Year, fecha.Month, 1);
            DateTime fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            model.FechaInicio = fechaInicio.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);

            return View(model);
        }
        
        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            EjecutadoModel model = new EjecutadoModel();
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Permite listar los registros de medidores de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, int nroPagina)
        {
            EjecutadoModel model = new EjecutadoModel();
            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(fechaFinal))
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

            List<MeMedicion48DTO> sumatoria = new List<MeMedicion48DTO>();
            model.ListaDatos = this.servicio.ObtenerConsultaEjecutado(fecInicio, fecFin, tiposEmpresa, empresas,
                tiposGeneracion, nroPagina, Constantes.PageSizeMedidores, out sumatoria);
            model.EntidadTotal = sumatoria;

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaInicial, string fechaFinal, string tiposEmpresa,
            string empresas, string tiposGeneracion)
        {
            EjecutadoModel model = new EjecutadoModel();
            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))            
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);            
            if (!string.IsNullOrEmpty(fechaFinal))            
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            
            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;       

            int nroRegistros = this.servicio.ObtenerNroRegistrosConsulta(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeMedidores;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a formato Excel el resultado de la consulta
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string tiposEmpresa,
            string empresas, string tiposGeneracion)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFinal))
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = NombreArchivo.ReporteEjecutadoDiario;

                TimeSpan ts = fecFin.Subtract(fecInicio);

                if (ts.TotalDays <= 365)
                {
                    this.servicio.GenerarArchivoExportacion(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, path, file);
                    return Json(1);
                }
                else
                {
                    return Json(2);
                }
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteEjecutadoDiario;
            string app = Constantes.AppExcel;            
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }

        #endregion

        #region Metodos consulta ejecutado acumulado mensual


        /// <summary>
        /// Muestra la pantalla inicial del formulario ejecutado acumulado
        /// </summary>
        /// <returns></returns>
        public ActionResult Acumulado()
        {
            AcumuladoModel model = new AcumuladoModel();
           
            model.FechaInicio = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoMesAnio);            
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            return View(model);            
        }

        /// <summary>
        /// Permite mostrar el resultado de la consulta ejecutado mensual
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultaAcumulado(string fechaInicio, string fechaFin, string tiposEmpresa, string empresas, 
            string tiposGeneracion) 
        {
            try
            {
                if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
                {
                    int mesIni = Int32.Parse(fechaInicio.Substring(0, 2));
                    int anioIni = Int32.Parse(fechaInicio.Substring(3, 4));
                    int mesFin = Int32.Parse(fechaFin.Substring(0, 2));
                    int anioFin = Int32.Parse(fechaFin.Substring(3, 4));
                    DateTime fecIni = new DateTime(anioIni, mesIni, 1);
                    DateTime fecFin = new DateTime(anioFin, mesFin, Tools.ObtenerNroDias(anioFin, mesFin));

                    if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                    if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                    List<MedicionReporteDTO> list = this.servicio.ObtenerReporteConsolidado(fecIni, fecFin,
                        tiposEmpresa, empresas, tiposGeneracion);

                    return Json(MedidorHelper.ObtenerReporteConsolidado(list, 1));
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite exportar el reporte de despacho ejecutado acumulado mensual
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarAcumulado(string fechaInicio, string fechaFin, string tiposEmpresa, string empresas,
            string tiposGeneracion)
        {
            try
            {

                if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
                {
                    int mesIni = Int32.Parse(fechaInicio.Substring(0, 2));
                    int anioIni = Int32.Parse(fechaInicio.Substring(3, 4));
                    int mesFin = Int32.Parse(fechaFin.Substring(0, 2));
                    int anioFin = Int32.Parse(fechaFin.Substring(3, 4));
                    DateTime fecIni = new DateTime(anioIni, mesIni, 1);
                    DateTime fecFin = new DateTime(anioFin, mesFin, Tools.ObtenerNroDias(anioFin, mesFin));

                    if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                    if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                    List<MedicionReporteDTO> list = this.servicio.ObtenerReporteConsolidado(fecIni, fecFin,
                         tiposEmpresa, empresas, tiposGeneracion);

                    MedidorHelper.GenerarReporteExcelConsolidado(list);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarAcumulado()
        {
            string file = NombreArchivo.ReporteEjecutadoAcumuladoMensual;
            string app = Constantes.AppExcel;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, app, file);
        }

        #endregion
    }
}
