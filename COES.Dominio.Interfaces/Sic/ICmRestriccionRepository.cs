using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_RESTRICCION
    /// </summary>
    public interface ICmRestriccionRepository
    {
        int Save(CmRestriccionDTO entity);
        void Update(CmRestriccionDTO entity);
        void Delete(int cmrestcodi);
        CmRestriccionDTO GetById(int cmrestcodi);
        List<CmRestriccionDTO> List();
        List<CmRestriccionDTO> GetByCriteria();

        List<CmRestriccionDTO> ObtenerRestriccionPorCorrida(int correlativo);
    }
}
