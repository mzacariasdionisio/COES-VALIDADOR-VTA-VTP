using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.CostoOportunidad.Helper;
using COES.MVC.Intranet.Areas.CostoOportunidad.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CostoOportunidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Controllers
{
    public class ConsultasController : BaseController
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        private readonly CostoOportunidadAppServicio servicio = new CostoOportunidadAppServicio();

        /// <summary>
        /// Pagina inicial
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ConsultasModel model = new ConsultasModel();
            model.ListaPeriodo = this.servicio.GetByCriteriaCoPeriodos(-1);
            model.ListadoURS = this.servicio.ObtenerListaURS();
            return View(model);
        }

        /// <summary>
        /// Permite tener las versiones de un periodo
        /// </summary>
        /// <param name="idPeriodo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerVersiones(int idPeriodo)
        {
            try
            {
                return Json(this.servicio.GetByCriteriaCoVersions(idPeriodo));
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener los datos de la version
        /// </summary>
        /// <param name="idVersion"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ObtenerDatoVersion(int idVersion)
        {
            ConsultasModel model = new ConsultasModel();

            CoVersionDTO version = this.servicio.GetByIdCoVersion(idVersion);
            model.FechaInicio = ((DateTime)version.Coverfecinicio).ToString(Constantes.FormatoFecha);
            model.FechaFin = ((DateTime)version.Coverfecfin).ToString(Constantes.FormatoFecha);

            return Json(model);
        }

        /// <summary>
        /// Permite mostrar el reporte
        /// </summary>
        /// <param name="idVersion"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="idTipoInformacion"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Consultar(int idVersion, string fechaInicio, string fechaFin, int idUrs, int idTipoInformacion)
        {
            ConsultasModel model = new ConsultasModel();
            int indicador = 1;
            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<List<int>> reprogramas = new List<List<int>>();
            int[][] colores = null;
            string[][] data = null;

            if (idTipoInformacion < 13)
            {
               data = this.servicio.ObtenerReporteProceso(idVersion, fecInicio, fecFin, idUrs, idTipoInformacion,
                    out indicador, out reprogramas, out colores);
            }
            else if(idTipoInformacion == 13)
            {
                data = this.servicio.ConsultaFactoresUtilizacion(idVersion, fecInicio, fecFin);
                indicador = 3;
            }

            model.Data = data;
            model.Indicador = indicador;

            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            var result = new ContentResult
            {
                Content = serializer.Serialize(model),
                ContentType = "application/json"
            };

            return result;
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
        public JsonResult Exportar(int idVersion, string fechaInicio, string fechaFin, int idUrs, int idTipoInformacion)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = this.ObtenerNombreArchivo(idTipoInformacion);
                int indicador = 1;
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                List<List<int>> reprogramas = new List<List<int>>();
                int[][] colores = null;
                string[][] data = null;

                if (idTipoInformacion < 13)
                {
                    data = this.servicio.ObtenerReporteProceso(idVersion, fecInicio, fecFin, idUrs, idTipoInformacion,
                        out indicador, out reprogramas, out colores);
                }
                else if(idTipoInformacion == 13)
                {
                    data = this.servicio.ConsultaFactoresUtilizacion(idVersion, fecInicio, fecFin);
                    indicador = 1;
                }

                this.servicio.GenerarReporteExcel(data, path, file, indicador, idTipoInformacion, reprogramas, colores);

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
        public virtual ActionResult Descargar(int idTipoInformacion)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + this.ObtenerNombreArchivo(idTipoInformacion);
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, this.ObtenerNombreArchivo(idTipoInformacion));
        }

        /// <summary>
        /// Permite obtener el nombre del archivo excel
        /// </summary>
        /// <param name="idTipoInformacion"></param>
        /// <returns></returns>
        private string ObtenerNombreArchivo(int idTipoInformacion)
        {
            if (idTipoInformacion == 1) return "ProgramadoConReserva.xlsx";
            else if (idTipoInformacion == 2) return "ProgramadoSinReserva.xlsx";
            else if (idTipoInformacion == 3) return "RAUpProgramada.xlsx";
            else if (idTipoInformacion == 4) return "RADownProgramada.xlsx";
            else if (idTipoInformacion == 5) return "RAUpEjecutada.xlsx";
            else if (idTipoInformacion == 6) return "RADownEjecutada.xlsx";
            else if (idTipoInformacion == 7) return "DespachoConReserva.xlsx";
            else if (idTipoInformacion == 8) return "DespachoSinReserva.xlsx";
            else if (idTipoInformacion == 9) return "DespachoConReservaOpcional.xlsx";
            else if (idTipoInformacion == 10) return "DespachoSinReservaOpcional.xlsx";
            else if (idTipoInformacion == 11) return "DespachoConReservaAjustado.xlsx";
            else if (idTipoInformacion == 12) return "DespachoSinReservaAjustado.xlsx";
            else if (idTipoInformacion == 13) return "FactorUtilizacion.xlsx";

            return ConstantesCOportunidad.ArchivoReporte;
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
        public JsonResult ExportarPublicacion(int idVersion, string fechaInicio, string fechaFin)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
                string file = ConstantesCOportunidad.ArchivoReporteCostoOportunidad;
                
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);               
                int[][] colores = null;
                string[][] dataAjustado = this.servicio.ObtenerReportePublicacion(idVersion, fecInicio, fecFin, out colores, 0);
                string[][] data = this.servicio.ObtenerReportePublicacion(idVersion, fecInicio, fecFin, out colores, 1);
                this.servicio.GenerarReporteExcelPublicacion(dataAjustado, data, path, file, 2, colores);

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
        public virtual ActionResult DescargarPublicacion()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + ConstantesCOportunidad.ArchivoReporteCostoOportunidad;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppExcel, ConstantesCOportunidad.ArchivoReporteCostoOportunidad);
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
        public JsonResult ExportarSeniales(int idVersion, string fechaInicio, string fechaFin, int idUrs, int idTipoInformacion)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion;
               
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string fileName = string.Empty;

                this.servicio.GenerarReporteMedicion60(idVersion, idTipoInformacion, idUrs, fecInicio, fecFin, path, out fileName);

                return Json(new { Result = 1, FileName = fileName });
            }
            catch
            {
                return Json(new { Result = -1, FileName = string.Empty });
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarSeniales(string fileName)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + ConstantesCOportunidad.RutaExportacion + fileName;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppZip, fileName);
        }
    }
}