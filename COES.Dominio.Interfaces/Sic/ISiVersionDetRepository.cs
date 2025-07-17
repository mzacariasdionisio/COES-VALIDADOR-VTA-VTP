using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_VERSION_DET
    /// </summary>
    public interface ISiVersionDetRepository
    {
        int GetMaxId();
        int Save(SiVersionDetDTO entity);
        int SaveTransaccional(SiVersionDetDTO entity, IDbConnection conn, DbTransaction tran);
        SiVersionDTO GetByVersionDetIEOD(int verscodi, decimal nroReporte);
        SiVersionDetDTO GetByIdVersionYNumeral(int verscodi, int mrepcodi);
    }
}
