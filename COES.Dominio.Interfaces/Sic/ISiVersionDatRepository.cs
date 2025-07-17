using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_VERSION_DAT
    /// </summary>
    public interface ISiVersionDatRepository
    {
        int GetMaxId();
        int Save(SiVersionDatDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiVersionDatDTO entity);
        void Delete(int verdatcodi);
        SiVersionDatDTO GetById(int verdatcodi);
        List<SiVersionDatDTO> List();
        List<SiVersionDatDTO> GetByCriteria(int versdtcodi);
    }
}
