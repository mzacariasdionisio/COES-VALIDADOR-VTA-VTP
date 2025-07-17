using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Sic.PowerBI;
using COES.Framework.Base.Tools;
using COES.MVC.Publico.Helper;
using COES.MVC.Publico.Models;
using COES.Servicios.Aplicacion.General;
using log4net;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Publico.Controllers
{
    public class PortalInformacionController : Controller
    {
        /// <summary>
        /// Instancia de la clase servicio
        /// </summary>
        PortalAppServicio servicio = new PortalAppServicio();
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(PortalInformacionController));

        #region Generacion

        /// <summary>
        /// Carga de la pantalla principal generacion
        /// </summary>
        /// <returns></returns>
        public ActionResult Generacion()
        {
            PortalInformacionModel model = new PortalInformacionModel();            
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaISO);            
            
            return View(model);
        }

        /// <summary>
        /// Datos para pantalla de generación SCADA y Medidores
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="indicador">0: Generación SCADA, 1: Generación Medidores</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Generacion(string fechaInicial, string fechaFinal, int indicador)
        {
            try
            {
                GeneracionScadaModel model = new GeneracionScadaModel();

                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                if (indicador == 0)
                {
                    model.ListadoPorEmpresa = this.servicio.ObtenerGeneracionPorEmpresa(fechaInicio, fechaFin);
                    model.GraficoPorEmpresa = this.servicio.ObtenerGeneracionPorEmpresaTipoGeneracion(fechaInicio, fechaFin);
                    model.GraficoTipoCombustible = this.servicio.ObtenerGeneracionPorTipoCombustible(fechaInicio, fechaFin);
                }
                else
                {
                    List<MeMedicion48DTO> listEmpresa = new List<MeMedicion48DTO>();
                    ChartGeneracion graficoGeneracion = new ChartGeneracion();
                    ChartStock chartStock = new ChartStock();

                    this.servicio.ObtenerGeneracionMedidores(fechaInicio, fechaFin, out listEmpresa, out graficoGeneracion, out chartStock);
                    model.ListadoPorEmpresa = listEmpresa;
                    model.GraficoPorEmpresa = graficoGeneracion;
                    model.GraficoTipoCombustible = chartStock;
                }

                model.Indicador = indicador;

                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(model),
                    ContentType = "application/json"
                };

                return result;
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite exportar el reporte de generacion
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarGeneracion(string fechaInicial, string fechaFinal, int indicador)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = NombreArchivo.ReporteGeneracionPortal;
                this.servicio.ObtenerExportacionGeneracion(fechaInicio, fechaFin, indicador, path, file);

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
        public virtual ActionResult DescargarGeneracion()
        {
            string file = NombreArchivo.ReporteGeneracionPortal;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, Constantes.AppExcel, file);
        }

        #endregion

        #region Demanda

        /// <summary>
        /// Carga la pantalla inicial de demanda
        /// </summary>
        /// <returns></returns>
        public ActionResult Demanda()
        {
            PortalInformacionModel model = new PortalInformacionModel();
            model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaISO);
            model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFechaISO);

            DateTime fechaFin = EPDate.f_fechafinsemana(DateTime.Now).AddDays(7);

            model.FechaFinSemanaSiguienteOperativa = fechaFin.ToString(Constantes.FormatoFechaISO);

            return View(model);
        }

        /// <summary>
        /// Datos para la pantalla de Demanda COES
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Demanda(string fechaInicial, string fechaFinal)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                DateTime fechaEjecutado = DateTime.Now;
                decimal valorEjecutado = 0;
                ChartDemanda demanda = this.servicio.ObtenerReporteDemanda(fechaInicio, fechaFin, out fechaEjecutado, out valorEjecutado);


                var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
                var result = new ContentResult
                {
                    Content = serializer.Serialize(demanda),
                    ContentType = "application/json"
                };

                return result;

                //return Json(demanda);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public String ObtenerPronosticoTiempoRealFechaDeActualizacion()
        {
            List<PronosticoTempoRealModel> data = new List<PronosticoTempoRealModel>();
            string path = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealDirectorio];
            string fechaFile = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealArchivoFecha];
            FileData fileInfoFecha = FileServer.ObtenerArchivoEspecifico(path, fechaFile);
            string fecha = "";

            if (fileInfoFecha == null)
            {
                log.Error("No existe archivo de fecha");
                return fecha;
            }

            try
            {
                using (var reader = FileServer.OpenReaderFile(fechaFile, path))
                {
                    int pos = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] columns = line.Split(',');

                        if (pos == 0)
                        {
                            pos++;
                            continue;
                        }

                        fecha = columns[0];
                        pos++;
                    }
                }

                return fecha;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            
            return fecha;
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult ObtenerPronosticoTiempoReal()
        {
            try
            {
                List<PronosticoTempoRealModel> data = new List<PronosticoTempoRealModel>();
                DateTime fechaConsulta = DateTime.Now;
                string path = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealDirectorio];
                string DataFile = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealArchivoData];
                FileData fileInfoData = FileServer.ObtenerArchivoEspecifico(path, DataFile);

                if (fileInfoData == null)
                {
                    return Json("No existe archivo de data o fecha", JsonRequestBehavior.AllowGet);
                }

                using (var reader = FileServer.OpenReaderFile(DataFile, path))
                {
                    int pos = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] columns = line.Split(',');

                        if (pos == 0)
                        {
                            pos++;
                            continue;
                        }

                        PronosticoTempoRealModel pronosticoTempoRealModel = new PronosticoTempoRealModel();
                        pronosticoTempoRealModel.fecha = columns[0];
                        pronosticoTempoRealModel.Rdo = String.IsNullOrEmpty(columns[1]) ? 0 : Convert.ToDouble(columns[1]);

                        if (DateTime.ParseExact(pronosticoTempoRealModel.fecha, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) >= fechaConsulta)
                        {
                            pronosticoTempoRealModel.PronosticoTiempoReal = String.IsNullOrEmpty(columns[2]) ? 0 : Convert.ToDouble(columns[2]);
                            pronosticoTempoRealModel.PronosticoMinimo = String.IsNullOrEmpty(columns[3]) ? 0 : Convert.ToDouble(columns[3]);
                            pronosticoTempoRealModel.PronosticoMaximo = String.IsNullOrEmpty(columns[4]) ? 0 : Convert.ToDouble(columns[4]);
                        }
                        else
                        {
                            pronosticoTempoRealModel.PronosticoTiempoReal = -1;
                            pronosticoTempoRealModel.PronosticoMinimo = -1;
                            pronosticoTempoRealModel.PronosticoMaximo = -1;
                        }

                        if (DateTime.ParseExact(pronosticoTempoRealModel.fecha, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) <= fechaConsulta && !String.IsNullOrEmpty(columns[5]))
                        {
                            pronosticoTempoRealModel.Ejecutado = Convert.ToDouble(columns[5]);
                        }
                        else
                        {
                            pronosticoTempoRealModel.Ejecutado = -1;
                        }

                        data.Add(pronosticoTempoRealModel);
                        pos++;
                    }
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite obtener la data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ExportarPronosticoTiempoReal()
        {
            string contentType = "application/octet-stream";
            string fileName = "data.xlsx";
            MemoryStream memoryStream = new MemoryStream();
            List<PronosticoTempoRealModel> data = new List<PronosticoTempoRealModel>();

            try
            {
                DateTime fechaACtualzacion = DateTime.Now;
                string path = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealDirectorio];
                string DataFile = ConfigurationManager.AppSettings[RutaDirectorio.PronosticoTempoRealArchivoData];
                FileData fileInfoData = FileServer.ObtenerArchivoEspecifico(path, DataFile);

                if (fileInfoData == null)
                {
                    return Json("No existe archivo de data o fecha", JsonRequestBehavior.AllowGet);
                }

                using (ExcelPackage package = new ExcelPackage(memoryStream))
                {
                    ExcelWorksheet worksheet;
                    worksheet = package.Workbook.Worksheets.Add("data");

                    using (var reader = FileServer.OpenReaderFile(DataFile, path))
                    {
                        int pos = 1;
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] columns = line.Split(',');

                            if (pos == 1)
                            {
                                worksheet.Cells[pos, 1].Value = "Fecha";
                                worksheet.Cells[pos, 2].Value = "Reprogramación Diaria";
                                worksheet.Cells[pos, 3].Value = "Pronóstico de Demanda Automático de Tiempo Real";
                                worksheet.Cells[pos, 4].Value = "Rango Mínimo del Pronóstico de Demanda Automático de Tiempo Real";
                                worksheet.Cells[pos, 5].Value = "Rango Máximo del Pronóstico de Demanda Automático de Tiempo Real";
                                worksheet.Cells[pos, 6].Value = "Ejecutado";
                                pos++;
                                continue;
                            }

                            worksheet.Cells[pos, 1].Value = columns[0];
                            worksheet.Cells[pos, 2].Value = String.IsNullOrEmpty(columns[1]) ? 0 : Convert.ToDouble(columns[1]);
                            worksheet.Cells[pos, 3].Value = String.IsNullOrEmpty(columns[2]) ? 0 : Convert.ToDouble(columns[2]);
                            worksheet.Cells[pos, 4].Value = String.IsNullOrEmpty(columns[3]) ? 0 : Convert.ToDouble(columns[3]);
                            worksheet.Cells[pos, 5].Value = String.IsNullOrEmpty(columns[4]) ? 0 : Convert.ToDouble(columns[4]);
                            worksheet.Cells[pos, 6].Value = String.IsNullOrEmpty(columns[5]) ? 0 : Convert.ToDouble(columns[5]);
                            pos++;
                        }
                    }

                    //worksheet.Cells[1, i + 1].Value = i;
                    package.Save();
                }

                memoryStream.Position = 0;
                return File(memoryStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Permite exportar el reporte de generacion
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarDemanda(string fechaInicial, string fechaFinal)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = NombreArchivo.ReporteMedicionPortal;
                this.servicio.ObtenerExportacionDemanda(fechaInicio, fechaFin, path, file);

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
        public virtual ActionResult DescargarDemanda()
        {
            string file = NombreArchivo.ReporteMedicionPortal;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, Constantes.AppExcel, file);
        }

        #endregion
                
        #region Eventos y Fallas

        /// <summary>
        /// Carga de la pantalla principal fallas
        /// </summary>
        /// <returns></returns>
        //public ActionResult Fallas()
        //{
        //    PortalInformacionModel model = new PortalInformacionModel();
        //    model.ListaEmpresas = servicio.ListarEmpresas().ToList();
        //    model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
        //    model.FechaFin = DateTime.Now.ToString(Constantes.FormatoFecha);
        //    model.ListaTipoEmpresas = servicio.ListarTipoEmpresas();
        //    model.ListaFamilias = servicio.ListarFamilias();
        //    model.ListaCausaEvento = servicio.ListarCausasEventos();
        //    return View(model);
        //}

        /// <summary>
        /// Muestra la consulta de eventos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult EventoFallas(EventoFallaModel model)
        {
            DateTime fechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);
            model.ListaEventos = this.servicio.BuscarEventos(model.IdFallaCier, fechaInicio, fechaFin, "-1", string.Empty, model.TipoEmpresa,
                model.IdEmpresa, model.IdTipoEquipo, model.Interrupcion, model.NroPagina, Constantes.PageSizeEvento).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Permite mostrar el paginado de la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Paginado(EventoFallaModel modelo)
        {
            EventoFallaModel model = new EventoFallaModel();
            model.IndicadorPagina = false;

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            if (modelo.FechaInicio != null)
            {
                fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            if (modelo.FechaFin != null)
            {
                fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }
            fechaFin = fechaFin.AddDays(1);

            int nroRegistros = servicio.ObtenerNroFilasEvento(model.IdFallaCier, fechaInicio, fechaFin, "-1", string.Empty,
                modelo.TipoEmpresa, modelo.IdEmpresa, modelo.IdTipoEquipo, modelo.Interrupcion);

            if (nroRegistros > Constantes.PageSizeEvento)
            {
                int pageSize = Constantes.PageSizeEvento;
                int nroPaginas = (nroRegistros % pageSize == 0) ? nroRegistros / pageSize : nroRegistros / pageSize + 1;
                model.NroPaginas = nroPaginas;
                model.NroMostrar = Constantes.NroPageShow;
                model.IndicadorPagina = true;
            }

            return PartialView(model);
        }

        /// <summary>
        /// Carga las empresas de un determinado tipo de empresa
        /// </summary>
        /// <param name="idTipoEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CargarEmpresas(int idTipoEmpresa)
        {
            List<EmpresaDTO> entitys = new List<EmpresaDTO>();
            if (idTipoEmpresa != 0)
            {
                entitys = this.servicio.ListarEmpresasPorTipo(idTipoEmpresa).ToList(); ;
            }
            else
            {
                entitys = this.servicio.ListarEmpresas().ToList(); ;
            }
            SelectList list = new SelectList(entitys, "EMPRCODI", "EMPRNOMB");
            return Json(list);
        }

        /// <summary>
        /// Permite exportar los datos de eventos en la consulta
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarEvento(EventoFallaModel modelo)
        {
            int result = 1;
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (modelo.FechaInicio != null)
                {
                    fechaInicio = DateTime.ParseExact(modelo.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                if (modelo.FechaFin != null)
                {
                    fechaFin = DateTime.ParseExact(modelo.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                }
                fechaFin = fechaFin.AddDays(1);
                List<EventoDTO> list = servicio.ExportarEventos(modelo.IdFallaCier, fechaInicio, fechaFin, "-1", string.Empty,
                    modelo.TipoEmpresa, modelo.IdEmpresa, modelo.IdTipoEquipo, modelo.Interrupcion).ToList();
                ExcelHelper.GenerarReporteEvento(list, fechaInicio, fechaFin);
                result = 1;
            }
            catch
            {
                result = -1;
            }
            return Json(result);
        }

        /// <summary>
        /// Permite descargar los datos de los eventos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarEvento()
        {
            string fullPath = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + NombreArchivo.ReporteEvento;
            return File(fullPath, Constantes.AppExcel, NombreArchivo.ReporteEvento);
        }

        /// <summary>
        /// Arma los datos para los graficos de la grilla
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult DatosGraficoFalla(EventoFallaModel model)
        {
            DateTime fechaInicio = DateTime.ParseExact(model.FechaInicio, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(model.FechaFin, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            fechaFin = fechaFin.AddDays(1);
            model.ListaEventos = servicio.ExportarEventos(model.IdFallaCier, fechaInicio, fechaFin, "-1", string.Empty,
                    model.TipoEmpresa, model.IdEmpresa, model.IdTipoEquipo, model.Interrupcion).ToList();
            FallaModel modelo = new FallaModel();

            // PARTICIPACIÓN DEL NÚMERO DE FALLAS POR TIPO DE CAUSA DE FALLA CIER
            var QueryTipos =
                from p in model.ListaEventos
                group p by p.CAUSAEVENABREV into newGroup
                select newGroup;
            modelo.FallaCier = new List<FallaCier>();
            foreach (var nameGroup in QueryTipos)
            {
                FallaCier f = new FallaCier();
                f.name = nameGroup.Key;
                f.y = nameGroup.Count();
                modelo.FallaCier.Add(f);
            }

            // DURACIÓN DE FALLA POR TIPO DE EQUIPO Y CAUSA DE FALLA CIER
            modelo.NombresDuracionxTipoEquipoYCier = (from p in model.ListaEventos
                                                      select p.FAMNOMB).Distinct().ToList();
            modelo.DuracionxTipoEquipoYCier = new List<Models.Series>();
            foreach (var grupo in QueryTipos)
            {
                var QueryTipos1 = from q in grupo
                                  group q by q.FAMNOMB into newGrupo
                                  select newGrupo;
                Models.Series grupoxCier = new Models.Series();
                grupoxCier.name = grupo.Key;
                grupoxCier.data = new List<double>(new double[modelo.NombresDuracionxTipoEquipoYCier.Count]);
                foreach (var subGrupo in QueryTipos1)
                {
                    double suma = 0;
                    foreach (var item in subGrupo)
                    {
                        var t = item.EVENFIN - item.EVENINI;
                        suma += 1440 * t.Value.Days + 60 * t.Value.Hours + t.Value.Minutes;
                    }
                    int index = modelo.NombresDuracionxTipoEquipoYCier.IndexOf(subGrupo.First().FAMNOMB);
                    grupoxCier.data[index] = suma;
                }
                modelo.DuracionxTipoEquipoYCier.Add(grupoxCier);
            }

            // DURACIÓN DE FALLA POR TIPO DE CAUSA DE FALLA CIER Y NIVEL DE TENSIÓN
            modelo.NombresDuracionxCierYTension = (from p in model.ListaEventos
                                                      select p.EQUITENSION.ToString()).Distinct().ToList();
            modelo.DuracionxCierYTension = new List<Models.Series>();
            foreach (var grupo in QueryTipos)
            {
                var QueryTipos1 = from q in grupo
                                  group q by q.EQUITENSION into newGrupo
                                  select newGrupo;
                Models.Series grupoxCier = new Models.Series();
                grupoxCier.name = grupo.Key;
                grupoxCier.data = new List<double>(new double[modelo.NombresDuracionxCierYTension.Count]);
                foreach (var subGrupo in QueryTipos1)
                {
                    double suma = 0;
                    foreach (var item in subGrupo)
                    {
                        var t = item.EVENFIN - item.EVENINI;
                        suma += 1440 * t.Value.Days + 60 * t.Value.Hours + t.Value.Minutes;
                    }
                    int index = modelo.NombresDuracionxCierYTension.IndexOf(subGrupo.First().EQUITENSION.ToString());
                    grupoxCier.data[index] = suma;
                }
                modelo.DuracionxCierYTension.Add(grupoxCier);
            }
            //modelo.NombresDuracionxCierYTension[modelo.NombresDuracionxCierYTension.IndexOf("")] = "No Definido";

            // ENERGÍA INTERRUMPIDA APROXIMADA POR TIPO DE EQUIPO (MWh)
            QueryTipos =
                from p in model.ListaEventos
                group p by p.FAMNOMB into newGroup
                select newGroup;
            modelo.EnergiaInterrumpidaxEquipo = new Models.Series();
            modelo.EnergiaInterrumpidaxEquipo.data = new List<double>();
            modelo.NombresEnergiaInterrumpidaxEquipo = new List<string>();
            modelo.EnergiaInterrumpidaxEquipo.name = string.Empty;
            foreach (var nameGroup in QueryTipos)
            {
                double suma = 0;
                foreach (var valor in nameGroup)
                {
                    suma += (valor.INTERRUPCIONMW != null) ? (double)valor.INTERRUPCIONMW : 0;
                }
                modelo.EnergiaInterrumpidaxEquipo.data.Add(Math.Round(suma));
                modelo.NombresEnergiaInterrumpidaxEquipo.Add(nameGroup.Key);
            }

            return Json(modelo);
        }

        #endregion

        #region Hidrologia

        /// <summary>
        /// Carga la pantalla inicial de hidrologia
        /// </summary>
        /// <returns></returns>
        //public ActionResult Hidrologia()
        //{
        //    PortalInformacionModel model = new PortalInformacionModel();
        //    model.FechaInicio = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
        //    model.FechaFin = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

        //    return View(model);
        //}

        /// <summary>
        /// Permite exportar el reporte de generacion
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="indicador"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarHidrologia(string fechaInicial, string fechaFinal)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones];
                string file = NombreArchivo.ReporteHidrologiaPortal;
                this.servicio.ObtenerExportacionHidrologia(fechaInicio, fechaFin, path, file);

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
        public virtual ActionResult DescargarHidrologia()
        {
            string file = NombreArchivo.ReporteHidrologiaPortal;
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteMediciones] + file;
            return File(fullPath, Constantes.AppExcel, file);
        }


        /// <summary>
        /// Datos para hidrologia
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaHidrologia(string fechaInicial, string fechaFinal)
        {
            HidrologiaModel model = new HidrologiaModel();

            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;
            if (fechaInicial != null)
                fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            if (fechaFinal != null)
                fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);


            if (fechaInicio.Year == DateTime.Now.Year && fechaInicio.Month == DateTime.Now.Month && 
                fechaInicio.Day == DateTime.Now.Day) fechaFin = fechaInicio;

            List<string> puntos = new List<string>();
            List<DataHidrologia> list = this.servicio.ObtenerReporteHidrologia(fechaInicio, fechaFin, out puntos);

            model.Puntos = puntos;
            model.Data = list;

            return PartialView(model);
        }

        /// <summary>
        /// Obtiene los datos para el gráfico de hidrologia
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <param name="fuente"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoHidrologia(string fechaInicial, string fechaFinal, string fuente)
        {
            try
            {
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFin = DateTime.Now;
                if (fechaInicial != null)
                    fechaInicio = DateTime.ParseExact(fechaInicial, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                if (fechaFinal != null)
                    fechaFin = DateTime.ParseExact(fechaFinal, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<SeriesXY> list = this.servicio.ObtenerGraficoReporteHidrologia(fechaInicio, fechaFin, fuente);
                return Json(list);
            }
            catch
            {
                return Json(-1);
            }
        }

        #endregion

        #region Power BI

        /// <summary>
        /// Carga la pantalla inicial de los reportes de Power BI
        /// </summary>
        /// <returns></returns>
        public ActionResult VisorPowerBI()
        {
            return View();
        }

        /// <summary>
        /// Datos para la pantalla de Reportes de Power BI
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost]
        public ActionResult ObtenerReportePorTabId()
        {
            try
            {
                string body;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    body = reader.ReadToEnd();
                }

                string tabId = new JavaScriptSerializer().Deserialize<string>(body);

                var reporte = Task.Run(async () => await servicio.ObtenerReportePorTabId(tabId)).GetAwaiter().GetResult();
                return Json(reporte);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = true,
                    message = e.Message,
                    stack = e.StackTrace
                });
            }
        }


        [HttpPost]
        public ActionResult ObtenerReportes()
        {
            try
            {
                List<PowerBIReportDTO> list = Task.Run(async () => await servicio.ObtenerReportes()).GetAwaiter().GetResult();
                return Json(list);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    error = true,
                    message = e.Message,
                    stack = e.StackTrace 
                });
            }
        }
        

        #endregion

        #region Monitoreo SEIN

        public ActionResult MonitoreoSEIN()
        {
            return View();
        }

        #endregion
    }
}
