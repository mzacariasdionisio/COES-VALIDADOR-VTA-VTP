using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VCRepository
    {
        List<CuestionarioH2VCDTO> GetCuestionarioH2VCCodi(int proyCodi);
        bool SaveCuestionarioH2VC(CuestionarioH2VCDTO cuestionarioH2VCDTO);
        bool DeleteCuestionarioH2VCById(int id, string usuario);
        int GetLastCuestionarioH2VCId();
        List<CuestionarioH2VCDTO> GetCuestionarioH2VCById(int id);

        List<CuestionarioH2VCDTO> GetFormatoH2VCByFilter(string plancodi, string empresa, string estado);
    }
}