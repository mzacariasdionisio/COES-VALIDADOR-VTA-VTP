using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Repositorio.Transferencias;

namespace COES.Servicios.Aplicacion.Factory
{
    public class FactoryTransferencia
    {
        public static string StrConexion = "ContextoSIC";
        public static string StrConexionDTR = "ContextoDTR";

        //VALORIZACIÓN DE TRANSFERENCIA DE ENERGÍA ACTIVA

        public static IAreaRepository GetAreaRepository()
        {
            return new AreaRespository(StrConexion);
        }

        public static IBarraRepository GetBarraRepository()
        {
            return new BarraRepository(StrConexion);
        }

        public static IBarraRelacionRepository GetBarraRelacionRepository()
        {
            return new BarraRelacionRepository(StrConexion);
        }

        public static ITrnBarraUrsRepository GetBarraUrsRepository()
        {
            return new TrnBarraUrsRepository(StrConexion);
        }

        public static ICentralGeneracionRepository GetCentralGeneracionRepository()
        {
            return new CentralGeneracionRepository(StrConexion);
        }

        public static ICodigoEntregaRepository GetCodigoEntregaRepository()
        {
            return new CodigoEntregaRepository(StrConexion);
        }

        public static ICodigoInfoBaseRepository GetCodigoInfoBaseRepository()
        {
            return new CodigoInfoBaseRepository(StrConexion);
        }

        public static ICodigoRetiroRepository GetCodigoRetiroRepository()
        {
            return new CodigoRetiroRepository(StrConexion);
        }
        public static ICodigoRetiroRelacionEquivalenciasRepository GetCodigoRetiroRelacionEquivalenciasRepository()
        {
            return new CodigoRetiroRelacionEquivalenciasRepository(StrConexion);
        }
        public static ICodigoRetiroEquivalenciaDetalleRepository GetCodigoRetiroEquivalenciaDetalleRepository()
        {
            return new CodigoRetiroEquivalenciaDetalleRepository(StrConexion);
        }
        public static ICodigoRetiroDetalleRepository GetCodigoRetiroDetalleRepository()
        {
            return new CodigoRetiroDetalleRepository(StrConexion);
        }
        public static ICodigoRetiroGeneradoRepository GetCodigoRetiroGeneradoRepository()
        {
            return new CodigoRetiroGeneradoRepository(StrConexion);
        }

        public static IVtpCodigoConsolidadoRepository GetVtpCodigoConsolidadoRepository()
        {
            return new VtpCodigoConsolidadoRepository(StrConexion);
        }

        public static ISolicitudCodigoRepository GetSolicitudCodigoRepository()
        {
            return new SolicitudCodigoRepository(StrConexion);
        }

        public static ISolicitudCodigoDetalleRepository GetSolicitudCodigoDetalleRepository()
        {
            return new SolicitudCodigoDetalleRepository(StrConexion);
        }

        public static ICodigoRetiroSinContratoRepository GetCodigoRetiroSinContratoRepository()
        {
            return new CodigoRetiroSinContratoRespository(StrConexion);
        }

        public static ICompensacionRepository GetCompensacionRepository()
        {
            return new CompensacionRepository(StrConexion);
        }

        public static ICostoMarginalRepository GetCostoMarginalRepository()
        {
            return new CostoMarginalRepository(StrConexion);
        }

        public static IEmpresaPagoRepository GetEmpresaPagoRepository()
        {
            return new EmpresaPagoRepository(StrConexion);
        }

        public static IEmpresaRepository GetEmpresaRepository()
        {
            return new EmpresaRepository(StrConexion);
        }

        public static IEnvioInformacionRepository GetIEnvioInformacionRepository()
        {
            return new ExportExcelRepository(StrConexion);
        }

        public static IExportExcelRepository GetExportExcelRepository()
        {
            return new ExportExcelGRepository(StrConexion);
        }

        public static IFactorPerdidaRepository GetFactorPerdidaRepository()
        {
            return new FactorPerdidaRepository(StrConexion);
        }

        public static IInfoDesbalanceRepository GetInfoDesbalanceRepository()
        {
            return new InfoDesbalanceRepository(StrConexion);
        }

        public static IInfoDesviacionRepository GetInfoDesviacionRepository()
        {
            return new InfoDesviacionRepository(StrConexion);
        }

        public static IInfoFaltanteRepository GetInfoFaltanteRepository()
        {
            return new InfoFaltanteRepository(StrConexion);
        }

        public static IIngresoCompensacionRepository GetIngresoCompensacionRepository()
        {
            return new IngresoCompensacionRepository(StrConexion);
        }

        public static IIngresoPotenciaRepository GetIngresoPotenciaRepository()
        {
            return new IngresoPotenciaRepository(StrConexion);
        }

        public static IIngresoRetiroSCRepository GetIngresoRetiroSCRepository()
        {
            return new IngresoRetiroSCRepository(StrConexion);
        }

        public static IPeriodoRepository GetPeriodoRepository()
        {
            return new PeriodoRepository(StrConexion);
        }

        public static IRatioCumplimientoRepository GetRatioCumplimientoRepository()
        {
            return new RatioCumplimientoRepository(StrConexion);
        }

        public static IRecalculoRepository GetRecalculoRepository()
        {
            return new RecalculoRepository(StrConexion);
        }

        public static ISaldoEmpresaRepository GetSaldoEmpresaRepository()
        {
            return new SaldoEmpresaRepository(StrConexion);
        }

        public static ITipoContratoRepository GetTipoContratoRepository()
        {
            return new TipoContratoRespository(StrConexion);
        }

        public static ITipoEmpresaRepository GetTipoEmpresaRepository()
        {
            return new TipoEmpresaRepository(StrConexion);
        }

        public static ITipoTramiteRepository GetTipoTramiteRepository()
        {
            return new TipoTramiteRepository(StrConexion);
        }

        public static ITipoUsuarioRepository GetTipoUsuarioRepository()
        {
            return new TipoUsuarioRespository(StrConexion);
        }

        public static ITramiteRepository GetTramiteRepository()
        {
            return new TramiteRepository(StrConexion);
        }

        public static ITransferenciaEntregaRepository GetTransferenciaEntregaRepository()
        {
            return new TransferenciaEntregaRepository(StrConexion);
        }

        public static ITransferenciaEntregaDetalleRepository GetTransferenciaEntregaDetalleRepository()
        {
            return new TransferenciaEntregaDetalleRepository(StrConexion);
        }

        public static ITransferenciaEntregaDetalleRepository GetTransferenciEntregaRetiroDetalleRepository()
        {
            return new TransferenciaEntregaDetalleRepository(StrConexion);
        }

        public static ITrnBarraUrsRepository GetTrnBarraUrsRepository()
        {
            return new TrnBarraUrsRepository(StrConexion);
        }

        //ASSETEC 20181224
        public static ITrnFactorPerdidaMediaRepository GetTrnFactorPerdidaMediaRepository()
        {
            return new TrnFactorPerdidaMediaRepository(StrConexion);
        }

        public static ITrnInfoadicionalRepository GetTrnInfoadicionalRepository()
        {
            return new TrnInfoadicionalRepository(StrConexion);
        }

        public static ITrnMedborneRepository GetTrnMedborneRepository()
        {
            return new TrnMedborneRepository(StrConexion);
        }

        //ASSETEC 202001
        /// <summary>
        /// Factory para los metodos de la clase TrnEnvioAppServicio
        /// </summary>
        public static ITrnEnvioRepository GetTrnEnvioRepository()
        {
            return new TrnEnvioRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos de la clase TrnPotenciaContratadaAppServicio
        /// </summary>
        public static ITrnPotenciaContratadaRepository GetTrnPotenciaContratadaRepository()
        {
            return new TrnPotenciaContratadaRepository(StrConexion);
        }
        /// <summary>
        /// Periodo de declaracion que permitira administa los codigos VTA/VTP por periodo
        /// </summary>
        public static IPeriodoDeclaracionRepository GetPeriodoDeclaracionRepository()
        {
            return new PeriodoDeclaracionRepository(StrConexion);
        }
        /// <summary>
        ///Factory saldos sobrantes
        /// </summary>
        public static IActualizarTrasEmpFusionRepository GetActualizarTrasEmpFusionRepository()
        {
            return new ActualizarTrasEmpFusionRepository(StrConexion);
        }
        // ASSETEC 2019-11
        /// <summary>
        /// Factory para los metodos de la clase TrnModeloAppServicio
        /// </summary>
        public static ITrnModeloRepository GetTrnModeloRepository()
        {
            return new TrnModeloRepository(StrConexion);
        }

        //ASSETEC 202108 TIEE
        /// <summary>
        /// Factory para los metodos de la clase TrnMigracion
        /// </summary>
        public static ITrnMigracionRepository GetTrnMigracionRepository()
        {
            return new TrnMigracionRepository(StrConexion);
        }

        #region VALORIZACIÓN DE TRANSFERENCIA DE POTENCIA Y COMPENSACIÓN

        public static ITransferenciaRetiroRepository GetTransferenciaRetiroRepository()
        {
            return new TransferenciaRetiroRepository(StrConexion);
        }

        public static ITransferenciaRetiroDetalleRepository GetTransferenciaRetiroDetalleRepository()
        {
            return new TransferenciaRetiroDetalleRepository(StrConexion);
        }

        public static IValorTransferenciaRepository GetValorTransferenciaRepository()
        {
            return new ValorTransferenciaRepository(StrConexion);
        }
        public static IValorTransferenciaEmpresaRepository GetValorTransferenciaEmpresaRepository()
        {
            return new ValorTransferenciaEmpresaRepository(StrConexion);
        }

        public static IValorTotalEmpresaRepository GetValorTotalEmpresaRepository()
        {
            return new ValorTotalEmpresaRepository(StrConexion);
        }

        public static ISaldoCodigoRetiroSCRepository GetSaldoCodigoRetiroSCRepository()
        {
            return new SaldoCodigoRetiroSCRepository(StrConexion);
        }

        public static ITransferenciaInformacionBaseRepository GetTransferenciaInformacionBaseRepository()
        {
            return new TransferenciaInformacionBaseRepository(StrConexion);
        }

        public static ITransferenciaInformacionBaseDetalleRepository GetTransferenciaInformacionBaseDetalleRepository()
        {
            return new TransferenciaInformacionBaseDetalleRepository(StrConexion);
        }

        public static ISaldoRecalculoRepository GetSaldoRecalculoRepository()
        {
            return new SaldoRecalculoRepository(StrConexion);
        }

        public static IVtpEmpresaPagoRepository GetVtpEmpresaPagoRepository()
        {
            return new VtpEmpresaPagoRepository(StrConexion);
        }

        public static IVtpIngresoPotefrRepository GetVtpIngresoPotefrRepository()
        {
            return new VtpIngresoPotefrRepository(StrConexion);
        }

        public static IVtpIngresoPotefrDetalleRepository GetVtpIngresoPotefrDetalleRepository()
        {
            return new VtpIngresoPotefrDetalleRepository(StrConexion);
        }

        public static IVtpIngresoPotenciaRepository GetVtpIngresoPotenciaRepository()
        {
            return new VtpIngresoPotenciaRepository(StrConexion);
        }

        public static IVtpIngresoPotUnidIntervlRepository GetVtpIngresoPotUnidIntervlRepository()
        {
            return new VtpIngresoPotUnidIntervlRepository(StrConexion);
        }

        public static IVtpIngresoPotUnidPromdRepository GetVtpIngresoPotUnidPromdRepository()
        {
            return new VtpIngresoPotUnidPromdRepository(StrConexion);
        }

        public static IVtpIngresoTarifarioRepository GetVtpIngresoTarifarioRepository()
        {
            return new VtpIngresoTarifarioRepository(StrConexion);
        }

        public static IVtpPagoEgresoRepository GetVtpPagoEgresoRepository()
        {
            return new VtpPagoEgresoRepository(StrConexion);
        }

        public static IVtpPeajeCargoRepository GetVtpPeajeCargoRepository()
        {
            return new VtpPeajeCargoRepository(StrConexion);
        }

        public static IVtpPeajeEgresoRepository GetVtpPeajeEgresoRepository()
        {
            return new VtpPeajeEgresoRepository(StrConexion);
        }

        public static IVtpPeajeEgresoDetalleRepository GetVtpPeajeEgresoDetalleRepository()
        {
            return new VtpPeajeEgresoDetalleRepository(StrConexion);
        }

        public static IVtpPeajeEgresoMinfoRepository GetVtpPeajeEgresoMinfoRepository()
        {
            return new VtpPeajeEgresoMinfoRepository(StrConexion);
        }

        public static IVtpPeajeEmpresaRepository GetVtpPeajeEmpresaRepository()
        {
            return new VtpPeajeEmpresaRepository(StrConexion);
        }

        public static IVtpPeajeEmpresaPagoRepository GetVtpPeajeEmpresaPagoRepository()
        {
            return new VtpPeajeEmpresaPagoRepository(StrConexion);
        }

        public static IVtpPeajeIngresoRepository GetVtpPeajeIngresoRepository()
        {
            return new VtpPeajeIngresoRepository(StrConexion);
        }

        public static IVtpPeajeSaldoTransmisionRepository GetVtpPeajeSaldoTransmisionRepository()
        {
            return new VtpPeajeSaldoTransmisionRepository(StrConexion);
        }

        public static IVtpRecalculoPotenciaRepository GetVtpRecalculoPotenciaRepository()
        {
            return new VtpRecalculoPotenciaRepository(StrConexion);
        }

        public static IVtpRepaRecaPeajeRepository GetVtpRepaRecaPeajeRepository()
        {
            return new VtpRepaRecaPeajeRepository(StrConexion);
        }

        public static IVtpRepaRecaPeajeDetalleRepository GetVtpRepaRecaPeajeDetalleRepository()
        {
            return new VtpRepaRecaPeajeDetalleRepository(StrConexion);
        }

        public static IVtpRetiroPotescRepository GetVtpRetiroPotescRepository()
        {
            return new VtpRetiroPotescRepository(StrConexion);
        }

        public static IVtpSaldoEmpresaRepository GetVtpSaldoEmpresaRepository()
        {
            return new VtpSaldoEmpresaRepository(StrConexion);
        }

        public static IRntConfiguracionRepository GetRntConfiguracionRepository()
        {
            return new RntConfiguracionRepository(StrConexion);
        }

        public static IRntEmpresaRegptoentregaRepository GetRntEmpresaRegptoentregaRepository()
        {
            return new RntEmpresaregptoentregaRepository(StrConexion);
        }

        public static IRntPeriodoRepository GetRntPeriodoRepository()
        {
            return new RntPeriodoRepository(StrConexion);
        }

        public static IRntRegPuntoEntregaRepository GetRntRegPuntoEntregaRepository()
        {
            return new RntRegpuntoentregaRepository(StrConexion);
        }

        public static IRntRegRechazoCargaRepository GetRntRegRechazoCargaRepository()
        {
            return new RntRegrechazocargaRepository(StrConexion);
        }

        public static IRntTipoInterrupcionRepository GetRntTipoInterrupcionRepository()
        {
            return new RntTipointerrupcionRepository(StrConexion);
        }

        public static IVtpSaldoEmpresaAjusteRepository GetVtpSaldoEmpresaAjusteRepository()
        {
            return new VtpSaldoEmpresaAjusteRepository(StrConexion);
        }

        public static IVtpIngresoTarifarioAjusteRepository GetVtpIngresoTarifarioAjusteRepository()
        {
            return new VtpIngresoTarifarioAjusteRepository(StrConexion);
        }

        public static IVtpPeajeCargoAjusteRepository GetVtpPeajeCargoAjusteRepository()
        {
            return new VtpPeajeCargoAjusteRepository(StrConexion);
        }

        public static IVtpPeajeEmpresaAjusteRepository GetVtpPeajeEmpresaAjusteRepository()
        {
            return new VtpPeajeEmpresaAjusteRepository(StrConexion);
        }

        /**Transferencias DTR**/

        public static IBarraRepository GetBarraDTRRepository()
        {
            return new BarraRepository(StrConexionDTR);
        }

        public static IPeriodoRepository GetPeriodoDTRRepository()
        {
            return new PeriodoRepository(StrConexionDTR);
        }

        public static IRecalculoRepository GetRecalculoDTRRepository()
        {
            return new RecalculoRepository(StrConexionDTR);
        }

        public static ICostoMarginalRepository GetCostoMarginalDTRRepository()
        {
            return new CostoMarginalRepository(StrConexionDTR);
        }

        public static IStCentralgenRepository GetStCentralgenRepository()
        {
            return new StCentralgenRepository(StrConexion);
        }

        public static IStCompensacionRepository GetStCompensacionRepository()
        {
            return new StCompensacionRepository(StrConexion);
        }

        public static IStCompmensualRepository GetStCompmensualRepository()
        {
            return new StCompmensualRepository(StrConexion);
        }

        public static IStCompmensualeleRepository GetStCompmensualeleRepository()
        {
            return new StCompmensualeleRepository(StrConexion);
        }

        public static IStDistelectricaRepository GetStDistelectricaRepository()
        {
            return new StDistelectricaRepository(StrConexion);
        }

        public static IStDistelectricaGeneleRepository GetStDistelectricaGeneleRepository()
        {
            return new StDistelectricaGeneleRepository(StrConexion);
        }

        public static IStDsteleBarraRepository GetStDsteleBarraRepository()
        {
            return new StDsteleBarraRepository(StrConexion);
        }

        public static IStElementoCompensadoRepository GetStElementoCompensadoRepository()
        {
            return new StElementoCompensadoRepository(StrConexion);
        }

        public static IStEnergiaRepository GetStEnergiaRepository()
        {
            return new StEnergiaRepository(StrConexion);
        }

        public static IStFactorRepository GetStFactorRepository()
        {
            return new StFactorRepository(StrConexion);
        }

        public static IStFactorpagoRepository GetStFactorpagoRepository()
        {
            return new StFactorpagoRepository(StrConexion);
        }

        public static IStGeneradorRepository GetStGeneradorRepository()
        {
            return new StGeneradorRepository(StrConexion);
        }

        public static IStPeriodoRepository GetStPeriodoRepository()
        {
            return new StPeriodoRepository(StrConexion);
        }

        public static IStRecalculoRepository GetStRecalculoRepository()
        {
            return new StRecalculoRepository(StrConexion);
        }

        public static IStRespagoRepository GetStRespagoRepository()
        {
            return new StRespagoRepository(StrConexion);
        }

        public static IStRespagoeleRepository GetStRespagoeleRepository()
        {
            return new StRespagoeleRepository(StrConexion);
        }

        public static IStSistematransRepository GetStSistematransRepository()
        {
            return new StSistematransRepository(StrConexion);
        }

        public static IStTransmisorRepository GetStTransmisorRepository()
        {
            return new StTransmisorRepository(StrConexion);
        }

        public static IStBarraRepository GetStBarraRepository()
        {
            return new StBarraRepository(StrConexion);
        }

        internal static IStPagoasignadoRepository GetStPagoasignadoRepository()
        {
            return new StPagoasignadoRepository(StrConexion);
        }

        #endregion

        #region COMPENSACION POR REGULACION SECUNDARIA DE FRECUENCIA

        public static IVcrAsignacionpagoRepository GetVcrAsignacionpagoRepository()
        {
            return new VcrAsignacionpagoRepository(StrConexion);
        }

        public static IVcrAsignacionreservaRepository GetVcrAsignacionreservaRepository()
        {
            return new VcrAsignacionreservaRepository(StrConexion);
        }

        public static IVcrCargoincumplRepository GetVcrCargoincumplRepository()
        {
            return new VcrCargoincumplRepository(StrConexion);
        }

        public static IVcrCmpensoperRepository GetVcrCmpensoperRepository()
        {
            return new VcrCmpensoperRepository(StrConexion);
        }

        public static IVcrCostoportdetalleRepository GetVcrCostoportdetalleRepository()
        {
            return new VcrCostoportdetalleRepository(StrConexion);
        }

        public static IVcrCostoportunidadRepository GetVcrCostoportunidadRepository()
        {
            return new VcrCostoportunidadRepository(StrConexion);
        }

        public static IVcrCostvariableRepository GetVcrCostvariableRepository()
        {
            return new VcrCostvariableRepository(StrConexion);
        }

        public static IVcrDespachoursRepository GetVcrDespachoursRepository()
        {
            return new VcrDespachoursRepository(StrConexion);
        }

        public static IVcrEmpresarsfRepository GetVcrEmpresarsfRepository()
        {
            return new VcrEmpresarsfRepository(StrConexion);
        }

        public static IVcrMedborneRepository GetVcrMedborneRepository()
        {
            return new VcrMedborneRepository(StrConexion);
        }

        public static IVcrMedbornecargoincpRepository GetVcrMedbornecargoincpRepository()
        {
            return new VcrMedbornecargoincpRepository(StrConexion);
        }

        public static IVcrOfertaRepository GetVcrOfertaRepository()
        {
            return new VcrOfertaRepository(StrConexion);
        }

        public static IVcrPagorsfRepository GetVcrPagorsfRepository()
        {
            return new VcrPagorsfRepository(StrConexion);
        }

        public static IVcrProvisionbaseRepository GetVcrProvisionbaseRepository()
        {
            return new VcrProvisionbaseRepository(StrConexion);
        }

        public static IVcrRecalculoRepository GetVcrRecalculoRepository()
        {
            return new VcrRecalculoRepository(StrConexion);
        }

        public static IVcrReduccpagoejeRepository GetVcrReduccpagoejeRepository()
        {
            return new VcrReduccpagoejeRepository(StrConexion);
        }

        public static IVcrReservasignRepository GetVcrReservasignRepository()
        {
            return new VcrReservasignRepository(StrConexion);
        }

        public static IVcrServiciorsfRepository GetVcrServiciorsfRepository()
        {
            return new VcrServiciorsfRepository(StrConexion);
        }

        public static IVcrTermsuperavitRepository GetVcrTermsuperavitRepository()
        {
            return new VcrTermsuperavitRepository(StrConexion);
        }

        public static IVcrUnidadexoneradaRepository GetVcrUnidadexoneradaRepository()
        {
            return new VcrUnidadexoneradaRepository(StrConexion);
        }

        public static IVcrVerdeficitRepository GetVcrVerdeficitRepository()
        {
            return new VcrVerdeficitRepository(StrConexion);
        }

        public static IVcrVerporctreservRepository GetVcrVerporctreservRepository()
        {
            return new VcrVerporctreservRepository(StrConexion);
        }

        public static IVcrVerincumplimRepository GetVcrVerincumplimRepository()
        {
            return new VcrVerincumplimRepository(StrConexion);
        }

        public static IVcrVerrnsRepository GetVcrVerrnsRepository()
        {
            return new VcrVerrnsRepository(StrConexion);
        }

        public static IVcrVersiondsrnsRepository GetVcrVersiondsrnsRepository()
        {
            return new VcrVersiondsrnsRepository(StrConexion);
        }

        public static IVcrVersionincplRepository GetVcrVersionincplRepository()
        {
            return new VcrVersionincplRepository(StrConexion);
        }

        public static IVcrVersuperavitRepository GetVcrVersuperavitRepository()
        {
            return new VcrVersuperavitRepository(StrConexion);
        }

        #endregion

        #region CALCULO DE LOS PORCENTAJES DE LOS APORTES ANULAES DE LOS INTEGRANTES DEL COES

        public static ICaiAjusteRepository GetCaiAjusteRepository()
        {
            return new CaiAjusteRepository(StrConexion);
        }

        public static ICaiAjustecmarginalRepository GetCaiAjustecmarginalRepository()
        {
            return new CaiAjustecmarginalRepository(StrConexion);
        }

        public static ICaiAjusteempresaRepository GetCaiAjusteempresaRepository()
        {
            return new CaiAjusteempresaRepository(StrConexion);
        }

        public static ICaiEquisddpbarrRepository GetCaiEquisddpbarrRepository()
        {
            return new CaiEquisddpbarrRepository(StrConexion);
        }

        public static ICaiEquiunidbarrRepository GetCaiEquiunidbarrRepository()
        {
            return new CaiEquiunidbarrRepository(StrConexion);
        }

        public static ICaiGenerdemanRepository GetCaiGenerdemanRepository()
        {
            return new CaiGenerdemanRepository(StrConexion);
        }

        public static ICaiImpgeneracionRepository GetCaiImpgeneracionRepository()
        {
            return new CaiImpgeneracionRepository(StrConexion);
        }

        public static ICaiIngtransmisionRepository GetCaiIngtransmisionRepository()
        {
            return new CaiIngtransmisionRepository(StrConexion);
        }

        public static ICaiMaxdemandaRepository GetCaiMaxdemandaRepository()
        {
            return new CaiMaxdemandaRepository(StrConexion);
        }

        public static ICaiPorctaporteRepository GetCaiPorctaporteRepository()
        {
            return new CaiPorctaporteRepository(StrConexion);
        }

        public static ICaiPresupuestoRepository GetCaiPresupuestoRepository()
        {
            return new CaiPresupuestoRepository(StrConexion);
        }

        public static ICaiSddpDuracionRepository GetCaiSddpDuracionRepository()
        {
            return new CaiSddpDuracionRepository(StrConexion);
        }

        public static ICaiSddpGenmargRepository GetCaiSddpGenmargRepository()
        {
            return new CaiSddpGenmargRepository(StrConexion);
        }

        public static ICaiSddpParamsemRepository GetCaiSddpParamsemRepository()
        {
            return new CaiSddpParamsemRepository(StrConexion);
        }

        public static ICaiSddpParamdiaRepository GetCaiSddpParamdiaRepository()
        {
            return new CaiSddpParamdiaRepository(StrConexion);
        }

        public static ICaiSddpParametroRepository GetCaiSddpParametroRepository()
        {
            return new CaiSddpParametroRepository(StrConexion);
        }

        public static ICaiSddpParamintRepository GetCaiSddpParamintRepository()
        {
            return new CaiSddpParamintRepository(StrConexion);
        }

        //ASSETEC - 20181218

        public static ICaiEquisddpuniRepository GetCaiEquisddpuniRepository()
        {
            return new CaiEquisddpuniRepository(StrConexion);
        }

        #endregion

        public static IDemandaMercadoLibreRepository GetDemandaMercadoLibreRepository()
        {
            return new DemandaMercadoLibreRepository(StrConexion);
        }

        public static ITransferenciaRentaCongestionRepository GetTransferenciaRentaCongestionRepository()
        {
            return new TransferenciaRentaCongestionRepository(StrConexion);
        }

        public static DatosTransferenciaRepository GetDatosTransferenciaRepository()
        {
            return new DatosTransferenciaRepository(StrConexion);
        }

        public static IRcgCostoMarginalCabRepository GetRcgCostoMarginalCabRepository()
        {
            return new RcgCostoMarginalCab(StrConexion);
        }

        public static ITrnModeloEnvioRepository GetTrnModeloEnvioRepository()
        {
            return new TrnModeloEnvioRepository(StrConexion);
        }

        #region FIT - VALORIZACION DIARIA

        public static IVtdMontoPorEnergiaRepository GetVtdMontoPorEnergiaRepository()
        {
            return new VtdMontoPorEnergiaRepository(StrConexion);
        }

        public static IVtdMontoPorCapacidadRepository GetVtdMontoPorCapacidadRepository()
        {
            return new VtdMontoPorCapacidadRepository(StrConexion);
        }

        public static IVtdMontoPorPeajeRepository GetVtdMontoPorPeajeRepository()
        {
            return new VtdMontoPorPeajeRepository(StrConexion);
        }

        public static IVtdMontoSCeIORepository GetVtdMontoSCeIORepository()
        {
            return new VtdMontoSCeIORepository(StrConexion);
        }

        public static IVtdMontoPorExcesoRepository GetVtdMontoPorExcesoRepository()
        {
            return new VtdMontoPorExcesoRepository(StrConexion);
        }

        public static IVtdCargosEneReacRepository GetVtdCargosEneReacRepository()
        {
            return new VtdCargosEneReacRepository(StrConexion);
        }

        public static IVtdValorizacionRepository GetVtdValorizacion()
        {
            return new VtdValorizacionRepository(StrConexion);
        }

        public static IVtdValorizacionDetalleRepository GetVtdValorizacionDetalle()
        {
            return new VtdValorizacionDetalleRepository(StrConexion);
        }

        public static IVtdLogProcesoRepository GetVtdLogProceso()
        {
            return new VtdLogProcesoRepository(StrConexion);
        }

        public static IValorizacionDiariaRepository GetValorizacionDiaria()
        {
            return new ValorizacionDiariaRepository(StrConexion);
        }

        #endregion

        public static IVtpVariacionEmpresaRepository GetVtpVariacionEmpresaRepository()
        {
            return new VtpVariacionEmpresaRepository(StrConexion);
        }

        public static IVtpVariacionCodigoRepository GetVtpVariacionCodigoRepository()
        {
            return new VtpVariacionCodigoRepository(StrConexion);
        }

        public static IVtpValidacionEnvioRepository GetVtpValidacionEnvioRepository()
        {
            return new VtpValidacionEnvioRepository(StrConexion);
        }

        public static IVtpTipoAplicacionRepository GetVtpTipoAplicacionRepository()
        {
            return new VtpTipoAplicacionRepository(StrConexion);
        }

        public static IVtpAuditoriaProcesoRepository GetVtpAuditoriaProcesoRepository()
        {
            return new VtpAuditoriaProcesoRepository(StrConexion);
        }
        public static IVtpTipoProcesoRepository GetVtpTipoProcesoRepository()
        {
            return new VtpTipoProcesoRepository(StrConexion);
        }

        #region ASSETEC - Costo marginal ajuste de intervalos
        public static ITrnCostoMarginalAjusteRepository GetTrnCostoMarginalAjusteRepository()
        {
            return new TrnCostoMarginalAjusteRepository(StrConexion);
        }
        #endregion

        /// <summary>
        /// Factory para los metodos de la clase TrnConfiguracionPmme
        /// </summary>
        public static ITrnConfiguracionPmmeRepository GetTrnConfiguracionPmmeRepository()
        {
            return new TrnConfiguracionPmmeRepository(StrConexion);
        }
        /// <summary>
        /// Factory para los metodos de la clase TrnDemanda
        /// </summary>
        public static ITrnDemandaRepository GetTrnDemandaRepository()
        {
            return new TrnDemandaRepository(StrConexion);
        }
        /// <summary>
        /// Factory para los metodos de la clase TrnPeriodoCna
        /// </summary>
        public static ITrnPeriodoCnaRepository GetTrnPeriodoCnaRepository()
        {
            return new TrnPeriodoCnaRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos de la clase TrnConsumoNoAutorizado
        /// </summary>
        public static ITrnConsumoNoAutorizadoRepository GetTrnConsumoNoAutorizadoRepository()
        {
            return new TrnConsumoNoAutorizadoRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos de la clase TrnContadorCorreosCna
        /// </summary>
        public static ITrnContadorCorreosCnaRepository GetTrnContadorCorreosCnaRepository()
        {
            return new TrnContadorCorreosCnaRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos de la clase TrnLogCna
        /// </summary>
        public static ITrnLogCnaRepository GetTrnLogCnaRepository()
        {
            return new TrnLogCnaRepository(StrConexion);
        }

        #region PrimasRER.2023
        public static IRerCentralCodRetiroRepository GetRerCentralCodRetiroRepository()
        {
            return new RerCentralCodRetiroRepository(StrConexion);
        }

        public static IRerCentralLvtpRepository GetRerCentralLvtpRepository()
        {
            return new RerCentralLvtpRepository(StrConexion);
        }

        public static IRerCentralPmpoRepository GetRerCentralPmpoRepository()
        {
            return new RerCentralPmpoRepository(StrConexion);
        }

        public static IRerCentralRepository GetRerCentralRepository()
        {
            return new RerCentralRepository(StrConexion);
        }

        public static IRerComparativoCabRepository GetRerComparativoCabRepository()
        {
            return new RerComparativoCabRepository(StrConexion);
        }

        public static IRerComparativoDetRepository GetRerComparativoDetRepository()
        {
            return new RerComparativoDetRepository(StrConexion);
        }

        public static IRerEnergiaUnidadRepository GetRerEnergiaUnidadRepository()
        {
            return new RerEnergiaUnidadRepository(StrConexion);
        }

        public static IRerEvaluacionEnergiaUnidadRepository GetRerEvaluacionEnergiaUnidadRepository()
        {
            return new RerEvaluacionEnergiaUnidadRepository(StrConexion);
        }

        public static IRerEvaluacionRepository GetRerEvaluacionRepository()
        {
            return new RerEvaluacionRepository(StrConexion);
        }

        public static IRerEvaluacionSolicitudEdiRepository GetRerEvaluacionSolicitudEdiRepository()
        {
            return new RerEvaluacionSolicitudEdiRepository(StrConexion);
        }

        public static IRerFacPerMedDetRepository GetRerFacPerMedDetRepository()
        {
            return new RerFacPerMedDetRepository(StrConexion);
        }

        public static IRerFacPerMedRepository GetRerFacPerMedRepository()
        {
            return new RerFacPerMedRepository(StrConexion);
        }

        public static IRerOrigenRepository GetRerOrigenRepository()
        {
            return new RerOrigenRepository(StrConexion);
        }

        public static IRerParametroPrimaRepository GetRerParametroPrimaRepository()
        {
            return new RerParametroPrimaRepository(StrConexion);
        }

        public static IRerRevisionRepository GetRerRevisionRepository()
        {
            return new RerRevisionRepository(StrConexion);
        }

        public static IRerSolicitudEdiRepository GetRerSolicitudEdiRepository()
        {
            return new RerSolicitudEdiRepository(StrConexion);
        }

        public static IRerEnergiaUnidadDetRepository GetRerEnergiaUnidadDetRepository()
        {
            return new RerEnergiaUnidadDetRepository(StrConexion);
        }

        public static IRerEvaluacionEnergiaUnidDetRepository GetRerEvaluacionEnergiaUnidDetRepository()
        {
            return new RerEvaluacionEnergiaUnidDetRepository(StrConexion);
        }

        // RER Segunda Iteracción
        public static IRerInsumoRepository GetRerInsumoRepository()
        {
            return new RerInsumoRepository(StrConexion);
        }

        public static IRerInsumoDiaRepository GetRerInsumoDiaRepository()
        {
            return new RerInsumoDiaRepository(StrConexion);
        }

        public static IRerInsumoMesRepository GetRerInsumoMesRepository()
        {
            return new RerInsumoMesRepository(StrConexion);
        }

        public static IRerCalculoAnualRepository GetRerCalculoAnualRepository()
        {
            return new RerCalculoAnualRepository(StrConexion);
        }

        public static IRerCalculoMensualRepository GetRerCalculoMensualRepository()
        {
            return new RerCalculoMensualRepository(StrConexion);
        }

        public static IRerAnioVersionRepository GetRerAnioVersionRepository()
        {
            return new RerAnioVersionRepository(StrConexion);
        }

        public static IRerParametroRevisionRepository GetRerParametroRevisionRepository()
        {
            return new RerParametroRevisionRepository(StrConexion);
        }

        public static IRerSddpRepository GetRerSddpRepository()
        {
            return new RerSddpRepository(StrConexion);
        }

        public static IRerGerCsvRepository GetRerGerCsvRepository()
        {
            return new RerGerCsvRepository(StrConexion);
        }

        public static IRerGerCsvDetRepository GetRerGerCsvDetRepository()
        {
            return new RerGerCsvDetRepository(StrConexion);
        }

        public static IRerInsumoVtpRepository GetRerInsumoVtpRepository()
        {
            return new RerInsumoVtpRepository(StrConexion);
        }

        public static IRerInsumoVteaRepository GetRerInsumoVteaRepository()
        {
            return new RerInsumoVteaRepository(StrConexion);
        }
        #endregion

        #region CPPA.2024
        //Iteracion 2: Inicio
        public static ICpaDocumentosRepository GetCpaDocumentosRepository()
        {
            return new CpaDocumentosRepository(StrConexion);
        }

        public static ICpaDocumentosDetalleRepository GetCpaDocumentosDetalleRepository()
        {
            return new CpaDocumentosDetalleRepository(StrConexion);
        }

        public static ICpaPorcentajeRepository GetCpaPorcentajeRepository()
        {
            return new CpaPorcentajeRepository(StrConexion);
        }

        public static ICpaPorcentajeEnvioRepository GetCpaPorcentajeEnvioRepository()
        {
            return new CpaPorcentajeEnvioRepository(StrConexion);
        }

        public static ICpaPorcentajeEnergiaPotenciaRepository GetCpaPorcentajeEnergiaPotenciaRepository()
        {
            return new CpaPorcentajeEnergiaPotenciaRepository(StrConexion);
        }

        public static ICpaPorcentajeMontoRepository GetCpaPorcentajeMontoRepository()
        {
            return new CpaPorcentajeMontoRepository(StrConexion);
        }

        public static ICpaPorcentajePorcentajeRepository GetCpaPorcentajePorcentajeRepository()
        {
            return new CpaPorcentajePorcentajeRepository(StrConexion);
        }
        //Iteracion 2: Fin

        public static ICpaParametroHistoricoRepository GetCpaParametroHistoricoRepository()
        {
            return new CpaParametroHistoricoRepository(StrConexion);
        }

        public static ICpaAjustePresupuestalRepository GetCpaAjustePresupuestalRepository()
        {
            return new CpaAjustePresupuestalRepository(StrConexion);
        }

        public static ICpaCalculoCentralRepository GetCpaCalculoCentralRepository()
        {
            return new CpaCalculoCentralRepository(StrConexion);
        }

        public static ICpaCalculoEmpresaRepository GetCpaCalculoEmpresaRepository()
        {
            return new CpaCalculoEmpresaRepository(StrConexion);
        }

        public static ICpaCalculoRepository GetCpaCalculoRepository()
        {
            return new CpaCalculoRepository(StrConexion);
        }

        public static ICpaCentralPmpoRepository GetCpaCentralPmpoRepository()
        {
            return new CpaCentralPmpoRepository(StrConexion);
        }

        public static ICpaCentralRepository GetCpaCentralRepository()
        {
            return new CpaCentralRepository(StrConexion);
        }

        public static ICpaEmpresaRepository GetCpaEmpresaRepository()
        {
            return new CpaEmpresaRepository(StrConexion);
        }

        public static ICpaGercsvDetRepository GetCpaGercsvDetRepository()
        {
            return new CpaGercsvDetRepository(StrConexion);
        }

        public static ICpaGercsvRepository GetCpaGercsvRepository()
        {
            return new CpaGercsvRepository(StrConexion);
        }

        public static ICpaHistoricoRepository GetCpaHistoricoRepository()
        {
            return new CpaHistoricoRepository(StrConexion);
        }

        public static ICpaInsumoDiaRepository GetCpaInsumoDiaRepository()
        {
            return new CpaInsumoDiaRepository(StrConexion);
        }

        public static ICpaInsumoMesRepository GetCpaInsumoMesRepository()
        {
            return new CpaInsumoMesRepository(StrConexion);
        }

        public static ICpaInsumoRepository GetCpaInsumoRepository()
        {
            return new CpaInsumoRepository(StrConexion);
        }

        public static ICpaParametroRepository GetCpaParametroRepository()
        {
            return new CpaParametroRepository(StrConexion);
        }
        public static ICpaRevisionRepository GetCpaRevisionRepository()
        {
            return new CpaRevisionRepository(StrConexion);
        }

        public static ICpaSddpRepository GetCpaSddpRepository()
        {
            return new CpaSddpRepository(StrConexion);
        }

        public static ICpaTotalDemandaRepository GetCpaTotalDemandaRepository()
        {
            return new CpaTotalDemandaRepository(StrConexion);
        }

        public static ICpaTotalDemandaDetRepository GetCpaTotalDemandaDetRepository()
        {
            return new CpaTotalDemandaDetRepository(StrConexion);
        }

        public static ICpaTotalTransmisoresRepository GetCpaTotalTransmisoresRepository()
        {
            return new CpaTotalTransmisoresRepository(StrConexion);
        }

        public static ICpaTotalTransmisoresDetRepository GetCpaTotalTransmisoresDetRepository()
        {
            return new CpaTotalTransmisoresDetRepository(StrConexion);
        }
        #endregion 

        //GESPROTEC - 20241029
        #region GESPROTEC
        public static Dominio.Interfaces.Sic.IEqAreaRepository GetEqAreaRepository()
        {
            return new Infraestructura.Datos.Repositorio.Sic.EqAreaRepository(StrConexion);
        }
        #endregion

    }
}
