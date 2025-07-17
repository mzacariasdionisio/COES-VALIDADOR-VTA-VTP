using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSIC2010.Util
{
    public class Demanda
    {
        public string BarraCodi { set; get; }
        public double[] ld_array_demandaReal48;
        public double[] ld_array_pronostico48;
    }

    public class Pronostico
    {
        public string BarraCodi { set; get; }
        public double[] ld_array_pronostico96;
    }

    public class Empresa
    {
        public int EmpresaCodi { set; get; }
        public string EmpresaNomb { set; get; }
        public string EmpresaAbrev { set; get; }
        public string TipoEmpresaAbrev { set; get; }
    }

    public class ExcelUtil
    {

        public static string GetExcelColumnName(int pi_columnNumber)
        {
            int li_dividend = pi_columnNumber;
            string ls_columnName = String.Empty;
            int li_modulo;

            while (li_dividend > 0)
            {
                li_modulo = (li_dividend - 1) % 26;
                ls_columnName = Convert.ToChar(65 + li_modulo).ToString() + ls_columnName;
                li_dividend = (int)((li_dividend - li_modulo) / 26);
            }

            return ls_columnName;
        }

        public static string GetTime(int pi_time)
        {
            if (pi_time != 96)
            {
                switch (pi_time % 4)
                {
                    case 0: 
                        return pi_time / 4 + ":00";
                    case 1:
                        return pi_time / 4 + ":15";
                    case 2: 
                        return pi_time / 4 + ":30";
                    case 3: 
                        return pi_time / 4 + ":45";
                    default: 
                        return " ";
                }
            }
            else if (pi_time == 96)
                return "0:00";
            else
                return " ";
            
        }

    }
}