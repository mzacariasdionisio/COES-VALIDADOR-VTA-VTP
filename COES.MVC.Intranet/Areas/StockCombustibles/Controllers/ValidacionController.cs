using COES.MVC.Intranet.Areas.StockCombustibles.Helper;
using COES.MVC.Intranet.Areas.StockCombustibles.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.StockCombustibles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

namespace COES.MVC.Intranet.Areas.StockCombustibles.Controllers
{
    public class ValidacionController : Controller
    {
        //
        // GET: /StockCombustibles/Validacion/

        FormatoMedicionAppServicio servFormato = new FormatoMedicionAppServicio();
        StockCombustiblesAppServicio servicio = new StockCombustiblesAppServicio();

        public ActionResult Index()
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            model.ListaEmpresas = servFormato.GetListaEmpresaFormato(ConstantesStockCombustibles.IdFormatoConsumo);//logicGeneral.ObtenerEmpresasHidro();
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaCombo = servicio.ListaFormatos().OrderBy(x => x.Formatnombre).ToList();
            return View(model);
        }

        /// <summary>
        /// Obtiene la lista de validaciones.
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string sEmpresas, string fecha, int idformato)
        {
            StockCombustiblesModel model = new StockCombustiblesModel();
            DateTime fechaDate = DateTime.Now;

            if (fecha != null)
            {
                fechaDate = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            var listaEmpresas = servFormato.ListMeValidacions(fechaDate, idformato);
            model.ListaValidacion = listaEmpresas;
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

        [HttpPost]
        public JsonResult FinalizarValidacion(string empresas, int estado, string fecha, int formato)
        {
            DateTime fechaDate = DateTime.Now;
            int resultado = 0;
            if (fecha != null)
            {
                fechaDate = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            string usuario = User.Identity.Name;
            try
            {
                servFormato.ValidarEnvioEmpresas(fechaDate, formato, usuario, empresas, estado);
                resultado = 1;
            }
            catch (Exception ex)
            {

            }
            var jsonData = new
            {
                Resultado = resultado

            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }


    }

}
