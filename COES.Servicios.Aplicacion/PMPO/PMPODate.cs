using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.PMPO
{
    public class PMPODate
    {
        public PMPODate()
        {
        }

        /// <summary>
        /// Calcula el día del primer sábado de inicio de año
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static int PrimerSabadoDelMes(int anio, int mes)
        {
            var primerSabadoDelMes = new DateTime(anio, mes, 1);

            //Obtener el dia
            while (primerSabadoDelMes.DayOfWeek != DayOfWeek.Saturday)
                primerSabadoDelMes = primerSabadoDelMes.AddDays(1);

            return primerSabadoDelMes.Day;
        }

        /// <summary>
        /// Calcula la fecha Inicio para año operativo PMPO
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public static DateTime FechaInicioAnho(int anio)
        {
            DateTime fechaInicioAnio;
            int diaPrimersabado = PrimerSabadoDelMes(anio, 1);

            if (diaPrimersabado <= 4)
            {
                fechaInicioAnio = new DateTime(anio, 1, diaPrimersabado);
            }
            else
            {
                fechaInicioAnio = new DateTime(anio, 1, diaPrimersabado).AddDays(-7);
            }

            return fechaInicioAnio;
        }

        /// <summary>
        /// Calcula la fecha Fin para año operativo PMPO
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="numerosemana"></param>
        /// <param name="fechaInicioAnio"></param>
        /// <returns></returns>
        public static DateTime FechaFinAnho(int anio, int numerosemana, DateTime fechaInicioAnho)
        {
            return Fechainiciosemana(numerosemana, fechaInicioAnho).AddDays(6);
        }

        public static DateTime Fechainiciosemana(int numerosemana, DateTime fechaInicioAnho)
        {
            DateTime ldt_temp = fechaInicioAnho;

            if ((int)ldt_temp.DayOfWeek >= 3) //miercoles
            {
                return ldt_temp.AddDays(7 * (numerosemana - 1) + (int)DayOfWeek.Saturday - (int)ldt_temp.DayOfWeek);
            }
            else //La semana anterior
            {
                return ldt_temp.AddDays(7 * (numerosemana - 1) + (int)DayOfWeek.Saturday - (int)ldt_temp.DayOfWeek - 7);
            }
        }

        /// <summary>
        /// Calcula el día del primer sábado de un mes para un año respectivo
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static DateTime FechaInicioSemanaMes(int anio, int mes)
        {
            DateTime fechaInicioAnio;
            int diaPrimersabado = PrimerSabadoDelMes(anio, mes);

            if (diaPrimersabado <= 4)
            {
                fechaInicioAnio = new DateTime(anio, mes, diaPrimersabado);
            }
            else
            {
                fechaInicioAnio = new DateTime(anio, mes, diaPrimersabado).AddDays(-7);
            }

            return fechaInicioAnio;
        }

        /// <summary>
        /// Obtener el mes que pertenece la fecha de consulta. El resultado es el día 1 del mes
        /// </summary>
        /// <param name="fechaSabConsulta"></param>
        /// <returns></returns>
        public static DateTime FechaMes(DateTime fechaSabConsulta)
        {
            DateTime fecha1Mes = new DateTime(fechaSabConsulta.Year, fechaSabConsulta.Month, 1);
            DateTime fecha1MesAnt = fecha1Mes.AddMonths(-1);
            DateTime fecha1MesSig = fecha1Mes.AddMonths(1);

            DateTime fechaSabMesAnt = FechaInicioSemanaMes(fecha1MesAnt.Year, fecha1MesAnt.Month);
            DateTime fechaSabMes = FechaInicioSemanaMes(fecha1Mes.Year, fecha1Mes.Month);
            DateTime fechaSabMesSig = FechaInicioSemanaMes(fecha1MesSig.Year, fecha1MesSig.Month);

            if (fechaSabMes <= fechaSabConsulta && fechaSabConsulta < fechaSabMesSig)
                return fecha1Mes;
            else
            {
                if (fechaSabMesAnt <= fechaSabConsulta && fechaSabConsulta < fechaSabMes)
                    return fecha1MesAnt;
                else
                    return fecha1MesSig;
            }
        }

        /// <summary>
        /// Obtener NOmbre del mes
        /// </summary>
        /// <param name="numeroMes"></param>
        /// <returns></returns>
        public static string NombreMes(int numeroMes)
        {
            switch (numeroMes)
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
    }
}
