using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_RECURSO
    /// </summary>
    public interface IMpRecursoRepository
    {        
        void Update(MpRecursoDTO entity);
        void Delete(int mtopcodi, int mrecurcodi);
        MpRecursoDTO GetById(int mtopcodi, int mrecurcodi);
        List<MpRecursoDTO> List();
        List<MpRecursoDTO> GetByCriteria();

        List<MpRecursoDTO> ListarRecursosPorTopologia(int mtopcodi);
        int Save(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction);
        int SaveCopia(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(MpRecursoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateOrdenCentral(int orden, int topcodi, int recurcodi);
    }
}
