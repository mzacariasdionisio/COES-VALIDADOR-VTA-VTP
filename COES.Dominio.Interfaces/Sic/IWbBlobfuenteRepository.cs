using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_BLOBFUENTE
    /// </summary>
    public interface IWbBlobfuenteRepository
    {
        int Save(WbBlobfuenteDTO entity);
        void Update(WbBlobfuenteDTO entity);
        void Delete(int blofuecodi);
        WbBlobfuenteDTO GetById(int blofuecodi);
        List<WbBlobfuenteDTO> List();
        List<WbBlobfuenteDTO> GetByCriteria();
    }
}
