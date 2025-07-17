using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper
{
    /// <summary>
    /// Clase que permite exportar el reporte de cumplimiento servicio RPF
    /// </summary>
    public class ExcelDocument
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCarga(List<ServicioRpfDTO> list)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaExcel);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteCargaRPF);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteCargaRPF);
            }

            int row = 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                foreach (ServicioRpfDTO item in list)
                {
                    ws.Cells[row, 2].Value = (item.EMPRNOMB != null) ? item.EMPRNOMB : string.Empty;
                    ws.Cells[row, 3].Value = (item.EQUINOMB != null) ? item.EQUINOMB : string.Empty;
                    ws.Cells[row, 4].Value = (item.EQUIABREV != null) ? item.EQUIABREV : string.Empty;
                    ws.Cells[row, 5].Value = item.PTOMEDICODI.ToString();
                    ws.Cells[row, 6].Value = (item.INDICADOR != null) ? item.INDICADOR : string.Empty;

                    row++;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite elaborar el reporte de consistencia de datos
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteConsistencia(List<ServicioRpfDTO> list, DateTime fecha)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + NombreArchivo.PlantillaConsistenciaRPF);
            FileInfo newFile = new FileInfo(ruta + NombreArchivo.ReporteConsistenciaRPF);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivo.ReporteConsistenciaRPF);
            }

            int row = 5;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                ws.Cells[2, 3].Value = fecha.ToString(Constantes.FormatoFecha);

                foreach (ServicioRpfDTO item in list)
                {
                    ws.Cells[row, 2].Value = (item.EMPRNOMB != null) ? item.EMPRNOMB : string.Empty;
                    ws.Cells[row, 3].Value = (item.EQUINOMB != null) ? item.EQUINOMB : string.Empty;
                    ws.Cells[row, 4].Value = (item.EQUIABREV != null) ? item.EQUIABREV : string.Empty;
                    ws.Cells[row, 5].Value = (item.FechaCarga != null) ? item.FechaCarga : string.Empty;
                    ws.Cells[row, 6].Value = item.Consistencia.ToString();
                    ws.Cells[row, 7].Value = (item.EstadoOperativo != null) ? item.EstadoOperativo : string.Empty;
                    ws.Cells[row, 8].Value = (item.EstadoInformacion != null) ? item.EstadoInformacion : string.Empty;

                    row++;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el formato de potencias máximas
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReportePotencia(List<ServicioRpfDTO> list)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();

            FileInfo template = new FileInfo(ruta + Constantes.PlantillaPotencia);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteRPFPotencia);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteRPFPotencia);
            }

            int row = 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                foreach (ServicioRpfDTO item in list)
                {
                    ws.Cells[row, 2].Value = (item.EMPRNOMB != null) ? item.EMPRNOMB : string.Empty;
                    ws.Cells[row, 3].Value = (item.EQUINOMB != null) ? item.EQUINOMB : string.Empty;
                    ws.Cells[row, 4].Value = (item.EQUIABREV != null) ? item.EQUIABREV : string.Empty;
                    ws.Cells[row, 5].Value = item.PTOMEDICODI.ToString();
                    ws.Cells[row, 6].Value = (item.POTENCIAMAX != null) ? item.POTENCIAMAX.ToString(Constantes.FormatoDecimal) : string.Empty;

                    row++;
                }

                xlPackage.Save();
            }

        }


        /// <summary>
        /// Permite generar el reporte de cumplimiento de la evaluacion RPF
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCumplimiento(List<ServicioRpfDTO> list)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaCumplimiento);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteCumplimientoRPF);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteCumplimientoRPF);
            }

            int row = 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                foreach (ServicioRpfDTO item in list)
                {
                    ws.Cells[row, 2].Value = (item.EMPRNOMB != null) ? item.EMPRNOMB : string.Empty;
                    ws.Cells[row, 3].Value = (item.EQUINOMB != null) ? item.EQUINOMB : string.Empty;
                    ws.Cells[row, 4].Value = (item.EQUIABREV != null) ? item.EQUIABREV : string.Empty;
                    ws.Cells[row, 5].Value = item.PTOMEDICODI.ToString();
                    ws.Cells[row, 6].Value = (item.HORAINICIO != null) ? item.HORAINICIO : string.Empty;
                    ws.Cells[row, 7].Value = (item.HORAFIN != null) ? item.HORAFIN : string.Empty;
                    ws.Cells[row, 8].Value = item.PORCENTAJE.ToString(Constantes.FormatoNumero) + Constantes.CaracterPorcentaje;
                    ws.Cells[row, 9].Value = (item.INDCUMPLIMIENTO != null) ? item.INDCUMPLIMIENTO : string.Empty;

                    row++;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento de la evaluacion RPF
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarReporteCumplimientoFalla(List<ServicioRpfDTO> list)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaCumplimientoFalla);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReporteCumplimientoRPFFalla);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReporteCumplimientoRPFFalla);
            }

            int row = 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                foreach (ServicioRpfDTO item in list)
                {
                    ws.Cells[row, 2].Value = (item.EMPRNOMB != null) ? item.EMPRNOMB : string.Empty;
                    ws.Cells[row, 3].Value = (item.EQUINOMB != null) ? item.EQUINOMB : string.Empty;
                    ws.Cells[row, 4].Value = (item.EQUIABREV != null) ? item.EQUIABREV : string.Empty;
                    ws.Cells[row, 5].Value = item.PTOMEDICODI.ToString();
                    ws.Cells[row, 6].Value = item.PORCENTAJE.ToString(Constantes.FormatoNumero) + Constantes.CaracterPorcentaje;
                    ws.Cells[row, 7].Value = (item.INDCUMPLIMIENTO != null) ? item.INDCUMPLIMIENTO : string.Empty;

                    row++;
                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el archivo con los datos de las frecuencias
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoFrecuencias(List<ServicioGps> list)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + NombreArchivo.PlantillaFrecuenciaRPF);
            FileInfo newFile = new FileInfo(ruta + NombreArchivo.ReporteFrecuenciasRPF);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivo.ReporteFrecuenciasRPF);
            }

            int row = 4;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                if (list.Count > 0)
                {
                    ws.Cells[1, 2].Value = list[0].Fecha.ToString(Constantes.FormatoFecha);
                }

                foreach (ServicioGps item in list)
                {
                    ws.Cells[row, 1].Value = item.Fecha.ToString(Constantes.FormatoHora);
                    ws.Cells[row, 2].Value = item.Frecuencia;
                    row++;
                }

                xlPackage.Save();
            }
        }

        public static void GenerarArchivoEvaFrecuencias(List<ServicioGps> list, string filename, DateTime fecha)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
            FileInfo template = new FileInfo(ruta + NombreArchivo.PlantillaEvaFrecuenciaXLS);
            FileInfo newFile = new FileInfo(ruta + filename);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + filename);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["Frecuencia"];

                ws.Cells[4, 1].Value = string.Format("Fecha Inicial: {0} 00:00:00", fecha.ToString("yyyy-MM-dd"));
                ws.Cells[5, 1].Value = string.Format("Fecha Final: {0} 23:59:59", fecha.ToString("yyyy-MM-dd"));


                int col = 7;
                int row = 8;

                int index = 21599;
                if (list.Count == 86400)
                {
                    for (int i = 0; i <= index; i++)
                    {
                        ws.Cells[row + i, col + 0].Value = list[i + index * 0].Frecuencia;
                        ws.Cells[row + i, col + 1].Value = list[i + index * 1 + 1].Frecuencia;
                        ws.Cells[row + i, col + 2].Value = list[i + index * 2 + 2].Frecuencia;
                        ws.Cells[row + i, col + 3].Value = list[i + index * 3 + 3].Frecuencia;
                    }

                }

                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el reporte con los datos a utilizar en Matlab
        /// </summary>
        /// <param name="listConfiguration"></param>
        /// <param name="listDatos"></param>
        public static void GenerarReporteRangoHoras(List<ServicioRpfDTO> listConfiguration, List<RegistrorpfDTO> listDatos)
        {
            try
            {
                string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF].ToString();
                FileInfo newFile = new FileInfo(ruta + NombreArchivo.RangoHorasRPF);

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(ruta + NombreArchivo.RangoHorasRPF);
                }

                using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add(Constantes.HojaReporteExcel);

                    if (ws != null)
                    {
                        int column = 1;
                        int index = 1;
                        foreach (ServicioRpfDTO item in listConfiguration)
                        {
                            index = 1;
                            ws.Cells[index, column].Value = item.EMPRNOMB.Trim();
                            ws.Cells[index + 1, column].Value = item.EQUINOMB.Trim();
                            ws.Cells[index + 2, column].Value = item.EQUIABREV.Trim();
                            ws.Cells[index, column, index, column + 2].Merge = true;
                            ws.Cells[index + 1, column, index + 1, column + 2].Merge = true;
                            ws.Cells[index + 2, column, index + 2, column + 2].Merge = true;
                            ws.Cells[index + 3, column].Value = TextosPantalla.Hora;
                            ws.Cells[index + 3, column + 1].Value = TextosPantalla.Frecuencia;
                            ws.Cells[index + 3, column + 2].Value = TextosPantalla.Potencia;

                            List<RegistrorpfDTO> datos = listDatos.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).
                                OrderBy(y => y.FECHAHORA).ToList();

                            index = index + 4;
                            int contador = 0;
                            foreach (RegistrorpfDTO dato in datos)
                            {
                                ws.Cells[index, column].Value = dato.HORAINICIO.AddSeconds(contador).ToString(Constantes.FormatoHora);
                                ws.Cells[index, column + 1].Value = dato.FRECUENCIA;
                                ws.Cells[index, column + 2].Value = dato.POTENCIA;

                                index++;
                                contador++;
                            }

                            column = column + 3;
                        }

                        string border = Constantes.ColorPlomo;
                        ExcelRange rg = ws.Cells[1, 1, 304, column - 1];
                        rg.Style.Font.Size = 9;
                        rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(border));
                        rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(border));
                        rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(border));
                        rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(border));

                        rg = ws.Cells[1, 1, 4, column - 1];
                        rg.Style.Font.Bold = true;
                        rg.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                        rg = ws.Cells[1, 1, 3, column - 1];
                        rg.Style.Font.Size = 8;
                    }

                    xlPackage.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}