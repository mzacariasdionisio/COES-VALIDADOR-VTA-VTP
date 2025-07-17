using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Mape.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Siosein2;
using log4net;
using ListaSelect = COES.MVC.Intranet.Areas.Eventos.Models.ListaSelect;

namespace COES.MVC.Intranet.Areas.Mape.Controllers
{
    public class MapeController : Controller
    {
        //
        // GET: /Siosein2/Mape/

        private static readonly ILog Log = LogManager.GetLogger(typeof(MapeController));
        private static readonly string NameController = "MapeController";
        private readonly Siosein2AppServicio _servicio;

        public MapeController()
        {
            _servicio = new Siosein2AppServicio();
        }

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

        #region Mape

        #region Generacion Mape
        [HttpGet]
        public ActionResult Index()
        {
            var model = new Siosein2Model { Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha) };
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerarMape(string fecha)
        {
            int vermcodi = 0;
            var model = new Siosein2Model();
            MeMedicion48DTO medProgramacion = new MeMedicion48DTO();
            MeMedicion48DTO medEjecutado = new MeMedicion48DTO();
            MeMedicion48DTO medEjecutadoCoes = new MeMedicion48DTO();
            try
            {
                DateTime fechaMape = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaGeneracion = fechaMape.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);
                //Verificar si hay datos para generar version Mape
                _servicio.GetDataMape(fechaGeneracion,ref medProgramacion,ref medEjecutado,ref medEjecutadoCoes);
                if(medProgramacion == null)
                {
                    model.NRegistros = -1;
                    return Json(model);
                }
                if(medEjecutado == null)
                {
                    model.NRegistros = -1;
                    return Json(model);
                }
                ///
                int numeroVersion = _servicio.GetMaxVermnumero(fechaGeneracion);
                var versionDto = new MapVersionDTO
                {
                    Vermusucreacion = User.Identity.Name,
                    Vermfechaperiodo = fechaGeneracion,
                    Vermfeccreacion = DateTime.Now,
                    Vermestado = ConstantesSiosein2.EstadoGenerado,
                    Vermnumero = numeroVersion
                };

                vermcodi = _servicio.SaveMapVersion(versionDto);
                
                _servicio.GenerarMapeTotalRealYDesvio(fechaMape, vermcodi, medProgramacion, medEjecutado, medEjecutadoCoes);
                _servicio.GenerarMapeUlsYCorregido(fechaMape, vermcodi, User);

                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.NRegistros = 0;
                _servicio.DeleteMapVersion(vermcodi);

                Log.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vermcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarMapMedicion48(int vermcodi, string fecha)
        {
            var mapVersion = _servicio.GetByIdMapVersion(vermcodi);
            try
            {
                var entity = new MapVersionDTO
                {
                    Vermcodi = vermcodi,
                    Vermnumero = mapVersion.Vermnumero,
                    Vermfechaperiodo = mapVersion.Vermfechaperiodo,
                    Vermestado = ConstantesSiosein2.EstadoBaja,
                    Vermfecmodificacion = DateTime.Now,
                    Vermusumodificacion = User.Identity.Name,
                    Vermusucreacion = mapVersion.Vermusucreacion,
                    Vermfeccreacion = mapVersion.Vermfeccreacion
                };

                _servicio.UpdateMapVersion(entity);
                return Json(1);
            }
            catch (Exception ex)
            {

                Log.Error("Eliminar", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarMapMedicion48(string fecha)
        {
            var model = new Siosein2Model();

            var fechaMape = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var listaMedicion48 = _servicio
                .ListaMapMedicion48PorFecha(fechaMape, fechaMape)
                .Where(x => x.Tipoccodi <= 6 && x.Vermcodi > 0).ToList();

            string url = Url.Content("~/");
            model.Resultado = _servicio.ListaMapMedicion48PorFechaHtml(fechaMape, listaMedicion48, url);
            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vermcodi"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarDetalleMapMedicion48(int vermcodi, string fecha)
        {
            var model = new Siosein2Model();

            var fechaMape = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            var listaMedicion48 = _servicio.ListaMapMedicion48PorFecha(fechaMape, fechaMape).Where(x => x.Vermcodi == vermcodi && x.Tipoccodi <= 6).ToList();
            model.Resultado = _servicio.ListaMapMedicion48PorHorasHtml(fechaMape, listaMedicion48);
            return Json(model);
        }

        [HttpPost]
        public JsonResult ExportarMapeVersion(int vermcodi)
        {
            var model = new Siosein2Model();

            var rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            var nombreArchivo = string.Format("{0}{1}", ConstantesSiosein2.NombreExcelMapeVersion, ConstantesAppServicio.ExtensionExcel);
            var plantillaMape = string.Format("{0}{1}", ConstantesSiosein2.NombrePalntillaExcelMape, ConstantesAppServicio.ExtensionExcel);
            var rutaArchivo = rutaBase + nombreArchivo;
            var rutaPlantilla = rutaBase + plantillaMape;
            _servicio.GenerarArchivoExcelMapeVersion(vermcodi, rutaArchivo, rutaPlantilla);
            model.Resultado = nombreArchivo;
            model.NRegistros = 1;

            return Json(model);
        }

        #endregion

        #region Dashboard Mape

        [HttpGet]
        public ActionResult IndexDashboard()
        {
            var model = new Siosein2Model { Fecha = DateTime.Now.ToString(Constantes.FormatoFecha) };
            return View(model);
        }

        [HttpPost]
        public JsonResult ConstruirDashboard(string fecha)
        {
            var model = new List<Siosein2Model>();
            var fechaC = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int numeroVersion = _servicio.GetMaxVermnumero(fechaC);
            if (numeroVersion > 1)
            {


                var primerDiaDelMes = new DateTime(fechaC.Year, fechaC.Month, 1);
                var ultimoDiaDelMes = primerDiaDelMes.AddMonths(1).AddDays(-1);

                var primerDiaDelAño = new DateTime(fechaC.Year, 1, 1);
                var ultimoDiaDelAño = primerDiaDelAño.AddYears(1).AddDays(-1);

                var fechaInicio = new DateTime(fechaC.Year - 6, 1, 1);
                var fechaFin = ultimoDiaDelMes;

                var OHLC = _servicio.GenerarSerieGraficoOHLC(fechaInicio, fechaFin);
                model.Add(new Siosein2Model() { Grafico = OHLC });

                var mapeMensual = _servicio.GenerarSerieGraficoEvolucionMapeMensual(fechaInicio, fechaFin);
                model.Add(new Siosein2Model() { Grafico = mapeMensual });

                var mapeDesviacionEstanMensual = _servicio.GenerarGwebLineaMensualDeDesvio(fechaInicio, fechaFin, ConstantesSiosein2.DesviacionEstandar);
                model.Add(new Siosein2Model() { Grafico = mapeDesviacionEstanMensual });

                var mapeMendiaMensial = _servicio.GenerarGwebLineaMensualDeDesvio(fechaInicio, fechaFin, ConstantesSiosein2.Media);
                model.Add(new Siosein2Model() { Grafico = mapeMendiaMensial });

                var mapeDistribDesviacionDiaria = _servicio.GenerarGwebDistribucionDesviacionDiaria(fechaC.AddDays(-3), fechaC);
                model.Add(new Siosein2Model() { Grafico = mapeDistribDesviacionDiaria });

                var mapeDiario = _servicio.GenerarGwebTacometroDiario(fechaC, fechaC);
                model.Add(new Siosein2Model() { Grafico = mapeDiario });

                var mapeTMensual = _servicio.GenerarGwebTacometroMensual(primerDiaDelMes, ultimoDiaDelMes);
                model.Add(new Siosein2Model() { Grafico = mapeTMensual });

                var mapeAnual = _servicio.GenerarGwebTacometroAnual(primerDiaDelAño, ultimoDiaDelAño);
                model.Add(new Siosein2Model() { Grafico = mapeAnual });

            }

            return Json(model);
        }

        #endregion

        #region Reportes Mape

        [HttpGet]
        public ActionResult IndexReporte()
        {
            var model = new Siosein2Model { Fecha2 = DateTime.Now, Fecha = DateTime.Now.ToString(Constantes.FormatoFecha) };
            return View(model);
        }

        /// <summary>
        ///  Genera reporte mensual.
        /// </summary>
        /// <param name="anhorinicio"></param>
        /// <param name="anhofin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarEvolucionMapeMensual(string anhorinicio, string anhofin)
        {
            var model = new Siosein2Model();

            var fechaInicio = new DateTime(Convert.ToInt32(anhorinicio), 1, 1);
            var fechaFin = new DateTime(Convert.ToInt32(anhofin), 1, 1).AddYears(1).AddDays(-1);

            var listaEvolucionMensual = _servicio.ConsultarEvolucionMapeMensual(fechaInicio, fechaFin);

            model.Resultado = _servicio.ReporteEvolucionMapeMensualHtml(listaEvolucionMensual, fechaInicio, fechaFin);

            return Json(model);
        }

        [HttpPost]
        public JsonResult ExportarEvolucionMapeMensual(string anhorinicio, string anhofin)
        {
            var model = new Siosein2Model();

            var fechaInicio = new DateTime(Convert.ToInt32(anhorinicio), 1, 1);
            var fechaFin = new DateTime(Convert.ToInt32(anhofin), 1, 1).AddYears(1).AddDays(-1);

            var listaEvolucionMensual = _servicio.ConsultarEvolucionMapeMensual(fechaInicio, fechaFin);

            var rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            var nombreArchivo = string.Format("{0}-{1}_{2}{3}", ConstantesSiosein2.NombreExcelEvolucionMapeMensual, anhorinicio, anhofin, ConstantesAppServicio.ExtensionExcel);
            var rutaArchivo = rutaBase + nombreArchivo;
            _servicio.GenerarArchivoExcelEvolucionMapeMensual(listaEvolucionMensual, fechaInicio, fechaFin, rutaArchivo);

            model.Resultado = nombreArchivo;
            model.NRegistros = 1;

            return Json(model);
        }

        /// <summary>
        /// Genera reporte semanal-diario
        /// </summary>
        /// <param name="anho"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ConsultarEvolucionMapeSemanalDiario(string anho, string semana)
        {
            var model = new List<Siosein2Model>();

            var anhoConsulta = Convert.ToInt32(anho);
            var semanaConsulta = Convert.ToInt32(semana);
            var fechaInicio = EPDate.f_fechainiciosemana(anhoConsulta, semanaConsulta);
            var fechaFin = EPDate.f_fechafinsemana(anhoConsulta, semanaConsulta);


            var mapeSemanalDiario = _servicio.GenerarGwebEvolucionMapeSemanalDiario(fechaInicio.AddDays(-7), fechaFin);
            var mapeSemanalDiarioHtml = _servicio.ReporteEvolucionMapeSemanalDiarioHtml(fechaInicio.AddDays(-7), fechaFin);

            var desviacionSemanalDiarioHtml = _servicio.ReporteEvolucionDesviacionSemanalDiarioHtml(fechaInicio.AddDays(-7), fechaFin);
            var mapedeDesviacionSemanalDiario = _servicio.GenerarGwebLineaDesviacionSemanalDiario(fechaInicio.AddDays(-7), fechaFin);

            model.Add(new Siosein2Model { Grafico = mapeSemanalDiario, Resultado = mapeSemanalDiarioHtml });
            model.Add(new Siosein2Model { Grafico = mapedeDesviacionSemanalDiario, Resultado = desviacionSemanalDiarioHtml });


            return Json(model);
        }


        [HttpPost]
        public JsonResult ExportarEvolucionMapeSemanalDiario(string anho, string semana)
        {


            var model = new Siosein2Model();

            var anhoConsulta = Convert.ToInt32(anho);
            var semanaConsulta = Convert.ToInt32(semana);
            var fechaInicio = EPDate.f_fechainiciosemana(anhoConsulta, semanaConsulta);
            var fechaFin = EPDate.f_fechafinsemana(anhoConsulta, semanaConsulta);

            var rutaBase = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            var nombreArchivo = string.Format("{0}-{1}_{2}{3}", ConstantesSiosein2.NombreExcelEvolucionMapeSemanalDiario, anho, semana, ConstantesAppServicio.ExtensionExcel);
            var rutaArchivo = rutaBase + nombreArchivo;

            _servicio.GenerarArchivoExcelMapeSemanalDiario(fechaInicio.AddDays(-7), fechaFin, rutaArchivo, anho, semana);
            model.Resultado = nombreArchivo;
            model.NRegistros = 1;

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="anho"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerSemanas(string anho)
        {
            var entitys = new List<ListaSelect>();
            if (anho != "")
            {
                var dfecha = new DateTime(Int32.Parse(anho), 12, 31);
                var nsemanas = COES.Base.Tools.Util.ObtenerNroSemanasxAnho(dfecha, FirstDayOfWeek.Saturday);

                for (int i = 1; i <= nsemanas; i++)
                {
                    var reg = new ListaSelect { id = i, text = "Sem" + i + "-" + anho };
                    entitys.Add(reg);
                }
            }
            return Json(entitys);
        }

        #endregion

        #endregion


        /// <summary>
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }
    }
}
