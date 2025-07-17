using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.DemandaBarras.Helper;
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
    public class CumplimientoController : Controller
    {
        /// <summary>
        /// Formato consultado
        /// </summary>
        private int IdFormato = 4;

        /// <summary>
        /// Carga inicial de la página
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            CumplimientoDistribucionModel model = new CumplimientoDistribucionModel();

            model.ListaEmpresas = (new GeneralAppServicio()).ListarEmpresasPorTipo(2.ToString());
            model.FechaInicio = DateTime.Now.AddMonths(-1).ToString(Constantes.FormatoMesAnio);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoMesAnio);

            return View(model);
        }

        /// <summary>
        /// Permite listar los registros de medidores de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Lista(string fechaInicial, string fechaFinal, string empresas)
        {
            CumplimientoDistribucionModel model = new CumplimientoDistribucionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;
            
            int mesInicio = Int32.Parse(fechaInicial.Substring(0, 2));
            int anioInicio = Int32.Parse(fechaInicial.Substring(3, 4));
            int mesFin = Int32.Parse(fechaFinal.Substring(0, 2));
            int anioFin = Int32.Parse(fechaFinal.Substring(3, 4));

            fecInicio = new DateTime(anioInicio, mesInicio, 1);
            fecFin = new DateTime(anioFin, mesFin, 1).AddMonths(1).AddDays(-1);
            

            if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
            model.ListaDatos = (new ConsultaMedidoresAppServicio()).ObtenerReporteCumplimiento(empresas, this.IdFormato, fecInicio, fecFin);

            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el listado de envios
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Envios(int idEmpresa, string fechaInicial, string fechaFinal)
        {
            CumplimientoDistribucionModel model = new CumplimientoDistribucionModel();

            DateTime fecInicio = DateTime.Now;
            DateTime fecFin = DateTime.Now;

            int mesInicio = Int32.Parse(fechaInicial.Substring(0, 2));
            int anioInicio = Int32.Parse(fechaInicial.Substring(3, 4));
            int mesFin = Int32.Parse(fechaFinal.Substring(0, 2));
            int anioFin = Int32.Parse(fechaFinal.Substring(3, 4));

            fecInicio = new DateTime(anioInicio, mesInicio, 1);
            fecFin = new DateTime(anioFin, mesFin, 1).AddMonths(1).AddDays(-1);


            model.ListaDatos = (new ConsultaMedidoresAppServicio()).ObtenerEnviosCumplimiento(idEmpresa, this.IdFormato, fecInicio, fecFin);

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
        public JsonResult Exportar(string fechaInicial, string fechaFinal, string empresas)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                int mesInicio = Int32.Parse(fechaInicial.Substring(0, 2));
                int anioInicio = Int32.Parse(fechaInicial.Substring(3, 4));
                int mesFin = Int32.Parse(fechaFinal.Substring(0, 2));
                int anioFin = Int32.Parse(fechaFinal.Substring(3, 4));

                fecInicio = new DateTime(anioInicio, mesInicio, 1);
                fecFin = new DateTime(anioFin, mesFin, 1).AddMonths(1).AddDays(-1);                

                if (string.IsNullOrEmpty(empresas)) empresas = Constantes.ParametroDefecto;
                List<MeEnvioDTO> list = (new ConsultaMedidoresAppServicio()).ObtenerReporteCumplimiento(empresas, this.IdFormato, fecInicio, fecFin);

                string file = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + 
                    ConstantesDemandaBarras.NombreExportacionCumplimiento;

                DemandaBarrasHelper.GenerarReporteCumplimiento(list, file);

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
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteDemandaBarras] + ConstantesDemandaBarras.NombreExportacionCumplimiento;
            return File(fullPath, Constantes.AppExcel, ConstantesDemandaBarras.NombreExportacionCumplimiento);
        }
    }
}
