using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_PARAMETROPLAZO
    /// </summary>
    public interface IInParametroPlazoRepository
    {
        int Save(InParametroPlazoDTO entity);
        void Update(InParametroPlazoDTO entity);
        void Delete(int parplacodi);
        InParametroPlazoDTO GetById(int parplacodi);
        List<InParametroPlazoDTO> List();
        List<InParametroPlazoDTO> GetByCriteria();
    }
}
