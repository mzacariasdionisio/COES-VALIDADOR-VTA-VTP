using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_TIPOCORREO
    /// </summary>
    public interface IReTipocorreoRepository
    {
        int Save(ReTipocorreoDTO entity);
        void Update(ReTipocorreoDTO entity);
        void Delete(int retcorcodi);
        ReTipocorreoDTO GetById(int retcorcodi);
        List<ReTipocorreoDTO> List();
        List<ReTipocorreoDTO> GetByCriteria();
    }
}
