using COES.MVC.Extranet.Areas.Medidores.Models;
using COES.MVC.Extranet.Areas.Medidores.Helper;
using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using COES.Servicios.Aplicacion.Interconexiones;

namespace COES.MVC.Extranet.Areas.Medidores.Controllers
{
    public class ReportesInterconexionController : Controller
    {
        InterconexionesAppServicio logic = new InterconexionesAppServicio();

        public ActionResult Index()
        {
            InterconexionModel model = new InterconexionModel();
            model.ListaEmpresas = InterconexionHelper.ListarEmpresaInterconexion();
            model.Empresa = 12;
            model.Estado = 0;
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult Lista(int empresa, int estado,string fechaInicial, string fechaFinal, int nroPagina)
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
            fechaFin = fechaFin.AddDays(1);

            model.ListaEnvios = logic.GetListaMultipleMeEnvios(empresa.ToString(), ConstantesMedidores.LECTURA.ToString(), ConstantesMedidores.IdFormato.ToString(),
                estado.ToString(), fechaInicio, fechaFin, nroPagina, Constantes.PageSize);
                //logic.GetListaEnvioPagInterconexion(empresa,estado, fechaInicio, fechaFin,nroPagina,Constantes.PageSize);
            return PartialView(model);
        }

        public ActionResult IndexHistorico()
        {
            InterconexionModel model = new InterconexionModel();
            model.ListaEmpresas = InterconexionHelper.ListarEmpresaInterconexion();
            model.Empresa = 12;
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        [HttpPost]
        public PartialViewResult ListaHistorico(int ptomedicion, string fechaInicial, string fechaFinal,int pagina)
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
        public PartialViewResult Paginado(int ptomedicion,string fechaInicial, string fechaFinal)
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

        [HttpPost]
        public PartialViewResult PaginadoEnvio(string fechaInicial, string fechaFinal, int empresa,int estado)
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
            int nroRegistros = logic.TotalListaMultipleMeEnvios(empresa.ToString(), ConstantesMedidores.LECTURA.ToString(), ConstantesMedidores.IdFormato.ToString(),
                estado.ToString(), fechaInicio, fechaFin);
                //TotalEnvio(fechaInicio, fechaFin,empresa);
            if (nroRegistros > Constantes.NroPageShow)
            {
                int pageSize = Constantes.PageSize;
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
                //string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteInterconexion].ToString();
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
                logic.GenerarArchivoReporte(list, fechaInicio, fechaFin, ruta + NombreArchivoInterconexiones.PlantillaReporteHistorico,
                    ruta + NombreArchivoInterconexiones.NombreReporteHistorico);
                
            }
            catch
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
            nombreArchivo = NombreArchivoInterconexiones.NombreReporteHistorico;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpGet]
        public virtual ActionResult ExportarReporteEnvio()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = NombreArchivoInterconexiones.NombreReporteEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpPost]
        public JsonResult GenerarArchivoReporteEnvio(int empresa,int estado, string fechaInicial, string fechaFinal)
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
                fechaFin = fechaFin.AddDays(1);
                var list = logic.GetListaMultipleMeEnviosXLS(empresa.ToString(), ConstantesMedidores.LECTURA.ToString(), ConstantesMedidores.IdFormato.ToString(),
                    estado.ToString(), fechaInicio, fechaFin);
                    //GetByCriteriaExtArchivos(empresa,estado ,fechaInicio, fechaFin);
                string ruta =  AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderReporte;
                ExcelDocument.GenerarArchivoEnvio(list, fechaInicio, fechaFin,ruta);
            }
            catch
            {
                indicador = -1;
            }
            return Json(indicador);
        }

        /// <summary>
        /// Permite descargar el Reporte Envio
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivoEnvio(string archivo)
        {
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesMedidores.FolderUpload;
            string fullPath = ruta + archivo;
            return File(fullPath, Constantes.AppExcel, archivo);
        }

    }
}
