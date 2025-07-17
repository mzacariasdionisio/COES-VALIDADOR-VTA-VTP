using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.PotenciaFirme;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.Servicios.Aplicacion.SIOSEIN;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.PotenciaFirmeRemunerable.Controller
{
    public class ConfiguracionController : BaseController
    {
        private readonly PotenciaFirmeRemunerableAppServicio _pfrService;
        private readonly DespachoAppServicio _desapachoService;

        #region Declaración de variables

        private static readonly ILog Log = LogManager.GetLogger(typeof(ConfiguracionController));
        private static readonly string NameController = "ConfiguracionController";

        public ConfiguracionController()
        {
            _pfrService = new PotenciaFirmeRemunerableAppServicio();
            _desapachoService = new DespachoAppServicio();
        }

        /// <inheritdoc />
        /// <summary>
        /// Protected de log de errores page
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

        #region PARAMETROS

        /// <summary>
        /// Muestra la vista principal de Parametros
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexParametros(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }


        /// <summary>
        /// Listado de parametros
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoParametros(int pfrpercodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pfrpercodi);

            model.ListaParametros = _pfrService
                .ListarParametrosConfiguracionPorFecha(pfrPeriodo.FechaFin, ConstantesPotenciaFirmeRemunerable.ConcepcodiIngresos);
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);

            return PartialView("_ListadoParametros", model);
        }


        /// <summary>
        /// Listar historico de grupocodi y concepto
        /// </summary>
        /// <param name="grupocodi"></param>
        /// <param name="concepcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoParametros(int grupocodi, int concepcodi, int opedicion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.AccesoEditar = opedicion == 2 ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;
            model.ListaParametros = _pfrService.ListarGrupodatHistoricoValores(concepcodi, grupocodi);
            model.Conceptocodi = concepcodi;

            foreach (var reg in model.ListaParametros)
            {
                DateTime fechaDatDesc = DateTime.ParseExact(reg.FechadatDesc, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                reg.FechadatDesc = fechaDatDesc.Date.ToString(Constantes.FormatoFecha);
            }

            return PartialView("_ListarHistoricoParametros", model);
        }

        /// <summary>
        /// Registrar/editar parámetros configuración
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatGuardar(int tipoAccion, int grupocodi, int concepcodi, string strfechaDat, string formuladat, int deleted)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones

                if (concepcodi <= 0)
                {
                    throw new ArgumentException("Debe seleccionar un parámetro.");
                }

                if (deleted != 0)
                {
                    throw new ArgumentException("El registro ya ha sido eliminado, no puede modificarse.");
                }

                //Guardar
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    PrGrupodatDTO reg = new PrGrupodatDTO();
                    reg.Grupocodi = grupocodi;
                    reg.Formuladat = formuladat;
                    reg.Concepcodi = concepcodi;
                    reg.Fechadat = fechaDat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;
                    reg.Deleted = ConstantesSubasta.GrupodatActivo;

                    this._desapachoService.SavePrGrupodat(reg);
                }
                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PrGrupodatDTO reg = this._desapachoService.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);
                    if (reg == null)
                    {
                        throw new ArgumentException("El registro no existe, no puede modificarse.");
                    }
                    reg.Formuladat = formuladat;
                    reg.Lastuser = User.Identity.Name;
                    reg.Fechaact = DateTime.Now;

                    this._desapachoService.UpdatePrGrupodat(reg);
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
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrupodatEliminar(int grupocodi, int concepcodi, string strfechaDat, int deleted)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                //if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) throw new Exception(Constantes.MensajeOpcionNoPermitido);
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(strfechaDat, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PrGrupodatDTO reg = this._desapachoService.GetByIdPrGrupodat(fechaDat, concepcodi, grupocodi, ConstantesSubasta.GrupodatActivo);

                if (reg == null)
                {
                    throw new ArgumentException("El registro no existe o ha sido eliminada.");
                }

                reg.Lastuser = User.Identity.Name;
                reg.Fechaact = DateTime.Now;
                reg.Deleted2 = ConstantesSubasta.GrupodatInactivo;

                this._desapachoService.UpdatePrGrupodat(reg);

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
        /// Listar periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(anio);
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

        #region Index

        /// <summary>
        /// Muestra la vista principal de Relación Generadores GAMS y equipos
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexGamsEquipos(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                model.CodigoDisponibleGamsequipo = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos);

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();


                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);

                model.PfrPeriodo = regPeriodo;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Muestra la vista principal de Relación Generadores GAMS y SAAA
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexGamsSsaa(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();


                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);

                //model.ListaBarra = _pfrService.LstEquiposPorFamiliaEstado(Int32.Parse(ConstantesSioSein.FamcodiBarra), ConstantesPotenciaFirmeRemunerable.Aplicativo);
                //model.ListaBarra = _pfrService.LstEquiposPorEstado(ConstantesPotenciaFirmeRemunerable.Aplicativo).Where(c=>c.Famcodi == Int32.Parse(ConstantesSioSein.FamcodiBarra)).ToList();
                model.ListaUnidad = _pfrService.ListaUnidadesSsaa();
                model.PfrPeriodo = regPeriodo;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Muestra la vista principal de Relación Generadores GAMS y SAAA
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexGamsVtp(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);

                model.ListaBarraVtp = _pfrService.ListaBarraVtp().OrderBy(x => x.BarrNombre).ToList();
                model.PfrPeriodo = regPeriodo;

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        /// <summary>
        /// Muestra la vista principal de Parametros
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexEquipos(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                //Seteo de ultimos codigos equipos GAMS
                model.CodigoDisponibleBarra = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Barra);
                model.CodigoDisponibleLinea = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Linea);
                model.CodigoDisponibleTrafo2 = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Trafo2);
                model.CodigoDisponibleTrafo3 = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Trafo3);
                model.CodigoDisponibleCompDinamica = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.CompDinamica);

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                PfrPeriodoDTO regPeriodo;

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                    model.PfrPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);

                    DateTime fechaIniPeriodo = _pfrService.ObtenerPrimerDiaPeriodo(regPeriodo);
                    DateTime fechaFinPeriodo = _pfrService.ObtenerUltimoDiaPeriodo(regPeriodo);
                    //model.ListaBarra = _pfrService.ObtenerListadoEquiposPorVigenciaEstado(ConstantesPotenciaFirmeRemunerable.FamcodiBarra, fechaIniPeriodo, fechaFinPeriodo, (int)ConstantesPotenciaFirmeRemunerable.Estado.Activo, ConstantesPotenciaFirmeRemunerable.Aplicativo).OrderBy(x => x.Pfreqpid).ToList();

                    model.ListaBarrasnomb = _pfrService.GetListBarrasCategoria(ConstantesPotenciaFirmeRemunerable.CatecodiBarras).OrderBy(x => x.Gruponomb).ToList();
                }
                model.ListadoPeriodosFechasInicio = _pfrService.ObtenerListadoPeriodoFechasInicio();

            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Pagina principal de congestion
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexCongestion(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                //Seteo de ultimos codigos Congestion
                model.CodigoDisponibleCongestion = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Congestion);

                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                PfrPeriodoDTO regPeriodo;

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                    model.PfrPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);

                    DateTime fechaIniPeriodo = _pfrService.ObtenerPrimerDiaPeriodo(regPeriodo);
                    DateTime fechaFinPeriodo = _pfrService.ObtenerUltimoDiaPeriodo(regPeriodo);

                    model.ListaLineas = _pfrService.GetByCriteriaPfrEntidads((int)ConstantesPotenciaFirmeRemunerable.Tipo.Linea).OrderBy(x => x.Pfrentcodi).ToList();
                }
                model.ListadoPeriodosFechasInicio = _pfrService.ObtenerListadoPeriodoFechasInicio();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        /// <summary>
        /// Pagina principal de Penalidad
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexPenalidad(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                //Seteo de ultimos codigos Congestion                
                model.CodigoDisponiblePenalidad = "CPenaliza";


                DateTime fechaPeriodo = _pfrService.GetPeriodoActual();
                model.ListaAnio = _pfrService.ListaAnio(fechaPeriodo).ToList();
                model.Fecha = DateTime.Now.ToString(Constantes.FormatoFecha);
                PfrPeriodoDTO regPeriodo;

                if (pericodi.HasValue)
                {
                    model.IdPeriodo = pericodi.Value;
                    regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                    model.AnioActual = regPeriodo.FechaIni.Year;
                    model.ListaPeriodo = _pfrService.GetByCriteriaPfrPeriodos(regPeriodo.FechaIni.Year);
                    model.PfrPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);


                }
                model.ListadoPeriodosFechasInicio = _pfrService.ObtenerListadoPeriodoFechasInicio();
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);

        }

        #endregion

        /// <summary>
        /// Permite retornar en json lista de barras gams
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarUnidades(int pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {

                base.ValidarSesionJsonResult();
                var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi);
                model.ListaUnidad = _pfrService.ListaUnidades(regPeriodo.FechaIni, regPeriodo.FechaFin);
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
        /// Permite retornar en json lista de barras gams
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarBarrasGams(int pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.UsarLayoutModulo = ConstantesPotenciaFirmeRemunerable.UsarLayoutModulo;

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaBarra = _pfrService.GetByCriteriaPfrEntidads((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra).ToList();
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
        /// Devuelve el html de los combos con las barras actualizadas
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoActualBarras(int? pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi.Value);
                DateTime fechaIniPeriodo = _pfrService.ObtenerPrimerDiaPeriodo(regPeriodo);
                DateTime fechaFinPeriodo = _pfrService.ObtenerUltimoDiaPeriodo(regPeriodo);
                List<PfrEntidadDTO> listaBarras = _pfrService.GetByCriteriaPfrEntidads((int)ConstantesPotenciaFirmeRemunerable.Tipo.Barra).OrderBy(x => x.Pfrentid).ToList();

                string listaBarrasHtml = this._pfrService.GenerarHtmlListadoBarrasActualizadas(listaBarras);

                model.HtmlBarras = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbBarra", "Pfrentcodibarragamsd", false);
                model.HtmlBarras1 = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbBarra1", "Pfrentcodibarragams", false);
                model.HtmlBarras2 = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbBarra2", "Pfrentcodibarragams2", false);

                model.HtmlEBarras = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbEBarra", "Pfrentcodibarragamsd", true);
                model.HtmlEBarras1 = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbEBarra1", "Pfrentcodibarragams", true);
                model.HtmlEBarras2 = this._pfrService.HtmlListadoBarrasActualizadas(listaBarrasHtml, "cbEBarra2", "Pfrentcodibarragams2", true);


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

        #region Listado de entidades

        /// <summary>
        /// Listar equipos segun si tipo, vigenciaIni y estado
        /// </summary>
        /// <param name="pfrcatcodi"></param>
        /// <param name="pericodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEntidadXTipo(int pfrcatcodi, int pericodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = _pfrService.ObtenerHtmlRelacionEntidad(url, pfrcatcodi, pericodi, estado);
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
        /// Lista relación de Gams y Vtp
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRelacionGamsVtp(int pfrpercodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarRHtmlRelaciónGeneradoresGamsVtp(url, pfrpercodi, estado);
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
        /// Lista relación de Gams y Ssaa
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRelacionGamsSsaa(int pfrpercodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarRHtmlRelaciónGeneradoresGamsSsaa(url, pfrpercodi, estado);
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
        /// Lista relación de Gams y equipos
        /// </summary>
        /// <param name="pfrpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarRelacionGamsEquipos(int pfrpercodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarRHtmlRelaciónGeneradoresGamsEquipos(url, pfrpercodi, estado);
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
        /// Listar Congestion segun vigenciaIni y estado
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult ListarCongestion(int pericodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi);

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarRHtmlRelacionCongestion(url, pfrPeriodo, estado);
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
        /// Listar Congestion segun vigenciaIni y estado
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="estado"></param>
        /// <returns></returns> 
        [HttpPost]
        public JsonResult ListarPenalidad(int pericodi, int estado)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                PfrPeriodoDTO pfrPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi);

                string url = Url.Content("~/");
                model.Resultado = _pfrService.GenerarRHtmlRelacionPenalidad(url, pfrPeriodo, estado);
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

        #region Guardar

        /// <summary>
        /// Valida los equipos antes de Guardar
        /// </summary>
        /// <param name="pfrEquipo"></param>
        /// <param name="familia"></param>
        /// <param name="periodo"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarEntidad(PfrEntidadDTO pfrEntidad, int accion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                var usuario = User.Identity.Name;

                int retorno = _pfrService.ValidarBDEntidad(pfrEntidad, accion);

                //model.IdEquipo = pfrEquipo.Pfreqpid.Trim();
                model.Resultado = retorno.ToString();
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
        /// Valida una congestion, al ingresar o editar
        /// </summary>
        /// <param name="pfrCongestion"></param>
        /// <param name="periodo"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarCongestion(PfrEntidadDTO pfrCongestion, int accion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                int retorno = _pfrService.ValidarBDCongestion(pfrCongestion, accion);
                model.Resultado = retorno.ToString();
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
        /// Guarda la informacion de cada equipo
        /// </summary>
        /// <param name="pfrEntidad"></param>
        /// <param name="familia"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarEntidad(PfrEntidadDTO pfrEntidad, int accion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                var usuario = User.Identity.Name;

                if (accion == (int)ConstantesPotenciaFirmeRemunerable.Accion.Nuevo)
                {
                    _pfrService.GuardarBDEntidad(pfrEntidad, usuario);
                    model.CodigoDisponibleEquipo = _pfrService.ObtenerSiguienteIdDisponibleEquipo(pfrEntidad.Pfrcatcodi);
                }
                model.FamiliaEquipo = pfrEntidad.Pfrcatcodi;
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
        /// Guardar relación de Gams y Vtp
        /// </summary>
        /// <param name="pfrRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelacionGamsVtp(PfrEntidadDTO pfrRelacion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();

                _pfrService.GuardarBDEntidad(pfrRelacion, User.Identity.Name);
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
        /// Guardar relación de Gams y equipos
        /// </summary>
        /// <param name="pfrRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelacionGamsSsaa(PfrEntidadDTO pfrRelacion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                _pfrService.GuardarBDEntidad(pfrRelacion, User.Identity.Name);
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
        /// Guardar relación de Gams y equipos
        /// </summary>
        /// <param name="pfrRelacion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarRelacionGamsEquipos(PfrEntidadDTO pfrRelacion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                _pfrService.GuardarBDEntidad(pfrRelacion, User.Identity.Name);
                model.CodigoDisponibleGamsequipo = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.GamsEquipos);
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
        /// Guarda la informacion de cada congestion
        /// </summary>
        /// <param name="pfrCongestion"></param>
        /// <param name="periodo"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarCongestion(PfrEntidadDTO pfrCongestion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                _pfrService.GuardarBDEntidad(pfrCongestion, User.Identity.Name);
                model.CodigoDisponibleCongestion = _pfrService.ObtenerCodigoDisponibleEquipo(ConstantesPotenciaFirmeRemunerable.Tipo.Congestion);

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
        /// Guarda la informacion de cada penalizacion
        /// </summary>
        /// <param name="pfrPenalidad"></param>
        /// <param name="periodo"></param>
        /// <param name="accion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarPenalidad(PfrEntidadDTO pfrPenalidad)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                _pfrService.GuardarBDEntidad(pfrPenalidad, User.Identity.Name);

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

        #region Histórico de Propiedad

        /// <summary>
        /// Listado de parametros
        /// </summary>
        /// <param name="indpercodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListadoPropiedadXTipo(int pfrentcodi, int pericodi)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();

            var regPeriodo = _pfrService.GetByIdPfrPeriodo(pericodi);

            model.FechaIni = regPeriodo.FechaIni.ToString(ConstantesAppServicio.FormatoFecha);
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Editar, base.UserName);
            model.ListaPropiedadVigente = _pfrService.CompletarListaPropiedadVigente(pfrentcodi, regPeriodo.FechaIni);

            return PartialView(model);
        }

        /// <summary>
        /// Listar historico de entidad y concepto
        /// </summary>
        /// <param name="pfrentcodi"></param>
        /// <param name="pfrcnpcodi"></param>
        /// <param name="opedicion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListarHistoricoPropiedadXTipo(int pfrentcodi, int pfrcnpcodi, int opedicion)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            model.AccesoEditar = opedicion == Acciones.Editar ? base.VerificarAccesoAccion(Acciones.Editar, base.UserName) : false;
            model.ListaPropiedadVigente = _pfrService.GetByCriteriaPfrEntidadDats(pfrentcodi, pfrcnpcodi);

            return PartialView(model);
        }

        /// <summary>
        /// Registrar/editar parámetros configuración
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PfrEntidadDatGuardar(int tipoAccion, PfrEntidadDatDTO obj)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(obj.Prfdatfechavigdesc, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                //Validaciones
                if (obj.Pfrcnpcodi <= 0) throw new ArgumentException("Debe seleccionar una propiedad.");
                if (obj.Pfrdatdeleted != 0) throw new ArgumentException("El registro ya ha sido eliminado, no puede modificarse.");

                obj.Pfrdatvalor = (obj.Pfrdatvalor ?? "").Trim();
                // la letra s o n de vigencia debe guardarse con mayuscula
                if (obj.Pfrcnpcodi == (int)ConstantesPotenciaFirmeRemunerable.Concepto.Vigencia) 
                {
                    obj.Pfrdatvalor = obj.Pfrdatvalor.ToUpper();
                }

                //Guardar
                if (tipoAccion == ConstantesSubasta.AccionNuevo)
                {
                    obj.Prfdatfechavig = fechaDat;
                    obj.Pfrdatusucreacion = User.Identity.Name;
                    obj.Pfrdatfeccreacion = DateTime.Now;
                    obj.Pfrdatdeleted = ConstantesSubasta.GrupodatActivo;

                    this._pfrService.SavePfrEntidadDat(obj);
                }

                if (tipoAccion == ConstantesSubasta.AccionEditar)
                {
                    PfrEntidadDatDTO reg = this._pfrService.GetByIdPfrEntidadDat(obj.Pfrentcodi, obj.Pfrcnpcodi, fechaDat, ConstantesSubasta.GrupodatActivo);
                    if (reg == null)
                    {
                        throw new ArgumentException("El registro no existe, no puede modificarse.");
                    }
                    reg.Pfrdatvalor = obj.Pfrdatvalor;
                    reg.Pfrdatusumodificacion = User.Identity.Name;
                    reg.Pfrdatfecmodificacion = DateTime.Now;

                    this._pfrService.UpdatePfrEntidadDat(reg);
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
        /// Eliminar grupodat
        /// </summary>
        /// <param name="idFT"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PfrEntidadDatEliminar(PfrEntidadDatDTO obj)
        {
            PotenciaFirmeRemunerableModel model = new PotenciaFirmeRemunerableModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaDat = DateTime.ParseExact(obj.Prfdatfechavigdesc, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                PfrEntidadDatDTO reg = this._pfrService.GetByIdPfrEntidadDat(obj.Pfrentcodi, obj.Pfrcnpcodi, fechaDat, ConstantesSubasta.GrupodatActivo);

                if (reg == null)
                {
                    throw new ArgumentException("El registro no existe o ha sido eliminada.");
                }

                reg.Pfrdatusumodificacion = User.Identity.Name;
                reg.Pfrdatfecmodificacion = DateTime.Now;
                reg.Pfrdatdeleted2 = ConstantesSubasta.GrupodatInactivo;

                this._pfrService.UpdatePfrEntidadDat(reg);

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