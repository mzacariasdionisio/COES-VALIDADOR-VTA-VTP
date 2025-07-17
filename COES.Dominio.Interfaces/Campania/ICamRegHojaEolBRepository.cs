using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaEolBRepository
    {
        List<RegHojaEolBDTO> GetRegHojaEolBCodi(int id);

        bool SaveRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO);

        bool DeleteRegHojaEolBById(int id, string usuario);

        int GetLastRegHojaEolBId();

        RegHojaEolBDTO GetRegHojaEolBById(int id);

        bool UpdateRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO);

        List<RegHojaEolBDTO> GetRegHojaEolBByFilter(string plancodi, string empresa, string estado);
    }
}
