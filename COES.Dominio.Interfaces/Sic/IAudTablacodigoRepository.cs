using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_TABLACODIGO
    /// </summary>
    public interface IAudTablacodigoRepository
    {
        int Save(AudTablacodigoDTO entity);
        void Update(AudTablacodigoDTO entity);
        void Delete(int tabccodi);
        AudTablacodigoDTO GetById(int tabccodi);
        List<AudTablacodigoDTO> List();
        List<AudTablacodigoDTO> GetByCriteria();
    }
}
