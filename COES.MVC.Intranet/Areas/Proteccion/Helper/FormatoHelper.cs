using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using COES.MVC.Intranet.Areas.Proteccion.Models;

namespace COES.MVC.Intranet.Areas.Proteccion.Helper
{
    public class FormatoHelper
    {
        /// <summary>
        /// Lee archivo excel de ruta y devuelve matriz de datos o lista de entidades
        /// </summary>
        
        public static string[][] LeerExcelCargado(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                string[][] matriz = new string[0][];
                if (ws == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }
                var filas = ws.Dimension.End.Row;
                var columnas = titulos.Count;
                var columnasReales = ws.Dimension.End.Column;
                matriz = new string[0][];
                if (columnas != columnasReales)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener " + titulos.Count + " columnas" };
                    return matriz;
                }
                
                if (filas > 9)
                {
                    matriz = new string[filas - 8][];
                }
                var filaInicio = 8;
                for (int i = 8; i < filas; i++)
                {
                    matriz[i - filaInicio] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;

                        if(j == 0 && i > 9)
                        {
                            valor = "";
                        }

                        if(j== 6 && i > 9 && !string.IsNullOrEmpty(valor))
                        {

                            var valorCelda = ws.Cells[i + 1, j + 1].Value;
                            if(valorCelda is DateTime)
                            {
                                valor = ((DateTime)valorCelda).ToString("dd/MM/yyyy");
                            }
                            
                        }

                        matriz[i - filaInicio][j] = valor;
                    }
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static string[][] LeerExcelCargadoOtro(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                string[][] matriz = new string[0][];
                if (ws == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }
                var filas = ws.Dimension.End.Row;
                var columnas = titulos.Count;
                var columnasReales = ws.Dimension.End.Column;
                matriz = new string[filas][];
                if (columnas != columnasReales)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener " + titulos.Count + " columnas" };
                    return matriz;
                }

                if (filas > 9)
                {
                    matriz = new string[filas - 8][];
                }
                var filaInicio = 8;
                for (int i = 8; i < filas; i++)
                {
                    matriz[i - filaInicio] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;

                        if (j == 0 && i > 9)
                        {
                            valor = "";
                        }

                        if (j == 6 && i > 9 && !string.IsNullOrEmpty(valor))
                        {

                            var valorCelda = ws.Cells[i + 1, j + 1].Value;
                            if (valorCelda is DateTime)
                            {
                                valor = ((DateTime)valorCelda).ToString("dd/MM/yyyy");
                            }

                        }

                        matriz[i - filaInicio][j] = valor;
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
              

        public static string EncodeNombreEmpresa(StringBuilder cadena)
        {
            cadena.Replace("á", "a");
            cadena.Replace("é", "e");
            cadena.Replace("í", "i");
            cadena.Replace("ó", "o");
            cadena.Replace("ú", "u");
            cadena.Replace("Á", "A");
            cadena.Replace("É", "E");
            cadena.Replace("Í", "I");
            cadena.Replace("Ó", "O");
            cadena.Replace("Ú", "U");

            cadena.Replace("ñ", "?");
            cadena.Replace("Ñ", "?");

            return cadena.ToString();
        }

        public static string DecodeCaracteresEspeciales(string cadena)
        {
            cadena = cadena.Replace("_#38_", "&");
            cadena = cadena.Replace("_#47_", "/");

            return cadena;
        }
    }
}