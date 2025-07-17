using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Equipamiento.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using System.Configuration;

namespace COES.MVC.Intranet.Areas.Equipamiento.Controllers
{
    public class FTAreasRevisionController : BaseController
    {
        readonly FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        #region Declaracion de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FTAdministradorController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private int IdOpcionEnvioFormatoAreas = ConstantesFichaTecnica.IdOptionModuloAreas;

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

        #region Pantalla principal

        /// <summary>
        /// Ventana principal
        /// </summary>
        /// <param name="carpeta"></param>
        /// <returns></returns>
        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAreaRevisionModel model = new FTAreaRevisionModel();
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesFichaTecnica.EstadoPendiente : carpeta.Value;

            model.ListaEmpresas = servFictec.ListarEmpresasExtranetFT();
            model.ListaEtapas = servFictec.ListFtExtEtapas();
            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);
            model.StrIdsAreaDelUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);

            return View(model);
        }


        /// <summary>
        /// Devuelve el listado de envios segun carpeta
        /// </summary>
        /// <param name="areasUsuario"></param>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idCarpeta"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BloqueEnviosAreas(string areasUsuario, string empresas, int ftetcodi, int idCarpeta, string finicios, string ffins)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (areasUsuario == "")
                    areasUsuario = "0"; //area que no existe, solo para evitar errrores

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                //Habilitacion segun rol usuario
                bool habilitadoEditarInformacion = true;

                model.ListadoEnvios = servFictec.ObtenerListadoEnviosEtapaParaAreas(areasUsuario, empresas, idCarpeta, fechaInicio, fechaFin, ftetcodi.ToString(), habilitadoEditarInformacion, base.UserEmail);

                model.HtmlCarpeta = servFictec.GenerarHtmlCarpetaAreas(areasUsuario, empresas, idCarpeta, fechaInicio, fechaFin, ftetcodi);
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
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesFichaTecnica.ModuloManualUsuario;
            string nombreArchivo = ConstantesFichaTecnica.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesFichaTecnica.FolderRaizFichaTecnicaModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }

        /// <summary>
        /// /Genera el archivo excel de listado de envíos
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string areasUsuario, string empresas, int ftetcodi, int idCarpeta, string finicios, string ffins)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormatoAreas);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "ReportePorArea_EnvíoFichaTécnica_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) +
                                                string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                //Habilitacion segun rol usuario
                bool habilitadoEditarInformacion = true;

                servFictec.GenerarExportacionEnviosAreas(areasUsuario, ruta, pathLogo, empresas, fechaInicio, fechaFin, idCarpeta, ftetcodi, nameFile, habilitadoEditarInformacion, base.UserEmail);
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
        public virtual FileResult Exportar(string file_name)
        {
            base.ValidarSesionUsuario();

            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes + file_name;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file_name);
        }
        #endregion

        #region Envios etapa Conexion, Integracion y Modificacion de FT (sin dar baja)

        /// <summary>
        /// Muestra la ventana principal para envios en etapas Conexión, Integración y Modificación de Ficha Técnica  (Sin dar baja)
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="accion"></param>
        /// <param name="codigoEmpresa"></param>
        /// <param name="codigoEtapa"></param>
        /// <param name="codigoProyecto"></param>
        /// <param name="codigoEquipos"></param>
        /// <returns></returns>
        public ActionResult AreaEnvioFormato(int codigoEnvio, int area, int? tipoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAreaRevisionModel model = new FTAreaRevisionModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            int versionUsado = objEnvio.FtevercodiTemporalIntranet;
            int estenvcodi = objEnvio.Estenvcodi;
            string estadoNombEnvio = objEnvio.Estenvnomb;
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSolicitud) 
            {
                //solicitud
                if (objEnvio.FtevercodiTemporalIntranetSolicitud > 0) 
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSolicitud;
                    estenvcodi = ConstantesFichaTecnica.EstadoSolicitud;
                    estadoNombEnvio = "Solicitud";
                }
            }
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSubsanacionObs)
            {
                //subsanacion
                if (objEnvio.FtevercodiTemporalIntranetSubsanacion > 0)
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSubsanacion;
                    estenvcodi = ConstantesFichaTecnica.EstadoSubsanacionObs;
                    estadoNombEnvio = "Subsanación de Observaciones";
                }
            }

            model.IdEnvio = codigoEnvio;
            model.IdVersion = versionUsado;

            //ExtEstadoEnvioDTO estadoCarpeta = 
            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.Ftprycodi = objEnvio.Ftprycodi ?? 0;
            model.Ftprynombre = objEnvio.Ftprynombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = estenvcodi;
            model.CarpetaEstadoDesc = estadoNombEnvio;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuestaRevisionArea(objEnvio, area);
            model.TipoEnvioDesc = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSolicitud ? "solicitado" : (objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSubsanacionObs ? "subsanado" : "");
            model.EsFTModificada = servFictec.VerificarSiParametrosFueronModificados(model.IdVersion);


            FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(area);
            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            string nombreAreaRev = areaUser.Faremnombre;
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.NombreAreasDelUsuario = nombreAreas;
            model.IdAreaRevision = area;
            model.NombreAreaRevision = nombreAreaRev;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            //Habilitacion segun rol usuario 
            bool habilitadoEditarInformacion = true;
            bool habilitadoDescargaConfidencial = TienePermisosConfidencialFT(Acciones.Confidencial);
            model.HabilitadoEditarInformacion = habilitadoEditarInformacion;
            model.HabilitadoDescargaConfidencial = habilitadoDescargaConfidencial;

            FtExtEnvioAreaDTO envioArea = servFictec.GetFtExtEnvioAreaByVersionYArea(versionUsado, area);
            //string tipo = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? "V": (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? "E" : "V");
            bool usuarioPerteneceAArea = servFictec.VerificarSiUsuarioPerteneceArea(base.UserEmail, envioArea.Faremcodi);
            string tipo = servFictec.ObtenerTipoOpcionDelDetalleEnvio(envioArea, habilitadoEditarInformacion, usuarioPerteneceAArea);
            model.IdCarpetaArea = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? 2 : (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? 1 : 1);
            model.TipoOpcion = tipo;

            int porcentajeAvance = servFictec.ObtenerPorcantajeAvanceParametrosFT(codigoEnvio, area, versionUsado, estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);
            model.PorcentajeAvanceRevision = porcentajeAvance;
            model.HtmlPorcentajeAvance = servFictec.ObtenerHtmlPorcentajeAvance(porcentajeAvance);

            return View(model);
        }

        /// <summary>
        /// Lista los equipos del envio 
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="versionEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEqConexIntegModifXEnvio(int codigoEnvio, int versionEnvio)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);
                model.LstFteeqcodis = string.Join(",", model.ListaEnvioEq.Select(x => x.Fteeqcodi));
                model.LstEnviosEqNombres = model.ListaEnvioEq.Any() ? string.Join(",", model.ListaEnvioEq.Select(x => x.Nombreelemento.Trim())) : "";

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
        /// Obtiene la informacion para armar la estructura izquierda de la tabla
        /// </summary>
        /// <param name="fteeqcodi"></param>
        /// <param name="areasUsuario"></param>
        /// <param name="nombAreasUsuario"></param>
        /// <returns></returns>
        public JsonResult ObtenerDatosFT(int fteeqcodi, string areasTotal, string nombAreasUsuario, int idAreaRevision, string nombAreaRevision)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormatoAreas);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                model.ReporteDatoXEq = servFictec.ObtenerFichaTreeXEnvioEq(fteeqcodi, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionParametrosAFT(fteeqcodi, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionAreasFT = servFictec.ObtenerDatosRevisionAreasFT(fteeqcodi, areasTotal, nombAreasUsuario, idAreaRevision, nombAreaRevision);

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
        public JsonResult GenerarFormatoConexIntegModif(string areasTotal, int idAreaRevision, int codigoEnvio, int estado, int versionEnvio, string codigoEquipos = "")
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servFictec.GenerarFormatoConexIntegModifXEnvioDerivacion(ruta, pathLogo, codigoEnvio, codigoEquipos, estado, ConstantesFichaTecnica.INTRANET, versionEnvio, areasTotal, idAreaRevision, out string fileName);

                model.Resultado = fileName;
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
        public JsonResult LeerFileUpExcelFormatoConexIntegModif(int codigoEnvio, int estado, int versionEnvio, int idAreaRevision, string areasTotal)
        {
            FTAdministradorModel model = new FTAdministradorModel();
            try
            {
                base.ValidarSesionJsonResult();
                List<FTReporteExcel> listaEqEnv = servFictec.ListarLecturaExcelFormatoConexIntegModif(codigoEnvio, this.NombreFile, ConstantesFichaTecnica.INTRANET, estado, versionEnvio, "", idAreaRevision, areasTotal, out string mensajes);
                if (mensajes != "")
                    throw new ArgumentException(mensajes);

                model.ListaRevisionImportacion = listaEqEnv;

                ToolsFormato.BorrarArchivo(this.NombreFile);

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
        /// Permite cargar los archivos
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFormatoConexIntegModif()
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

        /// <summary>
        /// Guarda el avance de la revision de areas
        /// </summary>
        /// <param name="modelWeb"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDatosRevArea(FTReporteExcelAreas modelWeb)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormatoAreas);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                //guardar cambios
                servFictec.GuardarAvanceRevisionAreas(modelWeb, ConstantesFichaTecnica.FormatoConexIntegModif);

                int idEnvio = modelWeb.Ftenvcodi;
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(idEnvio);
                int idVersion = modelWeb.Ftevercodi;

                //Eliminar los archivos residuales (no forman parte de los archivos activos) FALTA-PENDIENTE
                List<FtExtEnvioArchivoDTO> listaArchivoPorVersion = servFictec.ListFtExtEnvioArchivosByVersionAreaYEquipo(modelWeb.Ftevercodi, modelWeb.Faremcodi, modelWeb.Fteeqcodi);
                List<string> listaArchivoValido = listaArchivoPorVersion.Select(x => x.Ftearcnombrefisico).Distinct().OrderBy(x => x).ToList();
                //servFictec.QuitarArchivosResidualesRevAreas(idEnvio, idVersion, listaArchivoValido); 

                int area = modelWeb.Faremcodi;
                FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(area);
                string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
                string nombreAreaRev = areaUser.Faremnombre;
                int porcentajeAvance = servFictec.ObtenerPorcantajeAvanceParametrosFT(idEnvio, area, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);
                model.PorcentajeAvanceRevision = porcentajeAvance;
                model.HtmlPorcentajeAvance = servFictec.ObtenerHtmlPorcentajeAvance(porcentajeAvance);

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
        /// Devuelve el listado de errores encontrados
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="idVersion"></param>
        /// <param name="idArea"></param>
        /// <param name="tipoFormato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarErroresRevAreas(int idEnvio, int idVersion, int idArea, int tipoFormato)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                //guardar cambios
                base.ValidarSesionJsonResult();
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(idEnvio);


                FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(idArea);
                string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
                string nombreAreaRev = areaUser.Faremnombre;

                List<ErrorRevisionAreas> lstErrores = new List<ErrorRevisionAreas>();

                if (tipoFormato == ConstantesFichaTecnica.FormatoConexIntegModif)
                    lstErrores = servFictec.ObtenerListadoErroresRevAreasParametrosFT(idEnvio, idArea, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);

                if (tipoFormato == ConstantesFichaTecnica.FormatoOperacionComercial || tipoFormato == ConstantesFichaTecnica.FormatoBajaModoOperacion)
                    lstErrores = servFictec.ObtenerListadoErroresRevAreasContenido(idEnvio, idArea, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev, tipoFormato);

                model.ListadoErroresRevArea = lstErrores;

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
        /// Evalua si no existen errores y realiza envio final de la revision
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="idVersion"></param>
        /// <param name="idArea"></param>
        /// <param name="tipoFormato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarRevisionFinal(int idEnvio, int idVersion, int idArea, int tipoFormato)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                //guardar cambios
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormatoAreas);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(idEnvio);


                FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(idArea);
                string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
                string nombreAreaRev = areaUser.Faremnombre;

                //Valido la existencia de errores
                List<ErrorRevisionAreas> lstErroresEncontrados = new List<ErrorRevisionAreas>();
                if (tipoFormato == ConstantesFichaTecnica.FormatoConexIntegModif)
                    lstErroresEncontrados = servFictec.ObtenerListadoErroresRevAreasParametrosFT(idEnvio, idArea, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);

                if (tipoFormato == ConstantesFichaTecnica.FormatoOperacionComercial || tipoFormato == ConstantesFichaTecnica.FormatoBajaModoOperacion)
                    lstErroresEncontrados = servFictec.ObtenerListadoErroresRevAreasContenido(idEnvio, idArea, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev, tipoFormato);

                if (lstErroresEncontrados.Any()) //debe mostrar los erroes encontrados
                {
                    model.ListadoErroresRevArea = lstErroresEncontrados;
                    model.Resultado = "0";
                }
                else //envia la revision
                {
                    servFictec.EnviarRevisionFinal(objEnvio, idVersion, idArea, base.UserName);
                    model.Resultado = "1";
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
        /// Graba los archivos directamente en la carpeta del id seleccionado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadTemporal(int idEnvio, int idVersion, int idElemento, string tipoArchivo)
        {
            FTArchivoModel model = new FTArchivoModel();

            try
            {
                base.ValidarSesionUsuario();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    DateTime fechaAhora = DateTime.Now;
                    servFictec.UploadArchivoEnvioTemporal(idEnvio, idVersion, idElemento, tipoArchivo, file.FileName,
                                                                file.InputStream, fechaAhora, out string fileNamefisico);

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

        #endregion

        #region Envío de Formato Intranet para Operacion Comercial

        /// <summary>
        /// muestra la ventana principal para envios en operacion comercial
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public ActionResult AreaEnvioFormatoOperacionComercial(int codigoEnvio, int area, int? tipoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAreaRevisionModel model = new FTAreaRevisionModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            int versionUsado = objEnvio.FtevercodiTemporalIntranet;
            int estenvcodi = objEnvio.Estenvcodi;
            string estadoNombEnvio = objEnvio.Estenvnomb;
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSolicitud)
            {
                //solicitud
                if (objEnvio.FtevercodiTemporalIntranetSolicitud > 0)
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSolicitud;
                    estenvcodi = ConstantesFichaTecnica.EstadoSolicitud;
                    estadoNombEnvio = "Solicitud";
                }
            }
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSubsanacionObs)
            {
                //subsanacion
                if (objEnvio.FtevercodiTemporalIntranetSubsanacion > 0)
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSubsanacion;
                    estenvcodi = ConstantesFichaTecnica.EstadoSubsanacionObs;
                    estadoNombEnvio = "Subsanación de Observaciones";
                }
            }

            model.IdEnvio = codigoEnvio;
            model.IdVersion = versionUsado;

            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.Ftprycodi = objEnvio.Ftprycodi ?? 0;
            model.Ftprynombre = objEnvio.Ftprynombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = estenvcodi;
            model.CarpetaEstadoDesc = estadoNombEnvio;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuestaRevisionArea(objEnvio, area);
            model.TipoEnvioDesc = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSolicitud ? "solicitado" : (objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSubsanacionObs ? "subsanado" : "");

            FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(area);
            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            string nombreAreaRev = areaUser.Faremnombre;
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.NombreAreasDelUsuario = nombreAreas;
            model.IdAreaRevision = area;
            model.NombreAreaRevision = nombreAreaRev;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            //Habilitacion segun rol usuario 
            bool habilitadoEditarInformacion = true;
            bool habilitadoDescargaConfidencial = TienePermisosConfidencialFT(Acciones.Confidencial);
            model.HabilitadoEditarInformacion = habilitadoEditarInformacion;
            model.HabilitadoDescargaConfidencial = habilitadoDescargaConfidencial;

            FtExtEnvioAreaDTO envioArea = servFictec.GetFtExtEnvioAreaByVersionYArea(versionUsado, area);
            //string tipo = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? "V" : (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? "E" : "V");
            bool usuarioPerteneceAArea = servFictec.VerificarSiUsuarioPerteneceArea(base.UserEmail, envioArea.Faremcodi);
            string tipo = servFictec.ObtenerTipoOpcionDelDetalleEnvio(envioArea, habilitadoEditarInformacion, usuarioPerteneceAArea);
            model.IdCarpetaArea = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? 2 : (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? 1 : 1);
            model.TipoOpcion = tipo;

            int porcentajeAvance = servFictec.ObtenerPorcantajeAvanceContenido(codigoEnvio, area, versionUsado, estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);
            model.PorcentajeAvanceRevision = porcentajeAvance;
            model.HtmlPorcentajeAvance = servFictec.ObtenerHtmlPorcentajeAvance(porcentajeAvance);

            return View(model);
        }

        /// <summary>
        ///  Lista filas de requisito
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="versionEnvio"></param>
        /// <param name="areasUsuario"></param>
        /// <param name="nombAreasUsuario"></param>
        /// <param name="idAreaRevision"></param>
        /// <param name="nombAreaRevision"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRequisitoXEnvio(int codigoEnvio, int versionEnvio, string areasTotal, string nombAreasUsuario, int idAreaRevision, string nombAreaRevision)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

                model.Evento = servFictec.GetByIdFtExtEvento(objEnvio.Ftevcodi ?? 0);
                model.ListaReqEvento = servFictec.ListarRequisitoXEnvioVersion(objEnvio.Ftevcodi ?? 0, versionEnvio, ConstantesFichaTecnica.INTRANET);
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);
                model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionContenidoReq(versionEnvio, ConstantesFichaTecnica.INTRANET);
                model.ListaRevisionAreasContenido = servFictec.ObtenerDatosRevisionAreasContenido(versionEnvio, areasTotal, nombAreasUsuario, idAreaRevision, nombAreaRevision);

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
        /// Guarda el avance de la revision de areas
        /// </summary>
        /// <param name="modelWeb"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarDatosRevAreaContenido(FTReporteExcelAreas modelWeb, int tipoFormato)
        {
            FTAreaRevisionModel model = new FTAreaRevisionModel();

            try
            {
                base.ValidarSesionJsonResult();
                bool tienePermisoAdmin = base.VerificarAccesoAccionXOpcion(Acciones.Grabar, base.UserName, this.IdOpcionEnvioFormatoAreas);
                if (!tienePermisoAdmin) throw new Exception(Constantes.MensajePermisoNoValido);

                if (tipoFormato != ConstantesFichaTecnica.FormatoOperacionComercial && tipoFormato != ConstantesFichaTecnica.FormatoBajaModoOperacion) throw new ArgumentException("Tipo de formato desconocido. No es permitido guardar el envío.");

                servFictec.GuardarAvanceRevisionAreas(modelWeb, tipoFormato);


                int idEnvio = modelWeb.Ftenvcodi;
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(idEnvio);
                int idVersion = modelWeb.Ftevercodi;

                ////Eliminar los archivos residuales (no forman parte de los archivos activos) FALTA PENDIENTE
                //List<FtExtEnvioArchivoDTO> listaArchivoPorVersion = servFictec.ListFtExtEnvioArchivosByVersionAreas(idVersion.ToString());
                //List<string> listaArchivoValido = listaArchivoPorVersion.Select(x => x.Ftearcnombrefisico).Distinct().OrderBy(x => x).ToList();
                //servFictec.QuitarArchivosResidualesVersion(idEnvio, idVersion, listaArchivoValido);

                //Actualizo porcentaje de avance
                int area = modelWeb.Faremcodi;
                FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(area);
                string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
                string nombreAreaRev = areaUser.Faremnombre;
                int porcentajeAvance = servFictec.ObtenerPorcantajeAvanceContenido(idEnvio, area, idVersion, objEnvio.Estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);
                model.PorcentajeAvanceRevision = porcentajeAvance;
                model.HtmlPorcentajeAvance = servFictec.ObtenerHtmlPorcentajeAvance(porcentajeAvance);

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
        public JsonResult GenerarFormatoRevisionContenido(int codigoEnvio, string areasTotal, int idAreaRevision)
        {
            FTAdministradorModel model = new FTAdministradorModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio); //tipo formato: operación comercial o dar de baja
                servFictec.GenerarFormatoRevisionContenido(ruta, pathLogo, codigoEnvio, objEnvioAct.Ftenvtipoformato, areasTotal, idAreaRevision, out string fileName);

                model.Resultado = fileName;
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

        #region Envío de Formato Extranet para Dar de baja a modo de operación

        /// <summary>
        /// Ventana principal para envios en dar de baja
        /// </summary>
        /// <param name="codigoEnvio"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public ActionResult AreaEnvioFormatoBajaModoOperacion(int codigoEnvio, int area, int? tipoEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            FTAreaRevisionModel model = new FTAreaRevisionModel();

            //envio realizado
            FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

            int versionUsado = objEnvio.FtevercodiTemporalIntranet;
            int estenvcodi = objEnvio.Estenvcodi;
            string estadoNombEnvio = objEnvio.Estenvnomb;
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSolicitud)
            {
                //solicitud
                if (objEnvio.FtevercodiTemporalIntranetSolicitud > 0)
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSolicitud;
                    estenvcodi = ConstantesFichaTecnica.EstadoSolicitud;
                    estadoNombEnvio = "Solicitud";
                }
            }
            if (tipoEnvio == ConstantesFichaTecnica.EstadoSubsanacionObs)
            {
                //subsanacion
                if (objEnvio.FtevercodiTemporalIntranetSubsanacion > 0)
                {
                    versionUsado = objEnvio.FtevercodiTemporalIntranetSubsanacion;
                    estenvcodi = ConstantesFichaTecnica.EstadoSubsanacionObs;
                    estadoNombEnvio = "Subsanación de Observaciones";
                }
            }

            model.IdEnvio = codigoEnvio;
            model.IdVersion = versionUsado;

            model.Emprcodi = objEnvio.Emprcodi;
            model.Emprnomb = objEnvio.Emprnomb;
            model.Ftetcodi = objEnvio.Ftetcodi;
            model.Ftetnombre = objEnvio.Ftetnombre;
            model.EnvioTipoFormato = objEnvio.Ftenvtipoformato;

            model.IdEstado = objEnvio.Estenvcodi;
            model.CarpetaEstadoDesc = estadoNombEnvio;
            model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuestaRevisionArea(objEnvio, area);
            model.TipoEnvioDesc = objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSolicitud ? "solicitado" : (objEnvio.Estenvcodi == ConstantesFichaTecnica.EstadoSubsanacionObs ? "subsanado" : "");

            FtExtCorreoareaDTO areaUser = servFictec.GetByIdFtExtCorreoarea(area);
            string strAreasUsuario = servFictec.ObtenerIdAreaDelUsuario(base.UserName, base.UserEmail, out string nombreAreas);
            string nombreAreaRev = areaUser.Faremnombre;
            model.StrIdsAreaDelUsuario = strAreasUsuario;
            model.NombreAreasDelUsuario = nombreAreas;
            model.IdAreaRevision = area;
            model.NombreAreaRevision = nombreAreaRev;
            model.StrIdsAreaTotales = servFictec.ObtenerIdAreaTotales();

            //Habilitacion segun rol usuario 
            bool habilitadoEditarInformacion = true;
            bool habilitadoDescargaConfidencial = TienePermisosConfidencialFT(Acciones.Confidencial);
            model.HabilitadoEditarInformacion = habilitadoEditarInformacion;
            model.HabilitadoDescargaConfidencial = habilitadoDescargaConfidencial;

            FtExtEnvioAreaDTO envioArea = servFictec.GetFtExtEnvioAreaByVersionYArea(versionUsado, area);
            //string tipo = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? "V" : (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? "E" : "V");
            bool usuarioPerteneceAArea = servFictec.VerificarSiUsuarioPerteneceArea(base.UserEmail, envioArea.Faremcodi);
            string tipo = servFictec.ObtenerTipoOpcionDelDetalleEnvio(envioArea, habilitadoEditarInformacion, usuarioPerteneceAArea);
            model.IdCarpetaArea = envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrAtendido ? 2 : (envioArea.Envarestado == ConstantesFichaTecnica.EstadoStrPendiente ? 1 : 1);
            model.TipoOpcion = tipo;

            int porcentajeAvance = servFictec.ObtenerPorcantajeAvanceContenido(codigoEnvio, area, versionUsado, estenvcodi, strAreasUsuario, nombreAreas, nombreAreaRev);
            model.PorcentajeAvanceRevision = porcentajeAvance;
            model.HtmlPorcentajeAvance = servFictec.ObtenerHtmlPorcentajeAvance(porcentajeAvance);


            return View(model);
        }

        #endregion

    }
}