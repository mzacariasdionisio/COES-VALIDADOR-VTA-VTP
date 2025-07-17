using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using COES.Dominio.DTO.Sic;

namespace COES.MVC.Intranet.Areas.Formulas.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarExcelPerfilScada(List<PerfilScadaDTO> list)
        {

            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReportePerfiles].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.PlantillaReportePerfilScadaExcel);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreReportePerfilScadaExcel);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreReportePerfilScadaExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                object valor = null;
                object tunnig = null;
                decimal suma = 0;

                int row = 7;
                int column = 1;
                int group = 0;

                if (ws != null)
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        for (int k = 1; k <= 48; k++)
                        {
                            ws.Cells[row, column].Value = Tools.ObtenerNombreDia(i);
                            ws.Cells[row, column + 1].Value = Tools.ObtenerHoraMedicion(k);
                            row = row + 1;
                        }
                    }

                    column = column + 2;

                    foreach (PerfilScadaDTO item in list)
                    {
                        ws.Cells[3, column].Value = item.EJRUCODI;
                        ws.Cells[3, column].StyleID = ws.Cells[3, 1].StyleID;
                        ws.Cells[4, column].Value = (item.LASTUSER != null) ? item.LASTUSER.ToUpper() : string.Empty;
                        ws.Cells[4, column].StyleID = ws.Cells[3, 1].StyleID;
                        ws.Cells[5, column].Value = item.PRRUABREV;
                        ws.Cells[5, column].StyleID = ws.Cells[3, 1].StyleID;
                        ws.Cells[6, column].Value = item.PRRUNOMB;
                        ws.Cells[6, column].StyleID = ws.Cells[3, 1].StyleID;
                        ws.Column(column).Width = 18;

                        row = 7;

                        for (int i = 1; i <= 7; i++)
                        {
                            if (i == 4 || i == 5 || i == 7 || i == 8) group = 1;
                            if (i == 1) group = 2;
                            if (i == 2) group = 3;
                            if (i == 3) group = 4;

                            PerfilScadaDetDTO entity = item.LISTAITEMS.Where(x => x.PERFCLASI == group).FirstOrDefault();

                            if (entity != null)
                            {
                                for (int k = 1; k <= 48; k++)
                                {
                                    valor = entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                    tunnig = entity.GetType().GetProperty("TH" + k).GetValue(entity, null);

                                    if (valor != null)
                                    {
                                        suma = Convert.ToDecimal(valor);
                                        if (tunnig != null)
                                            suma = suma + Convert.ToDecimal(tunnig);
                                    }
                                    else
                                    {
                                        if (tunnig != null)
                                            suma = Convert.ToDecimal(tunnig);
                                    }

                                    ws.Cells[row, column].Value = suma.ToString();
                                    row = row + 1;
                                }
                            }
                            else
                            {
                                row = row + 48;
                            }
                        }
                        column = column + 1;

                    }

                }
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Permite generar el formato de importación
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        public static void GenerarPlantillaImportacion(string subEstacion, string area, string agrupacion, int formula)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReportePerfiles].ToString();
            FileInfo template = new FileInfo(ruta + Constantes.NombrePlantillaImportacionPerfilExcel);
            FileInfo newFile = new FileInfo(ruta + Constantes.NombreFormatoImportacionPerfilExcel);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + Constantes.NombreFormatoImportacionPerfilExcel);
            }

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];

                ws.Cells[1, 2].Value = formula.ToString();                
                ws.Cells[2, 2].Value = subEstacion;
                ws.Cells[3, 2].Value = area;
                ws.Cells[4, 2].Value = agrupacion;
                
                xlPackage.Save();
            }
        }

        /// <summary>
        /// Lee los artículos desde el formato excel cargado
        /// </summary>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        public static ScadaDTO ImportarDatos(string file, out int codigo)
        {
            try
            {
                ScadaDTO entity = new ScadaDTO();                
                FileInfo fileInfo = new FileInfo(file);

                using (ExcelPackage xlPackage = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = xlPackage.Workbook.Worksheets[1];

                    decimal valor = 0;
                    decimal tunnig = 0;

                    codigo = 0;
                    
                    if (ws.Cells[1, 2] != null)
                    {
                        if (int.TryParse(ws.Cells[1, 2].Value.ToString(), out codigo)) { }
                    }

                    for (int i = 8; i <= 55; i++)
                    {
                        if (ws.Cells[i, 2].Value != null)
                        {
                            if (decimal.TryParse(ws.Cells[i, 2].Value.ToString(), out valor))
                            {
                                entity.GetType().GetProperty("H" + ((i - 7) * 2)).SetValue(entity, valor);
                            }
                        }

                        if (ws.Cells[i, 3].Value != null)
                        {
                            if (decimal.TryParse(ws.Cells[i, 3].Value.ToString(), out tunnig))
                            {
                                entity.GetType().GetProperty("TH" + ((i - 7) * 2)).SetValue(entity, tunnig);
                            }
                        }
                    }
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}