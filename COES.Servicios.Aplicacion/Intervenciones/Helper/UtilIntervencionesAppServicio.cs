using COES.Dominio.DTO.Sic;
using System;
using System.Linq;

namespace COES.Servicios.Aplicacion.Intervenciones.Helper
{
    /// <summary>
    /// Clase para colocar métodos comunes utilizados
    /// en la capa de aplicación
    /// </summary>    
    public class UtilIntervencionesAppServicio
    {
        /// <summary>
        /// Metodo que escribe una fila del archivo .CSV de Intervenciones Erroneas
        /// </summary>
        /// <param name="entity">Objeto InIntervencionDTO</param>
        /// <returns>Cadena</returns>
        public static string CreateFilaErroneaIntervencionString(InIntervencionDTO entity)
        {
            string sLine = string.Empty;

            sLine += entity.NroItem.ToString() + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Observaciones != null) ? entity.Observaciones.ToString().Replace(',', ';') : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Intercodsegempr != null) ? entity.Intercodsegempr.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.EquiNomb != null) ? entity.EquiNomb.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.EquiNomb != null) ? entity.EquiNomb.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Equicodi >0) ? entity.Equicodi.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Interfechaini != null) ? entity.Interfechaini.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Interfechafin != null) ? entity.Interfechafin.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Interdescrip != null) ? entity.Interdescrip.ToString().Replace(',', ';') : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += entity.Intermwindispo.ToString() + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Interindispo != null) ? entity.Interindispo.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.Interinterrup != null) ? entity.Interinterrup.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.IntNombTipoIntervencion != null) ? entity.IntNombTipoIntervencion.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;
            sLine += ((entity.IntNombClaseProgramacion != null) ? entity.IntNombClaseProgramacion.ToString() : "") + ConstantesIntervencionesAppServicio.SeparadorCampoCSV;

            return sLine;
        }

        /// <summary>
        /// Obtener día de la semana
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static string DiaDeLaSemana(DateTime fecha)
        {
            switch (fecha.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    return "Domingo";
                case System.DayOfWeek.Monday:
                    return "Lunes";
                case System.DayOfWeek.Tuesday:
                    return "Martes";
                case System.DayOfWeek.Wednesday:
                    return "Miercoles";
                case System.DayOfWeek.Thursday:
                    return "Jueves";
                case System.DayOfWeek.Friday:
                    return "Viernes";
                case System.DayOfWeek.Saturday:
                    return "Sábado";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Obtener último día del mes
        /// </summary>
        /// <param name="iMonthNumber"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static int GetUltimoDiaMes(int iMonthNumber, int year)
        {
            int iLastDay = 0;

            switch (iMonthNumber)
            {
                case 1:
                    iLastDay = 31;
                    break;
                case 2:
                    if (DateTime.IsLeapYear(year))
                        iLastDay = 29;
                    else
                        iLastDay = 28;
                    break;
                case 3:
                    iLastDay = 31;
                    break;
                case 4:
                    iLastDay = 30;
                    break;
                case 5:
                    iLastDay = 31;
                    break;
                case 6:
                    iLastDay = 30;
                    break;
                case 7:
                    iLastDay = 31;
                    break;
                case 8:
                    iLastDay = 31;
                    break;
                case 9:
                    iLastDay = 30;
                    break;
                case 10:
                    iLastDay = 31;
                    break;
                case 11:
                    iLastDay = 30;
                    break;
                case 12:
                    iLastDay = 31;
                    break;
            }

            return iLastDay;
        }

        /// <summary>
        /// Calcular dias de diferencia
        /// </summary>
        /// <param name="primerFecha"></param>
        /// <param name="segundaFecha"></param>
        /// <returns></returns>
        public static double CalcularDiasDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;
            return diferencia.Days;
        }

        /// <summary>
        /// Calcular horas de diferencia
        /// </summary>
        /// <param name="primerFecha"></param>
        /// <param name="segundaFecha"></param>
        /// <returns></returns>
        public static int CalcularHorasDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;
            return diferencia.Hours;
        }

        /// <summary>
        /// Calcular minutos de diferencia
        /// </summary>
        /// <param name="primerFecha"></param>
        /// <param name="segundaFecha"></param>
        /// <returns></returns>
        public static int CalcularMinutosDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;
            return diferencia.Minutes;
        }

        /// <summary>
        /// Listar 48 medias horas
        /// </summary>
        /// <param name="hora"></param>
        /// <returns></returns>
        public static string Listar48MediasHoras(int hora)
        {
            if (hora % 2 == 0)
            {
                return (Convert.ToString(hora / 2)).PadLeft(2, '0') + ":30";
            }
            else
            {
                return (Convert.ToString(hora / 2 + 1)).PadLeft(2, '0') + ":00";
            }
        }

        /// <summary>
        /// Permite obtener el valor actualizado de la variable
        /// </summary>
        /// <param name="reporte"></param>
        /// <param name="variable"></param>
        /// <param name="valor"></param>
        public static void ObtenerVariableReemplazo(InReporteDTO reporte, string variable, ref string valor)
        {
            string result = valor;
            if(reporte.IndicadorModificado == true)
            {
               InReporteVariableDTO entity = reporte.ListaVariables.Where(x => x.Invaridentificador == variable).FirstOrDefault();

                if(entity != null)
                {
                    if (!string.IsNullOrEmpty(entity.Inrevavalor))
                    {
                        result = entity.Inrevavalor;
                    }
                }
            }
            valor = result;
        }

    }
}
