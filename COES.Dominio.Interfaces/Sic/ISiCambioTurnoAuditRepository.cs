using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CAMBIO_TURNO_AUDIT
    /// </summary>
    public interface ISiCambioTurnoAuditRepository
    {
        int Save(SiCambioTurnoAuditDTO entity);
        void Update(SiCambioTurnoAuditDTO entity);
        void Delete(int turnoauditcodi);
        SiCambioTurnoAuditDTO GetById(int turnoauditcodi);
        List<SiCambioTurnoAuditDTO> List();
        List<SiCambioTurnoAuditDTO> GetByCriteria(int cambioTurnoCodi);
    }
}
