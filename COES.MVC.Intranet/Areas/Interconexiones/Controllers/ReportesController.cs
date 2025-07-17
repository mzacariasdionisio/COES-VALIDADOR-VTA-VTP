using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Interconexiones.Helper;
using COES.MVC.Intranet.Areas.Interconexiones.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Medidores;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Interconexiones.Controllers
{
    public class ReportesController : Controller
    {
        //
        // GET: /Interconexiones/Reportes/
        InterconexionesAppServicio logic = new InterconexionesAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ReportesController));
        private static string NameController = "ReportesController";

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

        public int TipoCapExc
        {
            get
            {
                return (Session["TipoCapExc"] != null) ?
                   int.Parse(Session["TipoCapExc"].ToString()) : 0;
            }
            set { Session["TipoCapExc"] = value; }
        }

        public string TituloCapExc
        {
            get
            {
                return (Session["TituloCapExc"] != null) ?
                   Session["TituloCapExc"].ToString() : "";
            }
            set { Session["TituloCapExc"] = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Index para reporte Historico
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexHistorico()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaInterconexion = this.logic.ListInInterconexions();
            model.IdPtomedicion = 0;
            if (model.ListaInterconexion.Count > 0)
                model.IdPtomedicion = (int)model.ListaInterconexion[0].Ptomedicodi;
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.IdMedidor = 0;
            model.ListaMedidor = this.logic.GetListaMedidorApp();
            return View(model);
        }
        /// <summary>
        /// Obtiene Listado para el reporte historico de interconexiones
        /// </summary>
        /// <param name="ptomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaHistorico(int ptomedicion, int medidor, string fechaInicial, string fechaFinal, int pagina)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            try
            {
                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                string resultado = this.logic.ObtenerConsultaHistoricaPagInterconexion(ptomedicion, medidor, fechaInicio, fechaFin, pagina);
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Para paginar Listado Historico
        /// </summary>
        /// <param name="ptomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int ptomedicion, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (fechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            int nroPaginas = logic.ObtenerTotalHistoricoInterconexion(ptomedicion, fechaInicio, fechaFin);
            int nroRegistros = nroPaginas * 96;
            if (nroRegistros > Constantes.NroPageShow)
            {
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }


        // exporta el reporte historico consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporteHistorico(int ptomedicion, int medidor, string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            try
            {
                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                var list = logic.ObtenerHistoricoInterconexion(ptomedicion, medidor, fechaInicio, fechaFin);
                logic.GenerarArchivoReporte(ptomedicion, list, medidor, fechaInicio, fechaFin, ruta + ConstantesInterconexiones.PlantillaReporteHistorico,
                    ruta + ConstantesInterconexiones.NombreReporteHistorico);
            }
            catch (Exception ex)
            {
                indicador = -1;
                Log.Error(NameController, ex);
            }
            return Json(indicador);
        }

        [HttpPost]
        public PartialViewResult ValidarPeriodo(int ptomedicion, int medidor, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            try
            {
                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                model.ListaErrores = this.logic.ValidarPeriodo(ptomedicion, fechaInicio, fechaFin);                
            }
            catch (Exception ex)
            {
                model.ListaErrores = new List<LogErrorInterconexion>();
            }
            return PartialView(model);
        }

        [HttpPost]
        public JsonResult PublicarPortalWeb(string fechaInicial, string fechaFinal)
        {
            DateTime fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            return Json(this.logic.PublicarPortalWeb(fechaInicio, fechaFin));
        }

        /// <summary>
        /// Genera Reporte respecto a la capacidad de importacion.
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexExec()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaHorizonte = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).ToList();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }


        /// <summary>
        /// Index de Capacidad Importacion
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCap()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaHorizonte = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).ToList();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Genera el reporte historico de intercambios internacionales
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteInterElect()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaInterconexion = this.logic.ListInInterconexions();
            model.IdPtomedicion = 0;
            if (model.ListaInterconexion.Count > 0)
                model.IdPtomedicion = (int)model.ListaInterconexion[0].Ptomedicodi;
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Devuelve el model para la presentacion del listado de reporte de intercambio
        /// de electricidad
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarIntercambioElectricidad(int idPtomedicion, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            var list = logic.ObtenerDataEvolucionEnergia(idPtomedicion, fechaIni, fechaFin);
            model.ListaReporteEvolucion = list;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el grafico de Reporte de Intercambio de Electricidad.
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFina
        [HttpPost]
        public JsonResult GraficoRepInterElect(int idPtomedicion, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model = GraficoInterElect(idPtomedicion, fechaIni, fechaFin);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Devuelve el model del Grafico para la presentacion de Reporte de Intercambio de Electricidad
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public InterconexionesModel GraficoInterElect(int idPtomedicion, DateTime fechaIni, DateTime fechaFin)
        {
            InterconexionesModel model = new InterconexionesModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion24DTO> lista24 = new List<MeMedicion24DTO>();
            lista24 = logic.ObtenerListaIntercambiosElectricidad(idPtomedicion, fechaIni, fechaFin);
            int totalIntervalos = lista24.Count;
            model.Grafico.SeriesData = new decimal?[4][];
            if (lista24.Count > 0)
            {
                var ListaFechas = lista24.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia";
                // titulo el reporte               
                model.Grafico.TitleText = @"EVOLUCIÓN DE LA MÁXIMA DEMANDA Y ENERGÍA IMPORTADA Y EXPORTADA DIARIA EN LOS
INTERCAMBIOS INTERNACIONALES";
                //model.Grafico.subtitleText = lista24[0].Equinomb + " (MW)" + " Del" + model.FechaInicio + " Al" + model.FechaFin;
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";

                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesName = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   

                foreach (var p in lista24)
                {
                    model.Grafico.XAxisCategories.Add(p.Medifecha.ToString(Constantes.FormatoFecha));
                }
                // Obtener lista de valores para las series del grafico

                model.Grafico.Series = new List<RegistroSerie>();
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                model.Grafico.Series.Add(new RegistroSerie());
                for (var i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            model.Grafico.Series[i].Name = "Energía Exportada (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#3498DB";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 1:
                            model.Grafico.Series[i].Name = "Máxima Potencia Exportada(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#DC143C";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;
                        case 2:
                            model.Grafico.Series[i].Name = "Energía Importada (MWh)";
                            model.Grafico.Series[i].Type = "column";
                            model.Grafico.Series[i].Color = "#3CB371";
                            model.Grafico.Series[i].YAxis = 0;
                            model.Grafico.Series[i].YAxisTitle = "MWh";
                            break;
                        case 3:
                            model.Grafico.Series[i].Name = "Máxima Potencia Importada(MW)";
                            model.Grafico.Series[i].Type = "spline";
                            model.Grafico.Series[i].Color = "#F0E68C";
                            model.Grafico.Series[i].YAxis = 1;
                            model.Grafico.Series[i].YAxisTitle = "MW";
                            factor = 1;
                            break;

                    }
                    model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                    for (var j = 1; j <= totalIntervalos; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)lista24[j - 1].GetType().GetProperty("H" + (i + 1).ToString()).GetValue(lista24[j - 1], null);
                        model.Grafico.SeriesData[i][j - 1] = valor * factor;
                    }
                }

                //modelGraficoDiario = model;

            }// end del if 
            return model;

        }

        /// <summary>
        /// Muestra el grafico de Reporte de Flujo de Potencia.
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoRepFlujoPot(int idPtomedicion, int tipoInterconexion, int parametro, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model = GraficoFlujoPotencia(idPtomedicion, tipoInterconexion, parametro, fechaIni, fechaFin);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        /// <summary>
        /// Genera el grafico para los reportes de Flujo de Potencia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public InterconexionesModel GraficoFlujoPotencia(int idPtomedicion, int tipoInterconexion, int parametro, DateTime fechaIni, DateTime fechaFin)
        {
            InInterconexionDTO interconexion = logic.ObtenerInterconexion(idPtomedicion);
            InterconexionesModel model = new InterconexionesModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;
            List<MeMedicion96DTO> lista = new List<MeMedicion96DTO>();
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            lista = logic.ObtenerInterconexionParametro(idPtomedicion, parametro, fechaIni, fechaFin);
            model.Grafico.SerieDataS = new DatosSerie[2][];
            model.Grafico.Series.Add(new RegistroSerie());
            model.Grafico.Series.Add(new RegistroSerie());
            switch (parametro)
            {
                case 1:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " Exportación MW";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[0].YAxisTitle = "MW";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Importación MW";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.Series[1].YAxisTitle = "MW";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    factor = 4;
                    model.Grafico.TitleText = @"Flujo en la línea " + interconexion.Interenlace + " de 220kV";
                    model.Grafico.YAxixTitle.Add("MW");
                    break;
                case 2:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " Exportación MVAR";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[0].YAxisTitle = "MVAR";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Importación MVAR";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.Series[1].YAxisTitle = "MVAR";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    factor = 4;
                    model.Grafico.TitleText = @"Flujo en la línea " + interconexion.Interenlace + " de 220kV";
                    model.Grafico.YAxixTitle.Add("MVAR");
                    break;
                case 3:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " Exportación MWh";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[0].YAxisTitle = "MWh";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Importación MWh";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.Series[1].YAxisTitle = "MWh";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.TitleText = @"Evolución del Flujo Energía Activa del Enlace Internacional 
en la línea " + interconexion.Interenlace + " de 220kV";
                    model.Grafico.YAxixTitle.Add("MWh");
                    break;
                case 4:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " Exportación MVARh";
                    model.Grafico.Series[0].Type = "area";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Importación MVARh";
                    model.Grafico.Series[1].Type = "area";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.TitleText = @"Evolución del Flujo Energía Activa del Enlace Internacional 
en la línea " + interconexion.Interenlace + " de 220kV";
                    model.Grafico.YAxixTitle.Add("MVARh");
                    break;
                case 5:
                    model.Grafico.Series[0].Name = interconexion.Internomlinea + " kV";
                    model.Grafico.Series[0].Type = "line";
                    model.Grafico.Series[0].Color = "#4F81BD";
                    model.Grafico.Series[0].YAxisTitle = "kV";
                    model.Grafico.Series[1].Name = interconexion.Internomlinea + " Amp";
                    model.Grafico.Series[1].Type = "line";
                    model.Grafico.Series[1].Color = "#C0504D";
                    model.Grafico.SerieDataS[0] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.SerieDataS[1] = new DatosSerie[lista.Count * 96 / 2];
                    model.Grafico.Series[1].YAxisTitle = "Amp";
                    model.Grafico.YAxixTitle.Add("kV");
                    model.Grafico.YAxixTitle.Add("Amp");
                    model.Grafico.TitleText = @"Evolución de la tensión y amperios de enlace internacional de linea " + interconexion.Interenlace + " durante los intercambios internacionales";
                    break;
            }

            if (lista.Count > 0)
            {
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte               

                //model.Grafico.subtitleText = lista24[0].Equinomb + " (MW)" + " Del" + model.FechaInicio + " Al" + model.FechaFin;
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);
                int indiceA = 0;
                int indiceB = 0;
                for (var i = 0; i < lista.Count(); i++)
                {
                    DateTime fecha = (DateTime)lista[i].Medifecha;
                    var registro = lista[i];
                    for (var j = 1; j <= 96; j++)
                    {
                        decimal? valor = 0;
                        valor = (decimal?)registro.GetType().GetProperty("H" + (j).ToString()).GetValue(registro, null);
                        if (valor == null)
                            valor = 0;
                        if (i == 0)
                            model.Grafico.XAxisCategories.Add(registro.Medifecha.Value.AddMinutes(j * 15).ToString(Constantes.FormatoFechaHora));
                        var serie = new DatosSerie();
                        serie.X = fecha.AddMinutes(j * 15);
                        serie.Y = valor * factor;
                        switch (registro.Tipoinfocodi)
                        {
                            case 3:
                                if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    try
                                    {
                                        model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                    }
                                    catch (Exception ex)
                                    {
                                        var msg = ex.Message;
                                    }
                                }
                                break;
                            case 4:
                                if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MVARr)
                                {
                                    model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                }
                                else
                                {
                                    model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                }
                                break;
                            case 5:
                                model.Grafico.SerieDataS[0][indiceB * 96 + (j - 1)] = serie;
                                break;
                            case 9:

                                model.Grafico.SerieDataS[1][indiceA * 96 + (j - 1)] = serie;
                                break;
                        }
                    }
                    switch (registro.Tipoinfocodi)
                    {
                        case 3:
                            if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MWh) indiceB++;
                            else indiceA++;
                            break;
                        case 4:
                            if (registro.Ptomedicodi == ConstantesInterconexiones.IdExportacionL2280MVARr) indiceB++;
                            else indiceA++;
                            break;
                        case 5:
                            indiceB++;
                            break;
                        case 9:
                            indiceA++;
                            break;

                    }
                }
                //modelGraficoDiario = model;
            }// end del if 
            return model;
        }

        /// <summary>
        /// Genera el reporte para los flujos de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult ReporteFlujoPotencia()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaInterconexion = this.logic.ListInInterconexions();
            model.IdPtomedicion = 0;
            if (model.ListaInterconexion.Count > 0)
                model.IdPtomedicion = (int)model.ListaInterconexion[0].Ptomedicodi;
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        ///Muestra el listado del reporte de flujo de potencia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaFlujoPotencia(int idPtomedicion, int tipoInterconexion, int parametro, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.IdParametro = parametro;
            model.Resultado = logic.GetHtmlInterconexionXParametro(idPtomedicion, parametro, fechaIni, fechaFin);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el listado de Capacidad de Importacion o Excedentes de Exportacion
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaExcedentes(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int origLectcodi = ConstantesInterconexiones.IdOrigenLectura2;
            string ptoMedicodi = ConstantesInterconexiones.IdPtoMediTumbes.ToString();
            var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
            int idLectura = 0;
            switch (idVersion)
            {
                case 0:
                    if (idHorizonte == 0)
                        idLectura = lista[0].Lectcodi;
                    else
                        idLectura = lista[1].Lectcodi;
                    break;
                case 1:
                    if (idHorizonte == 0)
                        idLectura = lista[2].Lectcodi;
                    else
                        idLectura = lista[3].Lectcodi;
                    break;
            }
            string resultado = this.logic.ObtenerReporteExcedente(idVersion, idLectura, origLectcodi, ptoMedicodi, fechaIni, fechaFin);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el listado de Capacidad de Importacion o Excedentes de Exportacion
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaCapImpor(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            int origLectcodi = ConstantesInterconexiones.IdOrigenLectura2;
            string ptoMedicodi = ConstantesInterconexiones.IdsPtoMediInterconexion;
            var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
            int idLectura = 0;
            switch (idVersion)
            {
                case 0:
                    if (idHorizonte == 0)
                        idLectura = lista[0].Lectcodi;
                    else
                        idLectura = lista[1].Lectcodi;
                    break;
                case 1:
                    if (idHorizonte == 0)
                        idLectura = lista[2].Lectcodi;
                    else
                        idLectura = lista[3].Lectcodi;
                    break;
            }
            string resultado = this.logic.GeneraVistaCapacImport(idVersion, idLectura, origLectcodi, ptoMedicodi, fechaIni, fechaFin);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Genera el grafico de excedentes de potencia 
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraGraficoExcedente(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //
            int idLectura = 0;
            var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
            switch (idVersion)
            {
                case 0:
                    if (idHorizonte == 0)
                        idLectura = lista[0].Lectcodi;
                    else
                        idLectura = lista[1].Lectcodi;
                    break;
                case 1:
                    if (idHorizonte == 0)
                        idLectura = lista[2].Lectcodi;
                    else
                        idLectura = lista[3].Lectcodi;
                    break;
            }

            model = GetModelGraficoExcedente(idLectura, fechaIni, fechaFin);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera grafico capacidad Importacion
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GeneraGraficoCapacImport(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            InterconexionesModel model = new InterconexionesModel();
            DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            //
            int idLectura = 0;
            var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
            switch (idVersion)
            {
                case 0:
                    if (idHorizonte == 0)
                        idLectura = lista[0].Lectcodi;
                    else
                        idLectura = lista[1].Lectcodi;
                    break;
                case 1:
                    if (idHorizonte == 0)
                        idLectura = lista[2].Lectcodi;
                    else
                        idLectura = lista[3].Lectcodi;
                    break;
            }

            model = GetModelGraficoCapacImport(idLectura, fechaIni, fechaFin);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera los datos para el grafico excedentes de potencia o capacidad de importacion
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public InterconexionesModel GetModelGraficoExcedente(int idLectura, DateTime fechaIni, DateTime fechaFin)
        {
            InterconexionesModel model = new InterconexionesModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            model.Grafico = grafico;

            List<MeMedicion48DTO> listaM48 = new List<MeMedicion48DTO>();
            int origLectcodi = ConstantesInterconexiones.IdOrigenLectura2;
            string ptoMedicodi = ConstantesInterconexiones.IdPtoMediTumbes.ToString();
            listaM48 = this.logic.ListaMed48Interconexiones(ConstantesInterconexiones.Excedente, idLectura, origLectcodi, ptoMedicodi, fechaIni, fechaFin);
            model.Grafico.SerieDataS = new DatosSerie[1][];
            model.Grafico.SerieDataS[0] = new DatosSerie[listaM48.Count * 48];
            model.Grafico.XAxisCategories = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.SeriesName.Add("L-2280 (ZORRITOS) MW");
            model.Grafico.YaxixTitle = "(MW)";
            model.SheetName = "GRAFICO";

            if (listaM48.Count > 0)
            {
                var ListaFechas = listaM48.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";
                //model.Grafico.TitleText = this.TituloCapExc.ToUpper() + " DIARIA EN ENLACE INTERNACIONAL DEL " + model.FechaInicio + " AL " + model.FechaFin;
                int totalIntervalos = 48;
                foreach (var p in listaM48)
                {
                    DateTime fecha = p.Medifecha;
                    for (var j = 0; j <= (totalIntervalos - 1); j++)
                    {
                        string fechahora = (fecha.AddMinutes(j * 30)).ToString(Constantes.FormatoHora);
                        model.Grafico.XAxisCategories.Add(fechahora);
                    }
                }
                // Obtener lista de valores para las series del grafico
                for (var i = 0; i < listaM48.Count(); i++)
                {
                    for (var j = 1; j <= totalIntervalos; j++)
                    {
                        DateTime fecha = listaM48[i].Medifecha;
                        decimal? valor = 0;
                        var entity = listaM48.Find(x => x.Medifecha == fecha);
                        if (entity != null)
                        {
                            valor = (decimal?)entity.GetType().GetProperty("H" + j).GetValue(entity, null);
                            if (valor == null)
                                valor = 0;
                            var serie = new DatosSerie();
                            serie.X = fecha.AddMinutes(j * 30);
                            serie.Y = valor;
                            model.Grafico.SerieDataS[0][i * 48 + (j - 1)] = serie;
                        }
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// Genera el Model para el grafico Capacidad de Importacion
        /// </summary>
        /// <param name="idLectura"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public InterconexionesModel GetModelGraficoCapacImport(int idLectura, DateTime fechaIni, DateTime fechaFin)
        {
            InterconexionesModel model = new InterconexionesModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            model.Grafico = grafico;

            List<MeMedicion48DTO> listaM48 = new List<MeMedicion48DTO>();
            int origLectcodi = ConstantesInterconexiones.IdOrigenLectura2;
            string ptoMedicodi = ConstantesInterconexiones.IdsPtoMediInterconexion;
            listaM48 = this.logic.ListaMed48Interconexiones(ConstantesInterconexiones.Capacidad, idLectura, origLectcodi, ptoMedicodi, fechaIni, fechaFin);
            var listaPtos = ConstantesInterconexiones.IdsPtoMediInterconexion.Split(',');
            int nSeries = listaPtos.Count();
            model.Grafico.SerieDataS = new DatosSerie[nSeries][];
            model.Grafico.Series = new List<RegistroSerie>();
            for (int i = 0; i < nSeries; i++)
            {
                model.Grafico.SerieDataS[i] = new DatosSerie[listaM48.Where(x => x.Ptomedicodi == int.Parse(listaPtos[i])).Count() * 48];
            }
            model.Grafico.XAxisCategories = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            //// Agregar nombres de Series para mas ptos
            var nserie = new RegistroSerie();
            nserie.Color = "SteelBlue";
            nserie.Name = "L-2280 (ZORRITOS - MACHALA) MW";
            model.Grafico.Series.Add(nserie);
            nserie = new RegistroSerie();
            nserie.Color = "Red";
            nserie.Name = "L-2249 (TALARA - ZORRITOS) MW";
            model.Grafico.Series.Add(nserie);

            model.Grafico.SeriesName.Add("L-2280 (ZORRITOS - MACHALA) MW");
            model.Grafico.SeriesName.Add("L-2249 (TALARA - ZORRITOS) MW");

            model.Grafico.YaxixTitle = "(MW)";
            model.SheetName = "GRAFICO";

            if (listaM48.Count > 0)
            {
                var ListaFechas = listaM48.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaIni = ListaFechas.Min();
                    fechaFin = ListaFechas.Max();
                }
                model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";
                int totalIntervalos = 48;

                for (int k = 0; k < nSeries; k++)
                {
                    var lista = listaM48.Where(x => x.Ptomedicodi == int.Parse(listaPtos[k])).OrderBy(x => x.Medifecha).ToList();
                    for (var i = 0; i < lista.Count(); i++)
                    {
                        for (var j = 1; j <= totalIntervalos; j++)
                        {
                            DateTime fecha = lista[i].Medifecha;
                            decimal? valor = 0;
                            var entity = lista.Find(x => x.Medifecha == fecha);
                            if (entity != null)
                            {
                                valor = (decimal?)entity.GetType().GetProperty("H" + j).GetValue(entity, null);
                                if (valor == null)
                                    valor = 0;
                                var serie = new DatosSerie();
                                serie.X = fecha.AddMinutes(j * 30);
                                serie.Y = valor;
                                model.Grafico.SerieDataS[k][i * 48 + (j - 1)] = serie;
                            }
                        }
                    }
                }
            }
            return model;

        }

        /// <summary>
        /// Genera el archivo de exportacion a excel para el grafico excedentes de potencia
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoExcedente(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            try
            {
                InterconexionesModel model = new InterconexionesModel();
                DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int idLectura = 0;
                var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
                switch (idVersion)
                {
                    case 0:
                        if (idHorizonte == 0)
                            idLectura = lista[0].Lectcodi;
                        else
                            idLectura = lista[1].Lectcodi;
                        break;
                    case 1:
                        if (idHorizonte == 0)
                            idLectura = lista[2].Lectcodi;
                        else
                            idLectura = lista[3].Lectcodi;
                        break;
                }
                model = GetModelGraficoExcedente(idLectura, fechaIni, fechaFin);
                if (model.Grafico.SerieDataS[0].Length > 0) // No hay data no se genera archivo excel
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                    logic.GenerarArchivoExcedente(model.Grafico, fechaInicial, fechaFinal,
                        ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones, ruta + ConstantesInterconexiones.PlantillaExcedente);
                    indicador = 1;
                }
                else
                {
                    indicador = 0;
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera el archivo de exportacion a excel para el grafico excedentes de potencia o capacidad de importacion
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="idHorizonte"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoCapacInter(int idVersion, int idHorizonte, string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            try
            {
                InterconexionesModel model = new InterconexionesModel();
                DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int idLectura = 0;
                var lista = this.logic.ListMeLecturas().Where(x => x.Origlectcodi == ConstantesInterconexiones.IdOrigenLectura2).OrderBy(x => x.Lectcodi).ToList();
                switch (idVersion)
                {
                    case 0:
                        if (idHorizonte == 0)
                            idLectura = lista[0].Lectcodi;
                        else
                            idLectura = lista[1].Lectcodi;
                        break;
                    case 1:
                        if (idHorizonte == 0)
                            idLectura = lista[2].Lectcodi;
                        else
                            idLectura = lista[3].Lectcodi;
                        break;
                }
                model = GetModelGraficoCapacImport(idLectura, fechaIni, fechaFin);
                if (model.Grafico.SerieDataS[0].Length > 0) // No hay data no se genera archivo excel
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                    logic.GenerarArchivoCapacInter(model.Grafico, fechaInicial, fechaFinal,
                        ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones, ruta + ConstantesInterconexiones.PlantillaCapacImp);
                    indicador = 1;
                }
                else
                {
                    indicador = 0;
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera el archivo del reporte de Interconexiones electricas
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGrafInterElect(int idPtomedicion, string fechaInicial, string fechaFinal)
        {
            int indicador = -1;
            try
            {
                InterconexionesModel model = new InterconexionesModel();
                DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model = GraficoInterElect(idPtomedicion, fechaIni, fechaFin);
                List<EstructuraEvolucionEnergia> list = logic.ObtenerDataEvolucionEnergia(idPtomedicion, fechaIni, fechaFin);
                if (model.Grafico.SeriesData != null)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                    logic.GenerarArchivoGrafInterElect(model.Grafico, fechaInicial, fechaFinal,
                        ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones2, ruta + Constantes.NombreLogoCoes, list);
                    indicador = 1;
                }
                else
                {
                    indicador = 0;
                }
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera el archivo de exportacion a excel para el grafico de flujo de potencia
        /// </summary>
        /// <param name="idPtomedicion"></param>
        /// <param name="tipoInterconexion"></param>
        /// <param name="parametro"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGrafFlujoPotencia(int idPtomedicion, int tipoInterconexion, int parametro, string fechaInicial, string fechaFinal)
        {
            int indicador = -1;
            try
            {
                InterconexionesModel model = new InterconexionesModel();
                DateTime fechaIni = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                model = GraficoFlujoPotencia(idPtomedicion, tipoInterconexion, parametro, fechaIni, fechaFin);
                model.IdParametro = parametro;
                if (model.Grafico.SerieDataS[0].Length > 0) // No hay data no se genera archivo excel
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
                    logic.GenerarArchivoGrafFlujoPotencia(model.Grafico, fechaInicial, fechaFinal, parametro,
             ruta + ConstantesInterconexiones.NombreRptGraficoInterconexiones3, ruta + Constantes.NombreLogoCoes);
                    indicador = 1;
                }
                else
                {
                    indicador = 0;
                }
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar el reporte general
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            var sTipoReporte = Request["tipo"];
            short tipoReporte = short.Parse(sTipoReporte);

            switch (tipoReporte)
            {
                case 1:
                    nombreArchivo = ConstantesInterconexiones.NombreRptGraficoInterconexiones;//Reporte gráfico Capacidad de Importacion en enlace internacional
                    break;
                case 2:
                    // Reporte INTERCAMBIO ELECTRICIDAD
                    nombreArchivo = ConstantesInterconexiones.NombreRptGraficoInterconexiones2;
                    break;
                case 3:
                    // FLUJO DE POTENCIA
                    nombreArchivo = ConstantesInterconexiones.NombreRptGraficoInterconexiones3;
                    break;
                case 4:
                    nombreArchivo = NombreArchivo.NombreReporteHistorico;
                    break;

            }
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.DirectorioInterconexiones;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
    }
}
