using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_COMITE_LISTA
    /// </summary>
    public interface IWbComiteListaRepository
    {
        int Save(WbComiteListaDTO entity);
        void Update(WbComiteListaDTO entity);
        void Delete(int comitelistacodi);
        List<WbComiteListaDTO> GetByCriteria();
        List<WbComiteListaDTO> ListByComite(int comitecodi);

    }
}
