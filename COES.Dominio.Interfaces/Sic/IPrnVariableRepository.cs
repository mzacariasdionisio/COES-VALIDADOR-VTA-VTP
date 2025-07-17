using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IPrnVariableRepository
    {
        List<PrnVariableDTO> List();
        PrnVariableDTO GetById(int codigo);
        void Delete(int codigo);
        void Save(PrnVariableDTO entity);
        void Update(PrnVariableDTO entity);
        List<PrnVariableDTO> ListVariableByTipo(string tipo);
    }
}
