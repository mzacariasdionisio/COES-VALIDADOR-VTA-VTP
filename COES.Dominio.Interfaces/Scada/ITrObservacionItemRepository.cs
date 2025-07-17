using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_OBSERVACION_ITEM
    /// </summary>
    public interface ITrObservacionItemRepository
    {
        int Save(TrObservacionItemDTO entity);
        void Update(TrObservacionItemDTO entity);
        void Delete(int obsitecodi);
        TrObservacionItemDTO GetById(int obsitecodi);
        List<TrObservacionItemDTO> List();
        List<TrObservacionItemDTO> GetByCriteria(int obscodi);
        List<TrObservacionItemDTO> ObtenerReporteSeniales(int idEmpresa);
    }
}
