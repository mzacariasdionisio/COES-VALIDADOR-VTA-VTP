using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class CargaDatosController : BaseController
    {
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();

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

        public CargaDatosController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult IndexReporteCaudal()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteModel model = new ReporteModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //si existe un arbol en ejecucion
            CpArbolContinuoDTO objUltimoArbol = yupanaServicio.GetUltimoArbol();
            bool existeArbolEnEjec = objUltimoArbol != null;

            DateTime fechaActual = DateTime.Now;
            if (existeArbolEnEjec) fechaActual = objUltimoArbol.Cparbfecha.AddHours(objUltimoArbol.Cparbbloquehorario);
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaHora = yupanaServicio.ListarHoras(fechaActual.Hour);

            model.Tyupcodi = ConstantesYupanaContinuo.TipoConfiguracionCaudal;

            return View(model);
        }

        public ActionResult IndexReporteRER()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ReporteModel model = new ReporteModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //si existe un arbol en ejecucion
            CpArbolContinuoDTO objUltimoArbol = yupanaServicio.GetUltimoArbol();
            bool existeArbolEnEjec = objUltimoArbol != null;

            DateTime fechaActual = DateTime.Now;
            if (existeArbolEnEjec) fechaActual = objUltimoArbol.Cparbfecha.AddHours(objUltimoArbol.Cparbbloquehorario);
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaHora = yupanaServicio.ListarHoras(fechaActual.Hour);

            model.Tyupcodi = ConstantesYupanaContinuo.TipoConfiguracionRer;

            return View(model);
        }

        [HttpPost]
        public JsonResult CargarFiltroYHandsonXDiaHora(string fecha, int hora, int tyupcodi, int cyupcodi)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                //obtener la configuracion más reciente de la hora seleccionada
                CpYupconCfgDTO objUltimoEnvio = yupanaServicio.GetUltimaConfiguracionXTipo(tyupcodi, fechaActual, false);
                if (objUltimoEnvio != null) model.Yupcfgcodi = objUltimoEnvio.Yupcfgcodi;

                //obtener el detalle de la media hora seleccionada
                List<CpYupconM48DTO> listaDet48 = yupanaServicio.ListarDetalleCargaConfiguracion(tyupcodi, cyupcodi, fechaActual);
                if (listaDet48.Any())
                {
                    model.Handson = yupanaServicio.ArmarHandsonDetalleCargaConfiguracion(tyupcodi, fechaActual, listaDet48);
                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GrabarHandsonDetalleCargaConfiguracion(string fecha, int hora, int tyupcodi, string stringJson)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                List<CpYupconM48DTO> listaDetalle = yupanaServicio.ListarDetalleCargaConfiguracionFromHandson(stringJson);

                //Guardar información importada                
                int envio = yupanaServicio.GuardarDetalleCargaConfiguracion(base.UserName, fechaActual, tyupcodi, listaDetalle);
                model.CodigoEnvio = envio;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult CargarHandsonFromExtranetXDiaHora(string fecha, int hora, int tyupcodi, string stringJson)
        {
            ConfiguracionModel model = new ConfiguracionModel();

            try
            {
                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                //obtener dato en pantalla
                List<CpYupconM48DTO> listaWeb = yupanaServicio.ListarDetalleCargaConfiguracionFromHandson(stringJson);

                //obtener el detalle de la media hora seleccionada
                List<CpYupconM48DTO> listaDet48 = yupanaServicio.ListarHandsonDetalleCargaConfiguracion(tyupcodi, fechaActual, listaWeb);
                if (listaDet48.Any())
                {
                    model.Handson = yupanaServicio.ArmarHandsonDetalleCargaConfiguracion(tyupcodi, fechaActual, listaDet48);
                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ListadoHistorial(string fecha, int hora, int tyupcodi)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                model.ListaEnvio = yupanaServicio.GetByCriteriaCpYupconEnvios(tyupcodi, fechaActual, hora);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ConfiguracionYReporteAutomatizado(string fecha, int hora, int tyupcodi)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                yupanaServicio.ActualizarAutomaticoConfyDetalleM48YupanaContinuo(tyupcodi, fechaActual, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult DataReporteAutomatizado(string fecha, int hora, int tyupcodi)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                //Verificar que existencia de escenario Yupana
                int topcodi = yupanaServicio.GetTopologiaByDate(fechaActual);
                if (topcodi == 0)
                {
                    throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
                }

                yupanaServicio.ActualizarAutomaticoDetalleM48YupanaContinuo(base.UserName, topcodi, fechaActual, tyupcodi);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="volcalcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarVersionCalculo(string fecha, int hora, int tyupcodi, int cyupcodi)
        {
            ReporteModel model = new ReporteModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                yupanaServicio.GenerarArchivoExcelDetalleM48YupanaContinuo(ruta, pathLogo, tyupcodi, cyupcodi, fechaActual, out string nombreArchivo);

                model.Resultado = nombreArchivo;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Devuelve un archivo excel exportado
        /// </summary>
        /// <returns></returns>
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }
    }
}