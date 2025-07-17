using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PROGAUDI_INVOLUCRADO
    /// </summary>
    public interface IAudProgaudiInvolucradoRepository
    {
        int Save(AudProgaudiInvolucradoDTO entity);
        void Update(AudProgaudiInvolucradoDTO entity);
        void Delete(AudProgaudiInvolucradoDTO entity);
        AudProgaudiInvolucradoDTO GetById(int progaicodi);
        AudProgaudiInvolucradoDTO GetByIdinvolucrado(int progacodi, int percodi);
        List<AudProgaudiInvolucradoDTO> List(int audicodi);
        List<AudProgaudiInvolucradoDTO> GetByCriteria(int progacodi);
    }
}
