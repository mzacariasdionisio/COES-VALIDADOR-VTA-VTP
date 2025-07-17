using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamLineasFichaADet1Repository
    {
        List<LineasFichaADet1DTO> GetLineasFichaADet1Codi(int fichaACodi);
        bool SaveLineasFichaADet1(LineasFichaADet1DTO LineasFichaADet1Dto);
        int GetLastLineasFichaADet1Id();
        bool DeleteLineasFichaADet1ById(int id, string usuario);
        LineasFichaADet1DTO GetLineasFichaADet1ById(int id);
    }
}