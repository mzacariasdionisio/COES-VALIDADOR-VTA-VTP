using COES.MVC.Intranet.Areas.Rdo.Models;
using COES.MVC.Intranet.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.MVC.Intranet.Areas.Rdo.Helper;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Globalization;
using System.Text;
using COES.MVC.Intranet.Helper;
using COES.Framework.Base.Core;
using log4net;

namespace COES.MVC.Intranet.Areas.Rdo.Controllers
{
    public class DisponibilidadCombustibleController : BaseController
    {       
        StockCombustiblesAppServicio servicio = new StockCombustiblesAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(DisponibilidadCombustibleController));
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

        public DisponibilidadCombustibleController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region METODOS DE REPORTE CONSUMO DE COMBUSTIBLES

        /// <summary>
        /// Permite listar el historico de Consumo de combustibles de las centrales termoeléctricas
        /// </summary>
        /// <returns></returns>
        public ActionResult DisponibilidadCombustible()
        {
            DisponibilidadCombustiblesModel model = new DisponibilidadCombustiblesModel();
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
                                                       string idsEstado, string idsFechaIni, string idsFechaFin, string horario)
        {
            DisponibilidadCombustiblesModel model = new DisponibilidadCombustiblesModel();
            List<MeMedicionxintervaloDTO> listaReporte = new List<MeMedicionxintervaloDTO>();
            string strCentralInt = this.servicio.GeneraCodCentralIntegrante(idsCentralInt);
            DateTime fechaInicial = DateTime.MinValue;
            DateTime fechaFinal = DateTime.MinValue;
            fechaInicial = DateTime.ParseExact(idsFechaIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFinal = DateTime.ParseExact(idsFechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            listaReporte = this.servicio.ListaMedDisponibilidadCombustible(ConstantesIntranet.LectCodiDisponibilidad, ConstantesStockCombustibles.Origlectcodi, idsAgente, fechaInicial, fechaFinal, idsEstado, idsRecurso, strCentralInt, horario);
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
            DisponibilidadCombustiblesModel model = new DisponibilidadCombustiblesModel();
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

        #endregion

        
        #region UTIL
        /// <summary>
        /// Obtiene las empresas seleccionadas según el filtro "INTEGRANTE", "NO INTEGRANTE"
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarAgentes(string idTipoAgente)
        {
            DisponibilidadCombustiblesModel model = new DisponibilidadCombustiblesModel();
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
            FiltroDisponibilidadCombustibleModel model = new FiltroDisponibilidadCombustibleModel();
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
            DisponibilidadCombustiblesModel model = new DisponibilidadCombustiblesModel();
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