using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Ajusta este namespace según sea necesario
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PORCENTAJE_ENVIO
    /// </summary>
    public interface ICpaPorcentajeEnvioRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CpaPorcentajeEnvioDTO entity);
        void Save(CpaPorcentajeEnvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CpaPorcentajeEnvioDTO entity);
        void Delete(int cpapecodi);
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);
        List<CpaPorcentajeEnvioDTO> List();
        List<CpaPorcentajeEnvioDTO> ListByRevision(int cparcodi);
        CpaPorcentajeEnvioDTO GetById(int cpapecodi);
        List<CpaPorcentajeEnvioDTO> GetByCparcodi(int cparcodi);
    }

}
