using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ABI_FACTORPLANTA
    /// </summary>
    public interface IAbiFactorplantaRepository
    {
        int Save(AbiFactorplantaDTO entity);
        void Update(AbiFactorplantaDTO entity);
        void Delete(int fpcodi);
        void DeleteByMes(DateTime fechaPeriodo);
        AbiFactorplantaDTO GetById(int fpcodi);
        List<AbiFactorplantaDTO> List();
        List<AbiFactorplantaDTO> GetByCriteria();
    }
}
