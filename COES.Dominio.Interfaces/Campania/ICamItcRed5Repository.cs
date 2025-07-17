using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcRed5Repository
    {
        List<ItcRed5Dto> GetItcRed5ByProyCodi(int proyCodi);
        bool SaveItcRed5(ItcRed5Dto camItcRed5Dto);
        bool DeleteItcRed5ById(int id, string usuario);
        int GetLastItcRed5Id();
        List<ItcRed5Dto> GetItcRed5ById(int id);
        bool UpdateItcRed5(ItcRed5Dto camItcRed5Dto);
        List<ItcRed5Dto> GetItcRed5ByFilter(string plancodi, string empresa, string estado);
    }
}
