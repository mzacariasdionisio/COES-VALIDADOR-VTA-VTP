using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_VERSION
    /// </summary>
    public interface ISiVersionRepository
    {
        int GetMaxId();
        int Save(SiVersionDTO entity);
        int SaveTransaccional(SiVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiVersionDTO entity);
        void Delete();
        SiVersionDTO GetById(int verscodi);
        List<SiVersionDTO> List();
        List<SiVersionDTO> GetByCriteria(DateTime fecha, int tmrepcodi);
        int MaximoXFecha(DateTime fecha, int tmrepcodi);
    }
}