using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.TransfSeniales;
using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Job.ScadaMuestraInstantanea
{
    internal class Program
    {
        private static readonly ILog log = log4net.LogManager.GetLogger("COES.Job.ScadaMuestraInstantanea");
        private static TransfSenialesAppServicio transfSenialesAppServicio = new TransfSenialesAppServicio();//SICOES
        private static TiempoRealAppServicio tiempoRealAppServicio = new TiempoRealAppServicio();//SCADA

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            ConcurrentQueue<Exception> exceptions = new ConcurrentQueue<Exception>();
            DateTime fecha = DateTime.Now;
            fecha = fecha.AddSeconds(-fecha.Second);
            fecha = fecha.AddMinutes(-5);
            List<int> canalesCodigos = getCanalesCodigos(fecha);
            log.Debug("JOB TRABAJARÁ CON LA FECHA: " + fecha);
            log.Debug("TOTAL CANALES A PROCESAR: " + canalesCodigos.Count);

            
            Parallel.ForEach(canalesCodigos, new ParallelOptions { MaxDegreeOfParallelism = 500 }, (canalcodi, state) =>
            {
                try
                {

                    List<TrCircularSp7DTO> trCircularSp7DTOs = tiempoRealAppServicio.ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea(canalcodi, fecha.AddMinutes(-15), fecha);

                    foreach (TrCircularSp7DTO circular in trCircularSp7DTOs)
                    {
                        if (circular.Canalfhsist.Minute % 5 == 0 && circular.Canalfhsist.Second == 0 && circular.Canalfhsist.Millisecond == 0)
                        {
                            continue;
                        }

                        TrCanalSp7DTO canal = tiempoRealAppServicio.GetCanalById(canalcodi);
                        tiempoRealAppServicio.GenerarEliminarMuestraInstantanea(canal.Canalcodi);
                        tiempoRealAppServicio.GenerarRegistroMuestraInstantanea(canal, circular, "JOBSCADAMUESTRAINSTANTANEA");

                        break;
                    }
                }
                catch (Exception e)
                {
                    exceptions.Enqueue(e);
                }
            });
            

            if (!exceptions.IsEmpty)
            {
                log.Debug(exceptions);
            }
        }

        static protected List<int> getCanalesCodigos(DateTime fecha)
        {
            return tiempoRealAppServicio.ObtenerCodigosDeCanalesParaMuestraInstantanea();
        }

    }
}