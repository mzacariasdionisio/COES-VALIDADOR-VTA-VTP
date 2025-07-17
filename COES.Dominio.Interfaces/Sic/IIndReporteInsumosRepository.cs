using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE_INSUMOS
    /// </summary>
    public interface IIndReporteInsumosRepository
    {
        int GetMaxId();
        void Save(IndReporteInsumosDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int irpinscodi);
        IndReporteInsumosDTO GetById(int irpinscodi);
        List<IndReporteInsumosDTO> GetByCriteria(string itotcodi);
        List<IndReporteInsumosDTO> List();

    }
}
