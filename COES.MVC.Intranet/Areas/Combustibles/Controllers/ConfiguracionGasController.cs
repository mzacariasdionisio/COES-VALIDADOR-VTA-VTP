using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Correo;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Combustibles.Controllers
{
    public class ConfiguracionGasController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CombustibleAppServicio servicioCombustible = new CombustibleAppServicio();
        CorreoAppServicio servCorreo = new CorreoAppServicio();
        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(ConfiguracionGasController));
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

        #region Configuracion_Centrales
        /// <summary>
        /// Muestra la pantalla de configuración de centrales termicas
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCentral()
        {
            CombustibleGasModel model = new CombustibleGasModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.ListadoCentrales = servicioCombustible.ObtenerListadoCentralesTermicas(false);

            return View(model);
        }

        /// <summary>
        /// Obtiene informacion de cierta central 
        /// </summary>
        /// <param name="cbcxfecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarCentral(int cbcxfecodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                CbCentralxfenergDTO central = servicioCombustible.ObtenerDatosCentralTermica(cbcxfecodi);
                model.CentralTermica = central;
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
        /// Guarda la configuracion de una central
        /// </summary>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCentral(CbCentralxfenergDTO central)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                servicioCombustible.GuardarDatosCentralTermica(central, base.UserName);
                model.CentralTermica = central;
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

        #region Automático

        /// <summary>
        /// Ejecutar proceso automatico
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarProcesoAutomatico(int tipo)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                servicioCombustible.EjecutarProcesoAutomaticoPR31(tipo);

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
        #endregion

        #region Configuracion_Plantilla_F3

        public ActionResult IndexPlantilla()
        {
            CombustibleGasModel model = new CombustibleGasModel();
            model.FechaActual = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Listado Reporte Configuracion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoReportePlantilla()
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.ListadoPlantilla = servicioCombustible.GetByCriteriaCbFichas();
                model.ListadoPropiedad = servicioCombustible.GetByCriteriaCbConceptocombs(ConstantesCombustibles.EstcomcodiGas).OrderBy(x => x.Ccombcodi).ToList();

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Guardar Configuracion
        /// </summary>
        /// <param name="yupcfgcodi"></param>
        /// <param name="recurcodi"></param>
        /// <param name="strConf"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPlantilla(string strConf)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CbFichaDTO objConf = !string.IsNullOrEmpty(strConf) ? serializer.Deserialize<CbFichaDTO>(strConf) : new CbFichaDTO();
                objConf.Cbftfechavigencia = DateTime.ParseExact(objConf.CbftfechavigenciaDesc, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //validar campos obligatorios de Nombre(máximo 100 caracteres)
                if (objConf.Cbftnombre == string.Empty)
                    throw new Exception("El nombre de la plantilla es un campo obligatorio");
                else
                {
                    if (objConf.Cbftnombre.Length > 100)
                        throw new Exception("El nombre de la plantilla supera 100 caracteres");
                }

                //validar campos obligatorios de Fecha de vigencia
                if (objConf.CbftfechavigenciaDesc == string.Empty)
                    throw new Exception("La fecha de vigencia es un campo obligatorio");

                servicioCombustible.GuardarPlantillaFormato3(objConf, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ObtenerPlantilla(int cbftcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                model.Plantilla = servicioCombustible.GetByIdCbFicha(cbftcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult EliminarPlantilla(int cbftcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);
                if (cbftcodi == 1)
                    throw new ArgumentException("No está permitido la eliminación de la plantilla Base");
                servicioCombustible.EliminarPlantillaFormato3(cbftcodi, base.UserName);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult CopiarPlantilla(int cbftcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                servicioCombustible.CopiarPlantillaFormato3(cbftcodi, base.UserName);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GuardarPropiedad(string strConf)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                CbConceptocombDTO objConf = !string.IsNullOrEmpty(strConf) ? serializer.Deserialize<CbConceptocombDTO>(strConf) : new CbConceptocombDTO();

                servicioCombustible.GuardarPropiedad(objConf, base.UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Configuracion Plantilla Correos
        /// <summary>
        /// Pagina principal de plantilla de correos
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexPlantillaCorreo()
        {
            CombustibleGasModel model = new CombustibleGasModel();

            return View(model);
        }

        /// <summary>
        /// Devuelve el listado de plantillas de acuerdo al tipo ce plnatilla y central
        /// </summary>
        /// <param name="tipoPlantilla"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarPlantillasCorreos(int tipoPlantilla, string tipoCentral)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoPlantillasCorreo = servicioCombustible.ListarPlantillasCorreo(tipoPlantilla, tipoCentral);
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
        /// Devuelve los detalles de la plantilla del correo
        /// </summary>
        /// <param name="plantillacodi"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDetalleCorreo(int plantillacodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                //Obtengo el registro de la plantilla
                SiPlantillacorreoDTO plantillaCompleta = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaConHoraEjecucion = new SiPlantillacorreoDTO();
                SiPlantillacorreoDTO plantillaBD = servCorreo.GetByIdSiPlantillacorreo(plantillacodi);
                SiPlantillacorreoDTO plantilla = servicioCombustible.GetCampoPara(plantillaBD);

                if (plantillacodi == ConstantesCombustibles.RecordatorioRevisarEvaluarInformaciónRecibida_CE || plantillacodi == ConstantesCombustibles.RecordatorioRevisarEvaluarInformaciónRecibida_CN ||
                    plantillacodi == ConstantesCombustibles.RecordatorioInformarVencimientoPlazoSubsanacion_CE || plantillacodi == ConstantesCombustibles.RecordatorioInformarVencimientoPlazoSubsanacion_CN ||
                    plantillacodi == ConstantesCombustibles.RecordatorioRevisarEvaluarSubsanacionPresentadas_CE || plantillacodi == ConstantesCombustibles.RecordatorioRevisarEvaluarSubsanacionPresentadas_CN)
                {
                    plantillaConHoraEjecucion = servicioCombustible.AgregarHoraEstadoRecordatorio(plantilla);
                    plantillaCompleta = servicioCombustible.AgregarParametrosDiaHora(plantillaConHoraEjecucion); //Agrega la hora y dias de la variable del contenido 
                }
                else
                {
                    plantillaCompleta = plantilla;
                }


                model.PlantillaCorreo = plantillaCompleta;

                //obtengo las variables del Contenido
                model.ListaVariables = servicioCombustible.ObtenerListadoVariables(plantillacodi, ConstantesCombustibles.VariableContenido);
                model.TipoCorreo = servicioCombustible.ObtenerTipoCorreo(plantillacodi);
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
        /// <param name="idBoton"></param>
        /// <param name="idPlantilla"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarVariables(int idPlantilla, int campo)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);


                model.ListaVariables = servicioCombustible.ObtenerListadoVariables(idPlantilla, campo);
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
        /// Guarda la informacion de una plantilla de correo
        /// </summary>
        /// <param name="plantillaCorreo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPlantillaCorreo(SiPlantillacorreoDTO plantillaCorreo)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Actualizo la plantilla
                servicioCombustible.ActualizarDatosPlantillaCorreo(plantillaCorreo, base.UserName);
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
        ///  Ejecutar recordatorio de manera manual
        /// </summary>
        /// <param name="plantcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarRecordatorio(int plantcodi)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                Dictionary<int, string> lstUsuarioPorEmpresa = new Dictionary<int, string>();

                List<CbCentralxfenergDTO> listaCentralTermicasTipoExistentesNuevas = servicioCombustible.ObtenerListadoCentralesTermicas(true).Where(x => x.Cbcxfeexistente == 1 || x.Cbcxfenuevo == 1).ToList();
                List<int> lstEmprcodisCentralesExistentesNuevas = listaCentralTermicasTipoExistentesNuevas.Select(x => x.Emprcodi).Distinct().ToList();

                foreach (var emprcodi in lstEmprcodisCentralesExistentesNuevas)
                {
                    string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(emprcodi, "");

                    lstUsuarioPorEmpresa.Add(emprcodi, otrosUsuariosEmpresa);
                }

                servicioCombustible.EjecutarRecordatoriosManualmente(plantcodi, lstUsuarioPorEmpresa);

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
        /// correos para CC
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="ususolicitud"></param>
        /// <returns></returns>
        private string ObtenerCCcorreosAgente(int idEmpresa, string ususolicitud)
        {
            ususolicitud = (ususolicitud ?? "").ToLower().Trim();

            var listaCorreo = ObtenerCorreosGeneradorModuloPr31(idEmpresa);
            listaCorreo = listaCorreo.Where(x => x != ususolicitud).OrderBy(x => x).ToList();

            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloPr31(int idEmpresa)
        {
            List<string> listaCorreo = new List<string>();

            //modulos extranet
            var listaModuloExtr = seguridad.ListarModulos().Where(x => (x.RolName.StartsWith("Usuario Extranet") || x.RolName.StartsWith("Extranet")) && x.ModEstado.Equals(ConstantesAppServicio.Activo)).OrderBy(x => x.ModNombre).ToList();

            //considerar solo a los usuarios activos de la empresa
            var listaUsuarios = seguridad.ListarUsuariosPorEmpresa(idEmpresa).Where(x => x.UserState == ConstantesAppServicio.Activo).ToList();
            foreach (var regUsuario in listaUsuarios)
            {
                var listaModuloXUsu = seguridad.ObtenerModulosPorUsuarioSelecion(regUsuario.UserCode).ToList();

                //modulos que tiene el usuario en extranet
                var listaModuloXUsuExt = listaModuloXUsu.Where(x => listaModuloExtr.Any(y => y.ModCodi == x.ModCodi)).ToList();

                var regPr31 = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesCombustibles.ModcodiPr31ExtranetGaseoso);
                if (regPr31 != null && regPr31.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }
        #endregion

        #region Configuracion Correos

        /// <summary>
        /// Pagina principal de envio de mensaje a agentes
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexCorreo()
        {
            CombustibleGasModel model = new CombustibleGasModel();

            DateTime hoy = DateTime.Now;
            DateTime primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);
            model.FechaInicio = primerDiaMes.AddMonths(-1).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = hoy.ToString(ConstantesAppServicio.FormatoFecha);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Lista los correos enviados a los agentes
        /// </summary>
        /// <param name="rangoIni"></param>
        /// <param name="rangoFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCorreosEnviados(string rangoIni, string rangoFin)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(rangoIni, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(rangoFin, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                model.ListadoCorreosEnviados = servicioCombustible.ListarCorreosEnviados(fechaInicio, fechaFin);
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
        public JsonResult ObtenerDatosDelCorreo(int corrcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                SiCorreoDTO correo;
                if (corrcodi != 0)//Ver detalles de un correo
                {
                    correo = (new CorreoAppServicio()).GetByIdSiCorreo(corrcodi);
                    List<CbArchivoenvioDTO> lstArchivos = servicioCombustible.ObtenerArchivosAdjuntados(corrcodi);

                    correo.Archivos = lstArchivos.Any() ? string.Join("/", lstArchivos.Select(x => x.Cbarchnombreenvio).OrderBy(x => x).ToList()) : "";

                }
                else // nuevo correo
                {
                    correo = new SiCorreoDTO();

                    //Agregamos firma en el pie de pagina
                    StringBuilder strHtml = new StringBuilder();
                    strHtml.Append("<br><br>");
                    strHtml.Append("<p><span style='font-size: 10pt; text - align: justify; '>Atentamente,</span></p>");
                    strHtml.Append("<p><span style='font-size: 13.3333px; text-align: justify;'><img src='https://tse1.mm.bing.net/th?id=OIP.oZNQ2yuNL0bHvM147UD3HgAAAA&amp;pid=Api&amp;P=0' alt='Logo Coes' width='127' height='66' /></span></p>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small;'><span style='font-size: 10pt;'><strong><span style='color: #15566b;'>Sub Direcci&oacute;n de Gesti&oacute;n de la Informaci&oacute;n</span></strong></span></div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'><span style='font-size: 8pt;'><span style='color: #15566b;'>D:</span>&nbsp;Av. Los Conquistadores N&deg; 1144, San Isidro, Lima - Per&uacute;</span></div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'><span style='font-size: 8pt;'><span style='font-size: 10.6667px;'><span style='color: #15566b;'>T:</span>&nbsp;+51 611 8585 - Anexo: 621 / 633</span></span></div>");
                    strHtml.Append("<div style='color: #222222; font-family: Arial, Helvetica, sans-serif; font-size: small; padding-left: 0px;'><a style='color: #1155cc;' title='p&aacute;gina web de COES' href='http://www.coes.org.pe/' target='_blank' data-saferedirecturl='https://www.google.com/url?q=http://www.coes.org.pe&amp;source=gmail&amp;ust=1673483286506000&amp;usg=AOvVaw1t-vqwH7I3O1tB4lBdSLLz'>www.coes.org.pe</a></div>");
                    correo.Corrcontenido = strHtml.ToString();
                }

                model.Correo = correo;
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
        /// Envia correos al agente
        /// </summary>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarCorreo(SiCorreoDTO correo)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //Direccion temporal donde se suben los archivos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;

                //Archivos
                List<string> listFiles = new List<string>();
                string files = correo.Archivos;
                if (!string.IsNullOrEmpty(files))
                    listFiles = files.Split('/').ToList();

                //Actualizo la plantilla
                servicioCombustible.EnviarMensajeAgentes(correo, base.UserName, path, listFiles);
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
        /// Carga archivos adjuntados
        /// </summary>
        /// <param name="chunks"></param>
        /// <param name="chunk"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload()
        {
            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesEnviarCorreos.RutaCorreo;
                    var file = Request.Files[0];
                    string fileName = path + file.FileName;
                    //this.NombreFile = fileName;

                    if (FileServer.VerificarExistenciaFile(null, file.FileName, path))
                    {
                        FileServer.DeleteBlob(file.FileName, path);
                    }

                    file.SaveAs(fileName);

                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Descarga los archivos adjuntados
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="corrcodi"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(string fileName, int corrcodi)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servicioCombustible.GetBufferArchivoAdjuntoCorreo(corrcodi, fileName);
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion
    }
}