using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using COES.Servicios.Aplicacion.CortoPlazo.Helper;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ValidacionCMController : BaseController
    {
        
        private readonly ReprocesoAppServicio servicio = new ReprocesoAppServicio();

        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(CortoPlazoAppServicio));
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

        #region Métodos 

        /// <summary>
        /// Index Compromiso Hidráulico
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {           
            ValidacionCMModel model = new ValidacionCMModel();
            model.Fecha = DateTime.Now.ToString(ConstantesAppServicio.FormatoFecha);

            return View(model);
        }

        /// <summary>
        /// Devuelve el listado de validaciones
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListarValidaciones(string fecha, string option)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
            string file = ConstantesCortoPlazo.ReporteValidacionCM;
            bool boolExportar = (option == Constantes.SI) ? true : false;

            return Json(servicio.ObtenerListadoValidacionesPorFecha(DateTime.ParseExact(
                fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture), boolExportar, path, file));
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarValidacionCM(string fecha)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteValidacionCM;
            return File(fullPath, Constantes.AppExcel, string.Format(ConstantesCortoPlazo.ReporteValidacionCM, fecha));
        }
        #endregion
    }
}