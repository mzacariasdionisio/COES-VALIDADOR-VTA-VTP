using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Valorizacion.Helper
{
    public class Dates
    {
        public static void ReturnMonthNumber(string monthName)
        {
            int number = 0;
            switch (monthName)
            {
                case "Ene":
                    number = 1;
                    break;
                case "Feb":
                    number = 2;
                    break;
                case "Mar":
                    number = 3;
                    break;
                case "Abr":
                    number = 4;
                    break;
                case "May":
                    number = 5;
                    break;
                case "Jun":
                    number = 6;
                    break;
                case "Jul":
                    number = 7;
                    break;
                case "Ago":
                    number = 8;
                    break;
                case "Sep":
                    number = 9;
                    break;
                case "Oct":
                    number = 10;
                    break;
                case "Nov":
                    number = 11;
                    break;
                case "Dic":
                    number = 12;
                    break;
            }


        }
    }
}


   