using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Globalization;
using System.Text;
using COES.MVC.Intranet.Helper;
using COES.Framework.Base.Core;
using log4net;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Controllers
{
    public class ReportesController : BaseController
    {
        //
        // GET: /StockCombustibles/Reportes/        
        StockCombustiblesAppServicio servicio = new StockCombustiblesAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ReportesController));
        /// <summary>
        /// Almacena los fechas del reporte
        /// </summary>
        public List<DateTime> ListaFechas
        {
            get
            {
                return (Session[ConstantesIntranet.ListaFechas] != null) ?
                    (List<DateTime>)Session[ConstantesIntranet.ListaFechas] : new List<DateTime>();
            }
            set { Session[ConstantesIntranet.ListaFechas] = value; }
        }

        public int NroPaginasReporte
        {
            get
            {
                return (Session[ConstantesIntranet.NroPaginasReporte] != null) ?
                     (int)Session[ConstantesIntranet.NroPaginasReporte] : new int();
            }
            set { Session[ConstantesIntranet.NroPaginasReporte] = value; }
        }

        /// <summary>
        /// Almacena los datos para el reporte grafico stock o Consumo de combustible
        /// </summary>
        public List<MeMedicionxintervaloDTO> ListaMedicionxIntervalo
        {
            get
            {
                return (Session[ConstantesIntranet.ListaMedicionxIntervalo] != null) ?
                    (List<MeMedicionxintervaloDTO>)Session[ConstantesIntranet.ListaMedicionxIntervalo] : new List<MeMedicionxintervaloDTO>();
            }
            set { Session[ConstantesIntranet.ListaMedicionxIntervalo] = value; }
        }

        /// <summary>
        /// Almacena los datos del reporte grafico Presión de Gas ó Temnperatura ambiente de combustible
        /// </summary>
        public List<MeMedicion24DTO> ListaMedicion24
        {
            get
            {
                return (Session[ConstantesIntranet.ListaMedicion24] != null) ?
                    (List<MeMedicion24DTO>)Session[ConstantesIntranet.ListaMedicion24] : new List<MeMedicion24DTO>();
            }
            set { Session[ConstantesIntranet.ListaMedicion24] = value; }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ReportesController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ReportesController", ex);
                throw;
            }
        }

        public ReportesController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region METODOS DE REPORTE DE STOCK DE COMBUSTIBLES

        /// <summary>
        /// Index de Reporte Stock de combustibles
        /// </summary>
        /// <returns></returns>
        public ActionResult Stock()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
            model.ListaEstFisCombustible = Util.ObtenerListaEstFisCombustible(ConstantesIntranet.TipoReporteStock);
            model.ListaCombustible = servicio.ListaCombustible();
            return View(model);

        }

        /// <summary>
        /// Devuelve lista parcial para el Listado de Stock de Combustible
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaStockCombustible(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo, int nroPagina)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();

            listaReporte = this.servicio.GetListaReporteStockPag(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente,
                                                       idsEstado, idsFechaIni, idsFechaFin, idsEquipo, nroPagina, Constantes.PageSize, Constantes.ParametroDefecto);
            string resultado = this.servicio.GeneraViewReporteStockCombustible(listaReporte, idsEstado);
            model.Resultado = resultado;
            return PartialView(model);
        }


        // Exporta el reporte general de stock de combustibles a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporteXLS(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, int iTipoReporte, string idsEquipo)
        {
            int indicador = 1;
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string ruta = string.Empty;
            try
            {
                switch (iTipoReporte)
                {
                    case ConstantesStockCombustibles.TipoReporteStock: // Reporte Stock de Combustible
                        ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                        listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente, idsEstado, idsFechaIni, idsFechaFin, idsEquipo, "-1");
                        if (listaReporte.Count > 0)
                        { // Si existen registros
                            this.servicio.GenerarArchivoExcelStock(listaReporte, idsFechaIni, idsFechaFin,
                                ruta + StockConsumoArchivo.RptExcelStock, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                            indicador = 1;
                        }
                        else
                            indicador = 2; // No existen registros
                        break;
                    case ConstantesStockCombustibles.TipoReporteAcumulado: // Reporte Recepcion acumulada combustibles
                        ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                        listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente, idsEstado, idsFechaIni, idsFechaFin, idsEquipo, "-1");
                        List<MeMedicionxintervaloDTO> lista = listaReporte.GroupBy(t => new { t.Ptomedicodi, t.Emprnomb, t.Equinomb, t.Fenergnomb, t.Tipoinfoabrev })
                        .Select(g => new MeMedicionxintervaloDTO()
                        {
                            Ptomedicodi = g.Key.Ptomedicodi,
                            Emprnomb = g.Key.Emprnomb,
                            Equinomb = g.Key.Equinomb,
                            Fenergnomb = g.Key.Fenergnomb,
                            Tipoinfoabrev = g.Key.Tipoinfoabrev,
                            H1Recep = g.Sum(t => t.H1Recep)
                        }).ToList();
                        if (listaReporte.Count > 0)
                        { // Si existen registros
                            this.servicio.GenerarArchivoExcelAcumulado(lista, idsFechaIni, idsFechaFin,
                                ruta + StockConsumoArchivo.RptExcelAcumulado, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                            indicador = 1;
                        }
                        else
                            indicador = 2; // No existen registros
                        break;

                    case ConstantesStockCombustibles.TipoReporteAcumuladoDet:
                        ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                        listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente, idsEstado, idsFechaIni, idsFechaFin, idsEquipo, "-1");

                        if (listaReporte.Count > 0)
                        { // Si existen registros
                            this.servicio.GenerarArchivoExcelAcumuladoDet(listaReporte, idsFechaIni, idsFechaFin,
                                ruta + StockConsumoArchivo.RptExcelAcumuladoDet, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                            indicador = 1;
                        }
                        else
                            indicador = 2; // No existen registros
                        break;

                }

            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Muestra el gráfico del stock de combustible de las centrales termoeléctricas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoReporteStock(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            //idsRecurso = string.Concat(idsRecurso, ",", ConstantesStockCombustibles.StrTptoRecepcion);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente,
                                                       idsEstado, fechaInicial.ToString(Constantes.FormatoFecha), fechaFinal.ToString(Constantes.FormatoFecha), idsEquipo, "-1");
            this.ListaMedicionxIntervalo = listaReporte;
            model = GraficoStock(listaReporte, idsEstado);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera los datos para el grafico Stock de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="idsEstado"></param>
        /// <returns></returns>
        public StockCombustiblesModel GraficoStock(List<MeMedicionxintervaloDTO> listaReporte, string idsEstado)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            //GraficoWeb grafico = new GraficoWeb();
            model.Grafico = new GraficoWeb();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;
            List<MeMedicionxintervaloDTO> listPtosMed = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Fenergnomb, x.Equinomb })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Equinomb = y.Key.Equinomb,
                                }
                                ).ToList();
            var listFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaIni = ListaFechas.Min();
                fechaFin = ListaFechas.Max();
            }
            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listPtosMed.Count][];
            for (int i = 0; i < listPtosMed.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listPtosMed[i].Equinomb + " - " + listPtosMed[i].Fenergnomb;

                regSerie.Type = "area";
                // regSerie.Color = "#3498DB";
                //regSerie.YAxisTitle = "MW";
                model.Grafico.Series.Add(regSerie);
                model.Grafico.SerieDataS[i] = new DatosSerie[listFechas.Count];

            }
            model.Grafico.TitleText = StockCombustiblesAppServicio.GeneraTituloListado(" DE COMBUSTIBLES ", idsEstado) + " EN LAS CENTRALES TÉRMOELECTRICAS DEL SEIN";

            if (listaReporte.Count > 0)
            {
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);
                for (var i = 0; i < listPtosMed.Count; i++)
                {
                    for (var j = 0; j < listFechas.Count; j++)
                    {
                        var entity = listaReporte.Find(x => x.Ptomedicodi == listPtosMed[i].Ptomedicodi && x.Medintfechaini == listFechas[j]);

                        decimal? valor = 0;
                        decimal? valorz = 0;
                        if (entity != null)
                        {
                            valor = (decimal?)entity.H1Fin;
                            valorz = (decimal?)entity.H1Recep;
                        }
                        if (valor == null)
                            valor = 0;
                        if (valorz == null)
                            valorz = 0;
                        var serie = new DatosSerie();
                        serie.X = listFechas[j];
                        serie.Y = valor;
                        serie.Z = valorz;
                        model.Grafico.SerieDataS[i][j] = serie;
                    }

                }
            }
            return model;
        }


        /// <summary>
        /// Genera Archivo para reporte general de stock de combustible
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGraficoStock(string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            try
            {
                List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
                lista = this.ListaMedicionxIntervalo;
                if (lista.Count > 0)
                { // Si existen registros
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                    this.servicio.GenerarArchivoGrafStock(lista, fechaInicial, fechaFinal,
                        ruta + StockConsumoArchivo.RptExcelGraficoStock, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                    indicador = 1;
                }
                else
                    indicador = 2; // No existen regsitros
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }


        /// <summary>
        /// Permite pintar el paginado del reporte de stock
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult PaginadoStock(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo)
        {
            Paginacion model = new Paginacion();
            List<MeMedicionxintervaloDTO> listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente,
                                                       idsEstado, idsFechaIni, idsFechaFin, idsEquipo, ConstantesStockCombustibles.StrTptoStock);
            int nroRegistros = listaReporte.Count();
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                this.NroPaginasReporte = nroPaginas;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }
        #endregion

        #region METODOS DE REPORTE CONSUMO DE COMBUSTIBLES

        /// <summary>
        /// Permite listar el historico de Consumo de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult Consumo()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
            model.ListaEstFisCombustible = Util.ObtenerListaEstFisCombustible(ConstantesIntranet.TipoReporteConsumo);
            return View(model);
        }

        /// <summary>
        /// Devuelve lista parcial para el Listado de Consumo de Combustible
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaConsumoCombustible(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            listaReporte = this.servicio.ListaMedxIntervConsumo(ConstantesStockCombustibles.LectCodiConsumo, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt);
            this.ListaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }
            string resultado = this.servicio.GeneraViewReporteConsumoCombustible(listaReporte, ListaFechas, idsEstado);
            model.Resultado = resultado;
            return PartialView(model);
        }


        /// <summary>
        /// Exporta el reporte general de Consumo de combustibles a archivo excel
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteXLSConsumo(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            int indicador = 1;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                listaReporte = this.servicio.ListaMedxIntervConsumo(ConstantesStockCombustibles.LectCodiConsumo, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt);
                if (listaReporte.Count > 0) // si hay elementos
                {
                    this.servicio.GenerarArchivoExcelConsumo(listaReporte, idsFechaIni, idsFechaFin,
                       ruta + StockConsumoArchivo.RptExcelConsumo, ruta + ConstantesStockCombustibles.NombreLogoCoes, idsEstado);
                    indicador = 1;
                }
                else
                    indicador = 2;
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteXLSConsumo", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Muestra el gráfico del consumo de combustible de las centrales termoeléctricas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoReporteConsumo(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            List<MeTipopuntomedicionDTO> ListaTptoMedicion = new List<MeTipopuntomedicionDTO>();
            List<MeTipopuntomedicionDTO> ListaTptoMedicionaux = new List<MeTipopuntomedicionDTO>();
            ListaTptoMedicion = this.servicio.ListaMeTipoPuntoMedicion(idsRecurso, idsEstado);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            listaReporte = this.servicio.ListaMedxIntervConsumo(ConstantesStockCombustibles.LectCodiConsumo, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt);
            this.ListaMedicionxIntervalo = listaReporte;
            foreach (var reg in ListaTptoMedicion)
            {
                MeTipopuntomedicionDTO entity = new MeTipopuntomedicionDTO();
                entity.Tipoptomedicodi = reg.Tipoptomedicodi;
                entity.Tipoptomedinomb = reg.Tipoptomedinomb.Substring(24, reg.Tipoptomedinomb.Length - 24);
                ListaTptoMedicionaux.Add(entity);
            }
            model = GraficoConsumo(listaReporte, idsEstado);
            model.ListaTipoPtoMedicion = ListaTptoMedicionaux;
            string resultado = this.servicio.GeneraTabReporteGrafico(idsRecurso, idsEstado);
            model.Resultado = resultado;
            model.ragFechas = (fechaFinal - fechaInicial).Days;
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera los datos para el grafico Consumo de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="idsEstado"></param>
        /// <returns></returns>
        public StockCombustiblesModel GraficoConsumo(List<MeMedicionxintervaloDTO> listaReporte, string idsEstado)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIni = DateTime.MinValue;
            DateTime fechaFin = DateTime.MinValue;

            List<MeMedicionxintervaloDTO> listaCabeceraM = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tptomedicodi, x.Fenergnomb, x.Equinomb, x.Equipadre, x.Equipopadre, x.Emprcoes, x.Ptomedielenomb })
                                .Select(y => new MeMedicionxintervaloDTO()
                                {
                                    Emprnomb = y.Key.Emprnomb,
                                    Ptomedicodi = y.Key.Ptomedicodi,
                                    Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                    Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                    Tptomedicodi = y.Key.Tptomedicodi,
                                    Fenergnomb = y.Key.Fenergnomb,
                                    Equinomb = y.Key.Equinomb,
                                    Equipadre = y.Key.Equipadre,
                                    Equipopadre = y.Key.Equipopadre,
                                    Emprcoes = y.Key.Emprcoes,
                                    Ptomedielenomb = y.Key.Ptomedielenomb,
                                }
                                ).ToList();


            var listFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaIni = ListaFechas.Min();
                fechaFin = ListaFechas.Max();
            }
            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
            model.Grafico = grafico;
            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaCabeceraM.Count][];
            for (int i = 0; i < listaCabeceraM.Count; i++)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = listaCabeceraM[i].Equinomb + " - " + listaCabeceraM[i].Fenergnomb;
                regSerie.Type = "area";
                regSerie.TipoPto = listaCabeceraM[i].Tptomedicodi;
                model.Grafico.Series.Add(regSerie);
                model.Grafico.SerieDataS[i] = new DatosSerie[listFechas.Count];

            }
            model.Grafico.TitleText = StockCombustiblesAppServicio.GeneraTituloListado("CONSUMO DE COMBUSTIBLES ", idsEstado) + " EN LAS CENTRALES TÉRMOELECTRICAS DEL SEIN Del " + model.FechaInicio + " Al " + model.FechaFin;

            if (listaReporte.Count > 0)
            {
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();
                model.Grafico.SeriesYAxis.Add(0);
                decimal? valorAcumulado;
                for (var i = 0; i < listaCabeceraM.Count; i++)
                {
                    valorAcumulado = 0;
                    for (var j = 0; j < listFechas.Count; j++)
                    {
                        var entity = listaReporte.Find(x => x.Ptomedicodi == listaCabeceraM[i].Ptomedicodi && x.Medintfechaini == listFechas[j]);


                        decimal? valor = 0;
                        if (entity != null)
                        {
                            valor = (decimal?)entity.Medinth1;
                        }
                        var serie = new DatosSerie();
                        serie.X = listFechas[j];
                        serie.Y = valor;
                        valorAcumulado += valor;
                        model.Grafico.SerieDataS[i][j] = serie;

                    }
                    model.Grafico.Series[i].Acumulado = (decimal?)valorAcumulado;
                }
            }
            return model;
        }

        /// <summary>
        /// Genera Archivo Gráfico para reporte general de consumo de combustible
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGraficoConsumo(string fechaInicial, string fechaFinal, int tipoGrafico)
        {
            int indicador = 1;
            try
            {
                List<MeMedicionxintervaloDTO> lista = new List<MeMedicionxintervaloDTO>();
                lista = this.ListaMedicionxIntervalo;
                if (lista.Count > 0)
                { // si hay registros
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                    this.servicio.GenerarArchivoGrafConsumo(lista, fechaInicial, fechaFinal,
                        ruta + StockConsumoArchivo.RptExcelGraficoConsumo, ruta + ConstantesStockCombustibles.NombreLogoCoes, tipoGrafico);
                    indicador = 1;
                }
                else
                    indicador = 2; // no existen registros
            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        #endregion

        #region METODOS DE REPORTE PRESION DE GAS / TEMPERATURA AMBIENTE

        /// <summary>
        /// Permite listar el historico de Presión de Gas ó Temperatura Ambiente de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult PresionGas()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
            model.ListaParametros = Util.ObtenerListaParametros();
            return View(model);
        }

        /// <summary>
        /// Devuelve lista parcial para el Listado de Presion de Gas y Temperatura
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idParametro"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaPresionGasTemperatura(string idsTipoAgente, string idsCentralInt, string idsAgente,
                                                       int idParametro, string idsFechaIni, string idsFechaFin, int nroPagina)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicion24DTO> listaReporte = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (idParametro == 1) // Presion gas
            {
                lista = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
            }
            else // Temperatura ambiente
            {
                lista = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
                //lista = this.servicio.ListaM24PresionGas(ConstantesCombutiblesPr5.LectCodiTempAmbiente, ConstantesCombutiblesPr5.Origlectcodi, idsAgente, fechaInicial, fechaFinal, ConstantesCombutiblesPr5.TipotomedicodiTemperatura, ConstantesCombutiblesPr5.TipoinfocodiTemperatura);
            }
            this.ListaFechas = lista.Select(x => x.Medifecha).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[nroPagina - 1];
                this.NroPaginasReporte = ListaFechas.Count;
            }

            if (idParametro == 1) // Presion Gas
            {
                listaReporte = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaInicial, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
            }
            else //Temperatura ambiente
            {
                listaReporte = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaInicial, strCentralInt);
            }
            string resultado = this.servicio.GeneraViewReportePresionGasTemperatura(listaReporte, fechaInicial, idParametro);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Muestra el gráfico de la Presión de Gas ó Temperatura Ambiente de las centrales termoeléctricas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idParametro"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoRptPresionGasTemperatura(string idsTipoAgente, string idsCentralInt, string idsAgente,
                                                       int idParametro, string idsFechaIni, string idsFechaFin, int nroPagina)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicion24DTO> listaReporte = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (idParametro == 1) // Presion gas
            {
                lista = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
            }
            else // Temperatura ambiente
            {
                lista = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
            }
            // reservamos la lista en la variable session para el reporte grafico en excel            

            this.ListaFechas = lista.Select(x => x.Medifecha).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[nroPagina - 1];
                NroPaginasReporte = ListaFechas.Count;
            }

            if (idParametro == 1) // Presion Gas
            {
                listaReporte = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaInicial, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
            }
            else //Temperatura ambiente
            {
                listaReporte = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaInicial, strCentralInt);
            }
            this.ListaMedicion24 = listaReporte;
            model = GraficoPresionGasTemperatura(listaReporte, idParametro);
            model.Resultado = "Resultado Presion Gas";
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Genera los datos para el grafico Presión de Gas ó Temperatura Ambiente de las centrales termoeléctricas
        /// </summary>
        /// <param name="listaReporte"></param>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public StockCombustiblesModel GraficoPresionGasTemperatura(List<MeMedicion24DTO> listaReporte, int idParametro)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            GraficoWeb grafico = new GraficoWeb();
            model.Grafico = new GraficoWeb();
            DateTime fechaIni = DateTime.MinValue;
            //model.Grafico = grafico;
            List<MeMedicion24DTO> listaCabeceraM24 = new List<MeMedicion24DTO>();
            if (idParametro == 1) // Presión de Gas
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Gruponomb, x.Ptomedielenomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Gruponomb = y.Key.Gruponomb,
                                     Ptomedielenomb = y.Key.Ptomedielenomb,
                                 }
                                 ).ToList();
            }
            else //Temperatura Ambiente
            {
                listaCabeceraM24 = listaReporte.GroupBy(x => new { x.Ptomedicodi, x.Emprnomb, x.Ptomedibarranomb, x.Tipoinfoabrev, x.Tipoptomedinomb, x.Equinomb })
                                 .Select(y => new MeMedicion24DTO()
                                 {
                                     Emprnomb = y.Key.Emprnomb,
                                     Ptomedicodi = y.Key.Ptomedicodi,
                                     Ptomedibarranomb = y.Key.Ptomedibarranomb,
                                     Tipoinfoabrev = y.Key.Tipoinfoabrev,
                                     Tipoptomedinomb = y.Key.Tipoptomedinomb,
                                     Equinomb = y.Key.Equinomb,
                                 }
                                 ).ToList();
            }
            var listFechas = listaReporte.Select(x => x.Medifecha).Distinct().ToList();
            if (listFechas.Count > 0)
            {
                fechaIni = listFechas.Min();
            }
            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
            //model.FechaInicio = listaReporte[0].Medifecha.ToString(Constantes.FormatoFecha);

            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();
            model.Grafico.SerieDataS = new DatosSerie[listaCabeceraM24.Count][];

            model.Grafico.XAxisTitle = "Dia: " + model.FechaInicio;


            if (idParametro == 1) //Presión de Gas
            {
                model.Grafico.TitleText = "PRESION DE GAS NATURAL DE LAS CENTRALES TÉRMOELECTRICAS";
            }
            else // Temperatura Ambiente
            {
                model.Grafico.TitleText = "TEMPERATURA AMBIENTE DE LAS CENTRALES TÉRMOELECTRICAS";
            }

            //NOmbre del eje Y: unidades
            if (listaReporte.Count > 0)
            {
                model.Grafico.YaxixTitle = "(" + listaReporte[0].Tipoinfoabrev + ")";
            }

            //intervalos de la categoria del gráfico: Horas
            model.Grafico.XAxisCategories = new List<string>();
            //nombres de las series del gráfico: Centrales ó Equipos
            model.Grafico.SeriesName = new List<string>();
            int totalIntervalos = 24;
            // Obtener Lista de intervalos categoria del gráfico  
            for (var j = 0; j <= (totalIntervalos - 1); j++)
            {
                string hora = ("0" + j.ToString()).Substring(("0" + j.ToString()).Length - 2, 2) + ":00";
                model.Grafico.XAxisCategories.Add(hora);
            }

            //Obtener lista de nombres para la serie, eje x.
            foreach (var reg in listaCabeceraM24)
            {
                string nombreSerie = string.Empty;

                if (idParametro == 1) //Presión de Gas
                {
                    nombreSerie = reg.Ptomedielenomb;
                }
                else
                {
                    nombreSerie = reg.Equinomb;
                }

                model.Grafico.SeriesName.Add(nombreSerie);
            }

            // Obtener lista de valores para las series del grafico

            model.Grafico.SeriesData = new decimal?[listaCabeceraM24.Count()][];

            for (var i = 0; i < listaCabeceraM24.Count(); i++)
            {

                model.Grafico.SeriesData[i] = new decimal?[totalIntervalos];
                for (var j = 1; j <= totalIntervalos; j++)
                {
                    decimal? valor = 0;

                    var entity = listaReporte.Find(x => x.Ptomedicodi == listaCabeceraM24[i].Ptomedicodi);

                    if (entity != null)
                    {
                        valor = (decimal?)entity.GetType().GetProperty("H" + j).GetValue(entity, null);
                        if (valor != null)
                            model.Grafico.SeriesData[i][j - 1] = valor;
                        else
                            model.Grafico.SeriesData[i][j - 1] = 0;
                    }
                }
            }

            return model;
        }


        /// <summary>
        /// Exporta el reporte general de Presión de Gas ó Temperatura Ambiente a archivo excel
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idParametro"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarRptExcelPresTemp(string idsTipoAgente, string idsCentralInt, string idsAgente, int idParametro, string idsFechaIni, string idsFechaFin)
        {
            int indicador = -1;
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicion24DTO> listaReporte = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                if (idParametro == 1) // Presion gas
                {
                    lista = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
                }
                else // Temperatura ambiente
                {
                    lista = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
                }
                this.ListaFechas = lista.Select(x => x.Medifecha).Distinct().ToList();
                if (ListaFechas.Count > 0)
                {
                    fechaInicial = ListaFechas.Min();
                    fechaFinal = ListaFechas.Max();
                }

                if (idParametro == 1) // Presion Gas
                {
                    listaReporte = this.servicio.ListaM24PresionGas(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, ConstantesStockCombustibles.TipotomedicodiPresionGas, ConstantesStockCombustibles.TipoInfocodiPresion, strCentralInt);
                }
                else //Temperatura ambiente
                {
                    listaReporte = this.servicio.ListaM24TemperaturaAmbiente(ConstantesStockCombustibles.LectCodiPresionGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
                }
                if (listaReporte.Count > 0) // Si existen registros
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                    this.servicio.GenerarExcelPresTemp(listaReporte, idsFechaIni, idsFechaFin, idParametro,
                        ruta + StockConsumoArchivo.RptExcelHistorico, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                    indicador = 1;
                }
                else // Si no existen registros
                    indicador = 2;

            }
            catch (Exception ex)
            {
                log.Error("GenerarRptExcelPresTemp", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Genera Archivo Gráfico para reporte general de Presión de Gas   ó Temperatura Ambiente
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarArchivoGraficoPresTemp(int idParametro)
        {
            int indicador = 1;
            try
            {
                List<MeMedicion24DTO> lista = new List<MeMedicion24DTO>();
                lista = this.ListaMedicion24;
                DateTime fechaInicial = lista[0].Medifecha;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                this.servicio.GenerarArchivoGrafPresTemp(lista, fechaInicial,
                    ruta + StockConsumoArchivo.RptExcelGraficoPresTemp, ruta + ConstantesStockCombustibles.NombreLogoCoes, idParametro);
                indicador = 1;
            }
            catch (Exception ex)
            {
                log.Error("GenerarArchivoGraficoPresTemp", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        #endregion

        #region METODOS DE REPORTE DISPONIBILIDAD DE GAS NATURAL
        /// <summary>
        /// Permite listar el historico de disponibilidad de Gas gas natural de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult DisponibilidadGas()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
            model.ListaYacimientos = servicio.ListEqCategoriaDetalleByCategoria(ConstantesIntranet.YacimientoGasCodi);
            return View(model);
        }



        /// <summary>
        /// Devuelve lista parcial para el Listado de Disponibilidad de Gas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaDisponibilidadGas(string idsTipoAgente, string idsCentralInt, string idsYacimientos, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            listaReporte = this.servicio.ListaMedxIntervDisponibilidad(ConstantesStockCombustibles.LectCodiDisponibilidad, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt, ConstantesIntranet.YacimientoGasCodi, idsYacimientos);
            this.ListaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            string resultado = this.servicio.GeneraViewReporteDisponibilidadGas(listaReporte, ListaFechas);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Exporta el reporte general de Disponibilidad de Gas a archivo excel
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteXLSDisponibilidad(string idsTipoAgente, string idsCentralInt, string idsYacimientos, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            int indicador = 1;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                listaReporte = this.servicio.ListaMedxIntervDisponibilidad(ConstantesStockCombustibles.LectCodiDisponibilidad, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt, ConstantesIntranet.YacimientoGasCodi, idsYacimientos);
                if (listaReporte.Count > 0)
                { //Si existen registros
                    this.servicio.GenerarArchivoExcelDisponibilidadGas(listaReporte, idsFechaIni, idsFechaFin,
                        ruta + StockConsumoArchivo.RptExcelDisponibilidad, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                    indicador = 1;
                }
                else
                    indicador = 2; // no existen registros

            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteXLSDisponibilidad", ex);
                indicador = -1;
            }

            return Json(indicador);
        }
        #endregion

        #region METODOS DE REPORTE QUEMA DE GAS
        /// <summary>
        /// Permite listar el historico de quema de Gas gas natural de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult QuemaGas()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
            model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
            return View(model);
        }
        /// <summary>
        /// Devuelve lista parcial para el Listado de Quema de Gas
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaQuemaGas(string idsTipoAgente, string idsCentralInt, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            listaReporte = this.servicio.ListaMedxIntervQuema(ConstantesStockCombustibles.LectCodiQuemaGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
            this.ListaFechas = listaReporte.Select(x => x.Medintfechaini).Distinct().ToList();
            if (ListaFechas.Count > 0)
            {
                fechaInicial = ListaFechas[0];
                fechaFinal = ListaFechas[ListaFechas.Count - 1];
            }

            string resultado = this.servicio.GeneraViewReporteQuemaGas(listaReporte, ListaFechas);
            model.Resultado = resultado;
            return PartialView(model);
        }

        /// <summary>
        /// Exporta el reporte histórico de Quema de Gas a archivo excel
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporteXLSQuema(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin)
        {
            int indicador = 1;
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                listaReporte = this.servicio.ListaMedxIntervQuema(ConstantesStockCombustibles.LectCodiQuemaGas, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, strCentralInt);
                if (listaReporte.Count > 0)
                { // Sino existen registros
                    this.servicio.GenerarArchivoExcelQuemaGas(listaReporte, idsFechaIni, idsFechaFin,
                        ruta + StockConsumoArchivo.RptExcelQuema, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                    indicador = 1;
                }
                else
                {// No existen registros
                    indicador = 2;
                }
            }
            catch (Exception ex)
            {
                log.Error("GenerarReporteXLSQuema", ex);
                indicador = -1;
            }

            return Json(indicador);
        }

        #endregion

        #region METODOS DE REPORTE COMBUSTIBLES ACUMULADOS

        /// <summary>
        /// Index de Reporte Acumulado de combustibles
        /// </summary>
        /// <returns></returns>
        //public ActionResult Acumulado()
        //{
        //    StockCombustiblesModel model = new StockCombustiblesModel();
        //    model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
        //    model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
        //    model.ListaTipoAgente = Util.ObtenerListaTipoAgente();
        //    model.ListaCentralIntegrante = Util.ObtenerCentralIntegrante();
        //    model.ListaEstFisCombustible = Util.ObtenerListaEstFisCombustible(ConstantesIntranet.TipoReporteStock);
        //    return View(model);

        //}

        /// <summary>
        /// Devuelve lista parcial para el Listado de Combustible Acumulado
        /// </summary>
        /// <param name="idsTipoAgente"></param>
        /// <param name="idsCentralInt"></param>
        /// <param name="idsRecurso"></param>
        /// <param name="idsAgente"></param>
        /// <param name="idsEstado"></param>
        /// <param name="idsFechaIni"></param>
        /// <param name="idsFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaAcumuladoCombustible(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente,
                                                       idsEstado, idsFechaIni, idsFechaFin, idsEquipo, "-1");
            this.ListaMedicionxIntervalo = listaReporte;
            //string resultado = this.servicio.GeneraViewReporteAcumuladoCombustible(listaReporte, idsEstado);
            //model.Resultado = resultado;            

            List<MeMedicionxintervaloDTO> listAcumulado = listaReporte.GroupBy(t => new { t.Ptomedicodi, t.Emprnomb, t.Equinomb, t.Fenergnomb, t.Fenercolor, t.Tipoinfoabrev, })
                                    .Select(g => new MeMedicionxintervaloDTO()
                                    {
                                        Ptomedicodi = g.Key.Ptomedicodi,
                                        Emprnomb = g.Key.Emprnomb,
                                        Equinomb = g.Key.Equinomb,
                                        Fenergnomb = g.Key.Fenergnomb,
                                        Fenercolor = g.Key.Fenercolor,
                                        Tipoinfoabrev = g.Key.Tipoinfoabrev,
                                        H1Recep = g.Sum(t => t.H1Recep)
                                    }).ToList();

            model.ListaAcumulado = listAcumulado.Where(x => x.H1Recep > 0).ToList();
            model.FechaInicio = idsFechaIni;
            model.FechaFin = idsFechaFin;
            return PartialView(model);
        }


        // Exporta el reporte general de combustibles acumulados a archivo excel
        [HttpPost]
        public JsonResult GenerarReporteAcumuladoXLS(string idsTipoAgente, string idsCentralInt, string idsRecurso, string idsAgente,
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string idsEquipo)
        {
            int indicador = 1;
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();

            try
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
                listaReporte = this.servicio.GetListaReporteStock(idsTipoAgente, idsCentralInt, idsRecurso, idsAgente, idsEstado, idsFechaIni, idsFechaFin, idsEquipo, "-1");
                if (listaReporte.Count > 0)
                { // Si existen registros
                    this.servicio.GenerarArchivoExcelAcumulado(listaReporte, idsFechaIni, idsFechaFin,
                        ruta + StockConsumoArchivo.RptExcelAcumulado, ruta + ConstantesStockCombustibles.NombreLogoCoes);
                    indicador = 1;
                }
                else
                    indicador = 2; // No existen registros

            }
            catch
            {
                indicador = -1;
            }

            return Json(indicador);
        }

        /// <summary>
        /// Muestra popup para detalle recepción de combustible acumulado
        /// </summary>
        /// <param name="ptomedicodi"></param>
        /// <returns></returns>
        public PartialViewResult VerDetalleAcumulado(int ptomedicodi)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            model.Emprnomb = string.Empty;
            model.Equinomb = string.Empty;

            if (ptomedicodi == -1)
                listaReporte = this.ListaMedicionxIntervalo;
            else
                listaReporte = this.ListaMedicionxIntervalo.Where(x => x.Ptomedicodi == ptomedicodi).ToList();
            if (listaReporte.Count > 0)
            {
                model.Emprnomb = listaReporte[0].Emprnomb;
                model.Equinomb = listaReporte[0].Equinomb;
            }
            model.ListaAcumulado = listaReporte;

            return PartialView(model);
        }

        #endregion

        #region Validacion Horas de Operacion con Despacho

        /// <summary>
        /// Index para Validacion de Horas de Operacion
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidacionHO()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            return View(model);
        }

        /// <summary>
        /// Devuelve lista parcial para el Listado de Horas de Operacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaValidacionHO(string fecha)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Resultado = servicio.GeneraViewReporteValidacion(dfecha);
            return PartialView(model);
        }
        #endregion

        #region Validacion Horas de Operacion con Medidores

        /// <summary>
        /// Index para Validacion de Horas de Operacion
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidacionHO96()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            return View(model);
        }

        /// <summary>
        /// Devuelve lista parcial para el Listado de Validacion de Horas de Operacion
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaValidacionHO96(string fecha)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Resultado = servicio.GeneraViewReporteValidacion96(dfecha);
            return PartialView(model);
        }
        #endregion

        #region UTIL
        /// <summary>
        /// Obtiene las empresas seleccionadas según el filtro "INTEGRANTE", "NO INTEGRANTE"
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarAgentes(string idTipoAgente)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            if (idTipoAgente != "-1")
            {
                entitys = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);
                List<string> tipoAgente = new List<string>();
                tipoAgente = idTipoAgente.Split(',').ToList();

                entitys = entitys.Where(x => tipoAgente.Contains(x.Emprcoes)).ToList();
            }
            else
            {
                //entitys = servicio.ListEqEquipoEmpresaGEN("-1").ToList();
            }
            model.ListaEmpresas = entitys;
            return PartialView(model);
        }

        public PartialViewResult CargarCentrales(string idsAgente)
        {
            FiltroStockCombustibleModel model = new FiltroStockCombustibleModel();
            model.ListaCentral = servicio.ListarCentralesXEmpresaGEN(idsAgente);

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el listado de los tipos de recurso energéticos según el estado físico del combustible seleccionado
        /// </summary>
        /// <param name="idEstadoFisico"></param>
        /// <returns></returns>
        public PartialViewResult CargarRecursoEnergetico(string idEstadoFisico, int iCodReporte)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            string strRecursoEner = string.Empty;// GeneraCodRecurEnergetico(idEstadoFisico, iCodReporte);
            List<MeTipopuntomedicionDTO> entitys = new List<MeTipopuntomedicionDTO>();
            if (iCodReporte == ConstantesIntranet.TipoReporteStock)
                strRecursoEner = ConstantesStockCombustibles.StrTptoStock;
            else
                strRecursoEner = ConstantesStockCombustibles.StrTptoConsumo;
            entitys = this.servicio.ListaMeTipoPuntoMedicion(strRecursoEner, idEstadoFisico);
            model.ListaRecursoEnergetico = entitys;
            model.codReporte = iCodReporte;
            return PartialView(model);


        }

        /// <summary>
        /// Permite exportar el reporte general a formato excel
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
                    nombreArchivo = StockConsumoArchivo.RptExcelStock; // Reporte Stock de combustibles 
                    break;
                case 2:
                    nombreArchivo = StockConsumoArchivo.RptExcelConsumo; // Reporte Consumo de combustibles
                    break;
                case 3:
                    nombreArchivo = StockConsumoArchivo.RptExcelHistorico; // Reporte Presión de Gas/Temperatura Ambiente
                    break;
                case 4:
                    nombreArchivo = StockConsumoArchivo.RptExcelGraficoStock; // Reporte Gráfico Stock de combustibles 
                    break;
                case 5:
                    nombreArchivo = StockConsumoArchivo.RptExcelGraficoConsumo; // Reporte Gráfico Consumo de combustibles 
                    break;
                case 6:
                    nombreArchivo = StockConsumoArchivo.RptExcelGraficoPresTemp; // Reporte Gráfico Presion o Temperatura de Gas
                    break;
                case 7:
                    nombreArchivo = StockConsumoArchivo.RptExcelDisponibilidad; // Reporte Disponibilidad de Gas
                    break;
                case 8:
                    nombreArchivo = StockConsumoArchivo.RptExcelQuema; // Reporte Quema de Gas
                    break;
                case 9:
                    nombreArchivo = StockConsumoArchivo.RptExcelAcumulado; // Reporte Combustible Acumulado
                    break;
                case 10:
                    nombreArchivo = StockConsumoArchivo.RptExcelAcumuladoDet; // Reporte Combustible Acumulado
                    break;
            }

            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.DirectorioVolumenCombustible;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesIntranet.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite pintar el paginado del reporte
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado()
        {
            Paginacion model = new Paginacion();
            int nroPaginas = this.NroPaginasReporte;
            model.NroPaginas = nroPaginas;
            model.NroMostrar = ConstantesIntranet.NroPageShow;
            model.IndicadorPagina = true;
            return base.Paginado(model);
        }


        #endregion

    }
}
