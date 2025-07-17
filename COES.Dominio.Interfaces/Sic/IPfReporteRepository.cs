using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_REPORTE
    /// </summary>
    public interface IPfReporteRepository
    {
        int Save(PfReporteDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfReporteDTO entity);
        void Delete(int pfrptcodi);
        PfReporteDTO GetById(int pfrptcodi);
        List<PfReporteDTO> List();
        List<PfReporteDTO> GetByCriteria(int pfrecacodi, int pfcuacodi);
    }
}
