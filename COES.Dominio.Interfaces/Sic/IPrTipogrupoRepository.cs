using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_TIPOGRUPO
    /// </summary>
    public interface IPrTipogrupoRepository
    {
        int Save(PrTipogrupoDTO entity);
        void Update(PrTipogrupoDTO entity);
        void Delete(int tipogrupocodi);
        PrTipogrupoDTO GetById(int tipogrupocodi);
        List<PrTipogrupoDTO> List();
        List<PrTipogrupoDTO> GetByCriteria();
    }
}

