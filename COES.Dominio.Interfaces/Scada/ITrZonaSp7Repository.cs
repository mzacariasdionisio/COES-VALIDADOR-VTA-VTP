using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_ZONA_SP7
    /// </summary>
    public interface ITrZonaSp7Repository
    {
        int Save(TrZonaSp7DTO entity);
        void Update(TrZonaSp7DTO entity);
        void Delete(int zonacodi);
        TrZonaSp7DTO GetById(int zonacodi);
        List<TrZonaSp7DTO> List();
        List<TrZonaSp7DTO> GetByCriteria(string emprcodi);
        int SaveTrZonaSp7Id(TrZonaSp7DTO entity);
        List<TrZonaSp7DTO> ListByEmpresa(int emprcodi);
        
    }
}
