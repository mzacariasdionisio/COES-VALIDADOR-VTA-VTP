using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Repositorio.Sic;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Factory;
using log4net;

namespace COES.Servicios.Aplicacion.Equipamiento
{
    public class EjecucionIndicadoresAppServicio
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EjecucionIndicadoresAppServicio));

        public EjecucionIndicadoresAppServicio()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Procesar()
        {
            DateTime fechaProceso = DateTime.Now.AddDays(-1);
            int GPS = 1;

            FactorySic.GetSiProcesoRepository().ExecIndicadores(fechaProceso, GPS);
        
        }
    }
}
