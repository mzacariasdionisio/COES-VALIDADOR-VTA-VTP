using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_PLAZOENVIO
    /// </summary>
    public interface ISiPlazoenvioRepository
    {
        int Save(SiPlazoenvioDTO entity);
        void Update(SiPlazoenvioDTO entity);
        void Delete(int plazcodi);
        SiPlazoenvioDTO GetById(int plazcodi);
        SiPlazoenvioDTO GetByFdatcodi(int fdatcodi);
        List<SiPlazoenvioDTO> List();
        List<SiPlazoenvioDTO> GetByCriteria();
    }
}
