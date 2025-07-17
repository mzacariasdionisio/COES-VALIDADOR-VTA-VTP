using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
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
    public class CumplimientoController : BaseController
    {
        //
        // GET: /IEOD/Cumplimiento/

        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "CumplimientoController";

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
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesHard.IdFormatoFlujoTrans);
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoFlujoTrans));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoDespacho));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoConsumo));
        }

        /// <summary>
        /// Devuelve vista parcial para mostrar listado de cumplimiento
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="idFormato"></param>
        /// <param name="fIni"></param>
        /// <param name="fFin"></param>
        /// <param name="mes1"></param>
        /// <param name="mes2"></param>
        /// <param name="semana1"></param>
        /// <param name="semana2"></param>
        /// <returns></returns>
        public PartialViewResult Lista(string sEmpresas, int idFormato, string fInicio, string fFin)
        {
            var formato = servFormato.GetByIdMeFormato(idFormato);

            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            GestionAdministradorModel model = new GestionAdministradorModel();
            model.Resultado = this.servicio.GeneraViewCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, (int)formato.Formatperiodo);
            model.NombreFortmato = formato.Formatnombre;
            return PartialView(model);
        }

        /// <summary>
        /// exporta el reporte general consultado a archivo excel
        /// </summary>
        /// <param name="sEmpresas"></param>
        /// <param name="idFormato"></param>
        /// <param name="mesIni"></param>
        /// <param name="mesFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporteCumplimiento(string sEmpresas, int idFormato, string fInicio, string fFin)
        {
            int indicador = 1;
            var formato = servFormato.GetByIdMeFormato(idFormato);

            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            GestionAdministradorModel model = new GestionAdministradorModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnvio.FolderReporte;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;


            try
            {
                this.servicio.GeneraExcelCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, formato.Formatnombre, (int)formato.Formatperiodo, ruta + ConstantesEnvio.NombreArchivoCumplimiento, pathLogo);
                indicador = 1;

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
            nombreArchivo = ConstantesEnvio.NombreArchivoCumplimiento;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnvio.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
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

    }
}
