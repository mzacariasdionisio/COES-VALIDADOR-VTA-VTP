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
    public class CalidadProductoController : BaseController
    {
        /// <summary>
        /// Instancia del servicio de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>        
        CalidadProductoAppServicio servicio = new CalidadProductoAppServicio();

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
            CalidadProductoModel model = new CalidadProductoModel();
                  
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).
               Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            model.IndicadorEmpresa = (model.ListaEmpresas.Count > 1) ? Constantes.SI : Constantes.NO;
            if (model.ListaEmpresas.Count > 0)
            {
                model.Emprcodi = model.ListaEmpresas[0].Emprcodi;
                model.Emprnombre = model.ListaEmpresas[0].Emprnomb;
            }
            model.Anio = DateTime.Now.Year;
            model.Mes = DateTime.Now.Month;
            model.ListaAnio = this.servicio.ListarAnios();

            return View(model);
        }


        /// <summary>
        /// Permite mostrar el dialogo de cambio de empresa
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresa()
        {
            CalidadProductoModel model = new CalidadProductoModel();
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
        public PartialViewResult Listado(int empresa, int anio, int mes, string buscar)
        {
            CalidadProductoModel model = new CalidadProductoModel();
            model.ListaEventos = this.servicio.ObtenerEventosPorSuministrador(empresa, anio, mes, buscar);
            return PartialView(model);
        }

    }
}