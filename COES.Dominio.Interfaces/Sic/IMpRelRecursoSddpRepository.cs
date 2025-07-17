using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_REL_RECURSO_SDDP
    /// </summary>
    public interface IMpRelRecursoSddpRepository
    {        
        void Update(MpRelRecursoSddpDTO entity);
        void Update(MpRelRecursoSddpDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int mtopcodi, int mrecurcodi, int sddpcodi);
        void Delete(int mtopcodi, int mrecurcodi, int sddpcodi, IDbConnection connection, IDbTransaction transaction);
        MpRelRecursoSddpDTO GetById(int mtopcodi, int mrecurcodi, int sddpcodi);
        List<MpRelRecursoSddpDTO> List();
        List<MpRelRecursoSddpDTO> GetByCriteria();        
        void Save(MpRelRecursoSddpDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<MpRelRecursoSddpDTO> ListarPorTopologia(int mtopcodi);
        List<MpRelRecursoSddpDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi);
        
    }
}
