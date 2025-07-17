using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Combustibles.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
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

namespace COES.MVC.Extranet.Areas.Combustibles.Controllers
{
    public class EnvioGasController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        CombustibleAppServicio servicioCombustible = new CombustibleAppServicio();

        #region Declaración de variables

        readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioGasController));
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

            ListarEmpresaAgente(ConstantesAppServicio.ParametroDefecto, out List<SiEmpresaDTO> listaEmpresas, out List<GenericoDTO> listaTipoCentral);
            model.ListaEmpresas = listaEmpresas;
            model.FlagCentralExistente = listaTipoCentral.Find(x => x.String1 == ConstantesCombustibles.CentralExistente) != null ? 1 : 0;
            model.FlagCentralNuevo = listaTipoCentral.Find(x => x.String1 == ConstantesCombustibles.CentralNueva) != null ? 1 : 0;

            return View(model);
        }

        /// <summary>
        /// Devuelve listado de envios y el html de las carpetas de envios
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
                base.ValidarSesionJsonResult();

                DateTime fechaInicio = DateTime.ParseExact(finicios, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(ffins, ConstantesAppServicio.FormatoMesAnio, CultureInfo.InvariantCulture);
                fechaFin = fechaFin.AddMonths(1).AddDays(-1);
                empresas = string.IsNullOrEmpty(empresas) ? ConstantesAppServicio.ParametroDefecto : empresas;

                if (fechaInicio < fechaFin.AddYears(-1).AddSeconds(-1))
                    throw new ArgumentException("El lapso de tiempo no puede ser mayor a un año.");

                model.ListadoEnvios = servicioCombustible.ObtenerListadoEnvios(empresas, idEstado, fechaInicio, fechaFin, ConstantesCombustibles.CombustiblesGaseosos)
                                                    .Where(x => x.Cbenvtipocarga != "IPC").ToList(); //OMITIR lA PRIMERA carga INTRANET
                model.IdEstado = idEstado;
                model.HtmlCarpeta = servicioCombustible.GenerarHtmlCarpeta(empresas, idEstado, fechaInicio, fechaFin, "IPC", (int)ConstantesCombustibles.Interfaz.Extranet);

                //Guarda el agente que abrio la carpeta de cierto estado de los envíos (fecha/usuario recepcion)
                if (EsHabilitadoAutoguardar())
                {
                    int num = servicioCombustible.GuardarFechaRecepcionDeCargo(model.ListadoEnvios, idEstado, base.UserEmail);
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

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
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
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string nameFile = "RptEnvios_" + estadoDesc + ".xlsx";

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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte+ nombreArchivo;

            //eliminar archivo temporal
            byte[] buffer = null;
            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, nombreArchivo); //exportar xlsx y xlsm
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
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (string.IsNullOrEmpty(motivo))
                {
                    throw new ArgumentException("No ingresó motivo.");
                }

                //envio
                CbEnvioDTO regEnvio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(regEnvio.Emprcodi, regEnvio.Cbenvususolicitud);

                servicioCombustible.CancelarEnvioExtranetPr31Gaseoso(regEnvio, motivo, base.UserEmail, otrosUsuariosEmpresa);
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

        #region Envío - Información Formulario

        public ActionResult EnvioCombustibleGas(int? idEnvio, string tipoCentral, string tipoOpcion)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (this.IdOpcion == null) return base.RedirectToHomeDefault();

            if (tipoCentral == null || (tipoCentral != ConstantesCombustibles.CentralNueva && tipoCentral != ConstantesCombustibles.CentralExistente))
                return base.RedirectToHomeDefault();

            CombustibleGasModel model = new CombustibleGasModel();

            model.IdEnvio = idEnvio ?? 0;
            model.HabilitarAutoguardado = EsHabilitadoAutoguardar() ? 1 : 0; //el autoguardado solo es para rol Usuario Extranet (agentes)
            //model.HayAutoguardados = ObtenerAutoguardados(idEnvio,tipoCentral)

            if (idEnvio > 0)
            {
                CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio.Value);
                model.IdEmpresa = envio.Emprcodi;
                model.Emprnomb = envio.Emprnomb;

                model.TipoCentral = envio.Cbenvtipocentral;
                model.TipoOpcion = envio.TipoOpcionAsignado;

                var listaEmpresa = new List<SiEmpresaDTO>();
                listaEmpresa.Add(new SiEmpresaDTO() { Emprcodi = envio.Emprcodi, Emprnomb = envio.Emprnomb });
                model.ListaEmpresas = listaEmpresa;
                model.IdEstado = envio.Estenvcodi;
                model.TienePermisoEditar = envio.EsEditableExtranet;

                //Lista desplegable mes de vigencia
                var sMes = envio.Cbenvfechaperiodo.Value.ToString(ConstantesAppServicio.FormatoMes);
                model.ListaMes = new List<GenericoDTO>();
                model.ListaMes.Add(new GenericoDTO() { String1 = sMes, String2 = sMes });

                //Guarda el usuario que abrió el envío por primera vez (fecha/usuario lectura)
                if (EsHabilitadoAutoguardar())
                {
                    int num = servicioCombustible.GuardarFechaLecturaDeCargo(envio, base.UserEmail);
                }
            }
            else
            {
                model.TipoCentral = tipoCentral;
                model.TipoOpcion = tipoOpcion;

                ListarEmpresaAgente(tipoCentral, out List<SiEmpresaDTO> listaEmpresas, out List<GenericoDTO> listaTipoCentral);
                model.ListaEmpresas = listaEmpresas;
                model.IdEstado = ConstantesCombustibles.EstadoSolicitud;
                model.TienePermisoEditar = true;

                bool esMesSiguiente = ConstantesCombustibles.CentralExistente == tipoCentral;

                //Lista desplegable mes de vigencia
                DateTime mesVigencia = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                if (esMesSiguiente) mesVigencia = mesVigencia.AddMonths(1);
                var sMes = mesVigencia.ToString(ConstantesAppServicio.FormatoMes);
                model.ListaMes = new List<GenericoDTO>();
                model.ListaMes.Add(new GenericoDTO() { String1 = sMes, String2 = sMes });
            }

            model.TipoCentralDesc = model.TipoCentral == ConstantesCombustibles.CentralExistente ? "Existente" : "Nueva";
            model.MinutosAutoguardado = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantesCombustibles.KeyFlagPR31MinutosAutoguardado]);

            //eliminar los archivos temporales de vista previa
            servicioCombustible.EliminarArchivosReporte();

            return View(model);
        }

        /// <summary>
        /// Envia model para visualizacion de grilla excel de combustible
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, int idVersion, string tipoCentral, string tipoOpcion, string mesVigencia)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdEnvio = idEnvio;
                model.IdEmpresa = idEmpresa;

                //envio
                model.EsIntranet = false;
                model.Envio = idEnvio != 0 ? servicioCombustible.GetByIdCbEnvio(idEnvio) : new CbEnvioDTO();
                model.TipoEnvio = idEnvio != 0 ? model.Envio.Cbenvtipoenvio.Value : -1;
                DateTime fechaVigencia = idEnvio != 0 ? model.Envio.Cbenvfechaperiodo.Value : DateTime.ParseExact(mesVigencia, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

                ValidarEmpresaUsuario(model.IdEmpresa);

                //formato 3
                model.ListaFormularioCentral = servicioCombustible.ListarFormularioCentralByEnvio(false, idEnvio, idVersion, idEmpresa, tipoCentral, tipoOpcion, fechaVigencia, ConstantesCombustibles.ArchivosTotales, out bool yaExisteSolicitud);
                model.TienePermisoEditar = model.ListaFormularioCentral[0].EsEditable;
                model.FlagExisteEnvio = yaExisteSolicitud;

                //archivo
                bool esEnvioAsignado = tipoOpcion == "NA" || model.Envio.Estenvcodi == ConstantesCombustibles.EstadoSolicitudAsignacion || model.Envio.Estenvcodi == ConstantesCombustibles.EstadoAsignado;
                if (!esEnvioAsignado)
                {
                    model.FormularioSustento = servicioCombustible.GetHandsonGasArchivoSustento(false, idEnvio, idVersion, false, null, new List<CbArchivoenvioDTO>()
                                                                    , model.ListaFormularioCentral[0].IncluirObservacion, model.ListaFormularioCentral[0].Estenvcodi);

                    model.FormularioSustento.EsEditable = model.ListaFormularioCentral[0].EsEditable && !model.FormularioSustento.SeccionCombustible.EsSeccionSoloLectura;
                    model.FormularioSustento.Readonly = !model.FormularioSustento.EsEditable;
                    model.FormularioSustento.EsEditableObs = model.ListaFormularioCentral[0].EsEditableObs;
                    model.FormularioSustento.IncluirObservacion = model.ListaFormularioCentral[0].IncluirObservacion;
                }

                model.ListadoVersiones = servicioCombustible.GetListaVersionesXEnvio(idEnvio, false);

                //copiar del fileserver al temporal para que se visualicen en pantalla
                if (idEnvio != 0)
                {
                    if (idVersion == 0) idVersion = model.Envio.Cbvercodi;
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
                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                model.ListaFormularioCentral = servicioCombustible.ActualizarHandsonFormulario(false, formulario.ListaFormularioCentral, false, formulario.Equicodi
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

                //si inicialmente el formulario es editable entonces se debe mantener hasta que "envíe al COES"
                var flagTabEditable = formulario.FormularioSustento.EsEditable;
                var flagTabEditableObs = formulario.FormularioSustento.EsEditableObs;
                var flagTabIncluirObs = formulario.FormularioSustento.IncluirObservacion;

                model.FormularioSustento = servicioCombustible.GetHandsonGasArchivoSustento(false, 0, 0, true, formulario.FormularioSustento.SeccionCombustible.ListaObs
                                                            , formulario.FormularioSustento.SeccionCombustible.ListaArchivo
                                                            , flagTabIncluirObs, formulario.FormularioSustento.Estenvcodi);
                model.FormularioSustento.EsEditable = flagTabEditable;
                model.FormularioSustento.EsEditableObs = flagTabEditableObs;
                model.FormularioSustento.IncluirObservacion = flagTabIncluirObs;

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
        public JsonResult GrabarDatosCombustible(string data)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                DateTime fechaRegistro = DateTime.Now;
                DateTime fechaVigencia = new DateTime(fechaRegistro.Year, fechaRegistro.Month, 1).AddMonths(1); //existente
                if (formulario.TipoCentral == ConstantesCombustibles.CentralNueva) fechaVigencia = fechaVigencia.AddMonths(-1);

                ValidarEmpresaUsuario(formulario.IdEmpresa);

                //Solo autoguardar para agentes
                if (!EsHabilitadoAutoguardar() && formulario.TipoGuardado != ConstantesCombustibles.GuardadoTemporal)
                    throw new ArgumentException("No está permitido el autoguardado.");

                if (formulario.TipoGuardado == ConstantesCombustibles.GuardadoOficial
                    && servicioCombustible.ExisteSolicitudXTipoCombustibleYVigenciaYTipocentralCbEnvio(formulario.IdEnvio, formulario.IdEmpresa, ConstantesCombustibles.EstcomcodiGas, fechaVigencia
                                                                , formulario.TipoCentral, -1, out int idEnvioExistente))
                    throw new ArgumentException("Ya existe una solicitud para el periodo seleccionado.");

                string otrosUsuariosEmpresa = ObtenerCCcorreosAgente(formulario.IdEmpresa, base.UserEmail);

                if (formulario.IdEnvio > 0)
                {
                    var regEnvio = servicioCombustible.GetByIdCbEnvio(formulario.IdEnvio);
                    if (!regEnvio.EsEditableExtranet)
                        throw new ArgumentException("No tiene permisos para realizar esta acción o el plazo para el envío ha vencido.");
                }
                CbEnvioDTO objEnvioGuardado = servicioCombustible.RealizarSolicitudCostoCombustibleGas(formulario.IdEnvio, formulario.IdEmpresa, formulario.TipoCentral, formulario.TipoOpcion
                                                                , base.UserEmail, "A", "P"
                                                                , formulario.ListaFormularioCentral, formulario.FormularioSustento, otrosUsuariosEmpresa, "", "E"
                                                                , formulario.TipoGuardado, formulario.DescVersion, formulario.CondicionEnvioPrevioTemporal, (int)ConstantesCombustibles.Interfaz.Extranet);
                if (objEnvioGuardado.Cbenvcodi != 0)
                {
                    //manejo de archivos en FileServer
                    var listaArchivoFisico = servicioCombustible.ListarArchivoFisicoFromEnvio(objEnvioGuardado);
                    servicioCombustible.GuardarArchivosFinal(GetCurrentCarpetaSesion(), objEnvioGuardado.Cbenvcodi, objEnvioGuardado.Cbvercodi
                                                    , formulario.TipoGuardado == ConstantesCombustibles.GuardadoTemporal, listaArchivoFisico);

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

        [HttpPost]
        public JsonResult ExportarFormularioEnvio(string data)
        {
            CombustibleModel model = new CombustibleModel();

            try
            {
                base.ValidarSesionJsonResult();

                FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesCombustibles.FolderReporte;
                servicioCombustible.GenerarArchivoExcelFormularioGaseosoEnvio(ruta, ConstantesCombustibles.NombreReporteFormularioEnvios, formulario.ListaFormularioCentral, ConstantesCombustibles.ArchivosTotales, false);

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
                List<CeldaErrorCombustible> listaError = new List<CeldaErrorCombustible>();
                if (string.IsNullOrEmpty(this.NombreFile))
                {
                    listaError.Add(new CeldaErrorCombustible() { Mensaje = "No se importó el archivo" });
                }
                else
                {
                    FormularioGasModel formulario = JsonConvert.DeserializeObject<FormularioGasModel>(data);

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

        public virtual ActionResult DescargarArchivoTemporal(int idConcepto, string fileName)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servicioCombustible.GetBufferArchivoEnvioTemporal(GetCurrentCarpetaSesion(), idConcepto, fileName);

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
        public virtual ActionResult DescargarArchivoEnvio(int idEnvio, int idVersion, int idConcepto, string fileName)
        {
            base.ValidarSesionUsuario();

            byte[] buffer = servicioCombustible.GetBufferArchivoEnvioFinal(idEnvio, idVersion, idConcepto, fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        private string GetCurrentCarpetaSesion()
        {
            return base.UserEmail;
        }

        #endregion

        /// <summary>
        /// Listar empresas permitidas segun usuario
        /// </summary>
        /// <param name="opAccesoEmpresa"></param>
        /// <returns></returns>
        private void ListarEmpresaAgente(string tipoCentral, out List<SiEmpresaDTO> listaEmpresas, out List<GenericoDTO> listaTipoCentral)
        {
            var listaEmpAll = servicioCombustible.ObtenerListadoEmpresas(tipoCentral);

            bool permisoEmpresas = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);

            listaEmpresas = new List<SiEmpresaDTO>();
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

            listaTipoCentral = servicioCombustible.ListarTipoCentralXEmpresa(listaEmpresas.Select(x => x.Emprcodi).ToList());
        }

        private void ValidarEmpresaUsuario(int emprcodiConsulta)
        {
            ListarEmpresaAgente(ConstantesAppServicio.ParametroDefecto, out List<SiEmpresaDTO> listaEmpresas, out List<GenericoDTO> listaTipoCentral);

            if (listaEmpresas.Find(x => x.Emprcodi == emprcodiConsulta) == null)
                throw new ArgumentException("Usted no tiene acceso a esta empresa.");
        }

        private bool EsHabilitadoAutoguardar()
        {
            //agente
            if (!base.UserEmail.ToLower().Contains("@coes.org.pe"))
                return true;

            return false; //Usuario COES Administrador o de consulta
        }

        #region Autoguardado

        /// <summary>
        /// Devuelve el id del ultimo envio autoguardado
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="mesVigencia"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BuscarEnviosAutoguardados(int idEnvio, string tipoCentral, string mesVigencia, int idEmpresa, int estenvcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                var lstVigencia = mesVigencia.Split(' ');
                DateTime mesDeVigencia = new DateTime(Int32.Parse(lstVigencia[1]), Int32.Parse(lstVigencia[0]), 1);

                CbEnvioDTO ultimoGuardado = new CbEnvioDTO();


                int debeBuscarAutoguardados = 1;
                //si ya existe solicitud
                if (servicioCombustible.ExisteSolicitudXTipoCombustibleYVigenciaYTipocentralCbEnvio(idEnvio, idEmpresa, ConstantesCombustibles.EstcomcodiGas
                                                                  , mesDeVigencia
                                                                  , tipoCentral, -1, out int idEnvioExistente))
                {
                    debeBuscarAutoguardados = 0;
                }
                if (idEnvio > 0)
                {
                    CbEnvioDTO envio = servicioCombustible.GetByIdCbEnvio(idEnvio);
                    //si el envio es solo lectura o ya pasó el plazo no debe buscar autoguardados
                    if (envio != null && !envio.EsEditableExtranet) debeBuscarAutoguardados = 0;
                }
                model.BuscaAutoguardados = debeBuscarAutoguardados;

                //busco los envios autoguardados 
                List<CbEnvioDTO> lstAutoguardados = servicioCombustible.ObtenerAutoguardados(tipoCentral, mesDeVigencia, idEmpresa, estenvcodi);

                //le quito los envios con errores (estado = E) dado que no seran tomados en cuenta
                List<CbEnvioDTO> lstAutoguardadosSinErrores = lstAutoguardados.Where(c => c.Cbenvestado != ConstantesCombustibles.EstadoConError).ToList();

                bool tieneAutoguardados = lstAutoguardadosSinErrores.Any() ? true : false;
                model.HayAutoguardados = 0;
                if (tieneAutoguardados)
                {
                    model.HayAutoguardados = 1;
                    ultimoGuardado = lstAutoguardadosSinErrores.First();

                    //el último autoguardado temporal tiene datos desactualizados. Respecto a la versi
                    servicioCombustible.ActualizarDatosAutoguardadoConEnvioActual(idEnvio, ultimoGuardado.Cbenvcodi);

                    model.IdEnvio = ultimoGuardado.Cbenvcodi;
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
        /// Devuelve el listado de versiones autoguardadas activas (estado != X)
        /// </summary>
        /// <param name="idEnvio"></param>
        /// <param name="tipoCentral"></param>
        /// <param name="mesVigencia"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="estenvcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BuscarVersionesAutoguardados(int idEnvio, string tipoCentral, string mesVigencia, int idEmpresa, int estenvcodi)
        {
            CombustibleGasModel model = new CombustibleGasModel();

            try
            {
                base.ValidarSesionJsonResult();

                var lstVigencia = mesVigencia.Split(' ');
                DateTime mesDeVigencia = new DateTime(Int32.Parse(lstVigencia[1]), Int32.Parse(lstVigencia[0]), 1);

                //busco los envios autoguardados con y sin errores
                List<CbEnvioDTO> lstAutoguardados = servicioCombustible.ObtenerAutoguardados(tipoCentral, mesDeVigencia, idEmpresa, estenvcodi);

                List<CbVersionDTO> lstVersionesAutoguardados = new List<CbVersionDTO>();

                bool tieneAutoguardados = lstAutoguardados.Any() ? true : false;
                model.HayAutoguardados = 0;
                if (tieneAutoguardados)
                {
                    model.HayAutoguardados = 1;

                    //busco las versiones de los envios autoguardados activos
                    lstVersionesAutoguardados = servicioCombustible.ObtenerVersionesAutoguardados(lstAutoguardados);
                }

                model.ListaAutoguardados = lstVersionesAutoguardados;
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

    }
}