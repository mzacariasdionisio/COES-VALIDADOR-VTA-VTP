using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using COES.MVC.Intranet.Areas.Evaluacion.Models;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Evaluacion.Helper
{
    public class FormatoHelper
    {
        public static List<EprCargaMasivaLineaDTO> LeerExcelCargado(string rutaCompletaArchivo, int titulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                List<EprCargaMasivaLineaDTO> matriz = new List<EprCargaMasivaLineaDTO>();

                if (xlPackage.Workbook.Worksheets["PLANTILLA"] == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;

                if (filas < 10)
                {
                    respuesta = new Respuesta { Exito = true };
                    return matriz;
                }

                for (int i = 10; i < filas; i++)
                {
                    var linea = new EprCargaMasivaLineaDTO();

                    linea.Item = (ws.Cells[i, 2].Value != null) ? (string.IsNullOrEmpty(ws.Cells[i, 2].Value.ToString()) ? "" : ws.Cells[i, 2].Value.ToString()): "";

                    if (string.IsNullOrEmpty(linea.Item)) break;

                    linea.Codigo = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                    linea.ComentarioCodigo = (ws.Cells[i, 3].Comment != null) ? ws.Cells[i, 3].Comment.Text : string.Empty;                    

                    linea.Ubicacion = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                    linea.ComentarioUbicacion = (ws.Cells[i, 4].Comment != null) ? ws.Cells[i, 4].Comment.Text : string.Empty;
                    linea.CapacidadA = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;
                    linea.ComentarioCapacidadA = (ws.Cells[i, 5].Comment != null) ? ws.Cells[i, 5].Comment.Text : string.Empty;
                    linea.Celda = (ws.Cells[i, 6].Value != null) ? ws.Cells[i, 6].Value.ToString() : string.Empty;
                    linea.ComentarioCelda = (ws.Cells[i, 6].Comment != null) ? ws.Cells[i, 6].Comment.Text : string.Empty;
                    linea.Celda2 = (ws.Cells[i, 7].Value != null) ? ws.Cells[i, 7].Value.ToString() : string.Empty;
                    linea.ComentarioCelda2 = (ws.Cells[i, 7].Comment != null) ? ws.Cells[i, 7].Comment.Text : string.Empty;
                    linea.BancoCondensador = (ws.Cells[i, 8].Value != null) ? ws.Cells[i, 8].Value.ToString() : string.Empty;
                    linea.ComentarioBancoCondensador = (ws.Cells[i, 8].Comment != null) ? ws.Cells[i, 8].Comment.Text : string.Empty;
                    linea.BancoCapacidadA = (ws.Cells[i, 9].Value != null) ? ws.Cells[i, 9].Value.ToString() : string.Empty;
                    linea.ComentarioBancoCapacidadA = (ws.Cells[i, 9].Comment != null) ? ws.Cells[i, 9].Comment.Text : string.Empty;
                    linea.BancoCapacidadMVAr = (ws.Cells[i, 10].Value != null) ? ws.Cells[i, 10].Value.ToString() : string.Empty;
                    linea.ComentarioBancoCapacidadMVAr = (ws.Cells[i, 10].Comment != null) ? ws.Cells[i, 10].Comment.Text : string.Empty;
                    linea.CapacTransCond1Porcen = (ws.Cells[i, 11].Value != null) ? ws.Cells[i, 11].Value.ToString() : string.Empty;
                    linea.ComentarioCapacTransCond1Porcen = (ws.Cells[i, 11].Comment != null) ? ws.Cells[i, 11].Comment.Text : string.Empty;
                    linea.CapacTransCond1Min = (ws.Cells[i, 12].Value != null) ? ws.Cells[i, 12].Value.ToString() : string.Empty;
                    linea.ComentarioCapacTransCond1Min = (ws.Cells[i, 12].Comment != null) ? ws.Cells[i, 12].Comment.Text : string.Empty;
                    linea.CapacTransCond2Porcen = (ws.Cells[i, 13].Value != null) ? ws.Cells[i, 13].Value.ToString() : string.Empty;
                    linea.ComentarioCapacTransCond2Porcen = (ws.Cells[i, 13].Comment != null) ? ws.Cells[i, 13].Comment.Text : string.Empty;
                    linea.CapacTransCond2Min = (ws.Cells[i, 14].Value != null) ? ws.Cells[i, 14].Value.ToString() : string.Empty;
                    linea.ComentarioCapacTransCond2Min = (ws.Cells[i, 14].Comment != null) ? ws.Cells[i, 14].Comment.Text : string.Empty;
                    linea.LimiteSegCoes = (ws.Cells[i, 15].Value != null) ? ws.Cells[i, 15].Value.ToString() : string.Empty;
                    linea.ComentarioLimiteSegCoes = (ws.Cells[i, 15].Comment != null) ? ws.Cells[i, 15].Comment.Text : string.Empty;
                    linea.Observaciones = (ws.Cells[i, 16].Value != null) ? ws.Cells[i, 16].Value.ToString() : string.Empty;
                    linea.ComentarioObservaciones = (ws.Cells[i, 16].Comment != null) ? ws.Cells[i, 16].Comment.Text : string.Empty;
                    linea.Motivo = (ws.Cells[i, 17].Value != null) ? ws.Cells[i, 17].Value.ToString() : string.Empty;
                    linea.ComentarioMotivo = (ws.Cells[i, 17].Comment != null) ? ws.Cells[i, 17].Comment.Text : string.Empty;
                   

                    matriz.Add(linea);
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static List<EprCargaMasivaLineaDTO> LeerExcelCargadoReactor(string rutaCompletaArchivo, int titulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                List<EprCargaMasivaLineaDTO> matriz = new List<EprCargaMasivaLineaDTO>();

                if (xlPackage.Workbook.Worksheets["PLANTILLA"] == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;


                if (filas < 10)
                {
                    respuesta = new Respuesta { Exito = true };
                    return matriz;
                }

                for (int i = 10; i < filas; i++)
                {
                    var linea = new EprCargaMasivaLineaDTO();


                    linea.Item = (ws.Cells[i, 2].Value != null) ? (string.IsNullOrEmpty(ws.Cells[i, 2].Value.ToString()) ? "" : ws.Cells[i, 2].Value.ToString()) : "";

                    if (string.IsNullOrEmpty(linea.Item)) break;

                    linea.Codigo = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                    linea.ComentarioCodigo = (ws.Cells[i, 3].Comment != null) ? ws.Cells[i, 3].Comment.Text : string.Empty;                  
                  
                    linea.Celda = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                    linea.ComentarioCelda = (ws.Cells[i, 4].Comment != null) ? ws.Cells[i, 4].Comment.Text : string.Empty;
                    linea.Celda2 = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;
                    linea.ComentarioCelda2 = (ws.Cells[i, 5].Comment != null) ? ws.Cells[i, 5].Comment.Text : string.Empty;
                    linea.CapacidadMvar = (ws.Cells[i, 6].Value != null) ? ws.Cells[i, 6].Value.ToString() : string.Empty;
                    linea.ComentarioCapacidadMvar = (ws.Cells[i, 6].Comment != null) ? ws.Cells[i, 6].Comment.Text : string.Empty;
                    linea.CapacidadA = (ws.Cells[i, 7].Value != null) ? ws.Cells[i, 7].Value.ToString() : string.Empty;
                    linea.ComentarioCapacidadA = (ws.Cells[i, 7].Comment != null) ? ws.Cells[i, 7].Comment.Text : string.Empty;
                  
                    linea.Observaciones = (ws.Cells[i, 8].Value != null) ? ws.Cells[i, 8].Value.ToString() : string.Empty;
                    linea.ComentarioObservaciones = (ws.Cells[i, 8].Comment != null) ? ws.Cells[i, 8].Comment.Text : string.Empty;
                    linea.Motivo = (ws.Cells[i, 9].Value != null) ? ws.Cells[i, 9].Value.ToString() : string.Empty;
                    linea.ComentarioMotivo = (ws.Cells[i, 9].Comment != null) ? ws.Cells[i, 9].Comment.Text : string.Empty;


                    matriz.Add(linea);
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static List<EprCargaMasivaCeldaAcoplamientoDTO> LeerExcelCargadoCeldaAcoplamiento(string rutaCompletaArchivo, int titulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                List<EprCargaMasivaCeldaAcoplamientoDTO> matriz = new List<EprCargaMasivaCeldaAcoplamientoDTO>();

                if (xlPackage.Workbook.Worksheets["PLANTILLA"] == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;

                if (filas < 10)
                {
                    respuesta = new Respuesta { Exito = true };
                    return matriz;
                }

                for (int i = 10; i < filas; i++)
                {
                    var linea = new EprCargaMasivaCeldaAcoplamientoDTO();

                    linea.Item = (ws.Cells[i, 2].Value != null) ? (string.IsNullOrEmpty(ws.Cells[i, 2].Value.ToString()) ? "" : ws.Cells[i, 2].Value.ToString()) : "";

                    if (string.IsNullOrEmpty(linea.Item)) break;

                    linea.Codigo = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                    linea.ComentarioCodigo = (ws.Cells[i, 3].Comment != null) ? ws.Cells[i, 3].Comment.Text : string.Empty;

                    linea.CodigoInterruptorAcoplamiento = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                    linea.ComentarioCodigoInterruptorAcoplamiento = (ws.Cells[i, 4].Comment != null) ? ws.Cells[i, 4].Comment.Text : string.Empty;
                    linea.CapacidadInterruptorAcoplamiento = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;
                    linea.ComentarioCapacidadInterruptorAcoplamiento = (ws.Cells[i, 5].Comment != null) ? ws.Cells[i, 5].Comment.Text : string.Empty;                  

                    linea.Observaciones = (ws.Cells[i, 6].Value != null) ? ws.Cells[i, 6].Value.ToString() : string.Empty;
                    linea.ComentarioObservaciones = (ws.Cells[i, 6].Comment != null) ? ws.Cells[i, 6].Comment.Text : string.Empty;
                    linea.Motivo = (ws.Cells[i, 7].Value != null) ? ws.Cells[i, 7].Value.ToString() : string.Empty;
                    linea.ComentarioMotivo = (ws.Cells[i, 7].Comment != null) ? ws.Cells[i, 7].Comment.Text : string.Empty;

                    matriz.Add(linea);
                }
                respuesta = new Respuesta { Exito = true };
                return matriz;
            }
        }

        public static List<EprCargaMasivaTransformadorDTO> LeerExcelCargadoTransformador(string rutaCompletaArchivo, int titulos, out Respuesta respuesta)
        {
            FileInfo fileInfo = new FileInfo(rutaCompletaArchivo);
            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                List<EprCargaMasivaTransformadorDTO> matriz = new List<EprCargaMasivaTransformadorDTO>();

                if (xlPackage.Workbook.Worksheets["PLANTILLA"] == null)
                {
                    respuesta = new Respuesta { Exito = false, Mensaje = "El archivo importado debe tener la hoja PLANTILLA" };
                    return matriz;
                }

                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PLANTILLA"];
                var filas = ws.Dimension.End.Row;
                var columnas = ws.Dimension.End.Column;


                if (filas < 10)
                {
                    respuesta = new Respuesta { Exito = true };
                    return matriz;
                }

                for (int i = 10; i < filas; i++)
                {
                    var linea = new EprCargaMasivaTransformadorDTO();

                    linea.Item = (ws.Cells[i, 2].Value != null) ? (string.IsNullOrEmpty(ws.Cells[i, 2].Value.ToString()) ? "" : ws.Cells[i, 2].Value.ToString()) : "";

                    if (string.IsNullOrEmpty(linea.Item)) break;

                    linea.Codigo = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                    linea.ComentarioCodigo = (ws.Cells[i, 3].Comment != null) ? ws.Cells[i, 3].Comment.Text : string.Empty;

                    linea.DevanadoCodigo = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCodigo = (ws.Cells[i, 4].Comment != null) ? ws.Cells[i, 4].Comment.Text : string.Empty;
                    linea.DevanadoCapacidadONAN = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCapacidadONAN = (ws.Cells[i, 5].Comment != null) ? ws.Cells[i, 5].Comment.Text : string.Empty;
                    linea.DevanadoCapacidadONAF = (ws.Cells[i, 6].Value != null) ? ws.Cells[i, 6].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCapacidadONAF = (ws.Cells[i, 6].Comment != null) ? ws.Cells[i, 6].Comment.Text : string.Empty;
                    linea.DevanadoDosCodigo = (ws.Cells[i, 7].Value != null) ? ws.Cells[i, 7].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoDosCodigo = (ws.Cells[i, 7].Comment != null) ? ws.Cells[i, 7].Comment.Text : string.Empty;
                    linea.DevanadoDosCapacidadONAN = (ws.Cells[i, 8].Value != null) ? ws.Cells[i, 8].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoDosCapacidadONAN = (ws.Cells[i, 8].Comment != null) ? ws.Cells[i, 8].Comment.Text : string.Empty;
                    linea.DevanadoDosCapacidadONAF = (ws.Cells[i, 9].Value != null) ? ws.Cells[i, 9].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoDosCapacidadONAF = (ws.Cells[i, 9].Comment != null) ? ws.Cells[i, 9].Comment.Text : string.Empty;
                    linea.DevanadoTresCodigo = (ws.Cells[i, 10].Value != null) ? ws.Cells[i, 10].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoTresCodigo = (ws.Cells[i, 10].Comment != null) ? ws.Cells[i, 10].Comment.Text : string.Empty;
                    linea.DevanadoTresCapacidadONAN = (ws.Cells[i, 11].Value != null) ? ws.Cells[i, 11].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoTresCapacidadONAN = (ws.Cells[i, 11].Comment != null) ? ws.Cells[i, 11].Comment.Text : string.Empty;
                    linea.DevanadoTresCapacidadONAF = (ws.Cells[i, 12].Value != null) ? ws.Cells[i, 12].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoTresCapacidadONAF = (ws.Cells[i, 12].Comment != null) ? ws.Cells[i, 12].Comment.Text : string.Empty;
                    linea.DevanadoCuatroCodigo = (ws.Cells[i, 13].Value != null) ? ws.Cells[i, 13].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCuatroCodigo = (ws.Cells[i, 13].Comment != null) ? ws.Cells[i, 13].Comment.Text : string.Empty;
                    linea.DevanadoCuatroCapacidadONAN = (ws.Cells[i, 14].Value != null) ? ws.Cells[i, 14].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCuatroCapacidadONAN = (ws.Cells[i, 14].Comment != null) ? ws.Cells[i, 14].Comment.Text : string.Empty;
                    linea.DevanadoCuatroCapacidadONAF = (ws.Cells[i, 15].Value != null) ? ws.Cells[i, 15].Value.ToString() : string.Empty;
                    linea.ComentarioDevanadoCuatroCapacidadONAF = (ws.Cells[i, 15].Comment != null) ? ws.Cells[i, 15].Comment.Text : string.Empty;
                                      
                    linea.Observaciones = (ws.Cells[i, 16].Value != null) ? ws.Cells[i, 16].Value.ToString() : string.Empty;
                    linea.ComentarioObservaciones = (ws.Cells[i, 16].Comment != null) ? ws.Cells[i, 16].Comment.Text : string.Empty;
                    linea.Motivo = (ws.Cells[i, 17].Value != null) ? ws.Cells[i, 17].Value.ToString() : string.Empty;
                    linea.ComentarioMotivo = (ws.Cells[i, 17].Comment != null) ? ws.Cells[i, 17].Comment.Text : string.Empty;


                    matriz.Add(linea);
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
}