using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_RELEMPETAPA
    /// </summary>
    public interface IFtExtRelempetapaRepository
    {
        int Save(FtExtRelempetapaDTO entity);
        int Save(FtExtRelempetapaDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtRelempetapaDTO entity);
        void Update(FtExtRelempetapaDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int fetempcodi);
        FtExtRelempetapaDTO GetById(int fetempcodi);
        List<FtExtRelempetapaDTO> List();
        List<FtExtRelempetapaDTO> GetByCriteria();
        List<FtExtRelempetapaDTO> GetByCriteriaProyAsigByFiltros(string sEmpresa, int idetapa);
        List<FtExtRelempetapaDTO> GetByEtapasPorElemento(int? equicodi, int? grupocodi);
        List<FtExtRelempetapaDTO> GetByProyectos(string ftprycodis);
    }
}
