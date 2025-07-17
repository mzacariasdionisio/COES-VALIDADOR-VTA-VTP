using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.CPPA.Helper;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ReporteFrecuenciaSGOCOESController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();

        public ActionResult Index()
        {
            ReporteSegundosFaltantesModel model = new ReporteSegundosFaltantesModel();
            List<ReporteSegundosFaltantesDTO> lista = new List<ReporteSegundosFaltantesDTO>();
            List<EquipoGPSDTO> listaEquipos = new List<EquipoGPSDTO>();

            List<EquipoGPSDTO> listaEquiposSelect = new List<EquipoGPSDTO>();

            List<ReporteFrecuenciaDescargaDTO> listaFrecuencia = new List<ReporteFrecuenciaDescargaDTO>();
            List<ReporteFrecuenciaDescargaDTO> listaFrecuenciaMinuto = new List<ReporteFrecuenciaDescargaDTO>();

            DateTime date = DateTime.Now;
            model.FechaInicial = date;
            int intAnio = 0;
            int intMes = 0;
            if (model.FechaInicial.Month == 1)
            {
                intAnio = date.Year - 1;
                intMes = 12;
            }
            else
            {
                intAnio = date.Year;
                intMes = model.FechaInicial.Month - 1;
            }
            model.FechaInicial = new DateTime(intAnio, intMes, 1, 0, 0, 0);
            var fechaFinal = model.FechaInicial.AddMonths(1).AddDays(-1);
            model.FechaFinal = new DateTime(fechaFinal.Year, fechaFinal.Month, fechaFinal.Day, 23, 59, 59);
            model.ListaReporte = lista;
            List<string> listaFechas = new List<string>();
            model.ListaFechas = listaFechas;
            model.ListaEquipos = listaEquipos;
            model.ListaEtapas = new EtapaERAAppServicio().GetListaEtapas();

            listaEquiposSelect = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSEstado == "A").OrderBy(x => x.NombreEquipo).ToList();
            TempData["ListaEquipos"] = new SelectList(listaEquiposSelect, "GPSCODI", "NOMBREEQUIPO");

            model.ListaFrecuencia = listaFrecuencia;
            model.ListaFrecuenciaMinuto = listaFrecuenciaMinuto;
            return View(model);
        }

        //POST
        [HttpPost]
        public ActionResult Lista(ReporteSegundosFaltantesModel model)
        {
            string mensajeError = "";
            List<ReporteFrecuenciaDescargaDTO> listaFrecuencia = new List<ReporteFrecuenciaDescargaDTO>();

            List<ReporteFrecuenciaDescargaDTO> listaFrecuenciaMinuto = new List<ReporteFrecuenciaDescargaDTO>();



            if (DateTime.Parse(model.FechaIni) > DateTime.Parse(model.FechaFin))
            {
                mensajeError = "La fecha final debe ser mayor que la fecha inicial.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else if (model.IdEquipo == 0)
            {
                mensajeError = "Debe Seleccionar el equipo GPS.";
                TempData["sMensajeExito"] = mensajeError;
            }
            else
            {
                ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
                param.FechaInicial = DateTime.Parse(model.FechaIni);
                param.FechaFinal = DateTime.Parse(model.FechaFin);
                param.IdGPS = model.IdEquipo;

                listaFrecuencia = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);

                listaFrecuenciaMinuto = new ReporteFrecuenciaAppServicio().ObtenerFrecuenciaMinuto(param);
            }

            model.ListaFrecuencia = listaFrecuencia;
            model.ListaFrecuenciaMinuto = listaFrecuenciaMinuto;
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult GeneraArchivo(int idGPS, string gps, DateTime fechaIni, DateTime fechaFin, string Tipo)
        {
            DataSet lds_result;
            string sarchivo;
            if (Tipo == "Frec")
            {
                lds_result = (System.Data.DataSet)new ReporteFrecuenciaAppServicio().Reportes(idGPS, gps, fechaIni, fechaFin, Tipo);
                sarchivo = "Frecuencia.xlsx";
            }
            else if (Tipo == "Ocur")
            {
                for (DateTime date = fechaIni.Date; date <= fechaFin.Date; date = date.AddDays(1))
                {
                    new ReporteFrecuenciaAppServicio().Indicadores(idGPS, gps, date);
                }
                lds_result = (System.Data.DataSet)new ReporteFrecuenciaAppServicio().Reportes(idGPS, gps, fechaIni, fechaFin, Tipo);
                sarchivo = "reporte Ocurrencias.xlsx";
            }
            else
            {
                lds_result = (System.Data.DataSet)new ReporteFrecuenciaAppServicio().Reportes(idGPS, gps, fechaIni, fechaFin, Tipo);
                sarchivo = "reporte " + Tipo + "min.xlsx";
            }

            string fileName = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString("N") + ".xlsx");

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                if (Tipo == "Ocur")
                {
                    //fechaIni = fechaIni.AddSeconds(fechaIni.Second * -1);
                    //fechaFin = fechaFin.AddSeconds(fechaIni.Second * -1 + 59);

                    /*ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
                    param.FechaInicial = fechaIni;
                    //param.FechaFinal = fechaFin;
                    string strFechaFinal = fechaFin.ToString("yyyy-MM-dd HH:mm:59");
                    param.FechaFinal = Convert.ToDateTime(strFechaFinal);
                    param.IdGPS = idGPS;
                    List<ReporteFrecuenciaDescargaDTO> listaFrecuencia = new List<ReporteFrecuenciaDescargaDTO>();
                    listaFrecuencia = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);
                    if (listaFrecuencia != null && listaFrecuencia.Count > 0)
                    {
                        double dblFrecMin = 0;
                        double dblFrecMax = 0;
                        dblFrecMin = Convert.ToDouble(listaFrecuencia[0].Frecuencia);
                        foreach (ReporteFrecuenciaDescargaDTO itemFecha in listaFrecuencia)
                        {
                            if (itemFecha.Frecuencia != "null")
                            {
                                if (Convert.ToDouble(itemFecha.Frecuencia) < dblFrecMin)
                                {

                                }
                            }
                        }
                    }*/

                    worksheet.Cells["A1"].Value = "Reporte de Ocurrencias";
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A2"].Value = "GPS: " + gps;
                    worksheet.Cells["A2"].Style.Font.Bold = true;
                    worksheet.Cells["A4"].Value = "Fecha Inicial: " + fechaIni.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells["A4"].Style.Font.Bold = true;
                    worksheet.Cells["A5"].Value = "Fecha Final: " + fechaFin.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells["A5"].Style.Font.Bold = true;
                    worksheet.Cells["A7"].Value = "Fecha";
                    worksheet.Cells["A7"].Style.Font.Bold = true;
                    worksheet.Cells["B7"].Value = "Hora";
                    worksheet.Cells["B7"].Style.Font.Bold = true;
                    worksheet.Cells["C7"].Value = "Valor";
                    worksheet.Cells["C7"].Style.Font.Bold = true;
                    worksheet.Cells["A9"].Value = "Máxima Frecuencia";
                    worksheet.Cells["A9"].Style.Font.Bold = true;
                    lds_result.Tables[0].DefaultView.RowFilter = "Tipo='FMAX'";
                    if (lds_result.Tables[0].DefaultView.Count == 0)
                    {
                        worksheet.Cells[10, 1].Value = "-------------------------";

                    }
                    else
                    {
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[10, 1].Value = ((DateTime)dr[1]).ToString("dd/MM/yyyy");
                            worksheet.Cells[10, 2].Value = ((DateTime)dr[1]).ToString("HH:mm:ss");
                            worksheet.Cells[10, 3].Value = dr[2];
                        }
                    }

                    worksheet.Cells["A12"].Value = "Minima Frecuencia";
                    worksheet.Cells["A12"].Style.Font.Bold = true;

                    lds_result.Tables[0].DefaultView.RowFilter = "Tipo='FMIN'";
                    if (lds_result.Tables[0].DefaultView.Count == 0)
                    {
                        worksheet.Cells[10, 1].Value = "-------------------------";

                    }
                    else
                    {
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[13, 1].Value = ((DateTime)dr[1]).ToString("dd/MM/yyyy");
                            worksheet.Cells[13, 2].Value = ((DateTime)dr[1]).ToString("HH:mm:ss");
                            worksheet.Cells[13, 3].Value = dr[2];
                        }
                    }

                    worksheet.Cells["A15"].Value = "Transgresiones Súbitas";
                    worksheet.Cells["A15"].Style.Font.Bold = true;
                    int fila = 16;
                    lds_result.Tables[0].DefaultView.RowFilter = "Tipo='TSUB'";

                    if (lds_result.Tables[0].DefaultView.Count == 0)
                    {
                        worksheet.Cells[fila, 1].Value = "-------------------------";
                        fila++;
                    }
                    else
                    {
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[fila, 1].Value = ((DateTime)dr[1]).ToString("dd/MM/yyyy");
                            worksheet.Cells[fila, 2].Value = ((DateTime)dr[1]).ToString("HH:mm:59");
                            worksheet.Cells[fila, 3].Value = dr[2];
                            fila++;
                        }
                    }
                    fila++;
                    worksheet.Cells[fila, 1].Value = "Transgresiones Sostenidas";
                    worksheet.Cells[fila, 1].Style.Font.Bold = true;
                    fila++;

                    lds_result.Tables[0].DefaultView.RowFilter = "Tipo='TSOS'";
                    if (lds_result.Tables[0].DefaultView.Count == 0)
                    {
                        worksheet.Cells[fila, 1].Value = "-------------------------";
                        fila++;
                    }
                    else
                    {
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[fila, 1].Value = ((DateTime)dr[1]).ToString("dd/MM/yyyy");
                            worksheet.Cells[fila, 2].Value = ((DateTime)dr[1]).ToString("HH:mm:ss");
                            worksheet.Cells[fila, 3].Value = dr[2];
                            fila++;
                        }
                    }
                    fila++;
                    worksheet.Cells[fila, 1].Value = "Minutos Faltantes";
                    worksheet.Cells[fila, 1].Style.Font.Bold = true;
                    fila++;
                    worksheet.Cells[fila, 1].Value = "Fecha";
                    worksheet.Cells[fila, 1].Style.Font.Bold = true;
                    worksheet.Cells[fila, 2].Value = "Hora Inicial";
                    worksheet.Cells[fila, 2].Style.Font.Bold = true;
                    worksheet.Cells[fila, 3].Value = "Hora Final";
                    worksheet.Cells[fila, 3].Style.Font.Bold = true;
                    worksheet.Cells[fila, 4].Value = "Minutos Faltantes";
                    worksheet.Cells[fila, 4].Style.Font.Bold = true;
                    fila++;
                    lds_result.Tables[0].DefaultView.RowFilter = "Tipo='FALI'";
                    if (lds_result.Tables[0].DefaultView.Count == 0)
                    {
                        worksheet.Cells[fila, 1].Value = "-------------------------";
                        fila++;
                    }
                    else
                    {
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[fila, 1].Value = ((DateTime)dr[1]).ToString("dd/MM/yyyy");
                            worksheet.Cells[fila, 2].Value = ((DateTime)dr[1]).ToString("HH:mm:ss");
                            worksheet.Cells[fila, 4].Value = dr[2];
                        }
                        lds_result.Tables[0].DefaultView.RowFilter = "Tipo='FALF'";
                        foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                        {
                            DataRow dr = rowView.Row;
                            worksheet.Cells[fila, 3].Value = ((DateTime)dr[1]).ToString("HH:mm:ss");
                        }
                    }
                }
                else
                {
                    worksheet.Cells["A1"].Value = "Estadísticas";
                    worksheet.Cells["A2"].Value = "GPS: " + gps;
                    worksheet.Cells["A4"].Value = "Fecha Inicial: " + fechaIni.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells["A5"].Value = "Fecha Final: " + fechaFin.ToString("dd/MM/yyyy HH:mm:ss");
                    worksheet.Cells["A7"].Value = "Fecha";

                    int i = 8;
                    if (Tipo == "Frec")
                    {
                        worksheet.Cells["B7"].Value = "Hora";
                        worksheet.Cells["C7"].Value = "Frecuencia";
                        worksheet.Cells["D7"].Value = "Tensión";
                        foreach (DataRow dr in lds_result.Tables[0].Rows)
                        {
                            worksheet.Cells[i, 1].Value = ((DateTime)dr[0]).ToString("dd/MM/yyyy");
                            worksheet.Cells[i, 2].Value = ((DateTime)dr[0]).ToString("HH:mm:ss");
                            worksheet.Cells[i, 3].Value = dr[1];
                            worksheet.Cells[i, 4].Value = dr[2];
                            i++;
                        }
                    }
                    else
                    {
                        worksheet.Cells["B7"].Value = "Frecuencia";
                        worksheet.Cells["C7"].Value = "Tensión";
                        foreach (DataRow dr in lds_result.Tables[0].Rows)
                        {
                            worksheet.Cells[i, 1].Value = ((DateTime)dr[0]).ToString("dd/MM/yyyy HH:mm:ss");
                            worksheet.Cells[i, 2].Value = dr[1];
                            worksheet.Cells[i, 3].Value = dr[2];
                            i++;
                        }
                    }
                }
                worksheet.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(fileName));
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            System.IO.File.Delete(fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sarchivo);
        }
        /// <summary>
        /// //Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesReportesFrecuencia.ModuloManualUsuario;
            string nombreArchivo = ConstantesReportesFrecuencia.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesReportesFrecuencia.FolderRaizReportesFrecuenciaModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);

                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");

            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}