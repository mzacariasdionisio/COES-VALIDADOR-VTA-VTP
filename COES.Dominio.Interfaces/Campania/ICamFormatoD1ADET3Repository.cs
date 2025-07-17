using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ADET3Repository
    {
        List<FormatoD1ADet3DTO> GetFormatoD1ADET3Codi(int proyCodi);
        bool SaveFormatoD1ADET3(FormatoD1ADet3DTO formatoD1ADET3DTO);
        bool DeleteFormatoD1ADET3ById(int id, string usuario);
        int GetLastFormatoD1ADET3Id();
        FormatoD1ADet3DTO GetFormatoD1ADET3ById(int id);
        //bool UpdateFormatoD1ADET3(FormatoD1ADet3DTO formatoD1ADET3DTO);

    }
}
