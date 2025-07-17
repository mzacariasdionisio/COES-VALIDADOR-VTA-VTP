using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IMeMedicion96Repository
    {
        void Save(MeMedicion96DTO entity);
        void SaveDemandaMercadoLibre(MeMedicion96DTO entity);

        void ActualizarMedicion(int lectcodi, DateTime fechahora, int tipoinfocodi, int ptomedicodi, int h, decimal? valor);

        List<MeMedicion96DTO> ObtenerConsultaMedidores(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante
            , int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int nroPagina, int nroRegistros, int tipoptomedicodi);

        List<MeMedicion96DTO> ObtenerConsultaServiciosAuxiliares(DateTime fechaIni, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante
            , int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int nroPagina, int nroRegistros, int tipoptomedicodi);

        List<MeMedicion96DTO> ObtenerExportacionConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        List<MeMedicion96DTO> ObtenerExportacionServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        List<MeMedicion96DTO> ObtenerTotalConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        List<MeMedicion96DTO> ObtenerTotalServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        int ObtenerNroElementosConsultaMedidores(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        int ObtenerNroElementosServiciosAuxiliares(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa
            , int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer, int tipoptomedicodi);

        List<MeMedicion96DTO> ListarTotalH(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central);

        List<MeMedicion96DTO> ListarDetalle(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central);

        List<MeMedicion96DTO> ObtenerReporteMedidores(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> ObtenerReporteMedidoresConsolidado(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> ObtenerDatosReporteMD(string empresas, int tipoGrupoCodi, string fuenteEnergia,
            DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> ObtenerMaximaDemandaPorRecursoEnergetico(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central);

        List<MeMedicion96DTO> ObtenerMaximaDemandaPorRecursoEnergeticoRER(DateTime fechaini, DateTime fechafin, string empresas,
            string tiposGeneracion, int central);

        DateTime? ObtenerFechaMinimaDemanda(DateTime fechaInicio, DateTime fechaFin, string empresas, string tiposGeneracion,
            int central);

        DateTime? ObtenerFechaMaximaDemanda(DateTime fechaInicio, DateTime fechaFin, string empresas, string tiposGeneracion,
            int central);

        List<MeMedicion96DTO> ObtenerEnvioPorEmpresa(int emprcodi, DateTime fechaPeriodo);

        void DeleteEnvioPorEmpresa(int emprcodi, DateTime fechaPeriodo);

        List<ConsolidadoEnvioDTO> ConsolidadoEnvioXEmpresa(int emprcodi, DateTime fechaPeriodo);

        List<MeMedicion96DTO> ObtenerEnvioInterconexion(int emprcodi, DateTime fechaini, DateTime fechafin);

        void DeleteEnvioInterconexion(DateTime medifecha);

        List<MeMedicion96DTO> ObtenerHistoricoInterconexion(int ptomedicodi, DateTime fechaini, DateTime fechafin);

        int ObtenerTotalHistoricoInterconexion(int ptomedicodi, DateTime fechaini, DateTime fechafin);

        List<MeMedicion96DTO> ObtenerHistoricoPagInterconexion(int ptomedicodi, DateTime fechaini, DateTime fechafin, int pagina);

        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa, DateTime fechaTiee);

        List<MeMedicion96DTO> GetEnvioArchivo(int idFormato, int idEmpresa, DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> GetEnvioArchivo(int idFormato, DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> GetByCriteria(int idTipoInformacion, int idPtoMedicion, int idLectura, DateTime fechaInicio,
            DateTime fechaFin);

        int ObtenerNroRegistrosMedDistribucion(string empresas, DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> ObtenerConsultaMedDistribucion(string empresas, DateTime fechaInicio, DateTime fechaFin, int nroPagina, int nroRegistros);

        List<MeMedicion96DTO> ObtenerTotalConsultaMedDistribucion(string empresas, DateTime fecInicio, DateTime fecFin);

        List<MeMedicion96DTO> ObtenerExportacionConsultaMedDistribucion(string empresas, DateTime fecInicio, DateTime fecFin);

        void Update(MeMedicion96DTO entity);

        MeMedicion96DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);

        List<MeMedicion96DTO> GetByPtoMedicion(int ptomedicodi);

        List<MeMedicion96DTO> ObtenerConsultaWeb(DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion48DTO> ObtenerConsultaWebReporte(DateTime fechaInicio, DateTime fechaFin);

        void Delete(int idPtomedicion, int idTipoInfo, DateTime fecha, int idLectura);

        List<DateTime> PaginacionInterconexiones(int idReporte, DateTime fechaInicio, DateTime fechaFin);

        List<MeMedicion96DTO> SqlObtenerDatosEjecutado(DateTime fecha);

        List<MeMedicion96DTO> ObtenerListaObservacionCoherenciaMensual(int enviocodiact, int enviocodiant, string fecIniAct, string fecFinAct, 
            string fecIniAnt, string fecFinAnt, int variacion);

        List<MeMedicion96DTO> ObtenerListaObservacionCoherenciaDiaria(int enviocodiact, int enviocodiant, string fecIniAct, string fecFinAct, 
            string fecIniAnt, string fecFinAnt, int variacion);
               
        List<MeMedicion96DTO> GetListReporteInformacion15min(int formato, string fechaIni, string fechaFin, string qEmpresa, string qTipoEmpresa,
            string qMaxDemanda, string lectCodiPR16, string lectCodiAlpha, int regIni, int regFin);

        int GetListReporteInformacion15minCount(int formato, string fechaIni, string fechaFin, string qEmpresa, string qTipoEmpresa,
            string qMaxDemanda, string lectCodiPR16, string lectCodiAlpha);

        void Delete(int idPtomedicion, int idTipoInfo, DateTime fechaInicio, DateTime fechaFin, int idLectura);

        List<MeMedicion96DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, int lectcodi, int tipoinfocodi, int ptomedicodi);

        //inicio agregado
        #region Modificacion PR15 - 24/11/2017
        List<ConsolidadoEnvioDTO> ConsolidadoEnvioByEmpresaAndFormato(int emprcodi, int lectcodi, int tipoinfocodi, DateTime fechaIni, DateTime fechaFin);
        #endregion
        void DeleteEnvioArchivo2(int idTptomedicodi, int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        List<MeMedicion96DTO> GetResumenMaximaDemanda(DateTime fechaInicio, DateTime fechaFin, int tipoCentral, string tipoGeneracion, string idEmpresa, int famcodiSSAA, string tipogrupocodiNoIntegrante, int lectcodi, int tipoinfocodi, string tipogrupocodiRer);
        List<MeMedicion96DTO> GetConsolidadoMaximaDemanda(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi);
        List<MeMedicion96DTO> GetConsolidadoExcesoPotenReact(int tipoCentral, int tipoGeneracion, string idEmpresa, string fechaIni, string fechaFin, int famcodiSSAA, int lectcodi, int tipoinfocodi, int tptomedicodi);
        //fin agregado

        #region Numerales Datos Base
        List<MeMedicion96DTO> ListaNumerales_DatosBase_5_1_1(string fechaIni, string fechaFin);
        #endregion

        #region FIT - VALORIZACION DIARIA

        List<MeMedicion96DTO> GetListFullInformacionPrevistaRPorParticipante(DateTime fechaInicio, DateTime fechaFinal, int emprcodi);
        List<MeMedicion96DTO> GetListPageByFilterInformacionPrevistaRPorParticipante(DateTime fechaInicio, DateTime fechaFinal, int emprcodi, int nroPage, int pageSize);

        #endregion

        List<MeMedicion96DTO> ObtenerDatosMedicionComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos, int lectcodi, int tipoInfoCodi);
    }
}
