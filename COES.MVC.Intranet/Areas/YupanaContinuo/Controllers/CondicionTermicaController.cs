using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.YupanaContinuo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.YupanaContinuo;
using COES.Servicios.Aplicacion.YupanaContinuo.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.YupanaContinuo.Controllers
{
    public class CondicionTermicaController : BaseController
    {
        private readonly HorasOperacionAppServicio _servHO = new HorasOperacionAppServicio();
        private readonly YupanaContinuoAppServicio yupanaServicio = new YupanaContinuoAppServicio();

        #region Declaración de variables

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
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

        public ActionResult IndexCoordinador()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            CondicionTermicaModel model = new CondicionTermicaModel();
            this.CargarFiltrosBusqueda(model, ConstantesHorasOperacion.IdTipoTermica.ToString());

            model.IdTipoCentral = ConstantesHorasOperacion.IdTipoTermica;
            model.IdEmpresa = int.Parse(ConstantesHorasOperacion.ParamEmpresaTodos);
            model.IdCentralSelect = int.Parse(ConstantesHorasOperacion.ParamCentralSeleccione);
            model.IdEquipo = int.Parse(ConstantesHorasOperacion.ParamModoSeleccione);
            model.TipoVistaCoordinador = ConstantesHorasOperacion.AlertaEmsSI;

            model.TienePermisoAdmin = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            //si existe un arbol en ejecucion
            CpArbolContinuoDTO objUltimoArbol = yupanaServicio.GetUltimoArbol();
            bool existeArbolEnEjec = objUltimoArbol != null;

            DateTime fechaActual = DateTime.Now;
            if (existeArbolEnEjec) fechaActual = objUltimoArbol.Cparbfecha.AddHours(objUltimoArbol.Cparbbloquehorario);
            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaHora = yupanaServicio.ListarHoras(fechaActual.Hour);

            return View(model);
        }

        /// <summary>
        /// Actualiza la informacion necesaria para cargar los filtros de busqueda en la vista principal
        /// </summary>
        /// <param name="model"></param>
        private void CargarFiltrosBusqueda(CondicionTermicaModel model, string tipoCentrales)
        {
            model.ListaEmpresas = _servHO.ListarEmpresasHorasOperacionByTipoCentral(true, null, tipoCentrales);
            model.ListaEmpresas = model.ListaEmpresas.Count > 0 ? model.ListaEmpresas : new List<SiEmpresaDTO>() { new SiEmpresaDTO() { Emprcodi = 0, Emprnomb = "No Existe" } };

            if (model.Fecha == null)
            {
                model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);
            }
        }

        /// <summary>
        /// Retorna una vista parcial para ingreso de periodos forzados
        /// </summary>
        /// <param name="objJsonForm"></param>
        /// <returns></returns>
        public PartialViewResult ViewIngresoPeriodoForzado(string objJsonForm)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            CondicionTermicaModel model = objJsonForm != null ? serializer.Deserialize<CondicionTermicaModel>(objJsonForm) : new CondicionTermicaModel();

            //
            this.CargarFiltrosBusqueda(model, model.IdTipoCentral == ConstantesHorasOperacion.IdTipoTermica ? ConstantesHorasOperacion.IdTipoTermica.ToString() : ConstantesHorasOperacion.CodFamiliasNoTermica);
            model.ListaEmpresas = model.IdPos >= 0 ? model.ListaEmpresas.Where(x => x.Emprcodi == model.IdEmpresa).ToList() : model.ListaEmpresas;

            //
            //DateTime fecha = DateTime.ParseExact(model.Fecha, ConstantesYupanaContinuo.FormatoFechaHora, CultureInfo.InvariantCulture);

            //
            model.ActFiltroCtral = (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoTermica) ? "disabled" : "";

            model.ListaCentrales = _servHO.ListarCentralesXEmpresaGener(model.IdEmpresa)
                                    .Where(x => x.Famcodipadre == model.IdTipoCentral)
                                    .GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                                    .Select(y => new EqEquipoDTO() { Equicodi = y.Key.Codipadre, Equinomb = y.Key.Nombrecentral })
                                    .ToList();

            if (model.IdPos > -1) // si es modificación de hora de operacion
            {
            }
            else // si es nueva hora de operacion 
            {
                //model.FechaFin = fecha.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
                model.IdCentralSelect = model.IdCentralSelect == -1 ? model.ListaCentrales[0].Equicodi : model.IdCentralSelect;
            }

            //Determinar si la Central es un Reserva fria que necesita registrar las unidades en el campo descripción

            if (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoHidraulica) //Ctral Hidraulica
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaGrupo;
            }

            if (model.IdTipoCentral == ConstantesHorasOperacion.IdTipoTermica) //Térmicas
            {
                model.EtiquetaFiltro = ConstantesHorasOperacion.EtiquetaModo;

                //*************************************************************************************************************
                //********* Para ciertas empresas y centrales con modos de operacion y unidades asociadas manualmente
                //*************************************************************************************************************

                int idModo = model.IdEquipoOrIdModo;
                PrGrupoDTO modoEspecial = idModo > 0 ? _servHO.ListarModoOperacionXCentralYEmpresa(model.IdCentralSelect, model.IdEmpresa)
                                           .Find(x => x.FlagModoEspecial == ConstantesHorasOperacion.FlagModoEspecial && x.Grupocodi == idModo) : null;
            }

            model.SelectListItem = ObtenerIntervaloMediasHorasInicio();
            model.SelectListItem2 = ObtenerIntervaloMediasHoras();

            return PartialView(model);
        }

        /// <summary>
        /// Retorna listado de centrales por empresa y tipo central
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="tipoCentral"></param>
        /// <returns></returns>
        public JsonResult CargarCentral2(int idEmpresa, int tipoCentral)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            List<EqEquipoDTO> Lista = (_servHO.ListarCentralesXEmpresaGener(idEmpresa).Where(x => x.Famcodipadre == tipoCentral).ToList()).GroupBy(x => new { x.Codipadre, x.Nombrecentral })
                    .Select(y => new EqEquipoDTO()
                    {
                        Equicodi = y.Key.Codipadre,
                        Equinomb = y.Key.Nombrecentral
                    }).ToList();

            model.ListaCentrales = Lista.OrderBy(x => x.Equinomb).ToList();

            return Json(model);
        }

        /// <summary>
        /// Carga el listado de modos de operacion o Grupos de operacion según el tipo de central seleccionada
        /// </summary>
        /// <param name="idCentral"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarListaGrupoModo(int idCentral, int idEmpresa, int idTipoCentral, int modoGrupoSelect, int pos, int viewCoordinador)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();
            model.TipoVistaCoordinador = viewCoordinador; // Si la vista es para el coordinador o no
            int idModGrup = 0;
            model.IdTipoCentral = idTipoCentral;
            model.IdPos = pos; // posicion de la hora de operacion a modificar en el listado Horas de operacion 
            model.ActFiltroCtral = "";

            model.SelectListItem = new List<SelectListItem>();

            if (pos > -1)
            {
                model.ActFiltroCtral = "disabled";
            }
            switch (idTipoCentral)
            {
                case ConstantesHorasOperacion.IdTipoHidraulica: //Hidraulicas                     
                    var lstGrupoxCentralGen = _servHO.ListarGruposxCentralGEN(idEmpresa, idCentral, ConstantesHorasOperacion.IdGeneradorHidroelectrico).OrderBy(x => x.Equinomb).ToList();

                    model.SelectListItem = lstGrupoxCentralGen.Select(x => new SelectListItem() { Value = x.Equicodi.ToString(), Text = x.Equinomb }).ToList();

                    if (lstGrupoxCentralGen.Count > 0)
                        idModGrup = (int)lstGrupoxCentralGen.FirstOrDefault().Equicodi;
                    break;
                case ConstantesHorasOperacion.IdTipoTermica: //Térmicas
                    var listaModosOperacion = _servHO.ListarModoOperacionXCentralYEmpresa(idCentral, idEmpresa).Where(x => x.Grupocodi == modoGrupoSelect ||
                                                                    (x.GrupoEstado == ConstantesAppServicio.Activo)).OrderBy(x => x.Gruponomb).ToList();
                    model.SelectListItem = listaModosOperacion.Select(x => new SelectListItem() { Value = x.Grupocodi.ToString(), Text = x.Gruponomb }).ToList();

                    if (listaModosOperacion.Count > 0)
                        idModGrup = (int)listaModosOperacion.First().Grupocodi;
                    break;
                case ConstantesHorasOperacion.IdTipoSolar: //Solares
                case ConstantesHorasOperacion.IdTipoEolica: //Eolicas
                    break;
            }
            if (modoGrupoSelect != 0)
                model.IdGrupoModo = modoGrupoSelect;
            else
                model.IdGrupoModo = idModGrup;

            if (model.TipoVistaCoordinador == ConstantesHorasOperacion.AlertaEmsSI)
            {
                model.IdGrupoModo = (modoGrupoSelect == 0 && idModGrup == 0) ? int.Parse(ConstantesHorasOperacion.ParamModoSeleccione) : model.IdGrupoModo;

                if (model.SelectListItem.Count == 0 || model.SelectListItem.Count > 1)
                {
                    model.SelectListItem.Insert(0, new SelectListItem() { Value = model.ValorParamIdModoSeleccione, Text = "--SELECCIONE--" });
                }
            }

            return PartialView(model);
        }

        private List<SelectListItem> ObtenerIntervaloMediasHoras()
        {
            var listaMediasHoras = new List<SelectListItem>();
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            var index = 0;

            while (hoy < mañana)
            {
                hoy = hoy.AddMinutes(30);
                var periodo = ++index;
                var text = !(hoy.Hour == 0 && hoy.Minute == 0) ? hoy.ToString("HH:mm") : "24:00";
                listaMediasHoras.Add(new SelectListItem() { Text = text + $" [Bloque {periodo}]", Value = $"{periodo}" });
            }

            return listaMediasHoras;
        }

        private List<SelectListItem> ObtenerIntervaloMediasHorasInicio()
        {
            var listaMediasHoras = new List<SelectListItem>();
            var hoy = DateTime.Today;
            var mañana = hoy.AddDays(1);

            var index = 0;

            while (hoy < mañana)
            {
                hoy = hoy.AddMinutes(30);
                var periodo = ++index;
                var text = hoy.AddMinutes(-30).ToString("HH:mm");
                listaMediasHoras.Add(new SelectListItem() { Text = text + $" [Bloque {periodo}]", Value = $"{periodo}" });
            }

            return listaMediasHoras;
        }

        /// <summary>
        /// crea/modifica registros de condiciones termicas
        /// </summary>
        /// <param name="cpForzadoDet"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MantenerCondicionTermico(CpForzadoDetDTO cpForzadoDet, string fecha, int hora)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                int topcodi = yupanaServicio.GetTopologiaByDate(fechaActual);
                if (topcodi == 0)
                {
                    throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
                }

                yupanaServicio.MantenerCondicionesTermicas(cpForzadoDet, topcodi, fechaActual, UserName);

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        /// <summary>
        /// Crea registros de condidicon termica forzado de manera automatizada
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CondicionTermicaAutomatizado(string fecha, int hora)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                int topcodi = yupanaServicio.GetTopologiaByDate(fechaActual);
                if (topcodi == 0)
                {
                    throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
                }

                yupanaServicio.ActualizarAutomaticoCondicionTermica(base.UserName, topcodi, fechaActual);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GraficarCondicionTermica(string fecha, int hora)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                CpForzadoCabDTO cabecera = yupanaServicio.GetByDateCpForzadoCab(fechaActual);
                if (cabecera != null)
                    model.HtmlList = yupanaServicio.GraficarCondicionTermica(cabecera.Cpfzcodi, fechaActual);
                else
                    model.HtmlList = "";
                model.ForzadoCab = cabecera;

                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult GraficarCondicionTermicaPorCodigo(int cpfzcodi)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                var regCab = yupanaServicio.GetByIdCpForzadoCab(cpfzcodi);
                model.HtmlList = yupanaServicio.GraficarCondicionTermica(cpfzcodi, regCab.Cpfzfecha);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ReporteHtmlEnvios(string fecha, int hora)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                model.HtmlList = yupanaServicio.ReporteHtmlEnvios(fechaActual);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult ObtenerDetallePorId(int cpfzdtcodi)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ForzadoDet = yupanaServicio.GetByIdCpForzadoDet(cpfzdtcodi);
                model.Resultado = "1";
            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }

        [HttpPost]
        public JsonResult EliminarCondicionTermica(CpForzadoDetDTO cpForzadoDet, string fecha, int hora)
        {
            CondicionTermicaModel model = new CondicionTermicaModel();

            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new Exception(Constantes.MensajePermisoNoValido);

                DateTime fechaActual = DateTime.ParseExact(fecha, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                fechaActual = fechaActual.AddHours(hora);

                int topcodi = yupanaServicio.GetTopologiaByDate(fechaActual);
                if (topcodi == 0)
                {
                    throw new ArgumentException(ConstantesYupanaContinuo.MensajeNoExisteTopologia);
                }

                yupanaServicio.EliminarPeriodoForzado(cpForzadoDet, topcodi, fechaActual, UserName);

                model.Resultado = "1";

            }
            catch (Exception ex)
            {
                model.Resultado = "-1";

                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
            }

            return Json(model);
        }
    }
}