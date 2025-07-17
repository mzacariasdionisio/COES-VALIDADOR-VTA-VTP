using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.StockCombustibles;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.General;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Controllers
{
    public class AmpliacionController : BaseController
    {
        //
        // GET: /StockCombustibles/Ampliacion/
        private StockCombustiblesAppServicio logic;
        private GeneralAppServicio logicGeneral;
        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        public AmpliacionController()
        {
            logic = new StockCombustiblesAppServicio();
            logicGeneral = new GeneralAppServicio();
        }

        public ActionResult Index()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);//logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.AddDays(-15).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaCombo = logic.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de todas las ampliaciones de plazo.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresas, string fIni, string fFin, string idformato)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            DateTime fechaIni = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (fIni != null)
            {
                fechaIni = DateTime.ParseExact(fIni, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fFin != null)
            {
                fechaFin = DateTime.ParseExact(fFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }


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
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);
            //int idOrigen = ConstantesHidrologia.IdOrigenHidro;
            //int idModulo = ConstantesHidrologia.IdModulo;
            //model.ListaFormato = logic.GetByModuloLecturaMeFormatos(idModulo, -1, -1);
            //model.ListaLectura = logic.ListMeLecturas().Where(x => x.Origlectcodi == idOrigen).ToList();
            model.ListaCombo = logic.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            model.Fecha = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaPlazo = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.HoraPlazo = DateTime.Now.Hour * 2 + 1;
            return PartialView(model);
        }

        /// <summary>
        /// Obtiene las empresas segun formato
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        public PartialViewResult CargarEmpresas(int idFormato)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
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
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(idFormato);
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
            DateTime fechaEnvio = DateTime.Now;

            if (fecha != null)
            {
                fechaEnvio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

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

    }
}
