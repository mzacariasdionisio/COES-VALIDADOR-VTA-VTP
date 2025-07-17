using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using COES.Servicios.Aplicacion.PotenciaFirme;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.Transferencias;
using COES.Servicios.Aplicacion.TransfPotencia;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Security.Authentication;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Controller
{
    public class PeriodoController : BaseController
    {
        readonly PotenciaFirmeRemunerableAppServicio pfrServicio = new PotenciaFirmeRemunerableAppServicio();
        readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();
        readonly INDAppServicio indServicio = new INDAppServicio();
        readonly TransfPotenciaAppServicio servicioTransfPotencia = new TransfPotenciaAppServicio();
        readonly PeriodoAppServicio servicioPeriodo = new PeriodoAppServicio();

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

        public PeriodoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index()
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaNuevo = this.pfrServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;
                model.ListaAnio = pfrServicio.ListaAnio(fechaNuevo);
                model.ListaMes = pfrServicio.ListaMes();
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

        #region Período y Recálculo

        /// <summary>
        /// Listado de periodos de Potencia Firme
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado()
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = this.pfrServicio.GenerarHtmlListadoPeriodo(url);
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
        /// Listado de recalculo de Potencia Firme
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoListado(int pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                var periodo = this.pfrServicio.GetByIdPfrPeriodo(pericodi);
                model.PfrPeriodo = periodo;
                DateTime fechaPeriodo = new DateTime(model.PfrPeriodo.Pfrperanio, model.PfrPeriodo.Pfrpermes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

                string url = Url.Content("~/");
                model.Resultado = this.pfrServicio.GenerarHtmlListadoRecalculo(url, model.TienePermisoEditar, pericodi, out string ultimoTipoRecalculo, out bool tieneReportePf);
                model.TipoRecalculo = ultimoTipoRecalculo;
                model.TieneReportePF = tieneReportePf;
                model.PeriodoActual = periodo.Pfrpermes + " " + periodo.Pfrperanio;
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
        /// <param name="recacodi"></param>
        /// <param name="pericodi"></param>
        /// <param name="comentario"></param>
        /// <param name="nombre"></param>
        /// <param name="revisionPf"></param>
        /// <param name="strFechaLimite"></param>
        /// <param name="informe"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoGuardar(int recacodi, int pericodi, string comentario, string nombre, string strFechaLimite, string informe, string tipo)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (!tienePermiso) throw new InvalidCredentialException(Constantes.MensajePermisoNoValido);

                //Guardar
                PfrRecalculoDTO reg = recacodi > 0 ? pfrServicio.GetByIdPfrRecalculo(recacodi) : new PfrRecalculoDTO();
                reg.Pfrrecdescripcion = comentario;
                reg.Pfrrecfechalimite = DateTime.ParseExact(strFechaLimite, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.Pfrrecinforme = informe;
                reg.Pfrrectipo = tipo;

                if (recacodi == 0) //si es nuevo recalculo
                {
                    reg.Pfrrecnombre = nombre;
                    reg.Pfrpercodi = pericodi;
                    reg.Pfrrecusucreacion = User.Identity.Name;
                    reg.Pfrrecfeccreacion = DateTime.Now;
                }
                else
                {
                    reg.Pfrrecusumodificacion = User.Identity.Name;
                    reg.Pfrrecfecmodificacion = DateTime.Now;
                }

                //¿Guardo o Edito?
                if (recacodi == 0) //Guardo
                {
                    var mensaje = pfrServicio.ValidarNombreRepetido(reg);
                    if (mensaje == "")
                        pfrServicio.GuardarRecalculo(reg, base.UserName);
                    else
                    {
                        model.Resultado = "2";
                        model.Mensaje = mensaje;
                        return Json(model);
                    }

                }
                else //Edito
                    pfrServicio.UpdatePfrRecalculo(reg);

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
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoObjeto(int recacodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                bool tienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                if (!tienePermiso) throw new InvalidCredentialException(Constantes.MensajePermisoNoValido);

                model.PfrRecalculo = pfrServicio.GetByIdPfrRecalculo(recacodi);
                model.PfrPeriodo = pfrServicio.GetByIdPfrPeriodo(model.PfrRecalculo.Pfrpercodi);

                var regPeriodo = pfrServicio.GetByIdPfrPeriodo(model.PfrRecalculo.Pfrpercodi);
                DateTime fechaPeriodo = new DateTime(regPeriodo.Pfrperanio, regPeriodo.Pfrpermes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

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

        #region Cálculo y Reporte

        /// <summary>
        /// Nuevo cálculo
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <param name="pfrreccodi"></param>
        /// <returns></returns>
        public PartialViewResult CargarVentanaProcesar(int pfrpercodi, int pfrreccodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            var regPeriodoPFR = pfrServicio.GetByIdPfrPeriodo(pfrpercodi);
            int anioPeriodoPFR = regPeriodoPFR.Pfrperanio;
            int mesPeriodoPFR = regPeriodoPFR.Pfrpermes;

            var lstPeriodosPFPorAnio = pfServicio.GetByCriteriaPfPeriodos(anioPeriodoPFR);
            var regPeriodoPF = lstPeriodosPFPorAnio.Find(x => x.Pfperimes == mesPeriodoPFR);

            int pfpericodi = 0;
            if (regPeriodoPF != null)
                pfpericodi = regPeriodoPF.Pfpericodi;


            model.ListaRevisionesPF = pfServicio.GetByCriteriaPfRecalculos(pfpericodi);
            model.IdRecalculo = pfrreccodi;
            PfrPeriodoDTO pfrPeriodo = regPeriodoPFR;

            DateTime mesAnterior = pfrServicio.ObtenerMesAnterior(pfrpercodi);
            int periodoAnteriorIND = indServicio.ObtenerPeriodoAnterior(mesAnterior);
            model.ListaRevisionesIndMesAnterior = indServicio.GetByCriteriaIndRecalculos(periodoAnteriorIND);

            List<PrGrupodatDTO> parametros = pfrServicio.ListarParametrosConfiguracionPorFecha(pfrPeriodo.FechaFin, ConstantesPotenciaFirmeRemunerable.ConcepcodiIngresos);
            var paramCA = parametros.Find(z => z.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCA);
            model.ValorActualCA = paramCA != null ? paramCA.Formuladat : "SIN DATO";
            var paramCR = parametros.Find(z => z.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiCR);
            model.ValorActualCR = paramCR != null ? paramCR.Formuladat : "SIN DATO";
            var paramMR = parametros.Find(z => z.Concepcodi == ConstantesPotenciaFirmeRemunerable.ConcepcodiMR);
            model.ValorActualMR = paramMR != null ? paramMR.Formuladat : "SIN DATO";

            //Relacion con transferencias
            List<PeriodoDTO> listaPeriodos = this.servicioPeriodo.ListPeriodo();
            PeriodoDTO periodoTransferencia = listaPeriodos.Find(x => x.AnioCodi == regPeriodoPFR.Pfrperanio && x.MesCodi == regPeriodoPFR.Pfrpermes);
            model.ListaRecalculoTransferencia = this.servicioTransfPotencia.ListByPericodiVtpRecalculoPotencia(periodoTransferencia.PeriCodi); //Ordenado en descendente

            return PartialView(model);
        }

        /// <summary>
        /// Webservice
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <param name="pfrecacodi"></param>
        /// <param name="indrecacodiant"></param>
        /// <param name="recpotcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarCalculoPFR(int pfrreccodi, int pfrecacodi, int indrecacodiant, int recpotcodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                ServiceReferencePotenciaFirme.PotenciaFirmeServicioClient cliente = new ServiceReferencePotenciaFirme.PotenciaFirmeServicioClient();

                // Ruta base de Potencia Firme Remunerable         
                string pathBasePFR = base.PathFiles + "\\" + ConstantesPotenciaFirmeRemunerable.PotenciaFirmeRemunerableFile;
                // crear y Obtener path carpeta actual
                string nombreCarpetaActual = ConstantesPotenciaFirmeRemunerable.SNombreCarpetaSalidasGams;
                FileServer.CreateFolder(pathBasePFR, nombreCarpetaActual, null); // para asegurarnos de su existencia

                //int reporteCodiGenerado = pfrServicio.CalcularReportePFR(pfrreccodi, pfrecacodi, indrecacodiant, recpotcodi, base.UserName);
                int reporteCodiGenerado = cliente.EjecutarPotenciaRemunerable(pfrreccodi, pfrecacodi, indrecacodiant, recpotcodi, base.UserName);
                model.IdReporte = reporteCodiGenerado;
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
        /// Tab Reportes
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReportesSalida(int pfrreccodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                var pfrrptcodiAux2 = pfrServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);
                model.Resultado = pfrServicio.GenerarHtmlTabSalida(url, pfrrptcodiAux2);
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
        /// Excel Reporte LVTP
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <param name="cuadro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoPFirmeRemunerable(int tipoReporte, int pfrreccodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                if (pfrreccodi > 0)
                {
                    pfrServicio.GenerarArchivoExcelPFR(ruta, tipoReporte, pfrreccodi, out string nameFile);
                    model.Resultado = nameFile;
                }
                else
                {
                    model.Mensaje = "Ocurrio un error.";
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
        /// Excel REPORTE LVTP-OPF
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="pfrreccodi"></param>
        /// <param name="numEscenario"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelLVTPOPF(int tipoReporte, int pfrreccodi, int numEscenario)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                var pfrrptcodiPF = pfrServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);
                var pfrrptcodidatos = pfrServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi, ConstantesPotenciaFirmeRemunerable.CuadroDatos);

                int pfrrptcodi = pfrrptcodiPF;
                if (pfrrptcodi > 0)
                {
                    pfrServicio.GenerarArchivoExcelPFR2(ruta, tipoReporte, pfrrptcodi, pfrrptcodidatos, numEscenario, out string nameFile);
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

            System.IO.File.Delete(ruta + nombreArchivo);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
        }

        /// <summary>
        /// Zip
        /// </summary>
        /// <param name="pfrreccodi"></param>
        /// <returns></returns>
        public JsonResult DescargarArchivosGams(int pfrreccodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                var pfrrptcodigeneral = pfrServicio.GetUltimoPfrrptcodiXRecalculo(pfrreccodi, ConstantesPotenciaFirmeRemunerable.CuadroPFirmeRemunerable);

                pfrServicio.GenerarGamsZip(ruta, pfrreccodi, out string nameFile);
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

        #endregion

    }
}