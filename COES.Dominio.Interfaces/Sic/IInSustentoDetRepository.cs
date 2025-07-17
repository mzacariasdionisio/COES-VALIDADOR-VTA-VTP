using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SUSTENTO_DET
    /// </summary>
    public interface IInSustentoDetRepository
    {
        int GetMaxId();
        int Save(InSustentoDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InSustentoDetDTO entity);
        void Delete(int instdcodi);
        InSustentoDetDTO GetById(int instdcodi);
        List<InSustentoDetDTO> List();
        List<InSustentoDetDTO> GetByCriteria(int instcodi);
    }
}
