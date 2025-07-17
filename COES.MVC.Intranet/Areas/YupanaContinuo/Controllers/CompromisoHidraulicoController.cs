using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Hidrologia;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class CompromisoHidraulicoController : BaseController
    {
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();
        readonly FormatoMedicionAppServicio formatoServicio = new FormatoMedicionAppServicio();
        readonly HidrologiaAppServicio hidrologiaServicio = new HidrologiaAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
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

        #endregion

        /// <summary>
        /// Index Compromiso Hidráulico
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CompromisoHidraulicoModel model = new CompromisoHidraulicoModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //si existe un arbol en ejecucion
            CpArbolContinuoDTO objUltimoArbol = yupanaServicio.GetUltimoArbol();
            bool existeArbolEnEjec = objUltimoArbol != null;

            DateTime fechaActual = DateTime.Today;
            if (existeArbolEnEjec) fechaActual = objUltimoArbol.Cparbfecha;
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            model.IdEnvioSC = 0;
            model.IdEnvioCC = 0;

            //verificar que exista envio del dia seleccionado
            yupanaServicio.ObtenerUltimosEnvios(fechaActual, out int ultimoIdEnvioSC, out int ultimoIdEnvioCC);
            model.Mensaje = "";
            if (ultimoIdEnvioSC <= 0) model.Mensaje = "Sin Compromiso";
            if (ultimoIdEnvioCC <= 0) model.Mensaje += (model.Mensaje != "" ? "," : "") + "Con Compromiso";
            if (model.Mensaje != "") model.Mensaje = string.Format("No existe configuración ({0}) para el día seleccionado.", model.Mensaje);

            return View(model);
        }

        /// <summary>
        /// Obtiene el html de la tabla a mostrar en la pestaña seleccionada
        /// </summary>
        /// <param name="pestania"></param>
        /// <param name="verscodi"></param>
        /// <param name="fechaConsulta"></param>
        /// <param name="idEnvioSC"></param>
        /// <param name="idEnvioCC"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarTablaCompromisoHidraulico(int pestania, string fechaConsulta, int idEnvioSC, int idEnvioCC)
        {
            CompromisoHidraulicoModel model = new CompromisoHidraulicoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                int formatoSC = -1;
                int formatoCC = -1;
                int pestaniaCargar = pestania;

                if (idEnvioSC == 0 && idEnvioCC == 0)  //Obtenemos ultimos envios para los casos: inicie la ventana, presiona "Consultar"
                {
                    yupanaServicio.ObtenerUltimosEnvios(fecha, out int ultimoIdEnvioSC, out int ultimoIdEnvioCC);
                    idEnvioSC = ultimoIdEnvioSC;
                    idEnvioCC = ultimoIdEnvioCC;
                }

                if (pestaniaCargar == ConstantesYupanaContinuo.PestaniaSinCompromiso)
                    formatoSC = ConstantesYupanaContinuo.FormatoSinCompromiso;
                else
                {
                    if (pestaniaCargar == ConstantesYupanaContinuo.PestaniaConCompromiso)
                        formatoCC = ConstantesYupanaContinuo.FormatoConCompromiso;
                    else
                    {
                        if (pestaniaCargar == ConstantesYupanaContinuo.PestaniaAmbos)
                        {
                            formatoSC = ConstantesYupanaContinuo.FormatoSinCompromiso;
                            formatoCC = ConstantesYupanaContinuo.FormatoConCompromiso;
                        }
                    }
                }

                FormatoModel jsModelSC = new FormatoModel();
                FormatoModel jsModelCC = new FormatoModel();
                var lstEmpresas = yupanaServicio.ObtenerEmpresas();


                if (formatoSC != -1)
                {
                    jsModelSC = yupanaServicio.ContenidoTabla(formatoSC, formatoSC, idEnvioSC, fecha, out string envioFechaSC, out bool esUltimaSC, out int totalEnviosSC);
                    model.HtmlSC = yupanaServicio.GenerarHtmlCompromisoHidraulico(jsModelSC, fecha, lstEmpresas, ConstantesYupanaContinuo.PestaniaSinCompromiso);
                    model.HtmlCambiosSC = yupanaServicio.GenerarHtmlListadoVersion(jsModelSC, ConstantesYupanaContinuo.PestaniaSinCompromiso);
                    model.VersionFechaSC = envioFechaSC;
                    model.EsUltimaVersionSC = esUltimaSC;
                    model.NumEnviosSC = totalEnviosSC;
                }

                if (formatoCC != -1)
                {
                    jsModelCC = yupanaServicio.ContenidoTabla(formatoCC, formatoCC, idEnvioCC, fecha, out string envioFechaCC, out bool esUltimaCC, out int totalEnviosCC);
                    model.HtmlCC = yupanaServicio.GenerarHtmlCompromisoHidraulico(jsModelCC, fecha, lstEmpresas, ConstantesYupanaContinuo.PestaniaConCompromiso);
                    model.HtmlCambiosCC = yupanaServicio.GenerarHtmlListadoVersion(jsModelCC, ConstantesYupanaContinuo.PestaniaConCompromiso);
                    model.VersionFechaCC = envioFechaCC;
                    model.EsUltimaVersionCC = esUltimaCC;
                    model.NumEnviosCC = totalEnviosCC;
                }


            }
            catch (Exception ex)
            {
                model.Mensaje = ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guarda la informacion de la tabla
        /// </summary>
        /// <param name="lstNoSeleccionados"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="formatoReal"></param>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDataTablaCompromiso(List<CompromisoHidraulico> lstNoSeleccionados, int idEmpresa, int formatoReal, string fechaConsulta)
        {
            ///////// Definicion de Variables ////////////////
            FormatoResultado model = new FormatoResultado();
            model.Resultado = 0;
            DateTime fecha = DateTime.ParseExact(fechaConsulta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                int exito = 0;
                List<string> celdas = new List<string>();

                int idFormato = hidrologiaServicio.ObtenerIdFormatoPadre(formatoReal);
                MeFormatoDTO formato = this.formatoServicio.GetByIdMeFormato(formatoReal);

                var cabercera = formatoServicio.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabercera.Cabcolumnas;
                formato.Formatrows = cabercera.Cabfilas;
                formato.Formatheaderrow = cabercera.Cabcampodef;
                int filaHead = formato.Formatrows;
                int colHead = formato.Formatcols;

                /////////////// Obtiene Fecha Inicio y Fecha Fin del Proceso //////////////
                formato.FechaProceso = fecha;
                formatoServicio.GetSizeFormato2(formato);

                var listaPto = this.formatoServicio.GetByCriteriaMeHojaptomeds(idEmpresa, idFormato, formato.FechaInicio, formato.FechaFin).OrderBy(x=>x.Ptomedicodi).ToList();
                int nPtos = listaPto.Count();

                /////////////// Grabar Config Formato Envio //////////////////
                MeConfigformatenvioDTO config = new MeConfigformatenvioDTO();
                config.Formatcodi = formatoReal;
                config.Emprcodi = idEmpresa;
                config.FechaInicio = formato.FechaInicio;
                config.FechaFin = formato.FechaFin;
                int idConfig = formatoServicio.GrabarConfigFormatEnvio(config);

                //////////////////////////////////////////////////////////////////////////

                ///////////////Grabar Envio//////////////////////////
                string mensajePlazo = string.Empty;
                Boolean enPlazo = formatoServicio.ValidarPlazoController(formato);//ValidarFecha(idEmpresa, formato.FechaInicio, idFormato, out mensajePlazo);

                int miidEnvio = 0;
                MeEnvioDTO envio = new MeEnvioDTO();
                envio.Archcodi = 0;
                envio.Emprcodi = idEmpresa;
                envio.Enviofecha = DateTime.Now;
                envio.Enviofechaperiodo = formato.FechaInicio;
                envio.Envioplazo = (enPlazo) ? "P" : "F";
                envio.Estenvcodi = ParametrosEnvio.EnvioEnviado;
                envio.Lastdate = DateTime.Now;
                envio.Lastuser = User.Identity.Name;
                envio.Userlogin = User.Identity.Name;
                envio.Formatcodi = formatoReal;
                envio.Cfgenvcodi = idConfig;
                miidEnvio = formatoServicio.SaveMeEnvio(envio);
                model.IdEnvio = miidEnvio;

                ///////////////////////////////////////////////////////
                int horizonte = formato.Formathorizonte;
                switch (formato.Formatresolucion)
                {
                    case ParametrosFormato.ResolucionHora:
                        var lista24_ = yupanaServicio.LeerTabla24(lstNoSeleccionados, listaPto, formato.Lectcodi, colHead, nPtos + 1, filaHead, 24 * formato.Formathorizonte, fecha);
                        this.formatoServicio.GrabarValoresCargados24(lista24_, User.Identity.Name, miidEnvio, idEmpresa, formato);
                        envio.Estenvcodi = ParametrosEnvio.EnvioAprobado;
                        envio.Enviocodi = miidEnvio;
                        formatoServicio.UpdateMeEnvio(envio);
                        exito = 1;
                        model.Resultado = 1;
                        model.Mensaje = MensajesDemandaCP.MensajeEnvioExito;
                        break;

                }
                model.Resultado = exito;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

    }
}