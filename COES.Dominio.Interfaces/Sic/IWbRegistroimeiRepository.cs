using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_REGISTROIMEI
    /// </summary>
    public interface IWbRegistroimeiRepository
    {
        int Save(WbRegistroimeiDTO entity);
        void Update(WbRegistroimeiDTO entity);
        void Delete(int regimecodi);
        WbRegistroimeiDTO GetById(int regimecodi);
        List<WbRegistroimeiDTO> List();
        List<WbRegistroimeiDTO> GetByCriteria();
    }
}
