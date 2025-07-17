using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_PROGAUDI_HALLAZGOS
    /// </summary>
    public interface IAudProgaudiHallazgosRepository
    {
        int Save(AudProgaudiHallazgosDTO entity);
        void Update(AudProgaudiHallazgosDTO entity);
        void Delete(AudProgaudiHallazgosDTO progaudiHallazgo);
        AudProgaudiHallazgosDTO GetById(int progahcodi);
        List<AudProgaudiHallazgosDTO> List();
        List<AudProgaudiHallazgosDTO> GetByCriteria(int progaecodi);
        int ObtenerNroRegistrosBusqueda(AudProgaudiHallazgosDTO progaudiHallazgo);
        List<AudProgaudiHallazgosDTO> GetByCriteriaPorAudi(AudProgaudiHallazgosDTO progaudiHallazgo);
    }
}
