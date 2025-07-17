using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_EMPRESAXCENTRAL
    /// </summary>
    public interface IIndEmpresaxcentralRepository
    {
        int Save(IndEmpresaxcentralDTO entity);
        void Update(IndEmpresaxcentralDTO entity);
        void Delete(int empctrcodi);
        IndEmpresaxcentralDTO GetById(int empctrcodi);
        List<IndEmpresaxcentralDTO> List();
        List<IndEmpresaxcentralDTO> GetByCriteria();
    }
}
