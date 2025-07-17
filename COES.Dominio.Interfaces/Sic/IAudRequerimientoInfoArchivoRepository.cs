using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_REQUERIMIENTO_INFORM
    /// </summary>
    public interface IAudRequerimientoInfoArchivoRepository
    {
        int Save(AudRequerimientoInfoArchivoDTO entity);
        void Delete(AudRequerimientoInfoArchivoDTO entity);
        List<AudRequerimientoInfoArchivoDTO> GetByCriteria(int progaecodi);
    }
}
