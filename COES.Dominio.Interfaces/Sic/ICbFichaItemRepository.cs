using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_FICHA_ITEM
    /// </summary>
    public interface ICbFichaItemRepository
    {
        int Save(CbFichaItemDTO entity);
        void Update(CbFichaItemDTO entity);
        void Delete(int cbftitcodi);
        CbFichaItemDTO GetById(int cbftitcodi);
        List<CbFichaItemDTO> List();
        List<CbFichaItemDTO> GetByCriteria(int cbftcodi);
    }
}
