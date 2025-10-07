using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_FORMATO
    /// </summary>
    public interface IPrRestricFormatoRepository
    {
        int Save(PrRestricFormatoDTO entity);
        void Update(PrRestricFormatoDTO entity);
        void Delete(int resfmtcodi);
        PrRestricFormatoDTO GetById(int resfmtcodi);
        List<PrRestricFormatoDTO> List();
        List<PrRestricFormatoDTO> GetByCriteria();
    }
}
