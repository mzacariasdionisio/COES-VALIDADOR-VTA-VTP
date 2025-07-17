using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VFRepository
    {
        List<CuestionarioH2VFDTO> GetCuestionarioH2VFCodi(int proyCodi);
        bool SaveCuestionarioH2VF(CuestionarioH2VFDTO cuestionarioH2VFDTO);
        bool DeleteCuestionarioH2VFById(int id, string usuario);
        int GetLastCuestionarioH2VFId();
        CuestionarioH2VFDTO GetCuestionarioH2VFById(int id);

        List<CuestionarioH2VFDTO> GetFormatoH2VFByFilter(string plancodi, string empresa, string estado);
    }
}