using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_INTERVENCION
    /// </summary>
    public interface IInIntervencionRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);

        #region Métodos de Intervenciones (Carga de datos)

        int GetMaxId();
        void Save(InIntervencionDTO entity, IDbConnection conn, DbTransaction tran);

        int DeshabilitarIntervencionesRecepcion(int progrcodi, IDbConnection conn, DbTransaction tran);
        int DeshabilitarIntervencionesEnReversion(int progrcodi, IDbConnection conn, DbTransaction tran);
        int DesabilitarIntervencion(string intercodi, IDbConnection conn, DbTransaction tran);
        void UpdateIsFiles(string intercodi);
        void UpdateTieneMensaje(string intercodi);
        void UpdateEstadoMensajeCOES(string intercodi, int estado);
        void UpdateEstadoMensajeAgente(string intercodi, int estado);

        InIntervencionDTO GetById(int interCodi);
        InIntervencionDTO GetByCodigoPadre(string intercodipadre);

        List<InIntervencionDTO> ListarIntervencionCandidatoPorCriterio(int equicodi, int tipoevencodi, DateTime fechaIni, DateTime fechaFin, int progrcodi);
        List<InIntervencionDTO> ExisteCodigoSeguimiento(string codigoSeguimiento);

        List<InIntervencionDTO> ConsultarIntervenciones(int idProgramacion, string idTipoProgramacion, string strIdsEmpresa, string strIdsTipoIntervencion, 
                                            string strIdsAreas, string strIdsFamilias, string strIndisponible, string strIdsEstados, int flagEliminado, int flagAprobado,
                                            DateTime fechaIni, DateTime fechaFin, string strIdEquipo = "0");        
        
        List<InIntervencionDTO> ConsultarTrazabilidad(int interCodi, int tipoProgramacion, string interCodSegEmpr);
        
        List<InIntervencionDTO> ConsultarIntervencionesXIds(string intercodis);

        int DeshabilitarIntervencionEnReversion(int progrcodi, IDbConnection conn, DbTransaction tran);

        int HabilitarIntervencionesRevertidas(int progrcodi, IDbConnection conn, DbTransaction tran);

        int DeshabilitarIntervencionEliminadaRechazada(int intercodi, int interprocesado, IDbConnection conn, DbTransaction tran);
        int RecuperarIntervencion(int intercodi, int interprocesado, IDbConnection conn, DbTransaction tran);

        void UpdateLeidoAgente(string intercodi);

        List<InIntervencionDTO> ListarIntervencionesSinArchivo(int progrcodi);

        #endregion

        #region Métodos Para Reportes
        #region Anexos Programacion Anual
        List<InIntervencionDTO> ListadoIntervencionesMayores(int idProgAnual, string strIdsEmpresa, string strIntersistemaAislado, string strInterinterrup, string strIndisponible);
        List<InIntervencionDTO> ReporteCruzadoIntervencionesMayoresGeneracionESFS(int idProgAnual, string strIdsEmpresa, int anio, int mes, string strIndispo);
        List<InIntervencionDTO> ReporteCruzadoIntervencionesMayoresTransmisionESFS(int idProgAnual, string strIdsEmpresa, int anio, int mes, string strIndispo);
        #endregion

        #region Intervenciones Mayores
        List<InIntervencionDTO> ReporteIntervencionesMayoresPorPeriodo(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Intervenciones Importantes
        List<InIntervencionDTO> ReporteIntervencionesImportantes(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Eventos
        List<InIntervencionDTO> ReporteEventos(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region Conexiones Provisionales
        List<InIntervencionDTO> ReporteConexionesProvisionales(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Sistemas Aislados
        List<InIntervencionDTO> ReporteSistemasAislados(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Interrupcion o Restriccion de Suministros
        List<InIntervencionDTO> ReporteInterrupcionRestriccionSuministros(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Agentes
        List<InIntervencionDTO> ReporteAgentes(int progrCodi, int idTipoProgramacion, string strIdsEmpresa, DateTime? fechaInicio, DateTime? fechaFin);
        #endregion

        #region Proc - 25 OSINERGMIN
        List<InIntervencionDTO> ReporteOSINERGMINProc257dListado();
        #endregion

        #region Intervenciones Formato OSINERGMIN
        List<InIntervencionDTO> ReporteIntervenciones(int idTipoProgramacion, string StrIdsEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<InIntervencionDTO> ListaMantenimientos25(int evenclasecodi, string evenclasedesc, DateTime fechaini, DateTime fechafin);
        List<InIntervencionDTO> ReporteIntervencionesOsinergmin(int progrCodi, int idTipoProgramacion, string StrIdsEmpresa, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region F1F2
        #region PARA INTERVENCIONES PROGRAMADAS
        List<InIntervencionDTO> ReporteIntervencionesF1F2Programados(int anio, int mes);
        bool BuscarEjecutadoPorCodSeguimiento(string codSeguimiento);
        #endregion

        #region PARA INTERVENCIONES EJECUTADAS
        List<InIntervencionDTO> ReporteIntervencionesF1F2Ejecutados(int anio, int mes);
        bool BuscarMensualProgramadoPorCodSeguimiento(string codSeguimiento);
        #endregion
        #endregion

        #endregion

        #region Métodos para Validación con Aplicativos
        List<InIntervencionDTO> ListarIntervencionesEquiposGen(DateTime dFechaInicio, DateTime dFechaFin, int famcodiGenerador, int famcodiCentral);
        #endregion

        #region Seguridad y permisos usuarios
        int UpdateUserPermiso(int userCodi, int valor);
        int ObtenerFlagUserPermiso(int userCodi);
        int ConsultarPermisos(string userlogin);
        #endregion

        #region Yupana - Portal

        List<InIntervencionDTO> ListarIntervencionesXPagina(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas);
        
        int ObtenerNroRegistrosPaginado(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);
        
        List<InIntervencionDTO> ListarIntervencionesGrafico(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
                    string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);
        List<InIntervencionDTO> ConsultarIntervencionesCruzadasTIITR(DateTime dFechaInicio, DateTime dFechaFin);

        #endregion
    }
}
