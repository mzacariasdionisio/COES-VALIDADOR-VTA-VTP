using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_TIPOSOLICITUD
    /// </summary>
    public interface IRiTiposolicitudRepository
    {
        int Save(RiTiposolicitudDTO entity);
        void Update(RiTiposolicitudDTO entity);
        void Delete(int tisocodi);
        RiTiposolicitudDTO GetById(int tisocodi);
        List<RiTiposolicitudDTO> List();
        List<RiTiposolicitudDTO> GetByCriteria();
    }
}
