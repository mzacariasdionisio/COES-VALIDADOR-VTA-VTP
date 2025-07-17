using System;
using log4net;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic.PowerBI;
using COES.Servicios.Aplicacion.PrimasRER;

using COES.Servicios.Aplicacion.General;
using System.Collections.Generic;
using System.Threading.Tasks;
using COES.MVC.Intranet.Areas.PrimasRER.Models;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.PrimasRER.Controllers
{
    public class ReportesBIController : Controller
    {
        // GET: /PrimasRER/CentralRER/

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();
        private readonly PrimasRERAppServicio primasRERAppServicio = new PrimasRERAppServicio();

        public ReportesBIController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Muestra el Reporte Bi Energía valorizada mensual
        /// </summary>
        /// <returns></returns>
        public ActionResult EnergiaValorizada()
        {
            PrimasRERBI model = new PrimasRERBI
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:energia_valorizada_id"].ToString(),
                sName = "Energía Valorizada"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Saldos, rentas de congestión e ingreso tarifario
        /// </summary>
        /// <returns></returns>
        public ActionResult SaldosRentasIngresoTarifario()
        {
            PrimasRERBI model = new PrimasRERBI
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:saldos_rentas_ingreso_tarifario_id"].ToString(),
                sName = "Saldos Rentas Ingreso Tarifario"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Ingreso tarifario en energía (MWh)
        /// </summary>
        /// <returns></returns>
        public ActionResult IngresoTarifario()
        {
            PrimasRERBI model = new PrimasRERBI 
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:ingreso_tarifario_id"].ToString(),
                sName = "Ingreso Tarifario"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Ingresos por Peaje por Conexión y Transmisión
        /// </summary>
        /// <returns></returns>
        public ActionResult IngresosXPeaje()
        {
            PrimasRERBI model = new PrimasRERBI 
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:ingreso_peaje_id"].ToString(),
                sName = "Ingreso por Peaje"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Ingresos Tarifarios a Transmisoras
        /// </summary>
        /// <returns></returns>
        public ActionResult IngresosTarifariosTransmisoras()
        {
            PrimasRERBI model = new PrimasRERBI
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:ingreso_tarifario_transmisoras_id"].ToString(),
                sName = "Ingresos Tarifarios Transmisoras"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Información ingresada para VTP
        /// </summary>
        /// <returns></returns>
        public ActionResult InformIngresadaVTP()
        {
            PrimasRERBI model = new PrimasRERBI 
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:inform_ingresada_id"].ToString(),
                sName = "Informe Ingresada VTP"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Egreso de Potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult EgresoPotencia()
        {
            PrimasRERBI model = new PrimasRERBI 
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:egreso_potencia_id"].ToString(),
                sName = "Egreso de Potencia"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Ingresos por Potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult IngresosPotencia()
        {
            PrimasRERBI model = new PrimasRERBI 
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:ingresos_potencia_id"].ToString(),
                sName = "Ingresos de Potencia"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Ingreso – Egresos por Potencia Mensual - Saldos
        /// </summary>
        /// <returns></returns>
        public ActionResult EgresosPotenciaMensual()
        {
            PrimasRERBI model = new PrimasRERBI {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:egreso_potencia_mensual_id"].ToString(),
                sName = "Egresos de Potencia Mensual"
            };
            return View(model);
        }

        /// <summary>
        /// Muestra el Reporte Bi Potencia Firme y Potencia Firme Remunerable
        /// </summary>
        /// <returns></returns>
        public ActionResult PotenciaFirmeRemunerable()
        {
            PrimasRERBI model = new PrimasRERBI
            {
                sId = ConfigurationManager.AppSettings["powerbi:primarer:potencia_firme_remunerable_id"].ToString(),
                sName = "Potencia Firme Remunerable"
            };
            return View(model);
        }

        #region Power BI

        /// <summary>
        /// Carga la pantalla inicial de los reportes de Power BI
        /// </summary>
        /// <returns></returns>
        public ActionResult VisorPowerBI()
        {
            return View();
        }

        /// <summary>
        /// Datos para la pantalla de Reportes de Power BI
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ObtenerReportes(string id, string name)
        {
            PrimasRERResultado resultado = new PrimasRERResultado();

            try
            {
                resultado.list = Task.Run(async () => await servicio.ObtenerReportesParaIntranet()).GetAwaiter().GetResult();
                resultado.list = resultado.list.FindAll(s => s.Id == id);
                if (resultado.list.Count == 1)
                {
                    resultado.iResultado = 1;
                    resultado.sMensaje = "Reporte encontrado";
                }
                else
                {
                    resultado.iResultado = -1;
                    resultado.sMensaje = "No se encontro el reporte solicitado:" + name;
                }
                return Json(resultado);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                resultado.iResultado = -1;
                resultado.sMensaje = ex.Message;
                return Json(resultado);
            }
        }

        #endregion


    }
}