using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.General;
using COES.WebService.Osinergmin.Contratos;

namespace COES.WebService.Osinergmin.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class OsinergminServicio : IOsinergminServicio
    {
        public List<CombustibleDTO> ListCombustibles()
        {
            return (new PrCombustibleAppServicio()).ListPrCombustibles();
        }


        public List<EqEquipoDTO> ListadoCentralesOsinergmin()
        {
            return (new EquipamientoAppServicio()).ListadoCentralesOsinergmin();
        }


        public List<GrupoGeneracionDTO> ListarGeneradoresDespachoOsinergmin()
        {
            return (new DespachoAppServicio()).ListarGeneradoresDespachoOsinergmin();
        }


        public List<SiFuenteenergiaDTO> ListarFuenteenergias()
        {
            return (new GeneralAppServicio()).ListSiFuenteenergias();
        }


        public List<PrGrupoDTO> ListaModosOperacionActivos()
        {
            return (new DespachoAppServicio()).ListaModosOperacionActivos();
        }
    }
}
