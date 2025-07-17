using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
//IND.PR25.2022
//using COES.MVC.Intranet.Helper;
//using System.Globalization;
//

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class CombustibleController : BaseController
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

        public CombustibleController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult IndexCombustible(int? pericodi)
        {
            var model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaEmpresa = _indAppServicio.ListarEmpresaRptCumplimientoCombustible();
                model.ListaRecalculo = new List<IndRecalculoDTO>();
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
        public JsonResult CargarReporteHtml(int pericodi, string empresa)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                IndPeriodoDTO regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi);
                DateTime fechaIni = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                model.Resultado = _indAppServicio.GenerarHtmlListadoReporteCumplimiento(fechaIni, fechaFin, empresa);
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
        public JsonResult GenerarArchivoExcelCumplimiento(int pericodi, string empresa)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                IndPeriodoDTO regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi);
                DateTime fechaIni = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                DateTime fechaFin = fechaIni.AddMonths(1).AddDays(-1);

                _indAppServicio.GenerarExcelListadoReporteCumplimiento(fechaIni, fechaFin, empresa, ruta, out string nameFile);

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

        #region IND.PR25.2022
        /// <summary>
        /// Importar stock de combustible de la Extranet hacia la Intranet.
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <param name="empresa"></param>
        /// <param name="historico"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportarStock(int ipericodi, string fechainicio, string fechafin, string empresa, bool historico)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ImportarStockCombustible(ipericodi, fechainicio, fechafin, empresa, historico, User.Identity.Name, out string mensaje);
                model.Mensaje = mensaje;
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
        /// Actualizar un stock de Combustible Detalle para un registro de tipo Modificado, y registrar su historial respectivo.
        /// </summary>
        /// <param name="stkdetcodiModi"></param>
        /// <param name="dia"></param>
        /// <param name="stockOrig"></param>
        /// <param name="stockModi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarStock(int stkdetcodiModi, int dia, string stockOrig, decimal stockModi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ActualizarStkCombustibleDetalle(stkdetcodiModi, dia, stockOrig, stockModi, User.Identity.Name, out string mensaje);
                model.Mensaje = mensaje;
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
        /// Leer el stock de combustible de la Intranet para un periodo y empresas específicas.
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarHtmlStockCombustibleDetalle(int ipericodi, string emprcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.GenerarHtmlStockCombustibleDetalle(ipericodi, emprcodi, out int NumeroDias, out HandsonModel handson, out string historial, out string mensaje);
                model.NumeroDias = NumeroDias;
                model.HandsonStkCmtDet = handson;
                model.RptHtmlCambios = historial;
                model.Mensaje = mensaje;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                model.NumeroDias = 0;
                model.HandsonStkCmtDet = new HandsonModel();
                model.RptHtmlCambios = "";
            }

            return Json(model);
        }

        #endregion
    }
}