using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CAI_AJUSTECMARGINAL
    /// </summary>
    public interface ICaiAjustecmarginalRepository
    {
        int Save(CaiAjustecmarginalDTO entity);
        void Update(CaiAjustecmarginalDTO entity);
        void Delete(int caiajcodi);
        CaiAjustecmarginalDTO GetById(int caajcmcodi);
        List<CaiAjustecmarginalDTO> List(int caiajcodi);
        List<CaiAjustecmarginalDTO> GetByCriteria();
    }
}
