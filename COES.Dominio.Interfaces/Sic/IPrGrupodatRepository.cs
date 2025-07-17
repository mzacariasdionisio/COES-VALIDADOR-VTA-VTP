using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GRUPODAT
    /// </summary>
    public interface IPrGrupodatRepository
    {
        void Save(PrGrupodatDTO entity);
        void SaveTransaccional(PrGrupodatDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(PrGrupodatDTO entity);
        PrGrupodatDTO GetById(DateTime fechadat, int concepcodi, int grupocodi, int deleted);
        List<PrGrupodatDTO> List();
        List<PrGrupodatDTO> GetByCriteria(int concepcodi);
        List<ConceptoDatoDTO> ListarDatosConceptoGrupoDat(int iGrupoCodi);
        List<PrGrupodatDTO> ListarHistoricoValores(string concepcodi, int grupocodi);
        List<PrGrupoConceptoDato> ObtenerDatosMO_URS(int iGrupoCodi, DateTime fechaRegistro);
        List<PrGrupoConceptoDato> ObtenerDatosMO_URS_Hidro(int iGrupoCodi, DateTime fechaRegistro);
        List<PrGrupoConceptoDato> ObtenerParametrosGeneralesUrs();
        List<PrGrupodatDTO> ParametrosPorFecha(DateTime fecha);
        List<PrGrupodatDTO> ParametrosGeneralesPorFecha(DateTime fecha);
        List<PrGrupodatDTO> ObtenerParametroPorCentral(string centrales);
        List<PrGrupodatDTO> ObtenerParametroPorConcepto(string concepCodi);
        List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustible(string empresas, DateTime fecha);
        List<PrGrupodatDTO> ObtenerParametroGeneral(DateTime fechaProceso);
        List<PrGrupodatDTO> ObtenerParametroModoOperacion(string idsGrupos, DateTime fechaProceso);
        void ActualizarParametro(int idGrupo, int concepcodi, string formula, string lastUser);
        // Inicio de Agregado - Sistema de Compensaciones
        List<PrGrupodatDTO> ListaModosOperacion(int ptoMediCodi, int pecacodi);
        List<PrGrupodatDTO> ListaCentral(int emprcodi);
        //DSH 19-06-2017 Inicio de Actualizacion
        List<PrGrupodatDTO> ListaGrupo(int emprcodi, int grupopadre);
        List<PrGrupodatDTO> ListaModo(int emprcodi, int grupopadre);
        // Fin de actualizacion
        List<PrGrupodatDTO> ListaCabecera(int periodo);
        IDataReader GetDataReaderCuerpo(int periodo);
        int GetGrupoCodi(string desc);
        List<PrGrupodatDTO> GetModosOperacion(string tipocatecodi);
        List<PrGrupodatDTO> ObtenerParametroPorModoOperacionPorFecha(string grupos, DateTime fechaDatos);
        List<PrGrupodatDTO> ListarAsignacionBarraModoOperacion(int grupocodi);
        // Fin de Agregado - Sistema de Compensaciones
        decimal GetValorModoOperacion(int grupoCodi, int concepCodi, DateTime fecha);

        #region NotificacionesCambiosEquipamiento
        /// <summary>
        /// Método que retorna el listado de conceptos (costos variables) que fueron modificados en el periodo consultado
        /// </summary>
        /// <param name="dtFechaInicio">Fecha Inicio</param>
        /// <param name="dtFechaFin">Fecha Fin</param>
        /// <returns>Listado de Conceptos actualizados</returns>
        List<PrGrupodatDTO> ListadoConceptosActualizados(DateTime dtFechaInicio, DateTime dtFechaFin);
        #endregion

        #region Curva Consumo
        List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustibleporGrupoCodi(string GrupoCodi, DateTime fecha);
        List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustibleporFecha(DateTime fecha);
        PrGrupodatDTO BuscaIDCurvaPrincipal(int curvcodi);
        string ObtenerFechaEdicionCurva(int grupoCodi);
        #endregion Curva Consumo

        #region MigracionSGOCOES-GrupoB
        List<PrGrupodatDTO> ListarParametrosActualizadosPorFecha(DateTime fecha);
        List<PrGrupodatDTO> ReporteControlCambios(DateTime fecha);
        #endregion

        #region SGOCOES func A                
        List<PrGrupodatDTO> ObtenerListaConfigScoSinac(DateTime fecha, int nroPage, int pageSize);
        int ObtenerTotalListaConfigScoSinac(DateTime fecha);

        #endregion

        #region FIT - VALORIZACION DIARIA
        PrGrupodatDTO ObtenerParametroValorizacion(DateTime fecha, int grupocodi, int concepcodi);
        #endregion

        #region Numerales Datos Base
        List<PrGrupodatDTO> ListaFechas_5_5_x(int stConcepcodi, string fechaIni);
        List<PrGrupodatDTO> ListaNumerales_DatosBase_5_5_n(int concepcCodi, string stiniVA, string fechaFin);
        List<PrGrupodatDTO> ListaNumerales_DatosBase_5_5_2(string stiniTC, string fechaFin);
        List<PrGrupodatDTO> ListaNumerales_DatosBase_5_6_4();
        #endregion

        #region SIOSEIN2
        List<PrGrupodatDTO> GetByCriteriaConceptoFecha(string concepcodi, DateTime fechaInicio, DateTime fechaFin);
        List<PrGrupodatDTO> ObtenerTodoParametroGeneral(DateTime fecha);
        List<PrGrupodatDTO> ObtenerTodoParametroModoOperacion(string grupocodi, DateTime fecha);
        #endregion

        #region Subastas
        List<PrGrupodatDTO> ParametrosConfiguracionPorFecha(DateTime fecha, string grupos, string conceptos);
        List<PrGrupodatDTO> BuscarEliminados(DateTime fechadat, int concepcodi, int grupocodi);
        #endregion

        #region FICHA TÉCNICA
        void Save(PrGrupodatDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(PrGrupodatDTO entity, IDbConnection connection, IDbTransaction transaction);

        List<PrGrupodatDTO> ListarGrupoConValorModificado(DateTime dtFechaInicio, DateTime dtFechaFin, string concepcodis, string catecodis);

        #endregion
    }
}
