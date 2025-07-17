using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CAMBIO_TURNO
    /// </summary>
    public interface ISiCambioTurnoRepository
    {
        int Save(SiCambioTurnoDTO entity);
        void Update(SiCambioTurnoDTO entity);
        void Delete(int cambioturnocodi);
        SiCambioTurnoDTO GetById(int cambioturnocodi);
        List<SiCambioTurnoDTO> List();
        List<SiCambioTurnoDTO> GetByCriteria();
        List<SiCambioTurnoDTO> ObtenerResponsables();
        int VerificarExistencia(int turno, DateTime fecha);
        List<string> ObtenerModosOperacion();
    }
}
