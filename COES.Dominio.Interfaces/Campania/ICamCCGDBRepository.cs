using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCCGDBRepository
    {
        List<CCGDBDTO> GetCamCCGDB(int proyCodi);
        bool SaveCamCCGDB(CCGDBDTO camCCGDBDto);
        bool UpdateCamCCGDB(CCGDBDTO camCCGDBDto);
        int GetLastCamCCGDBCodi();
        bool DeleteCamCCGDBById(int id, string usuario);
        List<CCGDBDTO> GetCamCCGDBById(int id);

        List<CCGDBDTO> GetCamCCGDBByFilter(string plancodi, string empresa, string estado);
        List<CCGDCDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado);
    }
}
