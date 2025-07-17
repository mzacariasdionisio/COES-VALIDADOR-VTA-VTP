using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndCapacidadTransporteRepository
    {
        int Save(IndCapacidadTransporteDTO entity);
        List<IndCapacidadTransporteDTO> ListCapacidadTransporte(int emprcodi, int ipericodi);
    }
}
