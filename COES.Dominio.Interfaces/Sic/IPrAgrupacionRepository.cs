using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_AGRUPACION
    /// </summary>
    public interface IPrAgrupacionRepository
    {
        int Save(PrAgrupacionDTO entity);
        void Update(PrAgrupacionDTO entity);
        void Delete(PrAgrupacionDTO entity);
        PrAgrupacionDTO GetById(int agrupcodi);
        List<PrAgrupacionDTO> List();
        List<PrAgrupacionDTO> GetByCriteria(int agrupfuente);
    }
}
