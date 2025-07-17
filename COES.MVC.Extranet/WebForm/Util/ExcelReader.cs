using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using Excel;

namespace WSIC2010.Util
{
    public static class ExcelReader
    {
        public static DataSet nf_get_excel_to_ds(string ps_path, string ps_extension)
        {
            try
            {
                DataSet ds = new DataSet();
                FileStream stream = File.Open(ps_path, FileMode.Open, FileAccess.Read);
                if (ps_extension.Equals(".xls"))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    ds = excelReader.AsDataSet();
                }
                else if (ps_extension.Equals(".xlsx"))
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    ds = excelReader.AsDataSet();
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}