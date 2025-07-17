using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_TIPOIMPUGNACION
    /// </summary>
    public interface IWbTipoimpugnacionRepository
    {
        int Save(WbTipoimpugnacionDTO entity);
        void Update(WbTipoimpugnacionDTO entity);
        void Delete(int timpgcodi);
        WbTipoimpugnacionDTO GetById(int timpgcodi);
        List<WbTipoimpugnacionDTO> List();
        List<WbTipoimpugnacionDTO> GetByCriteria(string nombreTipoImpugnacion);
    }
}
