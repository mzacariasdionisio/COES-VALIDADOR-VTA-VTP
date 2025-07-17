using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoSubestacionRepository
    {
        void Save(DpoSubestacionDTO entity);
        void Update(DpoSubestacionDTO entity);
        void Delete(string id);
        List<DpoSubestacionDTO> List();
        DpoSubestacionDTO GetById(string id);
    }
}
