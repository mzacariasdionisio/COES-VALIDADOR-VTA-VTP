using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamFormatoD1DRepository
    {
        List<FormatoD1DDTO> GetFormatoD1DCodi(int proyCodi);
        bool SaveFormatoD1D(FormatoD1DDTO formatoD1DDTO);
        bool DeleteFormatoD1DById(int id, string usuario);
        int GetLastFormatoD1DId();
        List<FormatoD1DDTO> GetFormatoD1DById(int id);

        //FormatoD1CDTO GetFormatoD1CById(int id);
        //bool UpdateFormatoD1C(FormatoD1CDTO formatoD1CDTO);
        List<FormatoD1DDTO> GetFormatoD1DByFilter(string plancodi, string empresa, string estado);

    }
}
