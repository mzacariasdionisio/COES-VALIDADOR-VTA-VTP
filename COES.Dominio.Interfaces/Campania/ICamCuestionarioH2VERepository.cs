using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VERepository
    {
        List<CuestionarioH2VEDTO> GetCuestionarioH2VECodi(int proyCodi);
        bool SaveCuestionarioH2VE(CuestionarioH2VEDTO cuestionarioH2VEDTO);
        bool DeleteCuestionarioH2VEById(int id, string usuario);
        int GetLastCuestionarioH2VEId();
        List<CuestionarioH2VEDTO> GetCuestionarioH2VEById(int id);

        List<CuestionarioH2VEDTO> GetFormatoH2VEByFilter(string plancodi, string empresa, string estado);
    }
}