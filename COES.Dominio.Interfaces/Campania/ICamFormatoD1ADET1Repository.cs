using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ADET1Repository
     {
        List<FormatoD1ADet1DTO> GetFormatoD1ADET1Codi(int proyCodi);
        bool SaveFormatoD1ADET1(FormatoD1ADet1DTO formatoD1ADET1DTO);
        bool DeleteFormatoD1ADET1ById(int id, string usuario);
        int GetLastFormatoD1ADET1Id();
        FormatoD1ADet1DTO GetFormatoD1ADET1ById(int id);
        bool UpdateFormatoD1ADET1(FormatoD1ADet1DTO formatoD1ADET1DTO);
    }
}
