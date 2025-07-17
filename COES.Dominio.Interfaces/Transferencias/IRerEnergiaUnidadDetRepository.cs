using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RER_ENERGIAUNIDAD_DET
    /// </summary>
    public interface IRerEnergiaUnidadDetRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        RerEnergiaUnidadDetDTO GetById(int rereudcodi);
        void Save(RerEnergiaUnidadDetDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(RerEnergiaUnidadDetDTO entity);
        void Update(RerEnergiaUnidadDetDTO entity);
        void Delete(int rereucodi, IDbConnection conn, DbTransaction tran);
        List<RerEnergiaUnidadDetDTO> List();
        List<RerEnergiaUnidadDetDTO> GetByCriteria(string rereucodi);
    }
}

