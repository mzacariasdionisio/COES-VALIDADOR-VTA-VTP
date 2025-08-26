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
        public static string GenerarReporteBarras(VtpDTO DatosVTP, string periodo, string version, string empresa,string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Barras_BRG_con_Error_{0}_{1}", periodo, version) + ".xlsx";

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

                var contFila = 12;

                #region Barras BRG

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 1. ANÁLISIS DEL PRECIO DE PEAJE - 1.1. REGISTROS CON ERROR EN EL PPM Y PEAJE";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 6].Value = "BARRAS BRG";
                ws.Cells[6, 6].Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[9, 2].Value = "Empresa";
                ws.Cells[9, 3].Value = string.IsNullOrEmpty(empresa)? "--TODOS--": empresa;

                ws.Cells[11, 2].Value = "Código VTP";
                ws.Cells[11, 3].Value = "Empresa";
                ws.Cells[11, 4].Value = "Cliente";
                ws.Cells[11, 5].Value = "Barra";
                ws.Cells[11, 6].Value = "Tipo de Usuario";

                ws.Cells[11, 7].Value = "Potencia Coincidente (kW)";
                ws.Cells[11, 8].Value = "Potencia Declarada (kW)";
                ws.Cells[11, 9].Value = "PPM \n(S/ / kW-mes)";
                ws.Cells[11, 10].Value = "VTP PPM \n(S/ / kW-mes)";
                ws.Cells[11, 11].Value = "Error PPM \n(S/ / kW-mes) ";
                ws.Cells[11, 12].Value = "Peaje \n(S/ / kW-mes)";
                ws.Cells[11, 13].Value = "VTP Peaje \n(S/ / kW-mes)";
                ws.Cells[11, 14].Value = "Error Peaje \n(S/ / kW-mes)";      


                ExcelRange rg1 = ws.Cells[11, 2, 11, 14];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 9, 2];
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

                contFila = 12;

                #region Barras No BRG

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Entrada VTP - 1. ANÁLISIS DEL PRECIO DE PEAJE - 1.1. REGISTROS CON ERROR EN EL PPM Y PEAJE";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 9].Merge = true;
                ws.Cells[5, 4, 5, 9].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[6, 6].Value = "BARRAS NO BRG";
                ws.Cells[6, 6].Style.Font.Bold = true;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

                ws.Cells[9, 2].Value = "Empresa";
                ws.Cells[9, 3].Value = string.IsNullOrEmpty(empresa) ? "--TODOS--" : empresa;

                ws.Cells[11, 2].Value = "Código VTP";
                ws.Cells[11, 3].Value = "Empresa";
                ws.Cells[11, 4].Value = "Cliente";
                ws.Cells[11, 5].Value = "Barra";
                ws.Cells[11, 6].Value = "Tipo de Usuario";

                ws.Cells[11, 7].Value = "Potencia Coincidente (kW)";
                ws.Cells[11, 8].Value = "Potencia Declarada (kW)";
                ws.Cells[11, 9].Value = "PPM \n(S/ / kW-mes)";
                ws.Cells[11, 10].Value = "VTP PPM \n(S/ / kW-mes)";
                ws.Cells[11, 11].Value = "Error PPM \n(S/ / kW-mes) ";
                ws.Cells[11, 12].Value = "Peaje \n(S/ / kW-mes)";
                ws.Cells[11, 13].Value = "VTP Peaje \n(S/ / kW-mes)";
                ws.Cells[11, 14].Value = "Error Peaje \n(S/ / kW-mes)";


                rg1 = ws.Cells[11, 2, 11, 14];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 9, 2];
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


                if (DatosVTP.TableVtpNoBrg.Count > 0)
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

            var archivoExcel = string.Format("Reporte_Barras_Sin_Analizar_{0}_{1}", periodo, version) + ".xlsx";

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

                ws.Cells[10, 2].Value = "Código VTP";
                ws.Cells[10, 3].Value = "Empresa";
                ws.Cells[10, 4].Value = "Cliente";
                ws.Cells[10, 5].Value = "Barra";
                ws.Cells[10, 6].Value = "Contrato";
                ws.Cells[10, 7].Value = "Tipo de usuario";
                ws.Cells[10, 8].Value = "Precio Potencia \n(S/ / kW-mes)";
                ws.Cells[10, 9].Value = "Potencia Coincidente (kW)";
                ws.Cells[10, 10].Value = "Potencia Declarada (kW)";
                ws.Cells[10, 11].Value = "Peaje Unitario \n(S/ / kW-mes)";
                ws.Cells[10, 12].Value = "Factor Pérdida";
               


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

            var archivoExcel = string.Format("Reporte_Empresas_Diferencia_Potencia_{0}_{1}", periodo, version) + ".xlsx";

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

                #region Barras Diferencia

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

                ws.Cells[10, 2].Value = "Código VTP";
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

        public static string GenerarReporteSalidaVTP(VtpValidacionDTO DatosVTP, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_VTP_Valorizacion_Compensacion_{0}_{1}", periodo, version) + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VTP Valorizacion");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 11;

                #region Valorizacion

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Salida VTP - Valorización";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 7].Merge = true;
                ws.Cells[5, 4, 5, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                              
                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;

               
                ws.Cells[10, 2].Value = "Empresa";             

                ws.Cells[10, 3].Value = "Potencia Consumida (kW)";
                ws.Cells[10, 4].Value = "Valorización (S/)";
                ws.Cells[10, 5].Value = "Predicción (S/)";
                ws.Cells[10, 6].Value = "Diferencia (S/)";
                ws.Cells[10, 7].Value = "Error (%)";
                ws.Cells[10, 8].Value = "Calidad";
              


                ExcelRange rg1 = ws.Cells[10, 2, 10, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.Valorizacion.TableAnas)
                {
                  
                    ws.Cells[contFila, 2].Value = item.Empresa;                   

                    if (item.PotenciaConsumida.HasValue)
                    {
                        ws.Cells[contFila, 3].Value = item.PotenciaConsumida;
                        ws.Cells[contFila, 3].Style.Numberformat.Format = "0.00";
                    }
                    if (item.Valorizacion.HasValue)
                    {
                        ws.Cells[contFila, 4].Value = item.Valorizacion;
                        ws.Cells[contFila, 4].Style.Numberformat.Format = "0.00";
                    }
                    if (item.Prediccion.HasValue)
                    {
                        ws.Cells[contFila, 5].Value = item.Prediccion;
                        ws.Cells[contFila, 5].Style.Numberformat.Format = "0.00";
                    }
                    if (item.Error.HasValue)
                    {
                        ws.Cells[contFila, 6].Value = item.Error;
                        ws.Cells[contFila, 6].Style.Numberformat.Format = "0.00";
                    }
                    if (item.ErrorPorcentaje.HasValue)
                    {
                        ws.Cells[contFila, 7].Value = item.ErrorPorcentaje;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = "0.00";
                    }

                    ws.Cells[contFila, 8].Value = item.Calidad;

                    if (item.Calidad.ToUpper().Equals("INCORRECTO"))
                    {
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                        ws.Cells[contFila, 2, contFila, 8].Style.Font.Color.SetColor(Color.White);
                    }
                   

                    contFila++;
                }


                if (DatosVTP.Valorizacion.TableAnas.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 15;


                #endregion

                ws = xlPackage.Workbook.Worksheets.Add("VTP Compensacion");
                ws.Cells.Style.Font.Name = "Calibri";

                contFila = 11;

                #region Compensacion

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Salida VTP - Compensación por peaje de transmisión";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 8].Merge = true;
                ws.Cells[5, 4, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;


                ws.Cells[10, 2].Value = "Empresa";

                ws.Cells[10, 3].Value = "Comp. peaje aprox. (S/)";
                ws.Cells[10, 4].Value = "Comp. peaje real (S/)";
                ws.Cells[10, 5].Value = "Predicción (S/)";
                ws.Cells[10, 6].Value = "Diferencia (S/)";
                ws.Cells[10, 7].Value = "Error (%)";
                ws.Cells[10, 8].Value = "Calidad";



                rg1 = ws.Cells[10, 2, 10, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.Peaje.TableAnas)
                {

                    ws.Cells[contFila, 2].Value = item.Empresa;

                    ws.Cells[contFila, 3].Value = item.CompPeajeAprox;
                    ws.Cells[contFila, 3].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 4].Value = item.CompPeajeReal;
                    ws.Cells[contFila, 4].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 5].Value = item.Prediccion;
                    ws.Cells[contFila, 5].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 6].Value = item.Error;
                    ws.Cells[contFila, 6].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 7].Value = item.ErrorPorcentaje;
                    ws.Cells[contFila, 7].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 8].Value = item.Calidad;

                    if (item.Calidad.ToUpper().Equals("INCORRECTO"))
                    {
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                        ws.Cells[contFila, 2, contFila, 8].Style.Font.Color.SetColor(Color.White);
                    }


                    contFila++;
                }


                if (DatosVTP.Peaje.TableAnas.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 15;


                #endregion


                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteCompensacionVTP(VtpValidacionDTO DatosVTP, string periodo, string version, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_VTP_Peaje_{0}_{1}", periodo, version) + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("VTP Compensacion");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 11;

                #region Compensacion

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de Salida VTP - Compensación por peaje de transmisión";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 8].Merge = true;
                ws.Cells[5, 4, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = version;


                ws.Cells[10, 2].Value = "Empresa";

                ws.Cells[10, 3].Value = "Comp. peaje aprox. (S/)";
                ws.Cells[10, 4].Value = "Comp. peaje real (S/)";
                ws.Cells[10, 5].Value = "Predicción (S/)";
                ws.Cells[10, 6].Value = "Diferencia (S/)";
                ws.Cells[10, 7].Value = "Error (%)";
                ws.Cells[10, 8].Value = "Calidad";



                ExcelRange rg1 = ws.Cells[10, 2, 10, 8];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 8, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.Peaje.TableAnas)
                {

                    ws.Cells[contFila, 2].Value = item.Empresa;

                    ws.Cells[contFila, 3].Value = item.CompPeajeAprox;
                    ws.Cells[contFila, 3].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 4].Value = item.CompPeajeReal;
                    ws.Cells[contFila, 4].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 5].Value = item.Prediccion;
                    ws.Cells[contFila, 5].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 6].Value = item.Error;
                    ws.Cells[contFila, 6].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 7].Value = item.ErrorPorcentaje;
                    ws.Cells[contFila, 7].Style.Numberformat.Format = "0.00";

                    ws.Cells[contFila, 8].Value = item.Calidad;

                    if (item.Calidad.ToUpper().Equals("INCORRECTO"))
                    {
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[contFila, 2, contFila, 8].Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                        ws.Cells[contFila, 2, contFila, 8].Style.Font.Color.SetColor(Color.White);
                    }


                    contFila++;
                }


                if (DatosVTP.Valorizacion.TableAnas.Count > 0)
                {
                    rg1 = ws.Cells[11, 2, contFila - 1, 8];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 25;
                ws.Column(4).Width = 25;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 15;


                #endregion


                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteDiferenciaVTPVTEA(VtpVteaDTO DatosVTP, string periodo, string versionVTP, string versionVTEA, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_Diferencias_VTP-VTEA_{0}_{1}_{2}", periodo, versionVTP, versionVTEA) + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Descarga Diferencia VTP-VTEA");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 12;

                #region Diferencia VTP-VTEA

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 4].Value = "Análisis de Datos de VTP y VTEA - Diferencia entre VTP y VTEA";
                ws.Cells[5, 4].Style.Font.Bold = true;
                ws.Cells[5, 4, 5, 8].Merge = true;
                ws.Cells[5, 4, 5, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = versionVTP;
                ws.Cells[9, 2].Value = "Versión de valorización VTEA";
                ws.Cells[9, 3].Value = versionVTEA;

                ws.Cells[11, 2].Value = "Código VTP";
                ws.Cells[11, 3].Value = "Empresa";
                ws.Cells[11, 4].Value = "Cliente";

                ws.Cells[11, 5].Value = "Potencia VTEA (MW)";
                ws.Cells[11, 6].Value = "Potencia VTP (MW)";
                ws.Cells[11, 7].Value = "Diferencia (MW)";
                ws.Cells[11, 8].Value = "Error resp. VTEA (%)";
                ws.Cells[11, 9].Value = "Error resp. VTP (%)";



                ExcelRange rg1 = ws.Cells[11, 2, 11, 9];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 9, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TablesD.OrderBy(p=>p.Empresa))
                {
                    ws.Cells[contFila, 2].Value = item.CodigoVtp;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;                   

                    if (item.PotenciaVtea.HasValue)
                    {
                        ws.Cells[contFila, 5].Value = item.PotenciaVtea;
                        ws.Cells[contFila, 5].Style.Numberformat.Format = "0.0000";
                    }

                    if (item.PotenciaVtp.HasValue)
                    {
                        ws.Cells[contFila, 6].Value = item.PotenciaVtp;
                        ws.Cells[contFila, 6].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.Diferencia.HasValue)
                    {
                        ws.Cells[contFila, 7].Value = item.Diferencia;
                        ws.Cells[contFila, 7].Style.Numberformat.Format = "0.0000";
                    }
                    if (item.ErrorVtea.HasValue)
                    {
                        ws.Cells[contFila, 8].Value = item.ErrorVtea;
                        ws.Cells[contFila, 8].Style.Numberformat.Format = "0.00";
                    }
                    if (item.ErrorVtp.HasValue)
                    {
                        ws.Cells[contFila, 9].Value = item.ErrorVtp;
                        ws.Cells[contFila, 9].Style.Numberformat.Format = "0.00";
                    }

                    contFila++;
                }


                if (DatosVTP.TablesD.Count > 0)
                {
                    rg1 = ws.Cells[12, 2, contFila - 1, 9];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
               

                #endregion

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteComparacionVTEA(VtpVteaDTO DatosVTP, string periodo, string versionVTP, string versionVTEA,string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];
                       
            var archivoExcel = string.Format("Reporte_VTP_Val-VTEA_Zer_{0}_{1}_{2}", periodo, versionVTP, versionVTEA) + ".xlsx";

            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Descarga VTP_Zer VTEA_Val");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 12;

                #region ComparacionVTEA

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 3].Value = "Análisis de Datos de VTP y VTEA - Información Cero/Vacía entre VTP y VTEA";
                ws.Cells[5, 3].Style.Font.Bold = true;
                ws.Cells[5, 3, 5, 6].Merge = true;
                ws.Cells[5, 3, 5, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[6, 3].Value = "Comparación de declaraciones VTEA cero/vacío - VTP";
                ws.Cells[6, 3].Style.Font.Bold = true;
                ws.Cells[6, 3, 6, 6].Merge = true;
                ws.Cells[6, 3, 6, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = versionVTP;
                ws.Cells[9, 2].Value = "Versión de valorización VTEA";
                ws.Cells[9, 3].Value = versionVTEA;

                ws.Cells[11, 2].Value = "Código VTP";
                ws.Cells[11, 3].Value = "Empresa";
                ws.Cells[11, 4].Value = "Cliente";

                ws.Cells[11, 5].Value = "Potencia VTEA (MW)";
                ws.Cells[11, 6].Value = "Potencia VTP (MW)";             


                ExcelRange rg1 = ws.Cells[11, 2, 11, 6];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 9, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TablesXY)
                {
                    ws.Cells[contFila, 2].Value = item.CodigoVtp;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;

                    if (item.PotenciaVtea.HasValue)
                    {
                        ws.Cells[contFila, 5].Value = item.PotenciaVtea;
                        ws.Cells[contFila, 5].Style.Numberformat.Format = "0.0000";
                    }

                    if (item.PotenciaVtp.HasValue)
                    {
                        ws.Cells[contFila, 6].Value = item.PotenciaVtp;
                        ws.Cells[contFila, 6].Style.Numberformat.Format = "0.0000";
                    }                  

                    contFila++;
                }


                if (DatosVTP.TablesXY.Count > 0)
                {
                    rg1 = ws.Cells[12, 2, contFila - 1, 6];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 40;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
              

                #endregion

                xlPackage.Save();
            }

            return archivoExcel;
        }

        public static string GenerarReporteComparacionVTP(VtpVteaDTO DatosVTP, string periodo, string versionVTP, string versionVTEA, string rutaLogo)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.RutaCargaFile];

            var archivoExcel = string.Format("Reporte_VTP_Zer-VTEA_Val_{0}_{1}_{2}", periodo, versionVTP, versionVTEA) + ".xlsx";
            
            FileInfo newFile = new FileInfo(ruta + archivoExcel);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + archivoExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Descarga VTP_Val VTEA_Zer");
                ws.Cells.Style.Font.Name = "Calibri";

                var contFila = 12;

                #region ComparacionVTEA

                if (File.Exists(rutaLogo))
                {
                    Image logo = Image.FromFile(rutaLogo);
                    var excelImage = ws.Drawings.AddPicture("Logo", logo);
                    excelImage.SetPosition(2, 0, 1, 0);
                    excelImage.SetSize(120, 60);
                }

                ws.Cells[5, 3].Value = "Análisis de Datos de VTP y VTEA - Información Cero/Vacía entre VTP y VTEA";
                ws.Cells[5, 3].Style.Font.Bold = true;
                ws.Cells[5, 3, 5, 6].Merge = true;
                ws.Cells[5, 3, 5, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[6, 3].Value = "Comparación de declaraciones VTEA - VTP cero/vacío ";
                ws.Cells[6, 3].Style.Font.Bold = true;
                ws.Cells[6, 3, 6, 6].Merge = true;
                ws.Cells[6, 3, 6, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[7, 2].Value = "Mes de Valorización";
                ws.Cells[7, 3].Value = periodo;
                ws.Cells[8, 2].Value = "Versión de valorización VTP";
                ws.Cells[8, 3].Value = versionVTP;
                ws.Cells[9, 2].Value = "Versión de valorización VTEA";
                ws.Cells[9, 3].Value = versionVTEA;

                ws.Cells[11, 2].Value = "Código VTP";
                ws.Cells[11, 3].Value = "Empresa";
                ws.Cells[11, 4].Value = "Cliente";

                ws.Cells[11, 5].Value = "Potencia VTEA (MW)";
                ws.Cells[11, 6].Value = "Potencia VTP (MW)";


                ExcelRange rg1 = ws.Cells[11, 2, 11, 6];
                ObtenerEstiloCelda(rg1, 2);
                rg1.Style.WrapText = true;

                rg1 = ws.Cells[7, 2, 9, 2];
                ObtenerEstiloCelda(rg1, 2);

                foreach (var item in DatosVTP.TablesYX)
                {
                    ws.Cells[contFila, 2].Value = item.CodigoVtp;
                    ws.Cells[contFila, 3].Value = item.Empresa;
                    ws.Cells[contFila, 4].Value = item.Cliente;

                    if (item.PotenciaVtea.HasValue)
                    {
                        ws.Cells[contFila, 5].Value = item.PotenciaVtea;
                        ws.Cells[contFila, 5].Style.Numberformat.Format = "0.0000";
                    }

                    if (item.PotenciaVtp.HasValue)
                    {
                        ws.Cells[contFila, 6].Value = item.PotenciaVtp;
                        ws.Cells[contFila, 6].Style.Numberformat.Format = "0.0000";
                    }

                    contFila++;
                }


                if (DatosVTP.TablesYX.Count > 0)
                {
                    rg1 = ws.Cells[12, 2, contFila - 1, 6];
                    ObtenerEstiloCelda(rg1, 1);
                }

                rg1.Style.WrapText = true;


                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 40;
                ws.Column(4).Width = 40;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;


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
                //rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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