using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;
using COES.Dominio.DTO.Sp7;

namespace COES.Dominio.Interfaces.Sp7
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_REPORTEVERSION_SP7
    /// </summary>
    public interface ITrReporteversionSp7Repository
    {
        int Save(TrReporteversionSp7DTO entity);
        void Update(TrReporteversionSp7DTO entity);
        void Delete(int revcodi);
        void DeleteVersion(int vercodi);
        TrReporteversionSp7DTO GetById(int revcodi);
        List<TrReporteversionSp7DTO> List();
        List<TrReporteversionSp7DTO> List(int verCodi);
        List<TrReporteversionSp7DTO> List(int verCodi, DateTime fechaIni, DateTime fechaFin);
        List<TrReporteversionSp7DTO> GetByCriteria();
        int SaveTrReporteversionSp7Id(TrReporteversionSp7DTO entity);
        List<TrReporteversionSp7DTO> BuscarOperaciones(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize);
        List<TrReporteversionSp7DTO> BuscarOperacionesResumen(int verCodi, DateTime fechaIni, DateTime fechaFin, int nroPage, int pageSize);
        int ObtenerNroFilas(int verCodi,DateTime revFeccreacion,DateTime revFecmodificacion);
        List<TrReporteversionSp7DTO> GetListaDispMensualVersion(int emprcodi, DateTime fecha);

        #region FIT - señales no disponibles
        TrReporteversionSp7DTO GetEmpresasDiasVersion(int emprcodi, DateTime fecha);
        bool GetCongelamientoSeñales(int emprcodi, DateTime fecha);
        bool ObtenerCaidaEnlace(int emprcodi, DateTime fecha, out string fechaDesconexion);

        #endregion

    }
}
