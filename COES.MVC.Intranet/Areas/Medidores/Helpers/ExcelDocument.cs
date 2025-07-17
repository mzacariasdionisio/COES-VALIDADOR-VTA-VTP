using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Medidores.Helper
{
    public class ExcelDocument
    {
        /// <summary>
        /// Permite exportar los perfiles almacenados en base de datos
        /// </summary>
        /// <param name="list"></param>
        public static void GenerarArchivoReporte(List<MeMedicion96DTO> lista,DateTime fini,DateTime ffin)
        {
            string ruta = ConfigurationManager.AppSettings[RutaDirectorio.ReporteInterconexion].ToString();
            FileInfo template = new FileInfo(ruta + NombreArchivo.PlantillaReporteHistorico);
            FileInfo newFile = new FileInfo(ruta + NombreArchivo.NombreReporteHistorico);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivo.NombreReporteHistorico);
            }
            int row = 8;
            int column = 2;
            int minuto = 0;

            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];
                ws.View.FreezePanes(row, 1);
                ws.Cells[2, 3].Value = fini.ToString("dd/MM/yyyy");
                ws.Cells[2, 5].Value = ffin.ToString("dd/MM/yyyy");
                if (lista.Count > 0)
                {
                    for (DateTime f = fini; f <= ffin; f = f.AddDays(1))
                    {
                        var list = lista.Where(x => x.Medifecha == f);
                        if (list.Count() != 0)
                            for (int k = 1; k <= 96; k++)
                            {
                                minuto = minuto + 15;
                                //Mw Interconexion
                                MeMedicion96DTO entity = list.Where(x => x.Tipoinfocodi == 3).ToList().FirstOrDefault();
                                
                                ws.Cells[row, column].Value = entity.Medifecha.Value.AddMinutes(minuto).ToString("dd/MM/yyyy HH:mm");
                                ws.Cells[row, column].StyleID = ws.Cells[8, column].StyleID;
                                decimal valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                decimal mwExport = 0;
                                decimal mwImport = 0;
                                if (valor > 0)
                                {
                                    mwExport = valor;
                                    mwImport = 0;
                                }
                                else
                                {
                                    valor = valor * (-1);
                                    mwImport = valor;
                                    mwExport = 0;
                                }
                                ws.Cells[row, column + 1].Value = mwExport;
                                ws.Cells[row, column + 2].Value = mwImport;
                                ws.Cells[row, column + 1].StyleID = ws.Cells[8, column + 1].StyleID;
                                ws.Cells[row, column + 2].StyleID = ws.Cells[8, column + 2].StyleID;
                                // Mvar Interconexion

                                decimal mvarExport = 0;
                                decimal mvarImport = 0;
                                entity = list.Where(x => x.Tipoinfocodi == 4).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                if (valor > 0)
                                {
                                    mvarExport = valor;
                                    mvarImport = 0;
                                }
                                else
                                {
                                    valor = valor * (-1);
                                    mvarImport = valor;
                                    mvarExport = 0;
                                }
                                ws.Cells[row, column + 3].Value = mvarExport;
                                ws.Cells[row, column + 4].Value = mvarImport;
                                ws.Cells[row, column + 3].StyleID = ws.Cells[8, column + 3].StyleID;
                                ws.Cells[row, column + 4].StyleID = ws.Cells[8, column + 4].StyleID;
                                // kV

                                entity = list.Where(x => x.Tipoinfocodi == 5).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                ws.Cells[row, column + 5].Value = valor;
                                ws.Cells[row, column + 5].StyleID = ws.Cells[8, column + 5].StyleID;
                                // Amp
                                entity = list.Where(x => x.Tipoinfocodi == 9).ToList().FirstOrDefault();
                                valor = (decimal)entity.GetType().GetProperty("H" + k).GetValue(entity, null);
                                ws.Cells[row, column + 6].Value = valor;
                                ws.Cells[row, column + 6].StyleID = ws.Cells[8, column + 6].StyleID;
 
                                row++;
                            }
                    }
                }
                xlPackage.Save();
            }
            
        }
    }
}