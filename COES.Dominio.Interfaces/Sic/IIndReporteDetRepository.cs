using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE_DET
    /// </summary>
    public interface IIndReporteDetRepository
    {
        int GetMaxId();
        int Save(IndReporteDetDTO entity, IDbConnection conn, DbTransaction tran);
        void Delete(int idetcodi);
        IndReporteDetDTO GetById(int idetcodi);
        List<IndReporteDetDTO> List();
        List<IndReporteDetDTO> GetByCriteria(string irptcodi);
        List<IndReporteDetDTO> ListConservarValorByPeriodoCuadro(int icuacodi, int ipericodi);
    }
}
