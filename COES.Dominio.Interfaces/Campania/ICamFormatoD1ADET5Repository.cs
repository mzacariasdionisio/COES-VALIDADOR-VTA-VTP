using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ADET5Repository
    {
        List<FormatoD1ADet5DTO> GetFormatoD1ADET5Codi(int proyCodi);
        bool SaveFormatoD1ADET5(FormatoD1ADet5DTO formatoD1ADET5DTO);
        bool DeleteFormatoD1ADET5ById(int id, string usuario);
        int GetLastFormatoD1ADET5Id();
        FormatoD1ADet5DTO GetFormatoD1ADET5ById(int id);
        //bool UpdateFormatoD1ADET5(FormatoD1ADet5DTO formatoD1ADET5DTO);

    }
}
