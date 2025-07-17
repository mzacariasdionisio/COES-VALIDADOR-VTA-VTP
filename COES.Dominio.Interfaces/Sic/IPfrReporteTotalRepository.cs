using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PFR_REPORTE_TOTAL
    /// </summary>
    public interface IPfrReporteTotalRepository
    {
        int Save(PfrReporteTotalDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfrReporteTotalDTO entity);
        void Delete(int pfrtotcodi);
        PfrReporteTotalDTO GetById(int pfrtotcodi);
        List<PfrReporteTotalDTO> List();
        List<PfrReporteTotalDTO> GetByCriteria();
        List<PfrReporteTotalDTO> ListByReportecodi(int pfrrptcodi);
    }
}
