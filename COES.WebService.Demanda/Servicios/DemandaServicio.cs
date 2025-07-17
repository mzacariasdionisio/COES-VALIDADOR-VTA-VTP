using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using COES.Servicios.Aplicacion.General;
using COES.WebService.Demanda.Contratos;

namespace COES.WebService.Demanda.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class DemandaServicio:IDemandaServicio
    {
        private PortalAppServicio portalServicio;
        public DemandaServicio ()
        {
            portalServicio= new PortalAppServicio();
        }

        public List<Dominio.DTO.Sic.MeMedicion48DTO> ObtenerDatosDemanda(DateTime fechaInicio, DateTime fechaFin, int lectoCodi)
        {
            return portalServicio.ObtenerDatosDemandaPortal(fechaInicio, fechaFin, lectoCodi);
        }

        public Dominio.DTO.Sic.SupDemandaGrafico ObtenerDatosGrafico(int tipo)
        {
            return (new COES.Servicios.Aplicacion.Coordinacion.SupervisionDemandaAppServicio()).ObtenerDatosGrafico(tipo);
        }
    }
}