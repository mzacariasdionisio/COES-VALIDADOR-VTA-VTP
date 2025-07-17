using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Mediciones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Mediciones.Controllers
{
    public class ReporteMedidoresController : Controller
    {
        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Almacena el resumen por fuente de energía
        /// </summary>
        public List<MedicionReporteDTO> ListaFuenteEnergia
        {
            get
            {
                return (Session[DatosSesion.ListaFuenteEnergia] != null) ?
                    (List<MedicionReporteDTO>)Session[DatosSesion.ListaFuenteEnergia] : new List<MedicionReporteDTO>();
            }
            set { Session[DatosSesion.ListaFuenteEnergia] = value; }
        }

        /// <summary>
        /// Almacena el reporte por empresas
        /// </summary>
        public List<MeMedicion96DTO> ListaReporteEmpresas
        {
            get
            {
                return (Session[DatosSesion.ReporteEmpresas] != null) ?
                    (List<MeMedicion96DTO>)Session[DatosSesion.ReporteEmpresas] : new List<MeMedicion96DTO>();
            }
            set { Session[DatosSesion.ReporteEmpresas] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte por tipo de generación
        /// </summary>
        public List<MedicionReporteDTO> ListaTipoGeneracion
        {
            get
            {
                return (Session[DatosSesion.ListaTipoGeneracion] != null) ?
                    (List<MedicionReporteDTO>)Session[DatosSesion.ListaTipoGeneracion] : new List<MedicionReporteDTO>();
            }
            set { Session[DatosSesion.ListaTipoGeneracion] = value; }
        }

        ReporteMedidoresAppServicio servicio = new ReporteMedidoresAppServicio();

        /// <summary>
        /// Muestra la página inicial del módulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ReporteMedidoresModel model = new ReporteMedidoresModel();

            //model.ListaEmpresas = this.servicio.ListaEmpresa();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaFuenteEnergia = this.servicio.ListaFuenteEnergia(0);
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

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
            ReporteMedidoresModel model = new ReporteMedidoresModel();
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            model.ListaEmpresas = entitys;

            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar las empresas por los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(string tiposEmpresa)
        {
            List<SiEmpresaDTO> entitys = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);
            SelectList list = new SelectList(entitys, EntidadPropiedad.Emprcodi, EntidadPropiedad.Emprnomb);
            return Json(list);
        }

        /// <summary>
        /// Permite mostrar el reporte de medidores resumen
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="fuentesEnergia"></param>
        /// <param name="central"></param>
        /// <param name="indAdjudicado"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central)
        {
            ReporteMedidoresModel model = new ReporteMedidoresModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;
            if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = Constantes.ParametroDefecto;

            model.FechaInicio = fecInicio.ToString(Constantes.FormatoFecha);
            model.FechaFin = fecFin.ToString(Constantes.FormatoFecha);
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);

            List<MedicionReporteDTO> listCuadrosFE = new List<MedicionReporteDTO>();
            List<MedicionReporteDTO> listCuadrosTG = new List<MedicionReporteDTO>();
            List<MedicionReporteDTO> listCuadrosUnidades = new List<MedicionReporteDTO>();
            MedicionReporteDTO umbrales = new MedicionReporteDTO();
            List<MedicionReporteDTO> listFuenteEnergia = new List<MedicionReporteDTO>();
            List<MeMedicion96DTO> reporteEmpresas = new List<MeMedicion96DTO>();
            List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();

            this.servicio.ObtenerReporteMedidores(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, fuentesEnergia, central,
                out listCuadrosFE, out listCuadrosTG, out listCuadrosUnidades, out umbrales, out listFuenteEnergia, out reporteEmpresas, out resultadoTG,
                out List<LogErrorHOPvsMedidores> listaValidacion);

            model.ListaCuadrosFE = listCuadrosFE;
            model.ListaCuadrosTG = listCuadrosTG;
            model.DatosReporte = umbrales;
            model.ReporteFuenteEnergia = listFuenteEnergia;

            model.TieneAlertaValidacion = listaValidacion.Any();
            model.ListaValidacion = listaValidacion;

            this.ListaFuenteEnergia = listFuenteEnergia;
            this.ListaReporteEmpresas = reporteEmpresas;
            this.ListaTipoGeneracion = resultadoTG;

            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();

            ViewBag.Mensaje = "";

            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string tiposEmpresa, string empresas,
            string tiposGeneracion, string fuentesEnergia, int central)
        {
            try
            {
                ReporteMedidoresModel model = new ReporteMedidoresModel();

                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;
                if (string.IsNullOrEmpty(fuentesEnergia)) fuentesEnergia = Constantes.ParametroDefecto;

                model.FechaInicio = fecInicio.ToString(Constantes.FormatoFecha);
                model.FechaFin = fecFin.ToString(Constantes.FormatoFecha);
                model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);

                List<MedicionReporteDTO> listCuadrosFE = new List<MedicionReporteDTO>();
                List<MedicionReporteDTO> listCuadrosTG = new List<MedicionReporteDTO>();
                List<MedicionReporteDTO> listCuadrosUnidades = new List<MedicionReporteDTO>();
                MedicionReporteDTO umbrales = new MedicionReporteDTO();
                List<MedicionReporteDTO> listFuenteEnergia = new List<MedicionReporteDTO>();
                List<MeMedicion96DTO> reporteEmpresas = new List<MeMedicion96DTO>();
                List<MedicionReporteDTO> resultadoTG = new List<MedicionReporteDTO>();
                string mensajeValidacion;

                this.servicio.ObtenerReporteMedidores(fecInicio, fecFin, tiposEmpresa, empresas, tiposGeneracion, fuentesEnergia, central,
                         out listCuadrosFE, out listCuadrosTG, out listCuadrosUnidades, out umbrales, out listFuenteEnergia, out reporteEmpresas, out resultadoTG,
                         out List<LogErrorHOPvsMedidores> listaValidacion);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel.ToString();
                this.servicio.GenerarArchivoExcel(listCuadrosFE, listCuadrosTG, umbrales, listFuenteEnergia, DateTime.Now, fecInicio, fecFin,
                    path, NombreArchivo.ReporteMedidoresGeneracion);

                return Json(1);
            }
            catch(Exception ex)
            {
                Log.Error(NameController, ex);
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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + NombreArchivo.ReporteMedidoresGeneracion;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteMedidoresGeneracion);
        }

        /// <summary>
        /// Permite generar gráfico por fuente de energía
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grafico()
        {
            var list = this.ListaFuenteEnergia.Where(x => !string.IsNullOrEmpty(x.Fenergnomb)).
                Select(x => new { x.Fenergnomb, x.EnergiaFuenteEnergia, x.MDFuenteEnergia }).OrderBy(x => x.Fenergnomb).ToList();

            return Json(list);
        }

        /// <summary>
        /// Permite generar el gráfico por tipo de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoTipoGeneracion()
        {
            var list = this.ListaTipoGeneracion.
               Select(x => new { x.Tgenernomb, x.MDFuenteEnergia }).OrderBy(x => x.Tgenernomb).ToList();

            return Json(list);
        }

        /// <summary>
        /// Permite generar el gráfico por empresas y tipo de generación
        /// </summary>
        /// <param name="tipoGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoEmpresas(int tipoGeneracion)
        {
            if (tipoGeneracion == 0)
            {
                var listEmpresa = (from t in this.ListaReporteEmpresas
                                   group t by new { t.Emprcodi, t.Emprnomb }
                                       into destino
                                   select new MeMedicion96DTO()
                                   {
                                       Emprcodi = destino.Key.Emprcodi,
                                       Emprnomb = destino.Key.Emprnomb,
                                       Meditotal = destino.Sum(t => t.Meditotal)
                                   }).Select(x => new { x.Emprnomb, x.Meditotal }).OrderBy(x => x.Emprnomb).ToList();

                return Json(listEmpresa);
            }
            else
            {
                var list = this.ListaReporteEmpresas.Where(x => (x.Tgenercodi == tipoGeneracion || tipoGeneracion == 0)).
                    Select(x => new { x.Emprnomb, x.Meditotal }).OrderBy(x => x.Emprnomb).ToList();
                return Json(list);
            }
        }
    }
}
