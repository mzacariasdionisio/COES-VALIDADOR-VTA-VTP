using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_MIGRACION
    /// </summary>
    public interface ITrnMigracionRepository
    {
        int Save(TrnMigracionDTO entity);
        void Update(TrnMigracionDTO entity);
        void Delete(int caiprscodi);
        TrnMigracionDTO GetById(int caiprscodi);
        List<TrnMigracionDTO> List();
        List<TrnMigracionDTO> ListMigracion();
        List<TrnMigracionDTO> GetByCriteria();
    }
}
