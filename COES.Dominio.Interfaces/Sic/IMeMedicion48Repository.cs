using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MEDICION48
    /// </summary>
    public interface IMeMedicion48Repository
    { 
        void Save(MeMedicion48DTO entity);
        void Update(MeMedicion48DTO entity);
        void Delete(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);
        MeMedicion48DTO GetById(int lectcodi, DateTime medifecha, int tipoinfocodi, int ptomedicodi);
        List<MeMedicion48DTO> List();
        List<MeMedicion48DTO> GetByCriteria();
        List<MeMedicion48DTO> ObtenerGeneracionRER(int idEmpresa, int lectCodi, DateTime fechaInicio, DateTime fechaFin);
        int EliminarValoresCargadosPorPunto(List<int> ptos, int lectcodi, DateTime fechaInicio, DateTime fechaFIn);
        int ObtenerEmpresaPorPuntoMedicion(int ptoMedicion);
        int ObtenerNroRegistrosEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas, string tiposGeneracion);
        List<MeMedicion48DTO> ObtenerConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas,
            string tiposGeneracion, int nroPagina, int nroRegistros);
        List<MeMedicion48DTO> ObtenerTotalConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal, string empresas,
            string tiposGeneracion);
        List<MeMedicion48DTO> ObtenerExportacionConsultaEjecutado(DateTime fechaInicial, DateTime fechaFinal,
            string empresas, string tiposGeneracion);
        List<MeMedicion48DTO> ObtenerConsolidadoEjecutado(DateTime fechaInicial, DateTime fechaFinal,
            string empresas, string fuentesEnergia);
        List<MeMedicion48DTO> ObtenerConsultaCMgRealPorArea(DateTime fechaInicio, DateTime fechaFin);
        void DeleteEnvioArchivo(int idLectura, DateTime fechaInicio, DateTime fechaFin, int idFormato, int idEmpresa);
        List<MeMedicion48DTO> GetEnvioArchivo(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi = -1);
        List<MeMedicion48DTO> GetEnvioArchivo2(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> GetByPtoMedicion(int ptomedicodi);
        List<MeMedicion48DTO> GetInterconexiones(int idLectura, int idOrigenLectura, string ptomedicodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerGeneracionPorEmpresa(DateTime fechaInicio, DateTime fechaFina);
        List<MeMedicion48DTO> ObtenerGeneracionPorEmpresaTipoGeneracion(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerGeneracionPorEmpresaTipoGeneracionMovil(DateTime fechaInicio, DateTime fechaFin);
        decimal ObtenerGeneracionAcumuladaAnual(DateTime fecha);
        List<MeMedicion48DTO> ObtenerConsultaWebReporte(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerDemandaPortalWeb(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerDemandaPortalWebTipo(int lectcodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerDemandaPicoDiaAnterior(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerDatosHidrologiaPortal(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerConsultaDemandaBarras(int lectcodi, string empresas, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> SqlObtenerDatosEjecutado(DateTime fecha);
        List<MeMedicion48DTO> GetListReporteInformacion30min(int formato, string inicio, string lectCodiPR16, string qEmpresa, string qTipoEmpresa,
            string fechaIni, string periodoSicli, string lectCodiAlpha, int regIni, int regFin);
        int GetListReporteInformacion30minCount(int formato, string inicio, string lectCodiPR16, string qEmpresa, string qTipoEmpresa, string fechaIni,
            string fechaFin, string lectCodiAlpha);
        List<MeMedicion48DTO> ObtenerDatosEquipoLectura(string equicodi, int lectcodi, string fecha);
        List<MeMedicion48DTO> ObtenerDatosPtoMedicionLectura(string ptomedicodi, int lectcodi, string fecha);
        List<MeMedicion48DTO> ObtenerDatosPtoMedicionLectura(string ptomedicodi, int lectcodi, int tipoinfocodi, string fecha);
        List<MeMedicion48DTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin, string lectcodi, int tipoinfocodi, string ptomedicodi);
        #region "COSTO OPORTUNIDAD"
        List<MeMedicion48DTO> GetDespachoProgramado(DateTime fecha, int lectcodi);
        List<MeMedicion48DTO> GetReservaProgramado(DateTime fecha, int lectcodi);
        List<MeMedicion48DTO> GetListadoReserva(DateTime fecha, int lectcodi);
        #endregion

        int SaveMeMedicion48Id(MeMedicion48DTO entity);
        List<MeMedicion48DTO> BuscarOperaciones(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate, int nroPage, int pageSize);
        int ObtenerNroFilas(int lectcodi, int tipoinfocodi, int ptomedicodi, DateTime medifecha, DateTime lastdate);

        #region PR5
        List<MeMedicion48DTO> ListarMeMedicion48ByFiltro(string lectcodi, DateTime fechaini, DateTime fechafin, string ptomedicodis, string tipoinfocodi = "-1");
        List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi);
        List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48SinGrupoIntegrante(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi, int tipoReporte);
        List<MeMedicion48DTO> GetConsolidadoMaximaDemanda48SinGrupoIntegrante(int tipoCentral, string tipoGeneracion, DateTime fechaIni, DateTime fechaFin, string idEmpresa, int lectcodi, int tipoinfocodi, int tptomedicodi);
        #endregion

        #region Indisponibilidades
        List<MeMedicion48DTO> GetIndisponibilidadesMedicion48Cuadro5(DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> GetIndisponibilidadesMedicion48Cuadro2(DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region SIOSEIN
        List<MeMedicion48DTO> GetCostosMarginalesProgPorRangoFechaStaRosa(DateTime fechaI, DateTime fechaF, int lectcodi);
        #endregion

        #region MigracionSGOCOES-GrupoB
        void SaveMemedicion48masivo(List<MeMedicion48DTO> entitys);
        void DeleteMasivo(int lectcodi, DateTime medifecha, string tipoinfocodi, string ptomedicodi);
        List<MeMedicion48DTO> RptCmgCortoPlazo(string lectcodi, int tipoinfocodi, int ptomedicodi, DateTime fecha1, DateTime fecha2);
        List<MeMedicion48DTO> GetListaDemandaBarras(string ptomedicodi, string lectcodi, DateTime fecInicio, DateTime fecFin);
        List<MeMedicion48DTO> GetListaMedicion48xlectcodi(int lectcodi);
        #endregion

        #region FIT - SGOCOES grupo A - Soporte

        void DeleteSCOActualizacion();

        #endregion

        List<MeMedicion48DTO> ObtenerProgramaReProgramaDia(DateTime fecha, int origlectcodi);

        #region Fit - VALORIZACION DIARIA     
        MeMedicion48DTO ObtenerIntervaloPuntaMes(DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region SIOSEIN 2
        List<MeMedicion48DTO> ObtenerListaMedicion48Ptomedicion(DateTime fechaInicio, DateTime fechaFin, string tipoinformacion, string lectocodi, string emprecodi);
        #endregion

        #region Numerales Datos Base        
        List<MeMedicion48DTO> ListaNumerales_DatosBase_5_8_4(string fechaIni, string fechaFin);
        List<MeMedicion48DTO> ListaNumerales_DatosBase_5_8_5(string fechaIni, string fechaFin);
        List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_1(string fechaIni, string fechaFin);
        List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_2(string fechaIni, string fechaFin);
        List<MeMedicion48DTO> ListaNumerales_DatosBase_5_9_3(string fechaIni, string fechaFin);
        List<MeMedicion48DTO> ListaMedUsuariosLibres(DateTime fechaIni, DateTime fechaFin);
        List<MeMedicion48DTO> ObtenerConsultaMedidores(DateTime fecInicio, DateTime fecFin, int nroPagina, object nroRegistros);
        int ObtenerNroElementosConsultaMedidores(DateTime fecInicio, DateTime fecFin);
        List<MeMedicion48DTO> ObtenerConsultaMedidores(DateTime fecInicio, DateTime fecFin);
        #endregion

        List<MeMedicion48DTO> ObtenerDatosDespachoComparativo(DateTime fechaInicio, DateTime fechaFin, string puntos, int lectcodi, int tipoinfocodi);

        #region Yupana Continuo

        List<MeMedicion48DTO> ObtenerAportesHidro(DateTime fechaIni, DateTime fechaFin, int topcodi, int idlecturaHidro);

        #endregion


        #region Mejoras RDO
        void SaveMeMedicion48Ejecutados(MeMedicion48DTO entity, int idEnvio, string usuario, int idEmpresa);
        List<MeMedicion48DTO> ObtenerGeneracionRERNC(int idEmpresa, int lectCodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> GetEnvioArchivoEjecutados(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi, string horario);
        void SaveMeMedicion48Intranet(MeMedicion48DTO entity, int idEnvio);
        List<MeMedicion48DTO> GetEnvioMeMedicion48Intranet(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int Lectcodi);
        List<MeMedicion48DTO> GetEnvioArchivoEjecutadosIntranet(int idFormato, DateTime fechaInicio, DateTime fechaFin, int lectocodi);
        List<MeMedicion48DTO> GetEnvioArchivoUltimoEjecutado(int idFormato, string idEmpresa, DateTime fechaInicio, DateTime fechaFin, int lectocodi, int enviocodi);
        #endregion

        #region Informes-SGI

        List<MeMedicion48DTO> ObtenerDatosPorReporte(int idReporte, DateTime fechaInicio, DateTime fechaFin, int tipoinfocodi);
        void SaveInfoAdicional(MeMedicion48DTO entity);
        List<MeMedicion48DTO> GetDemandaEjecutadaConEcuador(int lectcodi, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin, int lectcodiArea, int ptomedicodiL2280Ecuador);
        List<MeMedicion48DTO> GetDemandaCOESPtoMedicion48(int lectcodi, int tipoinfocodi, DateTime fechaInicio, DateTime fechaFin);
        List<MeMedicion48DTO> LeerMedidoresHidrologia(DateTime fecha);
        List<Medicion48DTO> ObtenerDemandaProgramadaDiariaAreas(DateTime dtFecha);
        List<Medicion48DTO> ObtenerProgramadaDiariaCOES(DateTime dtFecha);
        List<Medicion48DTO> ObtenerDemandaDiariaReal(DateTime fechaInicio);
        List<Medicion48DTO> LeerDemandaAreas(DateTime inicioFecha);

        #endregion

        #region Demanda PO
        List<MeMedicion48DTO> ObtenerDemandaGeneracionPO(string ptomedicodi, string fechaIni, string fechaFin);
        #endregion
    }
}
