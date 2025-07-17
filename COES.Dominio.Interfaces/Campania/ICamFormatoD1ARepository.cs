using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1ARepository
    {

        List<FormatoD1ADTO> GetFormatoD1ACodi(int proyCodi);
        bool SaveFormatoD1A(FormatoD1ADTO formatoD1ADTO);
        bool DeleteFormatoD1AById(int id, string usuario);
        int GetLastFormatoD1AId();
        FormatoD1ADTO GetFormatoD1AById(int id);

        List<FormatoD1ADTO> GetFormatoD1AByFilter(string plancodi, string empresa, string estado);
    }
}
