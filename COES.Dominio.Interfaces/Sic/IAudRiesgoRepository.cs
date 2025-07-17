using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_RIESGO
    /// </summary>
    public interface IAudRiesgoRepository
    {
        int Save(AudRiesgoDTO entity);
        void Update(AudRiesgoDTO entity);
        void Delete(AudRiesgoDTO riesgo);
        AudRiesgoDTO GetById(int riescodi);
        List<AudRiesgoDTO> List();
        List<AudRiesgoDTO> GetByCriteria(AudRiesgoDTO riesgo);
        int ObtenerNroRegistrosBusqueda(AudRiesgoDTO riesgo);
    }
}
