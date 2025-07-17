using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CAMBIO_TURNO_SUBSECCION
    /// </summary>
    public interface ISiCambioTurnoSubseccionRepository
    {
        int Save(SiCambioTurnoSubseccionDTO entity);
        void Update(SiCambioTurnoSubseccionDTO entity);
        void Delete(int subseccioncodi);
        SiCambioTurnoSubseccionDTO GetById(int subseccioncodi);
        List<SiCambioTurnoSubseccionDTO> List();
        List<SiCambioTurnoSubseccionDTO> GetByCriteria(int id);
        List<SiCambioTurnoSubseccionDTO> ObtenerRSF(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerReprogramas(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerMantenimientos(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerMantenimientoComentario(DateTime fechaAnterior, int turnoAnterior, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerSuministros(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerOperacionCentrales(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerLineasDesconectadas(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerEventosImportantes(DateTime fechaInicio, DateTime fechaFin, int seccion);
        List<SiCambioTurnoSubseccionDTO> ObtenerInformeFalla(DateTime fechaInicio, DateTime fechaFin, int seccion);
    }
}
