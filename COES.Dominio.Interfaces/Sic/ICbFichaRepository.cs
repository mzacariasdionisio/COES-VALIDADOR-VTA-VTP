using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_FICHA
    /// </summary>
    public interface ICbFichaRepository
    {
        int Save(CbFichaDTO entity);
        void Update(CbFichaDTO entity);
        CbFichaDTO GetById(int cbftcodi);
        List<CbFichaDTO> List();
        List<CbFichaDTO> GetByCriteria();
    }
}
