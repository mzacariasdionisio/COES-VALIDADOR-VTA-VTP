using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.Medidores.Helpers;
using COES.MVC.Intranet.Areas.Medidores.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.General.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.Medidores.Controllers
{
    public class ConsolidadoController : Controller
    {
        /// <summary>
        /// Instancia de clase de aplicación
        /// </summary>
        RankingConsolidadoAppServicio servicio = new RankingConsolidadoAppServicio();
        ParametroAppServicio servParametro = new ParametroAppServicio();

        /// <summary>
        /// Muestra la pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConsolidadoModel model = new ConsolidadoModel();
            model.FechaDesde = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            model.FechaHasta = DateTime.Now.ToString(Constantes.FormatoMesAnio);
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoMesAnio);
            model.ListaTipoGeneracion = this.servicio.ListaTipoGeneracion();
            model.ListaTipoEmpresas = this.servicio.ListaTipoEmpresas();
            model.ListaNormativa = this.ListarNormativaMaximaDemanda();

            return View(model);
        }

        private List<Normativa> ListarNormativaMaximaDemanda()
        {
            List<Normativa> lista = new List<Normativa>();

            List<SiParametroValorDTO> listaParam = this.servParametro.ListSiParametroValorByIdParametro(ConstantesParametro.IdParametroRangoPeriodoHP);
            var listaRangoPeriodoHP = this.servParametro.GetListaParametroRangoPeriodoHP(listaParam, null).Where(x => x.Estado == ConstantesParametro.EstadoActivo || x.Estado == ConstantesParametro.EstadoBaja).ToList();
            listaRangoPeriodoHP = listaRangoPeriodoHP.OrderBy(x => x.FechaInicio).ToList();

            foreach (var reg in listaRangoPeriodoHP)
            {
                if (reg.Normativa.Length > 0)
                {
                    int pos = reg.Normativa.IndexOf(":");

                    Normativa n = new Normativa();
                    n.DescripcionFull = reg.Normativa;
                    if (pos != -1)
                    {
                        n.Nombre = (reg.Normativa.Substring(0, pos + 1)).Trim();
                        n.Descripcion = (reg.Normativa.Substring(pos + 1, reg.Normativa.Length - pos - 1)).Trim();
                    }
                    else
                    {
                        n.Nombre = string.Empty;
                        n.Descripcion = reg.Normativa;
                    }

                    lista.Add(n);
                }
            }

            return lista;
        }

        /// <summary>
        /// Permite cargar las empresas pora los tipos seleccionados
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Empresas(string tiposEmpresa)
        {
            ConsolidadoModel model = new ConsolidadoModel();
            model.ListaEmpresas = this.servicio.ObteneEmpresasPorTipo(tiposEmpresa);

            return PartialView(model);
        }


        /// <summary>
        /// Permite obtener la consulta en base a los criterios de búsqueda
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Consulta(string fechaInicio, string fechaFin, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            List<ItemReporteMedicionDTO> resultadoTotal = new List<ItemReporteMedicionDTO>();
            try
            {
                if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
                {
                    int mesIni = Int32.Parse(fechaInicio.Substring(0, 2));
                    int anioIni = Int32.Parse(fechaInicio.Substring(3, 4));

                    int mesFin = Int32.Parse(fechaFin.Substring(0, 2));
                    int anioFin = Int32.Parse(fechaFin.Substring(3, 4));

                    DateTime fecIni = new DateTime(anioIni, mesIni, 1);
                    DateTime fecFin = new DateTime(anioFin, mesFin, Tools.ObtenerNroDias(anioFin, mesFin));

                    if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                    if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                    List<MedicionReporteDTO> list = (new ReporteMedidoresAppServicio()).ObtenerReporteConsolidado(resultadoTotal, fecIni, fecFin,
                        tiposEmpresa, empresas, tiposGeneracion, Constantes.ParametroDefecto, central); 

                    return Json(MedidorHelper.ObtenerReporteConsolidado(list, resultadoTotal,1));
                }
           
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }


        /// <summary>
        /// Generar el formato excel
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tiposEmpresa"></param>
        /// <param name="empresas"></param>
        /// <param name="tiposGeneracion"></param>
        /// <param name="central"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(string fechaInicio, string fechaFin, string tiposEmpresa, string empresas, string tiposGeneracion,
            int central)
        {
            List<ItemReporteMedicionDTO> resultadoTotal = new List<ItemReporteMedicionDTO>();
            try
            {
                if (!string.IsNullOrEmpty(fechaInicio) && !string.IsNullOrEmpty(fechaFin))
                {
                    int mesIni = Int32.Parse(fechaInicio.Substring(0, 2));
                    int anioIni = Int32.Parse(fechaInicio.Substring(3, 4));

                    int mesFin = Int32.Parse(fechaFin.Substring(0, 2));
                    int anioFin = Int32.Parse(fechaFin.Substring(3, 4));

                    DateTime fecIni = new DateTime(anioIni, mesIni, 1);
                    DateTime fecFin = new DateTime(anioFin, mesFin, Tools.ObtenerNroDias(anioFin, mesFin));

                    if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                    if (string.IsNullOrEmpty(tiposGeneracion)) tiposGeneracion = Constantes.ParametroDefecto;

                    List<MedicionReporteDTO> list = (new ReporteMedidoresAppServicio()).ObtenerReporteConsolidado(resultadoTotal,fecIni, fecFin,
                        tiposEmpresa, empresas, tiposGeneracion, Constantes.ParametroDefecto, central);

                    MedidorHelper.GenerarReporteExcelConsolidado(list, resultadoTotal);
                }

                return Json(1);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = NombreArchivo.ReporteConsolidoMensual;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }
    }
}
