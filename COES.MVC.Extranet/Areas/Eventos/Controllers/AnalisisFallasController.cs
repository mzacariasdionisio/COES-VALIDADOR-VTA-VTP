using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Eventos.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.Evento;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.Eventos.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Eventos.Controllers
{
    public class AnalisisFallasController : BaseController
    {
        private readonly SeguridadServicioClient _seguridadServicio;
        private readonly AnalisisFallasAppServicio _analisisFallasAppServicio;
        private readonly EventosAppServicio _servicioEvento;

        public AnalisisFallasController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _seguridadServicio = new SeguridadServicioClient();
            _analisisFallasAppServicio = new AnalisisFallasAppServicio();
            _servicioEvento = new EventosAppServicio();
        }

        #region Declaración de variables de Sesión
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
        #endregion

        #region Interrupcion suministros

        /// <summary>
        /// Pagina detaller interrupción suministros
        /// </summary>
        /// <param name="id">afecodi</param>
        /// <returns></returns>
        public ActionResult Interrupcionsuministro(int id)
        {
            if (!base.IsValidSesionView()) return RedirectToLogin();

            var model = new AnalisisFallasModel()
            {
                ListaEmpresa = ObtenerEmpresasPorUsuario(User.Identity.Name).OrderBy(x => x.Emprnomb).ToList(),
                ListaTipoInformacion = _analisisFallasAppServicio.ListSiFuentedatosByFdatpadre(ConstantesExtranetCTAF.FdatcodiCTAFExtranet).OrderBy(x => x.Fdatcodi).ToList(),
            };

            AnalisisFallaDTO oAnalisisFallaDTO = _analisisFallasAppServicio.ObtenerAnalisisFalla(id);
            model.LstEvento = _servicioEvento.ListarEventosSCO(oAnalisisFallaDTO);
            var idUltimo = model.LstEvento.OrderBy(x => x.EVENINI).First().EVENCODI;
            EventoDTO oEventoDTO = _analisisFallasAppServicio.ObtenerEvento((int)(idUltimo));
            oEventoDTO.Eveninidesc = oEventoDTO.EVENINI.Value.ToString(Constantes.FormatoFechaFull);
            oEventoDTO.Evenfindesc = oEventoDTO.EVENFIN.Value.ToString(Constantes.FormatoFechaFull);
            oEventoDTO.Afeanio = oAnalisisFallaDTO.AFEANIO.ToString();
            oEventoDTO.Afecorr = oAnalisisFallaDTO.AFECORR.ToString();

            ViewBag.Afecodi = id;

            oEventoDTO = _analisisFallasAppServicio.ValidarEvento(oEventoDTO, oAnalisisFallaDTO);

            model.oAnalisisFallaDTO = oAnalisisFallaDTO;
            model.oEventoDTO = oEventoDTO;
            //Equipos Involucrados
            model.ListaEquipo = _servicioEvento.GetEquiposPorEvento(oAnalisisFallaDTO.EVENCODI);

            //No mostrar la opción Interrupción por activación del ERACMF cuando ERACMF es NO
            if (oAnalisisFallaDTO.AFEERACMF != ConstantesAppServicio.SI)
                model.ListaTipoInformacion = model.ListaTipoInformacion.Where(x => x.Fdatcodi != (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF).ToList();

            //Obtener Interrupción
            EventoDTO eventoInterrup = _analisisFallasAppServicio.ObtenerInterrupcionByAfecodi(id);
            model.oEventoDTO.FechaInterrupcion = eventoInterrup.Afefechainterr != null ? eventoInterrup.Afefechainterr.Value.ToString(Constantes.FormatoFechaFull) : string.Empty;
            model.oEventoDTO.NumMaxSegundosInicio = ConstantesExtranetCTAF.NumeroMaxSegundosFechaIniExtranet;


            return View(model);
        }

        /// <summary>
        /// Upload Interrupcion Suministro
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadInterrupcionSuministro(FormCollection formCollection)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();

                int.TryParse(formCollection["emprcodi"], out int emprcodi);
                int.TryParse(formCollection["afecodi"], out int afecodi);
                int.TryParse(formCollection["idtipoinformacion"], out int idtipoinformacion);

                Stream stremExcel = (Request.Files.Count >= 1) ? Request.Files[0].InputStream : null;

                List<AfInterrupSuministroDTO> lstInterrSuminExcel = new List<AfInterrupSuministroDTO>();

                if (stremExcel != null)
                    lstInterrSuminExcel = _analisisFallasAppServicio.ObtenerInterrupcionSuministroDeDataExcel(stremExcel, idtipoinformacion);

                if (idtipoinformacion == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                {
             

                    model.ListaInterrupSuministro = _analisisFallasAppServicio.ObtenerInterrupcionSuministroUnionDb(afecodi, emprcodi, lstInterrSuminExcel);
                }
                else if (idtipoinformacion == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion || idtipoinformacion == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                {
                    model.ListaInterrupSuministro = lstInterrSuminExcel;
                }
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Cargar Interrupcion Suministro
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="afecodi"></param>
        /// <param name="fdatcodi"></param>
        /// <param name="enviocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarInterrupcionSuministro(int emprcodi, int afecodi, int fdatcodi, int enviocodi)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();

                //Envios
                List<MeEnvioDTO> lstEnvios = _analisisFallasAppServicio.ObtenerEnviosInterrupSuministro(emprcodi, afecodi, fdatcodi);
                model.ListaMeEnvio = _analisisFallasAppServicio.PrepararListaEnviosInterrupSuministro(lstEnvios).OrderByDescending(x => x.Enviofecha).ToList();

                //Validar Plazo 
                EventoDTO eventoInterrup = _analisisFallasAppServicio.ObtenerInterrupcionByAfecodi(afecodi);

                _analisisFallasAppServicio.DeterminarPlazoInterrupcion(eventoInterrup);
                eventoInterrup.EmpresaActivo = _analisisFallasAppServicio.EsEmpresaActivo(emprcodi);
                eventoInterrup.FechaInterrupcion = eventoInterrup.FechaInterrupcion != null? eventoInterrup.Afefechainterr.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
                model.oEventoDTO = eventoInterrup;

                //Generar handson
                if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF)
                {
                    model.ListaInterrupSuministro = _analisisFallasAppServicio.ObtenerDataEracmfEvento(afecodi, emprcodi, enviocodi);
                }
                else if (fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.Interrupcion || fdatcodi == (int)ConstantesExtranetCTAF.Fuentedato.ReduccionSuministros)
                {
                    model.ListaInterrupSuministro = _analisisFallasAppServicio.ObtenerUltimoEnvioInterrupcionSuministro(afecodi, emprcodi, fdatcodi, enviocodi);
                }

                //Mostrar descripción del envio realizado
                if (enviocodi > 0)
                {
                    var regEnvio = model.ListaMeEnvio.Find(x => x.Enviocodi == enviocodi);
                    model.FechaEnvio = regEnvio.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    model.oEventoDTO.EnPlazo = regEnvio.Envioplazo == "En plazo";
                }
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Descargar Formato Interrupcion Suministro
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarFormatoInterrupcionSuministro(int emprcodi, int idtipoinformacion, int afecodi, List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();

                string rutaBase = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento;

                AnalisisFallaDTO oAnalisisFallaDTO = _analisisFallasAppServicio.ObtenerAnalisisFalla(afecodi);
                EventoDTO oEventoDTO = _analisisFallasAppServicio.ObtenerEvento(oAnalisisFallaDTO.EVENCODI);
                oEventoDTO.CODIGO = oAnalisisFallaDTO.CodigoEvento;

                string nombreArchivoCompleto = _analisisFallasAppServicio.ObtenerNombreArchivoFormatoInterruSumini(oEventoDTO, rutaBase, idtipoinformacion, emprcodi, afecodi, listaInterrupcionSuministro);
                model.Resultado = nombreArchivoCompleto;
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Envio Interrupcion Suministro
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="afecodi"></param>
        /// <param name="listaInterrupcionSuministro"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnvioInterrupcionSuministro(int emprcodi, int idtipoinformacion, int afecodi, List<AfInterrupSuministroDTO> listaInterrupcionSuministro)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!_analisisFallasAppServicio.EsEmpresaActivo(emprcodi)) throw new Exception(Constantes.MensajeEmpresaNoVigente);

                int enviocodi = _analisisFallasAppServicio.EnviarDatosInterrupcionSuministro(User.Identity.Name, emprcodi, idtipoinformacion, afecodi, listaInterrupcionSuministro);

                //Validar Plazo 
                EventoDTO eventoInterrup = _analisisFallasAppServicio.ObtenerInterrupcionByAfecodi(afecodi);

                _analisisFallasAppServicio.DeterminarPlazoInterrupcion(eventoInterrup);
                eventoInterrup.EmpresaActivo = _analisisFallasAppServicio.EsEmpresaActivo(emprcodi);

                model.oEventoDTO = eventoInterrup;

                //Mostrar descripción del envio realizado
                if (enviocodi > 0)
                {
                    List<MeEnvioDTO> lstEnvios = _analisisFallasAppServicio.ObtenerEnviosInterrupSuministro(emprcodi, afecodi, idtipoinformacion);
                    model.ListaMeEnvio = _analisisFallasAppServicio.PrepararListaEnviosInterrupSuministro(lstEnvios).OrderByDescending(x => x.Enviofecha).ToList();
                    var regEnvio = model.ListaMeEnvio.Find(x => x.Enviocodi == enviocodi);
                    model.FechaEnvio = regEnvio.Enviofecha.Value.ToString(ConstantesAppServicio.FormatoFechaFull);
                    model.oEventoDTO.EnPlazo = regEnvio.Envioplazo == "En plazo";
                }

                model.Enviocodi = enviocodi;
                model.Resultado = "exito";
                model.StrMensaje = "El envío se realizó con éxito.";
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = "error";
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

        #region Listado 

        /// <summary>
        /// Pagina inicial del modulo
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return RedirectToLogin();

            ViewBag.EmpresaInvolucrada = _analisisFallasAppServicio.ObtenerEmpresasInvolucrada();
            ViewBag.EmpresaPropietaria = _analisisFallasAppServicio.ObtenerEmpresaPropietaria();
            ViewBag.TipoEquipo = _analisisFallasAppServicio.ObtenerTipoEquipo();

            ViewBag.FechaInicio = DateTime.Now.AddMonths(-2).ToString(Constantes.FormatoFecha);
            ViewBag.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View();
        }

        /// <summary>
        /// Listar las interrupciones
        /// </summary>
        /// <param name="miDataM"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoInterrupciones(AnalisisFallasModel miDataM)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();

            try
            {
                base.ValidarSesionJsonResult();

    

                EventoDTO oEventoDTO = new EventoDTO();
                oEventoDTO.EmpresaPropietaria = miDataM.EmpresaPropietaria;
                oEventoDTO.EmpresaInvolucrada = "0";
                oEventoDTO.TipoEquipo = "0";
                oEventoDTO.Estado = "N";
                oEventoDTO.RNC = "N";
                oEventoDTO.ERACMF = miDataM.ERACMF;
                oEventoDTO.ERACMT = "N";
                oEventoDTO.EDAGSF = "N";
                oEventoDTO.DI = miDataM.DI;
                oEventoDTO.DF = miDataM.DF;
                oEventoDTO.EveSinDatosReportados = miDataM.EveSinDatosReportados;
                oEventoDTO.EVENASUNTO = miDataM.Descripcion;
                if (oEventoDTO.EveSinDatosReportados == ConstantesAppServicio.NO)
                {
                    oEventoDTO.DI = "01/01/1900";
                    oEventoDTO.DF = "31/12/3000";
                }

                //Envios pendientes de informacion sobre las empresas que puedo cargar información, p.e. un Agente solo tiene normalmente una empresa, la validación debe ser sobre esa empresa no sobre todas las empresas.
                string parametroEmpresaAgente = "0";
                List<int> listaEmprcodi = ObtenerEmpresasPorUsuario(User.Identity.Name).Select(x => x.Emprcodi).ToList();
                if (listaEmprcodi.Count > 0)
                {
                    if (listaEmprcodi.Count < 100)
                        parametroEmpresaAgente = string.Join(",", listaEmprcodi);
                    else
                        parametroEmpresaAgente = ConstantesAppServicio.ParametroDefecto;
                }
                oEventoDTO.ListaEmprcodi = parametroEmpresaAgente;

                model.LstEvento = _analisisFallasAppServicio.ConsultarInterrupcionesSuministros(oEventoDTO).Where(x => x.Afefechainterr != null).ToList();

                //calcular plazo envío
                _analisisFallasAppServicio.CalcularPlazoInterrupcion(model.LstEvento);
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }

            return PartialView(model);
        }

        #endregion

        #region Solicitudes
        public ActionResult IndexSolicitudes()
        {
            if (!base.IsValidSesionView()) return RedirectToLogin();

            var model = new AnalisisFallasModel()
            {
                ListaEmpresa = ObtenerEmpresasPorUsuario(User.Identity.Name),
                ListaTipoInformacion = _analisisFallasAppServicio.ListSiFuentedatosByFdatpadre(ConstantesExtranetCTAF.FdatcodiCTAFExtranet),
            };

            return View(model);
        }

        /// <summary>
        /// Listado Solicitudes
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoSolicitudes(string emprcodi)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();

                AfSolicitudRespDTO SolicitudDTO = new AfSolicitudRespDTO();
                SolicitudDTO.Empresa = emprcodi;
                SolicitudDTO.Df = DateTime.Now.ToString(Constantes.FormatoFecha);
                SolicitudDTO.Sorespestadosol = "T";//TODOS
                model.ListSolicitudes = _analisisFallasAppServicio.ConsultarSolicitudesAsignacion(SolicitudDTO);
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return PartialView(model);
        }

        /// <summary>
        /// Exportación de archivo Excel de reporte seleccionado
        /// </summary>
        /// <param name="tipoReporte"></param>
        /// <param name="afecodi"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteSolicitudes(string emprcodi)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();

            try
            {
                base.ValidarSesionUsuario();

                AfSolicitudRespDTO solicitudDTO = new AfSolicitudRespDTO();
                solicitudDTO.Empresa = emprcodi;
                solicitudDTO.Df = DateTime.Now.ToString(Constantes.FormatoFecha);
                solicitudDTO.Sorespestadosol = "T";//TODOS

                model.NombreArchivo = _analisisFallasAppServicio.GenerarReporteSolicitudesExcel(solicitudDTO, false);
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

        public ActionResult NuevaSolicitud()
        {
            if (!base.IsValidSesionView()) return RedirectToLogin();

            // Ruta base de Eventos          
            string pathBaseEventos = base.PathFiles + "\\" + ConstantesExtranetCTAF.EventosFile;
            // Obtener un identificador unico
            string currentUserSession = HttpContext.Session.SessionID;

            // Crear carpeta temporal SNombreCarpetaTemporal
            string nombreCarpetaTemporal = ConstantesExtranetCTAF.SNombreCarpetaTemporal + "_" + currentUserSession;
            FileServer.CreateFolder(pathBaseEventos, nombreCarpetaTemporal, null); // para asegurarnos de su existencia
            string pathTemporal = pathBaseEventos + nombreCarpetaTemporal;
            FileServer fs = new FileServer();
            fs.DeleteFolder(pathTemporal);// borramos la carpeta    
            FileServer.CreateFolder(pathBaseEventos, nombreCarpetaTemporal, null); // creamos la nueva carpeta vacía

            // Crear segunda carpeta temporal 
            string nombreCarpetaTemporal2 = ConstantesExtranetCTAF.SNombreCarpetaTemporal2 + "_" + currentUserSession;
            FileServer.CreateFolder(pathBaseEventos, nombreCarpetaTemporal2, null); // para asegurarnos de su existencia
            string pathTemporal2 = pathBaseEventos + nombreCarpetaTemporal2;
            FileServer fs2 = new FileServer();
            fs2.DeleteFolder(pathTemporal2);// borramos la carpeta    
            FileServer.CreateFolder(pathBaseEventos, nombreCarpetaTemporal2, null); // creamos la nueva carpeta vacía

            var model = new AnalisisFallasModel()
            {
                ListaEmpresa = ObtenerEmpresasPorUsuario(User.Identity.Name),
                ListaTipoInformacion = _analisisFallasAppServicio.ListSiFuentedatosByFdatpadre(ConstantesExtranetCTAF.FdatcodiCTAFExtranet),
            };

            model.Modulo = ConstantesExtranetCTAF.SModuloEventos;

            return View(model);
        }

        /// <summary>
        /// Enviar Solicitud
        /// </summary>
        /// <param name="solicitud"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EnviarSolicitud(AfSolicitudRespDTO solicitud)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            AfSolicitudRespDTO solicitudDTO = new AfSolicitudRespDTO();
            try
            {
                base.ValidarSesionJsonResult();

                if (!_analisisFallasAppServicio.EsEmpresaActivo(solicitud.Emprcodi))
                {
                    throw new Exception(Constantes.MensajeEmpresaNoVigente);
                }

                // Obtener un identificador unico
                string currentUserSession = HttpContext.Session.SessionID;

                // Crear carpeta temporal          
                string nombreCarpetaTemporal = ConstantesExtranetCTAF.SNombreCarpetaTemporal + "_" + currentUserSession;
                string nombreCarpetaTemporal2 = ConstantesExtranetCTAF.SNombreCarpetaTemporal2 + "_" + currentUserSession;

                // Ruta base a mover           
                string pathTemporalEventos = base.PathFiles + "\\" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal;
                string pathTemporalEventos2 = base.PathFiles + "\\" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal2;

                // Ruta base de Eventos            
                string pathBaseEventos = base.PathFiles + "\\" + ConstantesExtranetCTAF.EventosFile;
                string fechaId = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha) + "_" + DateTime.Today.ToString("hh:mm:ss") + DateTime.Today.ToString("tt");

                fechaId = fechaId.Replace('/', '_').Replace(".", string.Empty).Replace(":", string.Empty).ToString();
                fechaId = fechaId.Trim();

                //La validación de extranet no tiene Acción Grabar (para los agentes) asi que no es necesario poner  if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);
                //Solo debe tener una sesión activa
                try
                {
                    DateTime fechaEventoIni = DateTime.ParseExact(solicitud.FechEvento, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                    solicitud.Sorespfechaevento = fechaEventoIni;
                }
                catch (Exception exFecha)
                {
                    model.Resultado = "-2";
                    model.StrMensaje = "Error: La fecha no tiene el Formato Correcto.";
                    return Json(model);
                }
                string usuario = User.Identity.Name;

                //Validar fecha máxim permitida
                if (solicitud.Sorespfechaevento > DateTime.Now)
                {
                    model.Resultado = "-2";
                    model.StrMensaje = "Error: La fecha y hora del evento no debe ser mayor a la fecha y hora actual.";
                    return Json(model);
                }

                //validar Diferencia de Dias
                var nroDias = this.CalcularDiasHabiles(solicitud.Sorespfechaevento.Value, DateTime.Now);
                if (nroDias > 3)
                {
                    model.Resultado = "-2";
                    model.StrMensaje = "Error: El plazo para enviar la solicitud está vencido de acuerdo a lo establecido en el numeral 9.3 del PR-40.";
                    return Json(model);
                }

                //archivos
                //Crear path temporal
                var archivoInforme = ObtenerArchivos(nombreCarpetaTemporal);
                solicitud.Soresparchivoinf = archivoInforme.ListaDocumentos.First().FileName;

                var archivoOtros = ObtenerArchivos(nombreCarpetaTemporal2);

                var cont = 0;
                foreach (var item in archivoOtros.ListaDocumentos)
                {
                    int count = archivoOtros.ListaDocumentos.Count;
                    string separador = ",";
                    if (cont + 1 < count)
                    {
                        solicitud.Soresparchivootros += item.FileName + separador;
                    }
                    else
                    {
                        solicitud.Soresparchivootros += item.FileName;
                    }
                    cont++;
                }

                //solicitud.Sorespobsarchivo = solicitud.Sorespobsarchivo;
                solicitud.Sorespestadosol = "P";
                solicitud.Sorespusucreacion = usuario;
                solicitud.Sorespfeccreacion = DateTime.Now;

                int envioCodi = _analisisFallasAppServicio.EnviarDatosSolicitud(User.Identity.Name, solicitud);

                solicitud.Enviocodi = envioCodi;

                model.CodSolicitud = _analisisFallasAppServicio.SaveAfSolicitudResp(solicitud);
                int IdSolicitud = model.CodSolicitud;
                FileServer fs = new FileServer();
                FileServer.CreateFolder(pathBaseEventos, IdSolicitud.ToString(), null);
                fs.CortarDirectory(pathTemporalEventos, pathBaseEventos + IdSolicitud + "\\" + ConstantesExtranetCTAF.SNombreCarpetaInforFinal);
                fs.CortarDirectory(pathTemporalEventos2, pathBaseEventos + IdSolicitud + "\\" + ConstantesExtranetCTAF.SNombreCarpetaOtrosArchivos);

                if (IdSolicitud > 0)
                {
                    model.Resultado = "1";
                    model.StrMensaje = "¡La Solicitud se Guardó correctamente!";
                }
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.Resultado = "-1";
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Obtener Archivos
        /// </summary>
        /// <param name="nombreCarpetaTemporal"></param>
        /// <returns></returns>
        public ArchivosModel ObtenerArchivos(string nombreCarpetaTemporal)
        {
            string path = "";
            path = base.PathFiles + "//" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal + "\\";
            ArchivosModel modelArchivos = new ArchivosModel();
            modelArchivos.ListaDocumentos = FileServer.ListarArhivos(path, null);
            return modelArchivos;
        }

        /// <summary>
        /// metodos archivos solicitudes
        /// </summary>
        /// <param name="sFecha"></param>
        /// <param name="sModulo"></param>
        /// <param name="nroOrden"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadSolicitudes(string sFecha, string sModulo, int nroOrden)
        {
            base.ValidarSesionUsuario();

            AnalisisFallasModel model = new AnalisisFallasModel();
            string sNombreArchivo = string.Empty;
            string path = "";
            string sNombreOriginal = "";
            string rootPath = FileServer.GetDirectory();
            string currentUserSession = HttpContext.Session.SessionID;

            if (String.Equals(sModulo, ConstantesExtranetCTAF.SModuloEventos))
            {
                // Obtener path temporal
                string nombreCarpetaTemporal = nroOrden == 1 ? ConstantesExtranetCTAF.SNombreCarpetaTemporal + "_" + currentUserSession : ConstantesExtranetCTAF.SNombreCarpetaTemporal2 + "_" + currentUserSession;
                path = base.PathFiles + "//" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal + "\\";
            }
            else
            {
                path = base.PathFiles + "//";
            }
            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    sNombreOriginal = file.FileName;
                    sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, path + "\\" + sNombreArchivo, null))
                    {
                        FileServer.DeleteBlob(path + "\\" + sNombreArchivo, null);
                    }

                    FileServer.UploadFromStream(file.InputStream, path, sNombreArchivo, null);
                }
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Metodo para eliminar los archivos en un registro nuevo
        /// </summary>
        /// <param name="nombreArchivo">Nombre de archivo</param>
        /// <returns>Entero</returns>
        [HttpPost]
        public int EliminarArchivosNuevo(string nombreArchivo, int nroOrden)
        {
            base.ValidarSesionUsuario();

            //path temporal   
            string currentUserSession = HttpContext.Session.SessionID;
            string nombreCarpetaTemporal = nroOrden == 1 ? ConstantesExtranetCTAF.SNombreCarpetaTemporal + "_" + currentUserSession : ConstantesExtranetCTAF.SNombreCarpetaTemporal2 + "_" + currentUserSession;
            string pathTemporal = base.PathFiles + "//" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal;

            string nombrePath = string.Empty;

            ArchivosModel modelArvhivos = new ArchivosModel();
            modelArvhivos.ListaDocumentos = FileServer.ListarArhivos(pathTemporal + "\\", null);
            foreach (var item in modelArvhivos.ListaDocumentos)
            {
                string subString = item.FileName;
                if (subString == nombreArchivo)
                {
                    nombrePath = item.FileName;
                    break;
                }
            }

            if (FileServer.VerificarExistenciaFile(pathTemporal, nombrePath, null))
            {
                FileServer.DeleteBlob(pathTemporal + "//" + nombrePath, null);
            }

            return -1;
        }

        /// <summary>
        /// Permite mostrar los archivos del directorio Files para los nuevos registros
        /// </summary>
        /// <param name="sModulo">Modulo</param>
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ListaArchivosNuevo(string sModulo, int nroOrden)
        {
            base.ValidarSesionUsuario();

            ArchivosModel model = new ArchivosModel();

            string path = "";
            string currentUserSession = HttpContext.Session.SessionID;

            if (String.Equals(sModulo, ConstantesExtranetCTAF.SModuloEventos))
            {
                //Crear path temporal    
                string nombreCarpetaTemporal = nroOrden == 1 ? ConstantesExtranetCTAF.SNombreCarpetaTemporal + "_" + currentUserSession : ConstantesExtranetCTAF.SNombreCarpetaTemporal2 + "_" + currentUserSession;
                path = base.PathFiles + "//" + ConstantesExtranetCTAF.EventosFile + nombreCarpetaTemporal + "\\";
            }
            else
            {
                path = base.PathFiles + "//";
            }

            model.ListaDocumentos = FileServer.ListarArhivos(path, null);
            model.ListaDocumentosFiltrado = new List<FileData>();
            foreach (var item in model.ListaDocumentos)
            {
                model.ListaDocumentosFiltrado.Add(item);
            }
            return Json(model);
        }

        /// <summary>
        /// Permite descargar el formato para la solicitud
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarFormato()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaSolicitudEvento + "InformeFInalPerturbacion.doc";
                return File(fullPath, Constantes.AppWord, "InformeFInalPerturbacion.doc");
            }
            return null;
        }

        /// <summary>
        /// ver archivos adjuntados
        /// </summary>
        /// <param name="codSoli"></param>
        /// <returns></returns>
        public PartialViewResult VerArchivosAdjuntados(int codSoli)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                var oSolicitud = _analisisFallasAppServicio.GetByIdAfSolicitudResp(codSoli);

                if (oSolicitud.Soresparchivootros != null)
                {
                    string[] separadas = oSolicitud.Soresparchivootros.Split(',');
                    List<string> cadena = new List<string>();
                    foreach (var item in separadas)
                    {
                        cadena.Add(item);
                    }

                    model.ListaArchivos = cadena;
                }
                else
                    model.ListaArchivos = new List<string>();

                model.ArchivoFinal = oSolicitud.Soresparchivoinf;

                ViewBag.Sorespcodi = codSoli;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Descargar Archivos
        /// </summary>
        /// <param name="nameArchivo"></param>
        /// <param name="idSoli"></param>
        /// <param name="tipoArchivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarInforme(string nameArchivo, int idSoli, int tipoArchivo)
        {
            try
            {
                string nombreCarpeta = tipoArchivo == 1 ? ConstantesExtranetCTAF.SNombreCarpetaInforFinal : ConstantesExtranetCTAF.SNombreCarpetaOtrosArchivos;
                string pathBaseEventos = ConstantesExtranetCTAF.EventosFile + idSoli + "\\" + nombreCarpeta + "\\" + nameArchivo;

                Stream stream = FileServer.DownloadToStream(pathBaseEventos, null);
                int indexOf = nameArchivo.LastIndexOf('.');
                string extension = nameArchivo.Substring(indexOf + 1, nameArchivo.Length - indexOf - 1);

                if (stream != null)
                    return File(stream, extension, nameArchivo);
                else
                {
                    Log.Info("Download - No se encontro el archivo: " + pathBaseEventos);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// calcular dias habiles
        /// </summary>
        /// <param name="fechEvento"></param>
        /// <param name="fechSolicitud"></param>
        /// <returns></returns>
        public int CalcularDiasHabiles(DateTime fechEvento, DateTime fechSolicitud)
        {
            var diasHabiles = _analisisFallasAppServicio.ObtenerDiasHabiles(fechEvento, fechSolicitud);
            return diasHabiles;
        }

        #endregion

        #region Metodos Auxiliares
        /// <summary>
        /// Obtener Empresas Por Usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public List<SiEmpresaDTO> ObtenerEmpresasPorUsuario(string usuario)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();

            bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, usuario);
            var empresas = _analisisFallasAppServicio.ObtenerListaEmpresas();

            if (accesoEmpresa)
            {
                listaEmpresas = empresas;
            }
            else
            {
                listaEmpresas = base.ListaEmpresas
                    .Where(x => empresas.Any(y => x.EMPRCODI == y.Emprcodi))
                    .Select(x => new SiEmpresaDTO() { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            }

            if (!listaEmpresas.Any())
                listaEmpresas.Add(new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No Existe" });

            return listaEmpresas;
        }

        /// <summary>
        /// //Descarga el archivo excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult Exportar()
        {
            string nombreArchivo = Request["fi"];
            string ruta = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaCargaInformeEvento;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, ConstantesAppServicio.ExtensionExcel, nombreArchivo);
        }

        /// <summary>
        /// //Descarga el archivo excel exportado
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual FileResult DescargarReporte(string file1)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesExtranetCTAF.RutaReportes;
            string fullPath = ruta + file1;

            byte[] buffer = null;

            if (System.IO.File.Exists(fullPath))
            {
                buffer = System.IO.File.ReadAllBytes(fullPath);

                System.IO.File.Delete(fullPath);
            }

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, file1);
        }

        /// <summary>
        /// //Mostrar del manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult MostrarManual()
        {
            if (Session[DatosSesion.SesionUsuario] != null)
            {
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Documentos/Manual_de_Usuario_Extranet_CTAF.pdf";
                return File(fullPath, Constantes.AppPdf, "Manual_de_Usuario_Extranet_CTAF.pdf");
            }
            return null;
        }
        #endregion

        /// <summary>
        /// Listar las Tipo de interrupciones
        /// </summary>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarTipoInterrupcion(int afecodi)
        {
            var model = new AnalisisFallasModel()
            {
                ListaTipoInformacion = _analisisFallasAppServicio.ListSiFuentedatosByFdatpadre(ConstantesExtranetCTAF.FdatcodiCTAFExtranet).OrderBy(x => x.Fdatcodi).ToList(),
            };

            AnalisisFallaDTO oAnalisisFallaDTO = _analisisFallasAppServicio.ObtenerAnalisisFalla(afecodi);

            //No mostrar la opción Interrupción por activación del ERACMF cuando ERACMF es NO
            if (oAnalisisFallaDTO.AFEERACMF != ConstantesAppServicio.SI)
                model.ListaTipoInformacion = model.ListaTipoInformacion.Where(x => x.Fdatcodi != (int)ConstantesExtranetCTAF.Fuentedato.InterrupcionActivacionERACMF).ToList();

            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Eliminar Interrupcion Suministro
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="idtipoinformacion"></param>
        /// <param name="afecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarInterrupcionSuministro(int emprcodi, int idtipoinformacion, int afecodi)
        {
            AnalisisFallasModel model = new AnalisisFallasModel();
            try
            {
                int enviocodi = _analisisFallasAppServicio.EliminarInterrupcionSuministro(User.Identity.Name,emprcodi, idtipoinformacion, afecodi);
               
                model.Resultado = "exito";
                model.StrMensaje = "Se eliminaron los datos con éxito.";
                model.NRegistros = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = "error";
                model.StrMensaje = "Se produjo un error: " + ex.Message;
                model.NRegistros = -1;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }
    }
}