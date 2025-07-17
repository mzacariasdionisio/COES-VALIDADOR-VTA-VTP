using COES.Dominio.DTO.ReportesFrecuencia;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.ReportesFrecuencia
{    public interface IReporteFrecuenciaAuditRepository
    {
        List<FrecuenciasAudit> GetFrecuenciasAudit(ReporteFrecuenciaParam param);
        FrecuenciasAudit GetFrecuenciaAudit(int id);
        int Grabar(ReporteFrecuenciaParam param);
        int Eliminar(int ID, string Usuario);
    }
}
