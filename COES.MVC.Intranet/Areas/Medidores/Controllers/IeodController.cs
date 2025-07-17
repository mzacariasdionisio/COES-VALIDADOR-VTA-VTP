using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Medidores.Helpers;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class IeodController : Controller
    {
        EjecutadoAppServicio servicio = new EjecutadoAppServicio();

        /// <summary>
        /// Permite mostrar la pantalla inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult CMgReal()
        {
            IeodModel model = new IeodModel();
            model.FechaInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Permite mostrar el listado de la consulta
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string fechaInicial, string fechaFinal)        
        {
            IeodModel model = new IeodModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;
            if (!string.IsNullOrEmpty(fechaInicial))
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);            
            if (!string.IsNullOrEmpty(fechaFinal))
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            model.ListaReporte = this.servicio.ObtenerConsultaCMgRealPorArea(fecInicio, fecFin);

            return PartialView(model);
        }

        /// <summary>
        /// Permite exportar a formato Excel el resultado de la consulta
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicial, string fechaFinal, int opcion)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;
                if (!string.IsNullOrEmpty(fechaInicial))
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFinal))
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);                
                string file = (opcion == 1) ? NombreArchivo.ReporteVerticalCMgReal : NombreArchivo.ReporteHorizontalCMgReal;
                
                if (opcion == 1)
                {
                    List<ReporteCMGRealDTO> list = this.servicio.ObtenerConsultaCMgRealPorArea(fecInicio, fecFin);
                    MedidorHelper.GenerarReporteVerticalCMgRealPorArea(list, file, fecInicio.ToString(Constantes.FormatoFecha),
                        fecFin.ToString(Constantes.FormatoFecha));
                }
                else { 
                     List<MeMedicion48DTO> list = this.servicio.ObtenerConsultaCMgReal(fecInicio, fecFin);
                     MedidorHelper.GenerarReporteHorizontalCMgRealPorArea(list, file, fecInicio.ToString(Constantes.FormatoFecha),
                         fecFin.ToString(Constantes.FormatoFecha), fecInicio);
                }

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
        public virtual ActionResult Descargar(int opcion)
        {
            string file = (opcion == 1) ? NombreArchivo.ReporteVerticalCMgReal : NombreArchivo.ReporteHorizontalCMgReal;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }
    }
}
