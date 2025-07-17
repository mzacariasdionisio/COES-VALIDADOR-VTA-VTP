using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1CRepository
    {
        List<FormatoD1CDTO> GetFormatoD1CCodi(int proyCodi);
        bool SaveFormatoD1C(FormatoD1CDTO formatoD1CDTO);
        bool DeleteFormatoD1CById(int id, string usuario);
        int GetLastFormatoD1CId();
        FormatoD1CDTO GetFormatoD1CById(int id);
        //bool UpdateFormatoD1C(FormatoD1CDTO formatoD1CDTO);
        List<FormatoD1CDTO> GetFormatoD1CByFilter(string plancodi, string empresa, string estado);
    }
}
