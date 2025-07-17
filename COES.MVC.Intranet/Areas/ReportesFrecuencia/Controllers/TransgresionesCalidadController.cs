using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;
using COES.Servicios.Aplicacion.IEOD;
using System.Data;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.CPPA.Helper;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class TransgresionesCalidadController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        PR5ReportesAppServicio servicio = new PR5ReportesAppServicio();

        public ActionResult Index()
        {
            base.ValidarSesionUsuario();
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteFrecuenciaAuditModel model = new ReporteFrecuenciaAuditModel();
            //////model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            ////model.bNuevo = true;
            ////model.bEditar = true;
            ////model.bGrabar = true;
            return View(model);
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
            string pathDestino = modulo + ConstantesReportesFrecuencia.FolderRaizAutomatizacionModuloManual;
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

        public ActionResult ObtenerDatos(string fecha)
        {
            DateTime ini = DateTime.ParseExact(fecha + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime dfin = ini.AddMonths(1);
            string inicio = ini.ToString("dd/MM/yyyy"); ;
            string fin = dfin.ToString("dd/MM/yyyy");
            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.CultureInfo("es-ES", false).DateTimeFormat;
            List<EquipoGPSDTO> Equipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSOficial == "S").ToList();
            for (DateTime date = ini.Date; date < dfin.Date; date = date.AddDays(1))
            {
                foreach (EquipoGPSDTO egps in Equipos)
                {
                    try
                    {
                        new ReporteFrecuenciaAppServicio().Indicadores(egps.GPSCodi, egps.NombreEquipo, date);
                    }
                    catch { }
                }
            }

            DataSet lds_result = (System.Data.DataSet)new ReporteFrecuenciaAppServicio().TransgresionMensual(inicio, fin);
            string jsonResult = JsonConvert.SerializeObject(lds_result.Tables[0], Formatting.Indented);
            return Content(jsonResult, "application/json");
        }
        [HttpPost]

        public ActionResult ReporteOficial(string fecha)
        {
            
            DateTime ini = DateTime.ParseExact(fecha + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime dfin = ini.AddMonths(1);
            string inicio = ini.ToString("dd/MM/yyyy"); ;
            string fin = dfin.ToString("dd/MM/yyyy");

            System.Globalization.DateTimeFormatInfo dtfi = new System.Globalization.CultureInfo("es-ES", false).DateTimeFormat;
            List<EquipoGPSDTO> Equipos = new EquipoGPSAppServicio().GetListaEquipoGPS().Where(x => x.GPSOficial == "S").ToList();
            for (DateTime date = ini.Date; date < dfin.Date; date = date.AddDays(1))
            {
                foreach (EquipoGPSDTO egps in Equipos)
                {
                    try
                    {
                        new ReporteFrecuenciaAppServicio().Indicadores(egps.GPSCodi, egps.NombreEquipo, date);
                    }
                    catch { }
                }
            }

            DataSet lds_result = (System.Data.DataSet)new ReporteFrecuenciaAppServicio().TransgresionMensual(inicio, fin);

            string sarchivo = "ReporteOficial.xlsx";
            string fileName = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString("N") + ".xlsx");

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Reporte");
                worksheet.Cells["B1"].Value = "Reporte de " + dtfi.GetMonthName(ini.Month) + " " + ini.Year.ToString();
                worksheet.Cells["B1"].Style.Font.Bold = true;
                worksheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells["B1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells["B1"].Style.Font.Size = 16;
                worksheet.Cells["B1:G1"].Merge = true;

                worksheet.Cells["B3"].Value = "Número de ocurrencias";
                worksheet.Cells["B3:F4"].Style.Font.Bold = true;

                var headerRange1 = worksheet.Cells["B4:F4"];
                headerRange1.Style.Font.Bold = true;
                headerRange1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange1.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                headerRange1.Style.Font.Color.SetColor(System.Drawing.Color.White);

                worksheet.Cells["B4"].Value = "IDENTIFICADOR";
                worksheet.Cells["C4"].Value = "CODIGO";
                worksheet.Cells["D4"].Value = "V_SOSTENIDA_1";
                worksheet.Cells["E4"].Value = "V_SOSTENIDA_2";
                worksheet.Cells["F4"].Value = "V_SUBITA";
                lds_result.Tables[0].DefaultView.RowFilter = "R=1";
                int i = 5;

                if (lds_result.Tables[0].DefaultView.Count > 0)
                {
                    foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                    {
                        DataRow dr = rowView.Row;
                        worksheet.Cells[i, 2].Value = dr[1];
                        worksheet.Cells[i, 3].Value = dr[2];
                        worksheet.Cells[i, 4].Value = string.IsNullOrEmpty(dr[4].ToString()) ? 0 : dr[4];
                        worksheet.Cells[i, 5].Value = string.IsNullOrEmpty(dr[5].ToString()) ? 0 : dr[5];
                        worksheet.Cells[i, 6].Value = string.IsNullOrEmpty(dr[3].ToString()) ? 0 : dr[3];

                        worksheet.Cells[i, 3, i, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                        i++;
                    }
                }
                var range = worksheet.Cells["B4:F" + (i - 1).ToString()];
                range.Style.Border.DiagonalDown = false;
                range.Style.Border.DiagonalUp = false;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                i++;

                worksheet.Cells[i, 2].Value = "Listado de ocurrencias";
                worksheet.Cells["B" + i.ToString() + ":G" + (i + 1).ToString()].Style.Font.Bold = true;
                i++;
                int j = i;
                worksheet.Cells[i, 2].Value = "Punto de Medición";
                worksheet.Cells[i, 3].Value = "CODIGO";
                worksheet.Cells[i, 4].Value = "INDICADOR";
                worksheet.Cells[i, 5].Value = "FECHA";
                worksheet.Cells[i, 6].Value = "INTERVALO";
                worksheet.Cells[i, 7].Value = "VALOR";

                // Estilo para los encabezados
                var headerRange2 = worksheet.Cells["B" + i.ToString() + ":G" + i.ToString()];
                headerRange2.Style.Font.Bold = true;
                headerRange2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                headerRange2.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                headerRange2.Style.Font.Color.SetColor(System.Drawing.Color.White);

                lds_result.Tables[0].DefaultView.RowFilter = "R=2";
                i++;
                if (lds_result.Tables[0].DefaultView.Count > 0)
                {
                    foreach (DataRowView rowView in lds_result.Tables[0].DefaultView)
                    {
                        DataRow dr = rowView.Row;
                        if (dr[3] == DBNull.Value || string.IsNullOrEmpty(dr[3].ToString()))
                        {
                            // Duplicar la fila con "T(sostenida)"
                            worksheet.Cells[i, 2].Value = dr[1];
                            worksheet.Cells[i, 3].Value = dr[2];
                            worksheet.Cells[i, 4].Value = "T(sostenida)";
                            worksheet.Cells[i, 5].Value = dr[4];
                            worksheet.Cells[i, 6].Value = dr[5];
                            worksheet.Cells[i, 7].Value = dr[6];
                            i++;

                            // Duplicar la fila con "S(súbita)"
                            worksheet.Cells[i, 2].Value = dr[1];
                            worksheet.Cells[i, 3].Value = dr[2];
                            worksheet.Cells[i, 4].Value = "S(súbita)";
                            worksheet.Cells[i, 5].Value = dr[4];
                            worksheet.Cells[i, 6].Value = dr[5];
                            worksheet.Cells[i, 7].Value = dr[6];
                            i++;
                        }
                        else
                        {
                            worksheet.Cells[i, 2].Value = dr[1];
                            worksheet.Cells[i, 3].Value = dr[2];
                            worksheet.Cells[i, 4].Value = dr[3].ToString() == "O" ? "T(sostenida)" : (dr[3].ToString() == "U" ? "S(súbita)" : dr[3]);
                            worksheet.Cells[i, 5].Value = dr[4];
                            worksheet.Cells[i, 6].Value = dr[5];
                            worksheet.Cells[i, 7].Value = dr[6];
                            i++;
                        }
                    
                    }
                }

                range = worksheet.Cells["B" + j.ToString() + ":G" + (i - 1).ToString()];
                range.Style.Border.DiagonalDown = false;
                range.Style.Border.DiagonalUp = false;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(fileName));
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileName);
            System.IO.File.Delete(fileName);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sarchivo);
        }
    }
}
