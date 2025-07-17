using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_REPORTE_TOTAL
    /// </summary>
    public interface IPfReporteTotalRepository
    {
        int Save(PfReporteTotalDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfReporteTotalDTO entity);
        void Delete(int pftotcodi);
        PfReporteTotalDTO GetById(int pftotcodi);
        List<PfReporteTotalDTO> List();
        List<PfReporteTotalDTO> GetByCriteria(string pfrptcodi);
    }
}
