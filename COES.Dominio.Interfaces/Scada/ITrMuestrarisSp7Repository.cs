using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_MUESTRARIS_SP7
    /// </summary>
    public interface ITrMuestrarisSp7Repository
    {
        void Save(TrMuestrarisSp7DTO entity);
        void Update(TrMuestrarisSp7DTO entity);
        void Delete(int canalcodi, DateTime canalfecha);
        TrMuestrarisSp7DTO GetById(int canalcodi, DateTime canalfecha);
        List<TrMuestrarisSp7DTO> List();
        List<TrMuestrarisSp7DTO> GetByCriteria();
       List<TrMuestrarisSp7DTO> GetListMuestraRis(int emprcodi);
       int GetPaginadoMuestraRis(int empresa);
    }
}