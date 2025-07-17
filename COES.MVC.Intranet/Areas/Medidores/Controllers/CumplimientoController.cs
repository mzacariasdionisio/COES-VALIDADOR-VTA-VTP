using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class CumplimientoController : BaseController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        GestionAdministradorAppServicio servGestionAdmin = new GestionAdministradorAppServicio();
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
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
            model.MesInicio = new DateTime(DateTime.Now.Year, 1, 1).ToString("MM yyyy");
            model.MesFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));
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
        public PartialViewResult Lista(string sEmpresas, int idFormato, string mesIni, string mesFin)
        {
            var formato = servFormato.GetByIdMeFormato(idFormato);

            DateTime fechaProcesoIni = EPDate.GetFechaIniPeriodo(5, mesIni, "", "", "");
            DateTime fechaProcesoFin = EPDate.GetFechaIniPeriodo(5, mesFin, "", "", "");
            DateTime fechaIni = fechaProcesoIni;
            DateTime fechaFin = fechaProcesoFin.AddMonths(1).AddDays(-1);

            GestionAdministradorModel model = new GestionAdministradorModel();
            model.Resultado = this.servGestionAdmin.GeneraViewCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, (int)formato.Formatperiodo);
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
        public JsonResult GenerarArchivoReporteCumplimiento(string sEmpresas, int idFormato, string mesIni, string mesFin)
        {
            int indicador = 1;
            var formato = servFormato.GetByIdMeFormato(idFormato);
            
            DateTime fechaProcesoIni = EPDate.GetFechaIniPeriodo(5, mesIni, "", "", "");
            DateTime fechaProcesoFin = EPDate.GetFechaIniPeriodo(5, mesFin, "", "", "");
            DateTime fechaIni = fechaProcesoIni;
            DateTime fechaFin = fechaProcesoFin.AddMonths(1).AddDays(-1);

            GestionAdministradorModel model = new GestionAdministradorModel();
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;


            try
            {
                this.servGestionAdmin.GeneraExcelCumplimiento(sEmpresas, fechaIni, fechaFin, idFormato, formato.Formatnombre, (int)formato.Formatperiodo, ruta + ConstantesIntranet.NombreArchivoCumplimiento,
                    ruta + Constantes.NombreLogoCoes);
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
            nombreArchivo = ConstantesIntranet.NombreArchivoCumplimiento;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntranet.FolderReporte;
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
