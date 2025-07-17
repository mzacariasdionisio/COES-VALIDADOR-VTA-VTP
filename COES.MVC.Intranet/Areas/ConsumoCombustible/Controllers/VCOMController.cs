using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.ConsumoCombustible.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.ConsumoCombustible;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.ConsumoCombustible.Controllers
{
    public class VCOMController : BaseController
    {
        private readonly ConsumoCombustibleAppServicio servComb = new ConsumoCombustibleAppServicio();

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

        public VCOMController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Procesar Versión

        public ActionResult Index(string fechaConsulta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            string strFecha = fechaConsulta == null ? DateTime.Today.AddMonths(-1).ToString(ConstantesAppServicio.FormatoMes) : fechaConsulta.Replace("-", " ");
            model.FechaPeriodo = strFecha;

            return View(model);
        }

        /// <summary>
        /// generar versión
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarVersion(string strFecha)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                int verscodi = this.servComb.ProcesarCalculoVCOM(fechaPeriodo, User.Identity.Name);

                model.Resultado = verscodi.ToString();
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
        /// generar versión
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarReporteDiario(string strFecha)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);
                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                model.Resultado = this.servComb.EjecutarReporteDiarioXPeriodo(fechaPeriodo, User.Identity.Name);
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
        public JsonResult GuardarRepVcom(int vercodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                this.servComb.EjecutarGuardarEnRepVcom(vercodi, User.Identity.Name);
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
        /// Listar versiones
        /// </summary>
        /// <param name="strFecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VersionListado(string strFecha)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");
                model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.Resultado = this.servComb.GenerarTablaHtmlListadoVersionVCOM(fechaPeriodo, url, model.TienePermiso);
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

        #endregion

        #region Reporte VCOM

        public ActionResult Reporte(int vercodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                CccVersionDTO regVersion = servComb.GetByIdCccVersion(vercodi);

                model.Version = regVersion;
                model.IdVersion = vercodi;
                model.FechaPeriodo = regVersion.CccverfechaDesc;

                string fileName = servComb.GetNombreArchivoLogCambio(vercodi);
                try
                {
                    byte[] buffer = servComb.GetBufferArchivoEnvioFinal(fileName);
                    model.TieneLogCambio = buffer != null;
                }
                catch (Exception ex) { }
                model.NombreArchivoLogCambio = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Reporte web
        /// </summary>
        /// <param name="vercodi"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ViewReporteWeb(int vercodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                List<CccVcomDTO> listaRept = servComb.ListarReporteVCOMxFiltro(vercodi, "-1", "-1", out CccVersionDTO regVersion);

                model.Version = regVersion;
                model.ListaDetalleVCOM = listaRept;
                model.ListaModoOp = servComb.ListarModoOperacionSEIN();
                model.ListaFuenteEnergia = servComb.GetByCriteriaSiFuenteenergias();

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
        /// Guardar listado de cambios
        /// </summary>
        /// <param name="smodel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarListaCambioVersion(string smodel)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ConsumoCombustibleModel modelInput = serializer.Deserialize<ConsumoCombustibleModel>(smodel);

                servComb.GuardarVersionConCambio(modelInput.IdVersion, modelInput.ListaCambio, base.UserName);

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
        /// Generar archivo excel del cuadro
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <param name="centralIntegrante"></param>
        /// <param name="empresa"></param>
        /// <param name="central"></param>
        /// <param name="famcodi"></param>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelReporte(int vercodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servComb.GenerarRptExcelVCOM(ruta, vercodi, "-1", "-1", out string nameFile);

                model.Resultado = nameFile;
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

        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
            string fullPath = ruta + nombreArchivo;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        /// <summary>
        /// Descargar archivo log
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoLog(string fileName)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servComb.GetBufferArchivoEnvioFinal(fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

    }
}
