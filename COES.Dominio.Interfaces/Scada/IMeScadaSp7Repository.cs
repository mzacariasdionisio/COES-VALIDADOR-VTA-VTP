using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_SCADA_SP7
    /// </summary>
    public interface IMeScadaSp7Repository
    {
        void Save(MeScadaSp7DTO entity);
        void Update(MeScadaSp7DTO entity);
        void Delete(int canalcodi, DateTime medifecha);
        MeScadaSp7DTO GetById(int canalcodi, DateTime medifecha);
        List<MeScadaSp7DTO> List();
        List<MeScadaSp7DTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string canalcodi);
        List<MeScadaSp7DTO> BuscarOperaciones(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin, int nroPage, int PageSize);
        int ObtenerNroFilas(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin);
        List<MeScadaSp7DTO> BuscarOperacionesReporte(bool ssee, int zonaCodi, DateTime mediFechaIni, DateTime mediFechaFin);
        List<MeScadaSp7DTO> GetDatosScadaAFormato(int formatcodi, int emprcodi, DateTime fechainicio, DateTime fechafin, string famcodis, string tipoinfocodis);
        List<MeScadaSp7DTO> GetByCriteria(DateTime fechainicio, DateTime fechafin, int formatcodi, int tipoinfocodi, int ptomedicodi);
        List<MeScadaSp7DTO> GetByCriteriaByPtoAndTipoinfocodi(DateTime fechainicio, DateTime fechafin, int tipoinfocodi, int ptomedicodi);

        List<MeScadaSp7DTO> ObtenerDatosSupervisionDemanda(DateTime fecha);
        bool SiExisteRegistroPorFechaYCanal(int canalcodi, DateTime fecha);
        void AgregarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque ,string usuario, decimal valor);
        void ActualizarRegistroPorFechaYBloque(int canalcodi, DateTime fecha, string bloque, string usuario, decimal valor);
        void ActualizarRegistrosNulosPorFechaYBloque(DateTime fecha, string bloque);

        #region Mejoras CMgN
        List<MeScadaSp7DTO> ObtenerComparativoDemanda(int cnfbarcodi, DateTime fechaInicio, DateTime fechaFin);
        MeScadaSp7DTO ObtenerDemandaPorInterconexion(DateTime fecha, int canalcodi);
        #endregion

        #region Informes SGI
        List<MeMedicion48DTO> ObtenerDatosPorReporte(int reporte, DateTime fecha, int tipoinfocodi);
        #endregion
    }
}
