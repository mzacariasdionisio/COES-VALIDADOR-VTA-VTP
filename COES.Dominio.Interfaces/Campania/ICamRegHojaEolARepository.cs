using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaEolARepository
    {
        List<RegHojaEolADTO> GetRegHojaEolACodi(int id);

        bool SaveRegHojaEolA(RegHojaEolADTO regHojaEolADTO);

        bool DeleteRegHojaEolAById(int id, string usuario);

        int GetLastRegHojaEolAId();

        RegHojaEolADTO GetRegHojaEolAById(int id);

        bool UpdateRegHojaEolA(RegHojaEolADTO regHojaEolADTO);

        List<RegHojaEolADTO> GetRegHojaEolAByFilter(string plancodi, string empresa, string estado);
    }
}
