using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcRed3Repository
    {
        List<ItcRed3Dto> GetItcRed3ByProyCodi(int proyCodi);
        bool SaveItcRed3(ItcRed3Dto camItcRed3Dto);
        bool DeleteItcRed3ById(int id, string usuario);
        int GetLastItcRed3Id();
        List<ItcRed3Dto> GetItcRed3ById(int id);
        bool UpdateItcRed3(ItcRed3Dto camItcRed3Dto);
        List<ItcRed3Dto> GetItcRed3ByFilter(string plancodi, string empresa, string estado);
    }
}
