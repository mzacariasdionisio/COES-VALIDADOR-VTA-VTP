using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_PTOXESTACION
    /// </summary>
    public interface IPmoPtoxestacionRepository
    {
        int Save(PmoPtoxestacionDTO entity);
        void Update(PmoPtoxestacionDTO entity);
        void Delete(int pmpxehcodi);
        PmoPtoxestacionDTO GetById(int pmpxehcodi);
        List<PmoPtoxestacionDTO> List();
        List<PmoPtoxestacionDTO> GetByCriteria(int pmehcodi);
        int Save(PmoPtoxestacionDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateEstadoPtoxestacion(PmoPtoxestacionDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
