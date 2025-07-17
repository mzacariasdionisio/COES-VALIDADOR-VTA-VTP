using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CAMBIO_TURNO_SECCION
    /// </summary>
    public interface ISiCambioTurnoSeccionRepository
    {
        int Save(SiCambioTurnoSeccionDTO entity);
        void Update(SiCambioTurnoSeccionDTO entity);
        void Delete(int seccioncodi);
        SiCambioTurnoSeccionDTO GetById(int seccioncodi);
        List<SiCambioTurnoSeccionDTO> List();
        List<SiCambioTurnoSeccionDTO> GetByCriteria(int id);
    }
}
