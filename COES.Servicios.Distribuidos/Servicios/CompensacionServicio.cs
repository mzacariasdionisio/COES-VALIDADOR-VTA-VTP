using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Distribuidos.Contratos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace COES.Servicios.Distribuidos.Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
    public class CompensacionServicio:ICompensacionServicio
    {
        DespachoAppServicio servicio = new DespachoAppServicio();
        public List<ModoOperacionParametrosDTO> ListarModosOperacionParametros(int iGrupoCodi)
        {
            return servicio.ListarModosOperacionParametros(iGrupoCodi);
        }

        public List<ModoOperacionCostosDTO> ListarModosOperacionCostos()
        {
            throw new NotImplementedException();
        }
    }
}