using COES.MVC.Extranet.Areas.RechazoCarga.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.RechazoCarga.Helper
{
    public class FormatoHelper
    {
        /// <summary>
        /// Lee archivo excel de ruta y devuelve matriz de datos 
        /// </summary>
        public static string[][] LeerExcelCargado(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;
                string[][] matriz = new string[filas][];
                if (columnas != titulos.Count)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener " + titulos.Count + " columnas" };
                    return matriz;
                }
                var columnasNoIncluidas = new List<string>();
                for (int i = 0; i < columnas; i++)
                {
                    if (titulos[i].Trim() != ws.Cells[numeroFilaTitulos, i + 1].Value.ToString().Trim())
                    {
                        columnasNoIncluidas.Add(titulos[i].Trim());
                    }
                }
                if (columnasNoIncluidas.Any())
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "No existen las siguientes columnas en el archivo importado: " + string.Join(",", columnasNoIncluidas) };
                    return matriz;
                }

                if (ws.Cells[filas, 6].Value != null && ws.Cells[filas, 6].Value.ToString().Trim().Equals("Resumen"))
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene el formato correcto" };
                    return matriz;
                }

                for (int i = 0; i < filas; i++)
                {   
                    matriz[i] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {                        
                       

                        if (i > 0 && j == 0)
                        {
                            ws.Cells[i + 1, j + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                            //string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;
                            decimal valorDec = 0;
                            bool esDecimal = decimal.TryParse(ws.Cells[i + 1, j + 1].Value.ToString(), out valorDec);
                            if (esDecimal)
                            {
                                var fecha = DateTime.FromOADate(Convert.ToDouble(valorDec));
                                matriz[i][j] = fecha.ToString("dd/MM/yyyy HH:mm");
                            }
                            else
                            {
                                string valorFecha = ws.Cells[i + 1, j + 1].Value.ToString();
                                if (valorFecha.Length > 16)
                                {
                                    valorFecha = valorFecha.Substring(0, 16);
                                }
                                matriz[i][j] = valorFecha;
                            }
                            
                        }

                        if ( i > 0 && j == 1 )
                        {
                            string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : "0";
                            valor = Convert.ToDecimal(valor).ToString("N3");
                            matriz[i][j] = valor;
                        }

                        
                    }
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static string[][] LeerExcelCargadoCabecera(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;
                string[][] matriz = new string[filas][];
                if (columnas != titulos.Count)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener " + titulos.Count + " columnas" };
                    return matriz;
                }
                var columnasNoIncluidas = new List<string>();
                for (int i = 0; i < columnas; i++)
                {
                    if (titulos[i].Trim() != ws.Cells[numeroFilaTitulos, i + 1].Value.ToString().Trim())
                    {
                        columnasNoIncluidas.Add(titulos[i].Trim());
                    }
                }
                if (columnasNoIncluidas.Any())
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "No existen las siguientes columnas en el archivo importado: " + string.Join(",", columnasNoIncluidas) };
                    return matriz;
                }

                if (ws.Cells[filas, 6].Value != null && ws.Cells[filas, 6].Value.ToString().Trim().Equals("Resumen"))
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene el formato correcto" };
                    return matriz;
                }

                for (int i = 0; i < filas; i++)
                {
                    matriz[i] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        if(i > 1 && (j==7 || j == 8))
                        {
                            if(ws.Cells[i + 1, j + 1].Value != null)
                            {
                                ws.Cells[i + 1, j + 1].Style.Numberformat.Format = "dd/MM/yyyy HH:mm";

                                decimal valorDec = 0;
                                bool esDecimal = decimal.TryParse(ws.Cells[i + 1, j + 1].Value.ToString(), out valorDec);
                                if (esDecimal)
                                {
                                    var fecha = DateTime.FromOADate(Convert.ToDouble(valorDec));
                                    matriz[i][j] = fecha.ToString("dd/MM/yyyy HH:mm");
                                }
                                else
                                {
                                    string valor = ws.Cells[i + 1, j + 1].Value.ToString();
                                    matriz[i][j] = valor;
                                }
                               
                            }
                            else
                            {
                                matriz[i][j] = string.Empty;
                            }
                        }
                        else
                        {
                            string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;
                            matriz[i][j] = valor;
                        }

                        
                    }
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static void BorrarArchivo(String archivo)
        {
            if (System.IO.File.Exists(@archivo))
            {
                try
                {
                    System.IO.File.Delete(@archivo);
                }
                catch (System.IO.IOException e)
                {
                    return;
                }
            }
        }
    }

    public class ErrorHelper
    {
        public string Celda { get; set; }
        public string Valor { get; set; }
        public string Error { get; set; }
    }
}