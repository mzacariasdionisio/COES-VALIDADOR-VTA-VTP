using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_FUENTEGAMS
    /// </summary>
    public interface ICpFuentegamsversionRepository
    {
        int Save(CpFuentegamsversionDTO entity);
        void Update(CpFuentegamsversionDTO entity);
        void Delete(int ftegcodi);
        CpFuentegamsversionDTO GetById(int ftegcodi);
        List<CpFuentegamsversionDTO> List();
        List<CpFuentegamsversionDTO> GetByCriteria(int topcodi);
    }
}
