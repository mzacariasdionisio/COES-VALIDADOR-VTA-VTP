using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_FACTORCONVERSION
    /// </summary>
    public interface ISiFactorconversionRepository
    {
        int Save(SiFactorconversionDTO entity);
        void Update(SiFactorconversionDTO entity);
        void Delete(int tconvcodi);
        SiFactorconversionDTO GetById(int tconvcodi);
        List<SiFactorconversionDTO> List();
        List<SiFactorconversionDTO> GetByCriteria();
    }
}
