using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_OBSERVACION_ESTADO
    /// </summary>
    public interface ITrObservacionEstadoRepository
    {
        int Save(TrObservacionEstadoDTO entity);
        void Update(TrObservacionEstadoDTO entity);
        void Delete(int obsestcodi);
        TrObservacionEstadoDTO GetById(int obsestcodi);        
        List<TrObservacionEstadoDTO> GetByCriteria(int obscodi);
    }
}
