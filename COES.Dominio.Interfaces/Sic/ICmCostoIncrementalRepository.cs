using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_COSTO_INCREMENTAL
    /// </summary>
    public interface ICmCostoIncrementalRepository
    {
        int Save(CmCostoIncrementalDTO entity);
        void Update(CmCostoIncrementalDTO entity);
        void Delete(int periodo, DateTime fechaDatos);
        CmCostoIncrementalDTO GetById(int cmcicodi);
        List<CmCostoIncrementalDTO> List();
        List<CmCostoIncrementalDTO> GetByCriteria(DateTime fecha);
    }
}
