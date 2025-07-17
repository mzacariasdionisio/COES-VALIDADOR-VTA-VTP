using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_COUNTER
    /// </summary>
    public interface IFwCounterRepository
    {
        void Update(FwCounterDTO entity);
        void Delete(string tablename);
        FwCounterDTO GetById(string tablename);
        List<FwCounterDTO> List();
        List<FwCounterDTO> GetByCriteria();
        void UpdateMaxCount(string tablename);
    }
}
