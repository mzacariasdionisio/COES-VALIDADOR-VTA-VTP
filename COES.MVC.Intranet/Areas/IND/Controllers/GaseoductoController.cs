using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using static COES.MVC.Intranet.Areas.IND.Models.IndModel;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class GaseoductoController : BaseController
    {
        private readonly INDAppServicio _indAppServicio;

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

        public GaseoductoController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        #region Mantener Restricción Relación de Gaseoductos con Centrales Térmicas

        /// <summary>
        /// Vista principal Restricción Relación de Gaseoductos con Centrales Térmicas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? pericodi)
        {
            var model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaEmpresa = _indAppServicio.ListarEmpresaConGasoducto();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

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
        /// Listar centrales y gasoductos por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltros(int emprcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.ObtenerFiltroListaGaseoductos(emprcodi, out List<EqEquipoDTO> listaGasoducto, out List<EqEquipoDTO> listaCentral);
                model.ListaCentral = listaCentral;
                model.ListaGasoducto = listaGasoducto;

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

                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(anio);
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
        /// Listar gaseoducto por central
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaGaseoducto()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                model.ListadoRelacionGaseoducto = _indAppServicio.ListIndGaseoductoxcentrals();

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
        /// Eliminar gaseoducto por codigo
        /// </summary>
        /// <param name="gasctrcodi"></param>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult EliminarGaseoducto(int gasctrcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                _indAppServicio.InactivarIndGaseoductoxcentral(gasctrcodi);
                model.Estado = (int)TipoEstado.Ok;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Estado = (int)TipoEstado.Error;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Guardar Gaseoducto
        /// </summary>
        /// <param name="indGaseoductoxcentral"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GuardarGaseoducto(IndGaseoductoxcentralDTO indGaseoductoxcentral)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

                indGaseoductoxcentral.Gasctrusucreacion = User.Identity.Name;
                indGaseoductoxcentral.Gasctrfeccreacion = DateTime.Now;
                indGaseoductoxcentral.Gasctrestado = ConstantesIndisponibilidades.Activo;

                _indAppServicio.SaveIndGaseoductoxcentral(indGaseoductoxcentral);
                model.Estado = (int)TipoEstado.Ok;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Estado = (int)TipoEstado.Error;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Listar reporte relación Gaseoducto, rendimiento
        /// </summary>
        /// <param name="fechaConsulta"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarListaReporteRendimiento(int pericodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                _indAppServicio.ObtenerFechaIniYFechaFin(pericodi, out DateTime fechaIni, out DateTime fechaFin);

                model.Resultado = _indAppServicio.GenerarReporteHtmlGaseoducto(fechaIni, fechaFin);
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