using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_IMPUGNACION
    /// </summary>
    public interface IWbImpugnacionRepository
    {
        int Save(WbImpugnacionDTO entity);
        void Update(WbImpugnacionDTO entity);
        void Delete(int impgcodi);
        WbImpugnacionDTO GetById(int impgcodi);
        List<WbImpugnacionDTO> List();
        List<WbImpugnacionDTO> GetByCriteria(int codigoTipo, DateTime fecha);
    }
}
