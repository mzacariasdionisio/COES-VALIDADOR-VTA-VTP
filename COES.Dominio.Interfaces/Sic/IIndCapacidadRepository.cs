using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndCapacidadRepository
    {
        int Save(IndCapacidadDTO entidad);
        void UpdateDateById(IndCapacidadDTO entity);
        List<IndCapacidadDTO> ListCapacidadByCabecera(int indcbrcodi);
        List<IndCapacidadDTO> ListCapacidadJoinCabecera(int emprcodi, int ipericodi, int indcbrtipo);
        List<IndCapacidadDTO> ListByCriteria(int ipericodi, string emprcodi, string indcbrtipo, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi, string indcpctipo);
    }
}
