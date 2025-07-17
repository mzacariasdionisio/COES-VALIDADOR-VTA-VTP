using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.IND.Controllers
{
    public class MantenimientoController : BaseController
    {
        readonly INDAppServicio servIndisp = new INDAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }

        public MantenimientoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        /// <summary>
        /// Vista principal Mantenimientos (registros del SGOCOES y registros del aplicativo PR25)
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? pericodi, int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.ListaTipoMantenimiento = this.servIndisp.ListarClaseEventos();
                model.ListaTipoEmpresa = this.servIndisp.ListarTipoEmpresas();
                model.ListaEmpresa = this.servIndisp.ListarEmpresasPorTipo(ConstantesAppServicio.ParametroDefecto);
                model.ListaFamilia = this.servIndisp.ListarFamilia(ConstantesAppServicio.ParametroDefecto);

                DateTime fechaPeriodo = servIndisp.GetPeriodoActual();
                model.ListaAnio = servIndisp.ListaAnio(fechaPeriodo).ToList();

                if (pericodi.GetValueOrDefault(0) <= 0)
                {
                    var listaPeriodo = servIndisp.GetByCriteriaIndPeriodos(fechaPeriodo.Year);
                    var regpertmp = listaPeriodo.Find(x=>x.FechaIni == fechaPeriodo);
                    pericodi = regpertmp.Ipericodi;
                }

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = servIndisp.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = servIndisp.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<IndRecalculoDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = servIndisp.GetByCriteriaIndRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Irecacodi : recacodi.GetValueOrDefault(0);
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
        /// Lista de mantenimientos
        /// </summary>
        /// <param name="tiposMantenimiento"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="indispo"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="interrupcion"></param>
        /// <param name="tiposMantto"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        public JsonResult Lista(string tiposMantenimiento, int pericodi, string tiposEmpresa,
            string empresas, string tiposEquipo, string flagFiltro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //maximo tres meses
                servIndisp.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni, out DateTime fechaFin);

                string url = Url.Content("~/");
                var listaManttos = servIndisp.ObtenerListaManttos(tiposMantenimiento, fechaIni, fechaFin, tiposEmpresa, empresas, tiposEquipo, flagFiltro);

                model.Resultado = this.servIndisp.GenerarReporteManttosHtml(true, flagFiltro, url, listaManttos);
                model.Resultado2 = servIndisp.GenerarReporteOmision7Dias(listaManttos.Where(x => x.Indmanomitir7d == ConstantesAppServicio.SI).ToList());
                model.Resultado3 = servIndisp.GenerarReporteOmisionExcesoPr(listaManttos.Where(x => x.Indmanomitirexcesopr == ConstantesAppServicio.SI).ToList());
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
        /// Listar empresas por tipo de empresa
        /// </summary>
        /// <param name="tiposEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EmpresaListado(string tiposEmpresa)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.ListaEmpresa = this.servIndisp.ListarEmpresasPorTipo(tiposEmpresa);

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
        /// Retorna Listado de Periodo por año en formato JSON
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PeriodoListado(int anio)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaPeriodo = servIndisp.GetByCriteriaIndPeriodos(anio);
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
        /// Listar recalculo por periodo en formato JSON
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoListado(int ipericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaRecalculo = servIndisp.GetByCriteriaIndRecalculos(ipericodi);
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
        /// Obtener datos de Recalculo por código de recalculo
        /// </summary>
        /// <param name="irecacodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecalculoDatosInsumo(int irecacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.IndRecalculo = servIndisp.GetByIdIndRecalculo(irecacodi);
                model.IndPeriodo = servIndisp.GetByIdIndPeriodo(model.IndRecalculo.Ipericodi);
                model.FechaIni = model.IndRecalculo.IrecafechainiDesc;
                model.FechaFin = model.IndRecalculo.IrecafechafinDesc;

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

        #region CRUD (EVE_MANTTO y IND_MANTTO) y Actualización Múltiple

        /// <summary>
        /// Visualizacion del Formulario de Registro, Edicion
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="idFuente"></param>
        /// <param name="id"></param>
        /// <param name="tipoAccion"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult MantenimientoFormulario(int pericodi, int idFuente, int tipoAccion, int? indmancodi = 0, int? manttocodi = 0)
        {
            DateTime fechaIni, fechaFin;

            EveManttoDTO mantto = new EveManttoDTO();
            BusquedaMantenimientoModel model = new BusquedaMantenimientoModel();
            model.TipoAccionFormulario = tipoAccion;
            model.FuenteDatos = idFuente;

            switch (tipoAccion)
            {
                case ConstantesIndisponibilidades.TipoAccionNuevo:

                    switch (idFuente)
                    {
                        case ConstantesIndisponibilidades.FuenteIndMantto:
                            servIndisp.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni1, out DateTime fechaFin1);
                            fechaIni = fechaIni1;
                            fechaFin = fechaIni.AddDays(1);

                            model.FechaActual = fechaIni.ToString(Constantes.FormatoFecha);
                            model.FechaSiguiente = fechaFin.ToString(Constantes.FormatoFecha);
                            model.FechaInicio = fechaIni.ToString(Constantes.FormatoFecha);
                            model.HoraInicial = fechaIni.ToString(Constantes.FormatoHora);
                            model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                            model.HoraFinal = fechaFin.ToString(Constantes.FormatoHora);
                            model.Empresa = string.Empty;
                            model.Familia = string.Empty;
                            model.Ubicacion = string.Empty;
                            model.Equinomb = string.Empty;
                            model.IdTipoMantenimiento = -1;
                            model.Descripcion = string.Empty;
                            model.Equicodi = 0;
                            model.Indmancodi = indmancodi.Value;
                            break;
                    }

                    break;
                case ConstantesIndisponibilidades.TipoAccionVer:
                case ConstantesIndisponibilidades.TipoAccionEditar:

                    switch (idFuente)
                    {
                        case ConstantesIndisponibilidades.FuenteIndMantto:
                            IndManttoDTO obj = this.servIndisp.GetByIdIndMantto2(indmancodi.Value);
                            mantto = this.servIndisp.GetEveManttoFromIndMantto(obj);
                            manttocodi = mantto.Manttocodi;
                            break;
                        case ConstantesIndisponibilidades.FuenteEveMantto:
                            List<IndManttoDTO> listaIndByEve = this.servIndisp.ListarIndManttoByEveMantto(manttocodi.Value.ToString())
                                                            .Where(x => x.Indmanestado == ConstantesAppServicio.Activo).ToList();
                            if (listaIndByEve.Count == 0)
                            {
                                mantto = this.servIndisp.GetByIdEveMantto2(manttocodi.Value);
                            }
                            else
                            {
                                indmancodi = listaIndByEve.First().Indmancodi;
                                IndManttoDTO obj2 = this.servIndisp.GetByIdIndMantto2(indmancodi.Value);
                                mantto = this.servIndisp.GetEveManttoFromIndMantto(obj2);
                            }
                            break;
                    }

                    fechaIni = mantto.Evenini.Value.Date;
                    fechaFin = fechaIni.AddDays(1);

                    model.FechaActual = fechaIni.ToString(Constantes.FormatoFecha);
                    model.FechaSiguiente = fechaFin.ToString(Constantes.FormatoFecha);
                    model.FechaInicio = mantto.Evenini.Value.ToString(Constantes.FormatoFecha);
                    model.HoraInicial = mantto.Evenini.Value.ToString(Constantes.FormatoHora);
                    model.FechaFin = mantto.Evenfin.Value.ToString(Constantes.FormatoFecha);
                    model.HoraFinal = mantto.Evenfin.Value.ToString(Constantes.FormatoHora);
                    model.Empresa = mantto.Emprnomb;
                    model.Familia = mantto.Famabrev;
                    model.Ubicacion = mantto.Areadesc;
                    model.Equinomb = mantto.Equiabrev;
                    model.IdTipoMantenimiento = mantto.Evenclasecodi.Value;
                    model.Descripcion = mantto.Evendescrip;
                    model.Equicodi = mantto.Equicodi.Value;
                    model.Manttocodi = manttocodi.Value;
                    model.Indmancodi = indmancodi.Value;

                    model.Tipoindisp = mantto.Eventipoindisp;
                    model.Pr = mantto.Evenpr;
                    model.Asocproc = mantto.Evenasocproc;
                    model.GrupoCogeneracion = mantto.Grupotipocogen;

                    break;
            }

            model.Tipoindisp = !string.IsNullOrEmpty(model.Tipoindisp) ? model.Tipoindisp : ConstantesAppServicio.ParametroDefecto;
            model.ListaTipoMantenimiento = this.servIndisp.ListarClaseEventos();
            model.ListaTipoindispPr25 = servIndisp.ListarTipoIndispPr25();

            return PartialView(model);
        }

        /// <summary>
        /// Registrar en la IND_MANTTO
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarMantenimiento(int idFuente, int tipoAccion, string data)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                JavaScriptSerializer serialize = new JavaScriptSerializer();
                BusquedaMantenimientoModel objData = serialize.Deserialize<BusquedaMantenimientoModel>(data);

                DateTime fechaIni = DateTime.ParseExact(objData.FechaInicio + " " + objData.HoraInicial, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(objData.FechaFin + " " + objData.HoraFinal, Constantes.FormatoFechaFull, CultureInfo.InvariantCulture);

                IndManttoDTO obj = new IndManttoDTO();
                obj.Equicodi = objData.Equicodi;
                obj.Evenclasecodi = objData.IdTipoMantenimiento;
                obj.Indmanfecini = fechaIni;
                obj.Indmanfecfin = fechaFin;
                obj.Indmandescripcion = objData.Descripcion;

                obj.Indmancodi = objData.Indmancodi;
                obj.Manttocodi = objData.Manttocodi;

                obj.Indmantipoindisp = objData.Tipoindisp;
                obj.Indmanpr = objData.Pr;
                obj.Indmanasocproc = objData.Asocproc;
                obj.Indmancomentario = objData.Comentario;

                this.servIndisp.GuardarIndMantto(tipoAccion, idFuente, objData.NroDiaReplicar, obj, User.Identity.Name);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
            }

            return Json(model);
        }

        /// <summary>
        /// Ver listado de cambios
        /// </summary>
        /// <param name="indmancodi"></param>
        /// <param name="manttocodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult VerHistorialCambio(int? indmancodi = 0, int? manttocodi = 0)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaIndMantto = servIndisp.ListarHistorialCambioMantto(indmancodi, manttocodi);
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
        /// Guardar Indisponibilidad de Eventos<
        /// </summary>
        /// <param name="strFechaIni"></param>
        /// <param name="strFechaFin"></param>
        /// <param name="tipoevento"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposEquipo"></param>
        /// <param name="lstIndEvento"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListadoManttoGuardar(string tiposMantenimiento, int pericodi, string tiposEmpresa,
            string empresas, string tiposEquipo, List<IndManttoDTO> listaUpdate, List<IndManttoDTO> listaDelete, string observacion)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();
                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                servIndisp.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni, out DateTime fechaFin);
                listaUpdate = listaUpdate != null ? listaUpdate : new List<IndManttoDTO>();
                listaDelete = listaDelete != null ? listaDelete : new List<IndManttoDTO>();

                servIndisp.GuardarCheckListaManttos(tiposMantenimiento, fechaIni, fechaFin, tiposEmpresa, empresas, tiposEquipo, listaUpdate, listaDelete, observacion, base.UserName);

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

        #region Listado de Equipos

        /// <summary>
        /// View Busqueda Linea de transmision
        /// </summary>
        /// <returns></returns>
        public PartialViewResult BusquedaEquipoIndex(int? filtroFamilia = 0)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            if (filtroFamilia == 0)
            {
                model.ListaEmpresa = this.servIndisp.ListarEmpresasxTipoEquipos(this.ListarFamiliaByFiltro(0, 0)).ToList();
                model.ListaFamilia = this.servIndisp.ListarFamilia(ConstantesAppServicio.ParametroDefecto);
            }
            else
            {
                model.ListaEmpresa = this.servIndisp.ListarEmpresasPorTipo(ConstantesAppServicio.ParametroDefecto);
                model.ListaFamilia = this.servIndisp.ListarFamilia(ConstantesAppServicio.ParametroDefecto);
            }
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
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            List<EqEquipoDTO> listaEquipo = new List<EqEquipoDTO>();
            var listaLinea = this.servIndisp.BuscarEquipoEvento(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro, nroPagina, Constantes.NroPageShow);

            foreach (var reg in listaLinea)
            {
                EqEquipoDTO eq = new EqEquipoDTO();
                eq.Emprnomb = reg.EMPRENOMB;
                eq.Areanomb = reg.AREANOMB;
                eq.Equicodi = reg.EQUICODI;
                eq.Equinomb = reg.EQUIABREV;
                eq.Equiabrev = reg.EQUIABREV;
                eq.Famabrev = reg.FAMABREV;
                eq.Emprcodi = reg.EMPRCODI;

                listaEquipo.Add(eq);
            }

            model.ListaEquipoFiltro = listaEquipo;
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
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.IndicadorPagina = false;
            int nroRegistros = this.servIndisp.ObtenerNroFilasBusquedaEquipo(idEmpresa, idArea, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia), filtro);

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
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            model.ListaArea = this.servIndisp.ObtenerAreaPorEmpresa(idEmpresa, this.ListarFamiliaByFiltro(idFamilia, filtroFamilia)).ToList();
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
            List<int> lista = this.servIndisp.ListaTipoEquipoFiltro();
            if (filtroFamilia == 0)
            {
                return idFamilia == 0 ? string.Join(",", lista) : idFamilia.ToString();
            }

            return idFamilia.ToString();
        }


        #endregion


        /// <summary>
        /// Listar recalculo por periodo en formato JSON
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarReporte(int irecacodi, int cuacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                servIndisp.ActualizarReporte(irecacodi, cuacodi, base.UserName);

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
    }
}
