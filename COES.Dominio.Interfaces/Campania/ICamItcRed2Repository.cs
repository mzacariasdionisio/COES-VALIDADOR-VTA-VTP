using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamItcRed2Repository
    {
        List<ItcRed2Dto> GetCamItcRed2ByProyCodi(int proyCodi);
        bool SaveCamItcRed2(ItcRed2Dto camItcRed2Dto);
        bool DeleteCamItcRed2ById(int id, string usuario);
        int GetLastCamItcRed2Id();
        List<ItcRed2Dto> GetCamItcRed2ById(int id);
        bool UpdateCamItcRed2(ItcRed2Dto camItcRed2Dto);
        List<ItcRed2Dto> GetCamItcRed2ByFilter(string plancodi, string empresa, string estado);
    }
}
