using COES.Dominio.DTO.Campania;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamLineasFichaARepository
    {
        List<LineasFichaADTO> GetLineasFichaACodi(int proyCodi);
        bool SaveLineasFichaA(LineasFichaADTO lineasFichaADto);
        int GetLastLineasFichaAId();
        bool DeleteLineasFichaAById(int id, string usuario);
        LineasFichaADTO GetLineasFichaAById(int id);
        List<LineasFichaADTO> GetLineasFichaAByFilter(string plancodi, string empresa, string estado);
    }
}
