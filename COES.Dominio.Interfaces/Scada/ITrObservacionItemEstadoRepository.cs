using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_OBSERVACION_ITEM_ESTADO
    /// </summary>
    public interface ITrObservacionItemEstadoRepository
    {
        int Save(TrObservacionItemEstadoDTO entity);
        void Update(TrObservacionItemEstadoDTO entity);
        void Delete(int obitescodi);
        TrObservacionItemEstadoDTO GetById(int obitescodi);
        List<TrObservacionItemEstadoDTO> GetByCriteria(int obsitemcodi);
    }
}
