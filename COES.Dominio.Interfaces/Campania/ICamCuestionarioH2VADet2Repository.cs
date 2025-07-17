using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VADet2Repository
    {
        List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2Codi(int h2vaCodi);
        bool SaveCuestionarioH2VADet2(CuestionarioH2VADet2DTO cuestionarioH2VADet2DTO);
        bool DeleteCuestionarioH2VADet2ById(int id, string usuario);
        int GetLastCuestionarioH2VADet2Id();
        List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2ById(int id);
    }
}