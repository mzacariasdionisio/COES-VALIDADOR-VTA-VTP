using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcRed1Repository
    {
        List<ItcRed1Dto> GetItcRed1ByProyCodi(int proyCodi);
        bool SaveItcRed1(ItcRed1Dto camItcRed1Dto);
        bool DeleteItcRed1ById(int id, string usuario);
        int GetLastItcRed1Id();
        List<ItcRed1Dto> GetItcRed1ById(int id);
        bool UpdateItcRed1(ItcRed1Dto camItcRed1Dto);
        List<ItcRed1Dto> GetItcRed1ByFilter(string plancodi, string empresa, string estado);
    }
}
