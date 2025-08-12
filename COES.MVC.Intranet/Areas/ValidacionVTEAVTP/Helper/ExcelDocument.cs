using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.MVC.Intranet.Areas.Evaluacion.Helper;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.ValidacionVTEAVTP.Helper
{
    public class ExcelDocument
    {
        public static string GenerarReporteBarras(VtpDTO DatosVTP, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = "ReporteBarras" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Barras BRG con Error");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 11;

                #region Barras BRG

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 1. ANALISIS DEL PRECIO DE PEAJE - 1.1. REGISTROS CON ERROR EN EL PPM Y PEAJE";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 6].Value = "BARRAS BRG";
                ws.Cells[6, 6].Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[10, 2].Value = "Codigo";
                ws.Cells[10, 3].Value = "Empresa";
                ws.Cells[10, 4].Value = "Cliente";
                ws.Cells[10, 5].Value = "Barra";
                ws.Cells[10, 6].Value = "Tipo de Usuario";

                ws.Cells[10, 7].Value = "Potencia Coincidente (kW)";
                ws.Cells[10, 8].Value = "Potencia Declarada (kW)";
                ws.Cells[10, 9].Value = "PPM \n(S/ / kW-mes)";
                ws.Cells[10, 10].Value = "VTP PPM \n(S/ / kW-mes)";
                ws.Cells[10, 11].Value = "Error PPM \n(S/ / kW-mes) ";
                ws.Cells[10, 12].Value = "Peaje \n(S/ / kW-mes)";
                ws.Cells[10, 13].Value = "VTP Peaje \n(S/ / kW-mes)";
                ws.Cells[10, 14].Value = "Error Peaje \n(S/ / kW-mes)";      


                ExcelRange rg1 = ws.Cells[10, 2, 10, 14];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TableVtpBrg)
                {
                    ws.Cells[contFila, 2].Value = item.Codigo;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;
                    ws.Cells[contFila, 5].Value = item.Barra;
                    ws.Cells[contFila, 6].Value = item.TipoUsuario;

                    if (item.PotenciaCoincidente.HasValue)
                    {
                        ws.Cells[contFila, 7].Value = item.PotenciaCoincidente;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.PotenciaDeclarada.HasValue)
                    {
                        ws.Cells[contFila, 8].Value = item.PotenciaDeclarada;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.Ppm.HasValue)
                    {
                        ws.Cells[contFila, 9].Value = item.Ppm;
                        ws.Cells[contFila, 9].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.VtpPpm.HasValue)
                    {
                        ws.Cells[contFila, 10].Value = item.VtpPpm;
                        ws.Cells[contFila, 10].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.ErrorPpm.HasValue)
                    {
                        ws.Cells[contFila, 11].Value = item.ErrorPpm;
                        ws.Cells[contFila, 11].Style.Numberformat.Format = "0.0000";
                    }

                    ws.Cells[contFila, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[contFila, 11].Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);

                    if (item.Peaje.HasValue)
                    {
                        ws.Cells[contFila, 12].Value = item.Peaje;
                        ws.Cells[contFila, 12].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.VtpPeaje.HasValue)
                    {
                        ws.Cells[contFila, 13].Value = item.VtpPeaje;
                        ws.Cells[contFila, 13].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.ErrorPeaje.HasValue)
                    {
                        ws.Cells[contFila, 14].Value = item.ErrorPeaje;
                        ws.Cells[contFila, 14].Style.Numberformat.Format = "0.0000";
                    }

                    ws.Cells[contFila, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[contFila, 14].Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);

                    contFila++;
                }


                if (DatosVTP.TableVtpBrg.Count > 0)
                {
                    rg1 = ws.Cells[11, 2,  contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 19;
                ws.Column(8).Width = 19;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 15;

                #endregion


                ws = xlPackage.Workbook.Worksheets.Add("Barras No BRG con Error");
                ws.Cells.Style.Font.Name = "Calibri";

                contFila = 11;

                #region Barras No BRG

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 1. ANALISIS DEL PRECIO DE PEAJE - 1.1. REGISTROS CON ERROR EN EL PPM Y PEAJE";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 6].Value = "BARRAS BRG";
                ws.Cells[6, 6].Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[10, 2].Value = "Codigo";
                ws.Cells[10, 3].Value = "Empresa";
                ws.Cells[10, 4].Value = "Cliente";
                ws.Cells[10, 5].Value = "Barra";
                ws.Cells[10, 6].Value = "Tipo de Usuario";

                ws.Cells[10, 7].Value = "Potencia Coincidente (kW)";
                ws.Cells[10, 8].Value = "Potencia Declarada (kW)";
                ws.Cells[10, 9].Value = "PPM \n(S/ / kW-mes)";
                ws.Cells[10, 10].Value = "VTP PPM \n(S/ / kW-mes)";
                ws.Cells[10, 11].Value = "Error PPM \n(S/ / kW-mes) ";
                ws.Cells[10, 12].Value = "Peaje \n(S/ / kW-mes)";
                ws.Cells[10, 13].Value = "VTP Peaje \n(S/ / kW-mes)";
                ws.Cells[10, 14].Value = "Error Peaje \n(S/ / kW-mes)";


                rg1 = ws.Cells[10, 2, 10, 14];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TableVtpNoBrg)
                {
                    ws.Cells[contFila, 2].Value = item.Codigo;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;
                    ws.Cells[contFila, 5].Value = item.Barra;
                    ws.Cells[contFila, 6].Value = item.TipoUsuario;

                    if (item.PotenciaCoincidente.HasValue)
                    {
                        ws.Cells[contFila, 7].Value = item.PotenciaCoincidente;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.PotenciaDeclarada.HasValue)
                    {
                        ws.Cells[contFila, 8].Value = item.PotenciaDeclarada;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.Ppm.HasValue)
                    {
                        ws.Cells[contFila, 9].Value = item.Ppm;
                        ws.Cells[contFila, 9].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.VtpPpm.HasValue)
                    {
                        ws.Cells[contFila, 10].Value = item.VtpPpm;
                        ws.Cells[contFila, 10].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.ErrorPpm.HasValue)
                    {
                        ws.Cells[contFila, 11].Value = item.ErrorPpm;
                        ws.Cells[contFila, 11].Style.Numberformat.Format = "0.0000";
                    }

                    ws.Cells[contFila, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[contFila, 11].Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);

                    if (item.Peaje.HasValue)
                    {
                        ws.Cells[contFila, 12].Value = item.Peaje;
                        ws.Cells[contFila, 12].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.VtpPeaje.HasValue)
                    {
                        ws.Cells[contFila, 13].Value = item.VtpPeaje;
                        ws.Cells[contFila, 13].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.ErrorPeaje.HasValue)
                    {
                        ws.Cells[contFila, 14].Value = item.ErrorPeaje;
                        ws.Cells[contFila, 14].Style.Numberformat.Format = "0.0000";                       
                    }

                    ws.Cells[contFila, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[contFila, 14].Style.Fill.BackgroundColor.SetColor(Color.SkyBlue);

                    contFila++;
                }


                if (DatosVTP.TableVtpBrg.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 14];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 19;
                ws.Column(8).Width = 19;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;
                ws.Column(13).Width = 15;
                ws.Column(14).Width = 15;

                #endregion

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteBarrasSinAnalizar(VtpDTO DatosVTP, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = "ReporteBarrasSinAnalizar" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Barras sin analizar");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 11;

                #region Barras Sin Analizar

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 1.2 BARRAS SIN ANALIZAR";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;              

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[10, 2].Value = "Codigo";
                ws.Cells[10, 3].Value = "Empresa";
                ws.Cells[10, 4].Value = "Cliente";
                ws.Cells[10, 5].Value = "Barra";
                ws.Cells[10, 6].Value = "Contrato";
                ws.Cells[10, 7].Value = "Tipo de usuario";
                ws.Cells[10, 8].Value = "Precio Potencia \n(S/ / kW-mes)";
                ws.Cells[10, 9].Value = "Potencia Coincidente (kW)";
                ws.Cells[10, 10].Value = "Potencia Declarada (kW)";
                ws.Cells[10, 11].Value = "Peaje Unitario \n(S/ / kW-mes)";
                ws.Cells[10, 12].Value = "Factor Perdida";
               


                ExcelRange rg1 = ws.Cells[10, 2, 10, 12];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TableVtpSinAnalizar)
                {
                    ws.Cells[contFila, 2].Value = item.Codigo;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;
                    ws.Cells[contFila, 5].Value = item.Barra;
                    ws.Cells[contFila, 6].Value = item.Contrato;
                    ws.Cells[contFila, 7].Value = item.TipoUsuario;

                    if (item.PrecioPotencia.HasValue)
                    {
                        ws.Cells[contFila, 8].Value = item.PrecioPotencia;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = "0.0000";
                    }

                    if (item.PotenciaCoincidente.HasValue)
                    {
                        ws.Cells[contFila, 9].Value = item.PotenciaCoincidente;
                        ws.Cells[contFila, 9].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.PotenciaDeclarada.HasValue)
                    {
                        ws.Cells[contFila, 10].Value = item.PotenciaDeclarada;
                        ws.Cells[contFila, 10].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.PeajeUnitario.HasValue)
                    {
                        ws.Cells[contFila, 11].Value = item.PeajeUnitario;
                        ws.Cells[contFila, 11].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.FactorPerdida.HasValue)
                    {
                        ws.Cells[contFila, 12].Value = item.FactorPerdida;
                        ws.Cells[contFila, 12].Style.Numberformat.Format = "0.0000";
                    }                                   

                    contFila++;
                }


                if (DatosVTP.TableVtpSinAnalizar.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 12];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 25;
                ws.Column(6).Width = 15;
                ws.Column(7).Width = 19;
                ws.Column(8).Width = 19;
                ws.Column(9).Width = 15;
                ws.Column(10).Width = 15;
                ws.Column(11).Width = 15;
                ws.Column(12).Width = 15;              

                #endregion             

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteBarrasDiferencia(VtpDTO DatosVTP, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = "ReporteBarrasDiferencia" + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Diferencia Potencia");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 11;

                #region Barras Sin Analizar

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 2. DIFERENCIA ENTRE POTENCIA COINCIDENTE Y DECLARADA";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
             
                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[10, 2].Value = "Codigo";
                ws.Cells[10, 3].Value = "Empresa";
                ws.Cells[10, 4].Value = "Cliente";
                ws.Cells[10, 5].Value = "Barra";                
               
                ws.Cells[10, 6].Value = "Potencia Coincidente (kW)";
                ws.Cells[10, 7].Value = "Potencia Declarada (kW)";
                ws.Cells[10, 8].Value = "Diferencia \n(kW)";            


                ExcelRange rg1 = ws.Cells[10, 2, 10, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TableAnas)
                {
                    ws.Cells[contFila, 2].Value = item.Codigo;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;
                    ws.Cells[contFila, 5].Value = item.Barra;                  
                                      

                    if (item.PotenciaCoincidente.HasValue)
                    {
                        ws.Cells[contFila, 6].Value = item.PotenciaCoincidente;
                        ws.Cells[contFila, 6].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.PotenciaDeclarada.HasValue)
                    {
                        ws.Cells[contFila, 7].Value = item.PotenciaDeclarada;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.Diferencia.HasValue)
                    {
                        ws.Cells[contFila, 8].Value = item.Diferencia;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = "0.0000";
                    }                   

                    contFila++;
                }


                if (DatosVTP.TableAnas.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 30;
                ws.Column(5).Width = 30;
                ws.Column(6).Width = 19;
                ws.Column(7).Width = 19;
                ws.Column(8).Width = 19;            

                #endregion

                xlPackage.Save();
            }

            return archivoExcel;
        }
        public static ExcelRange ObtenerEstiloCelda(ExcelRange rango, int seccion)
        {
            if (seccion == 0)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 1)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Font.Size = 10;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 2)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                rango.Style.Font.Color.SetColor(Color.White);
                rango.Style.Font.Size = 10;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            if (seccion == 3)
            {
                rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                rango.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#BFBFBF"));
                rango.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#000000"));
                rango.Style.Font.Size = 8;
                rango.Style.Font.Bold = true;
                rango.Style.WrapText = true;
                string colorborder = "#000000";
                rango.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                rango.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml(colorborder));
                rango.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            }

            return rango;
        }
    }
}