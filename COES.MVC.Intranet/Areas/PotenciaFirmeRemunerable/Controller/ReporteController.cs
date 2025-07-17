using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Controller
{
    public class ReporteController : BaseController
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReporteController));
        private static readonly string NameController = "ReporteController";


        private readonly PotenciaFirmeRemunerableAppServicio _pfrService;
        public ReporteController()
        {
            _pfrService = new PotenciaFirmeRemunerableAppServicio();
        }

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
        /// </summary>
        /// <param name="filterContext"></param>
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

        #region REPORTE POTENCIA FIRME REMUNERABLE

        /// <summary>
        /// Página Principal de Cálculo de Potencia Firme Remunerable
        /// </summary>
        /// <returns></returns>      
        public ActionResult IndexPotenciaFirmeRemunerable(int? pericodi, int? recacodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual(); // primer dia del mes anterior
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                PfrPeriodoDTO regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<PfrRecalculoDTO>();
                if (model.ListaPeriodo.Count > 0)
                {
                    model.ListaRecalculo = _pfrService.GetByCriteriaPfrRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrreccodi : recacodi.GetValueOrDefault(0);
                    model.IdReporte = _pfrService.GetUltimoPfrrptcodiXRecalculo(recacodi.Value, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);
                }

                model.HandsonModel = _pfrService.GenerarHandsonPFR(model.IdReporte);

                model.Resultado = "1";
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
        /// Listado de Potencia Firme por filtros
        /// </summary>
        /// <param name="recacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarLstPotenciaFirme(int recacodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);


                int pfrrptcodi = _pfrService.GetUltimoPfrrptcodiXRecalculo(recacodi, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);

                model.HandsonModel = _pfrService.GenerarHandsonPFR(pfrrptcodi);
                model.TieneRegistroPrevio = model.HandsonModel.ListaExcelData2.Count() > 2;
                model.IdRecalculo = recacodi;
                model.IdReporte = pfrrptcodi;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult cargarLstPotenciaFirmePorVersionReporte(int reportecodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);


                int pfrrptcodi = reportecodi;

                //_pfrService.ListarReportePotenciaFirme(pfrptcodi, out List<PfReporteTotalDTO> listaPF, out PfReporteTotalDTO regTotalPF, out List<PfReporteTotalDTO> listaPFxEmp
                //                                        , out List<PfEscenarioDTO> listaEscenario);

                //model.ListaPotenciaFirme = listaPF;
                //model.ListaEscenario = listaEscenario;

                model.HandsonModel = _pfrService.GenerarHandsonPFR(pfrrptcodi);
                model.TieneRegistroPrevio = model.HandsonModel.ListaExcelData2.Count() > 2;

                model.IdReporte = pfrrptcodi;

                model.TieneRegistroPrevio = false;
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Mensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Muestra las versiones de potencias firmes
        /// </summary>
        /// <param name="mirecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoDeCambiosPF(int mirecacodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarHtmlListadoVersionesPFR(url, model.TienePermisoEditar, mirecacodi);
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
        /// Guardar Informacion de Potencia Firme
        /// </summary>
        /// <param name="recacodi"></param>
        /// <param name="lstPFirme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarListadoEditadoPF(int recacodi, int reportecodi, string stringJson)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                var lstPFirme = JsonConvert.DeserializeObject<List<PfrReporteTotalDTO>>(stringJson);

                _pfrService.EditarReportePFRemunerableTransaccional(reportecodi,recacodi, lstPFirme, base.UserName);

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
        /// Permite generar archivo excel de Cuadro
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <param name="cuadro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoPFirme(int pfrtcodi, int tipoReporte)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (pfrtcodi > 0)
                {
                    //_pfrService.GenerarArchivoExcelPF(ruta, tipoReporte, pfrtcodi, out string nameFile);
                    //model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "No se ha realizado el Cálculo de la Potencia Firme para el Recálculo seleccionado.";
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

        #endregion
    }
}