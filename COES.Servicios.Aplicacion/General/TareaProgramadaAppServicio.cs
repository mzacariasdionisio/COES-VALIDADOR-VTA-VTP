using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.General
{
    public class TareaProgramadaAppServicio
    {
        /// <summary>
        /// Permite listar las tareas programdas
        /// </summary>
        /// <returns></returns>
        public List<SiProcesoDTO> ObtenerTareasProgramadas(DateTime fechaActual)
        {
            List<SiProcesoDTO> list = FactorySic.GetSiProcesoRepository().List();
            List<SiProcesoDTO> result = ListarProcesoXFechaHora(list, fechaActual, fechaActual.Hour, fechaActual.Minute);

            return result;
        }

        /// <summary>
        /// Permite listar las tareas programadas por bloque
        /// </summary>
        /// <returns></returns>
        public List<SiProcesoDTO> ObtenerTareasProgramadasXBloque(DateTime fechaActual, int hora, int minuto, int bloque)
        {
            List<SiProcesoDTO> list = FactorySic.GetSiProcesoRepository().List();
            list = list.Where(x => x.Prscbloque == bloque).ToList();
            List<SiProcesoDTO> result = ListarProcesoXFechaHora(list, fechaActual, hora, minuto);

            return result;
        }

        private List<SiProcesoDTO> ListarProcesoXFechaHora(List<SiProcesoDTO> list, DateTime fechaActual, int hora, int minuto)
        {
            List<SiProcesoDTO> result = new List<SiProcesoDTO>();

            foreach (SiProcesoDTO item in list)
            {
                if (item.Prscfrecuencia == FrecuenciaTarea.Diaria)
                {
                    if (item.Prschorainicio == hora && item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Hora)
                {
                    if (item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada5Minutos)
                {
                    if (minuto % 5 == 0)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada10Minutos)
                {
                    if (minuto % 10 == 0)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada15Minutos)
                {
                    if (minuto % 15 == 0)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada30Minutos)
                {
                    if (item.Prscminutoinicio == 0)
                    {
                        if (minuto % 30 == 0)
                            result.Add(item);
                    }
                    else
                    {
                        int delay = item.Prscminutoinicio ?? 0;

                        DateTime fechaAProcesar = DateTime.Today.AddHours(hora).AddMinutes(minuto);
                        fechaAProcesar = fechaAProcesar.AddMinutes(delay * -1);

                        if ((fechaAProcesar.Minute) % 30 == 0)
                            result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada03Horas)
                {
                    if (minuto == 0 && hora % 3 == 0)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada4DelMes)
                {
                    int day = DateTime.Now.Day;
                    if (day == 4 && item.Prschorainicio == hora && item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada21DelMes)
                {
                    int day = DateTime.Now.Day;
                    if (day == 21 && item.Prschorainicio == hora && item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == FrecuenciaTarea.Cada31DicDelAnio)
                {
                    int mes = DateTime.Now.Month;
                    int day = DateTime.Now.Day;
                    if (mes == 12 && day == 31 && item.Prschorainicio == hora && item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }
                else if (item.Prscfrecuencia == "DIA_" + EPDate.f_NombreDiaSemanaCorto(fechaActual.DayOfWeek))
                {
                    if (item.Prschorainicio == hora && item.Prscminutoinicio == minuto)
                    {
                        result.Add(item);
                    }
                }

            }

            return result;
        }
    }

    /// <summary>
    /// Frecuencias de ejecución de tareas
    /// </summary>
    public class FrecuenciaTarea
    {
        public const string Diaria = "D";
        public const string Hora = "HJ";
        public const string Cada5Minutos = "M5";
        public const string Cada10Minutos = "M10";
        public const string Cada30Minutos = "M30";
        public const string Cada15Minutos = "M15";
        public const string Cada03Horas = "H3";
        public const string Cada4DelMes = "ME4";
        public const string Diaria3 = "D3";
        public const string Cada21DelMes = "ME21";
        public const string Cada31DicDelAnio = "DIC31";
    }
}
