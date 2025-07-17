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
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class AmpliacionController : BaseController
    {
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        GestionAdministradorAppServicio servGestionAdmin = new GestionAdministradorAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "AmpliacionController";

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
            model.MesInicio = new DateTime(DateTime.Now.Year, 1, 1).ToString("MM yyyy");
            model.MesFin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM yyyy");
            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));
        }



        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresas, string mesIni, string mesFin, string idformato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            DateTime fechaProcesoIni = EPDate.GetFechaIniPeriodo(5, mesIni, "", "", "");
            DateTime fechaProcesoFin = EPDate.GetFechaIniPeriodo(5, mesFin, "", "", "");
            DateTime fechaIni = fechaProcesoIni;
            DateTime fechaFin = fechaProcesoFin.AddMonths(1).AddDays(-1);

            var lista = servFormato.ObtenerListaMultipleMeAmpliacionfechas(fechaIni, fechaFin, sEmpresas, idformato);
            model.ListaAmpliacion = lista;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene el model para pintar el popup de ingreso de la nueva ampliacion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult AgregarAmpliacion()
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva);

            model.ListaFormato = new List<MeFormatoDTO>();
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotActiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaCentralPotReactiva));
            model.ListaFormato.Add(servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoCargaServAuxPotActiva));

            model.Fecha = DateTime.Now.AddMonths(-1).ToString("MM yyyy"); ;
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            model.DiaMes = DateTime.Now.Day * -1;
            return PartialView(model);
        }


        /// <summary>
        /// Graba la Ampliacion ingresada.
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="empresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarAmpliacion(string fecha, int hora, int empresa, int idformato)
        {
            base.ValidarSesionUsuario();
            int resultado = 1;
            DateTime fechaEnvio = EPDate.GetFechaIniPeriodo(5, fecha, "", "", "");
            
            MeAmpliacionfechaDTO ampliacion = new MeAmpliacionfechaDTO();
            ampliacion.Lastuser = User.Identity.Name;
            ampliacion.Lastdate = DateTime.Now;
            ampliacion.Amplifecha = fechaEnvio;
            ampliacion.Formatcodi = idformato;
            ampliacion.Emprcodi = empresa;
            ampliacion.Amplifechaplazo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMinutes(hora * 30);
            try
            {
                var reg = servFormato.GetByIdMeAmpliacionfecha(fechaEnvio, empresa, idformato);
                if (reg == null)
                {
                    this.servFormato.SaveMeAmpliacionfecha(ampliacion);
                }
                else
                {
                    this.servFormato.UpdateMeAmpliacionfecha(ampliacion);
                }
            }
            catch
            {
                resultado = 0;
            }
            return Json(resultado);
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
        /// Obtiene las empresas segun formato para el popUp AgregarAmpliacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresasPopUp(int idFormato)
        {
            GestionAdministradorModel model = new GestionAdministradorModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
            return PartialView(model);
        }
    }
}
