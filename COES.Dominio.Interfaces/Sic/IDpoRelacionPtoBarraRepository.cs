using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoRelacionPtoBarraRepository
    {
        void Save(DpoRelacionPtoBarraDTO entity);
        void Update(DpoRelacionPtoBarraDTO entity);
        void Delete(int id);
        List<DpoRelacionPtoBarraDTO> List();
        DpoRelacionPtoBarraDTO GetById(int id);
        List<DpoRelacionPtoBarraDTO> ListBarraPuntoVersion(int version);
        List<DpoRelacionPtoBarraDTO> ListPuntoRelacionBarra(int codigo, string inicio, string fin);
        DpoRelacionPtoBarraDTO GetPuntoById(int codi);
    }
}
