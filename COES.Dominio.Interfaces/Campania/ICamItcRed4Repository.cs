using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcRed4Repository
    {
        List<ItcRed4Dto> GetItcRed4ByProyCodi(int proyCodi);
        bool SaveItcRed4(ItcRed4Dto camItcRed4Dto);
        bool DeleteItcRed4ById(int id, string usuario);
        int GetLastItcRed4Id();
        List<ItcRed4Dto> GetItcRed4ById(int id);
        bool UpdateItcRed4(ItcRed4Dto camItcRed4Dto);
        List<ItcRed4Dto> GetItcRed4ByFilter(string plancodi, string empresa, string estado);
    }
}
