using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcPrm2Repository
    {
        List<ItcPrm2Dto> GetItcPrm2Codi(int proyCodi);
        bool SaveItcPrm2(ItcPrm2Dto camItcPrm2Dto);
        bool DeleteItcPrm2ById(int id, string usuario);
        int GetLastItcPrm2Id();
        List<ItcPrm2Dto> GetItcPrm2ById(int id);
        bool UpdateItcPrm2(ItcPrm2Dto camItcPrm2Dto);

        List<ItcPrm2Dto> GetItcPrm2ByFilter(string plancodi, string empresa, string estado);
    }
}
