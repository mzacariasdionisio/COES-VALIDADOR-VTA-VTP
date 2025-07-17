using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EXT_LOGPRO
    /// </summary>
    public interface IExtLogproRepository
    {
        void Save(ExtLogproDTO entity);
        void Update(ExtLogproDTO entity);
        void Delete();
        ExtLogproDTO GetById();
        List<ExtLogproDTO> List();
        List<ExtLogproDTO> GetByCriteria();
    }
}
