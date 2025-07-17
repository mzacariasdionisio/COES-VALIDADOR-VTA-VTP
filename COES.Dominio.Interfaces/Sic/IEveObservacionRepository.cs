using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_OBSERVACION
    /// </summary>
    public interface IEveObservacionRepository
    {
        int Save(EveObservacionDTO entity);
        void Update(EveObservacionDTO entity);
        void Delete(int obscodi);
        EveObservacionDTO GetById(int obscodi);
        List<EveObservacionDTO> List();
        List<EveObservacionDTO> GetByCriteria(DateTime fecIni, string subcausacodi, string evenclasecodi);
    }
}
