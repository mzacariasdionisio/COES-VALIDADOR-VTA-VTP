using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_COMITE
    /// </summary>
    public interface IWbComiteRepository
    {
        int Save(WbComiteDTO entity);
        void Update(WbComiteDTO entity);
        void Delete(int comitecodi);
        WbComiteDTO GetById(int comitecodi);
        List<WbComiteDTO> List();
        List<WbComiteDTO> GetByCriteria();
    }
}
