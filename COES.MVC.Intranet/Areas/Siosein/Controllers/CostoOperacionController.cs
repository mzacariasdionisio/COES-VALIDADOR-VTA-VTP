using COES.MVC.Intranet.Areas.Siosein.Helper;
using COES.MVC.Intranet.Areas.Siosein.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Migraciones;
using COES.Servicios.Aplicacion.Migraciones.Helper;
using COES.Servicios.Aplicacion.SIOSEIN;
using log4net;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Siosein.Controllers
{
    public class CostoOperacionController : BaseController
    {
        SIOSEINAppServicio servicio = new SIOSEINAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CostoOperacionController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Protected de log de errores page
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
        /// pagina inicio costos de operacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();

            SioseinModel model = new SioseinModel();
            DateTime finicio = DateTime.Now;
            model.FechaInicio = finicio.ToString(ConstantesAppServicio.FormatoFecha);

            model.ListaEmpresas = servicio.GetListaCriteria(ConstantesSiosein.TipoEmpresa)
                .Select(x => new ListaSelect { id = x.Emprcodi, text = x.Emprnomb.Trim() }).ToList();

            return View(model);
        }

        /// <summary>
        /// carga lista de costos de operacion
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicio"></param>
        /// <returns></returns>
        public JsonResult CargarListaCostosOperacion(string idEmpresa, string fechaInicio, string tipoDatoMostrar)
        {
            SioseinModel model = new SioseinModel();
            DateTime fechaInicial = DateTime.ParseExact(fechaInicio, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            //cálculo
            var servMigra = new MigracionesAppServicio();
            servMigra.Load_Dispatch(fechaInicial, fechaInicial, 6, idEmpresa, true, true, true, null, out CDespachoGlobal regCDespacho);
            decimal costoTotalOperacion = regCDespacho.ListaAllMe1[0].H1.GetValueOrDefault(0);

            //salidas
            model.Resultado = UtilCdispatch.ListarCostoOperacionDiarioHtml(fechaInicial, regCDespacho.ListaAllCosto, regCDespacho.ListaAllPotencia, tipoDatoMostrar);
            model.CostoTotalOperacion = Math.Round(costoTotalOperacion, 0);

            var jsonResult = Json(model, JsonRequestBehavior.DenyGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

    }
}
