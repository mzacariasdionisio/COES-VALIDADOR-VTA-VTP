using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class ConfiguracionController : BaseController
    {
        readonly INDAppServicio indServicio = new INDAppServicio();

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

        public ConfiguracionController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion

        public ActionResult Index(int? pericodi, int? recacodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                //
                model.TienePermisoNuevo = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);


                DateTime fechaNuevo = this.indServicio.GetPeriodoActual();
                model.AnioActual = fechaNuevo.Year;
                model.MesActual = fechaNuevo.Month;

                DateTime fechaPeriodo = indServicio.GetPeriodoActual();
                model.ListaAnio = indServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = indServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaRecalculo = new List<IndRecalculoDTO>();
                model.ListaEmpresa = new List<SiEmpresaDTO>();
                if (model.ListaPeriodo.Any())
                {
                    model.ListaRecalculo = indServicio.GetByCriteriaIndRecalculos(model.IdPeriodo);
                    var regRecalculo = model.ListaRecalculo.FirstOrDefault();
                    model.IdRecalculo = regRecalculo != null && recacodi.GetValueOrDefault(0) == 0 ? regRecalculo.Irecacodi : recacodi.GetValueOrDefault(0);

                    indServicio.ListarFiltroUnidad(ConstantesHorasOperacion.IdTipoTermica, regPeriodo.FechaIni, regPeriodo.FechaFin, -1, -1
                                                            , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad);
                    model.ListaEmpresa = listaEmpresa;
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

        [HttpPost]
        public JsonResult CargarListadoEquivalencia()
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.TienePermisoEditar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
                string url = Url.Content("~/");

                model.Resultado = indServicio.GenerarReporteEquivalenciaHtml(model.TienePermisoEditar, url);
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
        public JsonResult GuardarEquivalencia(IndUnidadDTO inRegistro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (inRegistro.Equicodi <= 0)
                    throw new ArgumentException("Debe seleccionar un equipo.");

                if (string.IsNullOrEmpty(inRegistro.Iuninombcentral))
                    throw new ArgumentException("Debe ingresar nombre de central.");

                if (string.IsNullOrEmpty(inRegistro.Iuninombunidad))
                    throw new ArgumentException("Debe ingresar nombre de unidad.");

                if (indServicio.ExisteEquivalencia(inRegistro))
                    throw new ArgumentException("La equivalencia ya se encuentra registrado.");

                inRegistro.Iuninombcentral = inRegistro.Iuninombcentral.Trim().ToUpper();
                inRegistro.Iuninombunidad = inRegistro.Iuninombunidad.Trim().ToUpper();

                inRegistro.Iuniusucreacion = User.Identity.Name;
                inRegistro.Iunifeccreacion = DateTime.Now;
                inRegistro.Iuniactivo = 1;

                indServicio.SaveIndUnidad(inRegistro);

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
        public JsonResult EditarEquivalencia(IndUnidadDTO inRegistro)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (inRegistro.Iunicodi <= 0)
                    throw new ArgumentException("Registro no válido.");

                if (string.IsNullOrEmpty(inRegistro.Iuninombcentral))
                    throw new ArgumentException("Debe ingresar nombre de central.");

                if (string.IsNullOrEmpty(inRegistro.Iuninombunidad))
                    throw new ArgumentException("Debe ingresar nombre de unidad.");

                if (indServicio.ExisteEquivalencia(inRegistro))
                    throw new ArgumentException("La equivalencia ya se encuentra registrado.");

                inRegistro.Iuninombcentral = inRegistro.Iuninombcentral.Trim().ToUpper();
                inRegistro.Iuninombunidad = inRegistro.Iuninombunidad.Trim().ToUpper();

                inRegistro.Iuniusumodificacion= User.Identity.Name;
                inRegistro.Iunifecmodificacion = DateTime.Now;
                inRegistro.Iuniactivo = 1;

                indServicio.UpdateIndUnidad(inRegistro);

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
        public JsonResult EliminarEquivalencia(int id)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();
            try
            {
                base.ValidarSesionJsonResult();

                if (!base.VerificarAccesoAccion(Acciones.Grabar, base.UserName)) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                if (id <= 0)
                    throw new ArgumentException("Registro no válido.");

                var inRegistro = indServicio.GetByIdIndUnidad(id);

                inRegistro.Iuniusumodificacion = User.Identity.Name;
                inRegistro.Iunifecmodificacion = DateTime.Now;
                inRegistro.Iuniactivo = 0;

                indServicio.UpdateIndUnidad(inRegistro);

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
        /// Listar centrales y gasoductos por empresa
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarFiltros(int ipericodi, int famcodi, int emprcodi, int equipadre)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();
                var regPeriodo = indServicio.GetByIdIndPeriodo(ipericodi);

                indServicio.ListarFiltroUnidad(famcodi, regPeriodo.FechaIni, regPeriodo.FechaFin, emprcodi, equipadre
                                                        , out List<SiEmpresaDTO> listaEmpresa, out List<EqEquipoDTO> listaCentral, out List<EqEquipoDTO> listaUnidad);

                model.ListaEmpresa = listaEmpresa;
                model.ListaCentral = listaCentral;
                model.ListaEquipoFiltro = listaUnidad;

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

                model.ListaPeriodo = indServicio.GetByCriteriaIndPeriodos(anio);
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