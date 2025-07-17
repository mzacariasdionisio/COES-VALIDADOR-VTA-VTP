using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaCCTTCRepository
    {

        List<RegHojaCCTTCDTO> GetRegHojaCCTTCProyCodi( int proyCodi);
        
        bool SaveRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO);

        bool DeleteRegHojaCCTTCById(int id, string usuario);

        int GetLastRegHojaCCTTCId();

        RegHojaCCTTCDTO GetRegHojaCCTTCById(int id);

        bool UpdateRegHojaCCTTC(RegHojaCCTTCDTO regHojaCCTTCDTO);

        List<Det1RegHojaCCTTCDTO> GetRegHojaCCTTCByFilter(string plancodi, string empresa, string estado);
        
        List<Det2RegHojaCCTTCDTO> GetRegHojaCCTTC2ByFilter(string plancodi, string empresa, string estado);
    }
}
