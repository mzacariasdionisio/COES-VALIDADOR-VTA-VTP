using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE_TOTAL
    /// </summary>
    public interface IIndReporteTotalRepository
    {
        int GetMaxId();
        int Save(IndReporteTotalDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int itotcodi);
        IndReporteTotalDTO GetById(int itotcodi);
        List<IndReporteTotalDTO> List();
        List<IndReporteTotalDTO> GetByCriteria(string irptcodi);
        List<IndReporteTotalDTO> ListConservarValorByPeriodoCuadro(int icuacodi, int ipericodi);
    }
}
