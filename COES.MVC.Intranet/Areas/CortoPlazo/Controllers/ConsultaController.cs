using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ConsultaController : Controller
    {
              
        /// <summary>
        /// Muestra la pagina principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConsultaModel model = new ConsultaModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }


        /// <summary>
        /// Permite generar el formato de exportacion de datos
        /// </summary>        
        [HttpPost]
        public JsonResult Exportar(int tipo, string fechaInicio, string fechaFin)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
                string file = ConstantesCortoPlazo.ReporteConsultaDatos;
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicio))
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFin))
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
                servicio.ObtenerConsultaDatos(tipo, fecInicio, fecFin, path, file);                
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteConsultaDatos;
            return File(fullPath, Constantes.AppExcel, ConstantesCortoPlazo.ReporteConsultaDatos);
        }
    }
}
