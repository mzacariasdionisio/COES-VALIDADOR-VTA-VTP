using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface IRerEvaluacionEnergiaUnidDetRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int GetMaxId();
        void Save(RerEvaluacionEnergiaUnidDetDTO entity, IDbConnection conn, DbTransaction tran);
        int Save(RerEvaluacionEnergiaUnidDetDTO entity);
        void Update(RerEvaluacionEnergiaUnidDetDTO entity);
        void Delete(int rereedcodi);
        RerEvaluacionEnergiaUnidDetDTO GetById(int rereedcodi);
        List<RerEvaluacionEnergiaUnidDetDTO> List();
        List<RerEvaluacionEnergiaUnidDetDTO> GetByCriteria(string rereeucodi);

    }
}

