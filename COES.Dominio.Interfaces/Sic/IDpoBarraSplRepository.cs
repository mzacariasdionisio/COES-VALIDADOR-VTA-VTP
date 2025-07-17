using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoBarraSplRepository
    {
        void Save(DpoBarraSplDTO entity);
        void Update(DpoBarraSplDTO entity);
        void Delete(int id);
        List<DpoBarraSplDTO> List();
        DpoBarraSplDTO GetById(int id);
        void UpdateDpoBarraSPlEstado(string ids, string estado);
        List<DpoBarraSplDTO> ListBarrasSPLByGrupo(string barras);
    }
}
