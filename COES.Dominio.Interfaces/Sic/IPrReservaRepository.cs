using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESERVA
    /// </summary>
    public interface IPrReservaRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(PrReservaDTO entity);
        int SaveTransaccional(PrReservaDTO entity, IDbConnection connection, DbTransaction transaction);
        void Update(PrReservaDTO entity);
        void UpdateTransaccional(PrReservaDTO entity, IDbConnection connection, DbTransaction transaction);
        void Delete(int prsvcodi);
        PrReservaDTO GetById(int prsvcodi);
        List<PrReservaDTO> List();
        List<PrReservaDTO> GetByCriteria(DateTime fecha, string tipo);
        void ActualizarEstadoRegistro(PrReservaDTO entity);
    }
}
