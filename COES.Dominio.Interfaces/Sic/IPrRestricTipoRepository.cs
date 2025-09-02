using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_TIPO
    /// </summary>
    public interface IPrRestricTipoRepository
    {
        int Save(PrRestricTipoDTO entity);
        void Update(PrRestricTipoDTO entity);
        void Delete(int restipcodi);
        PrRestricTipoDTO GetById(int restipcodi);
        List<PrRestricTipoDTO> List();
        List<PrRestricTipoDTO> GetByCriteria();
    }
}
