using COES.Framework.Base.Tools;
using COES.MVC.Publico.Areas.DemandaBarras.Models;
using COES.MVC.Publico.Helper;
using COES.Servicios.Aplicacion.Mediciones;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Publico.Areas.DemandaBarras.Controllers
{
    public class ConsultaController : Controller
    {
                
        /// <summary>
        /// Instancia a la clase servicio
        /// </summary>
        DemandaBarrasAppServicio servicio = new DemandaBarrasAppServicio();

        /// <summary>
        /// Pantalla inicial del modulo
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public ActionResult Index(int tipo)
        {
            ConsultaModel model = new ConsultaModel();
            model.ListaEmpresas = this.servicio.ObtenerEmpresaPorTIpo(tipo);
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaISO);

            model.HistoricoDesde = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.HistoricoHasta = DateTime.Now.ToString(Constantes.FormatoFechaISO);
            model.DiarioDesde = DateTime.Now.AddDays(1).ToString(Constantes.FormatoFechaISO);
            model.DiarioHasta = DateTime.Now.AddDays(2).ToString(Constantes.FormatoFechaISO);
            model.SemanalDesde = EPDate.f_fechafinsemana(DateTime.Now.Date).AddDays(1).ToString(Constantes.FormatoFechaISO);
            model.SemanalHasta = EPDate.f_fechafinsemana(DateTime.Now.Date).AddDays(8).ToString(Constantes.FormatoFechaISO);
            model.Tipo = tipo;

            return View(model);
        }

        /// <summary>
        /// Permite consultar los datos de demanda en barras
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Grilla(int lectcodi, string empresas, string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;
                if (!string.IsNullOrEmpty(fechaInicio))
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFin))
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                int indicador = 0;
                if (fecInicio < new DateTime(2020, 7, 1))
                {
                    if (lectcodi == 103) lectcodi = 45;
                    if (lectcodi == 110) lectcodi = 46;
                    if (lectcodi == 102) lectcodi = 47;
                }

                string[][] data = this.servicio.ObtenerReporte(lectcodi, empresas, fecInicio, fecFin, out indicador);

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var result = new ContentResult();
                serializer.MaxJsonLength = Int32.MaxValue;                
                result.ContentType = "application/json";

                if (indicador == 1)
                {                  
                    result.Content = serializer.Serialize(data);
                }
                else
                {
                    result.Content = serializer.Serialize(0);                  
                }

                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Generar el archivo a exportar
        /// </summary>
        /// <param name="lectcodi"></param>
        /// <param name="empresas"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(int lectcodi, string empresas, string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;
                if (!string.IsNullOrEmpty(fechaInicio))
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFechaISO, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(fechaFin))
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFechaISO, CultureInfo.InvariantCulture);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = NombreArchivo.ReporteDemandaBarras;
                string emprnomb = "REPORTE";

                if (!string.IsNullOrEmpty(empresas))
                {
                    List<int> idsEmpresas = empresas.Split(',').Select(int.Parse).ToList();
                    if (idsEmpresas.Count == 1)
                    {
                        emprnomb = this.servicio.ObtenerNombreEmpresa(idsEmpresas[0]);
                    }
                }
                if (fecInicio < new DateTime(2020, 7, 1))
                {
                    if (lectcodi == 103) lectcodi = 45;
                    if (lectcodi == 110) lectcodi = 46;
                    if (lectcodi == 102) lectcodi = 47;
                }

                string prefijo = string.Empty;
                if (lectcodi == 45 || lectcodi == 103) prefijo = "HISTORICO-DIARIO-";
                else if (lectcodi == 46 || lectcodi == 110) prefijo = "PREVISTO-DIARIO-";
                else if (lectcodi == 47 || lectcodi == 102) prefijo = "PREVISTO-SEMANAL-";
                string nombre = prefijo + emprnomb + "-" + fecInicio.ToString("ddMMyy") + "-" + fecFin.ToString("ddMMyy") + ".xlsx";

                int indicador = 0;
                this.servicio.ExportarConsulta(path, file, lectcodi, empresas, fecInicio, fecFin, out indicador);

                if (indicador == 1)
                {
                    return Json(nombre);
                }
                else 
                {
                    return Json(2);
                }
            }
            catch
            {
                return Json((-1).ToString());
            }
        }

        /// <summary>
        /// Permite descargar el archivo generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Descargar(string file)
        {            
            string app = Constantes.AppExcel;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + NombreArchivo.ReporteDemandaBarras;
            return File(fullPath, app, file);
        }
    }
}
