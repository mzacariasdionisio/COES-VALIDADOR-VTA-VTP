using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_AREANIVEL
    /// </summary>
    public interface IEqAreanivelRepository
    {
        int Save(EqAreaNivelDTO entity);
        void Update(EqAreaNivelDTO entity);
        void Delete(int anivelcodi);
        EqAreaNivelDTO GetById(int anivelcodi);
        List<EqAreaNivelDTO> List();
        List<EqAreaNivelDTO> GetByCriteria();
    }
}
