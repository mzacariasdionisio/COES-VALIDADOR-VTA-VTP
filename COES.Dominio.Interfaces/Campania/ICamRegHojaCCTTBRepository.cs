using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaCCTTBRepository
    {
        List<RegHojaCCTTBDTO> GetRegHojaCCTTBProyCodi(int proyCodi);

        bool SaveRegHojaCCTTB(RegHojaCCTTBDTO regHojaCCTTBDTO);

        bool DeleteRegHojaCCTTBById(int id, string usuario);

        int GetLastRegHojaCCTTBId();

        RegHojaCCTTBDTO GetRegHojaCCTTBById(int id);

        bool UpdateRegHojaCCTTB(RegHojaCCTTBDTO regHojaCCTTBDTO);

        List<RegHojaCCTTBDTO> GetRegHojaCCTTBByFilter(string plancodi, string empresa, string estado);


    }
    
}
