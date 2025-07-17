using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_PROCESO
    /// </summary>
    public interface IWbProcesoRepository
    {
        int Save(WbProcesoDTO entity);
        void Update(WbProcesoDTO entity);
        void Delete(int procesocodi);
        WbProcesoDTO GetById(int procesocodi);
        List<WbProcesoDTO> List();
        List<WbProcesoDTO> GetByCriteria();
    }
}
