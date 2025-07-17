//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Microsoft.Office.Interop.Excel;
//using System.Runtime.InteropServices;
//using System.Runtime.InteropServices.ComTypes;

//namespace WSIC2010.Util
//{
//    public class EPExcel
//    {
//        public static object oMissing = System.Type.Missing;

//        public Application oExcel = null;
//        public Workbook oBook = null;
//        public Worksheet oSheet = null;

//        //public Range EPRangeExcel
//        public EPExcel()
//        {
//            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
//        }

//        public Range Range(string as_FromCell, string as_ToCell)
//        {
//            return ((Range)oSheet.get_Range(as_FromCell, as_ToCell));
//        }

//        public Range Range(int ai_FromCellRow, int ai_FromCellCol, int ai_ToCellRow, int ai_ToCellCol)
//        {
//            return oSheet.get_Range(oSheet.Cells[ai_FromCellRow, ai_FromCellCol], oSheet.Cells[ai_ToCellRow, ai_ToCellCol]);
//        }

//        public Range Rows(int ai_rowini, int ai_rowfin)
//        {
//            return (Range)oSheet.Rows[ai_rowini, ai_rowfin];
//        }

//        public Worksheet NewSheet()
//        {
//            oExcel = new Application();
//            oExcel.SheetsInNewWorkbook = 1;
//            oExcel.Visible = true;
//            oSheet = (Worksheet)oBook.ActiveSheet;
//            return oSheet;
//        }

//        public Worksheet NewSheet(string a_name)
//        {
//            //oSheet = NewSheet();
//            oSheet = (Worksheet)oBook.Worksheets.Add(EPExcel.oMissing, EPExcel.oMissing, EPExcel.oMissing, EPExcel.oMissing);
//            oSheet.Name = a_name;
//            return oSheet;
//        }

//        public Worksheet GetSheet(string a_CodeName)
//        {
//            foreach (Worksheet osheet in oBook.Worksheets)
//            {
//                if (osheet._CodeName == a_CodeName)
//                {
//                    return osheet;
//                }
//            }

//            return null;
//        }

//        public Worksheet ActivateSheet(string a_CodeName)
//        {
//            foreach (Worksheet osheet in oBook.Worksheets)
//            {
//                if (osheet._CodeName == a_CodeName)
//                {
//                    return osheet;
//                }
//            }

//            return null;
//        }

//        public bool ConnectWorkBook(string as_workbookname)
//        {
//            try
//            {
//                oExcel = (Application)Marshal.GetActiveObject("Excel.Application");
//                oExcel.SheetsInNewWorkbook = 1;
//                foreach (Workbook obook in oExcel.Worksheets)
//                {
//                    if (obook.Name == as_workbookname)
//                    {
//                        oBook = obook;
//                        return true;
//                    }
//                }
//            }
//            catch
//            {
//                //return false;
//            }
//            return false;
//        }

//        public void SetColumnNames(string as_ColumnName, string as_ColumnC)
//        {
//            oSheet.Names.Add(as_ColumnName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
//                                    Type.Missing, Type.Missing, as_ColumnC, Type.Missing);
//        }

//        public static void SetBoldFont(Range a_range)
//        {
//            a_range.Font.Bold = true;
//        }

//        public static void SetBorder(Range a_range)
//        {
//            a_range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = 1;
//        }

//        public static void SetDoubleBorder(Range a_range)
//        {
//            a_range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeLeft].Weight = -4138;
//            a_range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeTop].Weight = -4138;
//            a_range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeBottom].Weight = -4138;
//            a_range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = 1;
//            a_range.Borders[XlBordersIndex.xlEdgeRight].Weight = -4138;
//        }

//        [DllImport("ole32.dll")]
//        static extern int GetRunningObjectTable(uint reserved, out IRunningObjectTable prot);
//        [DllImport("ole32.dll")]
//        static extern int CreateBindCtx(uint reserved, out IBindCtx pctx);

//        public static Workbook RetrieveWorkbook(string xlfile)
//        {
//            IRunningObjectTable prot = null;
//            IEnumMoniker pmonkenum = null;

//            try
//            {
//                IntPtr pfetched = IntPtr.Zero;
//                //Query the running object table (ROT)

//                if (GetRunningObjectTable(0, out prot) != 0 || prot == null) return null;
//                prot.EnumRunning(out pmonkenum); pmonkenum.Reset();
//                IMoniker[] monikers = new IMoniker[1];
//                while (pmonkenum.Next(1, monikers, pfetched) == 0)
//                {
//                    IBindCtx pctx; string filepathname;
//                    CreateBindCtx(0, out pctx);
//                    //Get the name of the file

//                    monikers[0].GetDisplayName(pctx, null, out filepathname);
//                    //Clean up

//                    Marshal.ReleaseComObject(pctx);
//                    //Shearch for the workbook

//                    //if (filename.IndexOf(xlfile) != -1)
//                    if (filepathname.Substring(filepathname.LastIndexOf("\\") + 1) == xlfile)
//                    {
//                        object roval;
//                        //Get a handle on the workbook

//                        prot.GetObject(monikers[0], out roval);
//                        return roval as Workbook;
//                    }
//                    // //if (filepathname.IndexOf(".xls") != -1)
//                    ////if(filepathname.Substring(filepathname.Length -4) == ".xls")
//                    ////  listBoxXLS.Items.Add(filepathname.Substring(filepathname>LastIndexOf("\\") + 1))
//                }
//            }
//            catch
//            {
//                return null;
//            }
//            finally
//            {
//                //Clean up
//                if (prot != null) Marshal.ReleaseComObject(prot);
//                if (pmonkenum != null) Marshal.ReleaseComObject(pmonkenum);
//            }

//            return null;
//        }

//        public static List<string> RetrieveWorkbooks()//string xlfile)
//        {
//            List<string> L_excelfiles = new List<string>();
//            IRunningObjectTable prot = null;
//            IEnumMoniker pmonkenum = null;
//            try
//            {
//                IntPtr pfetched = IntPtr.Zero;
//                if (GetRunningObjectTable(0, out prot) != 0 || prot == null) return null;
//                prot.EnumRunning(out pmonkenum); pmonkenum.Reset();
//                IMoniker[] monikers = new IMoniker[1];
//                while (pmonkenum.Next(1, monikers, pfetched) == 0)
//                {
//                    IBindCtx pctx; string filepathname;
//                    CreateBindCtx(0, out pctx);
//                    monikers[0].GetDisplayName(pctx, null, out filepathname);
//                    Marshal.ReleaseComObject(pctx);
//                    if (filepathname.Substring(filepathname.Length - 4).ToUpper() == ".XLS")
//                        L_excelfiles.Add(filepathname.Substring(filepathname.LastIndexOf("\\") + 1));
//                }
//                return L_excelfiles;
//            }
//            catch
//            {
//                return null;
//            }
//            finally
//            {
//                //Clean up
//                if (prot != null) Marshal.ReleaseComObject(prot);
//                if (pmonkenum != null) Marshal.ReleaseComObject(pmonkenum);
//            }

//        }

//    }
//}