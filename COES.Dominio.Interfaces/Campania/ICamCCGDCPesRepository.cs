using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDCPesRepository
    {

        List<CCGDCPesDTO> GetCamCCGDC(int proyCodi);
        bool SaveCamCCGDC(CCGDCPesDTO camCCGDCDto);
        bool UpdateCamCCGDC(CCGDCPesDTO camCCGDCDto);
        int GetLastCamCCGDCCodi();
        bool DeleteCamCCGDCById(int id, string usuario);
        List<CCGDCPesDTO> GetCamCCGDCById(int id);
        List<CCGDCPesDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado);
    }
}
