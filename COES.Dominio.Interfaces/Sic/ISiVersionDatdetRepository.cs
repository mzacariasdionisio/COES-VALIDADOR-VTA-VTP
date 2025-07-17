using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_VERSION_DATDET
    /// </summary>
    public interface ISiVersionDatdetRepository
    {
        int GetMaxId();
        int Save(SiVersionDatdetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiVersionDatdetDTO entity);
        void Delete(int vdatdtcodi);
        SiVersionDatdetDTO GetById(int vdatdtcodi);
        List<SiVersionDatdetDTO> List();
        List<SiVersionDatdetDTO> GetByCriteria(int versdtcodi);
    }
}
