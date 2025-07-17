using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_VERSION
    /// </summary>
    public interface ICbVersionRepository
    {
        int Save(CbVersionDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbVersionDTO entity);
        void Delete(int cbvercodi);
        CbVersionDTO GetById(int cbvercodi);
        List<CbVersionDTO> List();
        List<CbVersionDTO> GetByCriteria(string cbenvcodi);
        void CambiarEstado(string versioncodis, string estado);
        List<CbVersionDTO> GetByPeriodoyEstado(string mesVigencia, string estados);
    }
}
