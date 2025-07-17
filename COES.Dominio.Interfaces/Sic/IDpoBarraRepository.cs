using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoBarraRepository
    {
        void Save(DpoBarraDTO entity);
        void Update(DpoBarraDTO entity);
        void Delete(string id);
        List<DpoBarraDTO> List();
        DpoBarraDTO GetById(string id);
    }
}
