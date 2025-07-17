using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.Data;

namespace WSIC2010.Util
{
    public static class UtilsExcel
    {
        public static byte[] CrearExcel2003(DataTable adt_table, string ps_titulo)
        {
            using (MemoryStream m = new MemoryStream())
            {

                HSSFWorkbook Workbook = new HSSFWorkbook();
                DocumentSummaryInformation DocInfo;
                SummaryInformation SummaryInfo;
                Sheet DataSheet;
                Row CurrRow;
                Cell CurrCell;
                CellStyle HeaderStyle;
                NPOI.SS.UserModel.Font HeaderFont;
                int CellIndex;
                DocInfo = PropertySetFactory.CreateDocumentSummaryInformation();
                DocInfo.Company = "COES-SINAC";
                Workbook.DocumentSummaryInformation = DocInfo;

                SummaryInfo = PropertySetFactory.CreateSummaryInformation();
                SummaryInfo.Subject = "Registro de Integrantes - COES SINAC";
                Workbook.SummaryInformation = SummaryInfo;

                DataSheet = Workbook.CreateSheet("Sheet1");
                DataSheet.DisplayGridlines = true;

                int li_titulo = 0;

                li_titulo++;
                CurrRow = DataSheet.CreateRow(li_titulo);
                CellIndex = 0;

                // Create header row for spreadsheet.
                for (int i = 0; i <= adt_table.Columns.Count - 1; i++)
                {
                    if (!String.IsNullOrEmpty(adt_table.Columns[i].ColumnName))
                    {
                        CurrCell = CurrRow.CreateCell(CellIndex);
                        CurrCell.SetCellValue(adt_table.Columns[i].ColumnName);
                    }
                    else
                    {
                        CurrCell = CurrRow.CreateCell(CellIndex);
                        CurrCell.SetCellValue("Columna " + i);
                    }

                    CellIndex++;
                }

                /*Set style cell (rows)*/
                CellStyle blackBorder = Workbook.CreateCellStyle();
                blackBorder.BorderBottom = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                blackBorder.BorderLeft = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                blackBorder.BorderRight = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                blackBorder.BorderTop = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                blackBorder.BottomBorderColor = HSSFColor.WHITE.index;
                blackBorder.LeftBorderColor = HSSFColor.BLACK.index;
                blackBorder.RightBorderColor = HSSFColor.BLACK.index;
                blackBorder.TopBorderColor = HSSFColor.WHITE.index;

                /*Estilo para la primera fila*/
                CellStyle styleInicio = Workbook.CreateCellStyle();
                styleInicio.BorderLeft = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleInicio.BorderRight = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleInicio.BorderTop = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleInicio.LeftBorderColor = HSSFColor.BLACK.index;
                styleInicio.RightBorderColor = HSSFColor.BLACK.index;
                styleInicio.TopBorderColor = HSSFColor.BLACK.index;

                /*Estilo para la ultima fila*/
                CellStyle styleFinal = Workbook.CreateCellStyle();
                styleFinal.BorderLeft = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleFinal.BorderRight = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleFinal.BorderBottom = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                styleFinal.LeftBorderColor = HSSFColor.BLACK.index;
                styleFinal.RightBorderColor = HSSFColor.BLACK.index;
                styleFinal.BottomBorderColor = HSSFColor.BLACK.index;

                DateTime ldt_fecha = new DateTime(2000, 1, 1);
                int li_valor = 0;
                double ld_valor = 0;
                // Iterate through data rows and add to spreadsheet.
                for (int i = li_titulo; i <= adt_table.Rows.Count; i++)
                {
                    CellIndex = 0;
                    CurrRow = DataSheet.CreateRow(i + 1);

                    // Iterate through data columns and add to spreadsheet.
                    for (int j = 0; j <= adt_table.Columns.Count - 1; j++)
                    {
                        if (DateTime.TryParse(adt_table.Rows[i - 1][j].ToString(), out ldt_fecha) && adt_table.Columns[j].DataType == typeof(System.DateTime))
                        {
                            CurrCell = CurrRow.CreateCell(CellIndex);
                            CellStyle cellStyle = Workbook.CreateCellStyle();
                            CurrCell.SetCellValue(ldt_fecha.ToString("dd/MM/yyyy HH:mm:ss"));
                            CellIndex += 1;
                        }
                        else if (double.TryParse(adt_table.Rows[i - 1][j].ToString(), out ld_valor) && Utils.IsNumeric(adt_table.Columns[j]))
                        {
                            CurrCell = CurrRow.CreateCell(CellIndex);
                            CurrCell.SetCellValue(ld_valor);
                            CellIndex += 1;
                        }
                        else if (Int32.TryParse(adt_table.Rows[i - 1][j].ToString(), out li_valor) && Utils.IsNumeric(adt_table.Columns[j]))
                        {
                            CurrCell = CurrRow.CreateCell(CellIndex);
                            CurrCell.SetCellValue(li_valor);
                            CellIndex += 1;
                        }
                        else
                        {
                            CurrCell = CurrRow.CreateCell(CellIndex);
                            CurrCell.SetCellValue(adt_table.Rows[i - 1][j].ToString().TrimEnd());
                            CellIndex += 1;
                        }

                        if (i == li_titulo)
                        {
                            CurrCell.CellStyle = styleInicio;
                        }
                        else if (i == adt_table.Rows.Count)
                        {
                            CurrCell.CellStyle = styleFinal;
                        }
                        else
                        {
                            CurrCell.CellStyle = blackBorder;
                        }



                    }
                }

                // Final formatting tasks: stretch column widths and format header.
                for (int i = 0; i <= DataSheet.GetRow(li_titulo).LastCellNum - 1; i++)
                {
                    DataSheet.AutoSizeColumn(i);
                }

                HeaderFont = Workbook.CreateFont();
                HeaderFont.Color = HSSFColor.WHITE.index;//BLACK.index;
                HeaderFont.Boldweight = (short)FontBoldWeight.BOLD;

                HeaderStyle = Workbook.CreateCellStyle();
                HeaderStyle.SetFont(HeaderFont);
                HeaderStyle.FillForegroundColor = HSSFColor.LIGHT_BLUE.index;//GREY_25_PERCENT.index;
                HeaderStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
                HeaderStyle.BorderLeft = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                HeaderStyle.BorderRight = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                HeaderStyle.BorderTop = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                HeaderStyle.LeftBorderColor = HSSFColor.BLACK.index;
                HeaderStyle.RightBorderColor = HSSFColor.BLACK.index;
                HeaderStyle.TopBorderColor = HSSFColor.BLACK.index;

                CurrRow = DataSheet.GetRow(li_titulo);

                for (int j = 0; j <= CurrRow.LastCellNum - 1; j++)
                {
                    CurrRow.GetCell(j).CellStyle = HeaderStyle;
                }

                /*set title*/
                CurrRow = DataSheet.CreateRow(0);
                CurrRow.HeightInPoints = 20;

                CurrCell = CurrRow.CreateCell(0);
                //set the title of the sheet
                CurrCell.SetCellValue(ps_titulo.ToUpper());

                CellStyle style = Workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.CENTER;
                style.BorderLeft = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                style.BorderRight = (CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                style.LeftBorderColor = HSSFColor.BLACK.index;
                style.RightBorderColor = HSSFColor.BLACK.index;
                //create a font style
                NPOI.SS.UserModel.Font font = Workbook.CreateFont();
                font.FontHeight = 15 * 15;
                style.SetFont(font);
                CurrCell.CellStyle = style;

                //merged cells on single row
                DataSheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, adt_table.Columns.Count - 1));

                Workbook.Write(m);

                return m.ToArray();

            }
        }
    }
}