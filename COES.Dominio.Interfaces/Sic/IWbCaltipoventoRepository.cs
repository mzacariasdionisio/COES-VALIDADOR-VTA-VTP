using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_CALTIPOVENTO
    /// </summary>
    public interface IWbCaltipoventoRepository
    {
        int Save(WbCaltipoventoDTO entity);
        void Update(WbCaltipoventoDTO entity);
        void Delete(int tipcalcodi);
        WbCaltipoventoDTO GetById(int tipcalcodi);
        List<WbCaltipoventoDTO> List();
        List<WbCaltipoventoDTO> GetByCriteria();
    }
}
