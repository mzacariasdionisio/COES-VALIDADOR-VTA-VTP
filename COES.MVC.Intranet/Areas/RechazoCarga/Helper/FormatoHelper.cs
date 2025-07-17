using COES.MVC.Intranet.Areas.RechazoCarga.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Text;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.RechazoCarga.Helper
{
    public class FormatoHelper
    {
        /// <summary>
        /// Lee archivo excel de ruta y devuelve matriz de datos o lista de entidades
        /// </summary>
        public static string[][] LeerExcelCargadoCuadroProg(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
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
                    //respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene el formato correcto" };
                    //return matriz;
                    //Se redimensiona la matriz y reduce la cantidad de filas
                    filas = filas - 1;
                    matriz = new string[filas][];
                }

                for (int i = 0; i < filas; i++)
                {
                    matriz[i] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        
                        if(i > 1 && (j == 13 || j == 14 || j == 16 || j == 17))
                        {
                            var cell = ws.Cells[i + 1, j + 1];

                            if(cell.Value!=null && cell.Text.Trim() != string.Empty)
                            {
                                //string[] valorCeldas = cell.Value.ToString().Split(' ');
                                //matriz[i][j] = valorCeldas.Count() > 1 ? valorCeldas[1].Trim() : string.Empty;
                                matriz[i][j] = FormatoHora(cell.Text.Trim());
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
                    //respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene el formato correcto" };
                    //return matriz;
                    //Se redimensiona la matriz y reduce la cantidad de filas
                    filas = filas - 1;
                    matriz = new string[filas][];
                }

                for (int i = 0; i < filas; i++)
                {
                    matriz[i] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;
                        matriz[i][j] = valor;
                    }
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static string[][] LeerExcelCargadoParametrosERAC(string rutaCompletaArchivo, List<string> titulos, int numeroFilaTitulos, out Respuesta respuesta)
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
                    //respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene el formato correcto" };
                    //return matriz;
                    //Se redimensiona la matriz y reduce la cantidad de filas
                    filas = filas - 1;
                    matriz = new string[filas][];
                }

                for (int i = 0; i < filas; i++)
                {
                    matriz[i] = new string[columnas];
                    for (int j = 0; j < columnas; j++)
                    {
                        string valor = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : string.Empty;
                        matriz[i][j] = valor;

                        if (i > 2 && j == 11)
                        {
                            matriz[i][j] = TipoEsquema(valor);
                        }
                    }
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static List<RcaDemandaUsuarioDTO> LeerExcelCargadoDemandaUsuario(string rutaCompletaArchivo, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            var listaDemandaUsuario = new List<RcaDemandaUsuarioDTO>();
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;
                string[][] matriz = new string[filas][];

                var indiceHP = 0;
                var indiceHFP = 0;
                var fechaArchivo = DateTime.Now;

                var valor = decimal.Zero;

                if (ws.Cells[ConstantesRechazoCarga.filaFechaArchivoDemanda, 3].Value == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene fecha de proceso" };
                    return listaDemandaUsuario;
                }

                //Validamos si las etiquetas de HP y HFP existen en el archivo
                for (int j = 0; j < columnas; j++)
                {
                    if (ws.Cells[ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda, j + 1].Value != null)
                    {
                        if (ws.Cells[ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda, j + 1].Value.Equals("HP") || ws.Cells[ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda, j + 1].Value.Equals("HFP"))
                        {
                            if (ws.Cells[ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda, j + 1].Value.Equals("HP"))
                            {
                                indiceHP = j;
                            }
                            if (ws.Cells[ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda, j + 1].Value.Equals("HFP"))
                            {
                                indiceHFP = j;
                            }
                            if (indiceHP > 0 && indiceHFP > 0)
                            {
                                break;
                            }
                        }
                    }

                }
                if (indiceHP == 0 || indiceHFP == 0)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene Cabecera Tipo Demanda" };
                    return listaDemandaUsuario;
                }

                //Lectura de Archivo
                for (int i = 0; i < filas; i++)
                {
                    if (i == (ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda - 1))
                    {
                        if (ws.Cells[i + 1, 3].Value != null)
                        {
                            var sinFormatoFecha = long.Parse(ws.Cells[i + 1, 3].Value.ToString());
                            fechaArchivo = sinFormatoFecha > 0 ? DateTime.FromOADate(sinFormatoFecha) : DateTime.Now;
                        }
                    }

                    if (i == (ConstantesRechazoCarga.filaTipoDemandaArchivoDemanda - 1))
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            if (ws.Cells[i + 1, j + 1].Value != null)
                            {
                                if (ws.Cells[i + 1, j + 1].Value.Equals("HP") || ws.Cells[i + 1, j + 1].Value.Equals("HFP"))
                                {
                                    if (ws.Cells[i + 1, j + 1].Value.Equals("HP"))
                                    {
                                        indiceHP = j;
                                    }
                                    if (ws.Cells[i + 1, j + 1].Value.Equals("HFP"))
                                    {
                                        indiceHFP = j;
                                    }
                                    if (indiceHP > 0 && indiceHFP > 0)
                                    {
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    if (i >= (ConstantesRechazoCarga.filaInicioDatos - 1))
                    {
                        var demandaUsuario = new RcaDemandaUsuarioDTO();
                        demandaUsuario.Osinergcodi = ws.Cells[i + 1, 3].Value != null ? ws.Cells[i + 1, 3].Value.ToString() : string.Empty;
                        demandaUsuario.Rcdeulfecmaxdem = fechaArchivo;
                        var cont = 1;
                        for (int j = 16; j < 112; j++)
                        {
                            valor = 0;
                            string valorArchivo = (ws.Cells[i + 1, j + 1].Value != null) ? ws.Cells[i + 1, j + 1].Value.ToString() : "0";
                            decimal.TryParse(valorArchivo, out valor);
                            valor = Math.Round(valor, 2);
                            demandaUsuario.GetType().GetProperty("RCDEULH" + cont.ToString()).SetValue(demandaUsuario, valor);

                            if (indiceHP == j)
                            {
                                demandaUsuario.Rcdeuldemandahp = valor;
                            }

                            if (indiceHFP == j)
                            {
                                demandaUsuario.Rcdeuldemandahfp = valor;
                            }

                            cont++;
                        }

                        listaDemandaUsuario.Add(demandaUsuario);
                    }

                }
                respuesta = new Respuesta { Exito = true };
                return listaDemandaUsuario;
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

        private static string TipoEsquema(string valor)
        {
            var resp = string.Empty;

            valor = string.IsNullOrEmpty(valor) ? "" : valor.Trim().ToUpper();

            switch (valor)
            {
                case "PRIMER ESQUEMA": resp = "1"; break;
                case "SEGUNDO ESQUEMA": resp = "2"; break;
            }

            return resp;
        }

        private static string FormatoHora(string valor)
        {
            var resp = string.Empty;

            resp = valor.Length != 5 ? "0" + valor : valor;

            

            return resp;
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