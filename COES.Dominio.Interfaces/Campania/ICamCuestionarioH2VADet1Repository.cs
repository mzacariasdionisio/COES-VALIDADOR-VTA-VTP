using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VADet1Repository
    {
        List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1Codi(int h2vaCodi);
        bool SaveCuestionarioH2VADet1(CuestionarioH2VADet1DTO cuestionarioH2VADet1DTO);
        bool DeleteCuestionarioH2VADet1ById(int id, string usuario);
        int GetLastCuestionarioH2VADet1Id();
        List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1ById(int id);
    }
}