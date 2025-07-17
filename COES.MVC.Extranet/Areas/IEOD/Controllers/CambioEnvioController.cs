using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.IEOD.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.StockCombustibles;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class CambioEnvioController : BaseController
    {
        //
        // GET: /IEOD/CambioEnvio/

        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CambioEnvioController));
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
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesHard.IdFormatoFlujoTrans);
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoFlujoTrans));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesHard.IdFormatoDespacho));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesStockCombustibles.IdFormatoConsumo));
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
        public PartialViewResult Lista(string sEmpresas, int idFormato, string fInicio)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            var formato = this.servFormato.GetByIdMeFormato(idFormato);

            DateTime fechaIni = fInicio != null && fInicio.Trim() != string.Empty ? DateTime.ParseExact(fInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;
            //DateTime fechaFin = fFin != null && fFin.Trim() != string.Empty ? DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture) : DateTime.Now;

            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(1, "", "", fInicio, Constantes.FormatoFecha);

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
