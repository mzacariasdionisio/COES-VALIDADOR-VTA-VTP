using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ReprocesoController : BaseController
    {
        private readonly ReprocesoAppServicio servicio = new ReprocesoAppServicio();


        #region Declaración de variables

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

        public ReprocesoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Cálculo del ángulo óptimo

        public ActionResult AnguloOptimo()
        {
            ReprocesoModel model = new ReprocesoModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaLinea = this.servicio.ObtenerLineas();
            model.FechaVigenciaPR07 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07];
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Periodos(string fecha, int version)
        {
            ReprocesoModel model = new ReprocesoModel();
            model.ListaProceso = this.servicio.ObtenerPeriodosCMPorVersion(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture), version);           
            return PartialView(model);
        }

        /// <summary>
        /// Permite obtener el listado de parámetros
        /// </summary>
        /// <param name="linea"></param>
        /// <param name="horas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerParametrosAngulo(string fecha, string linea, string horas) 
        {
            return Json(this.servicio.ObtenerParametrosAngulo(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture), horas, linea));
        }

        /// <summary>
        /// Permite calcular los ángulos óptimos para los periodos seleccionados
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CalcularAnguloOptimo(string[][] data)
        {
            return Json(this.servicio.CalcularAnguloOptimo(data));
        }

        /// <summary>
        /// Permite reprocesar los costos marginales por calculo de angulo optimo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="linea"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarPorAnguloOptimo(string fecha, string linea, string[][] data, int version)
        {
            DateTime inFechaOperar = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            ParametrosAnguloOptimo result = this.servicio.ReprocesarPorAnguloOptimo(inFechaOperar, linea, data, version);
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();

            try
            {
                if (result.Resultado == 1)
                {
                    string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaReprocesoCostosMarginalesTNA];
                    path = String.Format(path + "{0:0000}\\{1:00}\\{2:00}\\", inFechaOperar.Year, inFechaOperar.Month, inFechaOperar.Day);
                    foreach (DatosReproceso item in result.ListaProceso)
                    {
                        string fileName = NombreArchivo.ArchivoRawCM;

                        if (FileServerScada.VerificarExistenciaFile("Reproceso", fileName, path))
                        {
                            FileServerScada.DeleteBlob(@"Reproceso\" + fileName, path);
                        }

                        FileServerScada.UploadFromStream(item.FilePsse, @"Reproceso\", fileName, path);

                        ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                        cliente.EjecutarCostosMarginalesAlterno(item.FechaProceso, 1, true, false, false, string.Empty, true, 0, base.UserName,ConstantesCortoPlazo.EstimadorTNA, 2, version);
                    }
                }

                entity.Resultado = 1;
            }
            catch(Exception ex)
            {
                entity.Resultado = -1;
            }

            return Json(entity);
        }

        #endregion

        #region Transacciones internacionales

        /// <summary>
        /// Carga de pagina de recalculo por TIE
        /// </summary>
        /// <returns></returns>
        public ActionResult TransaccionInternacional()
        {
            ReprocesoModel model = new ReprocesoModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaBarra = this.servicio.ObtenerBarras();
            model.FechaVigenciaPR07 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07];
            return View(model);
        }

        /// <summary>
        /// Lista de transacciones
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult OperacionesVarias(string fecha)
        {
            ReprocesoModel model = new ReprocesoModel();
            model.ListaOperaciones = this.servicio.ObtenerTransacciones(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture));
            return PartialView(model);
        }

        /// <summary>
        /// Calculo de la enegía importada
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="horas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CalcularEnergiaImportada(string fecha, string horas, int barra, int version)
        {
            return Json(this.servicio.CalcularEnergiaImportada(DateTime.ParseExact(fecha, Constantes.FormatoFecha,
               CultureInfo.InvariantCulture), horas, barra, version));
        }

        /// <summary>
        /// Permite reprocesar los costos marginales por calculo de angulo optimo
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="linea"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarPorTransaccionInternacional(string fecha, string[][] data, int barra, int version)
        {
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha,
                CultureInfo.InvariantCulture);
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();
            try
            {
                ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                cliente.EjecutarReprocesoTIE(data, base.UserName, barra, fechaProceso, version);
                entity.Resultado = 1;
                entity.Cantidad = data.Length;
                
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
            }

            return Json(entity);
        }

        #endregion

        #region Ticket_6245
        public ActionResult ModificacionVA()
        {
            ReprocesoModel model = new ReprocesoModel();
            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);          
            model.FechaVigenciaPR07 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07];
            return View(model);
        }

        /// <summary>
        /// Permite ejecutar el reproceso por va
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="horas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarVA(string fecha, string horas, int version)
        {
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha,
               CultureInfo.InvariantCulture);
            ParametrosAnguloOptimo entity = new ParametrosAnguloOptimo();
            try
            {
                ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                cliente.EjecutarReprocesoVA(horas, base.UserName, fechaProceso, version);
                entity.Resultado = 1;
            }
            catch (Exception ex)
            {
                entity.Resultado = -1;
            }

            return Json(entity);
        }

        #endregion
    }
}