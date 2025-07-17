using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE_CALCULOS
    /// </summary>
    public interface IIndReporteCalculosRepository
    {
        int GetMaxId();
        void Save(IndReporteCalculosDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int irpcalcodi);
        IndReporteCalculosDTO GetById(int irpcalcodi);
        List<IndReporteCalculosDTO> GetByCriteria(string itotcodi);
        List<IndReporteCalculosDTO> List();
    }
}
