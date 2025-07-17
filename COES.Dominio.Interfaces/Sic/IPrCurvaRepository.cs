using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPO
    /// </summary>
    public interface IPrCurvaRepository
    {
        int Save(PrCurvaDTO entity);
        void Update(PrCurvaDTO entity);
        void Delete(int curvacodi);
        PrCurvaDTO GetById(int curvacodi);
        List<PrCurvaDTO> List();
        void AddDetail(int curvacodi, int grupocodi);
        void DeleteDetail(int curvacodi, int grupocodi);
        void UpdatePrincipal(int curvacodi, int grupocodi);
    }
}
