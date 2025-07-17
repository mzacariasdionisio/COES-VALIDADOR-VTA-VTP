using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ABI_ENERGIAXFUENTENERGIA
    /// </summary>
    public interface IAbiEnergiaxfuentenergiaRepository
    {
        int Save(AbiEnergiaxfuentenergiaDTO entity);
        void Update(AbiEnergiaxfuentenergiaDTO entity);
        void Delete(int mdfecodi);
        void DeleteByMes(DateTime fechaPeriodo);
        AbiEnergiaxfuentenergiaDTO GetById(int mdfecodi);
        List<AbiEnergiaxfuentenergiaDTO> List();
        List<AbiEnergiaxfuentenergiaDTO> GetByCriteria();
    }
}
