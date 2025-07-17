using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ENVIO
    /// </summary>
    public interface IMeEnvioRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        int Save(MeEnvioDTO entity, IDbConnection conn, IDbTransaction tran);
		
		
		// ---------------------------------------- 04-04-2019 -----------------------------------------
        // Se recargo la función corrigiendo la logica de generación del correlativo
        // ---------------------------------------------------------------------------------------------
        int Save(MeEnvioDTO entity, IDbConnection conn, DbTransaction tran, int correlativoEnvio);
        // ---------------------------------------------------------------------------------------------
		
        int Save(MeEnvioDTO entity);
        void Update(MeEnvioDTO entity);
        void Update1(MeEnvioDTO entity);
        void Delete();
        MeEnvioDTO GetById(int idEnvio);
        List<MeEnvioDTO> List();
        List<MeEnvioDTO> GetByCriteria(int idEmpresa, int idFormato, DateTime fecha);
        List<MeEnvioDTO> GetByCriteriaRango(int idEmpresa, int idFormato, DateTime fechaIni, DateTime fechaFin);
        List<MeEnvioDTO> GetListaMultiple(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni,
            DateTime fechaFin, int nroPaginas, int pageSize);
        int TotalListaMultiple(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin);
        List<MeEnvioDTO> GetListaMultipleXLS(string idsEmpresa, string idsLectura, string idsFormato, string idsEstado, DateTime fechaIni, DateTime fechaFin);
        List<MeEnvioDTO> ObtenerReporteEnvioCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeEnvioDTO> GetByCriteriaRangoFecha(int idEmpresa, int idFormato, DateTime fechaini, DateTime fechafin);
        int GetMaxIdEnvioFormato(int idFormato, int idEmpresa);
        List<MeEnvioDTO> ObtenerReporteCumplimiento(string empresas, int formatCodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeEnvioDTO> ObtenerReporteCumplimientoXBloqueHorario(string empresas, string formatCodi, DateTime fechaInicio, DateTime fechaFin);
        int GetByMaxEnvioFormatoPeriodo(int idFormato, int idEmpresa, DateTime periodo);
        List<MeEnvioDTO> ObtenerListaEnvioActual(int empresa, string periodo);
        List<MeEnvioDTO> ObtenerListaPeriodoReporte(string fecha);
        IDataReader GetListReporteCumplimiento(int formato, string qEmpresa, string qTipoEmpresa, string fechaIni, string fechaFin, 
            string cumplimiento, string ulcoes, string abreviatura, string origen);
        List<MeEnvioDTO> ObtenerEnvioXModulo(int modcodi, int emprcodi, DateTime fecha);
        List<MeEnvioDTO> GetListaReporteCumplimientoHidrologia(string sAreas, string sEmpresas, DateTime fechaIni, DateTime fechaFin);

        #region SIOSEIN2 - NUMERALES

        List<MeEnvioDTO> ListaMeEnvioByFdat(int fdatcodi, DateTime fecha);

        #endregion

        #region ASSETEC - CAMBIOS 24/07/2018
        void EliminarFisico(int idEnvio,IDbConnection conn, DbTransaction tran);
        #endregion

        // Costo MArginales Revisaddos
        void Update2(MeEnvioDTO entity);

        void Update3(MeEnvioDTO entity);

        #region Aplicativo Extranet CTAF

        List<MeEnvioDTO> ObtenerEnvioInterrupSuministro(int emprcodi, int afecodi, int fdatcodi);

        List<MeEnvioDTO> ListaEnviosPorEvento(MeEnvioDTO entity);
        List<MeEnvioDTO> ListaInformeEnvios(int idEvento);
        List<MeEnvioDTO> ListaInformeEnviosLog(MeEnvioDTO entity);


        #endregion

        #region Mejoras RDO
        List<MeEnvioDTO> GetByCriteriaCaudalVolumen(int idFormato, DateTime fecha);
        void SaveHorario(int idEnvio, int horario);
        List<MeEnvioDTO> GetByCriteriaxHorario(int idEmpresa, int idFormato, DateTime fecha, int horario);
        #endregion
        #region Mejoras RDO-II
        //List<MeEnvioDTO> GetByCriteriaMeEnviosUltimoEjecutado(int idEmpresa, int idFormato, DateTime fecha, int horario);
        #endregion
    }
}
