using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ValidacionController : BaseController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        GestionAdministradorAppServicio servGestionAdmin = new GestionAdministradorAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "ValidacionController";

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

        public ActionResult Index()
        {
            GestionAdministradorModel model = new GestionAdministradorModel();

            GenerarGestionAdministradorModel(model);
            return View(model);
        }

        /// <summary>
        /// Inicializar model
        /// </summary>
        /// <param name="model"></param>
        private void GenerarGestionAdministradorModel(GestionAdministradorModel model)
        {
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);
            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");
            model.ListaEstadoEnvio = servFormato.ListMeEstadoenvios();
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));
        }

        /// <summary>
        /// Obtiene la lista de validaciones.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresas, string mes, int idformato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");

            var listaEmpresas = servFormato.ListMeValidacions(fechaProceso, idformato);
            model.ListaValidacion = listaEmpresas;
            return PartialView(model);
        }
        
        /// <summary>
        /// Finalizar validacion
        /// </summary>
        /// <param name="empresas"></param>
        /// <param name="estado"></param>
        /// <param name="mes"></param>
        /// <param name="formato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FinalizarValidacion(string empresas, int estado, string mes, int formato)
        {
            int resultado = 0;
            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");

            string usuario = User.Identity.Name;
            try
            {
                this.servFormato.ValidarEnvioEmpresas(fechaProceso, formato, usuario, empresas, estado);
                resultado = 1;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
            }
            var jsonData = new
            {
                Resultado = resultado

            };
            return Json(jsonData);
        }

        /// <summary>
        /// Obtiene las empresas segun formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idFormato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }
    }
}
