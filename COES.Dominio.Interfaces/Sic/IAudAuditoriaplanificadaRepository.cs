using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_AUDITORIAPLANIFICADA
    /// </summary>
    public interface IAudAuditoriaplanificadaRepository
    {
        int Save(AudAuditoriaplanificadaDTO entity);
        void Update(AudAuditoriaplanificadaDTO entity);
        void Delete(AudAuditoriaplanificadaDTO auditoriaplanificada);
        void DeleteByAudPlan(AudAuditoriaplanificadaDTO auditoriaPlanificada);
        AudAuditoriaplanificadaDTO GetById(int audpcodi);
        List<AudAuditoriaplanificadaDTO> List();
        List<AudAuditoriaplanificadaDTO> GetByCriteria(int plancodi, string audphistorico);
        string GetByAudPlanificadaValidacion(int audpcodi);
    }
}
