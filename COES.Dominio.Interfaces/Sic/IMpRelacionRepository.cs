using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla MP_RELACION
    /// </summary>
    public interface IMpRelacionRepository
    {        
        void Update(MpRelacionDTO entity);
        void Delete(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2);
        void Delete(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2, IDbConnection connection, IDbTransaction transaction);
        MpRelacionDTO GetById(int mtopcodi, int mtrelcodi, int mrecurcodi1, int mrecurcodi2);
        List<MpRelacionDTO> List();
        List<MpRelacionDTO> GetByCriteria();        
        void Save(MpRelacionDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<MpRelacionDTO> ListarPorTopologia(int mtopcodi);
        List<MpRelacionDTO> ListarPorTopologiaYRecurso(int mtopcodi, int recurcodi1);
        

    }
}
