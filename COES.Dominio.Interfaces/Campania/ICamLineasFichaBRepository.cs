using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamLineasFichaBRepository
    {
        List<LineasFichaBDTO> GetLineasFichaB(int proyCodi);
        bool SaveLineasFichaB(LineasFichaBDTO lineasFichaBDto);
        bool UpdateLineasFichaB(LineasFichaBDTO lineasFichaBDto);
        int GetLastLineasFichaBCodi();
        bool DeleteLineasFichaBById(int id, string usuario);
        LineasFichaBDTO GetLineasFichaBById(int id);
    }
}
