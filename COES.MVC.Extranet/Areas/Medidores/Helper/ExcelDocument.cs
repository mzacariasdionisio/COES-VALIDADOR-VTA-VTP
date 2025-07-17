using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Helper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace COES.MVC.Extranet.Areas.Medidores.Helper
{
    public class ExcelDocument
    {
        public static void GenerarArchivoEnvio(List<MeEnvioDTO> lista, DateTime fini, DateTime ffin,string ruta)
        { 
            
            FileInfo template = new FileInfo(ruta + NombreArchivoInterconexiones.PlantillaReporteEnvio);
            FileInfo newFile = new FileInfo(ruta + NombreArchivoInterconexiones.NombreReporteEnvio);

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(ruta + NombreArchivoInterconexiones.NombreReporteEnvio);
            }
            int row = 5;
            int column = 2;
            using (ExcelPackage xlPackage = new ExcelPackage(newFile, template))
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets[Constantes.HojaReporteExcel];
                ws.View.FreezePanes(row, 1);
                ws.Cells[2, 3].Value = fini.ToString("dd/MM/yyyy");
                ws.Cells[2, 5].Value = ffin.ToString("dd/MM/yyyy");
                foreach (var reg in lista)
                {
                    string cumplimiento = string.Empty;
                    if (reg.Estenvcodi == ConstantesMedidores.EnPlazo)                    
                            cumplimiento = ConstantesMedidores.CumplimientoEnPlazo;
                    if (reg.Estenvcodi == ConstantesMedidores.FueraPlazo)
                        cumplimiento = ConstantesMedidores.CumplimientoFueraDePlazo;
                    ws.Cells[row, column].Value = reg.Emprnomb;
                    ws.Cells[row, column].StyleID = ws.Cells[5, column].StyleID;
                    ws.Cells[row, column + 1].Value = reg.Estenvnombre;
                    ws.Cells[row, column + 1].StyleID = ws.Cells[5, column + 1].StyleID;
                    ws.Cells[row, column + 2].Value = cumplimiento;
                    ws.Cells[row, column + 2].StyleID = ws.Cells[5, column + 2].StyleID;
                    ws.Cells[row, column + 3].Value = reg.Enviofechaperiodo.Value.ToString("dd/MM/yyyy");
                    ws.Cells[row, column + 3].StyleID = ws.Cells[5, column + 3].StyleID;
                    DateTime fechaenvio = (DateTime)reg.Enviofecha;
                    ws.Cells[row, column + 4].Value = fechaenvio.ToString("dd/MM/yyyy HH:mm:ss");
                    ws.Cells[row, column + 4].StyleID = ws.Cells[5, column + 4].StyleID;
                    ws.Cells[row, column + 5].Value = reg.Enviocodi;
                    ws.Cells[row, column + 5].StyleID = ws.Cells[5, column + 5].StyleID;

                    ws.Cells[row, column + 6].Value = reg.Username;
                    ws.Cells[row, column + 6].StyleID = ws.Cells[5, column + 6].StyleID;

                    ws.Cells[row, column + 7].Value = reg.Lastuser;
                    ws.Cells[row, column + 7].StyleID = ws.Cells[5, column + 7].StyleID;
                    
                    ws.Cells[row, column + 8].Value = reg.Usertlf;
                    ws.Cells[row, column + 8].StyleID = ws.Cells[5, column + 8].StyleID;
                    row++;
                }
                xlPackage.Save();
            }
        }
    }
}