using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSolHojaCRepository
    {

        List<SolHojaCDTO> GetSolHojaCProyCodi(int proyCodi);

        bool SaveSolHojaC(SolHojaCDTO SolHojaCDTO);

        bool DeleteSolHojaCById(int id, string usuario);

        int GetLastSolHojaCId();

        SolHojaCDTO GetSolHojaCById(int id);

        bool UpdateSolHojaC(SolHojaCDTO SolHojaCDTO);

        List<DetSolHojaCDTO> GetSolHojaCByFilter(string plancodi, string empresa, string estado);
    }
}
