using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaBarras.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.DemandaBarras.Controllers
{
    public class ConsultaController : Controller
    {
        /// <summary>
        /// Clase para acceso a los datos y bl
        /// </summary>
        ConsultaMedidoresAppServicio servicio = new ConsultaMedidoresAppServicio();

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            MedidoresDistribucionModel model = new MedidoresDistribucionModel();

            model.ListaEmpresas = (new GeneralAppServicio()).ListarEmpresasPorTipo(2.ToString());     
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);

            return View(model);
        }

    
        /// <summary>
        /// Permite listar los registros de medidores de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaInicial, string fechaFinal, string empresas, int nroPagina, int frecuencia)
        {
            MedidoresDistribucionModel model = new MedidoresDistribucionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;

            List<MeMedicion96DTO> sumatoria = new List<MeMedicion96DTO>();               
            
            model.ListaDatos = this.servicio.ObtenerConsultaMedDistribucion(fecInicio, fecFin,  empresas,              
                nroPagina, Constantes.PageSizeMedidores, out sumatoria);

            model.EntidadTotal = sumatoria;
            model.IndicadorMediaHora = frecuencia;
            
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(string fechaInicial, string fechaFinal, string empresas)
        {
            MedidoresDistribucionModel model = new MedidoresDistribucionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaInicial))
            {
                fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaFinal))
            {
                fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;            

            int nroRegistros = this.servicio.ObtenerNroRegistrosMedDistribucion(fecInicio, fecFin, empresas);

            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSizeMedidores;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
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
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string empresas,  int tipo, int frecuencia)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;               

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras];
                string file = string.Empty;

                if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
                if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;                
                
                this.servicio.GenerarArchivoExportacionDistribucion(fecInicio, fecFin, empresas, path, file, tipo, frecuencia);

                return Json("1");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar(int tipo)
        {
            string file = string.Empty;
            string app = Constantes.AppExcel;

            if (tipo == 1) file = NombreArchivo.ReporteMedidoresHorizontal;
            if (tipo == 2) file = NombreArchivo.ReporteMedidoresVertical;
           
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + file;
            return File(fullPath, app, file);
        }

        /// <summary>
        /// Valida la selección de datos de exportación
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ValidarExportacion(int formato, string fechaInicial, string fechaFinal, string parametros)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicial))
                {
                    fecInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFinal))
                {
                    fecFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                TimeSpan ts = fecFin.Subtract(fecInicio);

                //if (ts.TotalDays > 92)
                if (ts.TotalDays > 365)
                {
                    return Json(2);
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }



    }
}
