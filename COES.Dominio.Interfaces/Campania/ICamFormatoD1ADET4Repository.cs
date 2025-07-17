using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ADET4Repository
    {
        List<FormatoD1ADet4DTO> GetFormatoD1ADET4Codi(int proyCodi);
        bool SaveFormatoD1ADET4(FormatoD1ADet4DTO formatoD1ADET4DTO);
        bool DeleteFormatoD1ADET4ById(int id, string usuario);
        int GetLastFormatoD1ADET4Id();
        FormatoD1ADet4DTO GetFormatoD1ADET4ById(int id);
        //bool UpdateFormatoD1ADET4(FormatoD1ADet4DTO formatoD1ADET4DTO);

    }
}
