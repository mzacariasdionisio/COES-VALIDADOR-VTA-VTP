using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Combustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Combustibles;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Combustibles.Controllers
{
    public class GestionController : BaseController
    {
        readonly CombustibleAppServicio servicio = new CombustibleAppServicio();
        readonly SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(GestionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #region Principal

        /// <summary>
        /// Index para gestor de envio de combustibles líquidos y carbón
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CombustibleModel model = new CombustibleModel();
            model.FechaInicio = DateTime.Now.AddDays(-30).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFecha);
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesCombustibles.EstadoSolicitud : carpeta.Value;

            servicio.ListarEmpresasYCentralFormatoPr31((int)ConstantesCombustibles.Interfaz.Intranet, out List<SiEmpresaDTO> listaEmpAll, out List<EqEquipoDTO> listaCentralAll);

            model.ListaEmpresas = listaEmpAll;
            model.ListaCentral = listaCentralAll;

            return View(model);
        }

        /// <summary>
        /// Vista parcial para mostrar paginado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="tipocombustibles"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string empresas, int estado, string centrales, string finicios, string ffins)
        {
            empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;
            DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);

            CombustibleModel model = new CombustibleModel();
            model.IndicadorPagina = false;

            if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

            int nroRegistros = servicio.GetTotalEnvio(empresas, centrales, estado, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesLiquidosYSolidos, "-1");

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial de la lista de Envio de Combustibles
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estados"></param>
        /// <param name="nroPaginas"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <param name="tipocombustibles"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Lista(string empresas, int nroPaginas, string centrales, string finicios, string ffins, int idEstado)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                string url = Url.Content("~/");
                servicio.GenerarHtmlEnvio(url, empresas, centrales, idEstado, fechaInicio, fechaFin, nroPaginas, Constantes.PageSize
                                            , out string htmlCarpeta, out string htmlListado, (int)ConstantesCombustibles.Interfaz.Intranet);
                model.HtmlCarpeta = htmlCarpeta;
                model.HtmlListado = htmlListado;
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
        /// Permite cargar las centrales de la empresa seleccionada
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltroCentralXEmpresa(string idEmpresa)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                servicio.ListarEmpresasYCentralFormatoPr31((int)ConstantesCombustibles.Interfaz.Intranet, out List<SiEmpresaDTO> listaEmpAll, out List<EqEquipoDTO> listaCentralAll);

                if (idEmpresa == null) idEmpresa = string.Empty;
                var empresas = idEmpresa.Trim().Split(',').Select(x => int.Parse(x)).ToList();

                model.ListaCentral = listaCentralAll.Where(x => empresas.Contains(x.Emprcodi.Value)).ToList();
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
        /// Genera el archivo excel en el servidor web del reporte solicitado
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="centrales"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoReporte(string empresas, string centrales, string finicios, string ffins, int idEstado)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddDays(1);

                if (fechaInicio < fechaFin.AddYears(-1).AddDays(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servicio.GenerarArchivoExcelEnvios(ruta, empresas, centrales, fechaInicio, fechaFin, idEstado, pathLogo);

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
        /// Descarga el archivo excel generador por GenerarArchivoReporte
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = ConstantesCombustibles.NombreReporteEnvios;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        #endregion

        #region Exportación individual del formulario Extranet

        /// <summary>
        /// exportación de archivo excel
        /// </summary>
        /// <param name="cbenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFormularioEnvio(int cbenvcodi)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                servicio.GenerarArchivoExcelFormularioEnvio(ruta, cbenvcodi);

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

        #endregion

        #region Habilitación del plazo del módulo para corrección de la solicitud

        [HttpPost]
        public JsonResult ObtenerDatosEnvio(int idEnvio)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Envio = servicio.GetByIdCbEnvio(idEnvio);
                model.FechaPlazo = DateTime.Today.ToString(Constantes.FormatoFecha);
                model.HoraPlazo = DateTime.Now.Hour * 2 + 1;

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
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime dfechaObs = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecAmpl = dfechaObs.AddMinutes(hora * 30);

                if (fecAmpl < DateTime.Now)
                {
                    throw new ArgumentException("La fecha de ampliación debe ser mayor a la fecha actual.");
                }

                servicio.HabilitarPlazoEnvioPr31(idEnvio, base.UserName, fecAmpl);
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

        #region Habilitación de la casilla "1.06 Volumen de combustible en almacén"

        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HabilitarItem106(int idEnvio)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                servicio.HabilitarItem106(idEnvio, base.UserName);
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

        #region Envío - Información Formulario

        /// <summary>
        /// Muestra el formaulario para el envio de combustibles liquido
        /// </summary>
        /// <returns></returns>
        public ActionResult EnvioCombustible(int? idEnvio)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (idEnvio.GetValueOrDefault(0) <= 0) return base.RedirectToHomeDefault();

            CombustibleModel model = new CombustibleModel();

            var envio = servicio.GetByIdCbEnvio(idEnvio.Value);
            model.IdEnvio = idEnvio.Value;
            model.IdEmpresa = envio.Emprcodi;
            model.IdEquipo = envio.Equicodi;
            model.IdGrupo = envio.Grupocodi;
            model.IdFenerg = envio.Fenergcodi;
            model.Emprnomb = envio.Emprnomb;
            model.Equinomb = envio.Equinomb;
            model.Fenergnomb = envio.Fenergnomb;
            model.IdEstado = envio.Estenvcodi;
            model.IdTipoCombustible = envio.Estcomcodi;

            model.AccionEditar = envio.EsEditableIntranet;
            model.FechaVigencia = DateTime.Today.AddDays(1).ToString(Constantes.FormatoFecha);

            model.IdAgrup = CombustibleAppServicio.GetAgrupcodiByFenergcodi(envio.Fenergcodi);
            model.FechaConsulta = DateTime.Today.ToString(Constantes.FormatoFecha);
            if (envio.Estenvcodi == ConstantesCombustibles.EstadoAprobado)
                model.FechaConsulta = envio.Cbenvfecpreciovigente.Value.ToString(Constantes.FormatoFecha);
            model.FechaObs = servicio.FechaFinSubsanacionObservacion(envio.Estcomcodi, DateTime.Today).ToString(Constantes.FormatoFecha);

            model.HtmlListado = servicio.GenerarHtmlListaCostosVariable();

            //correos del generador
            model.To = envio.Cbenvususolicitud;
            model.CCcorreosAgente = ObtenerCCcorreosAgente(envio.Emprcodi, envio.Cbenvususolicitud);

            return View(model);
        }

        /// <summary>
        /// Envia model para visualizacion de grilla excel de combustible
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEnvio)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                var envio = servicio.GetByIdCbEnvio(idEnvio);
                model.IdEnvio = idEnvio;
                model.IdEmpresa = envio.Emprcodi;
                model.IdEquipo = envio.Equicodi;
                model.IdGrupo = envio.Grupocodi;
                model.IdFenerg = envio.Fenergcodi;
                model.IdTipoCombustible = CombustibleAppServicio.GetEstcomcodiByFenergcodi(model.IdFenerg);

                string url = Url.Content("~/");
                model.ModeloWeb = servicio.GetHandsonCombustible((int)ConstantesCombustibles.Interfaz.Intranet, idEnvio, model.IdTipoCombustible, model.IdEmpresa, model.IdGrupo, model.IdEquipo, model.IdFenerg);
                model.HtmlLogEnvio = servicio.GenerarHtmlLogEnvio(idEnvio);
                model.HtmlListaEnvioRelCv = servicio.GenerarHtmlListaEnvioRelacionCV(idEnvio, url);
                model.AccionEditar = model.ModeloWeb.Editable;

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

        #region Envío - Documentos

        /// <summary>
        /// Permite descargar el archivo al explorador
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(string fileName, int concepcodi, int idEnvio)
        {
            base.ValidarSesionUsuario();

            var envio = servicio.GetByIdCbEnvio(idEnvio);

            // Para obtener path base de acuerdo al modulo
            string path = CombustibleAppServicio.GetPathEmpresaEnvio("//", envio.Emprcodi, idEnvio) + concepcodi;

            //Manejo de carpetas
            string pathTemporal = path + "//" + fileName;

            byte[] buffer = FileServer.DownloadToArrayByte(pathTemporal, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

        #region Aprobación de envío, Observación, Desaprobacion

        /// <summary>
        /// Graba en BD la observacion hecha por el administrador para reportarlo al agente
        /// </summary>
        /// <param name="txtObserv"></param>
        /// <param name="icodenvio"></param>
        /// <returns></returns>
        public JsonResult GrabarObservacion(string txtObserv, int icodenvio, string fechaObs)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (string.IsNullOrEmpty(txtObserv))
                {
                    throw new ArgumentException("No ingresó observación.");
                }

                DateTime dfechaObs = DateTime.ParseExact(fechaObs, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                servicio.ObservarEnvioPr31(icodenvio, base.UserName, txtObserv, dfechaObs);
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
        /// Administrador Aprueba el envío
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AprobarEnvio(int idEnvio, string fechaVigencia, List<int> listaCodicocv, string correosCCagente)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime vigenciaEscogida = DateTime.ParseExact(fechaVigencia, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(correosCCagente))
                {
                    listaCorreoCCagente = correosCCagente.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                servicio.AprobarEnvioPr31(idEnvio, base.UserName, vigenciaEscogida, listaCodicocv, listaCorreoCCagente);
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
        /// Desaprobar envio
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DesaprobarEnvio(int idEnvio, string mensaje, string correosCCagente)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (string.IsNullOrEmpty(mensaje))
                {
                    throw new ArgumentException("No ingresó mensaje.");
                }

                List<string> listaCorreoCCagente = new List<string>();
                if (!string.IsNullOrEmpty(correosCCagente))
                {
                    listaCorreoCCagente = correosCCagente.Split(';').Select(x => x.Trim().ToLower()).ToList();
                }

                servicio.DesaprobarEnvioPr31(idEnvio, base.UserName, mensaje, listaCorreoCCagente);
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

                var regPr31 = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesCombustibles.ModcodiPr31Extranet);
                if (regPr31 != null && regPr31.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
        }

        #endregion

    }
}
