using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaEolCRepository
    {
        List<RegHojaEolCDTO> GetRegHojaEolCCodi(int proyCodi);

        bool SaveRegHojaEolC(RegHojaEolCDTO RegHojaEolCDTO);

        bool DeleteRegHojaEolCById(int id, string usuario);

        int GetLastRegHojaEolCId();

        RegHojaEolCDTO GetRegHojaEolCById(int id);

        bool UpdateRegHojaEolC(RegHojaEolCDTO RegHojaEolCDTO);

        List<DetRegHojaEolCDTO> GetRegHojaEolCByFilter(string plancodi, string empresa, string estado);
    }
}
