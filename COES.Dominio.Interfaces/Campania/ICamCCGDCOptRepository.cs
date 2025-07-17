using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDCOptRepository
    {
        List<CCGDCOptDTO> GetCamCCGDC(int proyCodi);
        bool SaveCamCCGDC(CCGDCOptDTO camCCGDCDto);
        bool UpdateCamCCGDC(CCGDCOptDTO camCCGDCDto);
        int GetLastCamCCGDCCodi();
        bool DeleteCamCCGDCById(int id, string usuario);
        List<CCGDCOptDTO> GetCamCCGDCById(int id);
        List<CCGDCOptDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado);
    }
}
