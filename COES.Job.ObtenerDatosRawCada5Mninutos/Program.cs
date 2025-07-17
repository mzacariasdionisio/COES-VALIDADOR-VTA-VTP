using COES.Servicios.Aplicacion.DPODemanda;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Job.ObtenerDatosRawCada5Mninutos
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            try
            {
                (new DemandaPOAppServicio()).ObtenerDatosRawCada5Mninutos();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                log.Error(ex);
            }
        }
    }
}
