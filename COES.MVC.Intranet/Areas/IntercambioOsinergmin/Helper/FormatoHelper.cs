using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Globalization;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Importacion;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Helper
{
    public class FormatoHelper
    {

        public static List<IioSicliOsigFacturaDTO> LeerExcelInformacionBase(string rutaCompletaArchivo, string periodo, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            var listaArchivoLectura = new List<IioSicliOsigFacturaDTO>();
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;
                //string[][] matriz = new string[filas][];

                
                var fechaArchivo = DateTime.Now;              
                

                if (ws.Cells[2, 1].Value == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene periodo." };
                    return listaArchivoLectura;
                }
                else
                {
                    var periodoArchivo = ws.Cells[2, 1].Value.ToString();
                    if (!periodoArchivo.Equals(periodo))
                    {
                        respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado no tiene periodo igual al seleccionado." };
                        return listaArchivoLectura;
                    }
                }                

                //Lectura de Archivo
                for (int i = 1; i < filas; i++)
                {
                    try
                    {
                        var sicliOsigFactura = new IioSicliOsigFacturaDTO();

                        var validaRegistro = true;

                        if (ws.Cells[i + 1, 1].Value == null || ws.Cells[i + 1, 2].Value == null
                            || ws.Cells[i + 1, 4].Value == null || ws.Cells[i + 1, 5].Value == null)
                        {
                            validaRegistro = false;
                        }

                        if (validaRegistro)
                        {
                            sicliOsigFactura.Clofacanhiomes = ws.Cells[i + 1, 1].Value.ToString().Trim();
                            sicliOsigFactura.Clofaccodempresa = ws.Cells[i + 1, 2].Value.ToString().Trim();
                            sicliOsigFactura.Clofacnomempresa = ws.Cells[i + 1, 3].Value.ToString().Trim();
                            sicliOsigFactura.Clofacruc = ws.Cells[i + 1, 4].Value.ToString().Trim();
                            sicliOsigFactura.Clofaccodcliente = ws.Cells[i + 1, 5].Value.ToString().Trim();
                            sicliOsigFactura.Clofacnomcliente = ws.Cells[i + 1, 6].Value.ToString().Trim();
                            sicliOsigFactura.Clofaccodbarrasumin = ws.Cells[i + 1, 7].Value.ToString().Trim();
                            sicliOsigFactura.Clofacnombarrasumin = ws.Cells[i + 1, 8].Value.ToString().Trim();
                            sicliOsigFactura.Clofactensionentrega = Convert.ToDecimal(ws.Cells[i + 1, 9].Value.ToString());

                            sicliOsigFactura.Clofaccodbrg = ws.Cells[i + 1, 10].Value.ToString().Trim();
                            sicliOsigFactura.Clofacnombrg = ws.Cells[i + 1, 11].Value.ToString().Trim();

                            sicliOsigFactura.Clofactensionbrg = Convert.ToDecimal(ws.Cells[i + 1, 12].Value.ToString());
                            sicliOsigFactura.Clofacphpbe = Convert.ToDecimal(ws.Cells[i + 1, 13].Text);
                            sicliOsigFactura.Clofacpfpbe = Convert.ToDecimal(ws.Cells[i + 1, 14].Text);
                            sicliOsigFactura.Clofacehpbe = Convert.ToDecimal(ws.Cells[i + 1, 15].Text);
                            sicliOsigFactura.Clofacefpbe = Convert.ToDecimal(ws.Cells[i + 1, 16].Text);
                            //sicliOsigFactura.Clofactensionentrega = Convert.ToDecimal(ws.Cells[i + 1, 2].Value.ToString());

                            listaArchivoLectura.Add(sicliOsigFactura);
                        }

                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException(ex.Message);
                    }                                                          
                }

                respuesta = new Respuesta { Exito = true };
                return listaArchivoLectura;
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
}