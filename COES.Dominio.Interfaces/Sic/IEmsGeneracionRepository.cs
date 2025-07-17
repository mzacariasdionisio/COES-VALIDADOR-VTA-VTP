using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EMS_GENERACION
    /// </summary>
    public interface IEmsGeneracionRepository
    {
        int Save(EmsGeneracionDTO entity);
        void Update(EmsGeneracionDTO entity);
        void Delete(int emggencodi);
        EmsGeneracionDTO GetById(int emggencodi);
        List<EmsGeneracionDTO> List();
        List<EmsGeneracionDTO> GetByCriteria();
        List<EmsGeneracionDTO> ObtenerDatosSupervisionDemanda(DateTime fecha);
    }
}
