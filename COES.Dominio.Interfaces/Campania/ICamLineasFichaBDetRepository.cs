using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamLineasFichaBDetRepository
    {
        List<LineasFichaBDetDTO> GetLineasFichaBDet(int fichaBCodi);
        bool SaveLineasFichaBDet(LineasFichaBDetDTO lineasFichaBDetDto);
        bool UpdateLineasFichaBDet(LineasFichaBDetDTO lineasFichaBDetDto);
        int GetLastLineasFichaBDetCodi();
        bool DeleteLineasFichaBDetById(int id, string usuario);
        LineasFichaBDetDTO GetLineasFichaBDetById(int id);
    }
}
