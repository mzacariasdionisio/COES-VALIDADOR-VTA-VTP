using System.Globalization;
using System;

namespace WSIC2010
{
    /// <summary>
    /// Summary description for EPDate.
    /// </summary>
    public class EPDate
    {
        public EPDate()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static DateTime FromExcelSerialDate(double SerialDate)
        {
            if (SerialDate > 59) SerialDate -= 1; //Excel/Lotus 2/29/1900 bug   
            return new DateTime(1899, 12, 31).AddDays(SerialDate);
        }

        public static DateTime ExcelSerialDateToDMY(int nSerialDate)
        {
            int nDay;
            int nMonth;
            int nYear;
            // Excel/Lotus 123 have a bug with 29-02-1900. 1900 is not a
            // leap year, but Excel/Lotus 123 think it is...
            if (nSerialDate == 60)
            {
                nDay = 29;
                nMonth = 2;
                nYear = 1900;
                return new DateTime(nYear, nMonth, nDay);
            }
            else if (nSerialDate < 60)
            {
                // Because of the 29-02-1900 bug, any serial date 
                // under 60 is one off... Compensate.
                nSerialDate++;
            }

            // Modified Julian to DMY calculation with an addition of 2415019
            int l = nSerialDate + 68569 + 2415019;
            int n = (4 * l) / 146097;
            l = l - (146097 * n + 3) / 4;
            int i = 4000 * (l + 1) / 1461001;
            l = l - (1461 * i) / 4 + 31;
            int j = (80 * l) / 2447;
            nDay = l - (2447 * j) / 80;
            l = j / 11;
            nMonth = j + 2 - (12 * l);
            nYear = 100 * (n - 49) + i + l;
            return new DateTime(nYear, nMonth, nDay);
        }

        public static int f_numerosemana(DateTime ad_fecha)
        {
            //if (ad_fecha.Year > 2005)         
            //return (ad_fecha.DayOfYear + (int)ad_fecha.DayOfWeek) / 7;
            //else
            //   return (ad_fecha.DayOfYear + (int)ad_fecha.DayOfWeek) / 7 - 1;        
            return EPWeekNumber_Entire4DayWeekRule(ad_fecha);
            // return EPWeekNumber(ad_fecha);
        }

        public static DateTime f_fechainiciosemana(DateTime adt_FechadentrodelaSemana)
        {
            if (adt_FechadentrodelaSemana.DayOfWeek == DayOfWeek.Saturday)
                return adt_FechadentrodelaSemana;
            else
                return adt_FechadentrodelaSemana.AddDays(-(int)adt_FechadentrodelaSemana.DayOfWeek - 1);
        }

        public static DateTime f_fechafinsemana(DateTime adt_FechadentrodelaSemana)
        {
            if (adt_FechadentrodelaSemana.DayOfWeek == DayOfWeek.Saturday)
                return adt_FechadentrodelaSemana.AddDays(6);
            else
                return adt_FechadentrodelaSemana.AddDays(5 - (int)adt_FechadentrodelaSemana.DayOfWeek);
        }

        public static DateTime f_fechainiciosemana(int ai_numeroanno, int ai_numerosemana)
        {
            DateTime ldt_temp = new DateTime(ai_numeroanno, 1, 1);
            return ldt_temp.AddDays(7 * (ai_numerosemana - 1) - (int)ldt_temp.DayOfWeek - 1);
        }

        public static DateTime f_fechafinsemana(int ai_numeroanno, int ai_numerosemana)
        {
            DateTime ldt_temp = new DateTime(ai_numeroanno, 1, 1);
            return ldt_temp.AddDays(7 * (ai_numerosemana - 1) - (int)ldt_temp.DayOfWeek + 5);
        }

        public static bool IsDate(string inValue)
        {
            bool bValid;
            try
            {
                IFormatProvider culture = new CultureInfo("fr-FR", true);
                DateTime myDateTimeFrench = DateTime.Parse(inValue, culture, DateTimeStyles.NoCurrentDateDefault);
                //DateTime myDT = DateTime.Parse(inValue);
                bValid = true;
            }
            catch //(FormatException e)
            {
                bValid = false;
            }
            return bValid;
        }

        public static DateTime ToDateMMDDYYYY(string inValue)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            DateTime myDateTimeEnglish = DateTime.Parse(inValue, culture, DateTimeStyles.NoCurrentDateDefault);
            return myDateTimeEnglish;
        }

        public static string f_NombreDiaSemana(DayOfWeek a_diasemana)
        {
            switch (a_diasemana)
            {
                case DayOfWeek.Sunday:
                    return "Domingo";
                case DayOfWeek.Monday:
                    return "Lunes";
                case DayOfWeek.Tuesday:
                    return "Martes";
                case DayOfWeek.Wednesday:
                    return "Miercoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sabado";
                default:
                    return "No definido!";
            }
        }

        public static string f_NombreDiaSemanaCorto(DayOfWeek a_diasemana)
        {
            switch (a_diasemana)
            {
                case DayOfWeek.Sunday:
                    return "DOM";
                case DayOfWeek.Monday:
                    return "LUN";
                case DayOfWeek.Tuesday:
                    return "MAR";
                case DayOfWeek.Wednesday:
                    return "MIE";
                case DayOfWeek.Thursday:
                    return "JUE";
                case DayOfWeek.Friday:
                    return "VIE";
                case DayOfWeek.Saturday:
                    return "SAB";
                default:
                    return "_NODEF";
            }
        }

        static public string f_FechaenLetras(DateTime adt_fecha)
        {
            return adt_fecha.Day + " de " + f_NombreMes(adt_fecha.Month) + " del " + adt_fecha.Year;
        }

        public static string f_NombreMes(int ai_MonthNumber)
        {
            switch (ai_MonthNumber)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return "No Definido";
            }
        }

        public static string f_NombreMesCorto(int ai_MonthNumber)
        {
            switch (ai_MonthNumber)
            {
                case 1:
                    return "ENE";
                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "ABR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AGO";
                case 9:
                    return "SET";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DIC";
                default:
                    return "_NODEF";
            }
        }


        public static DateTime ToDate(string inValue)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime myDateTimeFrench = DateTime.Parse(inValue, culture, DateTimeStyles.NoCurrentDateDefault);
            return myDateTimeFrench;
        }

        static public DateTime ToDateTime(string inValue)
        {
            DateTime myDateTimeFrench = new DateTime(2000, 1, 1, 0, 0, 0);

            try
            {
                IFormatProvider culture = new CultureInfo("fr-FR", true);
                myDateTimeFrench = DateTime.Parse(inValue, culture, DateTimeStyles.NoCurrentDateDefault);
            }
            catch //(FormatException e) 
            { }
            return myDateTimeFrench;
        }

        static public string SQLDateString(DateTime a_datetime)
        {
            return "'" + a_datetime.ToString("yyyy-MM-dd 00:00:00") + "'";
        }

        static public string SQLDateTimeString(DateTime a_datetime)
        {
            return "'" + a_datetime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
        }

        static public string SQLDateOracleString(DateTime a_datetime)
        {
            return " TO_DATE('" + a_datetime.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
        }

        static public string SQLDateTimeOracleString(DateTime a_datetime)
        {
            return " TO_DATE('" + a_datetime.ToString("yyyy-MM-dd HH:mm:ss") + "','YYYY-MM-DD HH24:MI:SS') ";
        }

        static public int XWeekNumber_Entire4DayWeekRule(DateTime date)
        {
            // Updated 2004.09.27. Cleaned the code and fixed a bug. Compared the algorithm with
            // code published here . Tested code successfully against the other algorithm 
            // for all dates in all years between 1900 and 2100.
            // Thanks to Marcus Dahlberg for pointing out the deficient logic.
            // Calculates the ISO 8601 Week Number
            // In this scenario the first day of the week is monday, 
            // and the week rule states that:
            // [...] the first calendar week of a year is the one 
            // that includes the first Thursday of that year and 
            // [...] the last calendar week of a calendar year is 
            // the week immediately preceding the first 
            // calendar week of the next year.
            // The first week of the year may thus start in the 
            // preceding year

            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int THURSDAY = 4;
            bool ThursdayFlag = false;

            // Get the day number since the beginning of the year
            int DayOfYear = date.DayOfYear;

            // Get the numeric weekday of the first day of the 
            // year (using sunday as FirstDay)
            int StartWeekDayOfYear =
                 (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int EndWeekDayOfYear =
                 (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            // Compensate for the fact that we are using monday
            // as the first day of the week
            if (StartWeekDayOfYear == 0)
                StartWeekDayOfYear = 7;
            if (EndWeekDayOfYear == 0)
                EndWeekDayOfYear = 7;

            // Calculate the number of days in the first and last week
            int DaysInFirstWeek = 6 - (StartWeekDayOfYear);
            int DaysInLastWeek = 6 - (EndWeekDayOfYear);

            // If the year either starts or ends on a thursday it will have a 53rd week
            if (StartWeekDayOfYear == THURSDAY || EndWeekDayOfYear == THURSDAY)
                ThursdayFlag = true;

            // We begin by calculating the number of FULL weeks between the start of the year and
            // our date. The number is rounded up, so the smallest possible value is 0.
            int FullWeeks = (int)Math.Ceiling((DayOfYear - (DaysInFirstWeek)) / 7.0);

            int WeekNumber = FullWeeks;

            // If the first week of the year has at least four days, then the actual week number for our date
            // can be incremented by one.
            if (DaysInFirstWeek >= THURSDAY)
                WeekNumber = WeekNumber + 1;

            // If week number is larger than week 52 (and the year doesn't either start or end on a thursday)
            // then the correct week number is 1. 
            if (WeekNumber > 52 && !ThursdayFlag)
                WeekNumber = 1;

            // If week number is still 0, it means that we are trying to evaluate the week number for a
            // week that belongs in the previous year (since that week has 3 days or less in our date's year).
            // We therefore make a recursive call using the last day of the previous year.
            if (WeekNumber == 0)
                WeekNumber = XWeekNumber_Entire4DayWeekRule(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));
            return WeekNumber;
        }

        static public int EPWeekNumber_Entire4DayWeekRule(DateTime date)
        {
            const int JAN = 1;
            const int DEC = 12;
            const int LASTDAYOFDEC = 31;
            const int FIRSTDAYOFJAN = 1;
            const int TUESDAY = 2;
            // const int THURSDAY = 4;
            //bool ThursdayFlag = false;

            // Get the day number since the beginning of the year
            int DayOfYear = date.DayOfYear;

            // Get the numeric weekday of the first day of the 
            // year (using sunday as FirstDay)
            int StartWeekDayOfYear = (int)(new DateTime(date.Year, JAN, FIRSTDAYOFJAN)).DayOfWeek;
            int EndWeekDayOfYear = (int)(new DateTime(date.Year, DEC, LASTDAYOFDEC)).DayOfWeek;

            // Compensate for the fact that we are using monday
            // as the first day of the week
            if (StartWeekDayOfYear == 0) StartWeekDayOfYear = 7;
            if (EndWeekDayOfYear == 0) EndWeekDayOfYear = 7;

            // Calculate the number of days in the first and last week
            int DaysInFirstWeek = 6 - (StartWeekDayOfYear);
            int DaysInLastWeek = 6 - (EndWeekDayOfYear);

            // If the year either starts or ends on a thursday it will have a 53rd week
            //if (StartWeekDayOfYear == TUESDAY || EndWeekDayOfYear == TUESDAY)
            //   ThursdayFlag = true;

            // We begin by calculating the number of FULL weeks between the start of the year and
            // our date. The number is rounded up, so the smallest possible value is 0.
            int FullWeeks = (int)Math.Ceiling((DayOfYear - (DaysInFirstWeek)) / 7.0);

            int WeekNumber = FullWeeks;

            // If the first week of the year has at least four days, then the actual week number for our date
            // can be incremented by one.
            if (DaysInFirstWeek >= 4)
                WeekNumber = WeekNumber + 1;

            // If week number is larger than week 52 (and the year doesn't either start or end on a thursday)
            // then the correct week number is 1. 
            //if (WeekNumber > 52 && !ThursdayFlag)
            //   WeekNumber = 1;

            // If week number is still 0, it means that we are trying to evaluate the week number for a
            // week that belongs in the previous year (since that week has 3 days or less in our date's year).
            // We therefore make a recursive call using the last day of the previous year.
            if (WeekNumber == 0)
                WeekNumber = EPWeekNumber_Entire4DayWeekRule(new DateTime(date.Year - 1, DEC, LASTDAYOFDEC));
            return WeekNumber;
        }

        //static public int EPWeekNumber(DateTime date)
        //{
        //   DateTime ldt_InicioSemana = f_fechainiciosemana(date);
        //   DateTime ldt_InicioSemanaInicioAnno = f_fechainiciosemana(new DateTime(ldt_InicioSemana.Year, 1, 1));         

        //   if (ldt_InicioSemanaInicioAnno.Day < 29)
        //   {
        //      ldt_InicioSemanaInicioAnno = f_fechainiciosemana(new DateTime(ldt_InicioSemana.Year+1, 1, 1)); 
        //   }

        //   return Convert.ToInt32((ldt_InicioSemana - ldt_InicioSemanaInicioAnno).TotalDays) / 7 ;

        //}
    }
}
