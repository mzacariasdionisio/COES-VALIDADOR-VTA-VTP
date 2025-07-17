using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1BRepository
    {
        List<FormatoD1BDTO> GetFormatoD1BCodi(int proyCodi);
        bool SaveFormatoD1B(FormatoD1BDTO formatoD1BDTO);
        bool DeleteFormatoD1BById(int id, string usuario);
        int GetLastFormatoD1BId();
        FormatoD1BDTO GetFormatoD1BById(int id);
        //bool UpdateFormatoD1B(FormatoD1BDTO formatoD1BDTO);
        List<FormatoD1BDTO> GetFormatoD1BByFilter(string plancodi, string empresa, string estado);
    }
}
