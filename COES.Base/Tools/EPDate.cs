using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Tools
{
    public class EPDate
    {
        public EPDate()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Obtiene SOLO el numero de semana, MEJOR UTILIZAR LA FUNCION f_numerosemana_y_anho(DateTime ad_fecha)
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static int f_numerosemana(DateTime ad_fecha)
        {
            return EPWeekNumber_Entire4DayWeekRule(ad_fecha).Item1;
        }

        /// <summary>
        /// Obtiene el numero de semana y el año de la semana (podria ser año diferente a la fecha para los ultimos dias del año)
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static Tuple<int, int> f_numerosemana_y_anho(DateTime ad_fecha)
        {
            return EPWeekNumber_Entire4DayWeekRule(ad_fecha);
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

            if ((int)ldt_temp.DayOfWeek >= 3) //miercoles
            {
                return ldt_temp.AddDays(7 * (ai_numerosemana - 1) + (int)DayOfWeek.Saturday - (int)ldt_temp.DayOfWeek);
            }
            else //La semana anterior
            {
                return ldt_temp.AddDays(7 * (ai_numerosemana - 1) + (int)DayOfWeek.Saturday - (int)ldt_temp.DayOfWeek - 7);
            }
        }

        public static DateTime f_fechafinsemana(int ai_numeroanno, int ai_numerosemana)
        {
            /*DateTime ldt_temp = new DateTime(ai_numeroanno, 1, 1);
            return ldt_temp.AddDays(7 * (ai_numerosemana - 1) - (int)ldt_temp.DayOfWeek + 5);*/
            return f_fechainiciosemana(ai_numeroanno, ai_numerosemana).AddDays(6);
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

        public static DateTime ToDate(string inValue)
        {
            IFormatProvider culture = new CultureInfo("fr-FR", true);
            DateTime myDateTimeFrench = DateTime.Parse(inValue, culture, DateTimeStyles.NoCurrentDateDefault);
            return myDateTimeFrench;
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
                    return "Miércoles";
                case DayOfWeek.Thursday:
                    return "Jueves";
                case DayOfWeek.Friday:
                    return "Viernes";
                case DayOfWeek.Saturday:
                    return "Sábado";
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

        /// <summary>
        /// Devuelve semana y año
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        static public Tuple<int, int> EPWeekNumber_Entire4DayWeekRule(DateTime fechaParam)
        {
            DateTime date = fechaParam.Date;

            //Año
            int anioAnt = date.Year - 1;
            int anio = date.Year;
            int anioDesp = date.Year + 1;

            int totalSemanas = 0;
            DateTime fechaContador = DateTime.MinValue, fechaIniSem01 = DateTime.MinValue;

            ////////////////////////////////////////////////////////////////////////////////////
            /// Buscar en que año pertenece la fecha que está como parámetro
            ////////////////////////////////////////////////////////////////////////////////////
            //Año de la fecha que esta como parametro
            totalSemanas = EPDate.TotalSemanasEnAnho(anio, (int)DayOfWeek.Saturday);
            fechaIniSem01 = EPDate.f_fechainiciosemana(anio, 1);
            fechaContador = fechaIniSem01;
            for (int sem = 1; sem <= totalSemanas; sem++)
            {
                if (fechaContador <= date && date <= fechaContador.AddDays(6))
                    return new Tuple<int, int>(sem, anio);

                fechaContador = fechaContador.AddDays(7);
            }

            //Año antes de la fecha que esta como parametro
            totalSemanas = EPDate.TotalSemanasEnAnho(anioAnt, (int)DayOfWeek.Saturday);
            fechaIniSem01 = EPDate.f_fechainiciosemana(anioAnt, 1);
            fechaContador = fechaIniSem01;
            for (int sem = 1; sem <= totalSemanas; sem++)
            {
                if (fechaContador <= date && date <= fechaContador.AddDays(6))
                    return new Tuple<int, int>(sem, anioAnt);

                fechaContador = fechaContador.AddDays(7);
            }

            //Año despues de la fecha que esta como parametro
            totalSemanas = EPDate.TotalSemanasEnAnho(anioDesp, (int)DayOfWeek.Saturday);
            fechaIniSem01 = EPDate.f_fechainiciosemana(anioDesp, 1);
            fechaContador = fechaIniSem01;
            for (int sem = 1; sem <= totalSemanas; sem++)
            {
                if (fechaContador <= date && date <= fechaContador.AddDays(6))
                    return new Tuple<int, int>(sem, anioDesp);

                fechaContador = fechaContador.AddDays(7);
            }

            return new Tuple<int, int>(1, anio);
        }
        /// <summary>
        /// Obtiene la fecha inicio del periodo (diario,semanal,mensual)
        /// </summary>
        /// <param name="periodo"></param>
        /// <param name="fecha"></param>
        /// <param name="formatoFecha"></param>
        /// <returns></returns>
        public static DateTime GetFechaIniPeriodo(int periodo, string mes, string semana, string dia, string formatoFecha)
        {
            DateTime fechaProceso = DateTime.MinValue;

            switch (periodo)
            {
                case 1:
                    fechaProceso = DateTime.ParseExact(dia, formatoFecha, CultureInfo.InvariantCulture);
                    break;
                case 2:
                    int anho = Int32.Parse(semana.Substring(0, 4));
                    int sem = Int32.Parse(semana.Substring(4, semana.Length - 4));
                    fechaProceso = EPDate.f_fechainiciosemana(anho, sem);
                    break;
                case 3:
                case 5:
                    int imes = 0;
                    int ianho = 0;

                    if (mes.Contains(" "))
                    {
                        imes = Int32.Parse(mes.Substring(0, 2));
                        ianho = Int32.Parse(mes.Substring(3, 4));
                    }
                    else
                    {
                        string[] subs = mes.Split('-');
                        imes = Int32.Parse(subs[1]);
                        ianho = Int32.Parse(subs[0]);
                    }

                    fechaProceso = new DateTime(ianho, imes, 1);
                    break;
                    //case 5: // Semanal Mediano 
                    //    //Calcular la fecha inicio de las cuatro semanas. anteriores al mes.
                    //    int imes2 = Int32.Parse(mes.Substring(0, 2));
                    //    int ianho2 = Int32.Parse(mes.Substring(3, 4));
                    //    DateTime fechaInicio = new DateTime(ianho2, imes2, 1).AddMonths(1);
                    //    fechaProceso = EPDate.f_fechainiciosemana(fechaInicio).AddDays(-29);
                    //    break;
            }
            return fechaProceso;
        }

        public static int TotalSemanasEnAnho(int year, int dayOfWeek)
        {
            int nroSemana = 0;
            DateTime fecha = new DateTime(year, 12, 28);
            switch (dayOfWeek)
            {
                case 0:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
                    break;
                case 6:
                    nroSemana = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(fecha, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Saturday);
                    break;
            }
            return nroSemana;

        }

        public static DateTime GetFechaSemana(string stFecha)
        {
            DateTime fecha = DateTime.MinValue;
            int year = int.Parse(stFecha.Substring(0, 4));
            int nroSemana = int.Parse(stFecha.Substring(9, 2));
            fecha = f_fechainiciosemana(year, nroSemana);
            return fecha;
        }

        public static DateTime GetFechaYearMonth(string stFecha)
        {
            DateTime fecha = DateTime.MinValue;

            string stMes = stFecha.Substring(5, 3);
            switch (stMes)
            {
                case "Ene":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 1, 1);
                    break;
                case "Feb":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 2, 1);
                    break;
                case "Mar":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 3, 1);
                    break;
                case "Abr":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 4, 1);
                    break;
                case "May":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 5, 1);
                    break;
                case "Jun":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 6, 1);
                    break;
                case "Jul":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 7, 1);
                    break;
                case "Ago":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 8, 1);
                    break;
                case "Sep":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 9, 1);
                    break;
                case "Set":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 9, 1);
                    break;
                case "Oct":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 10, 1);
                    break;
                case "Nov":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 11, 1);
                    break;
                case "Dic":
                    fecha = new DateTime(int.Parse(stFecha.Substring(0, 4)), 12, 1);
                    break;
            }
            return fecha;
        }

        public static string GetFechaMes(DateTime fecha)
        {
            string mesanho = string.Empty;
            if (fecha.Month < 10)
                mesanho = "0" + fecha.Month.ToString() + " " + fecha.Year.ToString();
            else
                mesanho = fecha.Month.ToString() + " " + fecha.Year.ToString();

            return mesanho;
        }

        /// <summary>
        /// Dado un par de fechas, devuelve un listado de periodos (primer día y final de mes)
        /// </summary>
        /// <param name="fechaIni">Fecha Fnicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <returns>Listado de Periodos</returns>
        public static List<Periodo> GetPeriodos(DateTime fechaIni, DateTime fechaFin)
        {
            List<Periodo> listPeriodo = new List<Periodo>();
            if (fechaFin < fechaIni)
                return null;
            if (fechaIni.Month == fechaFin.Month && fechaIni.Year == fechaFin.Year)
            {
                Periodo oPeriodo = new Periodo()
                {
                    FechaInicio = new DateTime(fechaIni.Year, fechaIni.Month, 1),
                    FechaFin = new DateTime(fechaIni.Year, fechaIni.Month, 1).AddMonths(1).AddDays(-1)
                };
                listPeriodo.Add(oPeriodo);
            }
            else
            {
                DateTime dfechaFinTotal = new DateTime(fechaFin.Year, fechaFin.Month, 1).AddMonths(1).AddDays(-1);
                for (DateTime auxDateTime = new DateTime(fechaIni.Year, fechaIni.Month, 1); auxDateTime < dfechaFinTotal; auxDateTime = auxDateTime.AddMonths(1))
                {
                    Periodo oPeriodo = new Periodo()
                    {
                        FechaInicio = auxDateTime,
                        FechaFin = auxDateTime.AddMonths(1).AddDays(-1)
                    };
                    listPeriodo.Add(oPeriodo);
                }
            }
            return listPeriodo;
        }


        /// <summary>
        /// Retorna colección combinado de periodos con interseccion
        /// </summary>
        /// <param name="periodos"></param>
        /// <returns></returns>
        public static IEnumerable<Periodo> GetPeriodosCombinadosXInterceccion(IEnumerable<Periodo> periodos)
        {
            DateTime puntoInicio, puntoFin;
            using (var periodo = periodos.OrderBy(r => r.FechaInicio).GetEnumerator())
            {
                bool haySigRegisto = periodo.MoveNext();
                while (haySigRegisto)
                {
                    puntoInicio = periodo.Current.FechaInicio;
                    puntoFin = periodo.Current.FechaFin;
                    while ((haySigRegisto = periodo.MoveNext()) && periodo.Current.FechaInicio < puntoFin)
                    {
                        if (periodo.Current.FechaFin > puntoFin)
                        {
                            puntoFin = periodo.Current.FechaFin;
                        }
                    }
                    yield return new Periodo() { FechaInicio = puntoInicio, FechaFin = puntoFin, Duracion = puntoFin.Subtract(puntoInicio) };
                }
            }
        }

        /// <summary>
        /// Retorna colección combinado de periodos consecutivos
        /// </summary>
        /// <param name="periodos"></param>
        /// <returns></returns>
        public static IEnumerable<Periodo> GetPeriodosCombinadosConsecutivos(IEnumerable<Periodo> periodos)
        {
            DateTime puntoInicio, puntoFin;
            using (var periodo = periodos.OrderBy(r => r.FechaInicio).GetEnumerator())
            {
                bool haySigRegisto = periodo.MoveNext();
                while (haySigRegisto)
                {
                    puntoInicio = periodo.Current.FechaInicio;
                    puntoFin = periodo.Current.FechaFin;
                    while ((haySigRegisto = periodo.MoveNext()) && periodo.Current.FechaInicio == puntoFin)
                    {
                        puntoFin = periodo.Current.FechaFin;
                    }
                    yield return new Periodo() { FechaInicio = puntoInicio, FechaFin = puntoFin, Duracion = puntoFin.Subtract(puntoInicio) };
                }
            }

        }

        public static List<Periodo> GetPeriodosSemanales(DateTime fechaIni, DateTime fechaFin)
        {
            var listaSemanas = new List<Periodo>();
            var fechaSemIni = f_fechainiciosemana(fechaIni);
            var fechaSemIni2 = f_fechainiciosemana(fechaFin);//Semana inicio de la fecha fin

            while (fechaSemIni <= fechaSemIni2)
            {
                listaSemanas.Add(new Periodo { FechaInicio = fechaSemIni, FechaFin = fechaSemIni.AddDays(6) });
                fechaSemIni = fechaSemIni.AddDays(7);
            }
            return listaSemanas;
        }
    }

    public class Periodo
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public TimeSpan Duracion { get; set; }
    }
}
