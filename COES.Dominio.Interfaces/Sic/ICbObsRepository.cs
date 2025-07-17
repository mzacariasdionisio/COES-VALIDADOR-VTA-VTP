using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_OBS
    /// </summary>
    public interface ICbObsRepository
    {
        int GetMaxId();
        int Save(CbObsDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbObsDTO entity);
        void Delete(int cbobscodi);
        CbObsDTO GetById(int cbobscodi);
        List<CbObsDTO> List(string cbcentcodis);
        List<CbObsDTO> ListByCbvercodi(int cbvercodi);
        List<CbObsDTO> GetByCriteria();
    }
}
