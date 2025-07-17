using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_REL_RECURSO_EQ
    /// </summary>
    public interface IMpRelRecursoEqRepository
    {
        void Save(MpRelRecursoEqDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(MpRelRecursoEqDTO entity);
        void Delete(int mtopcodi, int mrecurcodi, int equicodi);
        void Delete(int mtopcodi, int mrecurcodi, int equicodi, IDbConnection connection, IDbTransaction transaction);
        MpRelRecursoEqDTO GetById(int mtopcodi, int mrecurcodi, int equicodi);
        List<MpRelRecursoEqDTO> List();
        List<MpRelRecursoEqDTO> GetByCriteria();
        List<MpRelRecursoEqDTO> ListarPorTopologia(int mtopcodi);
        List<MpRelRecursoEqDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi);
        
    }
}
