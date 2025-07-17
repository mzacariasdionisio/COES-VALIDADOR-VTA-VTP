using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.PotenciaFirmeRemunerable;
using COES.WebService.PotenciaFirme.Contratos;

namespace COES.WebService.PotenciaFirme.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class PotenciaFirmeServicio : IPotenciaFirmeServicio
    {
        /// <summary>
        /// Ejecuta el proceso de costos marginales nodales
        /// </summary>
        /// <param name="fecha"></param>
        public int EjecutarPotenciaRemunerable(int pfpericodi, int pfrecacodi, int indrecacodiant, int recpotcodi, string usuario)
        {
            PotenciaFirmeRemunerableAppServicio cm = new PotenciaFirmeRemunerableAppServicio();

            int reporteCodiGenerado = cm.CalcularReportePFR(pfpericodi, pfrecacodi, indrecacodiant, recpotcodi, usuario);
            return reporteCodiGenerado;
        }
    }
}
