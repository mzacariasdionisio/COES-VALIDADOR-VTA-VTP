using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class EnvioController : FormatoController
    {
        //
        // GET: /IEOD/Envio/
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "ReporteInformesController";
        private static List<EstadoModel> ListaEstadoSistemaA = new List<EstadoModel>();

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

        public EnvioController()
        {
            ListaEstadoSistemaA = new List<EstadoModel>();
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "0", EstadoDescripcion = "NO" });
            ListaEstadoSistemaA.Add(new EstadoModel() { EstadoCodigo = "1", EstadoDescripcion = "SÍ" });
        }




        public ActionResult Index()
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaEstadoEnvio = servFormato.ListMeEstadoenvios();
            GenerarGestionAdministradorModel(model);

            return View(model);
        }

        /// <summary>
        /// Inicializar model
        /// </summary>
        /// <param name="model"></param>
        private void GenerarGestionAdministradorModel(GestionAdministradorModel model)
        {
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesHard.IdFormatoFlujoTrans);
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoFlujoTrans));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoDespacho));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoConsumo));
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idsEmpresa, string fInicio, string fFin, string idsFormato, string idsEstado)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.IndicadorPagina = false;
            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            int nroRegistros = servFormato.TotalListaMultipleMeEnvios(idsEmpresa, string.Empty, idsFormato, idsEstado, fechaIni, fechaFin);

            if (nroRegistros > 0)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de envío
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsEstado"></param>
        /// <param name="nPaginas"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string idsEmpresa, string fInicio, string fFin, string idsFormato, string idsEstado, int nPaginas)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            var lista = this.servFormato.GetListaMultipleMeEnvios(idsEmpresa, string.Empty, idsFormato, idsEstado, fechaIni, fechaFin, nPaginas, Constantes.PageSize);
            model.ListaEnvio = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene las empresas segun formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idFormato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }

        /// <summary>
        /// exporta el reporte general consultado a archivo excel
        /// </summary>
        /// <param name="idsEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="idsFormato"></param>
        /// <param name="idsLectura"></param>
        /// <param name="idsEstado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteXLS(string idsEmpresa, string fInicio, string fFin, string idsFormato, string idsLectura, string idsEstado)
        {
            int indicador = 1;

            GestionAdministradorModel model = new GestionAdministradorModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnvio.FolderReporte;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            try
            {
                this.servicio.GeneraExcelEnvio(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, ruta + ConstantesEnvio.NombreArchivoEnvio, pathLogo);
                indicador = 1;

            }
            catch (Exception ex)
            {
                indicador = -1;
                Log.Error(NameController, ex);
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
            nombreArchivo = ConstantesEnvio.NombreArchivoEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnvio.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        /// <summary>
        /// Permite generar el formato en un archivo Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(int idEnvio, int idFormato)
        {
            string ruta = string.Empty;
            try
            {
                MeEnvioDTO envio = base.servFormato.GetByIdMeEnvio(idEnvio);
                string mes = new DateTime(envio.Enviofechaperiodo.Value.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy"); ;
                FormatoModel model = BuildHojaExcelDetalleEnvio(envio.Emprcodi.Value, idEnvio, string.Empty, mes, idFormato);
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web del detalle del envio
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelDetalleEnvio(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato)
        {
            FormatoModel model = new FormatoModel();
            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, ConstantesFormato.NoVerUltimoEnvio);
            return model;
        }

    }
}
