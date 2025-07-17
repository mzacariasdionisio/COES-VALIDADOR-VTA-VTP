using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_REL_RECURSO_PTO
    /// </summary>
    public interface IMpRelRecursoPtoRepository
    {
        void Save(MpRelRecursoPtoDTO entity);
        void Save(MpRelRecursoPtoDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(MpRelRecursoPtoDTO entity);
        void Delete(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte);
        void Delete(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte, IDbConnection connection, IDbTransaction transaction);
        MpRelRecursoPtoDTO GetById(int mtopcodi, int mrecurcodi, int ptomedicodi, int lectcodi, string mrptohorizonte);
        List<MpRelRecursoPtoDTO> List();
        List<MpRelRecursoPtoDTO> GetByCriteria();
        List<MpRelRecursoPtoDTO> ListarPorTopologia(int mtopcodi);
        List<MpRelRecursoPtoDTO> ListarPorTopologiaYRecurso(int mtopcodi, int mrecurcodi);
        

    }
}
