using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PORCENTAJE_PORCENTAJE
    /// </summary>
    public interface ICpaPorcentajePorcentajeRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CpaPorcentajePorcentajeDTO entity);
        void Save(CpaPorcentajePorcentajeDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CpaPorcentajePorcentajeDTO entity);
        void Delete(int cpappcodi);
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);
        List<CpaPorcentajePorcentajeDTO> List();
        List<CpaPorcentajePorcentajeDTO> ListByRevision(int cparcodi);
        CpaPorcentajePorcentajeDTO GetById(int cpappcodi);
    }

}

