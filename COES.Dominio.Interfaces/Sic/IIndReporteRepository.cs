using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_REPORTE
    /// </summary>
    public interface IIndReporteRepository
    {
        int Save(IndReporteDTO entity, IDbConnection conn, DbTransaction tran);
        void UpdateAprobar(IndReporteDTO entity);
        void UpdateHistorico(IndReporteDTO entity);
        void Delete(int irptcodi);
        IndReporteDTO GetById(int irptcodi);
        List<IndReporteDTO> List();
        List<IndReporteDTO> GetByCriteria(int icuacodi, int irecacodi, string irptcodis);
        List<IndReporteDTO> ListadoReportesparaPFR(int indrecacodiant, int esVersionValidado, int reportePR25Cuadro3FactorK, int reportePR25FactorProgTermico, int reportePR25FactorProgHidro);
    }
}
