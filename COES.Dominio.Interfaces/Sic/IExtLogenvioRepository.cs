using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EXT_LOGENVIO
    /// </summary>
    public interface IExtLogenvioRepository
    {
        int Save(ExtLogenvioDTO entity);
        void Update(ExtLogenvioDTO entity);
        void Delete(int logcodi);
        ExtLogenvioDTO GetById(int logcodi);
        List<ExtLogenvioDTO> List();
        List<ExtLogenvioDTO> GetByCriteria(int lectCodi);
    }
}

