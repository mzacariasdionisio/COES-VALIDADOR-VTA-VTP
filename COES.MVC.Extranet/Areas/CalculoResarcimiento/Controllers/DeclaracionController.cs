using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.CalculoResarcimiento.Model;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.CalculoResarcimientos;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.CalculoResarcimiento.Controllers
{
    public class DeclaracionController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>        
        CalculoResarcimientoAppServicio servicioResarcimiento = new CalculoResarcimientoAppServicio();

        #region Declaración de variables

        //readonly SeguridadServicioClient seguridad = new SeguridadServicioClient();

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(DeclaracionController));
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

        /// <summary>
        /// Permite mostrar la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            DeclaracionModel model = new DeclaracionModel();

            model.Anio = DateTime.Now.Year;
            model.ListaPeriodo = this.servicioResarcimiento.ObtenerPeriodosPorAnio(model.Anio);
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count > 1) ? Constantes.SI : Constantes.NO;
            if (model.ListaEmpresas.Count > 0)
            {
                model.Emprcodi = model.ListaEmpresas[0].Emprcodi;
                model.Emprnombre = model.ListaEmpresas[0].Emprnomb;
            }

            return View(model);
        }

        /// <summary>
        /// Permite mostrar los periodos por anio
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerPeriodos(int anio)
        {
            return Json(this.servicioResarcimiento.ObtenerPeriodosPorAnio(anio));
        }

        /// <summary>
        /// Permite mostrar el dialogo de cambio de empresa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresa()
        {
            DeclaracionModel model = new DeclaracionModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite realizar la consulta de las declaraciones
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int empresa, int periodo, int declaracionId)
        {
            DeclaracionModel model = new DeclaracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                
                List<ReEnvioDTO> listadoEnvios = new List<ReEnvioDTO>();
                string indicador = "";

                if (declaracionId == -1) // desde consultar
                {
                    listadoEnvios = servicioResarcimiento.ListarEnvios(empresa, periodo, ConstantesCalculoResarcimiento.EnvioTipoDeclaracion);
                    indicador = listadoEnvios.Any() ? (listadoEnvios.First().Reenvindicador) : "";
                }
                else  //desde ver envios
                {
                    ReEnvioDTO env = servicioResarcimiento.GetByIdReEnvio(declaracionId);
                    listadoEnvios = servicioResarcimiento.ListarEnvios(env.Emprcodi.Value, env.Repercodi.Value, ConstantesCalculoResarcimiento.EnvioTipoDeclaracion);
                    indicador = env.Reenvindicador;
                }
                
                model.Indicador = indicador;
                model.ListaEnvios = listadoEnvios;                
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
        /// Envia una declaracion
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="periodo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Enviar(int empresa, int periodo, string valor)
        {
            DeclaracionModel model = new DeclaracionModel();

            try
            {
                base.ValidarSesionJsonResult();
                
                servicioResarcimiento.EnviarDatosDeclaracion(empresa, periodo, valor, User.Identity.Name);
                model.ListaEnvios = servicioResarcimiento.ListarEnvios(empresa,periodo, ConstantesCalculoResarcimiento.EnvioTipoDeclaracion);
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