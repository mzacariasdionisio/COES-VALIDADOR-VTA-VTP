using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDERepository
    {
        List<CCGDEDTO> GetCamCCGDE(int id);
        bool SaveCamCCGDE(CCGDEDTO camCCGDEDto);
        bool UpdateCamCCGDE(CCGDEDTO camCCGDEDto);
        int GetLastCamCCGDECodi();
        bool DeleteCamCCGDEById(int id, string usuario);
        CCGDEDTO GetCamCCGDEById(int id);

        List<CCGDEDTO> GetCamCCGDEByFilter(string plancodi, string empresa, string estado);
    }
}
