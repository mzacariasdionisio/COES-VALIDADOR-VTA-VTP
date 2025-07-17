using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_MEDICION48
    /// </summary>
    public interface ICoMedicion48Repository
    {
        int Save(CoMedicion48DTO entity);
        void Update(CoMedicion48DTO entity);
        void Delete(int comedcodi);
        CoMedicion48DTO GetById(int comedcodi);
        List<CoMedicion48DTO> List();
        List<CoMedicion48DTO> GetByCriteria();
        List<CoMedicion48DTO> ObtenerTopologias(DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerReporteReprograma(DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerReporteReprogramaSinRSF(DateTime fechaInicio, DateTime fechaFin);
        List<CpMedicion48DTO> GetByCriteria(string topcodi, DateTime medifecha, string srestcodi);
        List<CoMedicion48DTO> ObtenerDatosRAEjecutado(DateTime fechaInicio, DateTime fechaFin, out List<CoRaejecutadadetDTO> detalle);
        List<CoMedicion48DTO> ObtenerTopologiasSinReserva(DateTime fechaInicio, DateTime fechaFin);
        List<CpMedicion48DTO> ObtenerDatosAgrupados(string topcodi, DateTime medifecha, string srestcodi);
        void GrabarDatosBulk(List<CoMedicion48DTO> entitys, int idPeriodo, int idVersion, int tipoInfo, DateTime fechaInicio, DateTime fechaFin);
        void GrabarDatosBulkResult(List<CoMedicion48DTO> entitys, int idPeriodo, int idVersion, int tipoInfo, DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerDatosProgramaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin, int idTipoInfo);
        List<CoMedicion48DTO> ObtenerDatosReservaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin, int idTipoInfo);
        List<CoMedicion48DTO> ObtenerDatosReservaEjecutadaFinal(int idVersion, DateTime fechaInicio, DateTime fechaFin,
            int idTipoInfo);
        List<CoMedicion48DTO> ObtenerListadoURS();
        List<CoMedicion48DTO> ObtenerPropiedadesHidraulica(DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerPropiedadTermica(DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerPropiedadPotenciaEfectiva(int idEquipo);
        List<CoMedicion48DTO> ObtenerPropiedadPotenciaMinima(int idEquipo);
        List<CoMedicion48DTO> ObtenerHorasOperacion(DateTime fechaInicio, DateTime fechaFin);
        List<CoMedicion48DTO> ObtenerDatosResultadoFinal(int idVersion, int idTipoInfo);
        List<CoMedicion48DTO> ObtenerReporteProgramadoFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs);
        List<CoMedicion48DTO> ObtenerReporteReservaFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs);
        List<CoMedicion48DTO> ObtenerReporteDespachoFinal(int idVersion, int idTipoInformacion, DateTime fechaInicio, DateTime fechaFin, int idUrs);
        void EliminarLiquidacionFinal(int recacodi);
        List<Dominio.DTO.Transferencias.VcrDespachoursDTO> ObtenerLiquidacionResultadoFinal(int idVersion,
            int idTipoInfo, int recacodi, string usuario);
        List<CoMedicion48DTO> ObtenerReporteBandas(DateTime fechaInicio, DateTime fechaFin);
      
    }
}
