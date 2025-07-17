using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PLANAUDITORIA
    /// </summary>
    public interface IAudPlanauditoriaRepository
    {
        int Save(AudPlanauditoriaDTO entity);
        void Update(AudPlanauditoriaDTO entity);
        void Delete(AudPlanauditoriaDTO planAuditoria);
        AudPlanauditoriaDTO GetById(int plancodi);
        List<AudPlanauditoriaDTO> List();
        List<AudPlanauditoriaDTO> GetByCriteria(AudPlanauditoriaDTO planAuditoria);
        int ObtenerNroRegistrosBusqueda(AudPlanauditoriaDTO planAuditoria);
        string GetByPlanValidacion(int plancodi);
    }
}
