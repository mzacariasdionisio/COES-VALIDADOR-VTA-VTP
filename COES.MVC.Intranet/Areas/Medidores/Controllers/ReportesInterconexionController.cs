using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Medidores;
using COES.MVC.Intranet.Areas.Medidores.Models;
using System.Globalization;
using COES.MVC.Intranet.Areas.Medidores.Helper;
using System.Configuration;
using COES.MVC.Intranet.Helper;


namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ReportesInterconexionController : Controller
    {
        MedidoresAppServicio logic = new MedidoresAppServicio();
        //
        // GET: /Medidores/ReportesInterconexion/


        public ActionResult IndexHistorico()
        {
            InterconexionModel model = new InterconexionModel();
            model.ListaEmpresas = logic.ObtenerEmpresasSEIN().Where(x => x.Tipoemprcodi == 1 && x.Emprcodi == 12).ToList();
            model.empresa = 12; //Rep
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListaHistorico(int ptomedicion, string fechaInicial, string fechaFinal, int pagina)
        {
            InterconexionModel model = new InterconexionModel();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            try
            {
                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                string resultado = this.logic.ObtenerConsultaHistoricaPagInterconexion(ptomedicion, fechaInicio, fechaFin, pagina);
                model.Resultado = resultado;
            }
            catch (Exception ex)
            {
                model.Resultado = ex.Message;
            }
            return PartialView(model);
        }

        /// <summary>
        /// Para paginar Listado Historico
        /// </summary>
        /// <param name="ptomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(int ptomedicion, string fechaInicial, string fechaFinal)
        {
            InterconexionModel model = new InterconexionModel();
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (fechaInicial != null)
            {
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (fechaFinal != null)
            {
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            int nroRegistros = logic.ObtenerTotalHistoricoInterconexion(ptomedicion, fechaInicio, fechaFin);
            nroRegistros = nroRegistros * 96 / 4;
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = 96;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }


        // exporta el reporte historico consultado a archivo excel
        [HttpPost]
        public JsonResult GenerarArchivoReporte(int ptomedicion, string fechaInicial, string fechaFinal)
        {
            int indicador = 1;
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            try
            {
                if (fechaInicial != null)
                {
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (fechaFinal != null)
                {
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                var list = logic.ObtenerHistoricoInterconexion(ptomedicion, fechaInicio, fechaFin);
                ExcelDocument.GenerarArchivoReporte(list, fechaInicio, fechaFin);
            }
            catch(Exception ex)
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite exportar a excel el reporte historico
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarReporte()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = NombreArchivo.NombreReporteHistorico;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteInterconexion] + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

    }
}
