using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_PROPRECURSO
    /// </summary>
    public interface IMpProprecursoRepository
    {
        void Save(MpProprecursoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Save(MpProprecursoDTO entity);
        void Update(MpProprecursoDTO entity);
        void Delete(int mtopcodi, int mrecurcodi, int mpropcodi);
        void Delete(int mtopcodi, int mrecurcodi, int mpropcodi, IDbConnection connection, IDbTransaction transaction);
        MpProprecursoDTO GetById(int mtopcodi, int mrecurcodi, int mpropcodi);
        List<MpProprecursoDTO> List();
        List<MpProprecursoDTO> GetByCriteria();
        List<MpProprecursoDTO> ListarPorTopologia(int mtopcodi);
        List<MpProprecursoDTO> ListarPorTopologiaYRecurso(int mtopcodi, int recurcodi);
        
        void Update(MpProprecursoDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
