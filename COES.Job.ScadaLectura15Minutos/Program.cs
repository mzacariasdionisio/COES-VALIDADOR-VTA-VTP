using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using static log4net.Appender.RollingFileAppender;

namespace COES.Job.ScadaLectura15Minutos
{
    internal class Program
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("COES.Job.ScadaLectura15Minutos");
        private static ScadaSp7AppServicio ScadaSp7AppServicio = new ScadaSp7AppServicio();//SICOES
        private static TiempoRealAppServicio tiempoRealAppServicio = new TiempoRealAppServicio();//SCADA

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
            DateTime fecha = DateTime.Now;
            fecha = fecha.AddSeconds(-fecha.Second);
            fecha = fecha.AddMinutes(-5);
            string bloque = getBloque(fecha);
            int bloqueNumerico = getBloqueValorNumerico(fecha);
            List<TrCircularSp7DTO> trCircularSp7DTOs;
            if (fecha.Hour == 0 && fecha.Minute == 0)
            {
                trCircularSp7DTOs = getData(fecha.AddSeconds(-1));
            }
            else
            {
                trCircularSp7DTOs = getData(fecha);
            }

            log.Debug("JOB TRABAJARÁ CON LA FECHA: " + fecha);
            log.Debug("TOTAL DE REGISTROS (B:" + bloque + "): " + trCircularSp7DTOs.Count);

            Parallel.ForEach(trCircularSp7DTOs, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (trCircular, state) =>
            {
                bool existe = ScadaSp7AppServicio.GetSiExisteRegistroPorFechaYCanal(trCircular.Canalcodi, fecha);
                if (existe)
                {
                    try
                    {
                        ScadaSp7AppServicio.ActualizarRegistroPorFechaYBloque(trCircular.Canalcodi, fecha, bloque, "JOBLECTURA15MINUTOS", (decimal)trCircular.Canalvalor);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                }
                else
                {
                    try
                    {
                        ScadaSp7AppServicio.AgregarRegistroPorFechaYBloque(trCircular.Canalcodi, fecha, bloque, "JOBLECTURA15MINUTOS", (decimal)trCircular.Canalvalor);
                    }
                    catch (Exception e)
                    {
                        exceptions.Enqueue(e);
                    }
                }
            });

            if (!exceptions.IsEmpty)
            {
                log.Debug(exceptions);
            }

            /*
            for (int cnt = 1; cnt <= bloqueNumerico; cnt++)
            {
                ScadaSp7AppServicio.ActualizarRegistrosNulosPorFechaYBloque(fecha, ("H" + cnt));
            }
            */
        }

        static void reproceso()
        {
            log4net.Config.XmlConfigurator.Configure();
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
            DateTime startDate = DateTime.Parse("2025-04-02");
            List<DateTime> dateTimes = new List<DateTime>();

            for (int i = 0; i < (96*3); i++) // 96 intervals of 15 minutes in a day
            {
                dateTimes.Add(startDate.AddMinutes(i * 15));
            }


            foreach (var fecha in dateTimes)
            {
                string bloque = getBloque(fecha);
                int bloqueNumerico = getBloqueValorNumerico(fecha);
                List<TrCircularSp7DTO> trCircularSp7DTOs;
                if (fecha.Hour == 0 && fecha.Minute == 0)
                {
                    trCircularSp7DTOs = getData(fecha.AddSeconds(-1));
                }
                else
                {
                    trCircularSp7DTOs = getData(fecha);
                }

                log.Debug("JOB TRABAJARÁ CON LA FECHA: " + fecha);
                log.Debug("TOTAL DE REGISTROS (B:" + bloque + "): " + trCircularSp7DTOs.Count);

                Parallel.ForEach(trCircularSp7DTOs, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (trCircular, state) =>
                {
                    bool existe = ScadaSp7AppServicio.GetSiExisteRegistroPorFechaYCanal(trCircular.Canalcodi, fecha);
                    if (existe)
                    {
                        try
                        {
                            ScadaSp7AppServicio.ActualizarRegistroPorFechaYBloque(trCircular.Canalcodi, fecha, bloque, "JOBLECTURA15MINUTOS", (decimal)trCircular.Canalvalor);
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                    }
                    else
                    {
                        try
                        {
                            ScadaSp7AppServicio.AgregarRegistroPorFechaYBloque(trCircular.Canalcodi, fecha, bloque, "JOBLECTURA15MINUTOS", (decimal)trCircular.Canalvalor);
                        }
                        catch (Exception e)
                        {
                            exceptions.Enqueue(e);
                        }
                    }
                });

                if (!exceptions.IsEmpty)
                {
                    log.Debug(exceptions);
                }
            }
        }

        static protected List<TrCircularSp7DTO> getData(DateTime fecha) 
        {
            return tiempoRealAppServicio.ObtenerCircularesPorFecha(fecha);
        }

        static protected string getBloque(DateTime fecha)
        {
            return "H" + getBloqueValorNumerico(fecha);
        }

        static protected int getBloqueValorNumerico(DateTime fecha)
        {
            int bloque = 0;

            if (fecha.Hour == 0 && fecha.Minute == 0)
            {
                bloque = 96;
                return bloque;
            }


            return bloque + (4 * fecha.Hour + fecha.Minute / 15);
        }

    }
}
