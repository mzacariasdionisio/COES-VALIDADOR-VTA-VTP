using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_TIPO_INTERRUPCION
    /// </summary>
    public interface IRntTipoInterrupcionRepository
    {
        int Save(RntTipoInterrupcionDTO entity);
        void Update(RntTipoInterrupcionDTO entity);
        void Delete(int tipointcodi);
        RntTipoInterrupcionDTO GetById(int tipointcodi);
        List<RntTipoInterrupcionDTO> List();
        List<RntTipoInterrupcionDTO> GetByCriteria();
    }
}
