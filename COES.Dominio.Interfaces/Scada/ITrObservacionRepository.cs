using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_OBSERVACION
    /// </summary>
    public interface ITrObservacionRepository
    {
        int Save(TrObservacionDTO entity);
        void Update(TrObservacionDTO entity);
        void Delete(int obscancodi);
        TrObservacionDTO GetById(int obscancodi);
        List<TrObservacionDTO> List();
        List<TrObservacionDTO> GetByCriteria(int empresa, DateTime fechaInicio, DateTime fechaFin);
        List<ScEmpresaDTO> ObtenerEmpresasScada();
        List<TrZonaSp7DTO> ObtenerZonasPorEmpresa(int emprcodi);
        List<TrCanalSp7DTO> ObtenerCanalesObservacion(int empresa, int zona, string tipo, string descripcion, int nroPagina, int nroFilas);
        int ObtenerNroFilasBusquedaCanal(int empresa, int zona, string tipo, string descripcion);
        List<TrCanalSp7DTO> ObtenerCanalesPorCodigos(string canales);
        void ActualizarEstado(int idObservacion, string estado);
        TrObservacionDTO ObtenerEmpresa(int idEmpresa);

        #region "FIT - Señales No Disponibles"

        int ObtenerNroFilasBusquedaCanalSenalesObservadasBusqueda(int empresa);
        List<TrCanalSp7DTO> ObtenerCanalesObservacionSenalesObservadasBusqueda(int empresa, int nroPagina, int nroFilas);
        List<TrCanalSp7DTO> ObtenerSenalesObservadas(int empresa);
        List<TrCanalSp7DTO> ObtenerSenalesObservadasReportadas(int empresa);

        #endregion
    }
}
