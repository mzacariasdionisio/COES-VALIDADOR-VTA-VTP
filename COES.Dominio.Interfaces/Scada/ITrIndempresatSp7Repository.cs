using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_INDEMPRESAT_SP7
    /// </summary>
    public interface ITrIndempresatSp7Repository
    {
        void Save(TrIndempresatSp7DTO entity);
        void Update(TrIndempresatSp7DTO entity);
        void Delete(int emprcodi, DateTime fecha);
        TrIndempresatSp7DTO GetById(int emprcodi,DateTime fecha);
        List<TrIndempresatSp7DTO> List();
        List<TrIndempresatSp7DTO> GetByCriteria();
        List<TrIndempresatSp7DTO> GetListDispMensual(int emprcodi, DateTime fechaPeriodo);
        int GetPaginadoDispMensual(int emprcodi, DateTime fecha);
    }
}
