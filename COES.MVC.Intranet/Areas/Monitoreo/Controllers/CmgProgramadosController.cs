using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.App_Start;
using COES.MVC.Intranet.Areas.Monitoreo.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Monitoreo;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Monitoreo.Controllers
{
    [ValidarSesion]
    public class CmgProgramadosController : BaseController
    {
        MonitoreoAppServicio servMonitoreo = new MonitoreoAppServicio();
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ParametroController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        /// <summary>
        /// Inicio INDEX monitoreo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            CmgProgramadosModel model = new CmgProgramadosModel();
            try
            {
                int numDia;
                string strFechaIni;
                this.servMonitoreo.GetFechaMaxGeneracionPermitida(out strFechaIni, out numDia);
                model.DiaMes = numDia;
                model.FechaInicio = strFechaIni;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Reporte Log de errores Indicadores
        /// </summary>
        /// <param name="tipoIndicador"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="nroPagina"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult CargarCostosMarginalesProg(string fechaMes)
        {
            CmgProgramadosModel model = new CmgProgramadosModel();
            try
            {
                this.ValidarSesionJsonResult();
                //Obtien fecha inicio y fin
                DateTime fechaIni = new DateTime(Int32.Parse(fechaMes.Substring(3, 4)), Int32.Parse(fechaMes.Substring(0, 2)), 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);
                servicio.CargarCostosMarginalesProgramadosYupana(fechaIni, fechaFin, base.UserName);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Paginar indicador 4
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <returns></returns>
        public PartialViewResult ListaMensual(string fechaMes)
        {
            CmgProgramadosModel model = new CmgProgramadosModel();
            try
            {
                DateTime fechaIni = new DateTime(Int32.Parse(fechaMes.Substring(3, 4)), Int32.Parse(fechaMes.Substring(0, 2)), 1);
                model.ListaCmg = servicio.ListaTotalXDia(fechaIni);

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ex.Message;
            }

            return PartialView(model);
        }

    }
}