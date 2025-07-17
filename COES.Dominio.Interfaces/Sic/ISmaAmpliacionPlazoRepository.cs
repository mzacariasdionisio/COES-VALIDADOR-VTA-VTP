using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_AMPLIACION_PLAZO
    /// </summary>
    public interface ISmaAmpliacionPlazoRepository
    {
        int Save(SmaAmpliacionPlazoDTO entity);
        void Update(SmaAmpliacionPlazoDTO entity);
        void Delete(int smaapcodi);
        SmaAmpliacionPlazoDTO GetById(int smaapcodi);
        List<SmaAmpliacionPlazoDTO> List();
        List<SmaAmpliacionPlazoDTO> GetByCriteria();
    }
}
