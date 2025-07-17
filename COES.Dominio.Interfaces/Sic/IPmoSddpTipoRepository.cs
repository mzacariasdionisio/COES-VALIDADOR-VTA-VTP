using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_SDDP_TIPO
    /// </summary>
    public interface IPmoSddpTipoRepository
    {
        int Save(PmoSddpTipoDTO entity);
        void Update(PmoSddpTipoDTO entity);
        void Delete(int tsddpcodi);
        PmoSddpTipoDTO GetById(int tsddpcodi);
        List<PmoSddpTipoDTO> List();
        List<PmoSddpTipoDTO> GetByCriteria();
    }
}
