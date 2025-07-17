using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IND.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Indisponibilidades;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Ind.Controllers
{
    public class FactorKController : BaseController
    {
        #region Declaración de variables
        private readonly INDAppServicio _indAppServicio;
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;
        #endregion
        public FactorKController()
        {
            log4net.Config.XmlConfigurator.Configure();
            _indAppServicio = new INDAppServicio();
        }

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

        /// <summary>
        /// Muestra la página de inicio de insumos de factor K y carga sus datos respectivos.
        /// </summary>
        /// <param name="pericodi"></param>
        /// <returns></returns>
        public ActionResult IndexFactorK(int? pericodi)
        {
            var model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                DateTime fechaPeriodo = _indAppServicio.GetPeriodoActual();
                model.ListaAnio = _indAppServicio.ListaAnio(fechaPeriodo).ToList();

                model.IdPeriodo = pericodi.Value;
                var regPeriodo = _indAppServicio.GetByIdIndPeriodo(pericodi.Value);
                model.AnioActual = regPeriodo.FechaIni.Year;
                model.ListaPeriodo = _indAppServicio.GetByCriteriaIndPeriodos(regPeriodo.FechaIni.Year);

                model.ListaTipoEmpresa = _indAppServicio.ListarTipoEmpresas();
                model.ListaEmpresa = _indAppServicio.ListarEmpresasConGaseoductoDeRelacionEmpresas("0");//0=Todos
                model.ListaCentral = new List<EqEquipoDTO>();
                model.ListaUnidadNombre = new List<IndRelacionEmpresaDTO>();
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
        /// Retorna las empresas del módulo de identificación de empresas correspondientes a un tipo de empresa y
        /// su gaseoducto sea mayor a 0
        /// Si tipoemprcodi = 0, devolverá todas las empresas
        /// </summary>
        /// <param name="tipoemprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarEmpresasDeIdentificacionEmpresas(string tipoemprcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaEmpresa = _indAppServicio.ListarEmpresasConGaseoductoDeRelacionEmpresas(tipoemprcodi);
                model.ListaCentral = new List<EqEquipoDTO>();
                model.ListaUnidadNombre = new List<IndRelacionEmpresaDTO>();
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
        /// Retorna las centrales del módulo de identificación de empresas correspondientes a una empresa y 
        /// su gaseoducto sea mayor a 0
        /// Si emprcodi = 0, devolverá todas las empresas
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarCentralesDeIdentificacionEmpresas(string emprcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaCentral = _indAppServicio.ListarCentralesConGaseoductoDeRelacionEmpresas(emprcodi);
                model.ListaUnidadNombre = new List<IndRelacionEmpresaDTO>();
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
        /// Retorna las unidadnombres del módulo de identificación de empresas correspondientes a una empresa y a una central, y 
        /// su gaseoducto sea mayor a 0
        /// Si emprcodi = 0 y equicodicentral = 0 devolverá todas las unidadnombres
        /// </summary>
        /// <param name="emprcodi"></param>
        /// <param name="equicodicentral"></param>
        /// <returns></returns>
        public JsonResult ListarUnidadNombresDeIdentificacionEmpresas(string emprcodi, string equicodicentral)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.ListaUnidadNombre = _indAppServicio.ListarUnidadNombresConGaseoductoDeRelacionEmpresas(emprcodi, equicodicentral);
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
        /// Importar insumos de factor K de la Extranet hacia la Intranet,
        /// para las empresas/centrales que están registradas en Identificación de Empresas y
        /// que su gaseoducto sea mayor a 0.
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="fechainicio"></param>
        /// <param name="fechafin"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodicentral"></param>
        /// <param name="relempcodi"></param>
        /// <returns></returns>
        [HttpPost] 
        public JsonResult ImportarInsumos(int ipericodi, string fechainicio, string fechafin, string emprcodi, string equicodicentral, string relempcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ImportarInsumosFactorK(ipericodi, fechainicio, fechafin, emprcodi, equicodicentral, relempcodi, User.Identity.Name, out string mensaje);
                model.Mensaje = mensaje;
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
        /// Consulta un único registro de insumos del factor K (ind_insumos_factork) y sus detalles correspondientes en base al filtro especificado
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="relempcodi"></param>
        /// <returns></returns>
        public JsonResult ConsultarInsumos(int ipericodi, int relempcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ConsultarInsumosFactorK(ipericodi, relempcodi, out IndRelacionEmpresaDTO relacionEmpresa, out IndInsumosFactorKDTO insumosFactorK, out HandsonModel handson, out string mensaje);
                model.RelacionEmpresa = relacionEmpresa;
                model.InsumosFactorK = insumosFactorK;
                model.HandsonInfkdt = handson;
                model.Mensaje = mensaje;
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
        /// Consulta un único registro de insumos del factor K (ind_insumos_factork) y sus detalles correspondientes en base al filtro especificado
        /// </summary>
        /// <param name="ipericodi"></param>
        /// <param name="emprcodi"></param>
        /// <param name="equicodicentral"></param>
        /// <param name="equicodiunidad"></param>
        /// <param name="grupocodi"></param>
        /// <param name="famcodi"></param>
        /// <returns></returns>
        public JsonResult ConsultarInsumosPV(int ipericodi, int emprcodi, int equicodicentral, int equicodiunidad, int grupocodi, int famcodi)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ConsultarInsumosFactorK(ipericodi, emprcodi, equicodicentral, equicodiunidad, grupocodi, famcodi, out IndRelacionEmpresaDTO relacionEmpresa, out IndInsumosFactorKDTO insumosFactorK, out HandsonModel handson, out string mensaje);
                model.RelacionEmpresa = relacionEmpresa;
                model.InsumosFactorK = insumosFactorK;
                model.HandsonInfkdt = handson;
                model.Mensaje = mensaje;
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
        /// Actualiza los insumos del factor K: insfckfrc y sus detalles del factor K
        /// </summary>
        /// <param name="insfckcodi"></param>
        /// <param name="insfckfrc"></param>
        /// <param name="dataht"></param>
        /// <returns></returns>
        public JsonResult ActualizarInsumos(int insfckcodi, decimal insfckfrc, string[][] dataht)
        {
            IndisponibilidadesModel model = new IndisponibilidadesModel();

            try
            {
                base.ValidarSesionJsonResult();

                model.Resultado = _indAppServicio.ActualizarInsumosFactorK(insfckcodi, insfckfrc, dataht, User.Identity.Name, out string mensaje);
                model.Mensaje = mensaje;
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