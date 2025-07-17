using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDICIONXINTERVALO
    /// </summary>
    public interface IMeMedicionxintervaloRepository
    {
        void Save(MeMedicionxintervaloDTO entity);
        void Update(MeMedicionxintervaloDTO entity);
        MeMedicionxintervaloDTO GetById(int ptoMedicodi, DateTime fechaIni);
        List<MeMedicionxintervaloDTO> List();
        List<MeMedicionxintervaloDTO> GetByCriteria(int enviocodi);
        List<MeMedicionxintervaloDTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);
        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        List<MeMedicionxintervaloDTO> GetHidrologiaDescargaVert(int formatCodi, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicionxintervaloDTO> GetHidrologiaDescargaVertPag(int formatCodi, string idsEmpresa, DateTime fechaInicio, DateTime fechaFin, int nroPaginas, int pageSize);
        List<MeMedicionxintervaloDTO> GetListaMedxintervStock(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, string tptomedicodi);
        List<MeMedicionxintervaloDTO> GetListaMedxintervConsumo(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string ptomedicodi);
        List<MeMedicionxintervaloDTO> GetListaMedxintervDisponibilidad(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt, int idYacimGas, string idsYacimientos);        
        List<MeMedicionxintervaloDTO> GetListaMedxintervQuema(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string strCentralInt);
        List<MeMedicionxintervaloDTO> GetConsumoCentral(DateTime fecha, int idformato, int idempresa);
        void DeleteEnvioFormato(DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        void DeleteEnvioFormatoHojaColumna(DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa, int hoja, string sTptomedicion);
        List<MeMedicionxintervaloDTO> GetListaMedxintervStockPag(int lectCodi, int origlectcodi, string sEmprCodi, int famCodi, DateTime fechaInicial,
             DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string idsEquipo, int nroPaginas, int pageSize, string tptomedicodi);
        List<MeMedicionxintervaloDTO> List(int ptomedicodi, int lectcodi, int tipoinfocodi, DateTime fechaInicio,
            DateTime fechaFin);

        //- Agregado para PMPO
        int GetMaxId();
        void DeleteEnvioArchivo(int enviocodi);
        void Delete(int medintcodi);
        int SaveTransaccional(MeMedicionxintervaloDTO entity, IDbConnection connection, IDbTransaction transaction);
        List<MeMedicionxintervaloDTO> BuscarRegistroPeriodo(DateTime fechaini, DateTime fechafin, int ptomedicodi, int tipoinfocodi, int lectcodi);

        #region Pr31
        List<MeMedicionxintervaloDTO> GetCombustibleXCentral(DateTime fechaIni, DateTime fechaFin, string ptomedicion, int fenergcodi, int grupocodi);
        #endregion

        #region siosein2
        List<MeMedicionxintervaloDTO> GetListaMedicionXIntervaloByLecturaYTipomedicion(DateTime fechaPerido, DateTime fechaInicio, DateTime fechaFin, int lectcodi, string tptomedicodi, string ptomedicodi);
        List<MeMedicionxintervaloDTO> GetListaMedicionXIntervaloByLecturaYTipomedicionYCentral(DateTime fechaPerido, DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tptomedicodi, string ptomedicodi);
        #endregion

        #region PMPO 
        List<MeMedicionxintervaloDTO> ListarReporteGeneracionSDDP(int codigoenvio);
        List<MeMedicionxintervaloDTO> ListarReporteSDDP(int codigoenvio, string tptomedicodi, string ptomedicodi);
        #endregion

        #region FIT - VALORIZACION DIARIA      
        decimal GetDemandaMedianoPlazoCOES(int nWeek, DateTime date);
        #endregion

        #region Mejoras RDO
        List<MeMedicionxintervaloDTO> GetListaMedDisponibilidadCombustible(int lectCodi, int origlectcodi, string sEmprCodi, DateTime fechaInicial, DateTime fechaFinal, string idsEstado, string idsRecurso, string strCentralInt, string ptomedicodi, string horario);
        void SaveRDO(MeMedicionxintervaloDTO entity);
        List<MeMedicionxintervaloDTO> GetEnvioArchivoRDO(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin, string horario);
        #endregion

        #region PrimasRER.2023
        List<MeMedicionxintervaloDTO> ListarBarrasPMPO(string fechaInicio, string fechaFin);
        List<MeMedicionxintervaloDTO> ListarCentralesPMPO(int emprcodi);
        #endregion
    }
}
