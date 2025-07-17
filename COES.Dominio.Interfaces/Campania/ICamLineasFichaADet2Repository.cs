using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamLineasFichaADet2Repository
    {
        List<LineasFichaADet2DTO> GetLineasFichaADet2Codi(int fichaACodi);
        bool SaveLineasFichaADet2(LineasFichaADet2DTO camLineasFichaADet2Dto);
        int GetLastLineasFichaADet2Codi();
        bool DeleteLineasFichaADet2ById(int id,  string usuario);
        LineasFichaADet2DTO GetLineasFichaADet2ById(int id);
    }
}
