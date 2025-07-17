using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ADET2Repository
    {
        List<FormatoD1ADet2DTO> GetFormatoD1ADET2Codi(int proyCodi);
        bool SaveFormatoD1ADET2(FormatoD1ADet2DTO formatoD1ADET2DTO);
        bool DeleteFormatoD1ADET2ById(int id, string usuario);
        int GetLastFormatoD1ADET2Id();
        FormatoD1ADet2DTO GetFormatoD1ADET2ById(int id);
        //bool UpdateFormatoD1ADET2(FormatoD1ADet2DTO formatoD1ADET2DTO);
    }
}
