using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndCapacidadDiaRepository
    {
        int Save(IndCapacidadDiaDTO entity);
        void UpdateValueByIdByDate(IndCapacidadDiaDTO entity);
        List<IndCapacidadDiaDTO> ListCapacidadDiaByCapacidad(int indcpccodi);
        List<IndCapacidadDiaDTO> ListCapacidadDiaJoinCapacidad(int indcbrcodi);
    }
}
