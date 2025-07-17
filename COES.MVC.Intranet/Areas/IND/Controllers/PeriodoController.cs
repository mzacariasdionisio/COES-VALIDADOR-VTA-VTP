using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class PeriodoController : BaseController
    {
        private readonly INDAppServicio indServicio = new INDAppServicio();

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

        /// <summary>
        /// Muestra vista principal Periodo de Indisponibilidades
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string tipoMenu)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //realizar proceso 
                this.indServicio.CrearIndPeriodoAutomatico();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.TipoMenu = tipoMenu;

                DateTime fechaNuevo = this.indServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;
                model.ListaAnio = indServicio.ListaAnio(fechaNuevo);
                model.ListaMes = indServicio.ListaMes();
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
        /// Listado de periodos de indisponibilidades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int? omitirQuincenal)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = this.indServicio.GenerarHtmlListadoPeriodo(url, omitirQuincenal.GetValueOrDefault(0));
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
        /// Registrar periodo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoGuardar(int anio, int mes, string estado)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //Guardar
                IndPeriodoDTO reg = new IndPeriodoDTO();
                reg.Iperihorizonte = ConstantesIndisponibilidades.HorizonteMensual;
                reg.Iperianio = anio;
                reg.Iperimes = mes;
                reg.Iperinombre = anio + "." + EPDate.f_NombreMes(mes);
                reg.Iperianiomes = Convert.ToInt32(anio.ToString() + mes.ToString("D2"));
                reg.Iperiestado = !string.IsNullOrEmpty(estado) ? estado : ConstantesIndisponibilidades.EstadoPeriodoAbierto;
                reg.Iperiusucreacion = User.Identity.Name;
                reg.Iperifeccreacion = DateTime.Now;

                indServicio.SaveIndPeriodo(reg);

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
        /// Listado de recalculo de indisponibilidades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoListado(int ipericodi, int? omitirQuincenal)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.IndPeriodo = this.indServicio.GetByIdIndPeriodo(ipericodi);
                DateTime fechaPeriodo = new DateTime(model.IndPeriodo.Iperianio, model.IndPeriodo.Iperimes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

                string url = Url.Content("~/");
                model.Resultado = this.indServicio.GenerarHtmlListadoRecalculo(url, model.TienePermisoEditar, ipericodi, omitirQuincenal.GetValueOrDefault(0), out string ultimoTipoRecalculo);
                model.TipoRecalculo = ultimoTipoRecalculo;
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
        public JsonResult RecalculoGuardar(int irecacodi, int ipericodi, string tipo, string descripcion, string nombre, string strFechaIni, string strFechaFin
            , string strFechaLimite, string strFechaObs, string estado, string informe)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                IndRecalculoDTO recalculoAnterior = new IndRecalculoDTO();

                //Guardar
                IndRecalculoDTO reg = irecacodi > 0 ? indServicio.GetByIdIndRecalculo(irecacodi) : new IndRecalculoDTO();
                reg.Irecatipo = tipo;
                reg.Irecanombre = nombre;
                reg.Irecadescripcion = descripcion;
                reg.Irecafechaini = DateTime.ParseExact(strFechaIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.Irecafechafin = DateTime.ParseExact(strFechaFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                reg.Irecafechalimite = DateTime.ParseExact(strFechaLimite, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.Irecafechaobs = DateTime.ParseExact(strFechaObs, ConstantesAppServicio.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.Irecainforme = informe;
                reg.Estado = !string.IsNullOrEmpty(estado) ? estado : ConstantesIndisponibilidades.EstadoPeriodoAbierto;
                if (irecacodi == 0)
                {
                    reg.Ipericodi = ipericodi;
                    reg.Irecausucreacion = User.Identity.Name;
                    reg.Irecafeccreacion = DateTime.Now;
                    recalculoAnterior = indServicio.GetByCriteriaIndRecalculos(ipericodi).OrderByDescending(x => x.Orden).FirstOrDefault();
                }
                else
                {
                    reg.Irecausumodificacion = User.Identity.Name;
                    reg.Irecafecmodificacion = DateTime.Now;
                }

                if (irecacodi == 0)
                {
                    indServicio.SaveIndRecalculo(reg);
                    //guardar copia de calor útil
                    if(recalculoAnterior != null)
                        indServicio.GuardarCopiaCalorUtil(ipericodi, recalculoAnterior.Irecacodi, User.Identity.Name);
                }
                else
                    indServicio.UpdateIndRecalculo(reg);

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
        /// Editar circuito
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoObjeto(int irecacodi, string tipoMenu)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                model.IndRecalculo = indServicio.GetByIdIndRecalculo(irecacodi);
                model.IndPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);

                var regPeriodo = indServicio.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                DateTime fechaPeriodo = new DateTime(regPeriodo.Iperianio, regPeriodo.Iperimes, 1);
                model.FechaIni = fechaPeriodo.ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFin = fechaPeriodo.AddMonths(1).AddDays(-1).ToString(ConstantesAppServicio.FormatoFecha);

                model.IndRecalculo.FechaIniPeriodoDesc = model.FechaIni;
                model.IndRecalculo.FechaFinPeriodoDesc = model.FechaFin;

                string url = Url.Content("~/");
                if (ConstantesIndisponibilidades.MenuCuadro == tipoMenu)
                    model.Resultado2 = this.indServicio.GenerarHtmlListadoExcelCuadro(irecacodi, url);
                else
                    model.Resultado2 = this.indServicio.GenerarHtmlListadoExcelFactor(irecacodi, url);

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
        public JsonResult GenerarArchivoExcelCuadro(int irecacodi, int cuadro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;

                indServicio.GenerarArchivoExcelXCuadroAprobado(irecacodi, cuadro, ruta, out string nameFile, out string mensaje);

                model.Resultado = nameFile;
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

    }
}