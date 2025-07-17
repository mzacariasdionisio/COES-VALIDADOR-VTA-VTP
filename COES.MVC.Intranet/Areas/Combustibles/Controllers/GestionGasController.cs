using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Combustibles.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Combustibles.Controllers
{
    public class GestionGasController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CombustibleAppServicio servicioCombustible = new CombustibleAppServicio();
        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GestionGasController));
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

        /// <summary>
        /// Nombre del archivo
        /// </summary>
        public String NombreFile
        {
            get
            {
                return (Session[ConstantesFormato.SesionNombreArchivo] != null) ?
                    Session[ConstantesFormato.SesionNombreArchivo].ToString() : null;
            }
            set { Session[ConstantesFormato.SesionNombreArchivo] = value; }
        }

        #endregion

        #region Listado Envios

        /// <summary>
        /// Devuelve pagina pricipal del listado de envios
        /// </summary>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CombustibleGasModel model = new CombustibleGasModel();
            DateTime hoy = DateTime.Now;
            model.FechaInicio = (new DateTime((hoy.AddMonths(-1)).Year, (hoy.AddMonths(-1)).Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.FechaFin = (new DateTime(hoy.Year, hoy.Month, 1)).ToString(ConstantesAppServicio.FormatoMesAnio);
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesCombustibles.EstadoSolicitud : carpeta.Value;

            model.ListaEmpresas = servicioCombustible.ObtenerListadoEmpresas(ConstantesAppServicio.ParametroDefecto);
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Devuelve las cerpetas y el listado de envios
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BloqueEnvios(string empresas, int idEstado, string finicios, string ffins)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddSeconds(-1);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                model.ListadoEnvios = servicioCombustible.ObtenerListadoEnvios(empresas, idEstado, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesGaseosos);
                model.IdEstado = idEstado;
                model.HtmlCarpeta = servicioCombustible.GenerarHtmlCarpeta(empresas, idEstado, fechaInicio, fechaFin, "-1", (int)ConstantesCombustibles.Interfaz.Intranet);

                model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
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
        /// Devuelve el listado de cargos para cierto envío
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCargo(int idEnvio, int estado)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                model.ListadoEnvioLog = servicioCombustible.GetByCriteriaCbLogenvios(idEnvio).Where(x => x.Estenvcodi == estado).OrderByDescending(x => x.Logenvfeccreacion).ToList();
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
        /// Genera el archivo cargo a descargar
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarCargo(int idEnvio, int estado)
        {
            CombustibleGasModel model = new CombustibleGasModel();
            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                List<CbLogenvioDTO> lstCargoEnvios = servicioCombustible.GetByCriteriaCbLogenvios(idEnvio).Where(x => x.Estenvcodi == estado).OrderByDescending(x => x.Logenvfeccreacion).ToList();
                string nameFile = "cargo_" + envio.Cbenvcodi + "_" + envio.Emprnomb + "_" + envio.EstadoDesc + ".pdf";

                CombustiblePR31PdfHelper.GenerarReporteCargo(ruta, pathLogo, nameFile, lstCargoEnvios);
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

        /// <summary>
        /// Genera el archivo excel de listado de envíos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="idEstado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string empresas, string finicios, string ffins, int idEstado)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddDays(-1);

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                string estadoDesc = servicioCombustible.GetDescripcionExtEstado(idEstado);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "rptEnviosExistentes_" + estadoDesc + ".xlsx";

                servicioCombustible.GenerarExportacionEnvios(ruta, pathLogo, empresas, fechaInicio, fechaFin, idEstado, nameFile);
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

        /// <summary>
        /// Exporta archivo pdf, excel, csv, ...
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

        #region principal

        /// <summary>
        /// Muestra el formaulario para el envio de combustibles liquido
        /// </summary>
        /// <returns></returns>
        public ActionResult EnvioCombustible(int? idEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (idEnvio.GetValueOrDefault(0) <= 0) return base.RedirectToHomeDefault();

            CombustibleGasModel model = new CombustibleGasModel();
            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio.Value);
            List<CbEnvioCentralDTO> lstCentralesParticipantes = servicioCombustible.ListCbEnvioCentralsByEnvio(envio.Cbenvcodi).OrderBy(x => x.Equinomb).ToList();

            model.IdEnvio = idEnvio ?? 0;
            model.Envio = envio;
            model.OtrosUsuariosEmpresa = ObtenerCCcorreosAgente(envio.Emprcodi, envio.Cbverusucreacion); //usuario de ultima version que tenga @ 
            model.EsEnvioDeAsignacion = envio.Estenvcodi == ConstantesCombustibles.EstadoSolicitudAsignacion ? 1 : (envio.Estenvcodi == ConstantesCombustibles.EstadoAsignado ? 1 : 0);
            model.DiasParaSubsanar = (DateTime.Now.AddDays(3)).ToString(Constantes.FormatoFecha);
            model.NumCentralesEnF3 = lstCentralesParticipantes.Count();
            model.CostoCombustibleGaseoso = servicioCombustible.ObtenerCostoCombustibleActual();
            model.TipoCentral = envio.Cbenvtipocentral;
            model.TipoOpcion = envio.TipoOpcionAsignado;
            model.MesVigencia = envio.Cbenvfechaperiodo.Value.ToString(ConstantesAppServicio.FormatoMes);

            if (model.TienePermisoAdmin)
            {
                model.EsEditable = envio.EsEditableIntranet ? 1 : 0;
                model.UsarFechaSistemaManual = ConfigurationManager.AppSettings[ConstantesCombustibles.KeyFlagPR31HoraSistemaManual] == "S";
            }

            //eliminar los archivos temporales de vista previa
            servicioCombustible.EliminarArchivosReporte();

            return View(model);
        }

        /// <summary>
        /// Duarda la fecha del sistema para un envío
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="fechaSistema"></param>
        /// <returns></returns>
        public JsonResult GuardarFechaSistema(int idEnvio, string fechaSistema)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaHora = DateTime.ParseExact(fechaSistema, ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);

                if (servicioCombustible.GetByIdCbEnvio(idEnvio).Cbenvfecsistema >= fechaHora)
                    throw new ArgumentException("La fecha de sistema manual debe ser posterior a " + servicioCombustible.GetByIdCbEnvio(idEnvio).Cbenvfecsistema.Value.ToString(ConstantesAppServicio.FormatoFechaFull2));

                servicioCombustible.GuardarFechaSistema(idEnvio, base.UserName, fechaHora);
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
        /// Permite descargar formato3 e informes sustentatorios en archivo comprimido
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="mesVigencia"></param>
        /// <returns></returns>
        public JsonResult ExportarFormato(int idEnvio)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //DateTime fecha = DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);

                servicioCombustible.ExportarFormato3InfSustXEnvio(GetCurrentCarpetaSesion(), idEnvio, 0, base.UserName, out string nameFile);
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

        /// <summary>
        /// exportar comprimido
        /// </summary>
        /// <returns></returns>
        public virtual FileResult ExportarZip()
        {
            string nombreArchivo = Request["file_name"];

            string modulo = ConstantesCombustibles.ModuloArchivosXEnvio;
            string pathDestino = ConstantesCombustibles.FolderRaizPR31Gaseoso + "Temporal_" + modulo + @"/" + ConstantesCombustibles.NombreArchivosZip;
            string pathAlternativo = servicioCombustible.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        #endregion

        #region Observar envio

        /// <summary>
        /// Obrserva un envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="fechaMaxRpta"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarObservacion(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                if (string.IsNullOrEmpty(formulario.FechaMaxRpta))
                {
                    throw new ArgumentException("No ingresó la fecha máxima.");
                }

                DateTime fechaMaxR = DateTime.ParseExact(formulario.FechaMaxRpta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                //obtenemos lista de corrreos cc
                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(formulario.CorreosCc))
                {
                    listaCorreoCCagente = formulario.CorreosCc.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbverusucreacion);

                //envio
                CbEnvioDTO objEnvioGuardado = servicioCombustible.ObservarEnvioPr31Gaseoso(formulario.IdEnvio, base.UserName, fechaMaxR, listaCorreoCCagente
                                                            , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa);

                if (objEnvioGuardado.Cbenvcodi > 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi, false, listaArchivoFisico);
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

            return Json(model);
        }

        #endregion

        #region Aprobar envio

        /// <summary>
        /// Aprueba de envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAprobacion(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(formulario.Correo.PlanticorreosCc))
                {
                    listaCorreoCCagente = formulario.Correo.PlanticorreosCc.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbenvususolicitud);

                //setear estado "Aprobado" a las centrales
                foreach (var objCentral in formulario.ListaFormularioCentral) objCentral.Cbcentestado = "A";

                //envio
                CbEnvioDTO objEnvioGuardado = servicioCombustible.AprobarEnvioPr31Gaseoso(formulario.IdEnvio, base.UserName, listaCorreoCCagente
                                                        , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa);

                if (objEnvioGuardado.Cbenvcodi > 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi, false, listaArchivoFisico);
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

            return Json(model);
        }

        #endregion

        #region desaprobar envio

        /// <summary>
        /// Desaprueba envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarDesaprobacion(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(formulario.Correo.PlanticorreosCc))
                {
                    listaCorreoCCagente = formulario.Correo.PlanticorreosCc.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbenvususolicitud);

                //setear estado "Desaprobado" a las centrales
                foreach (var objCentral in formulario.ListaFormularioCentral) objCentral.Cbcentestado = "D";

                //envio
                CbEnvioDTO objEnvioGuardado = servicioCombustible.DesaprobarEnvioPr31Gaseoso(formulario.IdEnvio, base.UserName, formulario.Correo.Plantcontenido, listaCorreoCCagente
                                                        , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa);

                if (objEnvioGuardado.Cbenvcodi > 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi, false, listaArchivoFisico);
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

            return Json(model);
        }

        #endregion

        #region Aprobacion Parcial

        /// <summary>
        /// Aprueba parcialmente un envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAprobacionParcial(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(formulario.CorreosCc))
                {
                    listaCorreoCCagente = formulario.CorreosCc.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                List<int> listaCentralesAprobadas = new List<int>();
                if (!string.IsNullOrEmpty(formulario.LstCentralesAprob))
                {
                    listaCentralesAprobadas = formulario.LstCentralesAprob.Split(',').Select(x => Convert.ToInt32(x.Trim())).ToList();

                    //setear estado "Aprobado" a las centrales
                    foreach (var objCentral in formulario.ListaFormularioCentral)
                    {
                        if (listaCentralesAprobadas.Contains(objCentral.Equicodi))
                            objCentral.Cbcentestado = "A";
                    }
                }
                List<int> listaCentralesDesaprobadas = new List<int>();
                if (!string.IsNullOrEmpty(formulario.LstCentralesDesaprob))
                {
                    listaCentralesDesaprobadas = formulario.LstCentralesDesaprob.Split(',').Select(x => Convert.ToInt32(x.Trim())).ToList();

                    //setear estado "Aprobado" a las centrales
                    foreach (var objCentral in formulario.ListaFormularioCentral)
                    {
                        if (listaCentralesDesaprobadas.Contains(objCentral.Equicodi))
                            objCentral.Cbcentestado = "D";
                    }
                }

                if (!listaCentralesAprobadas.Any())
                    throw new Exception("Para aprobar parcialmente un envío debe existir centrales aprobadas. Corregir.");

                if (!listaCentralesDesaprobadas.Any())
                    throw new Exception("Para aprobar parcialmente un envío debe existir centrales desaprobadas. Corregir.");

                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbenvususolicitud);

                //envio
                CbEnvioDTO objEnvioGuardado = servicioCombustible.AprobarParcialmenteEnvioPr31Gaseoso(formulario.IdEnvio, base.UserName, formulario.Plantcontenido
                                                , formulario.ListaFormularioCentral, formulario.FormularioSustento, listaCorreoCCagente
                                                , otrosUsuariosEmpresa);

                if (objEnvioGuardado.Cbenvcodi > 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi, false, listaArchivoFisico);
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

            return Json(model);
        }

        #endregion

        #region Asignar costo

        /// <summary>
        /// Asigna un costo de combustible gaseoso
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="correo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAsignacion(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                //obtenemos lista de corrreos cc
                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(formulario.Correo.PlanticorreosCc))
                {
                    listaCorreoCCagente = formulario.Correo.PlanticorreosCc.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbenvususolicitud);

                //setear estado "Aprobado" a las centrales
                foreach (var objCentral in formulario.ListaFormularioCentral) objCentral.Cbcentestado = "G";

                servicioCombustible.AsignarEnvioPr31Gaseoso(formulario.IdEnvio, base.UserName, listaCorreoCCagente, formulario.ListaFormularioCentral, otrosUsuariosEmpresa);
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

        #region Ampliar Plazo 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatosEnvio(int idEnvio)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                model.Envio = envio;

                DateTime unoDelSiguienteMes = new DateTime(envio.Cbenvfecsolicitud.AddMonths(1).Year, envio.Cbenvfecsolicitud.AddMonths(1).Month, 1);
                DateTime finMes = unoDelSiguienteMes.AddDays(-1);
                model.FechaFin = finMes.ToString(Constantes.FormatoFecha);

                //solo permite ampliar si hoy es <= fin de mes (respecto ala fecha solicitud)
                model.PermiteAmpliacion = finMes.Date >= DateTime.Today.Date ? 1 : 0;

                model.FechaPlazo = model.PermiteAmpliacion == 1 ? DateTime.Today.AddDays(1).ToString(Constantes.FormatoFecha) : finMes.ToString(Constantes.FormatoFecha);
                model.HoraPlazo = DateTime.Now.Hour * 2 + 1;  //value de Hora actual en 30min
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
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAmpliacion(string fecha, int hora, int idEnvio)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime dateAmpliacion = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaAmpliada = dateAmpliacion.AddMinutes(hora * 30);

                //Validamos que la fecha estre entre la fecha solicitud y fin de mes
                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                DateTime fechaSolicitud = envio.Cbenvfecsolicitud;
                DateTime unoDelSiguienteMes = new DateTime(fechaSolicitud.AddMonths(1).Year, fechaSolicitud.AddMonths(1).Month, 1);
                DateTime fechaFinMes = unoDelSiguienteMes.AddMinutes(-1);

                int result1 = DateTime.Compare(fechaSolicitud, fechaAmpliada);
                int result2 = DateTime.Compare(fechaAmpliada, fechaFinMes);

                if (result1 > 0 || result2 > 0)
                {
                    throw new ArgumentException("La fecha de ampliación debe estar entre el rango: [" + fechaSolicitud + " , " + fechaFinMes + "].");
                }
                //busca otros usuarios (diferentes al usuario de solicitud) de cierta emrpesa                
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(envio.Emprcodi, envio.Cbenvususolicitud);

                servicioCombustible.AmpliarPlazoEnvioPr31Gaseoso(idEnvio, base.UserName, fechaAmpliada, otrosUsuariosEmpresa);
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

        #region Nuevo envío

        [HttpPost]
        public JsonResult ValidarNuevoEnvio(string mesVigencia, int idEmpresa, string tipoAccionForm, string usuarioGenerador, int esPrimeraCarga)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaVigencia = DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);

                if (tipoAccionForm == "NE") //realizar envio
                {
                    //si tiene check entonces validar solo en aprobados, sino validar en solicitud
                    int estenvcodi = esPrimeraCarga == 1 ? ConstantesCombustibles.EstadoAprobado : ConstantesCombustibles.EstadoSolicitud;

                    //if (servicioCombustible.ExisteSolicitudXTipoCombustibleYVigenciaYTipocentralCbEnvio(0, idEmpresa, ConstantesCombustibles.EstcomcodiGas, fechaVigencia
                    //                                            , "E", estenvcodi, out int idEnvioExistente))
                    //    throw new ArgumentException("Ya existe una solicitud para el periodo seleccionado.");
                }
                else
                {
                    int estenvcodi = ConstantesCombustibles.EstadoObservado;
                    if (!servicioCombustible.ExisteSolicitudXTipoCombustibleYVigenciaYTipocentralCbEnvio(0, idEmpresa, ConstantesCombustibles.EstcomcodiGas, fechaVigencia
                                                                , ConstantesAppServicio.ParametroDefecto, estenvcodi, out int idEnvioExistente))
                        throw new ArgumentException("No existe envío observado para el periodo seleccionado.");

                    model.IdEnvio = idEnvioExistente;
                }

                var listaTipoCentral = servicioCombustible.ListarTipoCentralXEmpresa(new List<int>() { idEmpresa }).ToList();
                model.FlagCentralExistente = listaTipoCentral.Find(x => x.String1 == ConstantesCombustibles.CentralExistente) != null ? 1 : 0;
                model.FlagCentralNuevo = listaTipoCentral.Find(x => x.String1 == ConstantesCombustibles.CentralNueva) != null ? 1 : 0;

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
        public JsonResult GrabarDatosCombustible(string data)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                DateTime fechaVigencia = DateTime.ParseExact(formulario.MesVigencia, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                string tipoCarga = formulario.TipoAccionForm == "NE" ? "IPC" : "I";

                CbEnvioDTO objEnvioGuardado = new CbEnvioDTO();
                if (formulario.TipoAccionForm == "NE")
                {
                    if (servicioCombustible.ExisteSolicitudXTipoCombustibleYVigenciaYTipocentralCbEnvio(formulario.IdEnvio, formulario.IdEmpresa, ConstantesCombustibles.EstcomcodiGas, fechaVigencia
                                                                    , formulario.TipoCentral, -1, out int idEnvioExistente))
                        throw new ArgumentException("Ya existe una solicitud para el periodo seleccionado.");

                    if (formulario.EsPrimeraCarga == 1)
                    {
                        //setear estado "Aprobado" a las centrales
                        foreach (var objCentral in formulario.ListaFormularioCentral) objCentral.Cbcentestado = "A";

                        //registro aprobado de carga inicial, no visible en Extranet
                        objEnvioGuardado = servicioCombustible.RealizarSolicitudCargaInicialCombustibleGas(formulario.IdEnvio, fechaVigencia, formulario.IdEmpresa, formulario.TipoCentral
                                                                    , base.UserName, "A", "P"
                                                                    , formulario.ListaFormularioCentral, formulario.FormularioSustento, tipoCarga
                                                                    , ConstantesCombustibles.GuardadoOficial, formulario.DescVersion, formulario.CondicionEnvioPrevioTemporal, (int)ConstantesCombustibles.Interfaz.Extranet);
                    }
                    else
                    {
                        //registro nueva solicitud de un usuario generador
                        string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(formulario.IdEmpresa, formulario.UsuarioGenerador);
                        objEnvioGuardado = servicioCombustible.RealizarSolicitudCostoCombustibleGas(formulario.IdEnvio, formulario.IdEmpresa, formulario.TipoCentral, ""
                                                                        , formulario.UsuarioGenerador, "A", "P"
                                                                        , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa, base.UserName, "NI"
                                                                        , ConstantesCombustibles.GuardadoOficial, formulario.DescVersion, formulario.CondicionEnvioPrevioTemporal, (int)ConstantesCombustibles.Interfaz.Extranet);
                    }
                }
                else
                {
                    //levantamiento de obs de un generador
                    string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(formulario.IdEmpresa, formulario.UsuarioGenerador);
                    objEnvioGuardado = servicioCombustible.RealizarSolicitudCostoCombustibleGas(formulario.IdEnvio, formulario.IdEmpresa, formulario.TipoCentral, ""
                                                                    , formulario.UsuarioGenerador, "A", "P"
                                                                    , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa, base.UserName, "SI"
                                                                    , ConstantesCombustibles.GuardadoOficial, formulario.DescVersion, formulario.CondicionEnvioPrevioTemporal, (int)ConstantesCombustibles.Interfaz.Extranet);
                }

                if (objEnvioGuardado.Cbenvcodi > 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi, false, listaArchivoFisico);
                }

                model.Resultado = "1";
                if (objEnvioGuardado.Cbenvcodi == 0)//no existe cambio
                    model.Resultado = "2";
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

        #region Envío - Información Formulario

        public ActionResult EnvioCombustibleGas(int? idEnvio, int? idEmpresa, string mesVigencia, string tipoCentral,
                                                string tipoAccionForm, string usuarioGenerador, int? esPrimeraCarga)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CombustibleGasModel model = new CombustibleGasModel();

            model.IdEnvio = idEnvio ?? 0;
            model.TipoCentral = tipoCentral;
            model.TipoCentralDesc = tipoCentral == ConstantesCombustibles.CentralExistente ? "Existente" : "Nueva";
            model.TipoAccion = tipoAccionForm;
            model.UsuarioGenerador = usuarioGenerador;
            model.EsPrimeraCarga = esPrimeraCarga ?? 0;

            if (idEnvio > 0)
            {
                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio.Value);
                //model.IdEmpresa = envio.Emprcodi;
                //model.Emprnomb = envio.Emprnomb;

                //model.TipoCentral = envio.Cbenvtipocentral;
                //model.TipoOpcion = envio.TipoOpcionAsignado;

                var listaEmpresa = new List<SiEmpresaDTO>();
                listaEmpresa.Add(new SiEmpresaDTO() { Emprcodi = envio.Emprcodi, Emprnomb = envio.Emprnomb });
                model.ListaEmpresas = listaEmpresa;
                model.IdEstado = envio.Estenvcodi;
                model.TienePermisoEditar = true;

                //Lista desplegable mes de vigencia
                var sMes = envio.Cbenvfechaperiodo.Value.ToString(ConstantesAppServicio.FormatoMes);
                model.ListaMes = new List<GenericoDTO>();
                model.ListaMes.Add(new GenericoDTO() { String1 = sMes, String2 = sMes });
            }
            else
            {
                if (idEmpresa.GetValueOrDefault(0) <= 0) return base.RedirectToHomeDefault();

                var objEmpresa = servicioCombustible.GetByIdSiEmpresa(idEmpresa.Value);
                model.ListaEmpresas = new List<SiEmpresaDTO>() { objEmpresa };
                model.IdEstado = ConstantesCombustibles.EstadoSolicitud;
                model.TienePermisoEditar = true;

                //Lista desplegable mes de vigencia
                DateTime fechaVigencia = DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                var sMes = fechaVigencia.ToString(ConstantesAppServicio.FormatoMes);
                model.ListaMes = new List<GenericoDTO>();
                model.ListaMes.Add(new GenericoDTO() { String1 = sMes, String2 = sMes });
            }

            return View(model);
        }

        /// <summary>
        /// Envia model para visualizacion de grilla excel de combustible
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, int idVersion, string tipoCentral, string tipoOpcion, string tipoAccionForm, string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                model.IdEnvio = idEnvio;
                model.IdEmpresa = idEmpresa;

                bool esIntranet = idEnvio > 0;
                if (tipoAccionForm != null) esIntranet = false;

                //envio
                model.EsIntranet = esIntranet;
                model.Envio = idEnvio > 0 ? servicioCombustible.GetByIdCbEnvio(idEnvio) : new CbEnvioDTO();
                DateTime fechaVigencia = idEnvio > 0 ? model.Envio.Cbenvfechaperiodo.Value : DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                //formato 3
                model.ListaFormularioCentral = servicioCombustible.ListarFormularioCentralByEnvio(esIntranet, idEnvio, idVersion, idEmpresa, tipoCentral, tipoOpcion
                                                , fechaVigencia, ConstantesCombustibles.ArchivosTotales, out bool yaExisteSolicitud);
                model.TienePermisoEditar = model.ListaFormularioCentral[0].EsEditable;

                //archivo
                bool esEnvioAsignado = model.Envio.Estenvcodi == ConstantesCombustibles.EstadoSolicitudAsignacion || model.Envio.Estenvcodi == ConstantesCombustibles.EstadoAsignado;
                if (!esEnvioAsignado)
                {
                    model.FormularioSustento = servicioCombustible.GetHandsonGasArchivoSustento(esIntranet, idEnvio, idVersion, false, null, new List<CbArchivoenvioDTO>()
                                                                        , model.ListaFormularioCentral[0].IncluirObservacion, model.ListaFormularioCentral[0].Estenvcodi);
                    model.FormularioSustento.EsEditable = model.ListaFormularioCentral[0].EsEditable && !model.FormularioSustento.SeccionCombustible.EsSeccionSoloLectura;
                    model.FormularioSustento.Readonly = !model.FormularioSustento.EsEditable;
                    model.FormularioSustento.EsEditableObs = model.ListaFormularioCentral[0].EsEditableObs;
                    model.FormularioSustento.IncluirObservacion = model.ListaFormularioCentral[0].IncluirObservacion;
                }
                model.ListadoVersiones = servicioCombustible.GetListaVersionesXEnvio(idEnvio, true);
                model.ListaLog = servicioCombustible.GetByCriteriaCbLogenvios(idEnvio);

                //copiar del fileserver al temporal para que se visualicen en pantalla
                if (idEnvio > 0)
                {
                    if (idVersion <= 0) idVersion = model.Envio.Cbvercodi;
                    servicioCombustible.CopiarArchivosFinalATemporal(GetCurrentCarpetaSesion(), idEnvio, idVersion);
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

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ActualizarGrilla(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                bool esIntranet = formulario.IdEnvio > 0;
                if (formulario.TipoAccionForm != null) esIntranet = false;

                model.ListaFormularioCentral = servicioCombustible.ActualizarHandsonFormulario(esIntranet, formulario.ListaFormularioCentral, false, formulario.Equicodi
                                            , formulario.CnpSeccion1, formulario.NumCol, formulario.NumColDesp, formulario.CnpSeccion0
                                            , formulario.TipoOpcionSeccion, formulario.MesAnteriorCentralNueva, ConstantesCombustibles.ArchivosTotales);

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
        public JsonResult ObtenerEnergiaXCentralYPeriodo(string mesConsulta, int idEmpresa, int equicodi, string tipoCentral)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                DateTime fechaPeriodo = DateTime.ParseExact(mesConsulta.Replace(" ", "-"), ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);

                DateTime fechaConsultaM96 = tipoCentral == ConstantesCombustibles.CentralNueva ? fechaPeriodo.AddMonths(-1) : fechaPeriodo.AddMonths(-2);

                model.ValorEnergia = servicioCombustible.ObtenerValorMedidoresCentral(fechaConsultaM96, idEmpresa, equicodi);

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
        public JsonResult ActualizarGrillaArchivo(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                bool esIntranet = formulario.IdEnvio > 0;
                if (formulario.TipoAccionForm != null) esIntranet = false;
                model.FormularioSustento = servicioCombustible.GetHandsonGasArchivoSustento(esIntranet, 0, 0, true, formulario.FormularioSustento.SeccionCombustible.ListaObs
                                                            , formulario.FormularioSustento.SeccionCombustible.ListaArchivo
                                                            , formulario.FormularioSustento.IncluirObservacion, formulario.FormularioSustento.Estenvcodi);
                model.FormularioSustento.EsEditable = formulario.FormularioSustento.EsEditable;
                model.FormularioSustento.Readonly = formulario.FormularioSustento.Readonly;
                model.FormularioSustento.EsEditableObs = formulario.FormularioSustento.EsEditableObs;
                model.FormularioSustento.IncluirObservacion = formulario.FormularioSustento.IncluirObservacion;

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
        public JsonResult ExportarFormularioEnvio(string data)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                servicioCombustible.GenerarArchivoExcelFormularioGaseosoEnvio(ruta, ConstantesCombustibles.NombreReporteFormularioEnvios, formulario.ListaFormularioCentral
                                                                        , ConstantesCombustibles.ArchivosTotales, true);

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
        /// Descarga el archivo excel generador por ExportarFormularioEnvio
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporteFormulario()
        {
            string nombreArchivo = ConstantesCombustibles.NombreReporteFormularioEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpPost]
        public JsonResult LeerFileUpExcelFormato3(string data)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                List<CeldaErrorCombustible> listaError = new List<CeldaErrorCombustible>();
                if (string.IsNullOrEmpty(this.NombreFile))
                {
                    listaError.Add(new CeldaErrorCombustible() { Mensaje = "No se importó el archivo" });
                }
                else
                {
                    //cargar formato
                    model.ListaFormularioCentral = servicioCombustible.ActualizarModeloConExcelTemporal(this.NombreFile, formulario.ListaFormularioCentral, ref listaError);

                    ToolsFormato.BorrarArchivo(this.NombreFile);
                }

                model.Resultado = !listaError.Any() ? "1" : "0";
                if (listaError.Any())
                {
                    if (listaError.Find(x => x.TipoError == 11) != null)
                    {
                        model.Mensaje = "El archivo importado pertenece a otra empresa, por favor corregir.";
                    }
                    else
                    {
                        model.Mensaje = "El archivo importado tiene diferente formato respecto a la plantilla a usar. Por favor corregir.";
                    }
                }
                model.ListaError = listaError;
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
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFormato3()
        {
            try
            {
                if (Request.Files.Count == 1)
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload;
                    var file = Request.Files[0];
                    string fileRandom = System.IO.Path.GetRandomFileName();
                    string fileName = ruta + fileRandom + "." + ConstantesFormato.ExtensionFile;
                    this.NombreFile = fileName;
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

        #endregion

        #region Envío - Documentos

        [HttpPost]
        public JsonResult VistaPreviaArchivo(int idConcepto, string fileName)
        {
            ArchivoGasModel model = new ArchivoGasModel();

            try
            {
                base.ValidarSesionUsuario();

                //Copiar archivo a reportes
                string pathTemporal = CombustibleAppServicio.GetPathSubcarpeta(ConstantesCombustibles.CarpetaTemporal);
                string pathSesionID = pathTemporal + GetCurrentCarpetaSesion() + @"/" + servicioCombustible.GetSubcarpetaEnvio(0, 0, idConcepto) + @"/";
                string pathDestino = ConstantesCombustibles.FolderReporte;

                FileServer.CopiarFileAlterFinalOrigen(pathSesionID, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                string url = pathDestino + fileName;

                model.Resultado = url;
                model.Detalle = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Graba los archivos directamente en la carpeta del id seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTemporal(int idConcepto)
        {
            ArchivoGasModel model = new ArchivoGasModel();

            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    servicioCombustible.UploadArchivoEnvioTemporal(GetCurrentCarpetaSesion(), idConcepto, file.FileName, file.InputStream, out string fileNamefisico);

                    return Json(new { success = true, nuevonombre = fileNamefisico }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(new { success = model.Resultado == "1", response = model }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListaArchivosXTemporal(int idConcepto)
        {
            ArchivoGasModel model = new ArchivoGasModel();

            try
            {
                base.ValidarSesionUsuario();

                model.ListaDocumentos = servicioCombustible.ListarArchivoEnvioTemporal(GetCurrentCarpetaSesion(), idConcepto);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        public virtual ActionResult DescargarArchivoTemporal(int idConcepto, string fileName, string esConf)
        {
            base.ValidarSesionUsuario();
            bool tienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            bool esDescargable = false;
            if (tienePermisoAdmin) esDescargable = true;
            if (!tienePermisoAdmin && esConf != "S") esDescargable = true;

            byte[] buffer = new byte[0];
            if (esDescargable)
                buffer = servicioCombustible.GetBufferArchivoEnvioTemporal(GetCurrentCarpetaSesion(), idConcepto, fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public JsonResult EliminarArchivoTemporal(int idConcepto, string fileName)
        {
            ArchivoGasModel model = new ArchivoGasModel();

            try
            {
                base.ValidarSesionUsuario();

                servicioCombustible.EliminarArchivoTemporal(GetCurrentCarpetaSesion(), idConcepto, fileName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar el archivo al explorador
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idConcepto"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(int idEnvio, int idVersion, int idConcepto, string fileName, string esConf)
        {
            base.ValidarSesionUsuario();
            bool tienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            bool esDescargable = false;
            if (tienePermisoAdmin) esDescargable = true;
            if (!tienePermisoAdmin && esConf != "S") esDescargable = true;

            byte[] buffer = new byte[0];
            if (esDescargable)
                buffer = servicioCombustible.GetBufferArchivoEnvioFinal(idEnvio, idVersion, idConcepto, fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetCurrentCarpetaSesion()
        {
            return base.UserName;
        }

        #endregion

    }
}