using COES.Dominio.DTO.Sic;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_REPORTE
    /// </summary>
    public interface IInReporteRepository
    {
        int Save(InReporteDTO entity);
        void Update(InReporteDTO entity);
        void Delete(int inrepcodi);
        InReporteDTO GetById(int inrepcodi);
        List<InReporteDTO> GetByCriteria();
        List<InReporteDTO> List();
        InReporteDTO ObtenerReportePorTipo(int tiporeporte, int progcodi);
    }
}
