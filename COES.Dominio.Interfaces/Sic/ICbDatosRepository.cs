using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_DATOS
    /// </summary>
    public interface ICbDatosRepository
    {
        int GetMaxId();
        int Save(CbDatosDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbDatosDTO entity);
        void Delete(int cbevdacodi);
        CbDatosDTO GetById(int cbevdacodi);
        List<CbDatosDTO> List(string cbcentcodis);
        List<CbDatosDTO> GetByCriteria(string cbvercodis, string ccombcodis);
        CbDatosDTO GetCostoCombustibleSolicitado(int ccombcodi, int idEnvio);
        List<CbDatosDTO> GetDatosReporteCV(string concepcodiS, string cbcentcodis);
    }
}
