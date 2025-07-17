using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INTERRUPCION_INSUMO
    /// </summary>
    public interface IReInterrupcionInsumoRepository
    {
        int Save(ReInterrupcionInsumoDTO entity);
        void Update(ReInterrupcionInsumoDTO entity);
        void Delete(int reinincodi);
        ReInterrupcionInsumoDTO GetById(int reinincodi);
        List<ReInterrupcionInsumoDTO> List();
        List<ReInterrupcionInsumoDTO> GetByCriteria();
        List<ReInterrupcionInsumoDTO> ObtenerPorPeriodo(int idPeriodo);
    }
}
