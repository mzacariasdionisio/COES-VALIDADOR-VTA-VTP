using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Equipamiento.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Equipamiento.Controllers
{
    public class EnvioController : BaseController
    {
        readonly FichaTecnicaAppServicio servFictec = new FichaTecnicaAppServicio();

        #region Declaracion de variables

        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
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

        #region Pantalla principal

        public ActionResult Index(int? carpeta)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            EnvioFormatoModel model = new EnvioFormatoModel();
            model.IdEstado = carpeta.GetValueOrDefault(0) <= 0 ? ConstantesFichaTecnica.EstadoSolicitud : carpeta.Value;

            ListarEmpresaAgente(out List<EmpresaCoes> listaEmpresas);
            model.ListaEmpresas = listaEmpresas;
            model.NumeroEmpresas = model.ListaEmpresas.Count();
            model.ListaEtapas = servFictec.ListFtExtEtapas();
            DateTime hoy = DateTime.Now;
            model.FechaInicio = (hoy.AddYears(-1)).ToString(ConstantesAppServicio.FormatoFecha);
            model.FechaFin = (hoy).ToString(ConstantesAppServicio.FormatoFecha);

            model.HabilitarAutoguardado = EsAgente() ? 1 : 0; //el autoguardado solo es para rol Usuario Extranet (agentes)

            return View(model);
        }

        /// <summary>
        /// Devuelve empresas del agente
        /// </summary>
        /// <param name="tipoCentral"></param>
        /// <param name="listaEmpresas"></param>
        /// <param name="listaTipoCentral"></param>
        private void ListarEmpresaAgente(out List<EmpresaCoes> listaEmpresas)
        {
            List<EmpresaCoes> listaEmpAll = servFictec.ListarEmpresasExtranetFT();

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            listaEmpresas = new List<EmpresaCoes>();
            if (permisoEmpresas)
            {
                listaEmpresas = listaEmpAll;
            }
            else
            {
                var lstEmpresa = this.seguridad.ObtenerEmpresasPorUsuario(base.UserName).ToList();
                foreach (var reg in listaEmpAll)
                {
                    var find = lstEmpresa.Find(x => x.EMPRCODI == (short)reg.Emprcodi);
                    if (find != null)
                    {
                        listaEmpresas.Add(reg);
                    }
                }
            }
        }

        private void ValidarCodigosEmpresaAgente(ref string empresas, out string mensaje)
        {
            mensaje = "";
            empresas = string.IsNullOrEmpty(empresas) ? "-1" : empresas;

            //validación de empresas
            ListarEmpresaAgente(out List<EmpresaCoes> listaEmpresas);
            if (listaEmpresas.Count == 0)
            {
                empresas = "0";
                mensaje = "El módulo extranet no cuenta con ningún equipo asignado a su empresa.";
            }
            else
            {
                if (empresas == "-1") empresas = string.Join(",", listaEmpresas.Select(x => x.Emprcodi).ToList());
            }
        }

        /// <summary>
        /// Devuelve el listado de envios segun carpeta
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="idEstado"></param>
        /// <param name="finicios"></param>
        /// <param name="ffins"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BloqueEnvios(string empresas, int ftetcodi, int idEstado, string finicios, string ffins)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                ValidarCodigosEmpresaAgente(ref empresas, out string mensajeEmp);
                model.Mensaje = mensajeEmp;

                model.ListadoEnvios = servFictec.ObtenerListadoEnviosEtapa(empresas, idEstado, fechaInicio, fechaFin, ftetcodi.ToString());
                if (!EsAgente()) //si es usuario @coes.org.pe no debe editar
                {
                    foreach (var item in model.ListadoEnvios)
                    {
                        item.EsEditableExtranet = false;
                    }
                }
                model.HtmlCarpeta = servFictec.GenerarHtmlCarpeta(empresas, idEstado, fechaInicio, fechaFin, ftetcodi);

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
        /// Devuelve el listado de etapas segun empresa con asignacion de proyectos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEtapasNE(int emprcodi)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaEtapas = servFictec.ListarEtapasPorEmpresaConAsignacion(emprcodi);
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
        /// Devuelve el listado de proyectos segun empresa con asignacion de proyectos
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarProyectosNE(int emprcodi, int ftetcodi)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaProyectos = servFictec.ListarProyectosPorEmpresaYEtapa(emprcodi, ftetcodi, ConstantesFichaTecnica.EstadoStrActivo);
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
        /// Devuelve el listado de equipos CIO (Conexion, integracion y operacion comercial)
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="ftetcodi"></param>
        /// <param name="ftprycodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerListadoEquiposCIMO(int emprcodi, int ftetcodi, int ftprycodi, string codigoEquipoNoSeleccionable)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Proyecto = servFictec.GetByIdFtExtProyecto(ftprycodi);

                model.ListaEquipoEnvio = servFictec.ListarEquiposNuevoEnvio(emprcodi, ftetcodi, ftprycodi);

                //los equipos del envio (carpeta observado) ya no pueden ser seleccionables en la opción "Agregar equipos"
                if (!string.IsNullOrEmpty(codigoEquipoNoSeleccionable))
                {
                    List<string> listaFteeqcodis = codigoEquipoNoSeleccionable.Split(',').ToList();

                    foreach (var item in model.ListaEquipoEnvio)
                    {
                        if (listaFteeqcodis.Contains(item.TipoYCodigo)) item.CheckSeleccionableEnNuevo = false;
                    }
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

        /// <summary>
        /// Cancela un envío solicitado
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="motivo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelarEnvio(int idEnvio, string motivo)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (string.IsNullOrEmpty(motivo))
                {
                    throw new ArgumentException("No ingresó motivo.");
                }

                //envio
                FtExtEnvioDTO regEnvio = servFictec.GetByIdFtExtEnvio(idEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Ftenvususolicitud);

                servFictec.CancelarEnvioExtranetFT(regEnvio, motivo, base.UserEmail, otrosUsuariosEmpresa);
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

            var listaCorreo = ObtenerCorreosGeneradorModuloFT(idEmpresa);
            listaCorreo = listaCorreo.Where(x => x != ususolicitud).OrderBy(x => x).ToList();

            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Devuelve lista de correos de agentes copropietarios
        /// </summary>
        /// <param name="idEmpresaCopropietaria"></param>
        /// <param name="ususolicitud"></param>
        /// <returns></returns>
        private string ObtenerAgentesCopropietarios(List<int> lstIdEmpresaCopropietarias, string ususolicitud)
        {
            ususolicitud = (ususolicitud ?? "").ToLower().Trim();

            List<string> listaCorreo = new List<string>();
            foreach (var idEmpresaCopropietaria in lstIdEmpresaCopropietarias)
            {
                var listaCorreoEmp = ObtenerCorreosGeneradorModuloFT(idEmpresaCopropietaria);
                listaCorreo.AddRange(listaCorreoEmp);
            }

            listaCorreo = listaCorreo.Where(x => x != ususolicitud).Distinct().OrderBy(x => x).ToList();

            return string.Join(";", listaCorreo);
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloFT(int idEmpresa)
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

                var regPr31 = listaModuloXUsuExt.Find(x => x.ModCodi == ConstantesFichaTecnica.ModcodiFichaTecnicaExtranet);
                if (regPr31 != null && regPr31.Selected > 0) //si tiene check opción activa
                {
                    listaCorreo.Add((regUsuario.UserEmail ?? "").ToLower().Trim());
                }
            }

            return listaCorreo;
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
        public JsonResult GenerarArchivoReporte(string empresas, int ftetcodi, int idEstado, string finicios, string ffins)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                ValidarCodigosEmpresaAgente(ref empresas, out string mensajeEmp);
                model.Mensaje = mensajeEmp;

                DateTime hoy = DateTime.Now;
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "Reporte_EnvíoFichaTécnica_" + hoy.Year + string.Format("{0:D2}", hoy.Month) + string.Format("{0:D2}", hoy.Day) +
                                                string.Format("{0:D2}", hoy.Hour) + string.Format("{0:D2}", hoy.Minute) + string.Format("{0:D2}", hoy.Second) + ".xlsx";

                servFictec.GenerarExportacionEnvios(ruta, pathLogo, empresas, fechaInicio, fechaFin, idEstado, ftetcodi, nameFile);
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

        #region Envío de Formato Extranet para Conexión, Integración y Modificación de Ficha Técnica 

        public ActionResult EnvioFormato(int accion = 0, int codigoEnvio = -1,
                            int codigoEmpresa = 0, int codigoEtapa = 0, int codigoProyecto = 0, string codigoEquipos = "")
        {
            string tipo = accion != 0 ? (accion == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            EnvioFormatoModel model = new EnvioFormatoModel();
            model.EnvioTipoFormato = ConstantesFichaTecnica.FormatoConexIntegModif;

            if (codigoEnvio <= 0) // 0 viene de la pantalla principal o negativo viene boton regresar del autoguardado
            {
                if (codigoEnvio == -1) return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });

                if (codigoEnvio == 0)
                {
                    if (ConstantesFichaTecnica.EtapaModificacion != codigoEtapa && (codigoEmpresa == 0 || codigoEtapa == 0 || codigoProyecto == 0 || codigoEquipos == ""))
                        return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });
                    if (ConstantesFichaTecnica.EtapaModificacion == codigoEtapa && (codigoEmpresa == 0 || codigoEtapa == 0 || codigoEquipos == ""))
                        return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });
                }

                //Verificar si existe envios temporales 
                servFictec.BuscarAutoguardado(codigoEnvio, codigoEmpresa, codigoEtapa, codigoProyecto, codigoEquipos, model.EnvioTipoFormato,
                                        out int codigoEnvioAUsar, out int codigoVersionAUsar, out bool existeEquipoAutoguardado);
                SiEmpresaDTO objEmp = servFictec.GetByIdSiEmpresa(codigoEmpresa);
                FtExtEtapaDTO objEtapa = servFictec.GetByIdFtExtEtapa(codigoEtapa);
                FtExtProyectoDTO objPry = codigoProyecto > 0 ? servFictec.GetByIdFtExtProyecto(codigoProyecto) : new FtExtProyectoDTO();

                model.IdEnvio = 0;
                model.IdEnvioTemporal = codigoEnvioAUsar;
                model.IdVersion = 0;
                model.IdVersionTemporal = codigoVersionAUsar;
                model.FlagEquipoAutoguardado = existeEquipoAutoguardado ? 1 : 0;
                model.IdEstado = ConstantesFichaTecnica.EstadoSolicitud;

                //datos
                model.TipoOpcion = "E"; //editable cuando es solicitud todavia no guardado
                model.CodigoEquipos = codigoEquipos;
                model.MsgCancelacion = "";

                model.Emprcodi = objEmp.Emprcodi;
                model.Emprnomb = objEmp.Emprnomb;
                model.Ftetcodi = objEtapa.Ftetcodi;
                model.Ftetnombre = objEtapa.Ftetnombre;
                model.Ftprycodi = objPry.Ftprycodi;
                model.Ftprynombre = objPry.Ftprynombre;
            }
            else
            {
                //verificar envio seleccionado
                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                model.IdEnvio = codigoEnvio;
                model.IdEnvioTemporal = codigoEnvio;
                model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvioAct, ConstantesFichaTecnica.EXTRANET);
                model.IdVersionTemporal = objEnvioAct.FtevercodiTemporalExtranet;
                model.FlagEquipoAutoguardado = objEnvioAct.FtevercodiTemporalExtranet > 0 ? 1 : 0;
                model.IdEstado = objEnvioAct.Estenvcodi;

                //datos
                model.TipoOpcion = tipo; //lectura o editable
                //model.CodigoEquipos = string.Join(",", servFictec.ListFtExtEnvioEqsXEnvio(model.IdVersionTemporal).Select(x => x.TipoYCodigo));
                model.EsFTModificada = servFictec.VerificarSiParametrosFueronModificados(objEnvioAct.FtevercodiTemporalExtranet);
                model.MsgCancelacion = objEnvioAct.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvioAct.Ftenvobs != null ? objEnvioAct.Ftenvobs.Trim() : "") : "";
                model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvioAct, ConstantesFichaTecnica.EXTRANET);
                model.HabilitarAddEquipo = objEnvioAct.Estenvcodi == ConstantesFichaTecnica.EstadoObservado && objEnvioAct.Ftenvflaghabeq == "S";

                model.Emprcodi = objEnvioAct.Emprcodi;
                model.Emprnomb = objEnvioAct.Emprnomb;
                model.Ftetcodi = objEnvioAct.Ftetcodi;
                model.Ftetnombre = objEnvioAct.Ftetnombre;
                model.Ftprycodi = objEnvioAct.Ftprycodi ?? 0;
                model.Ftprynombre = objEnvioAct.Ftprynombre;
            }

            model.HabilitarAutoguardado = EsAgente() ? 1 : 0; //el autoguardado solo es para rol Usuario Extranet (agentes)
            model.MinutosAutoguardado = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTMinutosAutoguardado]);
            model.ClaveCookie = GetKeyCookie(model.IdEnvioTemporal, model.Emprcodi, model.Ftetcodi, model.Ftprycodi, model.EnvioTipoFormato);

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarEqConexIntegModifXEnvio(int codigoEnvio, int versionEnvio)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);

                model.CodigoEquipos = string.Join(",", model.ListaEnvioEq.Select(x => x.TipoYCodigo));
                model.LstFteeqcodis = model.ListaEnvioEq.Any() ? string.Join(",", model.ListaEnvioEq.Select(x => x.Fteeqcodi.ToString())) : "";
                model.LstEnviosEqNombres = model.ListaEnvioEq.Any() ? string.Join(",", model.ListaEnvioEq.Select(x => x.Nombreelemento.Trim())) : "";

                model.ListaErrores = servFictec.ObtenerListadoErroresExtranet(codigoEnvio, versionEnvio);
                model.ListaVersion = servFictec.ListarVersionOficialExtranet(codigoEnvio);
                model.ListaAutoguardado = servFictec.ListarVersionAutoguardadoExtranet(codigoEnvio);

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
        /// Enviar solicitud o subsanacion a COES
        /// </summary>
        /// <param name="codigoEnvioTemporal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarSolicitudConexIntegModif(int codigoEnvio, int versionEnvio)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                string emailCCAgentesCopropietarios = "";
                if (codigoEnvio <= 0)
                {
                    //verifico si el elemento tiene empresas copropietarias y obtengo sus correos
                    List<int> lstIdEmpresaCopropietarias = servFictec.ListarEmprcodiCopropietarioDeEnvio(versionEnvio);
                    emailCCAgentesCopropietarios = ObtenerAgentesCopropietarios(lstIdEmpresaCopropietarias, base.UserEmail);
                }

                //guardar datos, archivos y enviar notificación
                FtExtEnvioDTO objEnvioTemp = servFictec.ActualizarSolicitudFormatoExtranetFromTemporal(codigoEnvio, versionEnvio, ConstantesFichaTecnica.FormatoConexIntegModif,
                                                                    emailCCAgentesCopropietarios, base.UserEmail, true);

                //Luego de realizada la solicitud o levantamiento de observaciones, crear una versión temporal para guardar los datos que luego trabajará el administrador
                if (objEnvioTemp.VersionActual.Ftevercodi > 0)
                {
                    servFictec.CrearVersionTrabajoFromVersionBD(objEnvioTemp.Ftenvcodi, objEnvioTemp.VersionActual.Ftevercodi,
                                                            objEnvioTemp.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporal, objEnvioTemp.Ftenvtipoformato, "SISTEMA");
                }

                model.Resultado = "1";
                model.IdEstado = objEnvioTemp.Estenvcodi;
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
        public JsonResult GenerarFormatoConexIntegModif(int codigoEnvio, int versionEnvio, string fteeqcodisLimpiar, string fteeqcodis = "")
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesFichaTecnica.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                servFictec.GenerarFormatoConexIntegModifXEnvio(ruta, pathLogo, codigoEnvio, fteeqcodis, fteeqcodisLimpiar, ConstantesFichaTecnica.EXTRANET, versionEnvio, "", out string fileName);

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
        public JsonResult LeerFileUpExcelFormatoConexIntegModif(int codigoEnvio, int estado, int versionEnvio, string mensajeAutoguardado)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                List<FTReporteExcel> listaEqEnv = servFictec.ListarLecturaExcelFormatoConexIntegModif(codigoEnvio, this.NombreFile, ConstantesFichaTecnica.EXTRANET, estado, versionEnvio, mensajeAutoguardado, 0, "", out string mensajes);
                if (mensajes != "")
                    throw new ArgumentException(mensajes);

                //realizar autoguardado
                bool flagHuboAutoguardado = servFictec.AutoguardarVersionTemporalExtranet(codigoEnvio, versionEnvio, ConstantesFichaTecnica.RealizadoPorManual, false, null, null,
                                                                                    listaEqEnv, false, base.UserEmail);

                //volver a obtener los errores y lista de autoguardados
                model.ListaErrores = servFictec.ObtenerListadoErroresExtranet(codigoEnvio, versionEnvio);
                model.ListaAutoguardado = servFictec.ListarVersionAutoguardadoExtranet(codigoEnvio);

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
        /// Obtiene la informacion para armar la estructura izquierda de la tabla
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult ObtenerDatosFT(int fteeqcodi, int tipoForm, string fteeqcodisLimpiar, int versionAnterior = 0)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (ConstantesFichaTecnica.OpcionVisualFormActualBD == tipoForm)
                {
                    FTReporteExcel rep = null;
                    if (!string.IsNullOrEmpty(fteeqcodisLimpiar))
                    {
                        //si el equipo ya estaba guardado y el usuario NO seleccionó la opción "precargar"
                        //entonces mantener la versión limpia
                        List<int> listaFteeqcodis = fteeqcodisLimpiar.Split(',').Select(x => int.Parse(x)).ToList();
                        if (listaFteeqcodis.Contains(fteeqcodi))
                            rep = servFictec.ObtenerFichaTreeXEnvioEqLimpia(fteeqcodi);
                    }

                    if (rep == null)
                        rep = servFictec.ObtenerFichaTreeXEnvioEq(fteeqcodi, ConstantesFichaTecnica.EXTRANET);

                    model.ReporteDatoXEq = rep;
                    model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionParametrosAFT(fteeqcodi, ConstantesFichaTecnica.EXTRANET);
                }

                if (ConstantesFichaTecnica.OpcionVisualFormLimpiar == tipoForm)
                {
                    FTReporteExcel rep = servFictec.ObtenerFichaTreeXEnvioEqLimpia(fteeqcodi);
                    model.ReporteDatoXEq = rep;
                    model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionParametrosAFT(fteeqcodi, ConstantesFichaTecnica.EXTRANET);
                }
                //los errores se vuelven a calcular cuando se realiza el automático o el usuario presiona el botón "enviar"

                if (ConstantesFichaTecnica.OpcionVisualFormCambioVersion == tipoForm)
                {
                    servFictec.BuscarDiferenciasEntreVersionesConexIntegModif(fteeqcodi, versionAnterior, out FTReporteExcel rep, out List<FTDatoRevisionParametrosAEnvio> listaRevisionParametrosAFT);
                    model.ReporteDatoXEq = rep;
                    model.ListaRevisionParametrosAFT = listaRevisionParametrosAFT;
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

        #endregion

        #region Envío de Formato Extranet para Operación Comercial

        public ActionResult EnvioFormatoOperacionComercial(int codigoEnvio = -1, int accion = 0,
                        int codigoEmpresa = 0, int codigoEtapa = 0, int codigoProyecto = 0, string codigoEquipos = "")
        {
            string tipo = accion != 0 ? (accion == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            EnvioFormatoModel model = new EnvioFormatoModel();
            model.EnvioTipoFormato = ConstantesFichaTecnica.FormatoOperacionComercial;

            if (codigoEnvio <= 0) // 0 viene de la pantalla principal o negativo viene boton regresar del detalle
            {
                if (codigoEnvio == -1) return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });

                if (codigoEnvio == 0)
                {
                    if (codigoEmpresa == 0 || codigoEtapa == 0 || codigoProyecto == 0 || codigoEquipos == "")
                        return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });
                }

                //Verificar si existe envios temporales 
                servFictec.BuscarAutoguardado(codigoEnvio, codigoEmpresa, codigoEtapa, codigoProyecto, codigoEquipos, model.EnvioTipoFormato,
                                        out int codigoEnvioAUsar, out int codigoVersionAUsar, out bool existeEquipoAutoguardado);
                SiEmpresaDTO objEmp = servFictec.GetByIdSiEmpresa(codigoEmpresa);
                FtExtEtapaDTO objEtapa = servFictec.GetByIdFtExtEtapa(codigoEtapa);
                FtExtProyectoDTO objPry = codigoProyecto > 0 ? servFictec.GetByIdFtExtProyecto(codigoProyecto) : new FtExtProyectoDTO();

                model.IdEnvio = 0;
                model.IdEnvioTemporal = codigoEnvioAUsar;
                model.IdVersion = 0;
                model.IdVersionTemporal = codigoVersionAUsar;
                model.FlagEquipoAutoguardado = existeEquipoAutoguardado ? 1 : 0;
                model.IdEstado = ConstantesFichaTecnica.EstadoSolicitud;

                //datos
                model.TipoOpcion = "E"; //editable cuando es solicitud todavia no guardado
                model.CodigoEquipos = codigoEquipos;
                model.MsgCancelacion = "";

                model.Emprcodi = objEmp.Emprcodi;
                model.Emprnomb = objEmp.Emprnomb;
                model.Ftetcodi = objEtapa.Ftetcodi;
                model.Ftetnombre = objEtapa.Ftetnombre;
                model.Ftprycodi = objPry.Ftprycodi;
                model.Ftprynombre = objPry.Ftprynombre;
            }
            else
            {
                //verificar envio seleccionado
                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                model.IdEnvio = codigoEnvio;
                model.IdEnvioTemporal = codigoEnvio;
                model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvioAct, ConstantesFichaTecnica.EXTRANET);
                model.IdVersionTemporal = objEnvioAct.FtevercodiTemporalExtranet;//si es cero entonces se creará una versión temporal de trabajo
                model.FlagEquipoAutoguardado = objEnvioAct.FtevercodiTemporalExtranet > 0 ? 1 : 0;
                model.IdEstado = objEnvioAct.Estenvcodi;

                //datos
                model.TipoOpcion = tipo; //lectura o editable
                model.MsgCancelacion = objEnvioAct.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvioAct.Ftenvobs != null ? objEnvioAct.Ftenvobs.Trim() : "") : "";
                model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvioAct, ConstantesFichaTecnica.EXTRANET);
                model.HabilitarAddEquipo = objEnvioAct.Estenvcodi == ConstantesFichaTecnica.EstadoObservado && objEnvioAct.Ftenvflaghabeq == "S";

                model.Emprcodi = objEnvioAct.Emprcodi;
                model.Emprnomb = objEnvioAct.Emprnomb;
                model.Ftetcodi = objEnvioAct.Ftetcodi;
                model.Ftetnombre = objEnvioAct.Ftetnombre;
                model.Ftprycodi = objEnvioAct.Ftprycodi ?? 0;
                model.Ftprynombre = objEnvioAct.Ftprynombre;
            }

            model.HabilitarAutoguardado = EsAgente() ? 1 : 0; //el autoguardado solo es para rol Usuario Extranet (agentes)
            model.MinutosAutoguardado = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTMinutosAutoguardado]);
            model.ClaveCookie = GetKeyCookie(model.IdEnvioTemporal, model.Emprcodi, model.Ftetcodi, model.Ftprycodi, model.EnvioTipoFormato);

            return View(model);
        }

        [HttpPost]
        public JsonResult ListarRequisitoXEnvio(int codigoEnvio, int versionEnvio, int tipoForm, int versionAnterior = 0)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                FtExtEnvioDTO objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

                model.Evento = servFictec.GetByIdFtExtEvento(objEnvio.Ftevcodi ?? 0);
                model.ListaEnvioEq = servFictec.ListFtExtEnvioEqsXEnvio(versionEnvio);
                model.CodigoEquipos = string.Join(",", model.ListaEnvioEq.Select(x => x.TipoYCodigo));

                if (ConstantesFichaTecnica.OpcionVisualFormActualBD == tipoForm)
                {
                    model.ListaReqEvento = servFictec.ListarRequisitoXEnvioVersion(objEnvio.Ftevcodi ?? 0, versionEnvio, ConstantesFichaTecnica.EXTRANET);
                    model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionContenidoReq(versionEnvio, ConstantesFichaTecnica.EXTRANET);
                    model.ListaErrores = servFictec.ValidarErroresExtranetReq(objEnvio, model.ListaReqEvento, model.ListaRevisionParametrosAFT);
                }
                if (ConstantesFichaTecnica.OpcionVisualFormLimpiar == tipoForm)
                {
                    model.ListaReqEvento = servFictec.ListarRequisitoXEnvioVersion(objEnvio.Ftevcodi ?? 0, versionEnvio, ConstantesFichaTecnica.EXTRANET);
                    model.ListaRevisionParametrosAFT = servFictec.ObtenerDatosRevisionContenidoReq(versionEnvio, ConstantesFichaTecnica.EXTRANET);
                    servFictec.LimpiarEnvioXRequisito(objEnvio, model.ListaReqEvento);
                    model.ListaErrores = servFictec.ValidarErroresExtranetReq(objEnvio, model.ListaReqEvento, model.ListaRevisionParametrosAFT);
                }

                if (ConstantesFichaTecnica.OpcionVisualFormCambioVersion == tipoForm)
                {
                    servFictec.BuscarDiferenciasEntreVersionesOpComercial(objEnvio.Ftevcodi ?? 0, versionEnvio, versionAnterior,
                                        out List<FtExtEventoReqDTO> listaReqEvento, out List<FTDatoRevisionParametrosAEnvio> listaRevisionParametrosAFT);
                    model.ListaReqEvento = listaReqEvento;
                    model.ListaRevisionParametrosAFT = listaRevisionParametrosAFT;
                }

                model.ListaVersion = servFictec.ListarVersionOficialExtranet(codigoEnvio);
                model.ListaAutoguardado = servFictec.ListarVersionAutoguardadoExtranet(codigoEnvio);

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
        public JsonResult EnviarSolicitudOperacionComercial(int codigoEnvio, int versionEnvio)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio); //tipo formato: operación comercial o dar de baja

                //guardar datos, archivos y enviar notificación
                FtExtEnvioDTO objEnvioTemp = servFictec.ActualizarSolicitudFormatoExtranetFromTemporal(codigoEnvio, versionEnvio, objEnvioAct.Ftenvtipoformato,
                                                                    null, base.UserEmail, true);

                //Luego de realizada la solicitud o levantamiento de observaciones, crear una versión temporal para guardar los datos que está trabajando el administrador
                if (objEnvioTemp.VersionActual.Ftevercodi > 0)
                {
                    servFictec.CrearVersionTrabajoFromVersionBD(objEnvioTemp.Ftenvcodi, objEnvioTemp.VersionActual.Ftevercodi,
                                                            objEnvioTemp.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporal, objEnvioAct.Ftenvtipoformato, "SISTEMA");
                }

                model.Resultado = "1";
                model.IdEstado = objEnvioTemp.Estenvcodi;
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

        public ActionResult EnvioFormatoBajaModoOperacion(int codigoEnvio = -1, int accion = 0,
                                                string codigoModoBaja = "")
        {
            string tipo = accion != 0 ? (accion == ConstantesFichaTecnica.AccionEditar ? "E" : "v") : "E";

            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            EnvioFormatoModel model = new EnvioFormatoModel();
            model.EnvioTipoFormato = ConstantesFichaTecnica.FormatoBajaModoOperacion;

            if (codigoEnvio <= 0) // 0 viene de la pantalla principal o negativo viene boton regresar del detalle
            {
                if (codigoEnvio == -1) return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });

                if (codigoEnvio == 0)
                {
                    if (codigoModoBaja == "")
                        return RedirectToAction("Index", "Envio", new { area = "Equipamiento" });
                }

                //Verificar si existe envios temporales 
                int grupocodi = Convert.ToInt32(codigoModoBaja.Replace("G", ""));
                int codigoEtapa = ConstantesFichaTecnica.EtapaModificacion;
                FtExtEtapaDTO objEtapa = servFictec.GetByIdFtExtEtapa(codigoEtapa);
                PrGrupoDTO objGrupo = servFictec.GetByIdPrGrupo(grupocodi);
                servFictec.BuscarAutoguardado(codigoEnvio, objGrupo.Emprcodi ?? 0, codigoEtapa, 0, codigoModoBaja, model.EnvioTipoFormato,
                                        out int codigoEnvioAUsar, out int codigoVersionAUsar, out bool existeEquipoAutoguardado);

                model.IdEnvio = 0;
                model.IdEnvioTemporal = codigoEnvioAUsar;
                model.IdVersion = 0;
                model.IdVersionTemporal = codigoVersionAUsar;
                model.FlagEquipoAutoguardado = existeEquipoAutoguardado ? 1 : 0;
                model.IdEstado = ConstantesFichaTecnica.EstadoSolicitud;

                //datos
                model.TipoOpcion = "E"; //editable cuando es solicitud todavia no guardado
                model.CodigoEquipos = codigoModoBaja;
                model.MsgCancelacion = "";

                model.Emprcodi = objGrupo.Emprcodi ?? 0;
                model.Emprnomb = objGrupo.Emprnomb;
                model.Ftetcodi = objEtapa.Ftetcodi;
                model.Ftetnombre = objEtapa.Ftetnombre;
            }
            else
            {
                //verificar envio seleccionado
                var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                model.IdEnvio = codigoEnvio;
                model.IdEnvioTemporal = codigoEnvio;
                model.IdVersion = servFictec.GetVersionSegunAmbiente(objEnvioAct, ConstantesFichaTecnica.EXTRANET);
                model.IdVersionTemporal = objEnvioAct.FtevercodiTemporalExtranet;//si es cero entonces se creará una versión temporal de trabajo
                model.FlagEquipoAutoguardado = objEnvioAct.FtevercodiTemporalExtranet > 0 ? 1 : 0;
                model.IdEstado = objEnvioAct.Estenvcodi;

                //datos
                model.TipoOpcion = tipo; //lectura o editable
                model.MsgCancelacion = objEnvioAct.Estenvcodi == ConstantesFichaTecnica.EstadoCancelado ? (objEnvioAct.Ftenvobs != null ? objEnvioAct.Ftenvobs.Trim() : "") : "";
                model.MsgFecMaxRespuesta = servFictec.ObtenerMensajeFechaMaxRespuesta(objEnvioAct, ConstantesFichaTecnica.EXTRANET);

                model.Emprcodi = objEnvioAct.Emprcodi;
                model.Emprnomb = objEnvioAct.Emprnomb;
                model.Ftetcodi = objEnvioAct.Ftetcodi;
                model.Ftetnombre = objEnvioAct.Ftetnombre;
            }

            model.HabilitarAutoguardado = EsAgente() ? 1 : 0; //el autoguardado solo es para rol Usuario Extranet (agentes)
            model.MinutosAutoguardado = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFlagFTMinutosAutoguardado]);
            model.ClaveCookie = GetKeyCookie(model.IdEnvioTemporal, model.Emprcodi, model.Ftetcodi, model.Ftprycodi, model.EnvioTipoFormato);

            return View(model);
        }

        #endregion

        #region Envío - Autoguardado automático y manual - Agregar equipos

        private bool EsAgente()
        {
            //return true;

            //agente
            if (!base.UserEmail.ToLower().Contains("@coes.org.pe"))
                return true;

            return false; //Usuario COES Administrador o de consulta
        }

        public async Task<JsonResult> CrearVersionTemporal(int codigoEnvio, int codigoVersion,
                    int codigoEmpresa = 0, int codigoEtapa = 0, int codigoProyecto = 0, string codigoEquipos = "", bool esLimpiarVersion = false)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                int idVersionTemporal = codigoVersion;
                string nuevoKeyCookie = "";
                string fteeqcodisLimpiar = "";

                //en estado SOLICITUD si no existe envio temporal entonces crear un envio y version temporal
                if (codigoEnvio <= 0)
                {
                    servFictec.ListarEquipoAdicionalSinAutoguardado(true, codigoVersion, codigoEmpresa, codigoEtapa, codigoProyecto, codigoEquipos
                                                    , out List<string> listaEqNuevo, out List<int> listaFteeqcodiUpdate, out List<int> listaFteeqcodiEliminar);

                    //generar estructura desde la ficha tecnica vigente
                    string carpetaUploadTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload + "FTExt_" + DateTime.Now.Ticks + "\\";
                    FtExtEnvioDTO objEnvioTemp = await servFictec.GetEnvioTemporalConexIntegModif(codigoEmpresa, codigoEtapa, codigoProyecto,
                                                                                    string.Join(",", listaEqNuevo), base.UserEmail, carpetaUploadTemporal);

                    //actualizar la replica de la configuración de los autoguardados
                    servFictec.ActualizarFormatoExtranetAutoguardado(listaFteeqcodiUpdate);

                    //crear una versión ficticia (no se considera versión temporal hasta que haya un autoguardado manual o automático desde el navegador web)
                    //Actualizar datos con casos especiales y guardar en bd
                    objEnvioTemp = servFictec.CrearOActualizarVersionTrabajoFromFichaVigente(codigoEnvio, codigoVersion, objEnvioTemp,
                                                                            carpetaUploadTemporal, listaFteeqcodiUpdate, listaFteeqcodiEliminar);

                    codigoEnvio = objEnvioTemp.Ftenvcodi;
                    idVersionTemporal = objEnvioTemp.VersionActual.Ftevercodi;
                    nuevoKeyCookie = GetKeyCookie(codigoEnvio, codigoEmpresa, codigoEtapa, codigoProyecto, ConstantesFichaTecnica.FormatoConexIntegModif);
                    if (esLimpiarVersion) fteeqcodisLimpiar = string.Join(",", listaFteeqcodiUpdate);

                    //crear una copia de los nuevos equipos del envio para luego verificar cuales son los cambios o para realizar la opción "Limpiar"
                    var objEnvioE = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                    var fteeqcodisNuevo = string.Join(",", objEnvioTemp.VersionActual.ListaEquipoEnvio.Where(x => listaEqNuevo.Contains(x.TipoYCodigo)).Select(x => x.Fteeqcodi).ToList());
                    servFictec.CrearOActualizarVersionTrabajoFromVersionBD(objEnvioTemp.Ftenvcodi, objEnvioTemp.VersionActual.Ftevercodi,
                                                            objEnvioTemp.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporalFTVigente, ConstantesFichaTecnica.FormatoConexIntegModif, base.UserEmail,
                                                            objEnvioE.FtevercodiTemporalFTVigente, fteeqcodisNuevo);
                }

                //en estado OBSERVACION crear version temporal
                if (codigoEnvio > 0)
                {
                    var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                    if (ConstantesFichaTecnica.AccionEditar == objEnvioAct.TipoAccionExtranet)
                    {

                        //Obtenemos ultima version TEMPORAL y ACTIVA del envio
                        if (codigoVersion == 0)
                        {
                            //crear una versión ficticia a partir de lo generado por el administrador
                            //(no se considera versión temporal hasta que haya un autoguardado manual o automático desde el navegador web)
                            idVersionTemporal = servFictec.CrearVersionTrabajoFromVersionBD(objEnvioAct.Ftenvcodi, objEnvioAct.VersionActual.Ftevercodi,
                                                                    objEnvioAct.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporal, ConstantesFichaTecnica.FormatoConexIntegModif, base.UserEmail);
                        }
                        else
                        {
                            //ocultar equipos que no son de la versión oficial
                            if (esLimpiarVersion)
                            {
                                //equipos de versión oficial
                                var listaEqVerOficial = servFictec.ListFtExtEnvioEqsXEnvio(objEnvioAct.FtevercodiOficial);
                                List<string> listaCodigoEquipoOficial = listaEqVerOficial.Select(x => x.TipoYCodigo).ToList();

                                //equipos de versión temporal
                                var listaEqVerTemporal = servFictec.ListFtExtEnvioEqsXEnvio(codigoVersion);

                                fteeqcodisLimpiar = string.Join(",", listaEqVerTemporal.Where(x => listaCodigoEquipoOficial.Contains(x.TipoYCodigo)).Select(x => x.Fteeqcodi));
                                var listaFteeqcodiOcultar = listaEqVerTemporal.Where(x => !listaCodigoEquipoOficial.Contains(x.TipoYCodigo)).Select(x => x.Fteeqcodi).ToList();

                                servFictec.ActualizarEstadoEquipoFormatoExtranet(null, null, listaFteeqcodiOcultar);
                            }
                        }
                    }
                }

                model.IdEnvio = codigoEnvio;
                model.IdVersionTemporal = idVersionTemporal;
                model.ClaveCookie = nuevoKeyCookie;
                model.FteeqcodisLimpiar = fteeqcodisLimpiar;

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

        public JsonResult CrearVersionTemporalReq(int tipoFormato, int codigoEnvio, int codigoVersion,
                    int codigoEmpresa = 0, int codigoEtapa = 0, int codigoProyecto = 0, string codigoEquipos = "", bool esLimpiarVersion = false)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                int idVersionTemporal = codigoVersion;
                string nuevoKeyCookie = "";

                //en estado SOLICITUD si no existe envio temporal entonces crear un envio y version temporal
                if (codigoEnvio <= 0)
                {
                    //no existe autoguardado previo o no quiere usar autoguardado
                    if (codigoVersion == 0)
                    {
                        //siempre crear autoguardado
                        FtExtEnvioDTO objEnvioTemp;
                        if (ConstantesFichaTecnica.FormatoOperacionComercial == tipoFormato)
                        {
                            objEnvioTemp = servFictec.GetEnvioTemporalOperacionComercial(codigoEmpresa, codigoEtapa, codigoProyecto, codigoEquipos, base.UserEmail);
                        }
                        else
                        {
                            objEnvioTemp = servFictec.GetEnvioEstructuraFromWebBajaModoOperacion(codigoEquipos, base.UserEmail);
                        }
                        objEnvioTemp.Ftenvcodi = codigoEnvio;
                        objEnvioTemp.VersionActual.Ftevercodi = 0;
                        objEnvioTemp.VersionActual.Estenvcodi = ConstantesFichaTecnica.EstadoSolicitud;
                        objEnvioTemp.VersionActual.Ftevertipo = ConstantesFichaTecnica.GuardadoTemporal; //4 luego se hará update a 2
                        servFictec.CrearVersionTemporalFormatoExtranet(objEnvioTemp);

                        codigoEnvio = objEnvioTemp.Ftenvcodi;
                        idVersionTemporal = objEnvioTemp.VersionActual.Ftevercodi;
                        nuevoKeyCookie = GetKeyCookie(codigoEnvio, codigoEmpresa, codigoEtapa, codigoProyecto, tipoFormato);
                    }
                    else
                    {
                        FtExtEnvioVersionDTO objVersion = servFictec.GetByIdFtExtEnvioVersion(codigoVersion);
                        codigoEnvio = objVersion.Ftenvcodi;
                        idVersionTemporal = codigoVersion;

                        //copiar datos del anterior autoguardado
                        if (codigoVersion > 0)
                        {
                            servFictec.ListarEquipoAdicionalSinAutoguardado(false, codigoVersion, codigoEmpresa, codigoEtapa, codigoProyecto, codigoEquipos
                                                            , out List<string> listaEqNuevo, out List<int> listaFteeqcodiUpdate, out List<int> listaFteeqcodiEliminar);

                            List<FtExtEnvioEqDTO> listaEqEnvNuevo = new List<FtExtEnvioEqDTO>();
                            foreach (var item in listaEqNuevo)
                            {
                                var regNuevo = new FtExtEnvioEqDTO();
                                regNuevo.Ftevercodi = codigoVersion;
                                if (item.Contains("E")) regNuevo.Equicodi = Int32.Parse(item.Replace("E", ""));
                                if (item.Contains("G")) regNuevo.Grupocodi = Int32.Parse(item.Replace("G", ""));
                                regNuevo.Fteeqestado = "S";
                                regNuevo.TipoYCodigo = item;

                                listaEqEnvNuevo.Add(regNuevo);
                            }

                            servFictec.ActualizarEstadoEquipoFormatoExtranet(listaEqEnvNuevo, listaFteeqcodiUpdate, listaFteeqcodiEliminar);
                        }
                    }
                }

                //en estado OBSERVACION crear version temporal
                if (codigoEnvio > 0)
                {
                    var objEnvioAct = servFictec.GetByIdFtExtEnvio(codigoEnvio);
                    if (ConstantesFichaTecnica.AccionEditar == objEnvioAct.TipoAccionExtranet)
                    {
                        //Obtenemos ultima version TEMPORAL y ACTIVA del envio
                        if (codigoVersion == 0)
                        {
                            //crear una versión ficticia a partir de lo generado por el administrador
                            //(no se considera versión temporal hasta que haya un autoguardado manual o automático desde el navegador web)
                            idVersionTemporal = servFictec.CrearVersionTrabajoFromVersionBD(objEnvioAct.Ftenvcodi, objEnvioAct.VersionActual.Ftevercodi,
                                                                    objEnvioAct.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporal, objEnvioAct.Ftenvtipoformato, base.UserEmail);
                        }
                        else
                        {
                            //ocultar equipos que no son de la versión oficial
                            if (esLimpiarVersion)
                            {
                                servFictec.ListarEquipoAdicionalSinAutoguardado(false, codigoVersion, objEnvioAct.Emprcodi, objEnvioAct.Ftetcodi, objEnvioAct.Ftprycodi ?? 0, codigoEquipos
                                                            , out List<string> listaEqNuevo, out List<int> listaFteeqcodiUpdate, out List<int> listaFteeqcodiEliminar);

                                List<FtExtEnvioEqDTO> listaEqEnvNuevo = new List<FtExtEnvioEqDTO>();
                                foreach (var item in listaEqNuevo)
                                {
                                    var regNuevo = new FtExtEnvioEqDTO();
                                    regNuevo.Ftevercodi = codigoVersion;
                                    if (item.Contains("E")) regNuevo.Equicodi = Int32.Parse(item.Replace("E", ""));
                                    if (item.Contains("G")) regNuevo.Grupocodi = Int32.Parse(item.Replace("G", ""));
                                    regNuevo.Fteeqestado = "S";
                                    regNuevo.TipoYCodigo = item;

                                    listaEqEnvNuevo.Add(regNuevo);
                                }

                                servFictec.ActualizarEstadoEquipoFormatoExtranet(listaEqEnvNuevo, listaFteeqcodiUpdate, listaFteeqcodiEliminar);
                            }
                        }
                    }
                }

                model.IdEnvio = codigoEnvio;
                model.IdVersionTemporal = idVersionTemporal;
                model.ClaveCookie = nuevoKeyCookie;

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
        public JsonResult GuardarDatosFT(FTReporteExcel modelWeb)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //var numero = 5;
                //var division = 5 / (numero - 5);
                //guardar cambios
                if (modelWeb.Ftenvcodi != 0 && modelWeb.Ftevercodi > 0)
                {
                    List<FTReporteExcel> listaModelWeb = new List<FTReporteExcel>() { modelWeb };

                    //hacer "Limpiar" a cada equipo cuando no se seleccionó el "precargar autoguardado"
                    var objEnvio = servFictec.GetByIdFtExtEnvio(modelWeb.Ftenvcodi);
                    if (objEnvio.Ftenvtipoformato == ConstantesFichaTecnica.FormatoConexIntegModif)
                    {
                        if (!string.IsNullOrEmpty(modelWeb.FteeqcodisLimpiar))
                        {
                            List<int> listaFteeqcodis = modelWeb.FteeqcodisLimpiar.Split(',').Select(x => int.Parse(x)).Where(x => x != modelWeb.Fteeqcodi).ToList();

                            foreach (var fteeqcodiOtro in listaFteeqcodis)
                            {
                                var repOtro = servFictec.ObtenerFichaTreeXEnvioEqLimpia(fteeqcodiOtro);
                                listaModelWeb.Add(repOtro);
                            }
                        }
                    }

                    bool flagHuboAutoguardado = servFictec.AutoguardarVersionTemporalExtranet(modelWeb.Ftenvcodi, modelWeb.Ftevercodi, modelWeb.TipoAutoguardado,
                                                                    modelWeb.HayPendiente1erAutoguardado, modelWeb.MensajeAutoguardado, modelWeb.MensajeNoConexion,
                                                                    listaModelWeb, true, base.UserEmail);

                    //volver a obtener los errores y lista de autoguardados
                    if (flagHuboAutoguardado)
                    {
                        //Actualizar flag de los cambios de los items de Modificación de Ficha técnica
                        servFictec.ListarCambiosEtapaModificacionFT(modelWeb.Ftenvcodi, modelWeb.Ftevercodi, true, out List<FTParametroModificacion> lstCambios);

                        //obtener errores
                        model.ListaErrores = servFictec.ObtenerListadoErroresExtranet(modelWeb.Ftenvcodi, modelWeb.Ftevercodi);
                        model.ListaAutoguardado = servFictec.ListarVersionAutoguardadoExtranet(modelWeb.Ftenvcodi);
                    }

                    model.Resultado = flagHuboAutoguardado ? "1" : "0";
                }
                else
                {
                    model.Resultado = "0";
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

        [HttpPost]
        public async Task<JsonResult> AgregarEquipoVersion(int codigoEnvio, int codigoVersion, string codigoEquipos = "")
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //guardar cambios
                if (codigoVersion > 0)
                {
                    var objEnvio = servFictec.GetByIdFtExtEnvio(codigoEnvio);

                    servFictec.ListarEquipoAdicionalSinAutoguardado(true, codigoVersion, objEnvio.Emprcodi, objEnvio.Ftetcodi, objEnvio.Ftprycodi ?? 0, codigoEquipos
                                                    , out List<string> listaEqNuevo, out List<int> listaFteeqcodiUpdate, out List<int> listaFteeqcodiEliminar);

                    //crear una versión ficticia (no se considera versión temporal hasta que haya un autoguardado manual o automático desde el navegador web)
                    string carpetaUploadTemporal = AppDomain.CurrentDomain.BaseDirectory + ConstantesFormato.FolderUpload + "FTExt_" + DateTime.Now.Ticks + "\\";
                    FtExtEnvioDTO objEnvioTemp = await servFictec.GetEnvioTemporalConexIntegModif(objEnvio.Emprcodi, objEnvio.Ftetcodi, objEnvio.Ftprycodi ?? 0,
                                                                                    string.Join(",", listaEqNuevo), base.UserEmail, carpetaUploadTemporal);

                    //crear una versión ficticia (no se considera versión temporal hasta que haya un autoguardado manual o automático desde el navegador web)
                    //Actualizar datos con casos especiales y guardar en bd
                    objEnvioTemp = servFictec.CrearOActualizarVersionTrabajoFromFichaVigente(codigoEnvio, codigoVersion, objEnvioTemp,
                                                                            carpetaUploadTemporal, listaFteeqcodiUpdate, new List<int>());

                    //crear una copia de los nuevos equipos del envio para luego verificar cuales son los cambios o para realizar la opción "Limpiar"

                    var fteeqcodisNuevo = string.Join(",", objEnvioTemp.VersionActual.ListaEquipoEnvio.Where(x => listaEqNuevo.Contains(x.TipoYCodigo)).Select(x => x.Fteeqcodi).ToList());
                    servFictec.CrearOActualizarVersionTrabajoFromVersionBD(codigoEnvio, codigoVersion,
                                                            objEnvio.Estenvcodi, ConstantesFichaTecnica.GuardadoTemporalFTVigente, ConstantesFichaTecnica.FormatoConexIntegModif, base.UserEmail,
                                                            objEnvio.FtevercodiTemporalFTVigente, fteeqcodisNuevo);

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
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

        [HttpPost]
        public JsonResult QuitarEquipo(int fteeqcodi)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                base.ValidarSesionJsonResult();
                if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                //baja lógica
                if (fteeqcodi > 0)
                {
                    //verificar envio seleccionado
                    FtExtEnvioEqDTO envioEq = servFictec.GetByIdFtExtEnvioEq(fteeqcodi);

                    //modificar flag estado
                    envioEq.Fteeqestado = "N";
                    servFictec.ActualizarEnvioEq(envioEq);

                    model.Resultado = "1";
                }
                else
                {
                    model.Resultado = "0";
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

        [HttpPost]
        public JsonResult ValidarCambiosSinGuardar(FTReporteExcel modelWeb)
        {
            EnvioFormatoModel model = new EnvioFormatoModel();

            try
            {
                //guardar cambios
                bool flagPuedeHaberAutoguardado = modelWeb.HayPendiente1erAutoguardado || !string.IsNullOrEmpty(modelWeb.FteeqcodisLimpiar)
                            || servFictec.ExisteCambioSinAutoguardar(modelWeb.Ftenvcodi, modelWeb.Ftevercodi, modelWeb.TipoAutoguardado, new List<FTReporteExcel>() { modelWeb });

                model.Resultado = flagPuedeHaberAutoguardado ? "1" : "0";
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

        private string GetKeyCookie(int idEnvio, int idEmpresa, int idEtapa, int idProyecto, int idTipoFormato)
        {
            return string.Format("ENV{0}_FMT_{4}_EMP{1}_ET{2}_PR{3}", idEnvio, idEmpresa, idEtapa, idProyecto, idTipoFormato);
        }

        #endregion

        #region Envío - Documentos

        [HttpPost]
        public JsonResult VistaPreviaArchivoEnvio(int idEnvio, int idVersion, int idElemento, string fileName, string tipoArchivo)
        {
            FTArchivoModel model = new FTArchivoModel();

            try
            {
                base.ValidarSesionUsuario();

                string pathSesionID = servFictec.GetPathSubcarpeta(ConstantesFichaTecnica.SubcarpetaSolicitud) + servFictec.GetSubcarpetaEnvio(idEnvio, idVersion, idElemento, tipoArchivo) + @"/";
                string pathDestino = ConstantesFichaTecnica.RutaReportes;
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

        /// <summary>
        /// Descargar el archivo que está en el FileServer
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="idConcepto"></param>
        /// <param name="idEnvio"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoEnvio(int idEnvio, int idVersion, int idElemento, string tipoArchivo, string fileName, string fileNameOriginal)
        {
            base.ValidarSesionUsuario();
            if (!EsAgente()) throw new ArgumentException(Constantes.MensajePermisoNoValido);

            byte[] buffer = servFictec.GetBufferArchivoEnvioFinal(idEnvio, idVersion, idElemento, tipoArchivo, fileName);

            var archivo = File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOriginal);

            return archivo;
        }

        #endregion
    }
}