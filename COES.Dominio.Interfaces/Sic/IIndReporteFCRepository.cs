using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE_FC
    /// </summary>
    public interface IIndReporteFCRepository
    {
        int GetMaxId();
        void Save(IndReporteFCDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int irptfccodi);
        IndReporteFCDTO GetById(int irptfccodi);
        List<IndReporteFCDTO> GetByCriteria(string itotcodi);
        List<IndReporteFCDTO> List();
    }
}
