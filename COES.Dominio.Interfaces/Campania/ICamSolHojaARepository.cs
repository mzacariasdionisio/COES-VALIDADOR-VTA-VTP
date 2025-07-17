using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSolHojaARepository
    {

        List<SolHojaADTO> GetSolHojaAProyCodi(int proyCodi);

        bool SaveSolHojaA(SolHojaADTO SolHojaADTO);

        bool DeleteSolHojaAById(int id, string usuario);

        int GetLastSolHojaAId();

        SolHojaADTO GetSolHojaAById(int id);

        bool UpdateSolHojaA(SolHojaADTO SolHojaADTO);

        List<SolHojaADTO> GetSolHojaAByFilter(string plancodi, string empresa, string estado);
    }
}
