using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.ReportesFrecuencia
{
    public class ReporteFrecuenciaAuditAppServicio : AppServicioBase
    {
        public List<FrecuenciasAudit> GetFrecuenciasAudit(ReporteFrecuenciaParam param)
        {
            return FactoryReportesFrecuencia.ReporteFrecuenciaAuditRepository().GetFrecuenciasAudit(param);
        }
        public FrecuenciasAudit GetFrecuenciaAudit(int id)
        {
            return FactoryReportesFrecuencia.ReporteFrecuenciaAuditRepository().GetFrecuenciaAudit(id);
        }

        public int Grabar(ReporteFrecuenciaParam param)
        {
            return FactoryReportesFrecuencia.ReporteFrecuenciaAuditRepository().Grabar(param);
        }

        public int Eliminar(int ID, string Usuario)
        {
            return FactoryReportesFrecuencia.ReporteFrecuenciaAuditRepository().Eliminar(ID, Usuario);
        }
    }
}
