using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_FUENTEGAMS
    /// </summary>
    public interface ICpFuentegamsRepository
    {
        int Save(CpFuentegamsDTO entity);
        void Update(CpFuentegamsDTO entity);
        void Delete(int ftegcodi);
        CpFuentegamsDTO GetById(int ftegcodi);
        List<CpFuentegamsDTO> List();
        List<CpFuentegamsDTO> GetByCriteria();
        CpFuentegamsDTO GetByIdVersion(int fverscodi);
        void ResetOficial();
        void SetOficial(int ftegcodi);
    }
}
