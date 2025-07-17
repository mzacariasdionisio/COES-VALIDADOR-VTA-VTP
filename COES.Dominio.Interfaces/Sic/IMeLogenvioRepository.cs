using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_LOGENVIO
    /// </summary>
    public interface IMeLogenvioRepository
    {
        void Save(MeLogenvioDTO entity);
        void Update(MeLogenvioDTO entity);
        void Delete();
        MeLogenvioDTO GetById();
        List<MeLogenvioDTO> List();
        List<MeLogenvioDTO> GetByCriteria();
    }
}
