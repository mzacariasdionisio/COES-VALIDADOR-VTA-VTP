using COES.Base.Core;
using COES.Dominio.DTO.Scada;
using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Medidores;
using log4net;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class EnvioController : FormatoController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        GestionAdministradorAppServicio servGestionAdmin = new GestionAdministradorAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "EnvioController";

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

        public ActionResult Index()
        {
            GestionAdministradorModel model = new GestionAdministradorModel();

            GenerarGestionAdministradorModel(model);
            return View(model);
        }

        /// <summary>
        /// Inicializar model
        /// </summary>
        /// <param name="model"></param>
        private void GenerarGestionAdministradorModel(GestionAdministradorModel model)
        {
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEstadoEnvio = servFormato.ListMeEstadoenvios();
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string idsEmpresa, string mes, string idsFormato, string idsEstado)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.IndicadorPagina = false;
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

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
        public PartialViewResult Lista(string idsEmpresa, string mes, string idsFormato, string idsEstado, int nPaginas)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

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
        public JsonResult GenerarArchivoReporteXLS(string idsEmpresa, string mes, string idsFormato, string idsLectura, string idsEstado)
        {
            int indicador = 1;

            GestionAdministradorModel model = new GestionAdministradorModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);

            try
            {
                this.servGestionAdmin.GeneraExcelEnvio(idsEmpresa, idsLectura, idsFormato, idsEstado, fechaIni, fechaFin, ruta + ConstantesIntranet.NombreArchivoEnvio,
                    ruta + Constantes.NombreLogoCoes);
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
            nombreArchivo = ConstantesIntranet.NombreArchivoEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
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
                string mes = new DateTime(envio.Enviofechaperiodo.Value.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
                FormatoModel model = BuildHojaExcelMedidorGeneracion(envio.Emprcodi.Value, idEnvio, string.Empty, mes, idFormato, ConstantesFormato.NoVerUltimoEnvio);
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
        /// Metodo llamado desde cliente web para consultar el formato excel web de Medidor de Generación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelMedidorGeneracion(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato, int verUltimoEnvio)
        {
            MedidorGeneracionModel model = new MedidorGeneracionModel();
                BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            return model;
        }
    }
}
