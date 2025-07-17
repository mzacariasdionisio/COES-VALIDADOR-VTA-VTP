using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PROGAUDI_ELEMENTO
    /// </summary>
    public interface IAudProgaudiElementoRepository
    {
        int Save(AudProgaudiElementoDTO entity);
        void Update(AudProgaudiElementoDTO entity);
        void Delete(AudProgaudiElementoDTO entity);
        AudProgaudiElementoDTO GetById(int progaecodi);
        AudProgaudiElementoDTO GetByElemcodi(int progracodi, int elemcodi);
        List<AudProgaudiElementoDTO> List(int audicodi);
        List<AudProgaudiElementoDTO> GetByCriteria(int progacodi);
        List<AudProgaudiElementoDTO> GetByCriteriaPorAuditoria(int audicodi);
    }
}
