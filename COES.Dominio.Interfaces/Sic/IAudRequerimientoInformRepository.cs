using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public interface IAudRequerimientoInformRepository
    {
        int Save(AudRequerimientoInformDTO entity);
        void Update(AudRequerimientoInformDTO entity);
        void Delete(AudRequerimientoInformDTO entity);
        AudRequerimientoInformDTO GetById(int reqicodi);
        List<AudRequerimientoInformDTO> List();
        List<AudRequerimientoInformDTO> GetByCriteria(int progaecodi);
        int ObtenerNroRegistrosBusqueda(AudRequerimientoInformDTO requerimientoInformacion);
        List<AudRequerimientoInformDTO> GetByCriteriaByAuditoria(AudRequerimientoInformDTO requerimientoInformacion);
    }
}
