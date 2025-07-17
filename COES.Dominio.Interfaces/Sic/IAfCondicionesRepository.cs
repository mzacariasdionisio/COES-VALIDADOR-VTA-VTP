using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AF_CONDICIONES
    /// </summary>
    public interface IAfCondicionesRepository
    {
        int Save(AfCondicionesDTO entity);
        void Update(AfCondicionesDTO entity);
        void Delete(int afcondcodi);
        AfCondicionesDTO GetById(int afcondcodi);
        List<AfCondicionesDTO> List();
        List<AfCondicionesDTO> GetByCriteria();

        #region Intranet CTAF
        List<AfCondicionesDTO> ListByAfecodi(int afecodi);
        void DeleteByAfecodi(int afecodi, IDbConnection connection, IDbTransaction transaction);
        int Save(AfCondicionesDTO entity, IDbConnection connection, IDbTransaction transaction);
        #endregion
    }
}
