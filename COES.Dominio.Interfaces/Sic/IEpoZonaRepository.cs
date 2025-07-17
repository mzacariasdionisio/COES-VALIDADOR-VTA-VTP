using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IEpoZonaRepository
    {

        List<EpoZonaDTO> List();

        EpoZonaDTO GetById(int ZonCodi);

        EpoZonaDTO GetByCriteria(int PuntCodi);
    }
}
