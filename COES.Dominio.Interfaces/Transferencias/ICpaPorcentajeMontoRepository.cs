using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PORCENTAJE_MONTO
    /// </summary>
    public interface ICpaPorcentajeMontoRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CpaPorcentajeMontoDTO entity);
        void Save(CpaPorcentajeMontoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CpaPorcentajeMontoDTO entity);
        void Delete(int cpapmtcodi);
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);
        List<CpaPorcentajeMontoDTO> List();
        List<CpaPorcentajeMontoDTO> ListByRevision(int cparcodi);
        CpaPorcentajeMontoDTO GetById(int cpapmtcodi);
    }

}

