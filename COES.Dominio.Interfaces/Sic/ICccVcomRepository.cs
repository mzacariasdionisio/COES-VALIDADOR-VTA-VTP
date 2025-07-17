using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CCC_VCOM
    /// </summary>
    public interface ICccVcomRepository
    {
        int GetMaxId();
        int Save(CccVcomDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CccVcomDTO entity);
        void Delete(int vcomcodi);
        CccVcomDTO GetById(int vcomcodi);
        List<CccVcomDTO> List();
        List<CccVcomDTO> GetByCriteria(int cccvercodi);
    }
}
