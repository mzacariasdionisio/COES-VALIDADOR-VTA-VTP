using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_SUBESTACION
    /// </summary>
    public interface IEprSubestacionRepository
    {
        int Save(EprSubestacionDTO entity);
        void Update(EprSubestacionDTO entity);
        void Delete_UpdateAuditoria(EprSubestacionDTO entity);
        EprSubestacionDTO GetById(int epsubecodi);
        List<EprSubestacionDTO> List(int areacodi, int zonacodi);

    }
}
