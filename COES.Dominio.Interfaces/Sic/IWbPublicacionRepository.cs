using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_PUBLICACION
    /// </summary>
    public interface IWbPublicacionRepository
    {
        int Save(WbPublicacionDTO entity);
        void Update(WbPublicacionDTO entity);
        void Delete(int publiccodi);
        WbPublicacionDTO GetById(int publiccodi);
        List<WbPublicacionDTO> List();
        List<WbPublicacionDTO> GetByCriteria();
    }
}
