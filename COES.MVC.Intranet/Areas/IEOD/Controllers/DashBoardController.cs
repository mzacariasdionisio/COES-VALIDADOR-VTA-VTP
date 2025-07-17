using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IEOD.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IEOD.Controllers
{
    public class DashBoardController : BaseController
    {
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(AnexoAController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
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

        #endregion

        //
        // GET: /IEOD/DashBoard/
        public ActionResult Index()
        {
            BusquedaIEODModel model = new BusquedaIEODModel();
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.ListaTipoDashboard = this.servicio.ListarTipoDashboardIEOD();
            return View(model);
        }

        /// <summary>
        /// Cargar reporte y graficos del tipo de Dashboard
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public JsonResult CargarTipoDashboard(string strFecha, int tipo)
        {
            DateTime fecha = DateTime.ParseExact(strFecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            List<PublicacionIEODModel> listaModel = new List<PublicacionIEODModel>();
            PublicacionIEODModel model;

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashProdYMaxDem:
                    #region
                    List<SiTipogeneracionDTO> listaCategoria = this.servicio.ListarSiTipogeneracion();
                    List<MeMedicion24DTO> listaDataFinal;
                    this.servicio.ListarBalanceEletrico24(fecha, listaCategoria, out listaDataFinal);

                    //html
                    model = new PublicacionIEODModel();
                    model.Resultado = this.servicio.ListarBalanceEletricoHtml(listaDataFinal, listaCategoria);
                    listaModel.Add(model);

                    //graficos
                    model = this.GraficoProdEnergYMaxDemXTipoGeneracion(fecha, listaCategoria, listaDataFinal, ConstantesPR5ReportesServicio.TipoDashBEDia);
                    listaModel.Add(model);

                    model = this.GraficoProdEnergYMaxDemXTipoGeneracion(fecha, listaCategoria, listaDataFinal, ConstantesPR5ReportesServicio.TipoDashBEMes);
                    listaModel.Add(model);

                    model = this.GraficoProdEnergYMaxDemXTipoGeneracion(fecha, listaCategoria, listaDataFinal, ConstantesPR5ReportesServicio.TipoDashBEAnio);
                    listaModel.Add(model);

                    model = this.GraficoProdEnergYMaxDemXTipoGeneracion(fecha, listaCategoria, listaDataFinal, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);
                    listaModel.Add(model);

                    List<MaximaDemandaDTO> listaMaximaDemada;
                    this.servicio.ListarDemandaMaximaDashboard48(fecha, out listaMaximaDemada);

                    //Demanda maxima
                    model = new PublicacionIEODModel();
                    model.Resultado = this.servicio.ListarDemandaMaximaDashboardHtml(listaMaximaDemada);
                    listaModel.Add(model);
                    #endregion
                    break;
                case ConstantesPR5ReportesServicio.TipoDashDemAreaOp:
                    #region
                    List<MeReporptomedDTO> listaAreaOperativaTotal = this.servicio.ListaAreaOperativaDashboard();
                    List<MeReporptomedDTO> listaAreaOperativa = listaAreaOperativaTotal.Where(x => x.Ptomedicodi != ConstantesPR5ReportesServicio.PtomedicodiSein).ToList();
                    List<MeMedicion24DTO> listaDataAreaOpFinal;
                    List<MaximaDemandaDTO> listaMaximaDemadaAreaOp, listaMaximaDemadaAreaOpTmp;
                    this.servicio.ListarDemandaXAreaOperativa24Data(fecha, listaAreaOperativaTotal, out listaDataAreaOpFinal, out listaMaximaDemadaAreaOp);

                    //html
                    model = new PublicacionIEODModel();
                    model.ListaAreaOperativa = listaAreaOperativaTotal;
                    model.Resultado = this.servicio.ListarDemandaXAreaOperativa24Html(listaDataAreaOpFinal, listaAreaOperativaTotal);
                    listaModel.Add(model);

                    //graficos
                    model = this.GraficoDemandaXAreaOperativa(fecha, listaAreaOperativa, listaDataAreaOpFinal, ConstantesPR5ReportesServicio.TipoDashBEDia);
                    listaModel.Add(model);

                    model = this.GraficoDemandaXAreaOperativa(fecha, listaAreaOperativa, listaDataAreaOpFinal, ConstantesPR5ReportesServicio.TipoDashBEMes);
                    listaModel.Add(model);

                    model = this.GraficoDemandaXAreaOperativa(fecha, listaAreaOperativa, listaDataAreaOpFinal, ConstantesPR5ReportesServicio.TipoDashBEAnio);
                    listaModel.Add(model);

                    model = this.GraficoDemandaXAreaOperativa(fecha, listaAreaOperativa, listaDataAreaOpFinal, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);
                    listaModel.Add(model);

                    //Demanda maxima
                    foreach (var rep in listaAreaOperativaTotal)
                    {
                        listaMaximaDemadaAreaOpTmp = listaMaximaDemadaAreaOp.Where(x => x.Ptomedicodi == rep.Ptomedicodi).ToList();

                        model = new PublicacionIEODModel();
                        model.Resultado = this.servicio.ListarDemandaMaximaDashboardHtml(listaMaximaDemadaAreaOpTmp);
                        listaModel.Add(model);
                    }

                    #endregion
                    break;
                case ConstantesPR5ReportesServicio.TipoDashConsGranUsuario:
                    #region
                    List<MeReporteDTO> listaReporte = UtilSemanalPR5.GetListaReporteUL(true);
                    List<MeReporptomedDTO> listaPtos = new List<MeReporptomedDTO>();
                    List<MeMedicion24DTO> listaDataFinalUL, listaDataFinalULArea;
                    this.servicio.ListarConsumoGrandesUsuarios(fecha, listaReporte, listaPtos, out listaDataFinalUL); //demora 15s

                    model = new PublicacionIEODModel();
                    model.Resultado = this.servicio.ListarConsumoGrandesUsuariosHtml(listaDataFinalUL, listaReporte, listaPtos);  //12 segundos
                    listaModel.Add(model);

                    listaDataFinalULArea = listaDataFinalUL.Where(x => x.TipoReporte == ConstantesPR5ReportesServicio.TipoReporteConsolidado).ToList();

                    model = this.GraficoConsGranUsuarioXArea(fecha, listaReporte, listaDataFinalULArea, ConstantesPR5ReportesServicio.TipoDashBEDia); //3 seg
                    listaModel.Add(model);

                    model = this.GraficoConsGranUsuarioXArea(fecha, listaReporte, listaDataFinalULArea, ConstantesPR5ReportesServicio.TipoDashBEMes);  // 4 seg
                    listaModel.Add(model);

                    model = this.GraficoConsGranUsuarioXArea(fecha, listaReporte, listaDataFinalULArea, ConstantesPR5ReportesServicio.TipoDashBEAnio);  //4 seg
                    listaModel.Add(model);

                    model = this.GraficoConsGranUsuarioXArea(fecha, listaReporte, listaDataFinalULArea, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);  //4 seg
                    listaModel.Add(model);
                    #endregion
                    break;
                case ConstantesPR5ReportesServicio.TipoDashCostOp:
                    break;
                case ConstantesPR5ReportesServicio.TipoDashCostMarg:
                    break;
                case ConstantesPR5ReportesServicio.TipoDashRREE:
                    #region
                    List<EstadoCombustibleIEOD> listaEstComb = this.servicio.ListarEstadoCombustible();
                    List<SiFuenteenergiaDTO> listaFteEnerg = this.servicio.ListarFuenteEnergia();

                    List<MeMedicion24DTO> listaDataRREECons, listaDataRREEDet, listaDataRERCons, listaDataRERDet;
                    this.servicio.ListarRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, out listaDataRREEDet, out listaDataRERDet);

                    listaDataRREECons = listaDataRREEDet.Where(x => x.TipoReporte == ConstantesPR5ReportesServicio.TipoReporteConsolidado).ToList();
                    listaDataRERCons = listaDataRERDet.Where(x => x.TipoReporte == ConstantesPR5ReportesServicio.TipoReporteConsolidado).ToList();

                    model = new PublicacionIEODModel();
                    model.Resultado = this.servicio.ListarRecursosEnergeticosDashboard24Html(listaDataRREEDet, listaFteEnerg, listaEstComb);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRREECons, listaDataRREEDet, ConstantesPR5ReportesServicio.TipoDashBEDia);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRREECons, listaDataRREEDet, ConstantesPR5ReportesServicio.TipoDashBEMes);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRREECons, listaDataRREEDet, ConstantesPR5ReportesServicio.TipoDashBEAnio);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRREECons, listaDataRREEDet, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);
                    listaModel.Add(model);

                    //RER
                    model = new PublicacionIEODModel();
                    model.Resultado = this.servicio.ListarRecursosEnergeticosRERDashboard24Html(listaDataRERDet, listaFteEnerg, listaEstComb);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRERCons, listaDataRERDet, ConstantesPR5ReportesServicio.TipoDashBEDia);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRERCons, listaDataRERDet, ConstantesPR5ReportesServicio.TipoDashBEMes);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRERCons, listaDataRERDet, ConstantesPR5ReportesServicio.TipoDashBEAnio);
                    listaModel.Add(model);

                    model = this.GraficoRecursosEnergeticosDashboard24(fecha, listaFteEnerg, listaEstComb, listaDataRERCons, listaDataRERDet, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);
                    listaModel.Add(model);

                    #endregion
                    break;
                case ConstantesPR5ReportesServicio.TipoDashRER:
                case ConstantesPR5ReportesServicio.TipoDashComb:
                    break;
                case ConstantesPR5ReportesServicio.TipoDashCostVar:
                    break;
                case ConstantesPR5ReportesServicio.TipoDashEnergPrim:
                    #region

                    List<TipoEnergiaPrimariaIEOD> listaRep = this.servicio.ListarReporteEnergiaPrimaria();
                    List<MePtomedicionDTO> listaPto = this.servicio.ListarPtoEnergiaPrimaria(fecha, fecha);
                    List<MePtomedicionDTO> listaPtoByRep;
                    List<MeMedicion24DTO> listaDataFinalEnergPrim, listaDataFinalEnergPrimByRep;

                    this.servicio.ListarEnergiaPrimariaDashboard24(fecha, listaPto, listaRep, out listaDataFinalEnergPrim);

                    //html
                    model = new PublicacionIEODModel();
                    model.ListaTipoEnergiaPrimaria = listaRep;
                    model.Resultado = this.servicio.ListarEnergiaPrimariaDashboardHtml(listaDataFinalEnergPrim, listaPto, listaRep);
                    listaModel.Add(model);

                    //graficos
                    foreach (var rep in listaRep)
                    {
                        listaPtoByRep = listaPto.Where(x => x.Tipoptomedicodi == rep.Tptomedicodi).ToList();
                        listaDataFinalEnergPrimByRep = listaDataFinalEnergPrim.Where(x => x.Tipoptomedicodi == rep.Tptomedicodi).ToList();

                        model = this.GraficoEnergPrim(fecha, listaPtoByRep, listaDataFinalEnergPrimByRep, ConstantesPR5ReportesServicio.TipoDashBEDia);
                        listaModel.Add(model);

                        model = this.GraficoEnergPrim(fecha, listaPtoByRep, listaDataFinalEnergPrimByRep, ConstantesPR5ReportesServicio.TipoDashBEMes);
                        listaModel.Add(model);

                        model = this.GraficoEnergPrim(fecha, listaPtoByRep, listaDataFinalEnergPrimByRep, ConstantesPR5ReportesServicio.TipoDashBEAnio);
                        listaModel.Add(model);

                        model = this.GraficoEnergPrim(fecha, listaPtoByRep, listaDataFinalEnergPrimByRep, ConstantesPR5ReportesServicio.TipoDashBEAnioMovil);
                        listaModel.Add(model);
                    }

                    #endregion
                    break;
            }

            return Json(listaModel);
        }

        /// <summary>
        /// Grafico TipoGeneracion por Prod Energ Y Max Demanda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaCategoria"></param>
        /// <param name="listaData"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoProdEnergYMaxDemXTipoGeneracion(DateTime fecha, List<SiTipogeneracionDTO> listaCategoria, List<MeMedicion24DTO> listaData, int tipo)
        {
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();
            decimal? total = 0;
            string titulo = string.Empty;

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashBEDia:
                    titulo = fecha.ToString(Constantes.FormatoFecha);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEMes:
                    titulo = COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnio:
                    titulo = fecha.Year.ToString();
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnioMovil:
                    titulo = ConstantesPR5ReportesServicio.DescAcumuladoAnioMovil;
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    break;
            }

            model.Grafico.TitleText = titulo;
            total = data.Sum(x => x.Meditotal);

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();
            foreach (var cat in listaCategoria)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = cat.Tgenernomb;
                regSerie.Color = cat.Tgenercolor;
                regSerie.Acumulado = data.Find(x => x.Tgenercodi == cat.Tgenercodi).Meditotal.GetValueOrDefault(0) / ConstantesPR5ReportesServicio.FactorGW;
                regSerie.Porcentaje = total > 0 ? regSerie.Acumulado / total * 100 : null;

                listaSerie.Add(regSerie);
            }

            model.Grafico.Series = listaSerie;

            return model;
        }

        /// <summary>
        /// Grafico Demanda X Area Operativa
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaCategoria"></param>
        /// <param name="listaData"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoDemandaXAreaOperativa(DateTime fecha, List<MeReporptomedDTO> listaAreaOperativa, List<MeMedicion24DTO> listaData, int tipo)
        {
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();
            decimal? total = 0;
            string titulo = string.Empty;

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashBEDia:
                    titulo = fecha.ToString(Constantes.FormatoFecha);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEMes:
                    titulo = COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnio:
                    titulo = fecha.Year.ToString();
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnioMovil:
                    titulo = ConstantesPR5ReportesServicio.DescAcumuladoAnioMovil;
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    break;
            }

            model.Grafico.TitleText = titulo;
            total = data.Sum(x => x.Meditotal);

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();
            foreach (var cat in listaAreaOperativa)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = cat.Ptomedibarranomb;
                //regSerie.Color = cat.Tgenercolor;
                regSerie.Acumulado = data.Find(x => x.Ptomedicodi == cat.Ptomedicodi).Meditotal.GetValueOrDefault(0) / ConstantesPR5ReportesServicio.FactorGW;
                regSerie.Porcentaje = total > 0 ? regSerie.Acumulado / total * 100 : null;

                listaSerie.Add(regSerie);
            }

            model.Grafico.Series = listaSerie;

            return model;
        }

        /// <summary>
        /// Grafico UL
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaReporte"></param>
        /// <param name="listaData"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoConsGranUsuarioXArea(DateTime fecha, List<MeReporteDTO> listaReporte, List<MeMedicion24DTO> listaData, int tipo)
        {
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();
            decimal? total = 0;
            string titulo = string.Empty;

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashBEDia:
                    titulo = fecha.ToString(Constantes.FormatoFecha);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEMes:
                    titulo = COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnio:
                    titulo = fecha.Year.ToString();
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnioMovil:
                    titulo = ConstantesPR5ReportesServicio.DescAcumuladoAnioMovil;
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    break;
            }

            model.Grafico.TitleText = titulo;
            total = data.Sum(x => x.Meditotal);

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();
            foreach (var cat in listaReporte)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = cat.AreaOperativa;
                regSerie.Color = cat.Reporcolor;
                regSerie.Acumulado = data.Find(x => x.Reporcodi == cat.Reporcodi).Meditotal.GetValueOrDefault(0) / ConstantesPR5ReportesServicio.FactorGW;
                regSerie.Porcentaje = total > 0 ? regSerie.Acumulado / total * 100 : null;

                listaSerie.Add(regSerie);
            }

            model.Grafico.Series = listaSerie;

            return model;
        }

        /// <summary>
        /// Pie - Donut de Recursos Energeticos y RER
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaFteEnerg"></param>
        /// <param name="listaEstComb"></param>
        /// <param name="listaDataRREECons"></param>
        /// <param name="listaDataRREEDet"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoRecursosEnergeticosDashboard24(DateTime fecha, List<SiFuenteenergiaDTO> listaFteEnerg, List<EstadoCombustibleIEOD> listaEstComb, List<MeMedicion24DTO> listaDataRREECons, List<MeMedicion24DTO> listaDataRREEDet, int tipo)
        {
            List<MeMedicion24DTO> dataDet = new List<MeMedicion24DTO>();
            List<MeMedicion24DTO> dataCons = new List<MeMedicion24DTO>();
            decimal? totalDet = 0, totalCons = 0;
            string titulo = string.Empty;

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashBEDia:
                    titulo = fecha.ToString(Constantes.FormatoFecha);
                    dataDet = listaDataRREEDet.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    dataCons = listaDataRREECons.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEMes:
                    titulo = COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month);
                    dataDet = listaDataRREEDet.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    dataCons = listaDataRREECons.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnio:
                    titulo = fecha.Year.ToString();
                    dataDet = listaDataRREEDet.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    dataCons = listaDataRREECons.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnioMovil:
                    titulo = ConstantesPR5ReportesServicio.DescAcumuladoAnioMovil;
                    dataDet = listaDataRREEDet.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    dataCons = listaDataRREECons.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    break;
            }

            model.Grafico.TitleText = titulo;
            totalDet = dataDet.Sum(x => x.Meditotal);
            totalCons = dataCons.Sum(x => x.Meditotal);
            model.Grafico.XAxisTitle = "Estado";
            model.Grafico.YaxixTitle = "Energía";

            //Inicializacion de la data
            List<RegistroSerie> listaSerie = new List<RegistroSerie>();
            foreach (var cat in listaEstComb)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Codigo = cat.Estcomcodi;
                regSerie.Name = cat.Estcomnomb;
                regSerie.Color = cat.Estcomcolor;
                regSerie.Acumulado = dataCons.Find(x => x.Estcomcodi == cat.Estcomcodi).Meditotal.GetValueOrDefault(0) / ConstantesPR5ReportesServicio.FactorGW;
                regSerie.Porcentaje = totalCons != 0 ? regSerie.Acumulado / totalCons * 100 : 0;
                listaSerie.Add(regSerie);
            }
            model.Grafico.Series = listaSerie;
            model.Grafico.SerieDataS = new DatosSerie[listaSerie.Count][];

            //Data para cada estado de combustible
            for (int i = 0; i < listaSerie.Count; i++)
            {
                var listaFEXEstComb = listaFteEnerg.Where(x => x.Estcomcodi == listaSerie[i].Codigo).ToList();
                List<int> listaFenergcodi = new List<int>();
                totalDet = dataDet.Where(x => listaFenergcodi.Contains(x.Fenergcodi)).Sum(x => x.Meditotal);

                model.Grafico.SerieDataS[i] = new DatosSerie[listaFEXEstComb.Count];
                int j = 0;
                foreach (var regFE in listaFEXEstComb)
                {
                    var regXEstcomb = dataDet.Find(x => x.Fenergcodi == regFE.Fenergcodi);

                    DatosSerie d = new DatosSerie();
                    d.Name = regFE.Fenergnomb;
                    d.Y = regXEstcomb != null ? regXEstcomb.Meditotal / ConstantesPR5ReportesServicio.FactorGW : 0;
                    d.Z = totalDet != 0 ? d.Y / totalCons * 100 : 0;
                    d.Color = regFE.Fenergcolor;
                    model.Grafico.SerieDataS[i][j] = d;
                    j++;
                }
            }

            return model;
        }

        /// <summary>
        /// Grafico 
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="listaCategoria"></param>
        /// <param name="listaData"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public PublicacionIEODModel GraficoEnergPrim(DateTime fecha, List<MePtomedicionDTO> listaPto, List<MeMedicion24DTO> listaData, int tipo)
        {
            List<MeMedicion24DTO> data = new List<MeMedicion24DTO>();
            decimal? total = 0;
            string titulo = string.Empty;

            PublicacionIEODModel model = new PublicacionIEODModel();
            model.Grafico = new GraficoWeb();

            switch (tipo)
            {
                case ConstantesPR5ReportesServicio.TipoDashBEDia:
                    titulo = fecha.ToString(Constantes.FormatoFecha);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEDia).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEMes:
                    titulo = COES.Base.Tools.Util.ObtenerNombreMes(fecha.Month);
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEMes).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnio:
                    titulo = fecha.Year.ToString();
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnio).ToList();
                    break;
                case ConstantesPR5ReportesServicio.TipoDashBEAnioMovil:
                    titulo = ConstantesPR5ReportesServicio.DescAcumuladoAnioMovil;
                    data = listaData.Where(x => x.TipoResultadoFecha == ConstantesPR5ReportesServicio.TipoDashBEAnioMovil).ToList();
                    break;
            }

            model.Grafico.TitleText = titulo;
            total = data.Sum(x => x.Meditotal);

            List<RegistroSerie> listaSerie = new List<RegistroSerie>();
            foreach (var cat in listaPto)
            {
                RegistroSerie regSerie = new RegistroSerie();
                regSerie.Name = cat.Ptomedidesc;
                regSerie.YAxisTitle = cat.Tipoinfoabrev;
                //regSerie.Color = cat.Tgenercolor;
                regSerie.Acumulado = data.Find(x => x.Ptomedicodi == cat.Ptomedicodi).Meditotal.GetValueOrDefault(0);
                regSerie.Porcentaje = total > 0 ? regSerie.Acumulado / total * 100 : null;

                listaSerie.Add(regSerie);
            }

            model.Grafico.Series = listaSerie;

            return model;
        }

        /// <summary>
        ///  Permite generar el archivo Excel del Tipo de Dashboard seleccionado
        /// </summary>
        /// <param name="strFecha"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarTipoDashboard(string strFecha, int tipo)
        {
            string rutaTmp = string.Empty;
            string nombreArchivo = string.Empty;
            string[] datos = new string[2];

            try
            {
                switch (tipo)
                {
                    case ConstantesPR5ReportesServicio.TipoDashProdYMaxDem:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashDemAreaOp:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashConsGranUsuario:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashCostOp:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashCostMarg:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashRREE:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashRER:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashComb:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashCostVar:
                        break;
                    case ConstantesPR5ReportesServicio.TipoDashEnergPrim:
                        break;
                }
                datos[0] = rutaTmp;
                datos[1] = nombreArchivo;

                return Json(datos);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { Error = -1, Descripcion = ex.Message, Detalle = ex.StackTrace });
            }
        }

        /// <summary>
        /// Permite descargar el Tipo de Dashboard seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarTipoDashboard()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesIEOD.ModuloManualUsuarioSGI;
            string nombreArchivo = ConstantesIEOD.ArchivoManualUsuarioIntranetSGI;
            string pathDestino = modulo + ConstantesIEOD.FolderRaizSGIModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}
