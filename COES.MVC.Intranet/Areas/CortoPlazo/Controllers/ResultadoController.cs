using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.CortoPlazo.Helper;
using COES.MVC.Intranet.Areas.CortoPlazo.Models;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CortoPlazo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace COES.MVC.Intranet.Areas.CortoPlazo.Controllers
{
    public class ResultadoController : BaseController
    {
        /// <summary>
        /// Creación de la instancia del servicio correspondiente
        /// </summary>
        CortoPlazoAppServicio servicio = new CortoPlazoAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(ResultadoController));

        public ResultadoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("ResultadoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("ResultadoController", ex);
                throw;
            }
        }

        /// <summary>
        /// Muestra la pagina de resultados
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ResultadoModel model = new ResultadoModel();
            model.FechaConsulta = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.PathPrincipal = this.servicio.GetPathPrincipal();
            model.FechaInicio = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.FechaInicioAnterior = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
            model.OpcionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);
            model.FechaVigenciaPR07 = ConfigurationManager.AppSettings[ConstantesCortoPlazo.FechaVigenciaPR07];
            return View(model);
        }

        /// <summary>
        /// Muestra la página de resultados
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Listado(string fecha, int tipoProceso)
        {
            ResultadoModel model = new ResultadoModel();
            DateTime fechaConsulta = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            model.Listado = this.servicio.ObtenerResultadoCostosMarginales(fechaConsulta, tipoProceso);
            model.OpcionGrabar = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return PartialView(model);
        }

        /// <summary>
        /// Muestra los datos de una corrida
        /// </summary>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Resultado(int correlativo, string fecha, string usuario, string tipo)
        {
            ResultadoModel model = new ResultadoModel();
            model.Listado = this.servicio.ObtenerDatosCostoMarginalCorrida(correlativo);
            model.ListadoGeneracionEms = servicio.ObtenerGeneracionEmsPorCorrelativo(correlativo);
            model.ListaRestricciones = servicio.ObtenerRestriccionesPorCorrida(correlativo);
            model.VersionPrograma = servicio.ObtenerVersionPrograma(correlativo);
            model.UsuarioEjecucion = usuario;
            model.FechaEjecucion = fecha;
            model.TipoProceso = tipo == "1" ? "Ejecución normal" : (tipo == "2" ? "Con ángulo óptimo" : (tipo == "3" ? "Por transacciones internacionales" : ""));

            return PartialView(model);
        }

        [HttpPost]
        public JsonResult EliminarCorrida(int correlativo) 
        {
            try
            {
                this.servicio.EliminarCorridaCostoMarginal(correlativo);
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar un nuevo reproceso
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Reprocesar(string fecha, string hora, int indicador, int indicadorNCP, int indicadorMostrarWeb,
            string rutaNCP, int version, string fuenteRD = ConstantesCortoPlazo.TipoNCP, int idEscenario = 0, 
            string tipoEMS = ConstantesCortoPlazo.EstimadorPSS)
        {
            try
            {                
                bool flagNCP = (indicadorNCP == 1) ? true : false;
                bool flagWeb = (indicadorMostrarWeb == 1) ? true : false;
                bool flagMD = (fuenteRD == ConstantesCortoPlazo.TipoMDCOES) ? true : false;

                string strFecha = fecha;

                if (hora == ConstantesCortoPlazo.UltimoPeriodo)
                {
                    DateTime newFecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture).AddDays(1);
                    strFecha = newFecha.ToString(Constantes.FormatoFecha);
                    hora = ConstantesCortoPlazo.PrimerPeriodo;
                }

                if (tipoEMS == ConstantesCortoPlazo.EstimadorPSS)
                {
                    flagWeb = false;
                }

                string time = strFecha + " " + hora;
                DateTime fechaProceso = DateTime.ParseExact(time, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                cliente.EjecutarCostosMarginalesAlterno(fechaProceso, indicador, true, flagNCP, flagWeb, rutaNCP, flagMD, idEscenario, base.UserName,
                    tipoEMS, 1, version);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("Reprocesar", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite generar el reproceso masivo
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarMasivo(string fechaInicio, string fechaFin, string horas, int version)
        {
            try
            {
                DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                string[] listaHoras = horas.Split(Constantes.CaracterComa);
                ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                int resultado = cliente.EjecutarReprocesoMasivo(fecInicio, fecFin, listaHoras, true, base.UserName, 
                    ConstantesCortoPlazo.EstimadorTNA, version);
                return Json(resultado);
            }
            catch (Exception) 
            {
                return Json(-1);
            }
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
        public JsonResult Exportar(int correlativo)
        {
            try
            {
                ResultadoModel model = new ResultadoModel();
                List<CmCostomarginalDTO> listado = this.servicio.ObtenerDatosCostoMarginalCorrida(correlativo);
                List<CmGeneracionEmsDTO> listadoGeneracion = servicio.ObtenerGeneracionEmsPorCorrelativo(correlativo);

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
                string file = ConstantesCortoPlazo.ReporteResultado;

                CalculoHelper.GenerarReporteCM(listado,listadoGeneracion, path, file);

                return Json(1);
            }
            catch(Exception ex)
            {
                log.Error("Exportar", ex);
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
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteResultado;
            return File(fullPath, Constantes.AppExcel, ConstantesCortoPlazo.ReporteResultado);
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
        public JsonResult ExportarMasivo(string fechaInicio, string fechaFin, string estimador, string fuentepd, int version)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                                
                List<CmCostomarginalDTO> listado = this.servicio.ObtenerReporteCostosMarginales(fecInicio, fecFin, estimador,  fuentepd, version);

                List<CmGeneracionEmsDTO> listadoGeneracion = servicio.ObtenerGeneracionEmsEntreFechas(fecInicio, fecFin, estimador);

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
                string file = ConstantesCortoPlazo.ReporteResultadoMasivo;

                CalculoHelper.GenerarReporteMasivoCM(listado, listadoGeneracion, path, file);

                return Json(1);
            }
            catch(Exception ex)
            {
                log.Error("ExportarMasivo",ex);
                return Json(-1);
            }
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
        public JsonResult ExportarMasivoWeb(string fechaInicio, string fechaFin)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
                DateTime fecFin = DateTime.Now;

                if (!string.IsNullOrEmpty(fechaInicio))
                {
                    fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(fechaFin))
                {
                    fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                List<CmCostomarginalDTO> listado = this.servicio.ObtenerReporteCostosMarginalesTR(fecInicio, fecFin);
                //List<CmGeneracionEmsDTO> listadoGeneracion = servicio.ObtenerGeneracionEmsEntreFechas(fecInicio, fecFin);

                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;
                string file = ConstantesCortoPlazo.ReporteResultadoMasivoPublicado;

                CalculoHelper.GenerarReporteMasivoCMWeb(listado, path, file);

                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error("ExportarMasivo", ex);
                return Json(-1);
            }
        }


        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarMasivo()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteResultadoMasivo;
            return File(fullPath, Constantes.AppExcel, ConstantesCortoPlazo.ReporteResultadoMasivo);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarMasivoWeb()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + ConstantesCortoPlazo.ReporteResultadoMasivoPublicado;
            return File(fullPath, Constantes.AppExcel, ConstantesCortoPlazo.ReporteResultadoMasivoPublicado);
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
        public JsonResult ExportarDAT(string fecha, int opcion, int tipo, string estimador, string fuentepd, int version)
        {
            try
            {
                DateTime fecInicio = DateTime.Now;
              
                if (!string.IsNullOrEmpty(fecha))
                {
                    fecInicio = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }

                List<CmCostomarginalDTO> listado = (opcion == 1)?this.servicio.ObtenerReporteCostosMarginales(fecInicio, fecInicio, estimador, fuentepd, version)
                    : this.servicio.ObtenerReporteCostosMarginalesTR(fecInicio, fecInicio);
                
                var entitys = listado.Select(x => new { x.Cmgncorrelativo, x.Cmgnfecha }).Distinct().ToList();

                List<CmCostomarginalDTO> result = new List<CmCostomarginalDTO>();
                foreach (var item in entitys)
                {
                    CmCostomarginalDTO entity = new CmCostomarginalDTO();
                    entity.Cmgncorrelativo = item.Cmgncorrelativo;
                    entity.Cmgnfecha = item.Cmgnfecha;
                    result.Add(entity);
                }
                
                string path = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal;


                string file = this.servicio.ExportarArchivosDAT(result, fecInicio, path, tipo);

                return Json(file);
            }
            catch (Exception ex)
            {
                log.Error("Exportar", ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarDAT(string file)
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.RutaExportacionCostoMarginal + file;
            var bytes = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(bytes, Constantes.AppZip, file);
        }

        /// <summary>
        /// Permite mostrar los archivos relacionado la corrida
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Folder(string fecha, int correlativo) 
        {
            ResultadoModel model = new ResultadoModel();
            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);            
            string path = this.servicio.GetPathCorrida(fechaProceso, correlativo);
            model.PathResultado = path;            
            return Json(model);
        }

        /// <summary>
        /// Permite mostrar el mapa de coordenadas
        /// </summary>
        /// <param name="idEquipo"></param>
        /// <returns></returns>        
        public ViewResult Mapa(int correlativo)
        {
            BarraModel model = new BarraModel();
            List<CmCostomarginalDTO> list = this.servicio.ObtenerCostosMarginalesMapa(correlativo);

            if (list.Count > 0)
            {
                string str = string.Empty;
                int index = 0;
                foreach (CmCostomarginalDTO item in list)
                {
                    string descripcion = string.Format(
                    @"<div class='popup-title'><span>Barra: {0}</span></div><div class='content-registro'><table style='width:100%'><tr><td class='registro-label'>Energía</td><td class='registro-control'>{1}</td></tr><tr><td class='registro-label'>Congestión</td><td class='registro-control'>{2}</td></tr><tr><td class='registro-label'>Total</td><td class='registro-control'>{3}</td></tr></table></div>", 
                    item.Cnfbarnombre, (item.Cmgnenergia != null) ? ((decimal)item.Cmgnenergia).ToString("#,###.00") : "",
                                          (item.Cmgncongestion != null) ? ((decimal)item.Cmgncongestion).ToString("#,###.00") : "",
                                          (item.Cmgntotal != null) ? ((decimal)item.Cmgntotal).ToString("#,###.00") : ""
                    );

                    if (index < list.Count - 1)
                        str += string.Format(@"[ coorx: {0}, coory: {1}, descripcion: #{2}#],", item.Cnfbarcoorx, item.Cnfbarcoory, descripcion);
                    else
                        str += string.Format(@"[ coorx: {0}, coory: {1}, descripcion: #{2}#]", item.Cnfbarcoorx, item.Cnfbarcoory, descripcion);
                    index++;
                }

                model.ListaCoordenada = "[" + str.Replace('#', '\"').Replace('[', '{').Replace(']', '}') + "]";
            }
            else            
            {
                model.ListaCoordenada = "[]";
            }

            return View(model);
        }

        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload(string tipoEMS)
        {
            try
            {
                string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaReprocesoCostosMarginales];

                if (tipoEMS == ConstantesCortoPlazo.EstimadorTNA)
                {
                    path = ConfigurationManager.AppSettings[RutaDirectorio.RutaReprocesoCostosMarginalesTNA];
                }

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = NombreArchivo.ArchivoRawCM;

                    if (FileServer.VerificarExistenciaFile("Reproceso", fileName, path)) 
                    {
                        FileServer.DeleteBlob(@"Reproceso\" + fileName, path);
                    }

                    FileServer.UploadFromStream(file.InputStream, @"Reproceso\", fileName, path);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                log.Error("Upload", ex);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ObtenerEscenarios(string fecha)
        {
            try
            {
                DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                var list = (new McpAppServicio()).ObtenerEscencariosPorDia(fechaProceso);
                return Json(list);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite realizr el reprocesamiento de los CM
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reprocesamiento(string fechaInicio, string fechaFin)
        {
            ReprocesamientoModel model = new ReprocesamientoModel();
            List<ReprocesoItemModel> ListaDetalle = new List<ReprocesoItemModel>();

            DateTime fecInicio = DateTime.ParseExact(fechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFin = DateTime.ParseExact(fechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

            int days = (int)fecFin.Subtract(fecInicio).TotalDays;
            int id = 1;
            for (int i = 0; i <= days; i++)
            {
                DateTime fecha = fecInicio.AddDays(i);
                List<CpTopologiaDTO> listTopologia = (new McpAppServicio()).ObtenerEscencariosPorDia(fecha);

                for(int j = 1; j <= 48; j++)
                {
                    string periodo = fecha.AddMinutes(j * 30).ToString(ConstantesAppServicio.FormatoHora);
                    if (j == 48) periodo = "23:59";
                    ReprocesoItemModel itemModel = new ReprocesoItemModel();
                    itemModel.Ems = "TNA";
                    itemModel.Fecha = fecha.ToString(Constantes.FormatoFecha);
                    itemModel.Hora = periodo;
                    itemModel.Id = id;
                    itemModel.ListaEscenarios = listTopologia;
                    ListaDetalle.Add(itemModel);
                    id++;
                }
            }
            model.ListaDetalle = ListaDetalle;
            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar el archivo de FPM
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadReproceso(string id)
        {
            try
            {
                string path = ConfigurationManager.AppSettings[RutaDirectorio.RutaReprocesoCostosMarginalesTNA];

                if (Request.Files.Count == 1)
                {
                    string filenameUpload = Request.Files[0].FileName;
                    DateTime fecha = getDateFromPsseFile(filenameUpload);

                    if(id.Trim() == "reprocesomasivo48" && !(fecha.Minute <= 15)) //!filenameUpload.Contains("0000")
                    {
                        fecha = fecha.AddDays(1);
                    }

                    path = String.Format(path + "{0:0000}\\{1:00}\\{2:00}\\", fecha.Year, fecha.Month, fecha.Day);
                    var file = Request.Files[0];
                    string fileName = id + ".raw";


                    if (FileServerScada.VerificarExistenciaFile("Masivo", fileName, path))
                    {
                        FileServerScada.DeleteBlob(@"Masivo\" + fileName, path);
                    }

                    FileServerScada.UploadFromStream(file.InputStream, @"Masivo\", fileName, path);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite generar el reproceso masivo
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="horas"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReprocesarModificado(string[][] data, int version)
        {
            try
            {               
                ServiceReferenceCostosMarginales.CostoMarginalServicioClient cliente = new ServiceReferenceCostosMarginales.CostoMarginalServicioClient();
                int resultado = cliente.EjecutarReprocesoMasivoModificado(data, base.UserName, version);
                return Json(resultado);
            }
            catch (Exception ex)
            {
                return Json(-1);
            }
        }

        private DateTime getDateFromPsseFile(string filename)
        {
            string dateString = filename.Substring(5, 12); // Obtiene "202504022359"

            if (!filename.StartsWith("PSSE_"))
            {
                dateString = filename.Substring(0, 8);
                dateString = dateString + filename.Substring(9, 4);
            }

            int year = int.Parse(dateString.Substring(0, 4));
            int month = int.Parse(dateString.Substring(4, 2));
            int day = int.Parse(dateString.Substring(6, 2));
            int hora = int.Parse(dateString.Substring(8, 2));
            int minuto = int.Parse(dateString.Substring(10, 2));

            return new DateTime(year, month, day, hora, minuto, 0);
        }
    }
}
