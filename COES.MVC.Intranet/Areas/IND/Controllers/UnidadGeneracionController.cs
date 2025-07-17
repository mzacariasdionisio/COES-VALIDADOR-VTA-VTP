using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class UnidadGeneracionController : BaseController
    {
        private readonly INDAppServicio _indAppServicio;

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

        public UnidadGeneracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Operación comercial

        /// <summary>
        /// Muestra la vista principal de Operación Comercial
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexOperacionComercial(int? pericodi)
        {
            var model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();

                if (pericodi.GetValueOrDefault(0) <= 0)
                {
                    var listaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);
                    var regpertmp = listaPeriodo.Find(x => x.FechaIni == fechaPeriodo);
                    pericodi = regpertmp.Ipericodi;
                }

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

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
        /// Carga Listado de Opetación Comercial en Formato JSON
        /// </summary>
        /// <param name="pericodi">Código de periodo</param>
        /// <param name="famcodi">Código de Familia</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaOperacionComercial(int pericodi, int tipo)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                IndPeriodoDTO regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi);
                DateTime fechaIni = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                model.Resultado = _indAppServicio.GenerarReporteHtmlOperacionComercial(fechaIni, fechaFin, tipo);
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
        /// Descargar Listado de Opetación Comercial en Formato JSON
        /// </summary>
        /// <param name="pericodi">Código de periodo</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarListaOperacionComercial(int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (pericodi > 0)
                {
                    _indAppServicio.GenerarArchivoExcelOP(ruta, pericodi, out string nameFile);
                    model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "Error de Exportación.";
                    model.Resultado = "-1";
                }
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

        #region Potencia Efectiva

        /// <summary>
        /// Muestra la vista principal de Potencia Efectiva
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPotenciaEfectiva(int? pericodi)
        {
            var model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();

                if (pericodi.GetValueOrDefault(0) <= 0)
                {
                    var listaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(fechaPeriodo.Year);
                    var regpertmp = listaPeriodo.Find(x => x.FechaIni == fechaPeriodo);
                    pericodi = regpertmp.Ipericodi;
                }

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

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
        /// Carga Lista de Potencia Efectiva  en Formato JSON
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaPotenciaEfectiva(int pericodi, int tipo)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                IndPeriodoDTO regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi);
                DateTime fechaIni = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                int aplicativo = ConstantesIndisponibilidades.AppPR25;
                model.Resultado = _indAppServicio.GenerarReporteHtmlPotenciaEfectiva(aplicativo, fechaIni, fechaFin, tipo);

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
        /// Carga Lista de Potencia Efectiva  en Formato JSON
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarListaPotenciaEfectiva(int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            base.ValidarSesionJsonResult();

            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            if (pericodi > 0)
            {
                int aplicativo = ConstantesIndisponibilidades.AppPR25;
                _indAppServicio.GenerarArchivoExcelPotenciaEfectiva(ruta, pericodi, aplicativo, out string nameFile);
                model.Resultado = nameFile;
            }
            else
            {
                model.Mensaje = "Error de Exportación.";
                model.Resultado = "-1";
            }

            return Json(model);
        }

        #endregion

        /// <summary>
        /// Retorna Listado de Periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(anio);
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
        /// Permite exportar archivos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["file_name"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

            byte[] buffer = FileServer.DownloadToArrayByte(nombreArchivo, ruta);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

    }
}