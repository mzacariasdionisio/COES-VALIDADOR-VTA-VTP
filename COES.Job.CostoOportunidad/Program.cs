using COES.Job.CostoOportunidad.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Job.CostoOportunidad
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient cliente = new ServiceReferenceCostoOportunidad.CostoOportunidadServicioClient())
            {
                EventLogger logger = new EventLogger();
                try
                {
                    cliente.CalculoFactoresUtilizacion();
                    logger.Info("Proceso ejecutado correctamente");

                }
                catch (Exception ex)
                {
                    logger.Error("Error", ex);
                }
            }
        }
    }
}
