using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PF_REPORTE_DET
    /// </summary>
    public interface IPfReporteDetRepository
    {
        int Save(PfReporteDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PfReporteDetDTO entity);
        void Delete(int pfdetcodi);
        PfReporteDetDTO GetById(int pfdetcodi);
        List<PfReporteDetDTO> List();
        List<PfReporteDetDTO> GetByCriteria(int pfrptcodi);
    }
}
