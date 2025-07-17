using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_FACTORPERDIDA
    /// </summary>
    public interface ICmFactorperdidaRepository
    {
        int Save(CmFactorperdidaDTO entity);
        void Update(CmFactorperdidaDTO entity);
        void Delete(int cmfpmcodi);
        CmFactorperdidaDTO GetById(int cmfpmcodi);
        List<CmFactorperdidaDTO> List();
        List<CmFactorperdidaDTO> GetByCriteria(DateTime fecha);
    }
}
