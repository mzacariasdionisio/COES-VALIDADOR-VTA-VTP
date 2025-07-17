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
using System.Security.Authentication;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirme.Controllers
{
    public class PeriodoController : BaseController
    {
        readonly PotenciaFirmeAppServicio pfServicio = new PotenciaFirmeAppServicio();

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
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            model.UsarLayoutModulo = true;

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                DateTime fechaNuevo = this.pfServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;
                model.ListaAnio = pfServicio.ListaAnio(fechaNuevo);
                model.ListaMes = pfServicio.ListaMes();
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
        /// Listado de periodos de Potencia Firme
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado()
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                string url = Url.Content("~/");
                model.Resultado = this.pfServicio.GenerarHtmlListadoPeriodo(url);
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
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                var periodo = this.pfServicio.GetByIdPfPeriodo(pericodi);
                model.PfPeriodo = periodo;
                DateTime fechaPeriodo = new DateTime(model.PfPeriodo.Pfperianio, model.PfPeriodo.Pfperimes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

                string url = Url.Content("~/");
                model.Resultado = this.pfServicio.GenerarHtmlListadoRecalculo(url, model.TienePermisoEditar, pericodi, out string ultimoTipoRecalculo, out bool tieneReportePf);
                model.TipoRecalculo = ultimoTipoRecalculo;
                model.TieneReportePF = tieneReportePf;
                model.PeriodoActual = periodo.Pfperimes + " " + periodo.Pfperianio;
                model.ListaIndRecalculo = this.pfServicio.ListIndRecalculosByMes(fechaPeriodo);
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
        /// Registrar recalculo
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="descripcion"></param>
        /// <param name="nombre"></param>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <param name="strFechaLimite"></param>
        /// <param name="strFechaObs"></param>
        /// <param name="estado"></param>
        /// <param name="informe"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoGuardar(int recacodi, int pericodi, string comentario, string nombre, int irecacodi, string strFechaLimite, string informe, string tipo)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new InvalidCredentialException(Constantes.MensajePermisoNoValido);

                //Guardar
                PfRecalculoDTO reg = recacodi > 0 ? pfServicio.GetByIdPfRecalculo(recacodi) : new PfRecalculoDTO();
                reg.Pfrecadescripcion = comentario;
                if (recacodi == 0)
                    reg.Estado = ConstantesPotenciaFirme.Abierto;
                reg.Pfrecafechalimite = DateTime.ParseExact(strFechaLimite, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.Pfrecanombre = nombre;
                reg.Pfrecainforme = informe;
                reg.Pfrecatipo = tipo;
                reg.Irecacodi = irecacodi;

                //validar
                string mensaje = pfServicio.ValidarRecalculoIndisponibilidades(reg.Irecacodi);

                if (!string.IsNullOrEmpty(mensaje))
                {
                    model.Resultado = "2";
                    model.Mensaje = mensaje;
                    return Json(model);
                }

                if (recacodi == 0)
                {
                    reg.Pfpericodi = pericodi;

                    mensaje = pfServicio.ValidarNombreRepetido(reg);
                    if (!string.IsNullOrEmpty(mensaje))
                    {
                        model.Resultado = "2";
                        model.Mensaje = mensaje;
                        return Json(model);
                    }
                }

                //guardar
                if (recacodi == 0)
                {
                    reg.Pfrecanombre = nombre;
                    reg.Pfpericodi = pericodi;
                    reg.Pfrecausucreacion = User.Identity.Name;
                    reg.Pfrecafeccreacion = DateTime.Now;
                }
                else
                {
                    reg.Pfrecausumodificacion = User.Identity.Name;
                    reg.Pfrecafecmodificacion = DateTime.Now;
                }

                if (recacodi == 0)
                    pfServicio.GuardarRecalculo(reg, base.UserName);
                else
                    pfServicio.UpdatePfRecalculo(reg);

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
            PotenciaFirmeModel model = new PotenciaFirmeModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                model.PfRecalculo = pfServicio.GetByIdPfRecalculo(recacodi);
                model.PfPeriodo = pfServicio.GetByIdPfPeriodo(model.PfRecalculo.Pfpericodi);

                var regPeriodo = pfServicio.GetByIdPfPeriodo(model.PfRecalculo.Pfpericodi);
                DateTime fechaPeriodo = new DateTime(regPeriodo.Pfperianio, regPeriodo.Pfperimes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);
                model.IdReporte = pfServicio.GetUltimoPfrptcodiXRecalculo(recacodi, ConstantesPotenciaFirme.CuadroPFirme);
                model.ListaIndRecalculo = this.pfServicio.ListIndRecalculosByMes(fechaPeriodo);

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

        /// <summary>
        /// Carga las versiones aprobadas de los insumos
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public PartialViewResult CargarRevisionesInsumos(int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            model.IdRecalculo = recacodi;
            model.RevisionPG = pfServicio.ObtenerRevisionesVersionInsumo(recacodi, ConstantesPotenciaFirme.RecursoPGarantizada);
            model.RevisionPA = pfServicio.ObtenerRevisionesVersionInsumo(recacodi, ConstantesPotenciaFirme.RecursoPAdicional);
            model.RevisionFI = pfServicio.ObtenerRevisionesVersionInsumo(recacodi, ConstantesPotenciaFirme.RecursoFactorIndispFortuita);
            model.RevisionFP = pfServicio.ObtenerRevisionesVersionInsumo(recacodi, ConstantesPotenciaFirme.RecursoFactorPresencia);
            model.RevisionCCV= pfServicio.ObtenerRevisionesVersionInsumo(recacodi, ConstantesPotenciaFirme.RecursoContratosCV);

            return PartialView(model);
        }

        /// <summary>
        /// Genera los calculos de Potencia Firme
        /// </summary>
        /// <param name="version_pg"></param>
        /// <param name="version_pa"></param>
        /// <param name="version_cu"></param>
        /// <param name="version_fi"></param>
        /// <param name="version_fp"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarCalculoPF(int versionPG, int versionPA, int versionFI, int versionFP, int versionCCV, int recacodi)
        {
            PotenciaFirmeModel model = new PotenciaFirmeModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                int reporteCodiGenerado = pfServicio.CalcularReportePFirmeTransaccional(recacodi, versionPG, versionPA, versionFI, versionFP, versionCCV, base.UserName);

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
    }
}