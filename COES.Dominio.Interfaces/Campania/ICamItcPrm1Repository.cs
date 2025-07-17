using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcPrm1Repository
    {
        List<ItcPrm1Dto> GetItcPrm1ByProyCodi(int proyCodi);
        bool SaveItcPrm1(ItcPrm1Dto ItcPrm1Dto);
        bool DeleteItcPrm1ById(int id, string usuario);
        int GetLastItcPrm1Id();
        List<ItcPrm1Dto> GetItcPrm1ById(int id);
        bool UpdateItcPrm1(ItcPrm1Dto ItcPrm1Dto);
        List<ItcPrm1Dto> GetItcPrm1ByFilter(string plancodi, string empresa, string estado);
    }
}
