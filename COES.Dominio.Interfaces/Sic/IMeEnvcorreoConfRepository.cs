using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ENVCORREO_CONF
    /// </summary>
    public interface IMeEnvcorreoConfRepository
    {
        int Save(MeEnvcorreoConfDTO entity);
        void Update(MeEnvcorreoConfDTO entity);
        void Delete(int ecconfcodi);
        MeEnvcorreoConfDTO GetById(int ecconfcodi);
        List<MeEnvcorreoConfDTO> List();
        List<MeEnvcorreoConfDTO> GetByCriteria();
    }
}
