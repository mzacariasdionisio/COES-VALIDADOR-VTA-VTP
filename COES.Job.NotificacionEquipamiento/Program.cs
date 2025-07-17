using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using log4net;

namespace COES.Job.NotificacionEquipamiento
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        private static readonly TareaProgramadaAppServicio logic = new TareaProgramadaAppServicio();
        static void Main(string[] args)
        {
            try
            {
                EjecutarProcesoTarea();
            }
            catch (Exception ex)
            {
               log.Error("Error: ", ex);
            }
        }

        private static void EjecutarProcesoTarea()
        {
            log.Info("Inicio Proceso Tarea");
            List<SiProcesoDTO> listTask = logic.ObtenerTareasProgramadas(DateTime.Now);
            foreach (SiProcesoDTO item in listTask)
            {
                switch (item.Prcsmetodo)
                {
                    case "NotificacionEquipamiento":
                        {
                            log.Info("NotificacionEquipamiento");
                            NotificacionCambioEquipos servicio = new NotificacionCambioEquipos();
                            servicio.Procesar(5);
                            break;
                        }
                }
            }
            log.Info("Fin Proceso Tarea");
        }
    }
}
