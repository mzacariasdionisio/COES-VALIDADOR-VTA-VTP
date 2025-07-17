using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PORCENTAJE_ENERGIAPOTENCIA
    /// </summary>
    public interface ICpaPorcentajeEnergiaPotenciaRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CpaPorcentajeEnergiaPotenciaDTO entity);
        void Save(CpaPorcentajeEnergiaPotenciaDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CpaPorcentajeEnergiaPotenciaDTO entity);
        void Delete(int cpapepcodi);
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);
        List<CpaPorcentajeEnergiaPotenciaDTO> List();
        List<CpaPorcentajeEnergiaPotenciaDTO> ListByRevision(int cparcodi);
        CpaPorcentajeEnergiaPotenciaDTO GetById(int cpapepcodi);

    }

}

