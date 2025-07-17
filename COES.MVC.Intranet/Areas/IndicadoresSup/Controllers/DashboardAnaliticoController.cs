using COES.MVC.Intranet.Areas.IndicadoresSup.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IndicadoresSup.Controllers
{
    public class DashboardAnaliticoController : BaseController
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(DashboardAnaliticoController));
        private static readonly string NameController = "DashboardAnaliticoController";
        private readonly Siosein2AppServicio _servicioSiosein2;

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        public DashboardAnaliticoController()
        {
            _servicioSiosein2 = new Siosein2AppServicio();
        }

        #region propiedades y methodos Generales

        public const string ConstanteIndexReporte = "IndexReporte";
        public const string ConstanteTiprepcodi = "Tiprepcodi";
        public const string ConstanteFechaFilter1 = "FechaFilter1";
        public const string ConstanteFechaFilter2 = "FechaFilter2";
        public const string ConstanteTpriecodi = "Tpriecodi";

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public string Tiprepcodi
        {
            get
            {
                return (Session[ConstanteTiprepcodi] != null) ?
                    Session[ConstanteTiprepcodi].ToString() : "";
            }
            set { Session[ConstanteTiprepcodi] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public string IndexReporte
        {
            get
            {
                return (Session[ConstanteIndexReporte] != null) ?
                    Session[ConstanteIndexReporte].ToString() : "";
            }
            set { Session[ConstanteIndexReporte] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public DateTime FechaFilter1
        {
            get
            {
                return (DateTime?)Session[ConstanteFechaFilter1] ?? new DateTime();
            }
            set { Session[ConstanteFechaFilter1] = value; }
        }

        /// <summary>
        /// Almacena filtro de fecha para seteo de variable de busqueda Amexo A PR5
        /// </summary>
        public DateTime FechaFilter2
        {
            get
            {
                return (DateTime?)Session[ConstanteFechaFilter2] ?? new DateTime();
            }
            set { Session[ConstanteFechaFilter2] = value; }
        }

        /// <summary>
        /// Determinar si la sesion es válida, si se selecciono fecha para reporte de Anexo A
        /// </summary>
        /// <returns></returns>
        public bool EsOpcionValida()
        {
            return base.IsValidSesion
                   && this.Tiprepcodi != null && this.Tiprepcodi.Trim() != string.Empty
                   && Session[ConstanteFechaFilter1] != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RedireccionarOpcionValida()
        {
            if (!IsValidSesion)
            {
                return base.RedirectToLogin();
            }
            else
            {
                return RedirectToAction("Index", "DashboardAnalitico");
            }
        }

        /// <summary>
        /// Establecer valor a filtro de fecha
        /// </summary>
        /// <param name="fec"></param> 
        /// <returns></returns>
        public JsonResult SetearFechaFilter(string fec1, string fec2)
        {
            Siosein2Model model = new Siosein2Model();
            DateTime dtfecha1 = DateTime.MinValue, dtfecha2 = DateTime.MinValue;
            try
            {
                dtfecha1 = DateTime.ParseExact(fec1, Constantes.FormatoMesAnio, CultureInfo.InvariantCulture);
                dtfecha2 = dtfecha1.AddMonths(1).AddDays(-1);
                this.FechaFilter1 = dtfecha1;
                this.FechaFilter2 = dtfecha2;

                model.Total = 1;
            }
            catch
            {
                model.Total = -1;
            }

            return Json(model);
        }

        #endregion


        // GET: /Siosein2/DashboardAnalitico/
        public ActionResult Index()
        {
            Siosein2Model model = new Siosein2Model();
            var fecha = DateTime.Now.AddMonths(-1);
            string periodo = string.Format("{0:MM yyyy}", fecha);
            model.Fecha = periodo;
            model.Titulo = "DASHBOARD ANALÍTICO";
            model.Resultados = _servicioSiosein2.GetByCriteriaSpoNumhistoria().OrderBy(x => x.Numecodi).Select(x => x.Numhisdescripcion).ToList();
            var fechaActual = DateTime.Today;
            this.FechaFilter1 = new DateTime(fechaActual.Year, fechaActual.Month, 1);
            this.FechaFilter2 = FechaFilter1.AddMonths(1).AddDays(-1);
            return View(model);
        }

        [HttpPost]
        public JsonResult ConsultarConceptoNumeral(int numeral)
        {
            var listaConceptos = _servicioSiosein2.GetByCriteriaSpoConceptos(numeral).OrderBy(x=>x.Sconcodi).Select(x => new { x.Sconcodi, x.Sconnomb });
            return Json(listaConceptos);
        }

        [HttpPost]
        public JsonResult ConsultarDashboardAnalíticoMensual(int numeral, int mesesmoviles, int? concepto)
        {
            Siosein2Model model = new Siosein2Model
            {
                Grafico = _servicioSiosein2.GenerarGWebDashboardAnalítico(FechaFilter1, numeral, concepto),
                Resultado = _servicioSiosein2.GenerarRHtmlDashboardAnalíticoMensual(FechaFilter1, mesesmoviles, numeral, concepto)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult ConsultarDashboardAnalíticoDiario(int numeral, int diasmoviles, int clasicodi, int? concepto)
        {
            Siosein2Model model = new Siosein2Model();

            var listaData = _servicioSiosein2.ObtenerDataDashboardAnalíticoDiario(FechaFilter1, FechaFilter2, diasmoviles, numeral, clasicodi, concepto);
            model.Resultado = _servicioSiosein2.GenerarRHtmlDashboardAnalíticoDiario(listaData, ConstantesSiosein2.TacometroNumeralTickPositions[numeral]);
            model.Grafico = _servicioSiosein2.GenerarGwebLineDashboardAnalitico(listaData, ConstantesSiosein2.TacometroNumeralTickPositions[numeral]);

            return Json(model);
        }

        [HttpPost]
        public JsonResult GraficoDisponibleNoDespachada(int mesesmoviles)
        {
            Siosein2Model model = new Siosein2Model();
            model.Grafico = _servicioSiosein2.GenerarGWebDisponibleNoDespachada(FechaFilter1, mesesmoviles);
            return Json(model);
        }

        [HttpPost]
        public JsonResult ConsultarDashboardAnalítico(int numeral)
        {
            var listaConceptos = _servicioSiosein2.GetByCriteriaSpoConceptos(numeral).OrderBy(x => x.Sconcodi).ToList();
            var listConceptoNo = new List<int>() { 108 };

            listaConceptos = listaConceptos.Where(x => !listConceptoNo.Contains(x.Sconcodi)).ToList();
            Siosein2Model model = new Siosein2Model
            {
                Graficos = new List<Framework.Base.Core.GraficoWeb>(),
                Resultados = new List<string>(),
                Conceptos = listaConceptos.Select(x => x.Sconcodi).ToList()
            };

            foreach (var concepto in listaConceptos)
            {
                model.Graficos.Add(_servicioSiosein2.GenerarGWebDashboardAnalítico(FechaFilter1, numeral, concepto.Sconcodi));
            }

            return Json(model);
        }

    }
}
