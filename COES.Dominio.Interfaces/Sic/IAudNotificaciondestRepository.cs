using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_NOTIFICACIONDEST
    /// </summary>
    public interface IAudNotificaciondestRepository
    {
        int Save(AudNotificaciondestDTO entity);
        void Update(AudNotificaciondestDTO entity);
        void Delete(int notdcodi);
        AudNotificaciondestDTO GetById(int notdcodi);
        List<AudNotificaciondestDTO> List();
        List<AudNotificaciondestDTO> GetByCriteria();
    }
}
