//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;
//using Microsoft.Office.Interop.Excel;

//namespace WSIC2010.Util
//{
//    public class EPExcelRange
//    {
//        public int i_FromCellRow;
//        public int i_FromCellCol;
//        public int i_ToCellRow;
//        public int i_ToCellCol;
//        public object oMissing = System.Type.Missing;

//        public Worksheet oSheet = null;
//        public Range XLRange;

//        public EPExcelRange(Worksheet a_sheet, int ai_FromCellRow, int ai_FromCellCol, int ai_ToCellRow, int ai_ToCellCol)
//        {
//            oSheet = a_sheet;
//            i_FromCellRow = ai_FromCellRow;
//            i_FromCellCol = ai_FromCellCol;
//            i_ToCellRow = ai_ToCellRow;
//            i_ToCellCol = ai_ToCellCol;
//            XLRange = oSheet.get_Range(oSheet.Cells[ai_FromCellRow, ai_FromCellCol], oSheet.Cells[ai_ToCellRow, ai_ToCellCol]);
//        }

//        public EPExcelRange(Worksheet a_sheet, int ai_FromCellRow, int ai_FromCellCol)
//        {
//            oSheet = a_sheet;
//            i_FromCellRow = ai_FromCellRow;
//            i_FromCellCol = ai_FromCellCol;
//            i_ToCellRow = ai_FromCellRow;
//            i_ToCellCol = ai_FromCellCol;
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//        }

//        public EPExcelRange(Worksheet a_sheet)
//        {
//            oSheet = a_sheet;
//        }

//        public EPExcelRange SetCells(int ai_FromCellRow, int ai_FromCellCol, int ai_ToCellRow, int ai_ToCellCol)
//        {
//            i_FromCellRow = ai_FromCellRow;
//            i_FromCellCol = ai_FromCellCol;
//            i_ToCellRow = ai_FromCellRow;
//            i_ToCellCol = ai_FromCellCol;
//            XLRange = oSheet.get_Range(oSheet.Cells[ai_FromCellRow, ai_FromCellCol], oSheet.Cells[ai_ToCellRow, ai_ToCellCol]);
//            return this;
//        }

//        public EPExcelRange SetCells(int ai_FromCellRow, int ai_FromCellCol)
//        {
//            i_FromCellRow = ai_FromCellRow;
//            i_FromCellCol = ai_FromCellCol;
//            i_ToCellRow = ai_FromCellRow;
//            i_ToCellCol = ai_FromCellCol;
//            XLRange = oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            return this;
//        }

//        public EPExcelRange SetCellValue(int ai_FromCellRow, int ai_FromCellCol, object o_value)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            XLRange.Value2 = o_value;
//            return this;
//        }

//        public object GetCellValue(int ai_FromCellRow, int ai_FromCellCol)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            return XLRange.Value2;
//        }

//        public DateTime GetCellDate(int ai_FromCellRow, int ai_FromCellCol)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            return ExcelSerialDateToDMY(Convert.ToInt32(XLRange.Value2));
//        }

//        public int GetCellColorIndex(int ai_FromCellRow, int ai_FromCellCol)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            return Convert.ToInt32(XLRange.Interior.ColorIndex);
//        }

//        public void SetCellColorIndex(int ai_FromCellRow, int ai_FromCellCol, int ai_XLColor)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            XLRange.Interior.ColorIndex = ai_XLColor;

//        }

//        public void SetCellColorIndex(int ai_FromCellRow, int ai_FromCellCol, int ai_ToCellRow, int ai_ToCellCol, int ai_XLColor)
//        {
//            XLRange = oSheet.get_Range(oSheet.Cells[ai_FromCellRow, ai_FromCellCol], oSheet.Cells[ai_ToCellRow, ai_ToCellCol]);
//            XLRange.Interior.ColorIndex = ai_XLColor;
//        }

//        public EPExcelRange SetCellFormulaR1C1(int ai_FromCellRow, int ai_FromCellCol, object o_value)
//        {
//            XLRange = (Range)oSheet.Cells[ai_FromCellRow, ai_FromCellCol];
//            XLRange.FormulaR1C1 = o_value;
//            return this;
//        }

//        public EPExcelRange HorizontalAlignmentRight()
//        {
//            XLRange.HorizontalAlignment = XlHAlign.xlHAlignRight;
//            return this;
//        }

//        public EPExcelRange HorizontalAlignmentCenter()
//        {
//            XLRange.HorizontalAlignment = XlHAlign.xlHAlignCenter;
//            return this;
//        }

//        public EPExcelRange SetBorder()
//        {
//            XLRange.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeRight].LineStyle = 1;
//            return this;
//        }

//        public EPExcelRange SetInsideVertical()
//        {
//            XLRange.Borders[XlBordersIndex.xlInsideVertical].LineStyle = 1;
//            return this;
//        }

//        public EPExcelRange SetDoubleBorder()
//        {
//            XLRange.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeLeft].Weight = -4138;
//            XLRange.Borders[XlBordersIndex.xlEdgeTop].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeTop].Weight = -4138;
//            XLRange.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeBottom].Weight = -4138;
//            XLRange.Borders[XlBordersIndex.xlEdgeRight].LineStyle = 1;
//            XLRange.Borders[XlBordersIndex.xlEdgeRight].Weight = -4138;
//            return this;
//        }

//        public EPExcelRange SetBoldFont()
//        {
//            XLRange.Font.Bold = true;
//            return this;
//        }

//        public EPExcelRange SetFontSize(int ai_size)
//        {
//            if (ai_size < 8) ai_size = 8;
//            XLRange.Font.Size = ai_size;
//            return this;
//        }

//        public EPExcelRange Sort(int ai_key1)//, int ai_key2, int ai_key3)
//        {
//            XLRange.Sort(XLRange.Columns[ai_key1, Type.Missing],
//                  XlSortOrder.xlAscending,
//                  Type.Missing,
//                  Type.Missing,
//                  XlSortOrder.xlAscending,
//                  Type.Missing,
//                  XlSortOrder.xlAscending,
//                  XlYesNoGuess.xlNo,
//                  Type.Missing,
//                  Type.Missing,
//                  XlSortOrientation.xlSortColumns,
//                  XlSortMethod.xlPinYin);
//            ////Excel.XlSortDataOption.xlSortNormal,
//            ////Excel.XlSortDataOption.xlSortNormal,
//            ////Excel.XlSortDataOption.xlSortNormal);
//            return this;
//        }

//        public DateTime ExcelSerialDateToDMY(int nSerialDate)
//        {
//            int nDay;
//            int nMonth;
//            int nYear;
//            // Excel/Lotus 123 have a bug with 29-02-1900. 1900 is not a
//            // leap year, but Excel/Lotus 123 think it is...
//            if (nSerialDate == 60)
//            {
//                nDay = 29;
//                nMonth = 2;
//                nYear = 1900;
//                return new DateTime(nYear, nMonth, nDay);
//            }
//            else if (nSerialDate < 60)
//            {
//                // Because of the 29-02-1900 bug, any serial date 
//                // under 60 is one off... Compensate.
//                nSerialDate++;
//            }

//            // Modified Julian to DMY calculation with an addition of 2415019
//            int l = nSerialDate + 68569 + 2415019;
//            int n = (4 * l) / 146097;
//            l = l - (146097 * n + 3) / 4;
//            int i = 4000 * (l + 1) / 1461001;
//            l = l - (1461 * i) / 4 + 31;
//            int j = (80 * l) / 2447;
//            nDay = l - (2447 * j) / 80;
//            l = j / 11;
//            nMonth = j + 2 - (12 * l);
//            nYear = 100 * (n - 49) + i + l;
//            return new DateTime(nYear, nMonth, nDay);
//        }

//        public int DMYToExcelSerialDate(int nDay, int nMonth, int nYear)
//        {
//            // Excel/Lotus 123 have a bug with 29-02-1900. 1900 is not a
//            // leap year, but Excel/Lotus 123 think it is...
//            if (nDay == 29 && nMonth == 02 && nYear == 1900)
//                return 60;

//            // DMY to Modified Julian calculatie with an extra substraction of 2415019.
//            long nSerialDate =
//                    (1461 * (nYear + 4800 + (nMonth - 14) / 12)) / 4 +
//                    (367 * (nMonth - 2 - 12 * ((nMonth - 14) / 12))) / 12 -
//                    (3 * ((nYear + 4900 + (nMonth - 14) / 12) / 100)) / 4 +
//                    nDay - 2415019 - 32075;

//            if (nSerialDate < 60)
//            {
//                // Because of the 29-02-1900 bug, any serial date 
//                // under 60 is one off... Compensate.
//                nSerialDate--;
//            }

//            return (int)nSerialDate;
//        }

//    }
//}