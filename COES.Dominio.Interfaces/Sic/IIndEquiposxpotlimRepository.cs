using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_EQUIPOSXPOTLIM
    /// </summary>
    public interface IIndEquiposxpotlimRepository
    {
        int Save(IndEquiposxpotlimDTO entity);
        void Update(IndEquiposxpotlimDTO entity);
        void Delete(int Cuadr3codi);
        IndEquiposxpotlimDTO GetById(int equlimcodi);
        List<IndEquiposxpotlimDTO> List();
        List<IndEquiposxpotlimDTO> GetByCriteria();
    }
}
