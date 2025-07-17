using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PotenciaFirme.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Controllers
{
    public class ReporteController : BaseController
    {
        readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();

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

        public ReporteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #region GENERAL

        /// <summary>
        /// Obtiene el año y mes concatenado
        /// </summary>
        /// <param name="nomPeriodo"></param>
        /// <returns></returns>
        public int ObtenerPeriAnioMes(string nomPeriodo)
        {
            DateTime fechaPeriodo = DateTime.ParseExact(nomPeriodo, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
            var periAnioMes = int.Parse(fechaPeriodo.ToString(ConstantesAppServicio.FormatoAnioMes));

            return periAnioMes;
        }

        /// <summary>
        /// Obtiene los recálculos para un periodo seleccionado
        /// </summary>
        /// <param name="nomPeriodo"></param>
        /// <returns></returns>
        public JsonResult CargarRevisiones(int pfpericodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(pfpericodi);
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
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(anio);
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

        #region REPORTE POTENCIA FIRME

        /// <summary>
        /// Página Principal de Cálculo de Potencia Firme
        /// </summary>
        /// <returns></returns>      
        public ActionResult IndexPotenciaFirme(int? pericodi, int? recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaPeriodo = pfServicio.GetPeriodoActual(); // primer dia del mes anterior
                model.ListaAnio = pfServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = pfServicio.GetByIdPfPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = pfServicio.GetByCriteriaPfPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<PfRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = pfServicio.GetByCriteriaPfRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Pfrecacodi : recacodi.GetValueOrDefault(0);
                }

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
        //public JsonResult CargarLstPotenciaFirme(int pfrptcodi, int revision)
        public JsonResult CargarLstPotenciaFirme(int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);


                int pfrptcodi = pfServicio.GetUltimoPfrptcodiXRecalculo(recacodi, ConstantesPotenciaFirme.CuadroPFirme);
                this.pfServicio.ListarReportePotenciaFirme(pfrptcodi, out List<PfReporteTotalDTO> listaPF, out PfReporteTotalDTO regTotalPF, out List<PfReporteTotalDTO> listaPFxEmp
                                                        , out List<PfEscenarioDTO> listaEscenario);

                model.ListaPotenciaFirme = listaPF;
                model.ListaEscenario = listaEscenario;

                model.IdRecalculo = recacodi;
                model.IdReporte = pfrptcodi;

                model.TieneRegistroPrevio = listaPF.Count > 0;
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
        public JsonResult CargarLstPotenciaFirmePorVersionReporte(int reportecodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);


                int pfrptcodi = reportecodi;
                this.pfServicio.ListarReportePotenciaFirme(pfrptcodi, out List<PfReporteTotalDTO> listaPF, out PfReporteTotalDTO regTotalPF, out List<PfReporteTotalDTO> listaPFxEmp
                                                        , out List<PfEscenarioDTO> listaEscenario);

                model.ListaPotenciaFirme = listaPF;
                model.ListaEscenario = listaEscenario;

                model.IdReporte = pfrptcodi;

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
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = this.pfServicio.GenerarHtmlListadoVersionesPF(url, model.TienePermisoEditar, mirecacodi);
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
        public JsonResult GuardarListadoEditadoPF(int recacodi, int reportecodi, List<PfReporteTotalDTO> lstPFirme)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                pfServicio.EditarReportePFirmeTransaccional(reportecodi, lstPFirme, base.UserName);

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
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                if (pfrtcodi > 0)
                {
                    pfServicio.GenerarArchivoExcelPF(ruta, tipoReporte, pfrtcodi, out string nameFile);
                    model.Resultado = nameFile;
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