using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_AUDITORIAELEMENTO
    /// </summary>
    public interface IAudAuditoriaprocesoRepository
    {
        int Save(AudAuditoriaprocesoDTO entity);
        void Update(AudAuditoriaprocesoDTO entity);
        void Delete(AudAuditoriaprocesoDTO auditoriaproceso);
        void DeleteAllAudAuditoriaproceso(AudAuditoriaprocesoDTO auditoriaproceso);
        AudAuditoriaprocesoDTO GetById(int audipcodi);
        AudAuditoriaprocesoDTO GetByAudppcodi(int audicodi, int Audppcodi, int Proccodi);
        List<AudAuditoriaprocesoDTO> List(int audpcodi);
        List<AudAuditoriaprocesoDTO> GetByCriteria(int audicodi);
        List<AudAuditoriaprocesoDTO> GetByAuditoriaElementoPorTipo(int audicodi, int tabcdcoditipoelemento);
    }
}
