using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDDRepository
    {
        List<CCGDDDTO> GetCamCCGDD(int id);
        bool SaveCamCCGDD(CCGDDDTO camCCGDDDto);
        bool UpdateCamCCGDD(CCGDDDTO camCCGDDDto);
        int GetLastCamCCGDDCodi();
        bool DeleteCamCCGDDById(int id, string usuario);
        List<CCGDDDTO> GetCamCCGDDById(int id);
        List<CCGDDDTO> GetCamCCGDDByFilter(string plancodi, string empresa, string estado);
    }
}
