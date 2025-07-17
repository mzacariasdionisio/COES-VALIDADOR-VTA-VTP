using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDFRepository
    {
        List<CCGDFDTO> GetCamCCGDF(int id);
        bool SaveCamCCGDF(CCGDFDTO camCCGDFDto);
        bool UpdateCamCCGDF(CCGDFDTO camCCGDFDto);
        int GetLastCamCCGDFCodi();
        bool DeleteCamCCGDFById(int id, string usuario);
        List<CCGDFDTO> GetCamCCGDFById(int id);
        List<CCGDFDTO> GetCamCCGDFByFilter(string plancodi, string empresa, string estado);
    }
}
