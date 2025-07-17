using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.ConsumoCombustible.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.ConsumoCombustible;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ConsumoCombustible.Controllers
{
    public class VersionController : BaseController
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

        public VersionController()
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

            string strFecha = fechaConsulta == null ? DateTime.Today.AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha) : fechaConsulta.Replace("-", "/");
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
                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                int verscodi = this.servComb.ProcesarCalculoConsumoCombustible(fechaPeriodo, User.Identity.Name, false);

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

                DateTime fechaPeriodo = DateTime.ParseExact(strFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                string url = Url.Content("~/");
                model.Resultado = this.servComb.GenerarTablaHtmlVersionDiario(fechaPeriodo, url);
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

        #region Reporte de Desviación de Consumo de Combustible

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
                model.ListaLeyenda = servComb.ListarLeyenda(regVersion.Cccverfecha);

                servComb.ListaDataXVersionReporte(vercodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                , out List<CccReporteDTO> listaReptotOut
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqEquipoDTO> listaCentral
                                                , out List<EqEquipoDTO> listaEquipo);

                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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
        /// Cargar los filtros comunes para todos los cuadros
        /// </summary>
        /// <param name="irptcodi"></param>
        /// <param name="centralIntegrante"></param>
        /// <param name="empresa"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ViewCargarFiltros(int vercodi, string empresa)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                servComb.ListaDataXVersionReporte(vercodi, empresa, ConstantesAppServicio.ParametroDefecto
                                                , out List<CccReporteDTO> listaReptotOut
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqEquipoDTO> listaCentral
                                                , out List<EqEquipoDTO> listaEquipo);

                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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
        public JsonResult ViewReporteWeb(int vercodi, string empresa, string central)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = servComb.GenerarTablaHtmlListadoReporte(vercodi, empresa, central);
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
        public JsonResult GenerarArchivoExcelReporte(int vercodi, string empresa, string central)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                servComb.GenerarRptExcel(ruta, vercodi, empresa, central, out string nameFile);

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

        #endregion

        #region Grafico

        public ActionResult Grafico(int vercodi)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                CccVersionDTO regVersion = servComb.GetByIdCccVersion(vercodi);

                model.Version = regVersion;
                model.IdVersion = vercodi;
                model.FechaPeriodo = regVersion.CccverfechaDesc;

                servComb.ListaDataXVersionReporte(vercodi, ConstantesAppServicio.ParametroDefecto, ConstantesAppServicio.ParametroDefecto
                                                , out List<CccReporteDTO> listaReptotOut
                                                , out List<SiEmpresaDTO> listaEmpresa
                                                , out List<EqEquipoDTO> listaCentral
                                                , out List<EqEquipoDTO> listaEquipo);

                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
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

        [HttpPost]
        public JsonResult ListaGrafico(int vercodi, int empresa, int central)
        {
            ConsumoCombustibleModel model = new ConsumoCombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaGrafico = new List<GraficoWeb>();
                if (empresa > 0 && central > 0)
                    model.ListaGrafico = servComb.ListarGraficoXUnidad(vercodi, empresa, central);
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

        #endregion
    }
}
