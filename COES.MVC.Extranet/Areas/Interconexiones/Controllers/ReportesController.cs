using COES.MVC.Extranet.Areas.Interconexiones.Models;
using COES.MVC.Extranet.Areas.Interconexiones.Helper;
using COES.MVC.Extranet.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.MVC.Extranet.Controllers;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Extranet.Areas.Interconexiones.Controllers
{
    public class ReportesController : Controller
    {
        InterconexionesAppServicio logic = new InterconexionesAppServicio();
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        //
        // GET: /Interconexiones/Reportes/
        /// <summary>
        /// Index para reporte de envios
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Where(
                x => x.EMPRCODI == ConstantesInterconexiones.IdEmpresaInterconexion).Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList();
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }
            model.Estado = 0;
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }
        /// <summary>
        /// Lista de envios de interconexion
        /// </summary>
        /// <param name="empresa"></param>
        /// <param name="estado"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="nroPagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(int empresa, int estado, string fechaInicial, string fechaFinal, int nroPagina)
        {
            InterconexionesModel model = new InterconexionesModel();
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

            model.ListaEnvios = logic.GetListaMultipleMeEnvios(empresa.ToString(), ConstantesInterconexiones.IdLectura.ToString(), ConstantesInterconexiones.IdFormato.ToString(),
                estado.ToString(), fechaInicio, fechaFin, nroPagina, Constantes.PageSize);
            //logic.GetListaEnvioPagInterconexion(empresa,estado, fechaInicio, fechaFin,nroPagina,Constantes.PageSize);
            return PartialView(model);
        }

        /// <summary>
        /// Index para reporte historico
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexHistorico()
        {
            InterconexionesModel model = new InterconexionesModel();
            model.ListaEmpresas = this.seguridad.ObtenerEmpresasPorUsuario(User.Identity.Name).Where(
                x => x.EMPRCODI == ConstantesInterconexiones.IdEmpresaInterconexion).Select(x => new SiEmpresaDTO { Emprcodi = x.EMPRCODI, Emprnomb = x.EMPRNOMB }).ToList(); ;
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.IdMedidor = 0;
            model.ListaMedidor = this.logic.GetListaMedidorApp();
            return View(model);
        }
        /// <summary>
        /// LIsta de informacion historica
        /// </summary>
        /// <param name="ptomedicion"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaHistorico(int ptomedicion, int medidor, string fechaInicial, string fechaFinal, int pagina)
        {
            InterconexionesModel model = new InterconexionesModel();
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
                string resultado = this.logic.ObtenerConsultaHistoricaPagInterconexion(ptomedicion, medidor, fechaInicio, fechaFin, pagina);
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
            InterconexionesModel model = new InterconexionesModel();
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
            int nroPaginas = logic.ObtenerTotalHistoricoInterconexion(ptomedicion, fechaInicio, fechaFin);
            int nroRegistros = nroPaginas * 96;
            if (nroRegistros > Constantes.NroPageShow)
            {
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }
            return PartialView(model);
        }

        [HttpPost]
        public PartialViewResult PaginadoEnvio(string fechaInicial, string fechaFinal, int empresa, int estado)
        {
            InterconexionesModel model = new InterconexionesModel();
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
            int nroRegistros = logic.TotalListaMultipleMeEnvios(empresa.ToString(), ConstantesInterconexiones.IdLectura.ToString(), ConstantesInterconexiones.IdFormato.ToString(),
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
        public JsonResult GenerarArchivoReporte(int ptomedicion, int medidor, string fechaInicial, string fechaFinal)
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

                var list = logic.ObtenerHistoricoInterconexion(ptomedicion, medidor, fechaInicio, fechaFin);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.FolderReporte;
                logic.GenerarArchivoReporte(1, list, medidor, fechaInicio, fechaFin, ruta + NombreArchivoInterconexiones.PlantillaReporteHistorico,
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
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }

        [HttpGet]
        public virtual ActionResult ExportarReporteEnvio()
        {
            string nombreArchivo = string.Empty;
            nombreArchivo = NombreArchivoInterconexiones.NombreReporteEnvio;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.FolderReporte;
            string fullPath = ruta + nombreArchivo;
            return File(fullPath, Constantes.AppExcel, nombreArchivo);
        }
        [HttpPost]
        public JsonResult GenerarArchivoReporteEnvio(int empresa, int estado, string fechaInicial, string fechaFinal)
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
                var list = logic.GetListaMultipleMeEnviosXLS(empresa.ToString(), ConstantesInterconexiones.IdLectura.ToString(), ConstantesInterconexiones.IdFormato.ToString(),
                    estado.ToString(), fechaInicio, fechaFin);
                //GetByCriteriaExtArchivos(empresa,estado ,fechaInicio, fechaFin);
                string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.FolderReporte;
                ExcelDocument.GenerarArchivoEnvio(list, fechaInicio, fechaFin, ruta);
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
            string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.FolderUpload;
            string fullPath = ruta + archivo;
            return File(fullPath, Constantes.AppExcel, archivo);
        }

    }
}
