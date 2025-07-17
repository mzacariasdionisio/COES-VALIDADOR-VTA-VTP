using COES.Dominio.DTO.Sic;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.CalculoResarcimiento.Helper
{
    public class LecturaExcel
    {
        public static List<RePtoentregaPeriodoDTO> ObtenerPuntoEntrega(string filename)
        {
            List<RePtoentregaPeriodoDTO> entitys = new List<RePtoentregaPeriodoDTO>();
            FileInfo fileInfo = new FileInfo(filename);
            int cantidad = 2000;

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                for (int i = 1; i <= cantidad; i++)
                {
                    if (ws.Cells[i, 1].Value != null)
                    {
                        string ptoEntrega = ws.Cells[i, 1].Value.ToString();
                        entitys.Add(new RePtoentregaPeriodoDTO { Repentnombre = ptoEntrega });
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return entitys;
        }

        public static List<RePtoentregaSuministradorDTO> ObtenerSuministradoresPorPuntoEntrega(string filename, out List<string> validaciones)
        {
            List<string> errores = new List<string>();
            List<RePtoentregaSuministradorDTO> entitys = new List<RePtoentregaSuministradorDTO>();
            FileInfo fileInfo = new FileInfo(filename);
            int cantidad = 2000;

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets["PtoEntregaSuministrador"];

                if (ws != null)
                {
                    if (ws.Cells[1, 1].Value == null || ws.Cells[1, 2].Value == null)
                    {
                        errores.Add("El formato del archivo de carga ha sido modificado. Por favor revise.");
                    }
                    else
                    {
                        if (ws.Cells[1, 1].Value.ToString() != "Punto de Entrega" || ws.Cells[1, 2].Value.ToString() != "Sumistrador")
                        {
                            errores.Add("El formato del archivo de carga ha sido modificado. Por favor revise.");
                        }
                        else
                        {
                            for (int i = 2; i <= cantidad; i++)
                            {
                                if (ws.Cells[i, 1].Value != null && ws.Cells[i, 2].Value != null)
                                {
                                    string ptoEntrega = ws.Cells[i, 1].Value.ToString();
                                    string suministrador = ws.Cells[i, 2].Value.ToString();
                                    entitys.Add(new RePtoentregaSuministradorDTO { Repentnombre = ptoEntrega, Emprnomb = suministrador });
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    errores.Add("No se encuentra la hoja PtoEntregaSuministrador, por favor revise.");
                }
            }
            validaciones = errores;

            return entitys;
        }

        public static List<ReEventoPeriodoDTO> ObtenerEventos(string filename)
        {
            List<ReEventoPeriodoDTO> entitys = new List<ReEventoPeriodoDTO>();
            FileInfo fileInfo = new FileInfo(filename);
            int cantidad = 2000;

            using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                for (int i = 2; i <= cantidad; i++)
                {
                    if (ws.Cells[i, 1].Value != null)
                    {
                        string evento = (ws.Cells[i, 1].Value != null) ? ws.Cells[i, 1].Value.ToString() : string.Empty;
                        string fecha = (ws.Cells[i, 2].Value != null) ? ws.Cells[i, 2].Value.ToString() : string.Empty;
                        string empresa1 = (ws.Cells[i, 3].Value != null) ? ws.Cells[i, 3].Value.ToString() : string.Empty;
                        string porcentaje1 = (ws.Cells[i, 4].Value != null) ? ws.Cells[i, 4].Value.ToString() : string.Empty;
                        string empresa2 = (ws.Cells[i, 5].Value != null) ? ws.Cells[i, 5].Value.ToString() : string.Empty;
                        string porcentaje2 = (ws.Cells[i, 6].Value != null) ? ws.Cells[i, 6].Value.ToString() : string.Empty;
                        string empresa3 = (ws.Cells[i, 7].Value != null) ? ws.Cells[i, 7].Value.ToString() : string.Empty;
                        string porcentaje3 = (ws.Cells[i, 8].Value != null) ? ws.Cells[i, 8].Value.ToString() : string.Empty;
                        string empresa4 = (ws.Cells[i, 9].Value != null) ? ws.Cells[i, 9].Value.ToString() : string.Empty;
                        string porcentaje4 = (ws.Cells[i, 10].Value != null) ? ws.Cells[i, 10].Value.ToString() : string.Empty;
                        string empresa5 = (ws.Cells[i, 11].Value != null) ? ws.Cells[i, 11].Value.ToString() : string.Empty;
                        string porcentaje5 = (ws.Cells[i, 12].Value != null) ? ws.Cells[i, 12].Value.ToString() : string.Empty;
                        string comentario = (ws.Cells[i, 13].Value != null) ? ws.Cells[i, 13].Value.ToString() : string.Empty;

                        ReEventoPeriodoDTO entity = new ReEventoPeriodoDTO();
                        entity.Reevedescripcion = evento;
                        entity.FechaEvento = fecha;
                        entity.Responsablenomb1 = empresa1;
                        entity.Porcentaje1 = porcentaje1;
                        entity.Responsablenomb2 = empresa2;
                        entity.Porcentaje2 = porcentaje2;
                        entity.Responsablenomb3 = empresa3;
                        entity.Porcentaje3 = porcentaje3;
                        entity.Responsablenomb4 = empresa4;
                        entity.Porcentaje4 = porcentaje4;
                        entity.Responsablenomb5 = empresa5;
                        entity.Porcentaje5 = porcentaje5;
                        entity.Reevecomentario = comentario;

                        entitys.Add(entity);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return entitys;
        }
    }
}