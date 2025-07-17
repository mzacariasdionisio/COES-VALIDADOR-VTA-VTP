using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamSolHojaBRepository
    {

        List<SolHojaBDTO> GetSolHojaBProyCodi(int proyCodi);

        bool SaveSolHojaB(SolHojaBDTO SolHojaBDTO);

        bool DeleteSolHojaBById(int id, string usuario);

        int GetLastSolHojaBId();

        SolHojaBDTO GetSolHojaBById(int id);

        bool UpdateSolHojaB(SolHojaBDTO SolHojaBDTO);

        List<SolHojaBDTO> GetSolHojaBByFilter(string plancodi, string empresa, string estado);
    }
}
