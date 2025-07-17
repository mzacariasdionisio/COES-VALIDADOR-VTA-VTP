using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_RELACION_TNA
    /// </summary>
    public interface IEqRelacionTnaRepository
    {
        int Save(EqRelacionTnaDTO entity);
        void Update(EqRelacionTnaDTO entity);
        void Delete(int reltnacodi);
        EqRelacionTnaDTO GetById(int reltnacodi);
        List<EqRelacionTnaDTO> List();
        List<EqRelacionTnaDTO> GetByCriteria(int relaciocodi);
    }
}
