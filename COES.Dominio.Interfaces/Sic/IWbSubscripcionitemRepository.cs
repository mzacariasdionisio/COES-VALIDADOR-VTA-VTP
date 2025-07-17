using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_SUBSCRIPCIONITEM
    /// </summary>
    public interface IWbSubscripcionitemRepository
    {
        void Save(WbSubscripcionitemDTO entity);
        void Update(WbSubscripcionitemDTO entity);
        void Delete(int subscripcodi);
        WbSubscripcionitemDTO GetById(int subscripcodi, int publiccodi);
        List<WbSubscripcionitemDTO> List();
        List<WbSubscripcionitemDTO> GetByCriteria(int idSubscripcion);
    }
}
