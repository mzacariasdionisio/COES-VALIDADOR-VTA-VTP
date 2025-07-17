using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_PARAM_PROCESO
    /// </summary>
    public interface ISmaParamProcesoRepository
    {
        int Save(SmaParamProcesoDTO entity);
        SmaParamProcesoDTO GetValidRangeNCP();
        void UpdateInactive(int papocodi);
        void Update(SmaParamProcesoDTO entity);
        void Delete(int papocodi);
        SmaParamProcesoDTO GetById(int papocodi);
        List<SmaParamProcesoDTO> List();
        List<SmaParamProcesoDTO> GetByCriteria();
    }
}
