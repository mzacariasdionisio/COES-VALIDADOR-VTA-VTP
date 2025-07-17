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
    public class StockCombustibleController : Controller
    {
        /// <summary>
        /// Instancia del servicio asociado
        /// </summary>
        StockCombustibleAppServicio servicio = new StockCombustibleAppServicio();

        /// <summary>
        /// Pagina inicial de stock de combustibles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StockCombustibleModel model = new StockCombustibleModel();
            model.FechaInicio = DateTime.Now.AddDays(-7).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.ListaEmpresas = this.servicio.ObtenerEmpresasStock();
            model.ListaCombustible = this.servicio.ObtenerTipoCombustible();
            return View(model);
        }

        /// <summary>
        /// Permite obtener los grupos de una empresa
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerGrupos(int idEmpresa)
        {
            List<PrGrupoDTO> list = this.servicio.ObtenerGruposStock(idEmpresa);
            return Json(list);
        }

        /// <summary>
        /// Permite relizar la consulta de stock de combustible
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idGrupo"></param>
        /// <returns></returns>
        public PartialViewResult Listar(string fechaDesde, string fechaHasta, int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            StockCombustibleModel model = new StockCombustibleModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (!string.IsNullOrEmpty(fechaDesde))
            {
                fechaInicio = DateTime.ParseExact(fechaDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (!string.IsNullOrEmpty(fechaHasta))
            {
                fechaFin = DateTime.ParseExact(fechaHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            if (idEmpresa == null) idEmpresa = -1;
            if (idGrupo == null) idGrupo = -1;
            if (idTipoCombustible == null) idTipoCombustible = -1;

            model.ListaConsulta = this.servicio.ObtenerConsultaStock(fechaInicio, fechaFin, idEmpresa, idGrupo, idTipoCombustible);

            return PartialView(model);
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
        public JsonResult Exportar(string fechaDesde, string fechaHasta, int? idEmpresa, int? idGrupo, int? idTipoCombustible)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaDesde))
                {
                    fechaInicio = DateTime.ParseExact(fechaDesde, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaHasta))
                {
                    fechaFin = DateTime.ParseExact(fechaHasta, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                if (idEmpresa == null) idEmpresa = -1;
                if (idGrupo == null) idGrupo = -1;
                if (idTipoCombustible == null) idTipoCombustible = -1;

                List<MeMedicion1DTO> list = this.servicio.ObtenerConsultaStock(fechaInicio, fechaFin, idEmpresa, idGrupo,
                    idTipoCombustible).OrderBy(x => x.Medifecha).ToList();

                MedidorHelper.GenerarReporteStockCombustible(list, fechaDesde, fechaHasta);

                return Json(1);

            }
            catch (Exception)
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
            string file = NombreArchivo.ReporteStockCombustible;
            string app = Constantes.AppExcel;
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(fullPath, app, file);
        }

    }
}
