using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_NOTIFICACION
    /// </summary>
    public interface IAudNotificacionRepository
    {
        int Save(AudNotificacionDTO entity);
        void Update(AudNotificacionDTO entity);
        void Delete(int noticodi);
        AudNotificacionDTO GetById(int noticodi);
        List<AudNotificacionDTO> List();
        List<AudNotificacionDTO> GetByCriteria();
    }
}
