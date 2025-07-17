using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SUSTENTO
    /// </summary>
    public interface IInSustentoRepository
    {
        int GetMaxId();
        int Save(InSustentoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InSustentoDTO entity);
        void Delete(int instcodi);
        InSustentoDTO GetById(int instcodi);
        List<InSustentoDTO> List();
        List<InSustentoDTO> GetByCriteria();

        InSustentoDTO GetByIntercodi(int intercodi);
    }
}
