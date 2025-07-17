using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndCabeceraRepository
    {
        int Save(IndCabeceraDTO entidad);
        List<IndCabeceraDTO> ListCabecera(int emprcodi, int ipericodi);
    }
}
