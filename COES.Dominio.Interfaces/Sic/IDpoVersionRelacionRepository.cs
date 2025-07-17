using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoVersionRelacionRepository
    {
        int Save(DpoVersionRelacionDTO entity);
        void Update(DpoVersionRelacionDTO entity);
        void Delete(int id);
        List<DpoVersionRelacionDTO> List();
        DpoVersionRelacionDTO GetById(int id);
    }
}
