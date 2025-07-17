using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_AUDITORIA
    /// </summary>
    public interface IAudAuditoriaRepository
    {
        int Save(AudAuditoriaDTO entity);
        void Update(AudAuditoriaDTO entity);
        void Delete(AudAuditoriaDTO auditoria);
        AudAuditoriaDTO GetById(int audicodi);
        List<AudAuditoriaDTO> List();
        List<AudAuditoriaDTO> GetByCriteria(AudAuditoriaDTO auditoria);
        List<string> MostrarAnios();
        int ObtenerNroRegistrosBusqueda(AudAuditoriaDTO auditoria);
        List<AudAuditoriaDTO> MostrarAuditoriasEjecutar(AudAuditoriaDTO auditoria);
        List<AudAuditoriaDTO> VerResultados(AudAuditoriaDTO auditoria);
        int ObtenerNroRegistrosBusquedaResultados(int audicodi);
    }
}
