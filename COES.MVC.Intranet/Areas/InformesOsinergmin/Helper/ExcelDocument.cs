using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace COES.MVC.Intranet.Areas.InformesOsinergmin.Helper
{
    public class ConstantesCMR
    {
        public const string DatoNegativo = "No se permiten dato negativo: hoja {0}, fila {1}, columna {2}";
        public const string FechaHojasNoCoincide = "Las fechas en 5.2 y 5.10 no coinciden";
        public const string FechaNoCoincide = "Fecha no corresponde al archivo hoja {0}, fila {1}, columna {2}";
        public const string DiaInvalido = "El día no es valido hoja {0}, fila {1}, columna {2}";
        public const string FormatoIncorrecto = "Una de las fechas del archivo tiene un formato incorrecto";
        public const string Error = "Hubo un error";
        public const string FaltaHojas = "Falta una de las hojas 5.2 o 5.10";
        public const string ErrorOrden = "Error en 5.10, el día no cumple la secuencia: fila {0}, columna {1}";
        public const string CeldaVacia = "Error en {0}, fecha nula: fila {1}, columna {2}";
        public const string CeldaLetra = "Error en {0}, Solo se permiten números: fila {1}, columna {2}";
        public const string CeldaRealVacia = "Error en {0}, debe ingresar valor: fila {1}, columna {2}";
        public const string CeldaVaciaDia = "Error en {0}, debe ingresar día: fila {1}, columna {2}";
       
    }

    public class ExcelDocument
    {
        public static void GenerarReporteRPF(List<PsuRpfhidDTO> list, DateTime fecInicio, DateTime fecFin, string path)
        {
            string file = path + "InformesRPF.xlsx";
            string fileTemplate = path + "Plantilla.xlsx";
            
            FileInfo template = new FileInfo(fileTemplate);
            FileInfo newFile = new FileInfo(file);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["INFORMES RPF"];

                if (ws != null)
                {
                    ExcelRange rg = ws.Cells[2, 3, 3, 3];
                    int row = 7;

                    ws.Cells[3, 3].Value = fecInicio.ToString("MMM yyyy");
                    ws.Cells[4, 3].Value = fecFin.ToString("MMM yyyy");

                    foreach (PsuRpfhidDTO item in list)
                    {
                        ws.Cells[row, 2].Value = item.Rpfhidfecha.ToString("MMM yyyy", new System.Globalization.CultureInfo("es-Es"));
                        ws.Cells[row, 3].Value = item.Rpfenetotal;
                        ws.Cells[row, 4].Value = item.Rpfpotmedia;
                        ws.Cells[row, 5].Value = item.Potindhidra;
                        ws.Cells[row, 6].Value = item.Eneindhidra;

                        rg = ws.Cells[row, 2, row, 6];
                        rg.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#246189"));

                        row++;
                    }
                    rg = ws.Cells[7, 2, row - 1, 6];
                    rg.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Left.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Right.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Top.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                    rg.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    rg.Style.Border.Bottom.Color.SetColor(ColorTranslator.FromHtml("#9F9F9F"));
                }
                xlPackage.Save();
            }
        }

        public static List<PsuDesvcmgsncDTO> ObtenerCMRDiario(string fileName)
        {
            List<PsuDesvcmgsncDTO> list = new List<PsuDesvcmgsncDTO>();
            FileInfo file = new FileInfo(fileName);

            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["5.10"];
                if (ws != null) {
                    DateTime mesInforme = Convert.ToDateTime(ws.Cells[4, 2].Value);
                    int numDiasMes = DateTime.DaysInMonth(mesInforme.Year, mesInforme.Month);
                    for (int i = 0; i < numDiasMes; i++)
                    {
                        PsuDesvcmgsncDTO diario = new PsuDesvcmgsncDTO();
                        DateTime f = Convert.ToDateTime(ws.Cells[i + 4, 2].Value);
                        f = f.AddDays(-f.Day);
                        f = f.AddDays(int.Parse(ws.Cells[i + 4, 3].Value.ToString()));
                        diario.Desvfecha = f;
                        diario.Cmgsnc = decimal.Parse(ws.Cells[i + 4, 6].Value.ToString());
                        list.Add(diario);
                    }
                }
            }
            return list;
        }

        public static PsuDesvcmgDTO ObtenerCMRMensual(string fileName)
        {
            PsuDesvcmgDTO registro = new PsuDesvcmgDTO();
            FileInfo file = new FileInfo(fileName);
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["5.2"];
                if (ws != null)
                {
                    string s = ws.Cells[4, 3].Value.ToString() + " " + ws.Cells[4, 4].Value.ToString();
                    DateTime mesInforme = DateTime.ParseExact(s, "yyyy MMM", CultureInfo.CurrentCulture);
                    registro.Desvfecha = mesInforme;
                    registro.Cmgrpunta = decimal.Parse(ws.Cells[4, 8].Value.ToString());
                    registro.Cmgrmedia = decimal.Parse(ws.Cells[4, 9].Value.ToString());
                    registro.Cmgrbase = decimal.Parse(ws.Cells[4, 10].Value.ToString());
                }
            }
            return registro;
        }

        public static List<string> validacion(string fileName, DateTime fechaMes)
        {
            FileInfo file = new FileInfo(fileName);
            List<string> validaciones = new List<string>();
            using (ExcelPackage xlPackage = new ExcelPackage(file))
            {
                ExcelWorksheet ws2 = xlPackage.Workbook.Worksheets["5.2"];
                ExcelWorksheet ws10 = xlPackage.Workbook.Worksheets["5.10"];
                if (ws2 != null && ws10 != null)
                {
                    try
                    {
                        string s = ws2.Cells[4, 3].Value.ToString() + " " + ws2.Cells[4, 4].Value.ToString();
                        DateTime fech2 = DateTime.ParseExact(s, "yyyy MMM", CultureInfo.InvariantCulture);


                        if (fechaMes.ToString("yyyy MM") != fech2.ToString("yyyy MM"))
                            validaciones.Add(string.Format(ConstantesCMR.FechaNoCoincide, "5.2", 4, 3));

                        if (decimal.Parse(ws2.Cells[4, 8].Value.ToString()) < 0)
                            validaciones.Add(string.Format(ConstantesCMR.DatoNegativo, "5.2", 4, 8));
                        if (decimal.Parse(ws2.Cells[4, 9].Value.ToString()) < 0)
                            validaciones.Add(string.Format(ConstantesCMR.DatoNegativo, "5.2", 4, 9));
                        if (decimal.Parse(ws2.Cells[4, 10].Value.ToString()) < 0)
                            validaciones.Add(string.Format(ConstantesCMR.DatoNegativo, "5.2", 4, 10));

                        DateTime fech10 = Convert.ToDateTime(ws10.Cells[4, 2].Value);
                        if (fech10.ToString() != fech2.ToString())
                            validaciones.Add(ConstantesCMR.FechaHojasNoCoincide);

                        int numDiasMes = DateTime.DaysInMonth(fech10.Year, fech10.Month);
                        for (int i = 0; i < numDiasMes; i++)
                        {
                            if (ws10.Cells[i + 4, 3].Value != null)
                            {
                                int nroDia = 0;

                                if (int.TryParse(ws10.Cells[i + 4, 3].Value.ToString(), out nroDia))
                                {
                                    if (int.Parse(ws10.Cells[i + 4, 3].Value.ToString()) != i + 1)
                                        validaciones.Add(string.Format(ConstantesCMR.ErrorOrden, i + 4, 3));
                                    
                                    if (ws10.Cells[i + 4, 2].Value != null)
                                    {
                                        DateTime f = Convert.ToDateTime(ws10.Cells[i + 4, 2].Value);
                                        if (f.ToString("yyyy MM") != fech2.ToString("yyyy MM"))
                                            validaciones.Add(string.Format(ConstantesCMR.FechaNoCoincide, "5.10", i + 4, 2));
                                    }
                                    else 
                                    {
                                        validaciones.Add(string.Format(ConstantesCMR.CeldaVacia, "5.10", i + 4, 2));
                                    }
                                    
                                    //if (int.Parse(ws10.Cells[i + 4, 3].Value.ToString()) <= 0 ||
                                    //    int.Parse(ws10.Cells[i + 4, 3].Value.ToString()) > numDiasMes)
                                    //    validaciones.Add(string.Format(ConstantesCMR.DiaInvalido, "5.10", i + 4, 3));
                                    
                                    decimal real = 0;

                                    if (ws10.Cells[i + 4, 6].Value != null)
                                    {
                                        if (decimal.TryParse(ws10.Cells[i + 4, 6].Value.ToString(), out real))
                                        {
                                            if (real < 0)
                                                validaciones.Add(string.Format(ConstantesCMR.DatoNegativo, "5.10", i + 4, 6));
                                        }
                                        else
                                        {
                                            validaciones.Add(string.Format(ConstantesCMR.CeldaLetra, "5.10", i + 4, 6));
                                        }
                                    }
                                    else 
                                    {
                                        validaciones.Add(string.Format(ConstantesCMR.CeldaRealVacia, "5.10", i + 4, 6));
                                    }

                                }
                                else
                                {
                                    validaciones.Add(string.Format(ConstantesCMR.CeldaLetra, "5.10", i + 4, 3));
                                }
                            }
                            else
                            {
                                validaciones.Add(string.Format(ConstantesCMR.CeldaVaciaDia, "5.10", i + 4, 3));
                            }
                        }
                    }
                    catch (NullReferenceException)
                    {
                        validaciones.Add(ConstantesCMR.FormatoIncorrecto);
                    }
                    catch (Exception e)
                    {
                        validaciones.Add(ConstantesCMR.Error);
                    }
                }
                else
                {
                    validaciones.Add(ConstantesCMR.FaltaHojas);
                }
            }
            return validaciones;
        }

        public static string ObtieneListaValidacion(List<string> errors)
        {
            StringBuilder str = new StringBuilder();

            str.Append("<ul>");

            foreach (string error in errors)
            {
                str.Append(string.Format("<li>{0}</li>", error));
            }

            str.Append("</ul>");

            return str.ToString();
        }
    }
}