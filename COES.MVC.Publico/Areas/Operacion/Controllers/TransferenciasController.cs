using COES.MVC.Publico.Areas.Operacion.Helper;
using COES.MVC.Publico.Areas.Operacion.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.Operacion.Controllers
{
    public class TransferenciasController : Controller
    {
        /// <summary>
        /// Instancia de la clase de aplicación correspondiente
        /// </summary>
        ReporteCostoMarginalAppServicio servicio = new ReporteCostoMarginalAppServicio();
 
        /// <summary>
        /// Carga la pantalla inicial del reporte
        /// </summary>
        /// <returns></returns>
        public ActionResult CostosMarginales()
        {
            TransferenciaModel model = new TransferenciaModel();
            List<int> meses = new List<int>();
            List<int> anios = new List<int>();
            for (int i = 1; i <= 12; i++) meses.Add(i);

            int nroAnio = DateTime.Now.Year;
            int nroMes = DateTime.Now.Month;

            if (nroMes == 1)
            {
                nroAnio = nroAnio - 1;
                nroMes = 12;
            }
            else 
            {               
                nroMes = nroMes - 1;            
            }
                        
            for (int i = nroAnio; i >= 2005; i--) anios.Add(i);

            model.ListaMeses = meses;
            model.ListaAnios = anios;
            model.ListaBarras = this.servicio.ListarBarras();
            model.ListaBarrasDTR = this.servicio.ListarBarraDTR();

            model.Anio = nroAnio;
            model.Mes = nroMes;            

            return View(model);
        }

        /// <summary>
        /// Permite generar el formato horizontal
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int anio, int mes, int barra)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias;
                string file = OperacionHelper.ReporteCostosMarginales;
                int result = this.servicio.ExportarReporteCostoMarginales(anio, mes, barra, path, file);
                return Json(result);
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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.ReporteTransferencias + OperacionHelper.ReporteCostosMarginales;
            return File(fullPath, Constantes.AppExcel, OperacionHelper.ReporteCostosMarginales);
        }
    }
}
