using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDICION60
    /// </summary>
    public interface IMeMedicion60Repository
    {
        void Save(MeMedicion60DTO entity, int mes);
        void Update(MeMedicion60DTO entity, int mes);
        void Delete(DateTime fechahora, int tipoinfocodi, int ptomedicodi, int mes);
        MeMedicion60DTO GetById(DateTime fechahora, int tipoinfocodi, int ptomedicodi, int mes);
        List<MeMedicion60DTO> List(int mes);
        List<MeMedicion60DTO> GetByCriteria(int mes);
        List<int> VerificarCarga(DateTime fechaConsulta);
        List<MeMedicion60DTO> DescargarEnvio(List<int> codigos, DateTime fechaConsulta);
        List<int> ObtenerValorencero(DateTime fecha, int tipoinfocodi);
        List<RegistrorpfDTO> ObtenerRango(string ptomedicodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion60DTO> ObtenerDatosComparacionRango(DateTime fechaConsulta, string ptomedicodi, int resolucion, string mes);
        List<RegistrorpfDTO> ObtenerPotenciasMaximas(DateTime fechaConsulta);
        List<ReporteEnvioDTO> ObtenerReporte(DateTime fechaConsulta);
        void EliminarReporte(DateTime fecha);
        void GrabarReporte(ReporteEnvioDTO item);
        LogEnvioMedicionDTO ObtenerLogPorPuntoFecha(int idPto, DateTime fecha);
        void GrabarLogReporte(string descrip);
        List<MeMedicion60DTO> ListaMedicionesTmp(DateTime fechaInicial, DateTime fechaFinal, int idPto, int p);
        void GrabarDatosRpf(List<MeMedicion60DTO> entitys, DateTime fechaCarga, int mes);
        int GrabarLogEnvio(LogEnvioMedicionDTO entity);
        int EliminarCargaRpf(string ptomedicodi, DateTime fecha1, DateTime fecha2, int mes, string tipoinfocodi);
        List<MeMedicion60DTO> BuscarDatosRpf(DateTime fechaini, DateTime fechafin, int ptomedicodi, int idtipodato);
        #region Generar FileZip Pr21
        int ObtenerGenerarFileZip(DateTime fechaInicio, DateTime fechaFinal, string path, List<int> codigos);
        #endregion
    }
}
