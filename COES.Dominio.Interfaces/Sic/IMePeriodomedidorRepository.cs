using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_PERIODOMEDIDOR
    /// </summary>
    public interface IMePeriodomedidorRepository
    {
        void Update(MePeriodomedidorDTO entity);
        void Delete();
        void Save(MePeriodomedidorDTO entity);
        MePeriodomedidorDTO GetById();
        List<MePeriodomedidorDTO> List();
        List<MePeriodomedidorDTO> GetByCriteria(int idEnvio);
        List<MePeriodomedidorDTO> GetByCriteriaRango(DateTime fechaIni, DateTime fechaFin);
    }
}
