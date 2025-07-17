using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDARepository
    {
        List<CCGDADTO> GetCamCCGDA(int id);
        bool SaveCamCCGDA(CCGDADTO camCCGDADto);
        bool DeleteCamCCGDAById(int id, string usuario);
        int GetLastCamCCGDAId();
        CCGDADTO GetCamCCGDAById(int id);

        List<CCGDADTO> GetCamCCGDAByFilter(string plancodi, string empresa, string estado);
        bool UpdateCamCCGDA(CCGDADTO camCCGDADto);
    }

}
