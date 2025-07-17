using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VTD_VALORIZACIONDETALLE
    /// </summary>
    public interface IVtdValorizacionDetalleRepository
    {
        int Save(VtdValorizacionDetalleDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(VtdValorizacionDetalleDTO entity);
        void Delete(int Valdcodi);
        VtdValorizacionDetalleDTO GetById(int Valdcodi);
        List<VtdValorizacionDetalleDTO> List();
        List<VtdValorizacionDetalleDTO> GetByCriteria();
        int GetMaxId();

        #region Monto Por Energia
        /* Consulta para Obtener energia entregada */
        List<VtdValorizacionDetalleDTO> ObtenerEntregaParticipante(DateTime date);
        List<VtdValorizacionDetalleDTO> ObtenerEnergiaEntregadaByEmpresas(DateTime date, string participantes);
        /* Consulta para Obtener energia prevista */
        List<VtdValorizacionDetalleDTO> ObtenerEnergiaPrevista(DateTime date);
        decimal ObtenerEnergiaPrevistaRetirar(DateTime date, int emprcodi, int tpEPR);

        decimal ObtenerEnergiaPrevistaRetirarTotal(DateTime date, int tpEPR);

        #endregion

        #region Monto Por Capacidad
        /*Consulta para Obtener PotenciaFirmeRemunerable */
        List<VtdValorizacionDetalleDTO> ObtenerPotenciaFirmeRemunerable(int pericodi);
        /*5.3 Margen de Reserva -listo*/
        /*Consulta para obtener DemandaCoincidente*/
        List<VtdValorizacionDetalleDTO> ObtenerDemandaCoincidente(DateTime fechaIntervalo, int hora);
        /*Consulta para obtener PrecioPeaje*/
        decimal ObtenerPrecioPeaje(int pericodi);
        #endregion

        #region Monto por exceso de consumo de energ�a reactiva
        VtdValorizacionDetalleDTO GetValorizacionDetalleporFechaParticipante(DateTime date, int participante);

        #endregion
    }
}
