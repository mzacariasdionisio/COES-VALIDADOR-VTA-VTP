using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.Intervenciones.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.DirectorioServicio;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.Intervenciones.Controllers
{
    public class RegistroController : BaseController
    {
        readonly IntervencionesAppServicio intServicio = new IntervencionesAppServicio();
        readonly IEODAppServicio servIeod = new IEODAppServicio();
        readonly EventoAppServicio servEvento = new EventoAppServicio();

        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public RegistroController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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
        /// Almacena filtro de empresa
        /// </summary>
        public string SessionIdsEmpresaSelected
        {
            get
            {
                return (Session[ConstantesIntervencionesAppServicio.IdsEmpresasSeleccionado] != null) ?
                    Session[ConstantesIntervencionesAppServicio.IdsEmpresasSeleccionado].ToString() : string.Empty;
            }
            set { Session[ConstantesIntervencionesAppServicio.IdsEmpresasSeleccionado] = value; }
        }

        /// <summary>
        /// Almacena filtro de empresa
        /// </summary>
        public string SessionIdsEmpresaTotal
        {
            get
            {
                return (Session[ConstantesIntervencionesAppServicio.IdsEmpresasTotal] != null) ?
                    Session[ConstantesIntervencionesAppServicio.IdsEmpresasTotal].ToString() : string.Empty;
            }
            set { Session[ConstantesIntervencionesAppServicio.IdsEmpresasTotal] = value; }
        }

        #endregion

        #region INTERVENCIONES - PROGRAMACIONES (PAGINA INICIAL)

        /// GET: Intervenciones/Registro/
        /// <summary>
        /// Carga el modulo de programaciones.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Programaciones(int tipoProgramacion = 0, string emprcodis = "")
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion();
            List<int> lstemprcodi = new List<int>();
            if (emprcodis != string.Empty) lstemprcodi = emprcodis.Split(',').Select(x => Int32.Parse(x)).Distinct().ToList();

            try
            {
                // Obtener empresas por usuario
                string idEmpresasXUser = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName)); //si el usuario no tiene empresas entonces el sistema enviará excepcion

                this.SessionIdsEmpresaTotal = idEmpresasXUser;
                this.SessionIdsEmpresaSelected = idEmpresasXUser; //inicializar variable de sesion if (string.IsNullOrEmpty(this.SessionIdsEmpresaSelected))
                model.sIdsEmpresas = lstemprcodi.Count > 0 ? emprcodis : this.SessionIdsEmpresaSelected;
                model.ListaCboEmpresa = intServicio.ListarComboEmpresasById(idEmpresasXUser);

                model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
                model.IdTipoProgramacion = tipoProgramacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Carga las programaciones por tipo de programación.
        /// </summary>
        /// <param name="idTipoProgramacion">Id de Tipo de Programación</param> 
        /// <returns>ActionResult</returns>
        [HttpPost]
        public PartialViewResult ProgramacionesListado(int idTipoProgramacion)
        {
            Programacion model = new Programacion();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdTipoProgramacion = idTipoProgramacion;
                model.ListaProgramaciones = intServicio.ListarProgramacionesRegistro(idTipoProgramacion);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Establecer valor a filtro de empresa
        /// </summary>
        /// <param name="emprcodis"></param>
        /// <returns></returns>
        public JsonResult SetearEmpresaFilter(string emprcodis)
        {
            Programacion model = new Programacion();

            try
            {
                if (!string.IsNullOrEmpty(emprcodis))
                {
                    this.SessionIdsEmpresaSelected = emprcodis;
                }

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
            }

            return Json(model);
        }

        #region Nueva programación

        /// <summary>
        /// Agregar nueva programación
        /// </summary>
        /// <param name="idprogramacion"></param>
        /// <returns></returns>
        public PartialViewResult NuevaProgramacion(int idprogramacion)
        {
            Intervencion model = new Intervencion
            {
                ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta)
            };
            model.NombPrograDetallado = model.ListaTiposProgramacion.Find(x => x.Evenclasecodi == idprogramacion).Evenclasedesc;
            model.IdTipoProgramacion = idprogramacion;

            //diario
            model.InterfechainiD = DateTime.Today.AddDays(8);

            //programa semanal
            DateTime finicio = EPDate.f_fechainiciosemana(DateTime.Today);
            finicio = finicio.AddDays(7 * 5);
            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(finicio);
            model.AnhoIni = anioSemIni.Item2.ToString();
            model.SemanaIni = anioSemIni.Item1.ToString();

            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaSemanasIni = ListarSemanasByAnio(anioSemIni.Item2);

            //mensual
            model.Mes = DateTime.Today.AddMonths(1).ToString(ConstantesAppServicio.FormatoMes);

            return PartialView(model);
        }

        /// <summary>
        /// Guardar nueva programación
        /// </summary>
        /// <param name="idprogramacion"></param>
        /// <param name="strfechaInicio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NuevaProgramacionGuardar(int idprogramacion, string strfechaInicio)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaInput = DateTime.MinValue;
                if (idprogramacion == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoMensual)
                {
                    fechaInput = DateTime.ParseExact(strfechaInicio, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                }
                else
                {
                    fechaInput = DateTime.ParseExact(strfechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                }
                //Guardar
                int progrcodi = intServicio.GuardarInProgramacion(idprogramacion, fechaInput, base.UserEmail);

                model.Resultado = progrcodi.ToString();
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
        /// Listar la lista de semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        private List<FechaSemanas> ListarSemanasByAnio(int idAnho)
        {
            List<FechaSemanas> entitys = new List<FechaSemanas>();

            int nsemanas = EPDate.TotalSemanasEnAnho(idAnho, FirstDayOfWeek.Saturday);
            DateTime dtfecha = EPDate.f_fechainiciosemana(idAnho, 1);

            for (int i = 1; i <= nsemanas; i++)
            {
                DateTime dtfechaIniSem = dtfecha.AddDays(7 * (i - 1));
                DateTime dtfechaFinSem = dtfecha.AddDays(7 * (i - 1) + 6);
                FechaSemanas reg = new FechaSemanas
                {
                    IdTipoInfo = i,
                    NombreTipoInfo = "Sem" + i + "-" + idAnho,
                    FechaIniSem = dtfechaIniSem.ToString(ConstantesAppServicio.FormatoFecha),
                    FechaFinSem = dtfechaFinSem.ToString(ConstantesAppServicio.FormatoFecha)
                };

                entitys.Add(reg);
            }

            return entitys;
        }

        #endregion

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesIntervencionesAppServicio.ModuloManualUsuario;
            string nombreArchivo = ConstantesIntervencionesAppServicio.ArchivoManualUsuarioExtranet;
            string pathDestino = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + modulo;
            string pathAlternativo = intServicio.GetPathPrincipal();

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

        #endregion

        /// GET: Intervenciones/Registro/IntervencionesRegistro
        /// <summary>
        /// Carga el formulario principal de operaciones con intervenciones al haber seleccionado la programación.
        /// </summary>
        public ActionResult IntervencionesRegistro(int progCodi = 0)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            if (progCodi <= 0) return base.RedirectToHomeDefault();
            if (string.IsNullOrEmpty(this.SessionIdsEmpresaSelected)) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion();

            InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(progCodi);

            model.IdProgramacion = progCodi;
            model.IdTipoProgramacion = regProg.Evenclasecodi;
            model.NombreProgramacion = regProg.Nomprogramacion;
            model.Evenclasedesc = regProg.Evenclasedesc;
            model.Progrfechaini = regProg.Progrfechaini.ToString(ConstantesAppServicio.FormatoFecha);
            model.Progrfechafin = regProg.Progrfechafin.ToString(ConstantesAppServicio.FormatoFecha);
            model.EsCerrado = regProg.EsCerradoExtranet;
            model.EstadoProgramacion = regProg.EstadoExtranet;
            model.EstadoProgramacionDesc = regProg.EstadoExtranetDesc.ToUpper();

            // Obtener ids empresas
            model.ListaCboEmpresa = intServicio.ListarComboEmpresasById(this.SessionIdsEmpresaSelected);
            model.sIdsEmpresas = this.SessionIdsEmpresaSelected;

            model.ListaCboUbicacion = intServicio.ListarComboUbicacionesXEmpresa(model.sIdsEmpresas);
            model.ListaFamilias = intServicio.ListarComboFamilias();
            model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            //sustento
            model.FamcodiSustentoObligatorio = string.Join(",", intServicio.ListarFamcodiSustentoObligatorio());
            model.FamcodiSustentoOpcional = string.Join(",", intServicio.ListarFamcodiSustentoOpcional());

            return View(model);
        }

        #region 1.0 Consultar Registro

        /// <summary>
        /// Carga el formulario principal de operaciones con intervenciones al haber seleccionado la programación.
        /// </summary>        
        [HttpPost]
        public JsonResult MostrarListadoRegistro(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaEmprcodiLectura = this.ObtenerEmpresasPorUsuario(base.UserName);

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                model.Progrcodi = objFiltro.Progrcodi;
                model.IdTipoProgramacion = objFiltro.Evenclasecodi;

                //validar si es está cerrado o abierto
                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(objFiltro.Progrcodi);
                model.EsCerrado = regProg.EsCerradoExtranet;

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                if (listaIntervenciones.Count > 5000) throw new ArgumentException("El listado contiene más de 3000 registros. Utilice filtros para reducir la cantidad de registros de la vista web.");

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = objFiltro.ListaIntercodiSel;

                //Verificación de mensajes enviados
                foreach (var obj in listaIntervenciones)
                {
                    obj.TipoComunicacion = obj.Intermensajeagente;
                    if (obj.Intermensajeagente == NotificacionAplicativo.TipoComunicacionNinguna && obj.Intermensajecoes != NotificacionAplicativo.TipoComunicacionNinguna)
                        obj.TipoComunicacion = NotificacionAplicativo.TipoComunicacionExisteMensaje;
                    obj.ChkMensaje = obj.Intermensaje == ConstantesAppServicio.SI;
                    obj.ChkAprobacion = listaIntercodiChecked.Contains(obj.Intercodi);
                }

                //Verificacion si tiene archivos
                model.TieneArchivos = listaIntervenciones.Find(x => x.Interisfiles.Trim() == "S") != null;

                //alerta creacion de intervenciones del coordinador
                model.ListaIntervCount = listaIntervenciones.Where(x => x.Interfuentestado <= 4 && x.Interdeleted != 1).ToList().Count;

                //generar objeto para la vista
                model.ListaFilaWeb = listaIntervenciones.Select(x => new IntervencionFila()
                {
                    Intercodi = x.Intercodi,
                    Progrcodi = x.Progrcodi,

                    EmprNomb = x.EmprNomb,
                    Operadornomb = x.Operadornomb,
                    Tipoevenabrev = x.Tipoevenabrev,
                    AreaNomb = x.AreaNomb,
                    Famabrev = x.Famabrev,
                    Equiabrev = x.Equiabrev,
                    InterfechainiDesc = x.Interfechaini.ToString(ConstantesAppServicio.FormatoFechaFull),
                    InterfechafinDesc = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFechaFull),
                    Intermwindispo = x.Intermwindispo,
                    InterconexionprovDesc = x.InterconexionprovDesc,
                    IntersistemaaisladoDesc = x.IntersistemaaisladoDesc,
                    InterindispoDesc = x.InterindispoDesc,
                    InterinterrupDesc = x.InterinterrupDesc,
                    Interdescrip = x.Interdescrip,
                    Interisfiles = x.Interisfiles,
                    Intercodsegempr = x.Intercodsegempr,
                    Interflagsustento = x.Interflagsustento ?? 0,

                    TipoComunicacion = x.TipoComunicacion,
                    ChkMensaje = x.ChkMensaje,
                    ChkAprobacion = x.ChkAprobacion,
                    UltimaModificacionUsuarioDesc = x.UltimaModificacionUsuarioDesc,
                    UltimaModificacionFechaDesc = x.UltimaModificacionFechaDesc,
                    UltimaModificacionUsuarioAgrupDesc = x.UltimaModificacionUsuarioAgrupDesc,
                    Estadocodi = x.Estadocodi,
                    Interdeleted = x.Interdeleted,
                    Interfuentestado = x.Interfuentestado,

                    EstaFraccionado = x.EsContinuoFraccionado,
                    EsConsecutivoRangoHora = x.EsConsecutivoRangoHora,
                    EstadoRegistro = x.EstadoRegistro
                }).ToList();
                var listaNotificacion = listaIntervenciones.Where(x => x.Interleido == 0).ToList();
                model.ListaNotificaciones = listaNotificacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return JsonResult;
        }

        #endregion

        #region 1.1 - NUEVO, 1.2 EDICIÓN

        /// <summary>
        /// Obtiene La intervención de la BD
        /// </summary>
        /// <param name="interCodi"></param>
        /// <param name="progrCodi"></param>
        /// <param name="tipoProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerIntervencion(int interCodi, int progrCodi, int tipoProgramacion, bool escruzadas = false, int equicodi = 0)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                if (tipoProgramacion <= 0 || tipoProgramacion > 5) tipoProgramacion = 3; //por defecto el semanal
                model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioMantenimiento);
                model.ListaClaseProgramacion = intServicio.ListarComboClasesProgramacion();
                model.Entidad = new InIntervencionDTO();
                model.AccionEditar = false;

                InProgramacionDTO regProg = null;

                if (interCodi == 0)
                {
                    model.AccionNuevo = true;

                    model.Entidad = intServicio.ObtenerIntervencionWeb(interCodi, equicodi);

                    regProg = intServicio.ObtenerProgramacionesPorId(progrCodi);

                    model.Entidad.Interfechaini = regProg.Progrfechaini;
                    model.Entidad.Interfechafin = regProg.Progrfechafin.AddDays(1);

                    //validar si aún sigue abierto o ya está cerrado
                    model.EsCerrado = regProg.EsCerradoExtranet;
                    model.IdTipoProgramacion = regProg.Evenclasecodi;
                    model.Evenclasedesc = regProg.Evenclasedesc;
                    model.NombreProgramacion = regProg.ProgrnombYPlazo;
                    if (!model.EsCerrado) model.AccionNuevo = true;

                    //combos
                    model.ListaCausas = intServicio.ListarComboSubCausas((int)model.Entidad.Tipoevencodi);

                    model.Entidad.Evenclasecodi = regProg.Evenclasecodi;
                    model.Entidad.Progrcodi = progrCodi;

                    #region Indisponibilidad PR25

                    model.ListaTipoindispPr25 = intServicio.ListarTipoIndispPr25();
                    //model.Entidad.Intertipoindisp = "-1";
                    //model.Entidad.Interasocproc = "N";

                    //Determinar horizonte utilizado para Indispo PR25
                    var listEvenclase = new List<int> { ConstantesAppServicio.EvenclasecodiEjecutado, ConstantesAppServicio.EvenclasecodiProgDiario };
                    model.Esindisponilidadpr25 = listEvenclase.Contains(regProg.Evenclasecodi);
                    if (model.Esindisponilidadpr25)
                    {
                        model.Entidad.Intertipoindisp = "-1";
                        model.Entidad.Interasocproc = "N";
                    }
                    #endregion

                    //inicializar lista archivos
                    model.Entidad.Intercarpetafiles = Math.Abs((int)DateTime.Now.Ticks) * -1; //carpeta negativa para guardar archivos temporales, cuando se guarde cambiará a positivo
                    model.Entidad.ListaArchivo = new List<InArchivoDTO>();
                    model.Entidad.Nomprogramacion = regProg.Nomprogramacion;
                }
                else
                {
                    model.Entidad = intServicio.GetByIdInIntervencionYSustento(interCodi);

                    regProg = intServicio.ObtenerProgramacionesPorId(model.Entidad.Progrcodi);

                    //validar si aún sigue abierto o ya está cerrado
                    model.EsCerrado = regProg.EsCerradoExtranet;
                    model.IdTipoProgramacion = regProg.Evenclasecodi;
                    model.Evenclasedesc = regProg.Evenclasedesc;
                    model.NombreProgramacion = regProg.ProgrnombYPlazo;
                    if (!model.EsCerrado) model.AccionEditar = true;

                    //combos
                    model.ListaCausas = intServicio.ListarComboSubCausas((int)model.Entidad.Tipoevencodi);

                    model.Entidad.Interjustifaprobrechaz = model.Entidad.Interjustifaprobrechaz;

                    #region Indisponibilidad PR25

                    model.ChkPr25 = !string.IsNullOrEmpty(model.Entidad.Intertipoindisp) && model.Entidad.Intertipoindisp != "-1";
                    model.ListaTipoindispPr25 = intServicio.ListarTipoIndispPr25();
                    //Determinar horizonte utilizado para Indispo PR25
                    var listEvenclase = new List<int> { ConstantesAppServicio.EvenclasecodiEjecutado, ConstantesAppServicio.EvenclasecodiProgDiario };
                    var listFamilia = new List<int> {
                    ConstantesAppServicio.FamcodiCentralEolico, ConstantesAppServicio.FamcodiCentralHidro, ConstantesAppServicio.FamcodiCentralSolar,ConstantesAppServicio.FamcodiCentralTermo,
                    ConstantesAppServicio.FamcodiGeneradorEolico,ConstantesAppServicio.FamcodiGeneradorHidro,ConstantesAppServicio.FamcodiGeneradorSolar,ConstantesAppServicio.FamcodiGeneradorTermo
                    };
                    model.Esindisponilidadpr25 = listEvenclase.Contains(regProg.Evenclasecodi) && listFamilia.Contains(model.Entidad.Famcodi);

                    #endregion

                    model.Entidad.Nomprogramacion = regProg.Nomprogramacion;
                }

                model.Entidad.FinFecha = model.Entidad.Interfechafin.ToString(ConstantesAppServicio.FormatoFecha);
                model.Entidad.FinHora = model.Entidad.Interfechafin.ToString("HH");
                model.Entidad.FinMinuto = model.Entidad.Interfechafin.ToString("mm");
                model.Entidad.IniFecha = model.Entidad.Interfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                model.Entidad.IniHora = model.Entidad.Interfechaini.ToString("HH");
                model.Entidad.IniMinuto = model.Entidad.Interfechaini.ToString("mm");

                //fechas
                model.ProgrfechainiDesc = regProg.ProgrfechainiDesc;
                model.ProgrfechafinDesc = regProg.ProgrfechafinDesc;
                model.Progrfechaini = regProg.Progrfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechainiH = regProg.Progrfechaini.ToString("HH");
                model.ProgrfechainiM = regProg.Progrfechaini.ToString("mm");
                model.Progrfechafin = regProg.Progrfechafin.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechafinH = regProg.Progrfechafin.ToString("HH");
                model.ProgrfechafinM = regProg.Progrfechafin.ToString("mm");

                // Desabilita inputs de empresa, equipo, area, tipo intervencion y codigo seguimiento
                if (!String.IsNullOrEmpty(model.Entidad.Intercodsegempr))
                    model.EsDasabilitadoCodSeguimiento = true;

                if (model.Entidad.Estadocodi == ConstantesIntervencionesAppServicio.InEstadoAprobado)
                    model.EsDesabilitado = true; //Desabilita los inputs

                // Si el horizonte esta aprobado entonces deshabilita los campos del formulario
                if (model.Entidad.Estadocodi == ConstantesIntervencionesAppServicio.InEstadoRechazado || model.EsCerrado)
                    model.EsDesabilitado = true; // Desabilita los inputs

                if (model.Entidad.Interdeleted == ConstantesIntervencionesAppServicio.iSi)
                {
                    model.AccionEditar = false;
                    model.EsCerrado = true;
                    model.EsDesabilitado = true; // Desabilita los inputs
                }

                model.IdIntervencion = interCodi;
                model.EsCruzadas = escruzadas;
                model.IdProgramacion = progrCodi;

                if (model.EsCruzadas)
                {
                    model.AccionNuevo = false;
                    model.AccionEditar = false;
                    model.EsDesabilitado = true; // Desabilita los inputs
                }

                model.Entidad.EsEditable = model.AccionNuevo || model.AccionEditar;

                //fecha hoy (validacion ejecutados)
                model.FechaHoy = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

                model.Entidad.Interipagente = GetIpAgente();

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
        /// Guarda el registro de la intervención.
        /// </summary>
        /// <param name="dataJson"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IntervencionGuardar(string dataJson, int opcion, int confirmarvalinter, int confirmarvalinterejec)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                //Consultar permisos de Operador designado

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model.Entidad = new InIntervencionDTO();
                string usuarioModificacion = base.UserEmail;
                DateTime fechaModificacion = DateTime.Now;

                int idIntervencion = 0;
                int idCarpetaFisica = 0;
                if (opcion == ConstantesIntervencionesAppServicio.AccionNuevo)
                {
                    model.Entidad = serializer.Deserialize<InIntervencionDTO>(dataJson);
                    idCarpetaFisica = model.Entidad.Intercarpetafiles;

                    //las plantillas de sustento tienen que estar completas
                    bool hayArchivo = TieneArchivo(model.Entidad) || (model.Entidad.Sustento != null && model.Entidad.Sustento.TienePlantillaCompleta);

                    //validar si aún sigue abierto o ya está cerrado
                    var programaciones = intServicio.ObtenerProgramacionesPorId(model.Entidad.Progrcodi);
                    bool esCerrado = programaciones.EsCerradoExtranet;
                    if (esCerrado)
                    {
                        throw new Exception("No se puede registrar la intervención porque el programa ya se encuentra cerrado");
                    }

                    model.Entidad.Intermensaje = ConstantesIntervencionesAppServicio.sNo;
                    model.Entidad.Intermensajecoes = NotificacionAplicativo.TipoComunicacionNinguna;
                    model.Entidad.Intermensajeagente = NotificacionAplicativo.TipoComunicacionNinguna;
                    model.Entidad.Interfechapreini = null;
                    model.Entidad.Interfechaprefin = null;
                    model.Entidad.Interrepetir = ConstantesIntervencionesAppServicio.sNo;

                    model.Entidad.Interjustifaprobrechaz = null;
                    model.Entidad.Interfecaprobrechaz = null;
                    model.Entidad.Interisfiles = hayArchivo ? ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                    model.Entidad.CarpetaInTemporal = idCarpetaFisica;
                    model.Entidad.Intermanttocodi = null;
                    model.Entidad.Interevenpadre = null;
                    model.Entidad.Actividad = "Se ha registrado una intervencion";

                    // NO TIENE CODIGO DE REGISTRO PADRE POR SER OPERACIÓN NUEVO
                    model.Entidad.Intercodipadre = null;

                    model.Entidad.Interregprevactivo = ConstantesIntervencionesAppServicio.sSi;
                    model.Entidad.Estadocodi = ConstantesIntervencionesAppServicio.InEstadoPendienteEnProcesoEvaluacion;
                    model.Entidad.Interdeleted = ConstantesIntervencionesAppServicio.iNo;
                    model.Entidad.Interprocesado = ConstantesIntervencionesAppServicio.iNo;
                    model.Entidad.Intercreated = ConstantesIntervencionesAppServicio.iSi;
                    if (model.Entidad.Sustento != null && model.Entidad.Sustento.TienePlantillaCompleta) model.Entidad.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;

                    model.Entidad.Interusucreacion = usuarioModificacion;
                    model.Entidad.Interfeccreacion = fechaModificacion;
                    model.Entidad.Interusumodificacion = null;
                    model.Entidad.Interfecmodificacion = null;

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>Validar Intervencion servicio>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var entidadConFechas = intServicio.ValidarIntervencionWeb(model.Entidad);

                    //Validar con Intervenciones
                    if (model.IdTipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado)
                    {
                        //Validar con Intervenciones Programadas
                        if (confirmarvalinterejec != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                        {
                            if (ValidarHorariosEjecutadosProgramados(model.Entidad.Equicodi, model.Entidad.Intercodsegempr, model.Entidad.IniFecha, model.Entidad.IniHora, model.Entidad.IniMinuto, model.Entidad.FinFecha, model.Entidad.FinHora, model.Entidad.FinMinuto) == "1")
                            {
                                model.TieneValidacionEjecIgualProg = 1;
                            }
                        }

                        //Validar con Horas de Operación
                        if (confirmarvalinter != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                        {
                            List<ResultadoValidacionAplicativo> listaValHoFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValScadaFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValEmsFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValIDCCFinal = new List<ResultadoValidacionAplicativo>();

                            intServicio.ListarDetallesAlertasInter(model.Entidad, ref listaValHoFinal, ref listaValScadaFinal, ref listaValEmsFinal, ref listaValIDCCFinal);

                            if (listaValHoFinal.Count() > 0 || listaValScadaFinal.Count() > 0 || listaValEmsFinal.Count() > 0 || listaValIDCCFinal.Count() > 0)
                            {
                                model.TieneValidaciones = 1;
                                //ho
                                model.ListaValidacionHorasOperacion = listaValHoFinal;
                                if (listaValIDCCFinal.Count > 0)
                                {
                                    //idcc
                                    model.ListaValidacionIDCC = listaValIDCCFinal;
                                }
                                else
                                {
                                    if (listaValEmsFinal.Count > 0)
                                    {
                                        //ems
                                        model.ListaValidacionEms = listaValEmsFinal;
                                    }
                                    else
                                    {
                                        //scada
                                        model.ListaValidacionScada = listaValScadaFinal;
                                    }
                                }
                            }
                        }

                        if (model.TieneValidacionEjecIgualProg > 0 || model.TieneValidaciones > 0)
                        {
                            model.Resultado = "2";
                            return Json(model);
                        }
                    }

                    model.Entidad.Interipagente = GetIpAgente();

                    // Graba los cambio en la BD
                    intServicio.CrudListaIntervencion(new List<InIntervencionDTO>() { model.Entidad }, null, null, usuarioModificacion);
                    idIntervencion = model.Entidad.Intercodi;
                    model.IdIntervencion = idIntervencion;
                }
                else
                {
                    var intervencionEdit = serializer.Deserialize<InIntervencionDTO>(dataJson);
                    model.Entidad = intServicio.GetByIdInIntervencion(intervencionEdit.Intercodi);
                    model.Entidad.ListaArchivo = intervencionEdit.ListaArchivo ?? new List<InArchivoDTO>();

                    //las plantillas de sustento tienen que estar completas
                    bool hayArchivo = TieneArchivo(intervencionEdit) || (intervencionEdit.Sustento != null && intervencionEdit.Sustento.TienePlantillaCompleta);
                    bool hayArchivoEliminable = TieneArchivo(intervencionEdit) || (intervencionEdit.SustentoOld != null && intervencionEdit.SustentoOld.TienePlantillaCompleta);

                    //si se editó cambiando el equipo de la intervención o tienen sustento de exclusión
                    bool flagEsEliminable = intervencionEdit.Equicodi != model.Entidad.Equicodi || (intervencionEdit.SustentoOld != null && intervencionEdit.SustentoOld.TienePlantillaCompleta);

                    //>>>>>>>>>>>>>>>>>>>>>>>>>>Validar Intervencion servicio>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var entidadConFechas = intServicio.ValidarIntervencionWeb(intervencionEdit);

                    model.Entidad.Estadocodi = ConstantesIntervencionesAppServicio.InEstadoPendienteEnProcesoEvaluacion;

                    //capturar valores
                    model.Entidad.Interfechaini = entidadConFechas.Interfechaini;
                    model.Entidad.Interfechafin = entidadConFechas.Interfechafin;
                    model.Entidad.Tipoevencodi = intervencionEdit.Tipoevencodi;
                    model.Entidad.Subcausacodi = intervencionEdit.Subcausacodi;
                    model.Entidad.Claprocodi = intervencionEdit.Claprocodi;
                    model.Entidad.Equicodi = intervencionEdit.Equicodi;
                    model.Entidad.Areacodi = intervencionEdit.Areacodi;
                    model.Entidad.Emprcodi = intervencionEdit.Emprcodi;
                    model.Entidad.Intermwindispo = intervencionEdit.Intermwindispo;
                    model.Entidad.Intercodsegempr = intervencionEdit.Intercodsegempr;
                    model.Entidad.Interdescrip = intervencionEdit.Interdescrip;
                    model.Entidad.Interjustifaprobrechaz = intervencionEdit.Interjustifaprobrechaz;
                    model.Entidad.Interipagente = intervencionEdit.Interipagente;
                    //radio
                    model.Entidad.Interindispo = intervencionEdit.Interindispo;
                    //checks
                    model.Entidad.Interinterrup = intervencionEdit.Interinterrup;
                    model.Entidad.Intermantrelev = intervencionEdit.Intermantrelev;
                    model.Entidad.Interconexionprov = intervencionEdit.Interconexionprov;
                    model.Entidad.Intersistemaaislado = intervencionEdit.Intersistemaaislado;
                    //PR25 Indisponibilidades
                    model.Entidad.Intertipoindisp = intervencionEdit.Intertipoindisp;
                    model.Entidad.Interpr = intervencionEdit.Interpr;
                    model.Entidad.Interasocproc = intervencionEdit.Interasocproc;

                    //SI TIENE CODIGO DE REGISTRO PADRE POR SER OPERACIÓN EDITAR
                    model.Entidad.Intercodipadre = model.Entidad.Intercodi; // Para mantener el historial
                    model.Entidad.Interisfiles = hayArchivo ? ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                    if (intervencionEdit.Sustento != null && intervencionEdit.Sustento.TienePlantillaCompleta) model.Entidad.Sustento = intervencionEdit.Sustento;
                    if (model.Entidad.Sustento != null) model.Entidad.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;

                    model.Entidad.Interusumodificacion = usuarioModificacion;
                    model.Entidad.Interfecmodificacion = fechaModificacion;
                    model.Entidad.Actividad = "Se ha actualizado intervencion";

                    model.Detalle = "";

                    //Validar con Intervenciones
                    if (model.IdTipoProgramacion == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado)
                    {
                        //Validar con Intervenciones Programadas
                        if (confirmarvalinterejec != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                        {
                            if (ValidarHorariosEjecutadosProgramados(model.Entidad.Equicodi, model.Entidad.Intercodsegempr, model.Entidad.IniFecha, model.Entidad.IniHora, model.Entidad.IniMinuto, model.Entidad.FinFecha, model.Entidad.FinHora, model.Entidad.FinMinuto) == "1")
                            {
                                model.TieneValidacionEjecIgualProg = 1;
                            }
                        }

                        //Validar con Horas de Operación
                        if (confirmarvalinter != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                        {
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                            List<ResultadoValidacionAplicativo> listaValHoFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValScadaFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValEmsFinal = new List<ResultadoValidacionAplicativo>();
                            List<ResultadoValidacionAplicativo> listaValIDCCFinal = new List<ResultadoValidacionAplicativo>();

                            intServicio.ListarDetallesAlertasInter(model.Entidad, ref listaValHoFinal, ref listaValScadaFinal, ref listaValEmsFinal, ref listaValIDCCFinal);

                            if (listaValHoFinal.Count() > 0 || listaValScadaFinal.Count() > 0 || listaValEmsFinal.Count() > 0 || listaValIDCCFinal.Count() > 0)
                            {
                                model.TieneValidaciones = 1;
                                //ho
                                model.ListaValidacionHorasOperacion = listaValHoFinal;
                                //scada
                                model.ListaValidacionScada = listaValScadaFinal;
                                //ems
                                model.ListaValidacionEms = listaValEmsFinal;
                                //idcc
                                model.ListaValidacionIDCC = listaValIDCCFinal;
                            }
                            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                        }

                        if (model.TieneValidacionEjecIgualProg > 0 || model.TieneValidaciones > 0)
                        {
                            model.Resultado = "2";
                            return Json(model);
                        }
                    }

                    model.Entidad.Interipagente = GetIpAgente();

                    // Graba los cambio en la BD
                    if (flagEsEliminable)
                    {
                        InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(model.Entidad.Progrcodi);

                        //el registro que está en el formulario web se guardará como nuevo
                        model.Entidad.Intercodi = 0;
                        model.Entidad.Intercodipadre = null;
                        //model.Entidad.Interjustifaprobrechaz = null;
                        model.Entidad.EsCopiarArchivo = true; //copiar los archivos de sustento al destino
                        model.Entidad.CarpetaProgOrigenFS = regProg.CarpetaProgDefault;
                        model.Entidad.CarpetaProgDestinoFS = regProg.CarpetaProgDefault;
                        model.Entidad.CarpetafilesOrigenFS = model.Entidad.Intercarpetafiles;

                        //el registro histórico se guadará como eliminado
                        var regEliminar = intServicio.GetByIdInIntervencion(intervencionEdit.Intercodi);
                        if (intervencionEdit.SustentoOld != null && intervencionEdit.SustentoOld.TienePlantillaCompleta) regEliminar.Sustento = intervencionEdit.SustentoOld;
                        if (regEliminar.Sustento != null)
                        {
                            regEliminar.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                            regEliminar.Interisfiles = hayArchivoEliminable ? ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                            model.Entidad.CarpetaInTemporal = regEliminar.Intercarpetafiles;
                        }

                        intServicio.CrudListaIntervencion(new List<InIntervencionDTO>() { model.Entidad }, null, new List<InIntervencionDTO>() { regEliminar }, usuarioModificacion);
                    }
                    else
                    {
                        intServicio.CrudListaIntervencion(null, new List<InIntervencionDTO>() { model.Entidad }, null, usuarioModificacion);
                    }
                    idIntervencion = model.Entidad.Intercodi;
                    model.IdIntervencion = idIntervencion;

                    // Obtiene el registro de la intervencion para llenar los siguientes campos
                    model.Entidad = intServicio.GetByIdInIntervencion(idIntervencion);
                }

                // [Ejecutado] Enviar notificación Intervenciones
                if (model.Entidad.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado &&
                    confirmarvalinter == ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                {
                    InProgramacionDTO programa = intServicio.ObtenerProgramacionesPorIdSinPlazo(model.Entidad.Progrcodi);

                    List<ResultadoValidacionAplicativo> listaValInterv = this.intServicio.ListarAlertaHoraOperacionEquipoManttoRegistroInter(model.Entidad);
                    if (listaValInterv.Count() > 0)
                    {
                        this.intServicio.EnviarCorreoNotificacionHorasOperacion(model.Entidad, listaValInterv, fechaModificacion, programa.Progrfechaini, model.Entidad.Emprcodi);
                    }
                }

                model.StrMensaje = opcion == 3 ? "¡La Información se grabó correctamente!" : "¡La Información se actualizó correctamente!";
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

        private bool TieneArchivo(InIntervencionDTO entidad)
        {
            bool tieneArchivoANivelIntervencion = entidad.ListaArchivo != null && entidad.ListaArchivo.Any();

            return tieneArchivoANivelIntervencion;
        }

        [HttpPost]
        public JsonResult ValidarCodigoSeguimiento(string codigoSeguimiento, int equicodi)
        {
            base.ValidarSesionJsonResult();

            bool esValido = intServicio.ValidarCodigoSeguimiento(codigoSeguimiento, equicodi);
            return Json(esValido);
        }

        [HttpPost]
        public JsonResult ValidarExisteIntervencionDuplicada(string descripcion, int equiCodi
                                            , string fechaIni, string horaIni, string minutosIni
                                            , string fechaFin, string horaFin, string minutosFin
                                            , int interCodi, int programacion, int tipoevencodi)
        {
            var nuevaInterFechaIni = DateTime.ParseExact(fechaIni.Trim() + " " + horaIni + ":" + minutosIni + ":00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            var nuevaInterFechaFin = DateTime.ParseExact(fechaFin.Trim() + " " + horaFin + ":" + minutosFin + ":00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            bool existeDuplicado = intServicio.ValidarExisteIntervencionDuplicada(descripcion, equiCodi, nuevaInterFechaIni, nuevaInterFechaFin,
                                            interCodi, programacion, tipoevencodi, out _);

            return Json(existeDuplicado);
        }

        [HttpPost]
        public JsonResult ValidarExisteCambioDescripcionOEquipo(int intercodi, int equicodi, string descripcionOriginal, string descripcionNueva)
        {
            bool tieneSimilitud = intServicio.ExisteCambioDescripcionOEquipo(intercodi, equicodi, descripcionOriginal, descripcionNueva);

            return Json(tieneSimilitud);
        }

        /// <summary>
        /// Validar si ejecutado es idéntico a su programado
        /// </summary>
        /// <param name="equicodi"></param>
        /// <param name="codSeguimiento"></param>
        /// <param name="fechaIni"></param>
        /// <param name="horaIni"></param>
        /// <param name="minutosIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horaFin"></param>
        /// <param name="minutosFin"></param>
        /// <returns></returns>
        public string ValidarHorariosEjecutadosProgramados(int equicodi, string codSeguimiento, string fechaIni, string horaIni, string minutosIni, string fechaFin, string horaFin, string minutosFin)
        {
            Intervencion model = new Intervencion();
            string validacion;

            DateTime nuevaInterFechaIni = DateTime.ParseExact(fechaIni.Trim() + " " + horaIni + ":" + minutosIni + ":00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime nuevaInterFechaFin = DateTime.ParseExact(fechaFin.Trim() + " " + horaFin + ":" + minutosFin + ":00", "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            model.NroRegistros = this.intServicio.ValidarHorariosEjecutadosProgramados(equicodi, codSeguimiento, nuevaInterFechaIni, nuevaInterFechaFin);

            validacion = model.NroRegistros == 0 ? "0" : "1";

            return validacion;
        }

        /// <summary>
        /// Permite  obtener las fechas de programacion de acuerdo al tipo de programacion y programacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerFechaProgramacion(int progCodi)
        {
            Intervencion model = new Intervencion
            {
                IdProgramacion = progCodi
            };

            if (progCodi > 0)
            {
                var regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progCodi);

                model.ProgrfechainiDesc = regProg.ProgrfechainiDesc;
                model.ProgrfechafinDesc = regProg.ProgrfechafinDesc;
                model.Progrfechaini = regProg.Progrfechaini.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechainiH = regProg.Progrfechaini.ToString("HH");
                model.ProgrfechainiM = regProg.Progrfechaini.ToString("mm");
                model.Progrfechafin = regProg.Progrfechafin.ToString(ConstantesAppServicio.FormatoFecha);
                model.ProgrfechafinH = regProg.Progrfechafin.ToString("HH");
                model.ProgrfechafinM = regProg.Progrfechafin.ToString("mm");
            }

            return Json(model);
        }

        #region Búsqueda de Equipos

        /// <summary>
        /// View Busqueda Linea de transmision
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipo(int? filtroFamilia = 0)
        {
            Intervencion model = new Intervencion();

            string idEmpresas = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
            model.ListaCboEmpresa = intServicio.ListarComboEmpresasById(idEmpresas, true);
            model.ListaFamilia = this.servIeod.ListarFamilia();
            model.FiltroFamilia = filtroFamilia.Value;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra el resultado de la busqueda
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoResultado(int idEmpresa, int idFamilia, string filtro, int nroPagina, int? idArea = 0, int? filtroFamilia = 0)
        {
            Intervencion model = new Intervencion();
            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            int tipoEmpr = 0;

            string empresasExtranet;
            if (idEmpresa == 0)
                empresasExtranet = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
            else
                empresasExtranet = idEmpresa.ToString();

            if (idEmpresa > 0)
            {
                SiEmpresaDTO empresa = new SiEmpresaDTO();
                empresa = intServicio.GetByIdSiEmpresa(idEmpresa);
                tipoEmpr = empresa.Tipoemprcodi;
            }

            //var listaLinea = this.servEvento.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);
            var listaLinea = this.servEvento.BuscarEquipoEventoExtranet(empresasExtranet, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow, tipoEmpr);

            foreach (var reg in listaLinea)
            {
                EqEquipoDTO eq = new EqEquipoDTO
                {
                    Emprnomb = reg.EMPRENOMB,
                    Areanomb = reg.AREANOMB,
                    Equicodi = reg.EQUICODI,
                    Equinomb = reg.EQUIABREV,
                    Equiabrev = reg.EQUIABREV,
                    Famabrev = reg.FAMABREV,
                    Emprcodi = reg.EMPRCODI,
                    Areacodi = reg.AREACODI,
                    Famcodi = reg.FAMCODI,
                    Grupotipocogen = reg.Grupotipocogen
                };

                listaEquipo.Add(eq);
            }

            model.ListaEquipo = listaEquipo;
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado 
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFamilia"></param>
        /// <param name="idArea"></param>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoPaginado(int idEmpresa, int idFamilia, string filtro, int? idArea = 0, int? filtroFamilia = 0)
        {
            try
            {
                Intervencion model = new Intervencion();
                int tipoEmpr = 0;
                model.IndicadorPagina = false;
                //int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);
                string empresasExtranet = "0";
                if (idEmpresa == 0)
                    empresasExtranet = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                else
                    empresasExtranet = idEmpresa.ToString();

                if (idEmpresa > 0)
                {
                    SiEmpresaDTO empresa = new SiEmpresaDTO();
                    empresa = intServicio.GetByIdSiEmpresa(idEmpresa);
                    tipoEmpr = empresa.Tipoemprcodi;
                }

                int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipoExtranet(empresasExtranet, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, tipoEmpr);


                if (nroRegistros > Constantes.NroPageShow)
                {
                    int pageSize = Constantes.NroPageShow;
                    int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                    model.NroPaginas = nroPaginas;
                    model.NroMostrar = Constantes.NroPageShow;
                    model.IndicadorPagina = true;
                }

                return PartialView(model);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Muestra las areas de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BusquedaEquipoArea(int idEmpresa, int idFamilia, int? filtroFamilia = 0)
        {
            Intervencion model = new Intervencion
            {
                ListaArea = this.servEvento.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList()
            };
            return PartialView(model);
        }

        /// <summary>
        /// Funcion para obtener las familias permitidas. 
        /// </summary>
        /// <param name="idFamilia"></param>
        /// <param name="filtroFamilia"> -1: filtrar todas las familias, 0: filtrar solo para lineas de tranmision </param>
        /// <returns></returns>
        private string ListarFamiliaByFiltro(int idFamilia, int? filtroFamilia = 0)
        {
            if (filtroFamilia == 0)
            {
                return "0";
            }

            return idFamilia.ToString();
        }

        #endregion

        #endregion

        #region 1.3 - ELIMINAR

        /// GET: Intervenciones/Registro/
        [HttpPost]
        public JsonResult EliminarSeleccionados(string intercodis, string dataJson)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                //intervenciones con sustento. solo para semanal y diario
                List<InIntervencionDTO> listaWebSustentoExcl = new List<InIntervencionDTO>();
                if (dataJson != null)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    listaWebSustentoExcl = serializer.Deserialize<List<InIntervencionDTO>>(dataJson);
                }

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(intercodis)) listaIntercodiChecked = intercodis.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //primero se valida si las intervenciones pueden ser eliminadas
                intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteExtranet, listaIntercodiChecked,
                                                out List<InIntervencionDTO> listaEliminable, out List<InIntervencionDTO> listaWarning);

                foreach (var entity in listaEliminable)
                {
                    //setear IP
                    entity.Interipagente = GetIpAgente();

                    //setear sustento
                    var regWeb = listaWebSustentoExcl.Find(x => x.Intercodi == entity.Intercodi);
                    if (regWeb != null)
                    {
                        if (regWeb.Sustento != null && regWeb.Sustento.TienePlantillaCompleta) entity.Sustento = regWeb.Sustento;
                        if (entity.Sustento != null)
                        {
                            entity.Interisfiles = ConstantesAppServicio.SI;
                            entity.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                        }
                    }
                }

                //solo eliminar las no aprobadas
                intServicio.CrudListaIntervencion(null, null, listaEliminable, base.UserEmail);

                model.Mensaje = "Se eliminó satisfactoriamente los registros";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        #endregion

        #region 1.5 - Comunicaciones, Historial de modificaciones y Trazabilidad
        /// <summary>
        /// Carga el listado de la mensajeria
        /// </summary>
        /// <param name="interCodi">Id de intervención</param>
        /// <param name="tipotipoProgramacion">Tipo de progrmación</param>
        /// <param name="estado">Estado</param>
        /// <param name="programacion">Id de Programación</param>  
        /// <param name="aprobar">Variable cadena aprobar</param>
        /// <returns>Vista parcial del modelo View(model)</returns>
        [HttpPost]
        public JsonResult ListadoMensaje(int interCodi, string tipoRemitente, string estadoMensaje)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaEmprcodiLectura = this.ObtenerEmpresasPorUsuario(base.UserName);

                model.ListaMensajes = intServicio.ListSiMensajesXIntervencion(interCodi, ConstantesIntervencionesAppServicio.AmbienteExtranet, listaEmprcodiLectura, tipoRemitente, estadoMensaje);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult MarcarMensajeLeido(int interCodi, int msgcodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                InIntervencionDTO entidad = intServicio.GetByIdInIntervencion(interCodi);

                int emprcodiLectura = entidad.Emprcodi;
                intServicio.MarcarMensajeComoLeido(interCodi, msgcodi, ConstantesIntervencionesAppServicio.AmbienteExtranet, emprcodiLectura, base.UserEmail, base.UserEmail, entidad.Emprcodi);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult DescargarZipXMensaje(int interCodi)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaEmprcodiLectura = this.ObtenerEmpresasPorUsuario(base.UserName);

                var entidad = intServicio.GetByIdInIntervencion(interCodi);
                InProgramacionDTO objProgramacion = intServicio.ObtenerProgramacionesPorIdSinPlazo(entidad.Progrcodi);

                List<SiMensajeDTO> listaMensaje = intServicio.ListSiMensajesXIntervencion(interCodi, ConstantesIntervencionesAppServicio.AmbienteExtranet, listaEmprcodiLectura, "-1", "-1")
                    .Where(x => x.Msgflagadj == 1).ToList();
                if (!listaMensaje.Any())
                    throw new ArgumentException("No existe mensaje con archivos adjuntos");

                int numeroAleatorio = (int)DateTime.Now.Ticks;
                intServicio.DescargarArchivosAdjuntosComunicacion(listaMensaje, numeroAleatorio.ToString(), objProgramacion.CarpetaProgDefault, out string nameFile);
                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Listado de mensajes exportado en pdf
        /// <returns></returns>
        public ActionResult DownloadFilePdfListadoMensaje(int interCodi)
        {
            List<int> listaEmprcodiLectura = this.ObtenerEmpresasPorUsuario(base.UserName);
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string html = intServicio.GenerarPDFMensajes(ConstantesIntervencionesAppServicio.AmbienteExtranet, interCodi, listaEmprcodiLectura, out string filename);

            UtilDevExpressIntervenciones.GenerarPDFdeHtml(html, path, filename);

            return base.DescargarArchivoTemporalYEliminarlo(path, filename);
        }

        [HttpPost]
        public PartialViewResult ListadoModificaciones(int interCodi)
        {
            Intervencion model = new Intervencion
            {
                IdIntervencion = interCodi,
                ListaIntervenciones = intServicio.ListarModificacionesXIntervencion(interCodi)
            };

            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult ListadoTrazabilidad(int interCodi)
        {
            Intervencion model = new Intervencion();

            var entidad = intServicio.GetByIdInIntervencion(interCodi);

            model.IdIntervencion = interCodi;
            model.IdTipoProgramacion = entidad.Evenclasecodi;
            model.ListaIntervenciones = intServicio.ObtenerTrazabilidad(interCodi, entidad.Evenclasecodi, entidad.Intercodsegempr);
            model.ListaMensajes = new List<SiMensajeDTO>();

            foreach (var regInter in model.ListaIntervenciones)
            {
                List<SiMensajeDTO> listaMensajes = intServicio.ListSiMensajesXIntervencion(regInter.Intercodi, ConstantesIntervencionesAppServicio.AmbienteExtranet, new List<int>(), "-1", "-1");
                foreach (var mensaje in listaMensajes)
                {
                    mensaje.Intercodi = regInter.Intercodi;
                    model.ListaMensajes.Add(mensaje);
                }
            }

            return PartialView(model);
        }

        #endregion

        #region 1.6 - Sustento Inclusión / Exclusión

        /// <summary>
        /// Determinar que la intervención puede tener o no plantilla de justificación de inclusión / exclusión
        /// </summary>
        /// <param name="modelInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarIntervencionPuedeTenerJustificacion(string dataJson)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var entidad = serializer.Deserialize<InIntervencionDTO>(dataJson);
                var entidadConFechas = intServicio.ValidarIntervencionWeb(entidad);

                intServicio.ListarPlantillaSustentoInclusionExclusion(entidadConFechas, out InIntervencionDTO inIncl, out InIntervencionDTO inExcl);
                model.IntervencionIncl = inIncl;
                model.IntervencionExcl = inExcl;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult VerificarListaIntervencionPuedeTenerJustificacion(string intercodis, int progrcodi, int evenclasecodi)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(intercodis)) listaIntercodiChecked = intercodis.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //setear plantilla
                model.ListaExclusion = intServicio.ListarIntervencionEliminacionConPlantillaExclusion(evenclasecodi, progrcodi, listaIntercodiChecked);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public JsonResult ReplicarArchivoJustificacion(int progrcodi, int carpetaOrigen, string carpetaDestino)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                List<int> listaCarpetaDestino = new List<int>();
                if (!string.IsNullOrEmpty(carpetaDestino)) listaCarpetaDestino = carpetaDestino.Split(',').Select(x => Int32.Parse(x)).Distinct().ToList();
                listaCarpetaDestino = listaCarpetaDestino.Where(x => x != carpetaOrigen).ToList();

                //intervenciones que tienen check en la pantalla
                intServicio.ReplicarSustento(progrcodi, carpetaOrigen, listaCarpetaDestino);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpGet]
        public virtual ActionResult DescargarPDFSustento(int intercodi)
        {
            string pathLocal = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            intServicio.GenerarReportePDFSustento(intercodi, pathLocal, out string nombreArchivo);

            return base.DescargarArchivoTemporalYEliminarlo(pathLocal, nombreArchivo);
        }

        [HttpPost]
        public JsonResult DescargarExcelSustento(string dataJson)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                InIntervencionDTO entidad = serializer.Deserialize<InIntervencionDTO>(dataJson);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string fileName = IntervencionesAppServicio.GetPrefijoArchivoTemporal() + (entidad.Sustento.FlagTieneInclusion ? "Formato_Sustento_Inclusión.xlsx" : "Formato_Sustento_Exclusión.xlsx");

                //copiar imagenes
                intServicio.CopiarImagenADirectorioLocal(path, entidad, entidad.Sustento, out List<InArchivoDTO> listaPathImagen);

                //generar excel
                UtilExcelIntervenciones.GenerarExcelReporteSustento(entidad, entidad.Sustento, path, fileName, listaPathImagen);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        [HttpPost]
        public ActionResult UploadExcelSustento()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    var sNombreArchivo = DateTime.Now.Ticks + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        FileServer.DeleteBlob(sNombreArchivo, path + ConstantesIntervencionesAppServicio.Reportes);
                    }
                    file.SaveAs(path + sNombreArchivo);

                    return Json(new { success = true, nuevonombre = sNombreArchivo }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarDatosExcelSustento(int progrcodi, int intercarpetafiles, string dataJson, string nombreArchivoUpload)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                InIntervencionDTO entidad = serializer.Deserialize<InIntervencionDTO>(dataJson);

                intServicio.ObtenerSustentoYArchivoAImportarXLSX(progrcodi, intercarpetafiles, AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes, nombreArchivoUpload, ref entidad, out List<string> listaMsj);

                model.Resultado = "1";
                model.IntervencionImportada = entidad;
                model.ListaMensaje = listaMsj;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        #endregion

        #region 1.7 Cambiar Estado

        public JsonResult ActualizarLeidoAgente(string intercodis)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();
                intServicio.UpdateEstadoMensajeAgente(intercodis);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;
            return JsonResult;
        }

        #endregion

        #region 2.0 Mensajería

        /// GET: Intervenciones/Registro/IntervencionesMensaje
        /// <summary>
        /// Carga del modulo de mensajeria
        /// </summary>
        /// <param name="msgCodi">Id de mensaje</param>
        /// <param name="tipo">Tipo de mensaje</param>
        /// <param name="progrCodi">Id de Programación</param>
        /// <param name="tipoProgramacion">Id de Tipo de Programación</param>  
        /// <param name="estado">Estado del mensaje</param>
        /// <returns>Vista del modelo View(model);</returns>
        public ActionResult IntervencionesMensajeRegistro(string intercodis, int progrcodi, int evenclasecodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            //if (base.IdOpcion == null) return base.RedirectToHomeDefault();
            if (progrcodi == 0 || evenclasecodi == 0) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion
            {
                Intercodis = (intercodis ?? "").Replace(";", ","),
                IdProgramacion = progrcodi,
                IdTipoProgramacion = evenclasecodi,
                CorreoFrom = base.UserEmail,
                CorreoTo = intServicio.GetCorreoXTipoProgramacion(evenclasecodi),
                CarpetaFiles = Math.Abs((int)DateTime.Now.Ticks) * -1
            };

            return View(model);
        }

        /// <summary>
        /// Envia via email y graba en la BD el mensaje
        /// </summary>
        /// <param name="interCodi">Id de intervención</param>
        /// <param name="asunto">Asunto del mensaje</param>
        /// <param name="mensaje">Cuerpo del mensaje</param>
        /// <param name="detalle">Cadena de datos de las intervenciones asociadas al mensaje</param>
        /// <param name="empresa">Id de empresa</param>
        /// <param name="lista">Lista</param>
        /// <param name="sModulo">Modulo</param>
        /// <param name="tipoProgramacion">Tipo de progrmación</param>
        /// <param name="sConcopia">Lista de correos copiados (Cc)</param>        
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult SaveIntervencionesMensajeRegistro(SiPlantillacorreoDTO plantillaCorreo)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                this.intServicio.EnviarComunicacion(ConstantesIntervencionesAppServicio.AmbienteExtranet, plantillaCorreo, base.UserEmail,
                                                        plantillaCorreo.Progrcodi, plantillaCorreo.CarpetaFiles);

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

        [HttpPost]
        public JsonResult MostrarListadoIntervencionesXIds(string intercodis)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                var listaIntervenciones = intServicio.ConsultarIntervencionesXIds(intercodis);
                var regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(listaIntervenciones.First().Progrcodi);

                //generar objeto para la vista
                model.ListaFilaWeb = listaIntervenciones.Select(x => new IntervencionFila()
                {
                    Intercodi = x.Intercodi,
                    Progrcodi = x.Progrcodi,

                    Equicodi = x.Equicodi,

                    Emprcodi = x.Emprcodi,
                    EmprNomb = x.EmprNomb,
                    EmprAbrev = x.Emprabrev,
                    Operadornomb = x.Operadornomb,
                    Tipoevenabrev = x.Tipoevenabrev,
                    AreaNomb = x.AreaNomb,
                    Famabrev = x.Famabrev,
                    Equiabrev = x.Equiabrev,
                    InterfechainiDesc = x.Interfechaini.ToString(ConstantesAppServicio.FormatoFechaFull),
                    InterfechafinDesc = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFechaFull),
                    Intermwindispo = x.Intermwindispo,
                    InterconexionprovDesc = x.InterconexionprovDesc,
                    IntersistemaaisladoDesc = x.IntersistemaaisladoDesc,
                    InterindispoDesc = x.InterindispoDesc,
                    InterinterrupDesc = x.InterinterrupDesc,
                    Interdescrip = x.Interdescrip,
                    Interjustifaprobrechaz = x.Interjustifaprobrechaz,
                    Interisfiles = x.Interisfiles,
                    Intercodsegempr = x.Intercodsegempr,

                    TipoComunicacion = x.TipoComunicacion,
                    ChkMensaje = x.ChkMensaje,
                    ChkAprobacion = x.ChkAprobacion,
                    UltimaModificacionUsuarioDesc = x.UltimaModificacionUsuarioDesc,
                    UltimaModificacionFechaDesc = x.UltimaModificacionFechaDesc,
                    UltimaModificacionUsuarioAgrupDesc = x.UltimaModificacionUsuarioAgrupDesc,
                    Estadocodi = x.Estadocodi,
                    Interdeleted = x.Interdeleted,
                    Interfuentestado = x.Interfuentestado,
                    Interprocesado = x.Interprocesado,
                    EstadoRegistro = x.EstadoRegistro,
                    ClaseProgramacion = x.ClaseProgramacion,

                    IniFecha = x.Interfechaini.ToString(ConstantesAppServicio.FormatoFecha),
                    //IniHora = x.Interfechaini.ToString("HH"),
                    //IniMinuto = x.Interfechaini.ToString("mm"),
                    FinFecha = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFecha),
                    //FinHora = x.Interfechafin.ToString("HH"),
                    //FinMinuto = x.Interfechafin.ToString("mm"),
                    IniHoraMinuto = x.Interfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora),
                    FinHoraMinuto = x.Interfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora)
                }).ToList();

                //obtener asunto
                List<string> listaAsuntoCorreo = new List<string>();
                var listaAgrup = listaIntervenciones.GroupBy(x => new { x.Emprcodi, x.Interindispo });
                foreach (var agrup in listaAgrup)
                {
                    string nombreEmpresa = agrup.First().Emprabrev;
                    string equipos = string.Join(", ", agrup.Select(x => string.Format("{0} ({1})", x.Equiabrev, x.AreaNomb)).Distinct());
                    listaAsuntoCorreo.Add(string.Format("Intervención del {0} {1} por {2}.", equipos, agrup.First().InterindispoDesc, nombreEmpresa));
                }
                model.Asunto = (intServicio.PrefijoAsuntoXPrograma(regProg) + " " + string.Join(" ", listaAsuntoCorreo)).Trim();
                model.CC = string.Join("; ", ObtenerCCcorreosAgente(listaIntervenciones, new List<string>() { base.UserEmail }));

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// correos para CC
        /// </summary>
        /// <param name="listaIdEmpresa"></param>
        /// <param name="emailsAgente"></param>
        /// <returns></returns>
        private List<string> ObtenerCCcorreosAgente(List<InIntervencionDTO> listaIntervencion, List<string> emailsAgente)
        {
            List<string> listaAllCorreo = new List<string>();
            foreach (var intervencion in listaIntervencion)
            {
                var listaCorreo = ObtenerCorreosGeneradorModuloIntervenciones(intervencion.Emprcodi, intervencion.Evenclasecodi);
                listaCorreo = listaCorreo.Where(x => !emailsAgente.Any(y => y == x)).OrderBy(x => x).ToList();

                listaAllCorreo.AddRange(listaCorreo);
            }

            return listaAllCorreo.Distinct().ToList();
        }

        /// <summary>
        /// Consultar correos por empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        private List<string> ObtenerCorreosGeneradorModuloIntervenciones(int idEmpresa, int evenclasecodi)
        {
            List<string> listaCorreo = intServicio.ObtenerCorreosPorEmpresa(idEmpresa, evenclasecodi);
            return listaCorreo;
        }

        #endregion

        #region 3.0 Reporte

        /// <summary>
        /// Carga el formulario principal de operaciones con intervenciones al haber seleccionado la programación.
        /// </summary>        
        [HttpPost]
        public JsonResult ExportarIntervenciones(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                intServicio.GenerarReporteIntervenciones(listaIntervenciones, path, pathLogo, out string fileName);

                model.Resultado = "1"; //indica si va haber una siguiente descarga
                model.NombreArchivo = fileName;
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

        #region 4.0 Reporte en Plantilla

        [HttpPost]
        public JsonResult GenerarManttoConsultaRegistro(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;
                objFiltro.AgruparIntervencion = true;

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(objFiltro.Progrcodi);
                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                string idEmpresas = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                , regProg.Progrfechaini, regProg.Progrfechafin, objFiltro.Evenclasecodi, idEmpresas, out string fileNameRenombre);

                model.Resultado = "1"; //indica si va haber una siguiente descarga
                model.NombreArchivo = fileNameRenombre;
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

        #region 5.0 Transferir (COPIA)

        /// GET: Intervenciones/Registro/
        /// <summary>
        /// Copiar o transferir a la BD las intervenciones de un horizonte superior
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param> 
        /// <param name="idTipoProgramacion">Id de Tipo de Programación</param> 
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult CopiarIntervenciones(int idProgramacion, int idTipoProgramacion)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                if (idProgramacion <= 0) throw new Exception(Constantes.MensajePermisoNoValido);

                this.SessionIdsEmpresaTotal = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);
                IntervencionFiltro objFiltro = intServicio.GetFiltroConsulta2(new IntervencionFiltro()
                {
                    Progrcodi = idProgramacion,
                    Evenclasecodi = idTipoProgramacion,
                    FechaIni = regProg.Progrfechaini,
                    FechaFin = regProg.Progrfechafin,
                    StrIdsEmpresa = this.SessionIdsEmpresaTotal,
                    EsReporteExcel = true
                });

                if (!intServicio.ExisteHorizonteSuperiorAprobado(idProgramacion))
                {
                    throw new ArgumentException("¡No existen registros del horizonte temporal superior aprobados por el COES para transferir!");
                }

                var listaIntervencionesExistentes = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                //validar si ya hay intervenciones creadas o importadas para la programación
                if (listaIntervencionesExistentes.Any())
                {
                    throw new Exception("¡Existen registros válidos en el programa seleccionado, no se permite la opción de Transferir!");
                }

                //Extracción de los registros de intervenciones - CopiarIntervenciones");
                var listaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(idTipoProgramacion,
                                                                             regProg.Progrfechaini,
                                                                             regProg.Progrfechafin,
                                                                             this.SessionIdsEmpresaTotal, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);
                listaIntervenciones = intServicio.ListarIntervencionesValidasACopiar(idTipoProgramacion, listaIntervenciones);
                //Validar si no existen registros para transferir en el horizonte superior
                if (listaIntervenciones.Count == 0)
                {
                    throw new Exception("¡No existen registros del horizonte temporal superior apobados por el COES para transferir!");
                }

                //Ejecución de la Copia de Intervenciones a la BD - CopiarIntervenciones");

                foreach (var entity in listaIntervenciones)
                    entity.Interipagente = GetIpAgente();

                intServicio.CopiarIntervenciones(listaIntervenciones, idProgramacion, idTipoProgramacion, base.UserEmail);
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
        /// ver programados que van a ser copiados
        /// </summary>
        /// <param name="idProgramacion"></param>
        /// <param name="idTipoProgramacion"></param>
        /// <returns></returns>
        public PartialViewResult ValidarCopiarProgramados(int idProgramacion, int idTipoProgramacion)
        {
            Intervencion model = new Intervencion();

            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.ListaIntervencionesEjecutadasFuturas = new List<InIntervencionDTO>();
                model.ListaValidacionHorasOperacionFS = new List<ResultadoValidacionAplicativo>();
                model.ListaValidacionHorasOperacionES = new List<ResultadoValidacionAplicativo>();

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);

                model.ListaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(idTipoProgramacion,
                                                                         regProg.Progrfechaini,
                                                                         regProg.Progrfechafin,
                                                                         this.SessionIdsEmpresaTotal, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);

                model.NroRegistros = model.ListaIntervenciones.Count;

                if (model.NroRegistros > 0)
                {
                    model.Resultado = "-3";
                }
                else
                {
                    model.ListaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(idTipoProgramacion,
                                                                         regProg.Progrfechaini,
                                                                         regProg.Progrfechafin,
                                                                         this.SessionIdsEmpresaTotal, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);

                    //Validar si son anteriores a la fecha y hora del sistema
                    model.ListaIntervencionesEjecutadasFuturas = intServicio.ListarIntervEjecutadosFuturas(idTipoProgramacion, model.ListaIntervenciones);

                    //Validar los registros con horas de operación
                    model.ListaIntervenciones = intServicio.ListarIntervencionesValidasACopiar(idTipoProgramacion, model.ListaIntervenciones);

                    List<ResultadoValidacionAplicativo> listaValHoFinal = new List<ResultadoValidacionAplicativo>();
                    intServicio.ListarDetallesAlertasInterTransferirOImportar(idTipoProgramacion, model.ListaIntervenciones, ref listaValHoFinal);

                    List<int> listaIntercodiES = listaValHoFinal.Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.FlagIndispES).Where(x => x.Intervencion != null).Select(x => x.Intervencion.Intercodi).ToList();
                    model.ListaIntervencionesES = model.ListaIntervenciones.Where(x => listaIntercodiES.Contains(x.Intercodi)).ToList();
                    model.ListaValidacionHorasOperacionES = listaValHoFinal.Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.FlagIndispES).ToList();

                    List<int> listaIntercodiFS = listaValHoFinal.Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.FlagIndispFS).Where(x => x.Intervencion != null).Select(x => x.Intervencion.Intercodi).ToList();
                    model.ListaIntervencionesFS = model.ListaIntervenciones.Where(x => listaIntercodiFS.Contains(x.Intercodi)).ToList();
                    model.ListaValidacionHorasOperacionFS = listaValHoFinal.Where(x => x.Interindispo == ConstantesIntervencionesAppServicio.FlagIndispFS).ToList();

                    // si no existen intervenciones futuras debe realizar la transferencia
                    if (model.ListaIntervencionesEjecutadasFuturas.Count == 0 && listaValHoFinal.Count == 0)
                    {
                        model.Resultado = "-2";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return PartialView(model);
        }

        #endregion

        #region 6.0 Importación

        /// GET: Intervenciones/Registro/IntervencionesImportacion
        /// <summary>
        /// Carga del modulo de importación de intervenciones
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param>
        /// <param name="tipoProgramacion">Id de Tipo de Programación</param>
        /// <param name="emprcodi">Id de empresan</param>
        /// <returns>View(model)</returns>
        public ActionResult IntervencionesImportacion(int progrCodi)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();
            if (progrCodi == 0) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion
            {
                Entidad = new InIntervencionDTO()
            };
            model.Entidad.Progrcodi = progrCodi;
            model.sIdsEmpresas = this.SessionIdsEmpresaSelected;

            InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrCodi);
            model.IdTipoProgramacion = regProg.Evenclasecodi;
            model.NombreProgramacion = regProg.Nomprogramacion;

            return View(model);
        }

        /// <summary>
        /// Sube los archivos a una carpeta aislada para luego ser movida a la carpeta del id generado
        /// </summary>
        /// <param name="sFecha">Cadena de Fecha</param>        
        /// <returns>Json</returns>
        [HttpPost]
        public ActionResult UploadIntervencion(string sFecha)
        {
            base.ValidarSesionJsonResult();

            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

            try
            {
                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string sNombreArchivo = sFecha + "_" + file.FileName;

                    if (FileServer.VerificarExistenciaFile(null, sNombreArchivo, path))
                    {
                        FileServer.DeleteBlob(sNombreArchivo, path + ConstantesIntervencionesAppServicio.Reportes);
                    }
                    file.SaveAs(path + sNombreArchivo);

                    return Json(new
                    {
                        success = true,
                        nuevonombre = sNombreArchivo,
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Importar intervenciones desde un archivo de formato Excel
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param>
        /// <param name="tipoProgramacion">Id de Tipo de Programación</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="accion">Accion realizada: 1 = Reemplazar; 2 = Adicionar</param> 
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ImportarIntervencionesExcel(int progrCodi, string fileName, string accion, int evenclasecodi, string dataJsonIncl, string dataJsonExcl)
        {
            Intervencion model = new Intervencion
            {
                ListaIntervencionesCorrectas = new List<InIntervencionDTO>(),
                ListaIntervencionesErrores = new List<InIntervencionDTO>()
            };

            try
            {
                base.ValidarSesionJsonResult();

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

                // Obtener array de Ids de empresas del usuario autenticado
                int[] idEmpresasUsuario = this.ObtenerEmpresasPorUsuario(base.UserName).ToArray();

                //intervenciones con sustento. solo para semanal y diario
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<InIntervencionDTO> listaWebSustentoIncl = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaWebSustentoExcl = new List<InIntervencionDTO>();
                if (dataJsonIncl != null) listaWebSustentoIncl = serializer.Deserialize<List<InIntervencionDTO>>(dataJsonIncl);
                if (dataJsonExcl != null) listaWebSustentoExcl = serializer.Deserialize<List<InIntervencionDTO>>(dataJsonExcl);

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                // con el parametro de salida idEmpresasExcel para capturar las empresas de la lectura del ARCHIVO EXCEL
                intServicio.ValidarIntervencionesAImportarXLSX(ConstantesIntervencionesAppServicio.AmbienteExtranet, path, fileName, progrCodi, base.UserEmail, accion, idEmpresasUsuario,
                                                       out List<InIntervencionDTO> lstRegIntervencionesCorrectos,
                                                       out List<InIntervencionDTO> lstRegIntervencionesErroneos,
                                                       out List<InIntervencionDTO> listaNuevo,
                                                       out List<InIntervencionDTO> listaModificado,
                                                       out List<InIntervencionDTO> listaEliminado);

                model.ListaIntervencionesCorrectas = lstRegIntervencionesCorrectos;
                model.ListaIntervencionesErrores = lstRegIntervencionesErroneos;

                //1. validación si existen errores
                if (lstRegIntervencionesErroneos.Any())
                {
                    //Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 4");
                    string filenameCSV = intServicio.GenerarArchivoIntervencionesErroneasCSV(path, lstRegIntervencionesErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //2. validación si no tiene datos
                if (lstRegIntervencionesCorrectos.Count() == 0)
                {
                    //Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 4");
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //3. validación de sustento para inclusión / exclusión
                if (!listaWebSustentoIncl.Any() && !listaWebSustentoExcl.Any())
                {
                    intServicio.ListarIntervencionImportacionConPlantillaExclusionInclusion(evenclasecodi, progrCodi, listaNuevo, listaModificado, listaEliminado,
                                    out List<InIntervencionDTO> listaInterExcl, out List<InIntervencionDTO> listaInterIncl);

                    model.ListaInclusion = listaInterIncl;
                    model.ListaExclusion = listaInterExcl;

                    if (model.ListaInclusion.Any() || model.ListaExclusion.Any())
                        throw new Exception("Debe ingresar sustento de inclusión / exclusión para las siguientes intervenciones.");
                }
                else
                {
                    foreach (var entity in listaNuevo)
                    {
                        //setear sustento
                        var regWeb = listaWebSustentoIncl.Find(x => x.NroItem == entity.NroItem);
                        if (regWeb != null)
                        {
                            if (regWeb.Sustento != null && regWeb.Sustento.TienePlantillaCompleta) entity.Sustento = regWeb.Sustento;
                            if (entity.Sustento != null)
                            {
                                entity.CarpetaInTemporal = regWeb.Intercarpetafiles;
                                entity.Interisfiles = ConstantesAppServicio.SI;
                                entity.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                            }
                        }
                    }

                    var listaNuevoTmp = new List<InIntervencionDTO>();
                    var listaModificadoTmp = new List<InIntervencionDTO>();
                    var listaEliminadoTmp = new List<InIntervencionDTO>();
                    foreach (var entity in listaModificado)
                    {
                        //setear sustento
                        var regWebIncl = listaWebSustentoIncl.Find(x => x.NroItem == entity.NroItem);
                        var regWebExcl = listaWebSustentoExcl.Find(x => x.NroItem == entity.NroItem);

                        if (regWebIncl == null && regWebExcl == null)
                        {
                            listaModificadoTmp.Add(entity);
                        }
                        else
                        {
                            if (regWebExcl != null)
                            {
                                //el registro histórico se guadará como eliminado
                                var regEliminar = intServicio.GetByIdInIntervencion(entity.Intercodi);
                                if (regWebExcl.Sustento != null && regWebExcl.Sustento.TienePlantillaCompleta)
                                {
                                    regEliminar.Sustento = regWebExcl.Sustento;
                                    regEliminar.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                                }
                                listaEliminadoTmp.Add(regEliminar);

                                //el registro que está en el formulario web se guardará como nuevo
                                entity.Intercodi = 0;
                                entity.Intercodipadre = null;
                                if (regWebIncl.Sustento != null && regWebIncl.Sustento.TienePlantillaCompleta) entity.Sustento = regWebIncl.Sustento;
                                if (entity.Sustento != null)
                                {
                                    entity.Interisfiles = ConstantesAppServicio.SI;
                                    entity.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                                }
                                //model.Entidad.Interjustifaprobrechaz = null;
                                listaNuevoTmp.Add(entity);
                            }
                            else
                            {
                                if (regWebIncl.Sustento != null && regWebIncl.Sustento.TienePlantillaCompleta) entity.Sustento = regWebIncl.Sustento;
                                if (entity.Sustento != null)
                                {
                                    entity.Interisfiles = ConstantesAppServicio.SI;
                                    entity.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                                }
                                listaModificadoTmp.Add(entity);
                            }
                        }
                    }

                    foreach (var entity in listaEliminado)
                    {
                        //setear sustento
                        var regWeb = listaWebSustentoExcl.Find(x => x.NroItem == entity.NroItem);
                        if (regWeb != null)
                        {
                            if (regWeb.Sustento != null && regWeb.Sustento.TienePlantillaCompleta) entity.Sustento = regWeb.Sustento;
                            if (entity.Sustento != null)
                            {
                                entity.Interisfiles = ConstantesAppServicio.SI;
                                entity.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;
                            }
                        }
                    }
                }

                //4. Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 1");
                // Se guardan los registros importados en la BD con el parametroidEmpresasExcel para realizar
                // la eliminación fisica de registros de los Id empresas obtendos del archivo Excel

                foreach (var entity in lstRegIntervencionesCorrectos)
                {
                    entity.Interipagente = GetIpAgente();
                }

                intServicio.ImportarIntervenciones(listaNuevo, listaModificado, listaEliminado, progrCodi, base.UserEmail);

                model.StrMensaje = "¡La Información se grabó correctamente!";
            }
            catch (Exception ex)
            {
                model.StrMensaje = ex.Message;
                model.Resultado = "-1";
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }

            var json = Json(model);
            json.MaxJsonLength = Int32.MaxValue;

            return json;
        }

        /// <summary>
        /// Abrir archivo log de errores en formato CSV 
        /// </summary>
        /// <param name="file">Nombre del archivo</param>               
        /// <returns>Cadena del nombre del archivo</returns>
        public virtual ActionResult AbrirArchivoCSV(string file)
        {
            int formato = 4;
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes + file;

            //--------------------------------------------------------------------------------------------------------
            string app = (formato == 4) ? ConstantesIntervencionesAppServicio.AppCSV : (formato == 2) ? ConstantesIntervencionesAppServicio.AppPdf : ConstantesIntervencionesAppServicio.AppWord;
            //--------------------------------------------------------------------------------------------------------

            //lo guarda el CSV en la carpeta de descarga
            return File(path, app, file);
        }

        #endregion

        #region 7.0 Mantto Transf.

        /// <summary>
        /// Copiar o transferir intervenciones al formato Excel Mantto de un horizonte superior
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param>
        /// <param name="idTipoProgramacion">Id de Tipo de Programación</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult ExportarIntervencionesXLS(int idProgramacion)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);

                var listaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(regProg.Evenclasecodi,
                                                                                regProg.Progrfechaini,
                                                                                regProg.Progrfechafin,
                                                                                this.SessionIdsEmpresaSelected, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                model.NombreArchivo = fileName;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;
                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                string idEmpresas = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                    , regProg.Progrfechaini, regProg.Progrfechafin, regProg.Evenclasecodi, idEmpresas, out string fileNameRenombre);

                if (listaIntervenciones.Count == 0)
                {
                    model.Mensaje = "¡No existen registros del horizonte temporal superior apobados por el COES para exportar. Se descargará solo el archivo(plantilla)!";
                }
                else
                {
                    model.Mensaje = "¡Se ha realizado la exportación de los registros del horizonte superior que pueden ser transferidos!";
                }

                model.Resultado = "1";
                model.NombreArchivo = fileNameRenombre;
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
        /// verificar la obligatoriedad de transferir
        /// </summary>
        /// <param name="idProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerNumeroIntervencionHorizonteSuperior(int idProgramacion)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                //El COES no tiene programa aprobado en el horizonte superior
                if (!intServicio.ExisteHorizonteSuperiorAprobado(idProgramacion))
                {
                    model.Total = 1;
                }
                else
                {
                    InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);

                    //1 o más: El agente tiene registros del horizonte superior que pueden ser transferidos
                    var listaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(regProg.Evenclasecodi,
                                                                                regProg.Progrfechaini,
                                                                                regProg.Progrfechafin,
                                                                                this.SessionIdsEmpresaSelected, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);
                    model.Total = listaIntervenciones.Count;
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

        #region 8.0 Plantilla

        [HttpPost]
        public JsonResult DescargarManttoPlantillaActualizada(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(objFiltro.Progrcodi);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                string idEmpresas = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                intServicio.GenerarPlantillaActualizada(AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                , regProg.Progrfechaini, regProg.Progrfechafin, objFiltro.Evenclasecodi, idEmpresas, out string fileNameRenombre);

                model.Resultado = "1"; //indica si va haber una siguiente descarga
                model.NombreArchivo = fileNameRenombre;
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

        #region 9.0 Descargar Archivos Adjuntos

        /// <summary>
        /// Descargar Archivos adjuntos de las intervenciones seleccionadas
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarArchivosSeleccionados(string tipo)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //Validar que las intervenciones seleccionadas tengan archivos
                listaIntercodiChecked = intServicio.ObtenerIntercodisConAdjuntos(listaIntercodiChecked);
                if (listaIntercodiChecked.Count < 1)
                    throw new ArgumentException("No existe ninguna intervención con archivos adjuntos");

                int numeroAleatorio = (int)DateTime.Now.Ticks;
                intServicio.DescargarArchivosAdjuntos(numeroAleatorio.ToString(), listaIntercodiChecked, out string nameFile);
                model.Resultado = nameFile;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        public virtual FileResult ExportarZip()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string nombreArchivo = Request["file_name"];

            string modulo = ConstantesIntervencionesAppServicio.ModuloArchivosXIntervenciones;
            string pathDestino = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + "Temporal_" + modulo + @"/" + ConstantesIntervencionesAppServicio.NombreArchivosZip;
            string pathAlternativo = intServicio.GetPathPrincipal();
            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, sFecha + "_" + nombreArchivo);
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

        #region 10.0 Reporte de comunicaciones

        [HttpPost]
        public JsonResult DescargarReporteComunicacionSeleccionados(string intercodis)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaEmprcodiLectura = this.ObtenerEmpresasPorUsuario(base.UserName);

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(intercodis))
                {
                    listaIntercodiChecked = intercodis.Replace(";", ",").Split(',').Select(x => Int32.Parse(x)).Distinct().ToList();

                    var listaIntervenciones = intServicio.ConsultarIntervencionesXIds(intercodis.Replace(";", ",")).Where(x => x.Intermensaje == ConstantesAppServicio.SI).ToList();
                    if (!listaIntervenciones.Any())
                    {
                        throw new ArgumentException("Debe seleccionar al menos una intervención con comunicaciones.");
                    }

                    string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                    string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                    intServicio.GenerarReporteComunicacionesIntervenciones(listaIntervenciones, ConstantesIntervencionesAppServicio.AmbienteExtranet, listaEmprcodiLectura,
                                                                path, pathLogo, out string fileName);

                    model.Resultado = "1"; //indica si va haber una siguiente descarga
                    model.NombreArchivo = fileName;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        #endregion

        #region INTERVENCIONES - MODULO DE CONSULTA TABULAR

        /// GET: Intervenciones/Registro/IntervencionesConsulta
        /// <summary>
        /// Carga el formulario de consultas de intervenciones
        /// </summary>        
        public ActionResult IntervencionesConsulta()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion
            {
                Entidad = new InIntervencionDTO()
            };

            // Obtener empresas por usuario
            string idEmpresasXUser = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));

            this.SessionIdsEmpresaTotal = idEmpresasXUser;
            if (string.IsNullOrEmpty(this.SessionIdsEmpresaSelected)) this.SessionIdsEmpresaSelected = idEmpresasXUser; //inicializar variable de sesion
            model.ListaCboEmpresa = intServicio.ListarComboEmpresasById(idEmpresasXUser);

            model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaCboUbicacion = intServicio.ListarComboUbicacionesXEmpresa("0");
            model.ListaFamilias = intServicio.ListarComboFamilias();

            model.Progrfechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.Progrfechafin = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Carga el formulario de consultas de intervenciones
        /// </summary>    
        [HttpPost]
        public JsonResult MostrarListadoConsulta(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesTabulares(objFiltro);
                model.IdTipoProgramacion = objFiltro.Evenclasecodi;

                //generar objeto para la vista
                model.ListaFilaWeb = listaIntervenciones.Select(x => new IntervencionFila()
                {
                    EmprNomb = x.EmprNomb,
                    Tipoevenabrev = x.Tipoevenabrev,
                    AreaNomb = x.AreaNomb,
                    Famabrev = x.Famabrev,
                    Equiabrev = x.Equiabrev,
                    InterfechainiDesc = x.Interfechaini.ToString(ConstantesAppServicio.FormatoFechaFull),
                    InterfechafinDesc = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFechaFull),
                    Intermwindispo = x.Intermwindispo,
                    InterindispoDesc = x.InterindispoDesc,
                    InterinterrupDesc = x.InterinterrupDesc,
                    Interdescrip = x.Interdescrip,
                    EstadoRegistro = x.EstadoRegistro,
                    ChkMensaje = x.ChkMensaje,
                    Estadocodi = x.Estadocodi,
                    Interdeleted = x.Interdeleted,

                    UltimaModificacionUsuarioDesc = x.UltimaModificacionUsuarioDesc,
                    UltimaModificacionFechaDesc = x.UltimaModificacionFechaDesc
                }).ToList();
                var listaNotificacion = listaIntervenciones.Where(x => x.Interleido == 0).ToList();
                model.ListaNotificaciones = listaNotificacion;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return JsonResult;
        }

        /// <summary>
        /// Carga el formulario de consultas de intervenciones
        /// </summary>    
        [HttpPost]
        public JsonResult ExportarIntervencionesConsulta(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;

                var listaIntervenciones = intServicio.ConsultarIntervencionesTabulares(objFiltro);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                intServicio.GenerarReporteIntervenciones(listaIntervenciones, path, pathLogo, out string fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
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
        public JsonResult GenerarManttoConsultaConsulta(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;
                objFiltro.AgruparIntervencion = true;

                //Genera el archivo Mantto Excel.xlsm de consulta de intervenciones - GenerarManttoConsultaConsulta");
                var listaIntervenciones = intServicio.ConsultarIntervencionesTabulares(objFiltro);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                //Genera el archivo Mantto Excel.xlsm de consulta de intervenciones - GenerarManttoConsultaConsulta");
                string idEmpresas = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName,
                                                objFiltro.FechaIni, objFiltro.FechaFin, objFiltro.Evenclasecodi, idEmpresas, out string fileNameRenombre);

                model.Resultado = "1";
                model.NombreArchivo = fileNameRenombre;
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

        #region INTERVENCIONES - MODULO DE CONSULTA CRUZADA (XMANTTO)

        /// GET: Intervenciones/Registro/IntervencionesCruzadasConsulta
        public ActionResult IntervencionesCruzadasConsulta()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            Intervencion model = new Intervencion();

            // Obtener empresas por usuario
            string idEmpresasXUser = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));

            this.SessionIdsEmpresaTotal = idEmpresasXUser;
            if (string.IsNullOrEmpty(this.SessionIdsEmpresaSelected)) this.SessionIdsEmpresaSelected = idEmpresasXUser; //inicializar variable de sesion
            model.ListaCboEmpresa = intServicio.ListarComboEmpresasById(idEmpresasXUser);

            model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsultaCruzadas);

            model.ListadoHrasIndisponiblidad = new List<HorasIndispo>
            {
                new HorasIndispo { id = 0, value = "HrMaxIndisp" }
            };
            for (int i = 0; i < 48; i++)
            {
                var obj = new HorasIndispo
                {
                    id = i + 1,
                    value = UtilIntervencionesAppServicio.Listar48MediasHoras(i).ToString()
                };
                model.ListadoHrasIndisponiblidad.Add(obj);
            }

            model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioMantenimiento);
            model.ListaClaseProgramacion = intServicio.ListarComboClasesProgramacion();

            model.Progrfechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);
            model.Progrfechafin = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Muestra la grilla excel para inervenciones Cruzadas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrillaExcel(IntervencionInputWeb objInput)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();

                //empresa = ValidarComboEmpresa(empresa);
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                model.GridExcel = intServicio.ObtenerExcelWebIntervencionesCruzadas(objFiltro);
                model.GridExcel.IdTipoProgramacion = objFiltro.Evenclasecodi;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = int.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return JsonResult;
        }

        /// <summary>
        /// Permite  Exportar a Excel los resultados de la consulta cruzada
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarIntervencionesCruzadas(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                //Reporte con formato (logo, colores)
                intServicio.ExportarIntervencionesCruzadas(objFiltro, path, pathLogo, out string fileNameReporte);

                model.Resultado = "1"; //indica si va haber una siguiente descarga
                model.NombreArchivo = fileNameReporte;
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
        /// Combo en cascada Programaciones x Tipo Programacion
        /// </summary>
        [HttpPost]
        public JsonResult ListarProgramaciones(int idTipoProgramacion)
        {
            Programacion model = new Programacion
            {
                IdTipoProgramacion = idTipoProgramacion,
                ListaProgramaciones = intServicio.ListarProgramacionesRegistro(idTipoProgramacion)
            };
            model.Entidad = intServicio.GetProgramacionDefecto(idTipoProgramacion, model.ListaProgramaciones, DateTime.Today, false);

            return Json(model);
        }

        #endregion

        #region Carga de archivos - Intervenciones / Mensajes

        /// <summary>
        /// Sube los archivos a una carpeta aislada para luego ser movida a la carpeta del id generado
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="progrcodi"></param>
        /// <param name="carpetafiles"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFileEnPrograma(string modulo, int progrcodi, int carpetafiles, int subcarpetafiles)
        {
            ArchivosModel model = new ArchivosModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];

                    DateTime fechaAhora = DateTime.Now;
                    intServicio.UploadArchivoEnPrograma(modulo, progrcodi, carpetafiles, subcarpetafiles, file.FileName, file.InputStream, fechaAhora, out string fileNamefisico);

                    return Json(new
                    {
                        success = true,
                        nuevonombre = fileNamefisico,
                        tieneVistaPreviaOffice = IntervencionesAppServicio.TieneVistaPreviaOffice(fileNamefisico),
                        tieneVistaPreviaNoOffice = IntervencionesAppServicio.TieneVistaPreviaNoOffice(fileNamefisico),
                    }, JsonRequestBehavior.AllowGet);
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
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="progrcodi"></param>
        /// <param name="carpetafiles"></param>
        /// <param name="fileName"></param>
        /// <param name="fileNameOriginal"></param>
        /// <returns></returns>
        public virtual ActionResult DescargarArchivoDePrograma(string modulo, int progrcodi, int carpetafiles, int subcarpetafiles, string fileName, string fileNameOriginal)
        {
            byte[] buffer = intServicio.GetBufferArchivoEnPrograma(modulo, progrcodi, carpetafiles, subcarpetafiles, fileName);

            if (buffer != null)
            {
                return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOriginal);
            }

            return base.DescargarArchivoNoDisponible();
        }

        /// <summary>
        /// Copiar archivo a carpeta publica para que lo pueda consultar el Office.live
        /// </summary>
        /// <param name="modulo"></param>
        /// <param name="progrcodi"></param>
        /// <param name="carpetafiles"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VistaPreviaArchivoDePrograma(string modulo, int progrcodi, int carpetafiles, int subcarpetafiles, string fileName)
        {
            ArchivosModel model = new ArchivosModel();

            try
            {
                string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                intServicio.CopiarArchivoACarpetaPublica(modulo, pathDestino, progrcodi, carpetafiles, subcarpetafiles, fileName);

                string url = ConstantesIntervencionesAppServicio.RutaReportes + fileName;

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

        #endregion

        #region COMBOS EN CASCADA
        /// <summary>
        /// Combo en cascada Ubicación x Empresa
        /// </summary>
        [HttpPost]
        public JsonResult ListarCboUbicacion(List<string> idEmpresa)
        {
            base.ValidarSesionJsonResult();

            string lista = "0";

            if (idEmpresa.Count() > 0)
            {
                lista = string.Empty;
                for (int i = 0; i < idEmpresa.Count(); i++)
                {
                    lista += idEmpresa[i] + ",";
                }

                if (lista.TrimEnd(',') == string.Empty)
                {
                    lista = "0";
                }
                else
                {
                    lista = lista.TrimEnd(',');
                }
            }

            Intervencion model = new Intervencion
            {
                ListaCboUbicacion = intServicio.ListarComboUbicacionesXEmpresa(lista)
            };

            return Json(model);
        }

        /// <summary>
        /// Combo en cascada Causas x TipoEvenCodi (Tipo intervencion)
        /// </summary>
        [HttpPost]
        public JsonResult ListarCboCausa(List<string> claProCodi)
        {
            base.ValidarSesionJsonResult();

            Intervencion model = new Intervencion
            {
                ListaCausas = new List<EveSubcausaeventoDTO>()
            };
            string listaTipo = string.Join(",", claProCodi.ToArray());

            if (listaTipo != "")
            {
                List<int> listaclaprocodi = new List<int>();
                if (listaTipo != "0")
                {
                    listaclaprocodi = listaTipo.Split(',').Select(x => int.Parse(x)).ToList();
                }

                foreach (var item in listaclaprocodi)
                {
                    var listaparcial = intServicio.ListarComboSubCausas(item);

                    model.ListaCausas.AddRange(listaparcial);
                }

                //if (model.ListaCausas.Any())
                //    model.ListaCausas = model.ListaCausas;
            }

            return Json(model);
        }

        /// <summary>
        /// Combo en cascada Equipo x Ubiciación x Tipo Equipo (Familia) y tipo de programa
        /// </summary>
        /// <param name="idUbicacion"></param>
        /// <param name="idFamilia"></param>
        /// <param name="evenclasecodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEquiposXprograma(List<string> idUbicacion, List<string> idFamilia, int evenclasecodi)
        {
            base.ValidarSesionJsonResult();
            Intervencion model = new Intervencion();

            string listaUbicacion = string.Join(",", idUbicacion.ToArray());
            string listaFamilia = string.Join(",", idFamilia.ToArray());

            model.ListaCboEquipo = intServicio.ListarEquipoGenerico(intServicio.ListarEquiposXTipoPrograma(evenclasecodi, listaUbicacion, listaFamilia));

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;
            Response.AddHeader("Content-Encoding", "gzip");
            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return JsonResult;
        }

        #endregion

        #region METODOS COMUNES

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

            return base.DescargarArchivoTemporalYEliminarlo(path, file);
        }

        /// <summary>
        /// Función que se encarga de descargar el archivo ubicado en un directorio cualquiera,
        /// en el explorador
        /// </summary>
        /// <returns></returns>
        public FileResult DescargarArchivoDesdeCualquierDirectorio(string fullPath, string filename)
        {
            byte[] buffer = FileServer.DownloadToArrayByte(fullPath, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        #endregion

        private IntervencionFiltro GetFiltroConsultaWeb(IntervencionInputWeb input)
        {
            IntervencionFiltro obj = intServicio.GetFiltroConsulta1(input);

            //validación. Solo debe permitirse las acciones sobre las empresas asociadas al agente en sesión
            bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, base.UserName);
            var listaEmpresaAgente = this.ObtenerEmpresasPorUsuario(base.UserName);

            if (!accesoEmpresa)
            {
                //si quiere acceder a todas las empresas pero no tiene permiso entonces mandar mensaje
                if (obj.StrIdsEmpresa == "0")
                {
                    input.Emprcodi = string.Join(",", this.ObtenerEmpresasPorUsuario(base.UserName));
                    obj = intServicio.GetFiltroConsulta1(input);
                }

                //si el agente tiene 1 o más empresas
                if (obj.StrIdsEmpresa != "0")
                {
                    foreach (var emprcodiSeleccionado in obj.ListaEmprcodi)
                    {
                        if (!listaEmpresaAgente.Contains(emprcodiSeleccionado))
                        {
                            throw new ArgumentException("Usted no tiene acceso a una o varias de las empresas seleccionadas.");
                        }
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Obtener empresas segun usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="escenario"></param>
        /// <returns></returns>
        public List<int> ObtenerEmpresasPorUsuario(string usuario)
        {
            List<SiEmpresaDTO> listaEmpresas = new List<SiEmpresaDTO>();

            bool accesoEmpresa = base.VerificarAccesoAccion(Acciones.AccesoEmpresa, usuario);

            var empresas = intServicio.ListarComboEmpresas();
            if (accesoEmpresa)
            {
                if (empresas.Count > 0)
                    listaEmpresas = empresas;
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }
            else
            {
                var listaEmpresasSesion = base.ListaEmpresas;
                var emprUsuario = empresas.Where(x => listaEmpresasSesion.Any(y => x.Emprcodi == y.EMPRCODI)).ToList();
                if (emprUsuario.Count() > 0)
                {
                    listaEmpresas = emprUsuario.ToList();
                }
                else
                {
                    listaEmpresas = new List<SiEmpresaDTO>(){
                         new SiEmpresaDTO(){
                             Emprcodi = 0,
                             Emprnomb = "No Existe"
                         }
                     };
                }
            }

            if (!listaEmpresas.Any())
            {
                throw new ArgumentException("El módulo no está disponible. No existen empresas asociadas al usuario.");
            }

            return listaEmpresas.Select(x => x.Emprcodi).ToList();
        }

        /// <summary>
        /// Obtiene la Ip pública del Agente
        /// </summary>
        /// <returns></returns>
        private string GetIpAgente()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            var ipAgente = context.Request.ServerVariables["REMOTE_ADDR"];
            ipAgente = ipAgente == "::1" ? "127.0.0.1" : ipAgente;
            return ipAgente;

            //obtiene Ip Privada
            //var hostName = Dns.GetHostName();
            //IPHostEntry ipEntry = new IPHostEntry();
            //ipEntry = Dns.GetHostEntry(hostName);
            //return Convert.ToString(ipEntry.AddressList[ipEntry.AddressList.Length - 1]);
        }

    }
}
