using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Controllers;
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
    public class CambioEnvioController : BaseController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        GestionAdministradorAppServicio servGestionAdmin = new GestionAdministradorAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "CambioEnvioController";

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
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));
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

        /// <summary>
        /// Lista de Cambios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="fecha"></param>
        /// <param name="mes"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresas, int idFormato, string fecha, string mes, string semana)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            var formato = this.servFormato.GetByIdMeFormato(idFormato);

            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");

            var lista = this.servFormato.GetByCriteriaMeCambioenvios(sEmpresas, fechaProceso, idFormato, 0);

            switch (formato.Formatresolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    model.Columnas = 96;
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    model.Columnas = 48;
                    break;
                case ParametrosFormato.ResolucionHora:
                    model.Columnas = 24;
                    break;
                case ParametrosFormato.ResolucionDia:
                case ParametrosFormato.ResolucionSemana:
                case ParametrosFormato.ResolucionMes:
                    model.Columnas = 1;
                    break;
            }

            model.Resolucion = (int)formato.Formatresolucion;
            model.ListaCambioEnvio = lista;
            return PartialView(model);
        }
    }
}
