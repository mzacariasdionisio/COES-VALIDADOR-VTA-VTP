using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Intervenciones.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.Eventos;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Intervenciones;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.Intervenciones.Controllers
{
    public class RegistroController : BaseController
    {
        readonly IntervencionesAppServicio intServicio = new IntervencionesAppServicio();
        readonly IEODAppServicio servIeod = new IEODAppServicio();
        readonly EventoAppServicio servEvento = new EventoAppServicio();
        readonly SeguridadServicioClient servSeguridad = new SeguridadServicioClient();

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

        #endregion

        #region INTERVENCIONES - PROGRAMACIONES (PAGINA INICIAL)

        /// GET: Intervenciones/Registro/
        /// <summary>
        /// Carga el modulo de programaciones.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Programaciones(int tipoProgramacion = 0)
        {
            Intervencion model = new Intervencion
            {
                ListaCboEmpresa = intServicio.ListarComboEmpresas(),
                ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta),
                IdTipoProgramacion = tipoProgramacion,

                //ejecutado, diario
                Progrfechaini = DateTime.Today.AddYears(-1).ToString(ConstantesAppServicio.FormatoFecha),//diario y ejecutado
                Progrfechafin = DateTime.Today.AddDays(2).ToString(ConstantesAppServicio.FormatoFecha),//ejecutado
                ProgrfechafinH = DateTime.Today.AddDays(7).ToString(ConstantesAppServicio.FormatoFecha)// diario
            };

            //programa semanal
            DateTime finicio = EPDate.f_fechainiciosemana(DateTime.Today.AddYears(-1));
            DateTime ffin = EPDate.f_fechainiciosemana(DateTime.Today.AddDays(7 * 4));
            Tuple<int, int> anioSemIni = EPDate.f_numerosemana_y_anho(finicio);
            Tuple<int, int> anioSemFin = EPDate.f_numerosemana_y_anho(ffin);
            model.AnhoIni = anioSemIni.Item2.ToString();
            model.SemanaIni = anioSemIni.Item1.ToString();
            model.AnhoFin = anioSemFin.Item2.ToString();
            model.SemanaFin = anioSemFin.Item1.ToString();

            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaSemanasIni = ListarSemanasByAnio(anioSemIni.Item2);
            model.FechaFin = ffin.ToString(ConstantesAppServicio.FormatoFecha);
            model.ListaSemanasFin = ListarSemanasByAnio(anioSemFin.Item2);

            //mensual
            model.MesIni = DateTime.Today.AddYears(-2).ToString(ConstantesAppServicio.FormatoMes);
            model.MesFin = DateTime.Today.AddMonths(2).ToString(ConstantesAppServicio.FormatoMes);

            return View(model);
        }

        /// <summary>
        /// Carga las programaciones por tipo de programación.
        /// </summary>
        /// <param name="idTipoProgramacion">Id de Tipo de Programación</param> 
        /// <returns>ActionResult</returns>
        [HttpPost]
        public JsonResult ProgramacionesListado(int idTipoProgramacion, string fdesde, string fhasta)
        {
            Programacion model = new Programacion();

            try
            {
                base.ValidarSesionJsonResult();

                model.IdTipoProgramacion = idTipoProgramacion;

                DateTime desde = DateTime.MaxValue;
                DateTime hasta = DateTime.MaxValue;

                //convertir a fechas
                switch (idTipoProgramacion)
                {
                    //1 //2
                    case ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado:
                    case ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario:
                        desde = DateTime.ParseExact(fdesde, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        hasta = DateTime.ParseExact(fhasta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        break;
                    //3
                    case ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoSemanal:

                        desde = DateTime.ParseExact(fdesde, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        hasta = DateTime.ParseExact(fhasta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        break;
                    //4
                    case ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoMensual:
                        desde = DateTime.ParseExact(fdesde, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                        hasta = DateTime.ParseExact(fhasta, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);
                        break;
                    //5
                    case ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoAnual:
                        desde = DateTime.Today.AddYears(-2).AddMonths(-6);
                        hasta = DateTime.Today.AddMonths(7);
                        break;
                }

                model.ListaProgramaciones = intServicio.ListarProgramacionesRegistroFiltro(idTipoProgramacion, desde, hasta);
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
        /// Aprobar la programación seleccionada.
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param> 
        /// <param name="idTipoProgramacion">Id de Tipo de Programación</param> 
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult AprobarPlan(int progrCodi)
        {
            IntervencionResultado model = new IntervencionResultado
            {
                Resultado = "0"
            };

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrCodi);
                if (regProg.Progrsololectura == ConstantesIntervencionesAppServicio.ProgSoloLectura)
                {
                    throw new ArgumentException("El programa ya se encuentra aprobado.");
                }

                List<InIntervencionDTO> listaIntervenciones = intServicio.ObtenerIntervencionesPorAprobar(progrCodi, out List<string> listaMsj);
                if (listaMsj.Any())
                    throw new ArgumentException("Existen observaciones de los siguientes equipos: " + string.Join(".\n ", listaMsj));

                if (listaIntervenciones.Any())
                {
                    intServicio.AprobarIntervencionesPrograma(progrCodi, base.UserName);
                    model.Resultado = "1";
                }
                else
                {
                    throw new ArgumentException("No existen registros con estado Conforme para aprobar.");
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
        public JsonResult AprobarReversion(int progrCodi, int idTipoProgramacion)
        {
            IntervencionResultado model = new IntervencionResultado();
            //model.Resultado = "0";
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrCodi);
                if (regProg.Progresaprobadorev == ConstantesIntervencionesAppServicio.AprobadoReversion)
                {
                    throw new ArgumentException("Los registros revertidos del programa ya se encuentran aprobados.");
                }

                var listaIntervencionesXReversion = intServicio.ObtenerIntervencionesRevertidasProcesar(progrCodi, out List<string> listaMsj2);
                if (listaMsj2.Any())
                    throw new ArgumentException("Existen observaciones de los siguientes equipos: " + string.Join(".\n ", listaMsj2));

                if (listaIntervencionesXReversion.Any())
                {
                    intServicio.AprobarReversionIntervenciones(progrCodi, listaIntervencionesXReversion, idTipoProgramacion, base.UserName);
                    model.Resultado = "1";
                }
                else
                {
                    throw new ArgumentException("No existen registros para aprobar.");
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

        #region Informe Word

        /// <summary>
        /// Generar informe en formato Word de la programación seleccionada
        /// </summary>
        /// <param name="progrCodi"></param>
        /// <param name="tipo"></param>
        /// <param name="sTieneSelloSemanal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarWordInforme(int progrCodi, int tipo, string sTieneSelloSemanal)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
            string pathLogoIntervenciones = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogoIntervenciones;
            string pathFirmaIntervenciones = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathFirmaIntervenciones;
            string pathFirmaIntervencionesBlanco = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathFirmaIntervencionesBlanco;
            bool flagSello = sTieneSelloSemanal == ConstantesAppServicio.SI;

            intServicio.GenerarInformeWord(progrCodi, tipo, path, pathLogo, pathLogoIntervenciones, flagSello, pathFirmaIntervenciones, pathFirmaIntervencionesBlanco, out string file);

            return Json(file);
        }

        #endregion

        #region Informe Excel

        /// <summary>
        /// Generar reporte en formato Excel Mantto de la programación seleccionada.
        /// </summary>
        /// <param name="progrCodi">Id de Programación</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerarExcelReporteIntervencionesProcesadas(int progrCodi)
        {
            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            intServicio.GenerarInformeAgenteExcelXPrograma(progrCodi, pathDestino, out string fileName);

            return Json(fileName);
        }

        [HttpPost]
        public JsonResult GenerarExcelIntervencionesOsinergmin(int progrCodi)
        {
            string pathDestino = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            intServicio.GenerarInformeOsinergminExcelXPrograma(progrCodi, pathDestino, out string fileName);

            return Json(fileName);
        }

        #endregion

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
            model.Mes = DateTime.Today.AddMonths(3).ToString(ConstantesAppServicio.FormatoMes);

            model.Anho = DateTime.Today.AddYears(2).Year.ToString();

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
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

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
                int progrcodi = intServicio.GuardarInProgramacion(idprogramacion, fechaInput, base.UserName);

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

        #endregion

        #region Ampliación de plazo

        /// <summary>
        /// metodo para llenar poppup
        /// </summary>
        /// <param name="idprogramacion"></param>
        /// <param name="tipoProgra"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistorico(int idprogramacion)
        {
            base.ValidarSesionJsonResult();

            Intervencion model = new Intervencion
            {
                EntidadProgramacion = intServicio.ObtenerProgramacionesPorId(idprogramacion)
            };
            model.FechProgramacion = model.EntidadProgramacion.Progrfechalim.ToString(ConstantesAppServicio.FormatoFechaFull2);
            model.NombPrograDetallado = model.EntidadProgramacion.ProgrnombYPlazo;

            model.EntidadProgramacion.Ampliaciones = intServicio.ListInParametroplazos().Where(x => x.Progrcodi == idprogramacion).OrderByDescending(m => m.Parplafeccreacion).ToList();
            if (model.EntidadProgramacion.Ampliaciones.Count > 0)
                model.ULtimaAmpliacion = model.EntidadProgramacion.Ampliaciones.OrderByDescending(x => x.Parplafeccreacion).First().Parplafechasta.ToString(ConstantesAppServicio.FormatoFechaFull2);

            return PartialView(model);
        }

        /// <summary>
        /// Guardar Ampliación de la programación
        /// </summary>
        /// <param name="idprogramacion"></param>
        /// <param name="strfechaInicio"></param>
        /// <param name="strfechaFin"></param>
        /// <param name="hora"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProgrAmpliacionGuardar(int idprogramacion, string strfechaInicio, string strfechaFin, string hora, string minuto, string desc)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();


                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaIni = DateTime.ParseExact(strfechaInicio, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                var fechaFin = new DateTime();
                if (hora == "24" && minuto == "00")
                {
                    var fecha = DateTime.ParseExact(strfechaFin.Trim() + " " + "00:00:00", ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                    fechaFin = fecha.AddDays(1);
                }
                else
                {
                    fechaFin = DateTime.ParseExact(strfechaFin.Trim() + " " + hora + ":" + minuto + ":00", ConstantesAppServicio.FormatoFechaFull2, CultureInfo.InvariantCulture);
                }

                string fecHora = hora + ":" + minuto + ":00";

                if (!base.IsValidSesion) throw new Exception(Constantes.MensajeSesionExpirado);

                //Guardar
                InParametroPlazoDTO inParPlazo = new InParametroPlazoDTO
                {
                    Progrcodi = idprogramacion,
                    Parpladesc = desc,
                    Parplafecdesde = fechaIni,
                    Parplafechasta = fechaFin,
                    Parplahora = fecHora,
                    Parplasucreacion = base.UserName,
                    Parplafeccreacion = DateTime.Now
                };

                intServicio.SaveInParametroplazo(inParPlazo);

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
        /// Carga lista de Semanas
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemana(int idAnho, string tipoSem)
        {
            Intervencion model = new Intervencion
            {
                ListaSemanas = ListarSemanasByAnio(idAnho),
                TipoSemana = tipoSem
            };

            return PartialView(model);
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

        #region Cargar Informe a Portal

        /// <summary>
        /// Verificar la existencia de archivos
        /// </summary>
        /// <param name="progrcodi"></param>
        /// <param name="tipoInforme"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarFileInformeAPortal(int progrcodi, int tipoInforme)
        {
            Intervencion model = new Intervencion();

            try
            {
                bool flagExiste = intServicio.VerificarExisteFileInformePortal(progrcodi, tipoInforme);

                model.TieneArchivos = flagExiste;
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
        /// Subir informes zipeados a portal
        /// </summary>
        /// <param name="progrcodi"></param>
        /// <param name="tipoInforme"></param>
        /// <param name="sTieneSelloSemanal"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SubirInformeAPortal(int progrcodi, int tipoInforme, string sTieneSelloSemanal)
        {
            Intervencion model = new Intervencion();

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string pathLogoIntervenciones = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogoIntervenciones;
                string pathFirmaIntervenciones = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathFirmaIntervenciones;
                string pathFirmaIntervencionesBlanco = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathFirmaIntervencionesBlanco;
                bool flagSello = sTieneSelloSemanal == ConstantesAppServicio.SI;

                intServicio.SubirInformeAPortal(progrcodi, tipoInforme, path, pathLogo, pathLogoIntervenciones, flagSello, pathFirmaIntervenciones, pathFirmaIntervencionesBlanco, base.UserName);

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

        #endregion

        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesIntervencionesAppServicio.ModuloManualUsuarioNuevo;
            string nombreArchivo = ConstantesIntervencionesAppServicio.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesIntervencionesAppServicio.FolderRaizIntervenciones;
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

        #region Version Factores

        public JsonResult GuardarNuevaVersion(string fechaPeriodo, int horizonte)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                this.ValidarSesionJsonResult();

                // Validación
                DateTime dFechaFinData = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                int vercodi = intServicio.CrearVersionFactorF1F2SPR(dFechaFinData, base.UserName, horizonte);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = ConstantesAppServicio.ParametroDefecto;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        #endregion

        #endregion

        /// <summary>
        /// Carga el formulario principal de operaciones con intervenciones al haber seleccionado la programación.
        /// </summary>
        /// <param name="progCodi"></param>
        /// <returns></returns>
        public ActionResult IntervencionesRegistro(int progCodi = 0)
        {
            Intervencion model = new Intervencion();

            InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(progCodi);

            model.IdProgramacion = progCodi;
            model.IdTipoProgramacion = regProg.Evenclasecodi;
            model.NombreProgramacion = regProg.Nomprogramacion;
            model.Evenclasedesc = regProg.Evenclasedesc;
            model.Progrfechaini = regProg.Progrfechaini.ToString(ConstantesAppServicio.FormatoFecha);
            model.Progrfechafin = regProg.Progrfechafin.ToString(ConstantesAppServicio.FormatoFecha);
            model.EstadoProgramacion = regProg.EstadoIntranet;
            model.EstadoProgramacionDesc = regProg.EstadoIntranetDesc.ToUpper();
            model.EsCerrado = regProg.EsCerradoIntranet;
            model.PermiteReversion = regProg.PermiteReversion && regProg.Progrfechabrev == null;
            model.EsRevertido = regProg.EsPlanRevertido;
            if (regProg.EsPlanRevertido)
                model.FechaPlazo = regProg.Progrmaxfecreversion.Value.ToString(ConstantesAppServicio.FormatoFechaFull2);

            model.ListaCboEmpresa = intServicio.ListarComboEmpresas();
            model.ListaFamilias = intServicio.ListarComboFamilias();
            model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);
            model.ListaProgramaciones = intServicio.ListarProgramacionesRegistro(regProg.Evenclasecodi);
            model.ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

            string sIdsEmpresas = ConstantesIntervencionesAppServicio.FiltroEmpresaTodos;
            model.ListaCboUbicacion = intServicio.ListarComboUbicacionesXEmpresa(sIdsEmpresas);

            if (regProg.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoSemanal
                || regProg.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario
                || regProg.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado)
            {
                model.TienePermisoEjecManual = intServicio.EnviarNotificacionManual();
            }

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
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                List<int> listaEmprcodiLectura = new List<int>() { 1 };

                model.Progrcodi = objFiltro.Progrcodi;
                model.IdTipoProgramacion = objFiltro.Evenclasecodi;

                //validar si es está cerrado o abierto
                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(objFiltro.ProgrcodiReal);
                model.EsCerrado = regProg.EsCerradoIntranet;
                model.EsRevertido = regProg.EsPlanRevertido;

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                if (listaIntervenciones.Count > 5000) throw new ArgumentException("El listado contiene más de 3000 registros. Utilice filtros para reducir la cantidad de registros de la vista web.");

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = objFiltro.ListaIntercodiSel;

                //Verificación de mensajes enviados
                foreach (var obj in listaIntervenciones)
                {
                    obj.TipoComunicacion = obj.Intermensajecoes;
                    if (obj.Intermensajecoes == NotificacionAplicativo.TipoComunicacionNinguna && obj.Intermensajeagente != NotificacionAplicativo.TipoComunicacionNinguna)
                        obj.TipoComunicacion = NotificacionAplicativo.TipoComunicacionExisteMensaje;
                    obj.ChkMensaje = obj.Intermensaje == ConstantesAppServicio.SI;
                    obj.ChkAprobacion = false;//listaIntercodiChecked.Contains(obj.Intercodi);
                }

                //Verificacion si tiene archivos
                model.TieneArchivos = listaIntervenciones.Find(x => x.Interisfiles.Trim() == "S") != null;

                if (ConstantesIntervencionesAppServicio.TipoProgramacionProgramadoDiario == regProg.Evenclasecodi)
                {
                    if (listaIntervenciones.Any()) listaIntervenciones = intServicio.BuscarAlertaIntervencionProgramada(listaIntervenciones);

                    model.TieneAlertaNoEjecutado = listaIntervenciones.Find(x => x.AlertaNoEjecutado == 1) != null;
                    model.TieneAlertaEstadoPendiente = listaIntervenciones.Find(x => x.TieneAlertaEstadoPendiente) != null;
                    model.PintarAlerta += (model.TieneAlertaNoEjecutado ? 1 : 0);
                    model.PintarAlerta += (model.TieneAlertaEstadoPendiente ? 1 : 0);
                }

                if (ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado == regProg.Evenclasecodi)
                {
                    //alerta creacion/modificación de intervenciones por parte del agente agente
                    model.ListaIntervCount = listaIntervenciones.Where(x => x.Interfuentestado >= 5 && x.Interdeleted != 1).ToList().Count;

                    //Validaciones con otros aplicativos
                    if (int.Parse(objInput.TieneValidaciones) == 1)
                    {
                        //Alerta de Intervenciones
                        DateTime fechaPeriodo = objFiltro.FechaIni.Date;
                        List<MeMedicion96DTO> listaDatosRPFWs = new List<MeMedicion96DTO>();
                        bool esValidablePR21 = this.intServicio.EsValidableSegunPlazo(ConstantesHorasOperacion.IdFuenteDatoPR21, fechaPeriodo);
                        if (ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado == objFiltro.Evenclasecodi && esValidablePR21)
                        {
                            List<int> listaEquicodi = listaIntervenciones.Select(x => x.Equicodi).Where(x => x > 0).Distinct().ToList();
                            listaDatosRPFWs = this.ListarDatosRPFByWebService(listaEquicodi, fechaPeriodo);
                        }
                        this.intServicio.AlertarIntervencionesEjecutado(listaIntervenciones, objFiltro.Evenclasecodi, fechaPeriodo, listaDatosRPFWs);

                        model.TieneAlertaHoraOperacion = listaIntervenciones.Find(x => x.TieneAlertaHoraOperacion) != null;
                        model.TieneAlertaScada = listaIntervenciones.Find(x => x.TieneAlertaScada) != null;
                        model.TieneAlertaEms = listaIntervenciones.Find(x => x.TieneAlertaEms) != null;
                        model.TieneAlertaIDCC = listaIntervenciones.Find(x => x.TieneAlertaIDCC) != null;
                        model.TieneAlertaPR21 = listaIntervenciones.Find(x => x.TieneAlertaPR21) != null;
                        model.TieneAlertaMedidores = listaIntervenciones.Find(x => x.TieneAlertaMedidores) != null;

                        model.PintarAlerta += (model.TieneAlertaHoraOperacion ? 1 : 0);
                        model.PintarAlerta += (model.TieneAlertaScada || model.TieneAlertaEms || model.TieneAlertaIDCC ? 1 : 0);
                        model.PintarAlerta += (model.TieneAlertaPR21 ? 1 : 0);
                        model.PintarAlerta += (model.TieneAlertaMedidores ? 1 : 0);
                    }
                }

                //generar objeto para la vista
                model.ListaFilaWeb = listaIntervenciones.Select(x => new IntervencionFila()
                {
                    Intercodi = x.Intercodi,
                    Progrcodi = x.Progrcodi,

                    Equicodi = x.Equicodi,
                    EstadoRegistro = x.EstadoRegistro,
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
                    Interisfiles = x.Interisfiles,
                    Internota = x.Internota,
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
                    Interprocesado = x.Interprocesado,

                    EstaFraccionado = x.EsContinuoFraccionado,
                    EsConsecutivoRangoHora = x.EsConsecutivoRangoHora,

                    TieneAlertaHoraOperacion = x.TieneAlertaHoraOperacion,
                    TieneAlertaScada = x.TieneAlertaScada,
                    TieneAlertaEms = x.TieneAlertaEms,
                    TieneAlertaIDCC = x.TieneAlertaIDCC,
                    TieneAlertaPR21 = x.TieneAlertaPR21,
                    TieneAlertaMedidores = x.TieneAlertaMedidores,
                    AlertaNoEjecutado = x.AlertaNoEjecutado,
                    TieneAlertaEstadoPendiente = x.TieneAlertaEstadoPendiente,
                    IniFecha = x.Interfechaini.ToString(ConstantesAppServicio.FormatoFecha),
                    //IniHora = x.Interfechaini.ToString("HH"),
                    //IniMinuto = x.Interfechaini.ToString("mm"),
                    FinFecha = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFecha),
                    //FinHora = x.Interfechafin.ToString("HH"),
                    //FinMinuto = x.Interfechafin.ToString("mm"),
                    IniHoraMinuto = x.Interfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora),
                    FinHoraMinuto = x.Interfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora)
                }).ToList();

                // Lista Empresas sin abreviatura
                var lstSinAbreviatura = model.ListaFilaWeb.Where(x => x.EmprAbrev == null || x.EmprAbrev.Trim() == string.Empty).ToList();
                model.ListaEmpresasValidate = lstSinAbreviatura.GroupBy(x => x.Emprcodi).Select(x => new IntervencionFila() // quitar duplicados de empresa
                {
                    Emprcodi = x.Key,
                    EmprNomb = (x.First().EmprNomb ?? "").Trim(),
                    EmprAbrev = (x.First().EmprAbrev ?? "").Trim()
                }).OrderBy(x => x.EmprNomb).ThenBy(x => x.EmprAbrev).ToList();

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

        #region Alerta de Validación con otros Aplicativos (Horas de Operación, Scada, EMS, IDCC, PR21, Medidores)

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaHOPXIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                model.ListaValidacionHorasOperacion = this.intServicio.ListarAlertaHoraOperacionEquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString());
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

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaScadaXIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                model.ListaValidacionScada = this.intServicio.ListarAlertaScadaEquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString());
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

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaEmsXIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                model.ListaValidacionEms = this.intServicio.ListarAlertaEmsEquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString());
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

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaIDCCXIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                model.ListaValidacionIDCC = this.intServicio.ListarAlertaIDCCEquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString());
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

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaPR21XIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                List<MeMedicion96DTO> listaRpfWs = this.ListarDatosRPFByWebService(new List<int> { model.Entidad.Equicodi }, model.Entidad.Interfechaini.Date);
                model.ListaValidacionPR21 = this.intServicio.ListarAlertaPR21EquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString(), listaRpfWs);
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

        /// <summary>
        /// Interfaz para mostrar la alerta hora operación por Intervención
        /// </summary>
        /// <param name="sfecha"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="hopcodipadre"></param>
        /// <returns></returns>
        public PartialViewResult VerAlertaMedidoresXIntervencion(int intercodi)
        {
            Intervencion model = new Intervencion();
            try
            {
                if (!base.IsValidSesionView()) throw new Exception(Constantes.MensajeSesionExpirado);

                model.Entidad = intServicio.GetByIdInIntervencion(intercodi);
                model.ListaValidacionMedidores = this.intServicio.ListarAlertaMedidoresEquipoManttoByListaInter(model.Entidad.Interfechaini.Date, intercodi.ToString());
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

        #region Obtener datos PR21

        /// <summary>
        /// Obtener datos RPF por Web service
        /// </summary>
        /// <param name="equicodis"></param>
        /// <param name="fechaPeriodo"></param>
        /// <returns></returns>
        private List<MeMedicion96DTO> ListarDatosRPFByWebService(List<int> equicodis, DateTime fechaPeriodo)
        {
            List<MePtorelacionDTO> listaConfPto = (new FormatoMedicionAppServicio()).ListPuntosRPFByEquipo(equicodis, fechaPeriodo);

            List<MeMedicion96DTO> listaDataFinal = new List<MeMedicion96DTO>();
            foreach (var regConfPto in listaConfPto)
            {
                List<ServicioCloud.Medicion>  listRPF = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRangoResolucion(fechaPeriodo, regConfPto.Ptomedicodi.ToString(), ParametrosFormato.ResolucionCuartoHora).ToList();

                if (listRPF.Any() && listRPF.Count() == 96)
                {
                    MeMedicion96DTO reg = new MeMedicion96DTO
                    {
                        Medifecha = fechaPeriodo,
                        Ptomedicodi = regConfPto.Ptomedicodi.GetValueOrDefault(-1),
                        Equicodi = regConfPto.Equicodi,
                        Tipoinfocodi = ConstantesMedicion.TipoPotenciaActiva
                    };

                    for (int i = 1; i <= 96; i++)
                    {
                        decimal? valor1 = 0;

                        if (listRPF.Count == 96)
                        {
                            valor1 = listRPF[i - 1].H0;
                        }

                        reg.GetType().GetProperty(ConstantesAppServicio.CaracterH + i.ToString()).SetValue(reg, valor1);
                    }

                    listaDataFinal.Add(reg);
                }
                else
                {
                    if (ConstantesIntervencionesAppServicio.FlagCompletarDatosAplicativos)
                    {
                        MeMedicion96DTO reg = new MeMedicion96DTO
                        {
                            Medifecha = fechaPeriodo,
                            Ptomedicodi = regConfPto.Ptomedicodi.GetValueOrDefault(-1),
                            Equicodi = regConfPto.Equicodi,
                            Tipoinfocodi = ConstantesMedicion.TipoPotenciaActiva
                        };

                        listaDataFinal.Add(reg);
                    }
                }
            }

            return listaDataFinal;
        }

        #endregion

        #region Actualizar Abreviatura empresas

        [HttpPost]
        public JsonResult ActualizarAbreviaturaEmpresa(int emprcodi, string emprabrev, int progrcodi)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrcodi);
                SiEmpresaDTO empresaAntes = intServicio.GetByIdSiEmpresa(emprcodi);

                // validar duplicados de abreviatura
                var esUnico = intServicio.ActualizarAbreviaturaEmpresa(emprcodi, emprabrev);
                if (!esUnico)
                {
                    model.Resultado = "0";
                    model.Mensaje = "Ya existe la abreviatura para otra empresa. No se permite el duplicado.";
                }
                else
                {
                    model.Resultado = "1";
                    model.Mensaje = "Se actualizó satisfactoriamente el registro";

                    //valores para notificación
                    INNotificacionEmpresa empresaCambio = new INNotificacionEmpresa
                    {
                        Emprcodi = empresaAntes.Emprcodi,
                        Emprnomb = empresaAntes.Emprnomb,
                        Emprabrev = empresaAntes.Emprabrev ?? "",
                        EmprabrevNew = emprabrev.Trim(),
                        Emprfecmodificacion = DateTime.Now,
                        Emprusumodificacion = base.UserName
                    };

                    List<string> listaCorreoModulEmp = ObtenerListaCorreosAdminEmpresa();

                    //Enviar Notificación de asignación de abreviatura
                    intServicio.EnviarNotificacionAbreviaturaEmpresa(empresaCambio, regProg.Evenclasecodi, base.UserEmail, listaCorreoModulEmp);
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
        /// Obtener Lista de correos con rol de administrador de módulo empresa
        /// </summary>
        /// <returns></returns>
        private List<string> ObtenerListaCorreosAdminEmpresa()
        {
            List<string> lstCorreos = new List<string>();

            var listaUsurios = servSeguridad.ObtenerUsuariosPorRol(ConstantesIntervencionesAppServicio.RolAdministradorEmpresa);

            foreach (var item in listaUsurios)
            {
                lstCorreos.Add(item.UserEmail);
            }

            lstCorreos = lstCorreos.Where(x => x.Contains("@coes")).ToList();

            return lstCorreos;
        }

        #endregion

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
        public JsonResult ObtenerIntervencion(int interCodi, int progrCodi, int tipoProgramacion, string strFechaCol,
            bool escruzadas = false, bool tieneCeldaSelec = true, int equicodi = 0, int lecturaF1F2 = 0)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                if (tipoProgramacion <= 0 || tipoProgramacion > 5) tipoProgramacion = 3; //por defecto el semanal
                model.ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioMantenimiento);
                model.ListaClaseProgramacion = intServicio.ListarComboClasesProgramacion();
                model.Entidad = new InIntervencionDTO();

                InProgramacionDTO regProg = null;

                if (interCodi == 0)
                {
                    model.AccionNuevo = true;
                    model.sIdsEmpresas = "-1";

                    model.Entidad = intServicio.ObtenerIntervencionWeb(interCodi, equicodi);
                    model.ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta);

                    if (strFechaCol != null)
                    {
                        DateTime fechaDestino = DateTime.ParseExact(strFechaCol, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        var listaProgramaciones = intServicio.ListarProgramacionesRegistro(tipoProgramacion);
                        regProg = intServicio.GetProgramacionDefecto(tipoProgramacion, listaProgramaciones, fechaDestino, true);

                        if (regProg != null)
                        {
                            model.Entidad.Interfechaini = fechaDestino;
                            model.Entidad.Interfechafin = fechaDestino.AddDays(1);
                        }
                    }
                    if (regProg == null)
                    {
                        regProg = intServicio.ObtenerProgramacionesPorId(progrCodi);
                        model.Entidad.Interfechaini = regProg.Progrfechaini;
                        model.Entidad.Interfechafin = regProg.Progrfechafin.AddDays(1);
                    }
                    model.ListaProgramaciones = intServicio.ListarProgramacionesRegistro(regProg.Evenclasecodi);

                    //validar si aún sigue abierto o ya está cerrado
                    model.EsCerrado = regProg.EsCerradoIntranet;
                    model.EsRevertido = regProg.EsPlanRevertido;
                    model.IdTipoProgramacion = regProg.Evenclasecodi;
                    model.Evenclasedesc = regProg.Evenclasedesc;
                    model.NombreProgramacion = regProg.ProgrnombYPlazoCruzado;


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
                    model.EsCerrado = regProg.EsCerradoIntranet;
                    if (!model.EsCerrado) model.AccionEditar = true;
                    model.EsRevertido = regProg.EsPlanRevertido;
                    if (model.EsRevertido) model.AccionEditar = true;
                    model.IdTipoProgramacion = regProg.Evenclasecodi;
                    model.Evenclasedesc = regProg.Evenclasedesc;
                    model.NombreProgramacion = regProg.ProgrnombYPlazoCruzado;

                    //combos
                    model.ListaCausas = intServicio.ListarComboSubCausas((int)model.Entidad.Tipoevencodi);

                    model.Entidad.Interjustifaprobrechaz = model.Entidad.Interjustifaprobrechaz == "COPIA" || model.Entidad.Interjustifaprobrechaz == "IMPORTACION" ? "" : model.Entidad.Interjustifaprobrechaz;

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

                model.TienePermisoSPR = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                if (model.Entidad.Estadocodi == ConstantesIntervencionesAppServicio.InEstadoAprobado && !model.EsRevertido)
                {
                    model.EsDesabilitado = true; //Desabilita los inputs
                }
                else
                {
                    model.AccionEditar = model.AccionEditar && model.TienePermisoSPR;
                }
                if (!model.TienePermisoSPR)
                    model.EsDesabilitado = true; //Desabilita los inputs

                model.IdIntervencion = interCodi;
                model.EsCruzadas = escruzadas;
                model.TieneCeldaSelec = tieneCeldaSelec;
                model.IdProgramacion = progrCodi;

                //fecha hoy (validacion ejecutados)
                model.FechaHoy = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha);

                if (lecturaF1F2 == 1)
                {
                    model.AccionNuevo = false;
                    model.AccionEditar = false;
                    model.EsDesabilitado = true;
                }

                //si está eliminado
                if (model.Entidad.Interdeleted == ConstantesIntervencionesAppServicio.iSi)
                {
                    model.AccionEditar = false;
                    model.EsCerrado = true;
                    model.EsDesabilitado = true; // Desabilita los inputs
                }

                model.Entidad.EsEditable = model.AccionNuevo || model.AccionEditar;

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
        public JsonResult IntervencionGuardar(string dataJson, int opcion, int envioconfirmado)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                model.Entidad = new InIntervencionDTO();
                string usuarioModificacion = base.UserName;
                DateTime fechaModificacion = DateTime.Now;

                int idIntervencion = 0;
                int idCarpetaFisica = 0;
                if (opcion == ConstantesIntervencionesAppServicio.AccionNuevo)
                {
                    model.Entidad = serializer.Deserialize<InIntervencionDTO>(dataJson);
                    idCarpetaFisica = model.Entidad.Intercarpetafiles;

                    //valida si el programa está cerrado
                    InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(model.Entidad.Progrcodi);
                    model.Entidad.Evenclasecodi = regProg.Evenclasecodi;
                    if (regProg.Progrsololectura == 1 && !regProg.EsPlanRevertido)
                    {
                        throw new Exception("El Programa ya se encuentra cerrado");
                    }

                    model.Entidad.Intermensaje = ConstantesIntervencionesAppServicio.sNo;
                    model.Entidad.Intermensajecoes = NotificacionAplicativo.TipoComunicacionNinguna;
                    model.Entidad.Intermensajeagente = NotificacionAplicativo.TipoComunicacionNinguna;
                    model.Entidad.Interfechapreini = null;
                    model.Entidad.Interfechaprefin = null;
                    model.Entidad.Interrepetir = ConstantesIntervencionesAppServicio.sNo;

                    model.Entidad.Interjustifaprobrechaz = null;
                    model.Entidad.Interfecaprobrechaz = null;
                    model.Entidad.Interisfiles = model.Entidad.ListaArchivo != null && model.Entidad.ListaArchivo.Any() ? ConstantesAppServicio.SI : ConstantesAppServicio.NO;
                    model.Entidad.CarpetaInTemporal = idCarpetaFisica;
                    model.Entidad.Intermanttocodi = null;
                    model.Entidad.Interevenpadre = null;
                    model.Entidad.Actividad = "Se ha registrado una intervencion";

                    // NO TIENE CODIGO DE REGISTRO PADRE POR SER OPERACIÓN NUEVO
                    model.Entidad.Intercodipadre = null;

                    model.Entidad.Interregprevactivo = ConstantesIntervencionesAppServicio.sSi;
                    model.Entidad.Estadocodi = ConstantesIntervencionesAppServicio.InEstadoPendienteEnProcesoEvaluacion;
                    model.Entidad.Interprocesado = ConstantesIntervencionesAppServicio.iNo;
                    model.Entidad.Interdeleted = ConstantesIntervencionesAppServicio.iNo;
                    model.Entidad.Intercreated = ConstantesIntervencionesAppServicio.iSi;
                    if (model.Entidad.Sustento != null) model.Entidad.Interflagsustento = ConstantesIntervencionesAppServicio.FlagTieneSustento;

                    model.Entidad.Interusucreacion = usuarioModificacion;
                    model.Entidad.Interfeccreacion = fechaModificacion;
                    model.Entidad.Interusumodificacion = null;
                    model.Entidad.Interfecmodificacion = null;

                    if (regProg.EsCerradoIntranet && regProg.EsPlanRevertido)  //para registro y cruzadas
                    {
                        model.Entidad.Estadocodi = ConstantesIntervencionesAppServicio.InEstadoAprobado;
                        model.Entidad.Interprocesado = ConstantesIntervencionesAppServicio.FlagProcesadoReversion;
                    }
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>Validar Intervencion servicio>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var resultado1 = intServicio.ValidarIntervencionWeb(model.Entidad);

                    //Validar con Intervenciones
                    if (model.Entidad.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado &&
                        envioconfirmado != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                    {
                        List<ResultadoValidacionAplicativo> listaValInterv = this.intServicio.ListarAlertaHoraOperacionEquipoManttoRegistroInter(model.Entidad);
                        if (listaValInterv.Count() > 0)
                        {
                            model.ListaValidacionHorasOperacion = listaValInterv;
                            model.Resultado = "2";
                            return Json(model);
                        }
                    }

                    // Graba los cambio en la BD
                    intServicio.CrudListaIntervencion(new List<InIntervencionDTO>() { model.Entidad }, null, null, usuarioModificacion);
                    idIntervencion = model.Entidad.Intercodi;
                    model.IdIntervencion = idIntervencion;
                }
                else
                {
                    var intervencionEdit = serializer.Deserialize<InIntervencionDTO>(dataJson);
                    model.Entidad = intServicio.GetByIdInIntervencionYSustento(intervencionEdit.Intercodi);
                    model.Entidad.ListaArchivo = intervencionEdit.ListaArchivo ?? new List<InArchivoDTO>();

                    //si se editó cambiando el equipo de la intervención o tienen sustento de exclusión
                    bool flagEsEliminable = intervencionEdit.Equicodi != model.Entidad.Equicodi;
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>Validar Intervencion servicio>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var entidadConFechas = intServicio.ValidarIntervencionWeb(intervencionEdit);

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
                    model.Entidad.Internota = intervencionEdit.Internota;
                    model.Entidad.Estadocodi = intervencionEdit.Estadocodi;
                    model.Entidad.Interprocesado = intervencionEdit.Interprocesado;

                    //radio
                    model.Entidad.Interindispo = intervencionEdit.Interindispo;
                    model.Entidad.Estadocodi = intervencionEdit.Estadocodi;
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
                    model.Entidad.Intercodipadre = model.Entidad.Intercodi;
                    model.Entidad.Interisfiles = model.Entidad.ListaArchivo.Any() || model.Entidad.Interflagsustento == ConstantesIntervencionesAppServicio.FlagTieneSustento ? ConstantesAppServicio.SI : ConstantesAppServicio.NO;

                    model.Entidad.Interusumodificacion = usuarioModificacion;
                    model.Entidad.Interfecmodificacion = fechaModificacion;

                    InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorId(model.Entidad.Progrcodi);
                    if (regProg.EsCerradoIntranet && regProg.EsPlanRevertido)  //para registro y cruzadas
                    {
                        model.Entidad.Estadocodi = ConstantesIntervencionesAppServicio.InEstadoAprobado;
                        model.Entidad.Interprocesado = ConstantesIntervencionesAppServicio.FlagProcesadoReversion;
                    }

                    model.Entidad.Actividad = "Se ha actualizado intervencion";

                    model.Detalle = ((DateTime)model.Entidad.Interfeccreacion).ToString("dd/MM/yyyy HH:mm:ss");

                    //Validar con Intervenciones
                    if (model.Entidad.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado &&
                        envioconfirmado != ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                    {
                        List<ResultadoValidacionAplicativo> listaValInterv = this.intServicio.ListarAlertaHoraOperacionEquipoManttoRegistroInter(model.Entidad);
                        if (listaValInterv.Count() > 0)
                        {
                            model.ListaValidacionHorasOperacion = listaValInterv;
                            model.Resultado = "2";
                            return Json(model);
                        }
                    }

                    // Graba los cambio en la BD
                    if (flagEsEliminable)
                    {
                        //el registro que está en el formulario web se guardará como nuevo
                        model.Entidad.Intercodi = 0;
                        model.Entidad.Intercodipadre = null;
                        //model.Entidad.Interjustifaprobrechaz = null;
                        model.Entidad.EsCopiarArchivo = true; //copiar los archivos de sustento al destino
                        model.Entidad.CarpetaProgOrigenFS = regProg.CarpetaProgDefault;
                        model.Entidad.CarpetaProgDestinoFS = regProg.CarpetaProgDefault;
                        model.Entidad.CarpetafilesOrigenFS = model.Entidad.Intercarpetafiles;

                        //el registro histórico se guadará como eliminado
                        var regEliminar = intServicio.GetByIdInIntervencionYSustento(intervencionEdit.Intercodi);

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

                // Enviar notificación Intervenciones
                if (model.Entidad.Evenclasecodi == ConstantesIntervencionesAppServicio.TipoProgramacionEjecutado && envioconfirmado ==
                    ConstantesHorasOperacion.FlagConfirmarValIntervenciones)
                {
                    List<ResultadoValidacionAplicativo> listaValInterv = this.intServicio.ListarAlertaHoraOperacionEquipoManttoRegistroInter(model.Entidad);
                    if (listaValInterv.Count() > 0)
                    {
                        InProgramacionDTO programa = intServicio.ObtenerProgramacionesPorIdSinPlazo(model.Entidad.Progrcodi);

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

        /// <summary>
        /// Valida codigo de Seguimiento
        /// </summary>
        /// <param name="codigoSeguimiento">Código de seguimiento</param>
        /// <param name="equiCodi">Id de Equipo</param>       
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult ValidarCodigoSeguimiento(string codigoSeguimiento, int equiCodi)
        {
            base.ValidarSesionJsonResult();

            bool esValido = intServicio.ValidarCodigoSeguimiento(codigoSeguimiento, equiCodi);
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
            Intervencion model = new Intervencion
            {
                ListaCboEmpresa = intServicio.ListarComboEmpresasActivas(),
                ListaFamilia = this.servIeod.ListarFamilia(),
                FiltroFamilia = filtroFamilia.Value
            };

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
            var listaLinea = this.servEvento.BuscarEquipoEventoIntervenciones(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

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
            Intervencion model = new Intervencion
            {
                IndicadorPagina = false
            };
            int nroRegistros = this.servEvento.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

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

        /// <summary>
        /// Método para guardar edición de fechas desde el listado de Intervenciones ejecutadas
        /// </summary>
        /// <param name="arrayFilasEditadas"></param>
        /// <returns></returns>
        public JsonResult GuardarIntervEjecutadasEdicionFechas(string arrayFilasEditadas)
        {
            Intervencion model = new Intervencion
            {
                TienePermisoSPR = true
            };

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<InIntervencionDTO> listaEdicionFechasEjecutadas = serializer.Deserialize<List<InIntervencionDTO>>(arrayFilasEditadas);

                string usuarioModificacion = base.UserName;
                DateTime fechaModificacion = DateTime.Now;
                int idIntervencion = 0;

                //No es necesario validar con otros aplicativos
                //No es necesario mover los archivos, solo se modifican las fechas de la intervención
                foreach (var intervencionEdit in listaEdicionFechasEjecutadas)
                {
                    //Guardar las intervenciones
                    model.Entidad = intServicio.GetByIdInIntervencionYSustento(intervencionEdit.Intercodi);
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>Validar Intervencion servicio>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    var entidadConFechas = intServicio.ValidarIntervencionWeb(intervencionEdit);

                    //capturar valores
                    model.Entidad.Interfechaini = entidadConFechas.Interfechaini;
                    model.Entidad.Interfechafin = entidadConFechas.Interfechafin;
                    model.Entidad.Interusumodificacion = usuarioModificacion;
                    model.Entidad.Interfecmodificacion = fechaModificacion;
                    model.Entidad.Actividad = "Se ha actualizado intervencion";

                    // Graba los cambio en la BD
                    intServicio.CrudListaIntervencion(null, new List<InIntervencionDTO>() { model.Entidad }, null, usuarioModificacion);
                    idIntervencion = model.Entidad.Intercodi;
                    model.IdIntervencion = idIntervencion;
                }

                model.StrMensaje = "¡La Información se actualizó correctamente!";
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

        #endregion

        #region 1.3 - ELIMINAR

        /// GET: Intervenciones/Registro/
        /// <summary>
        /// Eliminar registros seleccionados
        /// </summary>
        /// <param name="tipo">Tipo de acción</param>       
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult EliminarSeleccionados(string tipo)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //primero se valida si las intervenciones pueden ser eliminadas
                intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteIntranet, listaIntercodiChecked,
                                                out List<InIntervencionDTO> listaEliminable, out List<InIntervencionDTO> listaWarning);

                //solo eliminar las no aprobadas
                intServicio.CrudListaIntervencion(null, null, listaEliminable, base.UserName);

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
                List<int> listaEmprcodiLectura = new List<int>() { 1 };

                model.ListaMensajes = intServicio.ListSiMensajesXIntervencion(interCodi, ConstantesIntervencionesAppServicio.AmbienteIntranet,
                                                            listaEmprcodiLectura, tipoRemitente, estadoMensaje);
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

                int emprcodiLectura = 1;
                intServicio.MarcarMensajeComoLeido(interCodi, msgcodi, ConstantesIntervencionesAppServicio.AmbienteIntranet, emprcodiLectura, base.UserName, base.UserEmail, -1);
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
                List<int> listaEmprcodiLectura = new List<int>() { 1 };

                var entidad = intServicio.GetByIdInIntervencion(interCodi);
                InProgramacionDTO objProgramacion = intServicio.ObtenerProgramacionesPorIdSinPlazo(entidad.Progrcodi);

                List<SiMensajeDTO> listaMensaje = intServicio.ListSiMensajesXIntervencion(interCodi, ConstantesIntervencionesAppServicio.AmbienteIntranet, listaEmprcodiLectura, "-1", "-1").Where(x => x.Msgflagadj == 1).ToList();
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
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            string html = intServicio.GenerarPDFMensajes(ConstantesIntervencionesAppServicio.AmbienteIntranet, interCodi, new List<int>() { 1 }, out string filename);

            UtilDevExpressIntervenciones.GenerarPDFdeHtml(html, path, filename);

            return base.DescargarArchivoTemporalYEliminarlo(path, filename);
        }

        [HttpPost]
        public PartialViewResult ListadoModificaciones(int interCodi)
        {
            base.ValidarSesionJsonResult();

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
            base.ValidarSesionJsonResult();

            Intervencion model = new Intervencion();

            var entidad = intServicio.GetByIdInIntervencion(interCodi);

            model.IdIntervencion = interCodi;
            model.IdTipoProgramacion = entidad.Evenclasecodi;
            model.ListaIntervenciones = intServicio.ObtenerTrazabilidad(interCodi, entidad.Evenclasecodi, entidad.Intercodsegempr);
            model.ListaMensajes = new List<SiMensajeDTO>();

            foreach (var regInter in model.ListaIntervenciones)
            {
                List<SiMensajeDTO> listaMensajes = intServicio.ListSiMensajesXIntervencion(regInter.Intercodi, ConstantesIntervencionesAppServicio.AmbienteIntranet, new List<int>(), "-1", "-1");
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

        /// <summary>
        /// validar sustento desde intervenciones cruzadas
        /// </summary>
        /// <param name="seleccion"></param>
        /// <param name="progrcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarDescargaSustento(string seleccion)
        {
            IntervencionResultado model = new IntervencionResultado();
            List<string> listaNombreArchivo = new List<string>();
            string pathLocal = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaIntercodiChecked = new List<int>();

                List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(seleccion, out List<int> listaIntercodi);
                listaIntercodiChecked = listaIntercodi;

                foreach (var item in listaIntercodiChecked)
                {
                    var ObjIntervencion = intServicio.GetByIdInIntervencion(item);

                    ObjIntervencion.Interflagsustento = ObjIntervencion.Interflagsustento ?? 0;
                    if (ObjIntervencion.Interflagsustento == 1)
                    {
                        //generar pdf de las interveciones
                        intServicio.GenerarReportePDFSustento(item, pathLocal, out string nombreArchivo);
                        listaNombreArchivo.Add(nombreArchivo);
                    }
                }

                if (listaNombreArchivo.Count == 0)
                {
                    throw new ArgumentException("No existe ninguna intervención con sustento");
                }

                model.Resultado = ConstantesIntervencionesAppServicio.RutaReportes;
                model.ListaNombreArchivo = listaNombreArchivo;
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
        public ActionResult IntervencionesMensajeRegistro(string intercodis, string origen)
        {
            Intervencion model = new Intervencion
            {
                Intercodis = (intercodis ?? "").Replace(";", ",")
            };

            var intercodi = model.Intercodis.Split(',').Select(x => Int32.Parse(x)).ToList()[0];
            InIntervencionDTO regInter = intServicio.GetByIdInIntervencion(intercodi);
            var progrcodi = regInter.Progrcodi;
            var evenclasecodi = regInter.Evenclasecodi;
            InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrcodi);

            model.IdProgramacion = progrcodi;
            model.IdTipoProgramacion = evenclasecodi;
            model.Correo = intServicio.GetCorreoXTipoProgramacion(evenclasecodi);
            model.CarpetaFiles = Math.Abs((int)DateTime.Now.Ticks) * -1;

            model.ListaEspecialista = (new MigracionesAppServicio()).GetListaPersonalRol(ConstantesIntervencionesAppServicio.AreacodiSPR.ToString(), regProg.Progrfechaini, regProg.Progrfechafin)
                                            .OrderBy(x => x.Pernomb).ToList();

            var regPersona = model.ListaEspecialista.Find(x => (x.Peremail ?? "").Trim().ToLower() == (base.UserEmail ?? "").Trim().ToLower());
            //si el usuario logueado no se encuentra en la lista de SPR entonces agregarlo
            if (regPersona == null)
            {
                regPersona = new SiPersonaDTO() { Percodi = 0, Percargo = "", Pernomb = base.UserName, Pertelefono = "" };
                model.ListaEspecialista.Add(regPersona);
            }
            model.IdEspecialista = regPersona.Percodi;
            model.CargoEspecialista = regPersona.Percargo;
            model.TelefonoEspecialista = regPersona.Pertelefono;

            model.LogoEmail = ConstantesAppServicio.LogoCoesEmailIntervenciones;
            model.Origen = origen;

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

                this.intServicio.EnviarComunicacion(ConstantesIntervencionesAppServicio.AmbienteIntranet, plantillaCorreo, base.UserName,
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

                //obtener destinatario
                List<string> listaAgenteCorreo = new List<string>();
                foreach (var reg in listaIntervenciones)
                {
                    if (!string.IsNullOrEmpty(reg.Interusucreacion) && reg.Interusucreacion.Contains("@")) listaAgenteCorreo.Add(reg.Interusucreacion);
                    if (!string.IsNullOrEmpty(reg.Interusumodificacion) && reg.Interusumodificacion.Contains("@")) listaAgenteCorreo.Add(reg.Interusumodificacion);
                }
                listaAgenteCorreo = listaAgenteCorreo.Distinct().ToList();
                model.Destinatario = string.Join("; ", listaAgenteCorreo);
                model.CC = string.Join("; ", ObtenerCCcorreosAgente(listaIntervenciones, listaAgenteCorreo));

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

        /// <summary>
        /// Combo en cascada Programaciones x Tipo Programacion
        /// </summary>
        [HttpPost]
        public JsonResult ObtenerCargoEspecialista(int percodi)
        {
            SiPersonaDTO regPersona = (new PersonaAppServicio()).GetByIdSiPersona(percodi);
            if (regPersona != null)
            {
                return Json(regPersona);
            }
            else
            {
                return Json("-1");
            }
        }

        #endregion

        #region 3.0 Reporte, Reporte Tabla Gen., Reporte Tabla Trans., Potencia Indisponible, Actividad

        /// <summary>
        /// Reporte
        /// </summary>        
        [HttpPost]
        public JsonResult ExportarIntervenciones(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
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

        /// <summary>
        /// Generar Reporte en Excel de las Intervenciones de los Equipos de Generación,“Reporte Tabla Gen.”, y de las Subestaciones y Líneas de Transmisión, denominado “Reporte Tabla Trans.” 
        /// </summary>        
        [HttpPost]
        public JsonResult ExportarReporteTablaIntervenciones(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                intServicio.GenerarReporteTablaIntervenciones(objFiltro.TipoReporte, path, objFiltro, out string fileNameReporte);

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
        /// Reporte de potencia indisponible
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteTablaPotenciaIndisponible(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                intServicio.GenerarReporteTablaIntervencionesPotenciaIndisponible(path, objFiltro, out string fileNameReporte);

                model.Resultado = "1";
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

        [HttpPost]
        public JsonResult ExportarReporteActividades(int progcodi)
        {
            Intervencion model = new Intervencion();

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

                intServicio.GenerarReporteActividades(progcodi, path, out string fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
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

        #region 4.0 Reporte en Plantilla

        /// <summary>
        /// Exportar a excel consulta en el modulo de registro de intervenciones
        /// </summary>
        /// <param name="objInput"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarManttoConsultaRegistro(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;
                objFiltro.AgruparIntervencion = true;

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(objFiltro.ProgrcodiReal);
                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                , regProg.Progrfechaini, regProg.Progrfechafin, objFiltro.Evenclasecodi, null, out string fileNameRenombre);

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
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                if (idProgramacion <= 0) throw new Exception(Constantes.MensajePermisoNoValido);

                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);
                IntervencionFiltro objFiltro = intServicio.GetFiltroConsulta2(new IntervencionFiltro()
                {
                    Progrcodi = idProgramacion,
                    Evenclasecodi = idTipoProgramacion,
                    FechaIni = regProg.Progrfechaini,
                    FechaFin = regProg.Progrfechafin,
                    EsReporteExcel = true
                });

                if (!intServicio.ExisteHorizonteSuperiorAprobado(idProgramacion))
                {
                    throw new ArgumentException("¡No existen registros del horizonte temporal superior apobados por el COES para transferir!");
                }

                var listaIntervencionesExistentes = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                //validar si ya hay intervenciones creadas o importadas para la programación
                if (listaIntervencionesExistentes.Any())
                {
                    throw new ArgumentException("¡Existen registros válidos en el programa seleccionado, no se permite la opción de Transferir!");
                }

                //Extracción de los registros de intervenciones - CopiarIntervenciones
                var listaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(idTipoProgramacion,
                                                                             regProg.Progrfechaini,
                                                                             regProg.Progrfechafin,
                                                                             ConstantesIntervencionesAppServicio.FiltroEmpresaTodos, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);
                //Validar si no existen registros para transferir en el horizonte superior
                if (listaIntervenciones.Count == 0)
                {
                    throw new ArgumentException("¡No existen registros del horizonte temporal superior apobados por el COES para transferir!");
                }

                //Ejecución de la Copia de Intervenciones a la BD - CopiarIntervenciones
                intServicio.CopiarIntervenciones(listaIntervenciones,

                                                 idProgramacion,
                                                 idTipoProgramacion, base.UserName);
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
            Intervencion model = new Intervencion
            {
                Entidad = new InIntervencionDTO()
            };
            model.Entidad.Progrcodi = progrCodi;
            model.sIdsEmpresas = "-1";

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
            try
            {
                base.ValidarSesionJsonResult();
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

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
        public JsonResult ImportarIntervencionesExcel(int progrCodi, string fileName, string accion)
        {
            Intervencion model = new Intervencion
            {
                ListaIntervencionesCorrectas = new List<InIntervencionDTO>(),
                ListaIntervencionesErrores = new List<InIntervencionDTO>()
            };

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //validar que no sea programa revertido
                var regProg = intServicio.ObtenerProgramacionesPorId(progrCodi);
                if (regProg.EsPlanRevertido)
                    throw new Exception("¡No se permite realizar esta acción en un programa con reversión.");

                // Ruta de los archivos EXCEL leidos
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

                //Ejecución de la Importación de Intervenciones de un archivo Excel - ImportarIntervencionesXLSX

                // Validar datos de Excel y realiza la importacion de los registros de este archivo           
                // con el parametro de salida idEmpresasExcel para capturar las empresas de la lectura del ARCHIVO EXCEL
                intServicio.ValidarIntervencionesAImportarXLSX(ConstantesIntervencionesAppServicio.AmbienteIntranet, path, fileName, progrCodi, base.UserName, accion, new int[1],
                                                       out List<InIntervencionDTO> lstRegIntervencionesCorrectos,
                                                       out List<InIntervencionDTO> lstRegIntervencionesErroneos,
                                                       out List<InIntervencionDTO> listaNuevo,
                                                       out List<InIntervencionDTO> listaModificado,
                                                       out List<InIntervencionDTO> listaEliminado);

                model.ListaIntervencionesCorrectas = lstRegIntervencionesCorrectos;
                model.ListaIntervencionesErrores = lstRegIntervencionesErroneos;

                //validación si existen errores
                if (lstRegIntervencionesErroneos.Any())
                {
                    //Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 4
                    string filenameCSV = intServicio.GenerarArchivoIntervencionesErroneasCSV(path, lstRegIntervencionesErroneos);
                    model.FileName = filenameCSV;

                    throw new Exception("¡No se guardó la información! Existen datos o registros que no permiten cargar el archivo completo.");
                }

                //validación si no tiene datos
                if (lstRegIntervencionesCorrectos.Count() == 0)
                {
                    //Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 4");
                    throw new Exception("Por favor ingrese un documento con registros nuevos y/o actualizados.");
                }

                //Ejecución de la grabación de Intervenciones de un archivo Excel - ImportarIntervenciones 1");
                // Se guardan los registros importados en la BD con el parametroidEmpresasExcel para realizar
                // la eliminación fisica de registros de los Id empresas obtendos del archivo Excel
                intServicio.ImportarIntervenciones(listaNuevo, listaModificado, listaEliminado, progrCodi, base.UserName);

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

            // lo guarda el CSV en la carpeta de descarga
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
                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(idProgramacion);

                var listaIntervenciones = intServicio.ListarIntervencionesHorizonteSuperior(regProg.Evenclasecodi,
                                                                                regProg.Progrfechaini,
                                                                                regProg.Progrfechafin,
                                                                                ConstantesIntervencionesAppServicio.FiltroEmpresaTodos, idProgramacion, ConstantesIntervencionesAppServicio.FiltroEquipoTodos);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                model.NombreArchivo = fileName;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;
                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                    , regProg.Progrfechaini, regProg.Progrfechafin, regProg.Evenclasecodi, null, out string fileNameRenombre);

                //Validar si no existen registros para transferir en el horizonte superior
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

        #endregion

        #region 8.0 Plantilla

        [HttpPost]
        public JsonResult DescargarManttoPlantillaActualizada(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                InProgramacionDTO regProg = intServicio.ObtenerProgramacionesPorIdSinPlazo(objFiltro.ProgrcodiReal);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;
                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                intServicio.GenerarPlantillaActualizada(AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName
                                                , regProg.Progrfechaini, regProg.Progrfechafin, objFiltro.Evenclasecodi, null, out string fileNameRenombre);

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

        #region 9.0 Habilitar Reversión

        /// <summary>
        /// Permite Habilitar la reversión de un programa
        /// </summary>
        /// <param name="progrCodi"></param>
        /// <param name="idTipoProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HabilitarReversion(int progrCodi)
        {
            Intervencion model = new Intervencion
            {
                ListaIntervenciones = new List<InIntervencionDTO>()
            };

            try
            {
                base.ValidarSesionJsonResult();

                //Actualizar programa como habilitado para reversión
                intServicio.HabilitarReversionIntervenciones(progrCodi, base.UserName);
                model.EsRevertido = true;
                model.StrMensaje = "Se habilitó satisfactoriamente la reversión";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);
            JsonResult.MaxJsonLength = Int32.MaxValue;

            return JsonResult;
        }

        /// <summary>
        /// Anular la reversión debido a que no lo aprobaron en el tiempo oportuno
        /// </summary>
        /// <param name="progrCodi"></param>
        /// <param name="idTipoProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EliminarReversion()
        {
            Intervencion model = new Intervencion
            {
                ListaIntervenciones = new List<InIntervencionDTO>()
            };

            try
            {
                base.ValidarSesionJsonResult();
                intServicio.EjecutarProcesoAutomaticoAnularReversion();

                model.Resultado = "1";
                model.EsRevertido = false;
                model.StrMensaje = "Se anuló el proceso de reversión satisfactoriamente";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.StrMensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            var JsonResult = Json(model);

            return JsonResult;
        }

        #endregion

        #region 10.0 Descargar Archivos Adjuntos

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

        #region 11.0 Reporte de comunicaciones

        [HttpPost]
        public JsonResult DescargarReporteComunicacionSeleccionados(string intercodis)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaEmprcodiLectura = new List<int>() { 1 };

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

                    intServicio.GenerarReporteComunicacionesIntervenciones(listaIntervenciones, ConstantesIntervencionesAppServicio.AmbienteIntranet, listaEmprcodiLectura,
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

        #region 12.0 Desagregar y Agrupar

        [HttpPost]
        public JsonResult DesagregarIntervenciones(int opcion, string tipo, string strHoraIni, string strHoraFin)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                List<InIntervencionDTO> listaNuevo = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaModificado = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaEliminable = new List<InIntervencionDTO>();

                if (opcion == 1) // desagrupar
                {
                    intServicio.ListarIntervencionesDesagrupables(listaIntercodiChecked,
                                out listaNuevo, out listaModificado, out List<InIntervencionDTO> listaWarning);
                }

                if (opcion == 2) //desagruparPorHora
                {
                    intServicio.ListarIntervencionesDesagrupablesXHora(listaIntercodiChecked, strHoraIni, strHoraFin,
                                out listaNuevo, out listaModificado, out List<InIntervencionDTO> listaWarning);
                }

                intServicio.CrudListaIntervencion(listaNuevo, listaModificado, listaEliminable, base.UserName, true);

                model.Resultado = "0";
                if (listaNuevo.Any() || listaModificado.Any() || listaEliminable.Any())
                {
                    model.Resultado = "1";
                    model.Mensaje = "Se desagregó satisfactoriamente los registros";
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

        [HttpPost]
        public JsonResult AgruparIntervenciones(string tipo)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                List<InIntervencionDTO> listaNuevo = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaModificado = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaEliminable = new List<InIntervencionDTO>();

                //Agrupar
                intServicio.ListarIntervencionesAgrupables(listaIntercodiChecked,
                            out listaModificado, out listaEliminable, out List<InIntervencionDTO> listaWarning);

                intServicio.CrudListaIntervencion(listaNuevo, listaModificado, listaEliminable, base.UserName, true);

                model.Resultado = "0";
                if (listaNuevo.Any() || listaModificado.Any() || listaEliminable.Any())
                {
                    model.Resultado = "1";
                    model.Mensaje = "Se Agrupó satisfactoriamente los registros";
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

        #region 13.0 Recuperar eliminados y rechazados

        [HttpPost]
        public JsonResult RecuperarIntervenciones(string tipo, int progrCodi)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();

                //solo eliminar las no aprobadas
                var esrecuperable = intServicio.RecuperarIntervenciones(listaIntercodiChecked, progrCodi);

                if (!esrecuperable) // validar duplicados al recuperar
                {
                    model.Resultado = "0";
                }
                else
                {
                    model.Resultado = "1";
                    model.Mensaje = "Se recuperó satisfactoriamente los registros";
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

        #region 14.0 Intervenciones del Agente

        /// <summary>
        /// Listar las intervenciones realizadas por el agente
        /// </summary>
        /// <param name="progrCodi"></param>
        /// <param name="idTipoProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarIntervencionesAgente(IntervencionInputWeb objInput)
        {
            base.ValidarSesionJsonResult();

            Intervencion model = new Intervencion();
            IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
            objFiltro.EsReporteExcel = true;
            objFiltro.AgruparIntervencion = true;

            var listIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);
            model.ListaIntervenciones = intServicio.FiltrarIntervencionesDelAgente(listIntervenciones);

            return PartialView(model);
        }

        /// <summary>
        /// Exportar intervenciones realizadas por el agente
        /// </summary>
        /// <param name="progrCodi"></param>
        /// <param name="idTipoProgramacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarIntervencionesAgente(IntervencionInputWeb objInput)
        {
            Intervencion model = new Intervencion();

            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;
                objFiltro.AgruparIntervencion = true;

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesRegistro(objFiltro);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;

                intServicio.GenerarReporteIntervencionesAgente(listaIntervenciones, path, pathLogo, out string fileName);

                model.Resultado = "1";
                model.NombreArchivo = fileName;
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

        #region 15.0 Cambiar Estado

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
        [HttpPost]
        public PartialViewResult IntervencionesCambiarEstado(string stringIntervenciones, int rep)
        {
            base.ValidarSesionJsonResult();
            string intercodis = string.Empty;
            Intervencion model = new Intervencion();
            if (stringIntervenciones != "[]")
            {
                if (rep == 0)
                {
                    List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(stringIntervenciones, out List<int> listaIntercodi);
                    intercodis = string.Join(",", listaIntercodi);
                }
                if (rep == 1)
                {
                    intercodis = stringIntervenciones.Replace(";", ",");
                }

                //intervenciones que tienen check en la pantalla
                List<int> listaIntercodiChecked = new List<int>();
                if (!string.IsNullOrEmpty(intercodis))
                {

                    var listaIntervenciones = intServicio.ConsultarIntervencionesXIds(intercodis);
                    if (!listaIntervenciones.Any())
                    {
                        throw new ArgumentException("Debe seleccionar al menos una intervención con comunicaciones.");
                    }
                    else
                    {
                        model.ListaIntervenciones = listaIntervenciones;
                        model.ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioMantenimiento).Where(x => x.Estadocodi != 2).ToList();
                        model.Intercodis = intercodis;
                    }
                }
                model.Resultado = "1";
            }
            else
            {
                model.Resultado = "-1";
            }

            return PartialView(model);
        }

        public JsonResult ActualizarEstado(int estadocodi, string intercodis)
        {
            Intervencion model = new Intervencion();
            try
            {
                model.TienePermisoSPR = true;
                List<int> listaIntercodiChecked = new List<int>();
                string usuarioModificacion = base.UserName;
                DateTime fechaModificacion = DateTime.Now;
                listaIntercodiChecked = intercodis.Replace(";", ",").Split(',').Select(x => Int32.Parse(x)).Distinct().ToList();

                //las intervenciones deben ser distintas al estado seleccionado
                var listaIntervenciones = intServicio.ConsultarIntervencionesXIdsYSustento(intercodis.Replace(";", ",")).Where(x => x.Estadocodi != estadocodi).ToList();

                if (!listaIntervenciones.Any())
                {
                    throw new ArgumentException("Debe seleccionar al menos una intervención para cambiar su estado.");
                }
                else
                {
                    var lestado = listaIntervenciones.Where(x => x.Estadocodi == estadocodi).ToList();
                    if (lestado.Count == 0)
                    {
                        var listaUpdateIn = new List<InIntervencionDTO>();

                        foreach (var intervencionEdit in listaIntervenciones)
                        {
                            //solo actualizar los no aprobados
                            if (ConstantesIntervencionesAppServicio.InEstadoAprobado != intervencionEdit.Estadocodi)
                            {
                                var entidad = intervencionEdit;
                                intervencionEdit.Estadocodi = estadocodi;
                                intervencionEdit.Interusumodificacion = usuarioModificacion;
                                intervencionEdit.Interfecmodificacion = fechaModificacion;

                                //capturar valores
                                entidad.Estadocodi = intervencionEdit.Estadocodi;
                                entidad.Interusumodificacion = usuarioModificacion;
                                entidad.Interfecmodificacion = fechaModificacion;
                                entidad.Actividad = "Se ha actualizado intervencion";
                                entidad.Interleido = ConstantesIntervencionesAppServicio.AgenteNoLeyo;

                                listaUpdateIn.Add(entidad);
                            }

                        }

                        // Graba los cambio en la BD
                        intServicio.CrudListaIntervencion(null, listaUpdateIn, null, usuarioModificacion);

                        model.StrMensaje = "¡La Información se actualizó correctamente!";
                        model.Resultado = "1";
                    }
                    else
                    {
                        model.StrMensaje = "¡Existen registros que ya tienen el estado seleccionado!";
                        model.Resultado = "-1";
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

            return Json(model);
        }

        #endregion

        #region 16.0 Descargar Archivos Adjuntos

        /// <summary>
        /// Descargar mensajes de las intervenciones seleccionadas
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DescargarMensajesMasivos(string tipo, int progrcodi, int modulo)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaIntercodiChecked = new List<int>();

                if (modulo == 0)
                {
                    List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(tipo, out List<int> listaIntercodi);
                    listaIntercodiChecked = listaIntercodi;
                }
                else
                {
                    //intervenciones que tienen check en la pantalla
                    if (!string.IsNullOrEmpty(tipo)) listaIntercodiChecked = tipo.Split(';').Select(x => Int32.Parse(x)).Distinct().ToList();
                }

                //Validar que las intervenciones seleccionadas tengan mensajes
                listaIntercodiChecked = intServicio.ObtenerIntercodisConMensajes(listaIntercodiChecked);
                if (listaIntercodiChecked.Count < 1)
                    throw new ArgumentException("No existe ninguna intervención con mensajes");

                InProgramacionDTO objProgramacion = intServicio.ObtenerProgramacionesPorIdSinPlazo(progrcodi);

                int numeroAleatorio = (int)DateTime.Now.Ticks;
                intServicio.DescargarZipMensajesMasivos(numeroAleatorio.ToString(), listaIntercodiChecked, objProgramacion.CarpetaProgDefault, out string nameFile);
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

        public virtual FileResult ExportarZipMsjMasivos()
        {
            string sFecha = DateTime.Now.ToString("yyyyMMddHHmmss");
            string nombreArchivo = Request["file_name"];

            string modulo = ConstantesIntervencionesAppServicio.sModuloTemporalMensaje;
            string pathDestino = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + modulo + @"/" + ConstantesIntervencionesAppServicio.NombreArchivosZip;
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

        #region 17.0 Historia equipo

        [HttpPost]
        public JsonResult FormularioIndexHistoria()
        {
            Intervencion model = new Intervencion
            {
                ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsultaCruzadas),

                Progrfechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha),
                Progrfechafin = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha)
            };

            return Json(model);
        }

        [HttpPost]
        public JsonResult MostrarHistoriaEquipo(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                List<InIntervencionDTO> listaFinal = new List<InIntervencionDTO>();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                model.Progrcodi = objFiltro.Progrcodi;
                model.IdTipoProgramacion = objFiltro.Evenclasecodi;

                listaFinal = intServicio.ObtenerIntervencionesHistoriaEquipo(objFiltro);

                listaFinal = listaFinal.OrderByDescending(x => x.Evenclasecodi).ToList();

                //generar objeto para la vista
                var listaFilaWeb = listaFinal.Select(x => new IntervencionFila()
                {
                    Intercodi = x.Intercodi,
                    Progrcodi = x.Progrcodi,

                    Equicodi = x.Equicodi,
                    Evenclasedesc = x.Evenclasedesc,

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
                    FinFecha = x.Interfechafin.ToString(ConstantesAppServicio.FormatoFecha),
                    IniHoraMinuto = x.Interfechaini.ToString(ConstantesAppServicio.FormatoOnlyHora),
                    FinHoraMinuto = x.Interfechafin.ToString(ConstantesAppServicio.FormatoOnlyHora)
                }).ToList();

                model.ListaFilaWeb = listaFilaWeb;

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
        public JsonResult ExportarHistoriaEquipo(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                model.Progrcodi = objFiltro.Progrcodi;
                model.IdTipoProgramacion = objFiltro.Evenclasecodi;
                objFiltro.EsReporteExcel = true;

                List<InIntervencionDTO> listaIntervenciones = intServicio.ObtenerIntervencionesHistoriaEquipo(objFiltro);

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string fileName = "Reporte_Historico.xlsx";

                intServicio.GenerarReporteIntervencionesHistoriaEquipo(listaIntervenciones, path, pathLogo, fileName);

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

        #region Dashboard

        [HttpPost]
        public JsonResult ConstruirDashboardFiltro(string fecha, int horizonte)
        {
            var model = new FactorF1F2Model();
            DateTime fechaC = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            fechaC = new DateTime(fechaC.Year, fechaC.Month, 1);

            model.Graficos = new List<GraficoWeb>();

            var graficoF1 = intServicio.GenerarGwebTacometroSPR(fechaC, 1, horizonte);
            model.Graficos.Add(graficoF1);

            var graficoF2 = intServicio.GenerarGwebTacometroSPR(fechaC, 2, horizonte);
            model.Graficos.Add(graficoF2);


            return Json(model);
        }

        [HttpGet]
        public ActionResult IndexDashboard(string fechaPeriodo)
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion == null) return base.RedirectToHomeDefault();

            string strFecha = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            if (fechaPeriodo != null)
            {

                DateTime Mes = DateTime.ParseExact(fechaPeriodo, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                strFecha = Mes.ToString("MM yyyy");
            }

            var modelo = new FactorF1F2Model
            {
                Mes = strFecha
            };

            return View(modelo);
        }

        [HttpPost]
        public JsonResult ConstruirDashboardSPR(string fecha)
        {
            var model = new FactorF1F2Model();
            DateTime fechaC = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoMes, CultureInfo.InvariantCulture);

            model.Graficos = new List<GraficoWeb>();

            var graficoF1 = intServicio.GenerarGwebTacometroSPR(fechaC, 1, 0);
            model.Graficos.Add(graficoF1);

            var graficoF2 = intServicio.GenerarGwebTacometroSPR(fechaC, 2, 0);
            model.Graficos.Add(graficoF2);

            var graficoMensualF1 = intServicio.GenerarGwebLineaMensualSPR(fechaC, 1);
            model.Graficos.Add(graficoMensualF1);

            var graficoMensualF2 = intServicio.GenerarGwebLineaMensualSPR(fechaC, 2);
            model.Graficos.Add(graficoMensualF2);

            return Json(model);
        }

        /// <summary>
        /// Genera un reporte Excel del Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarArchivoExcelDashboardSPR(DateTime fechaInicio)
        {
            FactorF1F2Model model = new FactorF1F2Model();

            try
            {
                base.ValidarSesionJsonResult();

                intServicio.GenerarArchivoExcelDashboardSPR(fechaInicio, out string nameFile);

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

        #endregion

        #region INTERVENCIONES - MODULO DE CONSULTA TABULAR

        /// GET: Intervenciones/Registro/IntervencionesConsulta
        /// <summary>
        /// Carga el formulario de consultas de intervenciones
        /// </summary>        
        public ActionResult IntervencionesConsulta()
        {
            Intervencion model = new Intervencion
            {
                Entidad = new InIntervencionDTO(),

                ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta),
                ListaCboIntervencion = intServicio.ListarComboTiposIntervenciones(ConstantesIntervencionesAppServicio.iEscenarioConsulta),

                ListaCboEmpresa = intServicio.ListarComboEmpresas(),

                ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioConsulta).Where(x => x.Estadocodi != ConstantesIntervencionesAppServicio.InEstadoRechazado).ToList(),
                ListaCboUbicacion = intServicio.ListarComboUbicacionesXEmpresa("0"),
                ListaFamilias = intServicio.ListarComboFamilias(),

                Progrfechaini = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha),
                Progrfechafin = DateTime.Today.ToString(ConstantesAppServicio.FormatoFecha)
            };

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
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                List<InIntervencionDTO> listaIntervenciones = intServicio.ConsultarIntervencionesTabulares(objFiltro);

                //generar objeto para la vista
                model.ListaFilaWeb = listaIntervenciones.Select(x => new IntervencionFila()
                {
                    Intercodi = x.Intercodi,
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

        /// <summary>
        /// Exportar a Excel consulta en el modulo de consulta de intervenciones
        /// </summary>        
        /// <param name="tipoProgramacion">Id de Tipo de Programación</param>
        /// <param name="tipoEvenCodi">Id de Tipo de evento</param>
        /// <param name="areaCodi">Id de Ubicación</param> 
        /// <param name="famCodi">Id de Familia</param>
        /// <param name="interIndispo">Indisponibilidad</param>
        /// <param name="estadoCodi">Id de estado de la intervención</param>
        /// <param name="interFechaIni">Fecha en que inicia la intervención</param>        
        /// <param name="interFechaFin">Fecha en que concluye la intervención</param>  
        /// <returns>Json</returns>
        [HttpPost]
        public JsonResult GenerarManttoConsultaConsulta(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                objFiltro.EsReporteExcel = true;
                objFiltro.AgruparIntervencion = true;

                var listaIntervenciones = intServicio.ConsultarIntervencionesTabulares(objFiltro);

                string fileName = ConstantesIntervencionesAppServicio.NombrePlantillaExcelManttoXlsm;

                string pathOrigen = ConstantesIntervencionesAppServicio.FolderRaizIntervenciones + ConstantesIntervencionesAppServicio.Plantilla;
                string pathDestino = ConstantesIntervencionesAppServicio.RutaReportes;

                FileServer.CopiarFileAlterFinalOrigen(pathOrigen, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName, null);

                //Genera el archivo Mantto Excel.xlsm de consulta de intervenciones - GenerarManttoConsultaConsulta");
                intServicio.GenerarManttoRegistro(listaIntervenciones, AppDomain.CurrentDomain.BaseDirectory + pathDestino, fileName,
                                                objFiltro.FechaIni, objFiltro.FechaFin, objFiltro.Evenclasecodi, null, out string fileNameRenombre);

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
            Intervencion model = new Intervencion
            {
                ListaCboEmpresa = intServicio.ListarComboEmpresas(),
                ListacboEstado = intServicio.ListarComboEstados(ConstantesIntervencionesAppServicio.iEscenarioConsulta).Where(x => x.Estadocodi != ConstantesIntervencionesAppServicio.InEstadoRechazado).ToList(),
                ListaTiposProgramacion = intServicio.ListarComboTiposProgramaciones(ConstantesIntervencionesAppServicio.iEscenarioConsultaCruzadas),

                ListadoHrasIndisponiblidad = new List<HorasIndispo>
            {
                new HorasIndispo { id = 0, value = "HrMaxIndisp" }
            }
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

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);

                model.GridExcel = intServicio.ObtenerExcelWebIntervencionesCruzadas(objFiltro);
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

        /// <summary>
        /// Permite  grabar  grabar la copia de un/los registros de intervencion 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EjecutarOpcionIntervencionCruzada(string opcion, string strFechaDestino, string dataArrayCruzada,
                                                        string strHoraIni, string strHoraFin, string marcarRelevante)
        {
            Intervencion model = new Intervencion();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(dataArrayCruzada, out List<int> listaIntercodi);

                List<InIntervencionDTO> listaNuevo = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaModificado = new List<InIntervencionDTO>();
                List<InIntervencionDTO> listaEliminable = new List<InIntervencionDTO>();

                List<InIntervencionDTO> listaWarning = new List<InIntervencionDTO>();
                bool tieneusuagrup = false;

                if (opcion == "pegar_agregar" || opcion == "pegar_sobrescribir")
                {
                    int tipoAccion = opcion == "pegar_agregar" ? ConstantesIntervencionesAppServicio.AccionCruzadaAgregar : ConstantesIntervencionesAppServicio.AccionCruzadaSobreescribir;
                    DateTime fechaDestino = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    intServicio.ListarIntervencionesCopiables(tipoAccion, listaColumnaOrigen, fechaDestino, base.UserName,
                                                    out listaNuevo, out listaModificado, out listaEliminable, out listaWarning);
                }

                if (opcion == "dejar_agregar" || opcion == "dejar_sobrescribir")
                {
                    int tipoAccion = opcion == "dejar_agregar" ? ConstantesIntervencionesAppServicio.AccionCruzadaAgregar : ConstantesIntervencionesAppServicio.AccionCruzadaSobreescribir;
                    DateTime fechaDestino = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                    intServicio.ListarIntervencionesMovibles(tipoAccion, listaColumnaOrigen, fechaDestino, base.UserName,
                                                    out listaNuevo, out listaModificado, out listaEliminable, out listaWarning);
                }

                if (opcion == "eliminar")
                {
                    //primero se valida si las intervenciones pueden ser eliminadas
                    intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteIntranet, listaIntercodi,
                                                    out listaEliminable, out listaWarning);
                }

                if (opcion == "eliminarPorDia")
                {
                    DateTime fechaDestino = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                    //primero se valida si las intervenciones pueden ser eliminadas
                    intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteIntranet, listaIntercodi,
                                            out List<InIntervencionDTO> listaIntersectado, out listaWarning);

                    //obtener intervenciones que serian el resultado de eliminar el dia
                    intServicio.ListarIntervencionesSinDiaEliminadoCruzadasRegistro(listaIntersectado, fechaDestino, fechaDestino,
                                                out listaNuevo, out listaModificado, out listaEliminable, out List<InIntervencionDTO> listaWarning2);
                    listaWarning.AddRange(listaWarning2);
                }

                if (opcion == "agrupar")
                {
                    tieneusuagrup = true;
                    intServicio.ListarIntervencionesAgrupables(listaIntercodi,
                                out listaModificado, out listaEliminable, out listaWarning);
                }

                if (opcion == "desagrupar")
                {
                    tieneusuagrup = true;
                    intServicio.ListarIntervencionesDesagrupables(listaIntercodi,
                                out listaNuevo, out listaModificado, out listaWarning);
                }

                if (opcion == "desagruparPorHora")
                {
                    tieneusuagrup = true;
                    intServicio.ListarIntervencionesDesagrupablesXHora(listaIntercodi, strHoraIni, strHoraFin,
                                out listaNuevo, out listaModificado, out listaWarning);
                }

                if (opcion == "relevante" || opcion == "norelevante")
                {
                    intServicio.ListarIntervencionMarcablesONoCruzadas(listaIntercodi, marcarRelevante == ConstantesAppServicio.SI,
                                                out listaModificado, out listaWarning);
                }

                intServicio.CrudListaIntervencion(listaNuevo, listaModificado, listaEliminable, base.UserName, tieneusuagrup);

                model.Resultado = "0";
                if (listaNuevo.Any() || listaModificado.Any() || listaEliminable.Any()) model.Resultado = "1";

                model.ListaIntervencionesErrores = listaWarning.Where(x => (x.Actividad ?? "") != "").
                                            OrderBy(x => x.Equicodi).ThenBy(x => x.Evenclasecodi).ThenBy(x => x.NroItem).ToList();
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

        /// <summary>
        /// Obtener intervenciones por lista de codigos
        /// </summary>
        /// <param name="dataArrayCruzada"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaXIntercodi(string opcion, string strFechaDestino, string dataArrayCruzada,
                                        string strHoraIni, string strHoraFin)
        {
            Intervencion model = new Intervencion();

            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(dataArrayCruzada, out List<int> listaIntercodi);

                //obtener registros de bd
                List<InIntervencionDTO> lInterCambio = new List<InIntervencionDTO>();
                List<InIntervencionDTO> lInterWarning = new List<InIntervencionDTO>();

                //
                switch (opcion)
                {
                    case "pegar_agregar":
                    case "pegar_sobrescribir":
                        int tipoAccion = opcion == "pegar_agregar" ? ConstantesIntervencionesAppServicio.AccionCruzadaAgregar : ConstantesIntervencionesAppServicio.AccionCruzadaSobreescribir;
                        DateTime fechaDestino = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        intServicio.ListarIntervencionesCopiables(tipoAccion, listaColumnaOrigen, fechaDestino, base.UserName,
                                                        out List<InIntervencionDTO> listaNuevo1, out List<InIntervencionDTO> listaModificado1,
                                                        out List<InIntervencionDTO> listaEliminable1, out List<InIntervencionDTO> listaWarning1);
                        //lInterCambio.AddRange(listaNuevo1);
                        //lInterCambio.AddRange(listaModificado1);
                        //lInterCambio.AddRange(listaEliminable1);
                        lInterWarning.AddRange(listaWarning1);

                        break;

                    case "dejar_agregar":
                    case "dejar_sobrescribir":
                        int tipoAccion2 = opcion == "dejar_agregar" ? ConstantesIntervencionesAppServicio.AccionCruzadaAgregar : ConstantesIntervencionesAppServicio.AccionCruzadaSobreescribir;
                        DateTime fechaDestino2 = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                        intServicio.ListarIntervencionesMovibles(tipoAccion2, listaColumnaOrigen, fechaDestino2, base.UserName,
                                                        out List<InIntervencionDTO> listaNuevo2, out List<InIntervencionDTO> listaModificado2,
                                                        out List<InIntervencionDTO> listaEliminable3, out List<InIntervencionDTO> listaWarning3);
                        //lInterCambio.AddRange(listaNuevo2);
                        //lInterCambio.AddRange(listaModificado2);
                        //lInterCambio.AddRange(listaEliminable3);
                        lInterWarning.AddRange(listaWarning3);

                        break;
                    case "eliminar":
                        intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteIntranet, listaIntercodi,
                                                        out List<InIntervencionDTO> listaEliminable, out List<InIntervencionDTO> listaWarning);
                        //lInterCambio.AddRange(listaEliminable);
                        lInterWarning.AddRange(listaWarning);
                        break;
                    case "eliminarPorDia":
                        DateTime fechaDestino3 = DateTime.ParseExact(strFechaDestino, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                        //primero se valida si las intervenciones pueden ser eliminadas
                        intServicio.ListarIntervencionEditableRegistro(ConstantesIntervencionesAppServicio.AmbienteIntranet, listaIntercodi,
                                                out List<InIntervencionDTO> listaIntersectado, out List<InIntervencionDTO> listaWarning8);

                        //obtener intervenciones que serian el resultado de eliminar el dia
                        intServicio.ListarIntervencionesSinDiaEliminadoCruzadasRegistro(listaIntersectado, fechaDestino3, fechaDestino3,
                                                    out List<InIntervencionDTO> listaNuevo8, out List<InIntervencionDTO> listaModificado8,
                                                    out List<InIntervencionDTO> listaEliminable8, out List<InIntervencionDTO> listaWarning89);
                        //lInterCambio.AddRange(listaNuevo8);
                        //lInterCambio.AddRange(listaModificado8);
                        //lInterCambio.AddRange(listaEliminable8);
                        lInterWarning.AddRange(listaWarning8);
                        //lInterWarning.AddRange(listaWarning89);
                        break;
                    case "agrupar":
                        intServicio.ListarIntervencionesAgrupables(listaIntercodi,
                                    out List<InIntervencionDTO> listaModificado7, out List<InIntervencionDTO> listaEliminado7, out List<InIntervencionDTO> listaWarning7);
                        //lInterCambio.AddRange(listaEliminado7);
                        //lInterCambio.AddRange(listaModificado7);
                        lInterWarning.AddRange(listaWarning7);
                        break;
                    case "desagrupar":
                        intServicio.ListarIntervencionesDesagrupables(listaIntercodi,
                                    out List<InIntervencionDTO> listaNuevo5, out List<InIntervencionDTO> listaModificado5, out List<InIntervencionDTO> listaWarning5);
                        //lInterCambio.AddRange(listaNuevo5);
                        //lInterCambio.AddRange(listaModificado5);
                        lInterWarning.AddRange(listaWarning5);
                        break;
                    case "desagruparPorHora":
                        intServicio.ListarIntervencionesDesagrupablesXHora(listaIntercodi, strHoraIni, strHoraFin,
                                    out List<InIntervencionDTO> listaNuevo6, out List<InIntervencionDTO> listaModificado6, out List<InIntervencionDTO> listaWarning6);
                        //lInterCambio.AddRange(listaNuevo6);
                        //lInterCambio.AddRange(listaModificado6);
                        lInterWarning.AddRange(listaWarning6);
                        break;
                    case "relevante":
                    case "norelevante":
                        intServicio.ListarIntervencionMarcablesONoCruzadas(listaIntercodi, true,
                                                    out List<InIntervencionDTO> listaEditable2, out List<InIntervencionDTO> listaWarning2);
                        //lInterCambio.AddRange(listaEditable2);
                        lInterWarning.AddRange(listaWarning2);
                        break;
                    case "verMensaje":
                        var listaWarning10 = intServicio.ConsultarIntervencionesXIdsYSustento(string.Join(",", listaIntercodi));
                        lInterWarning.AddRange(listaWarning10);
                        break;
                }

                model.ListaIntervenciones = lInterWarning.Where(x => (x.Comentario ?? "").ToUpper() == "ORIGEN").
                                                OrderBy(x => x.Equicodi).ThenBy(x => x.Evenclasecodi).ThenBy(x => x.NroItem).ToList();
                model.ListaIntervencionesErrores = lInterWarning.Where(x => (x.Actividad ?? "") != "").
                                                OrderBy(x => x.Equicodi).ThenBy(x => x.Evenclasecodi).ThenBy(x => x.NroItem).ToList();

                model.Resultado = "0";
                if (model.ListaIntervenciones.Any()) model.Resultado = "1";
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

        private List<IntervencionCopiaGrid> ObtenerDatosCeldaCruzada(string dataArrayCopia, out List<int> listaInterEnArray)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<IntervencionCopiaGrid> listaCopiaOrigen = serializer.Deserialize<List<IntervencionCopiaGrid>>(dataArrayCopia);

            int posOrigen = 0;
            listaInterEnArray = new List<int>();
            foreach (var regC in listaCopiaOrigen)
            {
                regC.Fecha = DateTime.ParseExact(regC.StrFecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                regC.ListaInterCodi = !string.IsNullOrEmpty(regC.StrListaInterCodi) ? regC.StrListaInterCodi.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                regC.Contador = posOrigen;

                regC.ListaInterCodi = regC.ListaInterCodi.Except(listaInterEnArray).ToList();

                listaInterEnArray.AddRange(regC.ListaInterCodi);
                posOrigen++;
            }
            listaCopiaOrigen = listaCopiaOrigen.Where(x => x.ListaInterCodi.Any()).ToList();

            return listaCopiaOrigen;
        }

        [HttpPost]
        public JsonResult ObtenerModificaciones(string seleccion)
        {
            Intervencion model = new Intervencion
            {
                ListaModificaciones = new List<InModificacion>()
            };
            try
            {
                base.ValidarSesionJsonResult();
                List<int> listaIntercodiChecked = new List<int>();

                List<IntervencionCopiaGrid> listaColumnaOrigen = ObtenerDatosCeldaCruzada(seleccion, out List<int> listaIntercodi);
                listaIntercodiChecked = listaIntercodi;

                foreach (var item in listaIntercodiChecked)
                {
                    InModificacion modificacion = new InModificacion
                    {
                        Intercodi = item,
                        ListaIntervenciones = intServicio.ListarModificacionesXIntervencion(item)
                    };
                    model.ListaModificaciones.Add(modificacion);
                }

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
            JsonResult.MaxJsonLength = Int32.MaxValue;

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

                string fileNameReporte;
                if (objFiltro.TipoReporte == 1)
                {
                    //Reporte con formato (logo, colores)
                    intServicio.ExportarIntervencionesCruzadas(objFiltro, path, pathLogo, out fileNameReporte);
                }
                else
                {
                    //Reporte sin formato (muestra codigos y potencia indisponible)
                    intServicio.ExportarIntervencionesCruzadasIndisponible(objFiltro, path, out fileNameReporte);
                }

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
        /// Permite  Exportar a Descargar procedimientos maniobras
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarReporteNTIITR(DateTime fechaIni, DateTime fechaFin)
        {
            IntervencionResultado model = new IntervencionResultado();
            try
            {
                base.ValidarSesionJsonResult();

                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;
                string nombreArchivo = string.Format("NTIITR_{0}_{1}.xlsx", fechaIni.ToString("ddMMyyyy"), fechaFin.ToString("ddMMyyyy"));

                intServicio.GenerarArchivoExcelTIITR(fechaIni, fechaFin, path + nombreArchivo);

                model.Resultado = "1";
                model.NombreArchivoTmp = path + nombreArchivo;
                model.NombreArchivo = nombreArchivo;
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
        /// Permite  Exportar a Descargar procedimientos maniobras
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarProcedimientosManiobras(IntervencionInputWeb objInput)
        {
            IntervencionResultado model = new IntervencionResultado();

            try
            {
                base.ValidarSesionJsonResult();

                IntervencionFiltro objFiltro = GetFiltroConsultaWeb(objInput);
                List<InIntervencionDTO> intervencionesCruzadas = intServicio.ConsultarIntervencionesCruzadas(objFiltro);
                var listaEquiposIds = intervencionesCruzadas.Select(x => x.Equicodi).Distinct().ToList();

                intServicio.DescargarProcedimientoManiobra(GetCurrentCarpetaSesion(), listaEquiposIds, out List<string> listaNoDescargado);
                model.Resultado = listaEquiposIds.Count().ToString();
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
        /// Permite descargar archivos en un zip
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="sModulo">Nombre del modulo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirDescargarProcedimientosManiobras()
        {
            string fileName = "Documentos.zip";
            byte[] buffer = intServicio.GetBufferArchivoProcedimientoManiobra(GetCurrentCarpetaSesion(), fileName);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

        #region COMBOS EN CASCADA

        /// <summary>
        /// Combo en cascada Ubicación x Empresa
        /// </summary>
        /// <param name="idEmpresa">Id de empresa</param>        
        /// <returns>Json</returns>
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
        /// <param name="claProCodi">Id de clase programación</param>        
        /// <returns>Json</returns>
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

                if (model.ListaCausas.Any())
                    model.ListaCausas = model.ListaCausas.DistinctBy(x => x.Subcausadesc).ToList();
            }

            return Json(model);
        }

        /// <summary>
        /// Combo en cascada Equipo x Ubiciación x Tipo Equipo (Familia) y tipo de programa
        /// </summary>
        /// <param name="idUbicacion">Id de ubicación</param>
        /// <param name="idFamilia">Id de tipo de Equipo (Familia)</param>
        /// <returns>Json</returns>
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

        #region METODOS COMUNES

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesIntervencionesAppServicio.RutaReportes;

            return base.DescargarArchivoTemporalYEliminarlo(path, file);
        }

        /// <summary>
        /// Función que se encarga de descargar el archivo ubicado en un directorio cualquiera,
        /// en el explorador
        /// </summary>
        /// <param name="fullPath">Ruta completa</param>   
        /// <param name="filename">Nombre del archivo</param>   
        /// <returns>Archivo</returns>
        public FileResult DescargarArchivoDesdeCualquierDirectorio(string fullPath, string filename)
        {
            byte[] buffer = FileServer.DownloadToArrayByte(fullPath, "");
            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        #endregion

        private IntervencionFiltro GetFiltroConsultaWeb(IntervencionInputWeb input)
        {
            IntervencionFiltro obj = intServicio.GetFiltroConsulta1(input);

            return obj;
        }

        private string GetCurrentCarpetaSesion()
        {
            return base.UserName;
        }

    }
}
