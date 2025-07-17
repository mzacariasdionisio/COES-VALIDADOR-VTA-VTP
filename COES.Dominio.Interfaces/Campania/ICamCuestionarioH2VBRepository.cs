using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VBRepository
    {
        List<CuestionarioH2VBDTO> GetCuestionarioH2VBCodi(int proyCodi);
        bool SaveCuestionarioH2VB(CuestionarioH2VBDTO cuestionarioH2VBDTO);
        bool DeleteCuestionarioH2VBById(int id, string usuario);
        int GetLastCuestionarioH2VBId();
        CuestionarioH2VBDTO GetCuestionarioH2VBById(int id);

        List<CuestionarioH2VBDTO> GetFormatoH2VBByFilter(string plancodi, string empresa, string estado);
    }
}