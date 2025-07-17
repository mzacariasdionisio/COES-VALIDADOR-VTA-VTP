using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias; // Asegúrate de ajustar este namespace si es necesario
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CPA_PORCENTAJE
    /// </summary>
    public interface ICpaPorcentajeRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CpaPorcentajeDTO entity);
        void Save(CpaPorcentajeDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CpaPorcentajeDTO entity);
        void UpdateEstadoPublicacion(int cparcodi, string cpapestpub, string cpapusumodificacion, DateTime cpapfecmodificacion);
        void Delete(int cpapcodi);
        void DeleteByRevision(int cparcodi, IDbConnection conn, DbTransaction tran);
        List<CpaPorcentajeDTO> List();
        CpaPorcentajeDTO GetById(int cpapcodi);
        CpaPorcentajeDTO GetByCriteria(int cparcodi);
    }

}

