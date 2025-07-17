using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_ESTACIONH
    /// </summary>
    public interface IPmoEstacionhRepository
    {
        int Save(PmoEstacionhDTO entity);
        void Update(PmoEstacionhDTO entity);
        void Delete(int pmehcodi);
        PmoEstacionhDTO GetById(int pmehcodi);
        List<PmoEstacionhDTO> List();
        List<PmoEstacionhDTO> GetByCriteria(int ptomedicodi);
        int Save(PmoEstacionhDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateEstadoEstacionHidro(PmoEstacionhDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateOrdenEstacionHidro(int orden, int sddpCodi);
    }
}
