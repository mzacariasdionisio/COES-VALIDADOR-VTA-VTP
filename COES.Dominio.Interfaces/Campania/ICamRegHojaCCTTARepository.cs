using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Campania;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRegHojaCCTTARepository
    {

        List<RegHojaCCTTADTO> GetRegHojaCCTTAProyCodi( int proyCodi);
        
        bool SaveRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO);

        bool DeleteRegHojaCCTTAById(int id, string usuario);

        int GetLastRegHojaCCTTAId();

        RegHojaCCTTADTO GetRegHojaCCTTAById(int id);

        bool UpdateRegHojaCCTTA(RegHojaCCTTADTO regHojaCCTTADTO);

        List<RegHojaCCTTADTO> GetRegHojaCCTTAByFilter(string plancodi, string empresa, string estado);


    }
}
