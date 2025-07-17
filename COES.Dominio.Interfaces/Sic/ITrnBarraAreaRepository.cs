using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_BARRA_AREA
    /// </summary>
    public interface ITrnBarraAreaRepository
    {
        int Save(TrnBarraAreaDTO entity);
        void Update(TrnBarraAreaDTO entity);
        void Delete(int bararecodi);
        TrnBarraAreaDTO GetById(int bararecodi);
        List<TrnBarraAreaDTO> List();
        List<TrnBarraAreaDTO> GetByCriteria();
    }
}
