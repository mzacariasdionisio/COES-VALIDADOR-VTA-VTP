using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_REPORTE_CENTRAL
    /// </summary>
    public interface ICbReporteCentralRepository
    {
        int getIdDisponible();
        void Save(CbReporteCentralDTO entity, IDbConnection conn, IDbTransaction tran);
        int Save(CbReporteCentralDTO entity);
        void Update(CbReporteCentralDTO entity);
        void Delete(int cbrcencodi);
        CbReporteCentralDTO GetById(int cbrcencodi);
        List<CbReporteCentralDTO> List();
        List<CbReporteCentralDTO> GetByCriteria();
        List<CbReporteCentralDTO> GetByIdReporte(int cbrepcodi);
    }
}
