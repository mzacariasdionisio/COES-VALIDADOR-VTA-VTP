using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_ESTADCANAL_SP7
    /// </summary>
    public interface ITrEstadcanalSp7Repository
    {
        void Save(TrEstadcanalSp7DTO entity);
        void Update(TrEstadcanalSp7DTO entity);
        void Delete(int canalcodi, DateTime fecha);
        TrEstadcanalSp7DTO GetById(int canalcodi, DateTime fecha);
        List<TrEstadcanalSp7DTO> List();
        List<TrEstadcanalSp7DTO> GetByCriteria();
         List<TrEstadcanalSp7DTO> GetDispDiaSignal(DateTime fecha, int emprcodi);
         int GetPaginadoDispDiaSignal( DateTime fecha, int emprcodi);
    }
}
