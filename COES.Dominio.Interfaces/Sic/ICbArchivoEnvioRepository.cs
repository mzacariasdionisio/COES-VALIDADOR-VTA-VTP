using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_ARCHIVOENVIO
    /// </summary>
    public interface ICbArchivoenvioRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        int GetMaxId();
        int Save(CbArchivoenvioDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbArchivoenvioDTO entity);
        void Delete(int cbarchcodi);
        CbArchivoenvioDTO GetById(int cbarchcodi);
        List<CbArchivoenvioDTO> List();
        List<CbArchivoenvioDTO> GetByCriteria(int cbvercodi);

        List<CbArchivoenvioDTO> GetByCorreo(string corrcodis);
    }
}
