using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_BLOBCONFIG
    /// </summary>
    public interface IWbBlobconfigRepository
    {
        int Save(WbBlobconfigDTO entity);
        void Update(WbBlobconfigDTO entity);
        void Delete(int configcodi);
        WbBlobconfigDTO GetById(int configcodi);
        List<WbBlobconfigDTO> List();
        List<WbBlobconfigDTO> GetByCriteria();
    }
}
