using COES.Dominio.Interfaces.Sic;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Repositorio.Sic;
using COES.Infraestructura.Datos.Repositorio.Transferencias;
using EmpresaRepository = COES.Infraestructura.Datos.Repositorio.Sic.EmpresaRepository;
using IEmpresaRepository = COES.Dominio.Interfaces.Scada.IEmpresaRepository;


namespace COES.Servicios.Aplicacion.Factory
{
    /// <summary>
    /// Clase que permite crear objetos repositorio
    /// </summary>
    public class FactorySic
    {
        public static string StrConexion = "ContextoSIC";
        public static string StrConexionSCADA = "ContextoSCADA";

        public static IEmpresaRepository GetEmpresaRepository()
        {
            return new EmpresaRepository(StrConexion);
        }

        public static EventoRepository ObtenerEventoDao()
        {
            return new EventoRepository(StrConexion);
        }

        public static CriteriosEventoRepositoy ObtenerCriterioEventoDao()
        {
            return new CriteriosEventoRepositoy(StrConexion);
        }

        

        public static IEveSubeventosRepository GetEveSubeventosRepository()
        {
            return new EveSubeventosRepository(StrConexion);
        }

        public static IEveInterrupcionRepository GetEveInterrupcionRepository()
        {
            return new EveInterrupcionRepository(StrConexion);
        }

        public static IEvePtointerrupRepository GetEvePtointerrupRepository()
        {
            return new EvePtointerrupRepository(StrConexion);
        }

        public static IEvePtoentregaRepository GetEvePtoentregaRepository()
        {
            return new EvePtoentregaRepository(StrConexion);
        }

        public static ISiEmpresaRepository GetSiEmpresaRepository()
        {
            return new SiEmpresaRepository(StrConexion);
        }

        public static TipoEventoRepository ObtenerTipoEventoDao()
        {
            return new TipoEventoRepository(StrConexion);
        }

        public static SubCausaEventoRepository ObtenerSubCausaEventoDao()
        {
            return new SubCausaEventoRepository(StrConexion);
        }

        public static IRpfEnergiaPotenciaRepository GetRPFhidraulicoRepository()
        {
            return new RpfEnergiaPotenciaRepository(StrConexion);
        }

        public static EveInformePerturbacionRepository ObtenerInformePerturbacionDao()
        {
            return new EveInformePerturbacionRepository(StrConexion);
        }

        public static EjecutadoRuleRepository ObtenerEjecutadoRuleDao()
        {
            return new EjecutadoRuleRepository(StrConexion);
        }

        public static ScadaRepository ObtenerScadaSICDao()
        {
            return new ScadaRepository(StrConexion);
        }

        public static ScadaRepository ObtenerScadaTRDao()
        {
            return new ScadaRepository(StrConexionSCADA);
        }

        public static ServicioRpfRepository ObtenerServicioRpfDao()
        {
            return new ServicioRpfRepository(StrConexion);
        }

        public static MePtorelacionRepository GetMePtorelacionRepository()
        {
            return new MePtorelacionRepository(StrConexion);
        }

        public static IeodCuadroRepository ObtenerIeodCuadroDao()
        {
            return new IeodCuadroRepository(StrConexion);
        }

        public static ParametroRpfRepository ObtenerParametroRpfDao()
        {
            return new ParametroRpfRepository(StrConexion);
        }

        public static IEqFamiliaRepository GetEqFamiliaRepository()
        {
            return new EqFamiliaRepository(StrConexion);
        }

        public static IMeMedicion48Repository GetMeMedicion48Repository()
        {
            return new MeMedicion48Repository(StrConexion);
        }

        public static IMeMedicion96Repository GetMeMedicion96Repository()
        {
            return new MeMedicion96Repository(StrConexion);
        }

        public static IDesviacionRepository GetDesviacionRepositoryOracle()
        {
            return new DesviacionRepository(StrConexion);
        }

        public static IPrCombustibleRepository GetPrCombustibleRepository()
        {
            return new PrCombustibleRepository(StrConexion);
        }

        public static IPrGrupoRepository GetPrGrupoRepository()
        {
            return new PrGrupoRepository(StrConexion);
        }

        public static IPrCategoriaRepository GetPrCategoriaRepository()
        {
            return new PrCategoriaRepository(StrConexion);
        }

        public static IPrCurvaRepository GetPrAgruparCurvaRepository()
        {
            return new PrCurvaRepository(StrConexion);
        }

        public static IEqEquipoRepository GetEqEquipoRepository()
        {
            return new EqEquipoRepository(StrConexion);
        }

        public static ISiFuenteenergiaRepository GetSiFuenteenergiaRepository()
        {
            return new SiFuenteenergiaRepository(StrConexion);
        }

        public static IMePerfilRuleRepository GetMePerfilRuleRepository()
        {
            return new MePerfilRuleRepository(StrConexion);
        }

        public static IMePerfilRuleRepository GetMePerfilRuleScadaRepository()
        {
            return new MePerfilRuleRepository(StrConexionSCADA);
        }

        public static IMePerfilRuleAreaRepository GetMePerfilRuleAreaRepository()
        {
            return new MePerfilRuleAreaRepository(StrConexion);
        }

        public static ISiTipoempresaRepository GetSiTipoempresaRepository()
        {
            return new SiTipoempresaRepository(StrConexion);
        }

        public static IEveCausaeventoRepository GetEveCausaeventoRepository()
        {
            return new EveCausaeventoRepository(StrConexion);
        }

        public static IPrTipogrupoRepository GetPrTipogrupoRepository()
        {
            return new PrTipogrupoRepository(StrConexion);
        }

        public static IWbGeneracionrerRepository GetWbGeneracionrerRepository()
        {
            return new WbGeneracionrerRepository(StrConexion);
        }

        public static IManRegistroRepository GetManRegistroRepository()
        {
            return new ManRegistroRepository(StrConexion);
        }

        public static IEqAreaRepository GetEqAreaRepository()
        {
            return new EqAreaRepository(StrConexion);
        }

        public static IEqArearelRepository GetEqArearelRepository()
        {
            return new EqArearelRepository(StrConexion);
        }

        public static IEqAreanivelRepository GetEqAreanivelRepository()
        {
            return new EqAreanivelRepository(StrConexion);
        }

        public static IEqEquirelRepository GetEqEquirelRepository()
        {
            return new EqEquirelRepository(StrConexion);
        }

        public static IEqFamrelRepository GetEqFamrelRepository()
        {
            return new EqFamrelRepository(StrConexion);
        }

        public static IEqPropequiRepository GetEqPropequiRepository()
        {
            return new EqPropequiRepository(StrConexion);
        }

        public static IEqPropiedadRepository GetEqPropiedadRepository()
        {
            return new EqPropiedadRepository(StrConexion);
        }

        public static IEqTipoareaRepository GetEqTipoareaRepository()
        {
            return new EqTipoareaRepository(StrConexion);
        }

        public static IEqTiporelRepository GetEqTiporelRepository()
        {
            return new EqTiporelRepository(StrConexion);
        }

        public static IManManttoRepository GetManManttoRepository()
        {
            return new ManManttoRepository(StrConexion);
        }

        public static IExtLogenvioRepository GetExtLogenvioRepository()
        {
            return new ExtLogenvioRepository(StrConexion);
        }

        public static ISiTipogeneracionRepository GetSiTipogeneracionRepository()
        {
            return new SiTipogeneracionRepository(StrConexion);
        }

        public static IPrConceptoRepository GetPrConceptoRepository()
        {
            return new PrConceptoRepository(StrConexion);
        }

        public static IPrGrupodatRepository GetPrGrupodatRepository()
        {
            return new PrGrupodatRepository(StrConexion);
        }

        public static IPrEquipodatRepository GetPrEquipodatRepository()
        {
            return new PrEquipodatRepository(StrConexion);
        }

        public static IPrEscenarioRepository GetPrEscenarioRepository()
        {
            return new PrEscenarioRepository(StrConexion);
        }

        public static IPrRepcvRepository GetPrRepcvRepository()
        {
            return new PrRepcvRepository(StrConexion);
        }

        public static IPrCvariablesRepository GetPrCvariablesRepository()
        {
            return new PrCvariablesRepository(StrConexion);
        }

        public static ISiPersonaRepository GetSiPersonaRepository()
        {
            return new SiPersonaRepository(StrConexion);
        }

        public static IEveEvenclaseRepository GetEveEvenclaseRepository()
        {
            return new EveEvenclaseRepository(StrConexion);
        }

        public static IEveManttoRepository GetEveManttoRepository()
        {
            return new EveManttoRepository(StrConexion);
        }

        public static IWbMedidoresValidacionRepository GetWbMedidoresValidacionRepository()
        {
            return new WbMedidoresValidacionRepository(StrConexion);
        }

        public static IMeMedicion1Repository GetMeMedicion1Repository()
        {
            return new MeMedicion1Repository(StrConexion);
        }

        public static IEveEventoRepository GetEveEventoRepository()
        {
            return new EveEventoRepository(StrConexion);
        }

        public static IEveSubcausaFamiliaRepository GetEveSubcausaFamiliaRepository()
        {
            return new EveSubcausaFamiliaRepository(StrConexion);
        }

        public static IEveMailsRepository GetEveMailsRepository()
        {
            return new EveMailsRepository(StrConexion);
        }

        public static IEveIeodcuadroRepository GetEveIeodcuadroRepository()
        {
            return new EveIeodcuadroRepository(StrConexion);
        }

        public static IEveIeodcuadroDetRepository GetEveIeodcuadroDetRepository()
        {
            return new EveIeodcuadroDetRepository(StrConexion);
        }

        public static IEvePaleatoriaRepository GetEvePaleatoriaRepository()
        {
            return new EvePaleatoriaRepository(StrConexion);
        }
        public static IEvePaleatoriaRepository ListaCoordinadores()
        {
            return new EvePaleatoriaRepository(StrConexion);
        }

        public static IEveInformefallaRepository GetEveInformefallaRepository()
        {
            return new EveInformefallaRepository(StrConexion);
        }

        public static IEveInformefallaN2Repository GetEveInformefallaN2Repository()
        {
            return new EveInformefallaN2Repository(StrConexion);
        }

        public static IEveTipoeventoRepository GetEveTipoeventoRepository()
        {
            return new EveTipoeventoRepository(StrConexion);
        }

        public static IEveSubcausaeventoRepository GetEveSubcausaeventoRepository()
        {
            return new EveSubcausaeventoRepository(StrConexion);
        }

        public static IEveEvenequipoRepository GetEveEvenequipoRepository()
        {
            return new EveEvenequipoRepository(StrConexion);
        }

        public static IEveInformeRepository GetEveInformeRepository()
        {
            return new EveInformeRepository(StrConexion);
        }

        public static IEveInformeFileRepository GetEveInformeFileRepository()
        {
            return new EveInformeFileRepository(StrConexion);
        }

        public static IEveInformeItemRepository GetEveInformeItemRepository()
        {
            return new EveInformeItemRepository(StrConexion);
        }

        public static IEveEventoLogRepository GetEveEventoLogRepository()
        {
            return new EveEventoLogRepository(StrConexion);
        }

        public static ISiPruebaRepository GetSiPruebaRepository()
        {
            return new SiPruebaRepository(StrConexion);
        }

        ///////// Combustibles /////////

        public static IExtEstadoEnvioRepository GetExtEstadoEnvioRepository()
        {
            return new ExtEstadoEnvioRepository(StrConexion);
        }

        #region PR31

        public static ICbArchivoenvioRepository GetCbArchivoenvioRepository()
        {
            return new CbArchivoenvioRepository(StrConexion);
        }

        public static ICbConceptocombRepository GetCbConceptocombRepository()
        {
            return new CbConceptocombRepository(StrConexion);
        }

        public static ICbDatosRepository GetCbDatosRepository()
        {
            return new CbDatosRepository(StrConexion);
        }

        public static ICbDatosDetalleRepository GetCbDatosDetalleRepository()
        {
            return new CbDatosDetalleRepository(StrConexion);
        }

        public static ICbEnvioRepository GetCbEnvioRepository()
        {
            return new CbEnvioRepository(StrConexion);
        }

        public static ICbLogenvioRepository GetCbLogenvioRepository()
        {
            return new CbLogenvioRepository(StrConexion);
        }

        public static ICbVersionRepository GetCbVersionRepository()
        {
            return new CbVersionRepository(StrConexion);
        }

        public static ICbCentralxfenergRepository GetCbCentralxfenergRepository()
        {
            return new CbCentralxfenergRepository(StrConexion);
        }

        public static ICbDatosxcentralxfenergRepository GetCbDatosxcentralxfenergRepository()
        {
            return new CbDatosxcentralxfenergRepository(StrConexion);
        }

        public static ICbEnviorelcvRepository GetCbEnviorelcvRepository()
        {
            return new CbEnviorelcvRepository(StrConexion);
        }

        public static ICbFichaRepository GetCbFichaRepository()
        {
            return new CbFichaRepository(StrConexion);
        }

        public static ICbFichaItemRepository GetCbFichaItemRepository()
        {
            return new CbFichaItemRepository(StrConexion);
        }

        public static ICbEnvioCentralRepository GetCbEnvioCentralRepository()
        {
            return new CbEnvioCentralRepository(StrConexion);
        }

        public static ICbObsRepository GetCbObsRepository()
        {
            return new CbObsRepository(StrConexion);
        }

        public static ICbObsxarchivoRepository GetCbObsxarchivoRepository()
        {
            return new CbObsxarchivoRepository(StrConexion);
        }

        public static ICbReporteCentralRepository GetCbReporteCentralRepository()
        {
            return new CbReporteCentralRepository(StrConexion);
        }

        public static ICbReporteDetalleRepository GetCbReporteDetalleRepository()
        {
            return new CbReporteDetalleRepository(StrConexion);
        }

        public static ICbReporteRepository GetCbReporteRepository()
        {
            return new CbReporteRepository(StrConexion);
        }

        public static ICbNotaRepository GetCbNotaRepository()
        {
            return new CbNotaRepository(StrConexion);
        }

        public static ICbRepCabeceraRepository GetCbRepCabeceraRepository()
        {
            return new CbRepCabeceraRepository(StrConexion);
        }

        public static ICbRepPropiedadRepository GetCbRepPropiedadRepository()
        {
            return new CbRepPropiedadRepository(StrConexion);
        }

        #endregion

        #region Ensayo
        public static IEnEnsayoRepository GetEnEnsayoRepository()
        {
            return new EnEnsayoRepository(StrConexion);
        }

        public static IEnEnsayoformatoRepository GetEnEnsayoformatoRepository()
        {
            return new EnEnsayoformatoRepository(StrConexion);
        }

        public static IEnEnsayounidadRepository GetEnEnsayounidadRepository()
        {
            return new EnEnsayounidadRepository(StrConexion);
        }

        public static IEnEnsayomodequiRepository GetEnEnsayomodequiRepository()
        {
            return new EnEnsayomodequiRepository(StrConexion);
        }

        public static IEnEnsayomodoRepository GetEnEnsayomodoRepository()
        {
            return new EnEnsayomodoRepository(StrConexion);
        }

        public static IEnFormatoRepository GetEnFormatoRepository()
        {
            return new EnFormatoRepository(StrConexion);
        }

        public static IEnEstensayoRepository GetEnEstensayoRepository()
        {
            return new EnEstensayoRepository(StrConexion);
        }

        public static IEnEstadosRepository GetEnEstadosRepository()
        {
            return new EnEstadoRepository(StrConexion);
        }

        public static IEnEstformatoRepository GetEnEstformatoRepository()
        {
            return new EnEstformatoRepository(StrConexion);
        }

        public static IEnEstformatoRepository GetEnEstformatoRepositori()
        {
            return new EnEstformatoRepository(StrConexion);
        }

        #endregion

        public static IMeFormatoRepository GetMeFormatoRepository()
        {
            return new MeFormatoRepository(StrConexion);
        }

        public static IMeFormatohojaRepository GetMeFormatohojaRepository()
        {
            return new MeFormatohojaRepository(StrConexion);
        }

        public static IMeHojaptomedRepository GetMeHojaptomedRepository()
        {
            return new MeHojaptomedRepository(StrConexion);
        }

        public static IMeAmpliacionfechaRepository GetMeAmpliacionfechaRepository()
        {
            return new MeAmpliacionfechaRepository(StrConexion);
        }

        public static IMeHeadcolumnRepository GetMeHeadcolumnRepository()
        {
            return new MeHeadcolumnRepository(StrConexion);
        }

        public static IMePtomedicionRepository GetMePtomedicionRepository()
        {
            return new MePtomedicionRepository(StrConexion);
        }

        public static IMeTipopuntomedicionRepository GetMeTipopuntomedicionRepository()
        {
            return new MeTipopuntomedicionRepository(StrConexion);
        }

        public static IMePeriodomedidorRepository GetMePeriodomedidorRepository()
        {
            return new MePeriodomedidorRepository(StrConexion);
        }

        public static IMeMedidorRepository GetMeMedidorRepository()
        {
            return new MeMedidorRepository(StrConexion);
        }

        public static IExtArchivoRepository GetExtArchivoRepository()
        {
            return new ExtArchivoRepository(StrConexion);
        }

        public static IFLecturaRepository GetFLecturaRepository()
        {
            return new FLecturaRepository(StrConexion);
        }

        public static ISiAuditoriaRegistroRepository GetSiAuditoriaRegistroRepository()
        {
            return new SiAuditoriaregistroRepository(StrConexion);
        }

        public static ISiTablaAuditableRepository GetSiTablaAuditableRepository()
        {
            return new SiTablaAuditableRepository(StrConexion);
        }
        public static IFIndicadorRepository GetFIndicadorRepository()
        {
            return new FIndicadorRepository(StrConexion);
        }

        public static IPsuRpfhidRepository GetPsuRpfhidRepository()
        {
            return new PsuRpfhidRepository(StrConexion);
        }

        public static IInfArchivoAgenteRepository GetInfArchivoAgenteRepository()
        {
            return new InfArchivoAgenteRepository(StrConexion);
        }

        public static IMeEnvioRepository GetMeEnvioRepository()
        {
            return new MeEnvioRepository(StrConexion);
        }

        public static ISHCaudalRepository GetSHCaudalRepository()
        {
            return new SHCaudalRepository(StrConexion);
        }

        public static IMeEnviodetRepository GetMeEnviodetRepository()
        {
            return new MeEnviodetRepository(StrConexion);
        }

        public static ISiFuentedatosRepository GetSiFuentedatosRepository()
        {
            return new SiFuentedatosRepository(StrConexion);
        }

        public static IMeJustificacionRepository GetMeJustificacionRepository()
        {
            return new MeJustificacionRepository(StrConexion);
        }

        public static IMeEstadoenvioRepository GetMeEstadoenvioRepository()
        {
            return new MeEstadoenvioRepository(StrConexion);
        }

        public static IMeArchivoRepository GetMeArchivoRepository()
        {
            return new MeArchivoRepository(StrConexion);
        }

        public static IMeLogenvioRepository GetMeLogenvioRepository()
        {
            return new MeLogenvioRepository(StrConexion);
        }

        public static IMeMensajeenvioRepository GetMeMensajeenvioRepository()
        {
            return new MeMensajeenvioRepository(StrConexion);
        }

        public static IMeCambioenvioRepository GetMeCambioenvioRepository()
        {
            return new MeCambioenvioRepository(StrConexion);
        }

        public static IMeModuloRepository GetMeModuloRepository()
        {
            return new MeModuloRepository(StrConexion);
        }

        //// Reportes hidrologia
        public static IMeReporteRepository GetMeReporteRepository()
        {
            return new MeReporteRepository(StrConexion);
        }

        public static IMeReporptomedRepository GetMeReporptomedRepository()
        {
            return new MeReporptomedRepository(StrConexion);
        }

        public static ISiTipoinformacionRepository GetSiTipoinformacionRepository()
        {
            return new SiTipoinformacionRepository(StrConexion);
        }

        public static IMeMedicion24Repository GetMeMedicion24Repository()
        {
            return new MeMedicion24Repository(StrConexion);
        }

        public static IMeOrigenlecturaRepository GetMeOrigenlecturaRepository()
        {
            return new MeOrigenlecturaRepository(StrConexion);
        }

        public static IMeLecturaRepository GetMeLecturaRepository()
        {
            return new MeLecturaRepository(StrConexion);
        }

        public static IFwCounterRepository GetFwCounterRepository()
        {
            return new FwCounterRepository(StrConexion);
        }

        public static IFwAreaRepository GetFwAreaRepository()
        {
            return new FwAreaRepository(StrConexion);
        }

        public static IExtLogproRepository GetExtLogproRepository()
        {
            return new ExtLogproRepository(StrConexion);
        }

        //// Costo Marginal Real

        public static IPsuDesvcmgRepository GetPsuDesvcmgRepository()
        {
            return new PsuDesvcmgRepository(StrConexion);
        }

        public static IPsuDesvcmgsncRepository GetPsuDesvcmgsncRepository()
        {
            return new PsuDesvcmgsncRepository(StrConexion);
        }

        public static IMeGpsRepository GetMeGpsRepository()
        {
            return new MeGpsRepository(StrConexion);
        }

        public static IMeCabeceraRepository GetCabeceraRepository()
        {
            return new MeCabeceraRepository(StrConexion);
        }

        public static ISiCambioTurnoRepository GetSiCambioTurnoRepository()
        {
            return new SiCambioTurnoRepository(StrConexion);
        }

        public static ISiCambioTurnoSeccionRepository GetSiCambioTurnoSeccionRepository()
        {
            return new SiCambioTurnoSeccionRepository(StrConexion);
        }

        public static ISiCambioTurnoSubseccionRepository GetSiCambioTurnoSubseccionRepository()
        {
            return new SiCambioTurnoSubseccionRepository(StrConexion);
        }

        public static ISiCambioTurnoAuditRepository GetSiCambioTurnoAuditRepository()
        {
            return new SiCambioTurnoAuditRepository(StrConexion);
        }

        public static IDocDiaEspRepository GetDocDiaEspRepository()
        {
            return new DocDiaEspRepository(StrConexion);
        }

        public static IInInterconexionRepository GetInInterconexionRepository()
        {
            return new InInterconexionRepository(StrConexion);
        }

        public static IWbComunicadosRepository GetWbComunicadosRepository()
        {
            return new WbComunicadosRepository(StrConexion);
        }

        public static IWbConvocatoriasRepository GetWbConvocatoriasRepository()
        {
            return new WbConvocatoriasRepository(StrConexion);
        }

        public static IPortalInformacionRepository GetPortalInformacionRepository()
        {
            return new PortalInformacionRepository(StrConexion);
        }

        public static IMeMedicionxintervaloRepository GetMeMedicionxintervaloRepository()
        {
            return new MeMedicionxintervaloRepository(StrConexion);
        }

        public static IMeConfigformatenvioRepository GetMeConfigformatenvioRepository()
        {
            return new MeConfigformatenvioRepository(StrConexion);
        }

        public static IWbTipoimpugnacionRepository GetWbTipoimpugnacionRepository()
        {
            return new WbTipoimpugnacionRepository(StrConexion);
        }

        public static IWbEventoagendaRepository GetWbEventoagendaRepository()
        {
            return new WbEventoagendaRepository(StrConexion);
        }

        public static IWbImpugnacionRepository GetWbImpugnacionRepository()
        {
            return new WbImpugnacionRepository(StrConexion);
        }

        public static IWbProveedorRepository GetWbProveedorRepository()
        {
            return new WbProveedorRepository(StrConexion);
        }

        public static IPrGrupoEquipoValRepository GetPrGrupoEquipoValRepository()
        {
            return new PrGrupoEquipoValRepository(StrConexion);
        }
        public static ISmaConfiguracionRepository GetSmaConfiguracionRepository()
        {
            return new SmaConfiguracionRepository(StrConexion);
        }

        public static ISmaModoOperValRepository GetSmaModoOperValRepository()
        {
            return new SmaModoOperValRepository(StrConexion);
        }

        public static ISmaOfertaRepository GetSmaOfertaRepository()
        {
            return new SmaOfertaRepository(StrConexion);
        }

        public static ISmaOfertaDetalleRepository GetSmaOfertaDetalleRepository()
        {
            return new SmaOfertaDetalleRepository(StrConexion);
        }

        public static ISmaParamProcesoRepository GetSmaParamProcesoRepository()
        {
            return new SmaParamProcesoRepository(StrConexion);
        }

        public static ISmaRelacionOdMoRepository GetSmaRelacionOdMoRepository()
        {
            return new SmaRelacionOdMoRepository(StrConexion);
        }

        public static ISmaUrsModoOperacionRepository GetSmaUrsModoOperacionRepository()
        {
            return new SmaUrsModoOperacionRepository(StrConexion);
        }

        public static ISmaUserEmpresaRepository GetSmaUserEmpresaRepository()
        {
            return new SmaUserEmpresaRepository(StrConexion);
        }

        public static ISmaUsuarioUrsRepository GetSmaUsuarioUrsRepository()
        {
            return new SmaUsuarioUrsRepository(StrConexion);
        }

        public static ISiProcesoRepository GetSiProcesoRepository()
        {
            return new SiProcesoRepository(StrConexion);
        }

        public static ISiProcesoLogRepository GetSiProcesoLogRepository()
        {
            return new SiProcesoLogRepository(StrConexion);
        }

        public static IMeFormatoEmpresaRepository GetMeFormatoEmpresaRepository()
        {
            return new MeFormatoEmpresaRepository(StrConexion);
        }

        public static IMePtosuministradorRepository GetMePtosuministradorRepository()
        {
            return new MePtosuministradorRepository(StrConexion);
        }

        public static ISiCorreoRepository GetSiCorreoRepository()
        {
            return new SiCorreoRepository(StrConexion);
        }

        public static ISiCorreoArchivoRepository GetSiCorreoArchivoRepository()
        {
            return new SiCorreoArchivoRepository(StrConexion);
        }

        public static ISiPlantillacorreoRepository GetSiPlantillacorreoRepository()
        {
            return new SiPlantillacorreoRepository(StrConexion);
        }

        public static ISiTipoplantillacorreoRepository GetSiTipoplantillacorreoRepository()
        {
            return new SiTipoplantillacorreoRepository(StrConexion);
        }

        public static IMeHojaRepository GetMeHojaRepository()
        {
            return new MeHojaRepository(StrConexion);
        }

        public static IMeValidacionRepository GetMeValidacionRepository()
        {
            return new MeValidacionRepository(StrConexion);
        }

        public static IIioControlCargaRepository GetControlCargaRepository()
        {
            return new IioControlCargaRepository(StrConexion);
        }

        public static IIioLogRemisionRepository GetLogRemisionRepository()
        {
            return new IioLogRemisionRepository(StrConexion);
        }

        public static IIioPeriodoSeinRepository GetPeriodoSeinRepository()
        {
            return new IioPeriodoSeinRepository(StrConexion);
        }

        public static IIioTablaSyncRepository GeTablaSyncRepository()
        {
            return new IioTablaSyncRepository(StrConexion);
        }

        public static ISiEmpresaCorreoRepository GetSiEmpresaCorreoRepository()
        {
            return new SiEmpresaCorreoRepository(StrConexion);
        }

        public static IIioPeriodoSicliRepository GetPeriodoSicliRepository()
        {
            return new IioPeriodoSicliRepository(StrConexion);
        }

        public static IWbPublicacionRepository GetWbPublicacionRepository()
        {
            return new WbPublicacionRepository(StrConexion);
        }

        public static IWbSubscripcionRepository GetWbSubscripcionRepository()
        {
            return new WbSubscripcionRepository(StrConexion);
        }

        public static IWbSubscripcionitemRepository GetWbSubscripcionitemRepository()
        {
            return new WbSubscripcionitemRepository(StrConexion);
        }

        #region RSF_2024
        public static ISmaMaestroMotivoRepository GetSmaMaestroMotivoRepository()
        {
            return new SmaMaestroMotivoRepository(StrConexion);
        }

        public static ISmaAmpliacionPlazoRepository GetSmaAmpliacionPlazoRepository()
        {
            return new SmaAmpliacionPlazoRepository(StrConexion);
        }

        public static ISmaActivacionOfertaRepository GetSmaActivacionOfertaRepository()
        {
            return new SmaActivacionOfertaRepository(StrConexion);
        }

        public static ISmaActivacionDataRepository GetSmaActivacionDataRepository()
        {
            return new SmaActivacionDataRepository(StrConexion);
        }

        public static ISmaActivacionMotivoRepository GetSmaActivacionMotivoRepository()
        {
            return new SmaActivacionMotivoRepository(StrConexion);
        }

        public static ISmaIndisponibilidadTemporalRepository GetSmaIndisponibilidadTemporalRepository()
        {
            return new SmaIndisponibilidadTemporalRepository(StrConexion);
        }

        public static ISmaIndisponibilidadTempCabRepository GetSmaIndisponibilidadTempCabRepository()
        {
            return new SmaIndisponibilidadTempCabRepository(StrConexion);
        }

        public static ISmaIndisponibilidadTempDetRepository GetSmaIndisponibilidadTempDetRepository()
        {
            return new SmaIndisponibilidadTempDetRepository(StrConexion);
        }
        #endregion


        /** COSTOS MARGINALES **/

        public static IEqRelacionRepository GetEqRelacionRepository()
        {
            return new EqRelacionRepository(StrConexion);
        }

        public static IEqCongestionConfigRepository GetEqCongestionConfigRepository()
        {
            return new EqCongestionConfigRepository(StrConexion);
        }

        public static IEqGrupoLineaRepository GetEqGrupoLineaRepository()
        {
            return new EqGrupoLineaRepository(StrConexion);
        }

        public static IPrGenforzadaMaestroRepository GetPrGenforzadaMaestroRepository()
        {
            return new PrGenforzadaMaestroRepository(StrConexion);
        }

        public static IPrGenforzadaRepository GetPrGenforzadaRepository()
        {
            return new PrGenforzadaRepository(StrConexion);
        }

        public static IPrCongestionRepository GetPrCongestionRepository()
        {
            return new PrCongestionRepository(StrConexion);
        }

        public static IPrCongestionGrupoRepository GetPrCongestionGrupoRepository()
        {
            return new PrCongestionGrupoRepository(StrConexion);
        }

        public static IPrConfiguracionPotEfectivaRepository GetPrConfiguracionPotEfectivaRepository()
        {
            return new PrConfiguracionPotEfectivaRepository(StrConexion);
        }

        public static IMdValidacionRepository GetMdValidacionRepository()
        {
            return new MdValidacionRepository(StrConexion);
        }

        /* Reserva Secundaria de Frecuencia */

        public static IEveRsfdetalleRepository GetEveRsfdetalleRepository()
        {
            return new EveRsfdetalleRepository(StrConexion);
        }

        public static IEveRsfhoraRepository GetEveRsfhoraRepository()
        {
            return new EveRsfhoraRepository(StrConexion);
        }

        public static IEveRsfequivalenciaRepository GetEveRsfequivalenciaRepository()
        {
            return new EveRsfequivalenciaRepository(StrConexion);
        }

        public static IIioControlImportacionRepository GetControlImportacionRepository()
        {
            return new IioControlImportacionRepository(StrConexion);
        }

        public static IIioTmpConsumoRepository GetTmpConsumoRepository()
        {
            return new IioTmpConsumoRepository(StrConexion);
        }

        public static IIioFacturaRepository GetFacturaRepository()
        {
            return new IioFacturaRepository(StrConexion);
        }

        public static IPrBarraRepository GetPrBarraRepository()
        {
            return new PrBarraRepository(StrConexion);
        }

        public static IWbContactoRepository GetWbContactoRepository()
        {
            return new WbContactoRepository(StrConexion);
        }

        public static IPrLogsorteoRepository GetPrLogsorteoRepository()
        {
            return new PrLogsorteoRepository(StrConexion);
        }

        public static IAgcControlRepository GetAgcControlRepository()
        {
            return new AgcControlRepository(StrConexion);
        }

        public static IWbDecisionejecutivaRepository GetWbDecisionejecutivaRepository()
        {
            return new WbDecisionejecutivaRepository(StrConexion);
        }

        public static IWbDecisionejecutadoDetRepository GetWbDecisionejecutadoDetRepository()
        {
            return new WbDecisionejecutadoDetRepository(StrConexion);
        }

        public static IAgcControlPuntoRepository GetAgcControlPuntoRepository()
        {
            return new AgcControlPuntoRepository(StrConexion);
        }

        public static IWbNotificacionRepository GetWbNotificacionRepository()
        {
            return new WbNotificacionRepository(StrConexion);
        }

        public static IWbCmvstarifaRepository GetWbCmvstarifaRepository()
        {
            return new WbCmvstarifaRepository(StrConexion);
        }

        public static IWbResumengenRepository GetWbResumengenRepository()
        {
            return new WbResumengenRepository(StrConexion);
        }

        public static IWbVersionappRepository GetWbVersionappRepository()
        {
            return new WbVersionappRepository(StrConexion);
        }

        public static IWbCalendarioRepository GetWbCalendarioRepository()
        {
            return new WbCalendarioRepository(StrConexion);
        }

        public static IWbMescalendarioRepository GetWbMescalendarioRepository()
        {
            return new WbMescalendarioRepository(StrConexion);
        }

        public static IWbCaltipoventoRepository GetWbCaltipoventoRepository()
        {
            return new WbCaltipoventoRepository(StrConexion);
        }

        public static IWbRegistroimeiRepository GetWbRegistroimeiRepository()
        {
            return new WbRegistroimeiRepository(StrConexion);
        }

        public static IWbAyudaappRepository GetWbAyudaappRepository()
        {
            return new WbAyudaappRepository(StrConexion);
        }

        //- alpha.HDT - 26/10/2016: Cambio para atender el requerimiento. 
        public static IIioAsignacionPendienteRepository GetIioAsignacionPendienteRepository()
        {
            return new IioAsignacionPendienteRepository(StrConexion);
        }
        //- alpha.JDEL - Inicio 19/12/2016: Cambio para atender el requerimiento.
        public static IEntidadListadoRepository GetEntidadListadoRepository()
        {
            return new EntidadListadoRepository(StrConexion);
        }

        //- alpha.JDEL - Inicio 24/10/2016: Cambio para atender el requerimiento.
        public static IIioLogImportacionRepository GetIioLogImportacionRepository()
        {
            return new IioLogImportacionRepository(StrConexion);
        }

        public static ICmAlertavalorRepository GetCmAlertavalorRepository()
        {
            return new CmAlertavalorRepository(StrConexion);
        }

        public static ICmConfigbarraRepository GetCmConfigbarraRepository()
        {
            return new CmConfigbarraRepository(StrConexion);
        }

        public static ICmFlujoPotenciaRepository GetCmFlujoPotenciaRepository()
        {
            return new CmFlujoPotenciaRepository(StrConexion);
        }

        public static ICmBarrageneradorRepository GetCmBarrageneradorRepository()
        {
            return new CmBarrageneradorRepository(StrConexion);
        }

        public static ICmCostomarginalprogRepository GetCmCostomarginalprogRepository()
        {
            return new CmCostomarginalprogRepository(StrConexion);
        }

        public static ICmCostomarginalRepository GetCmCostomarginalRepository()
        {
            return new CmCostomarginalRepository(StrConexion);
        }

        public static ICmRestriccionRepository GetCmRestriccionRepository()
        {
            return new CmRestriccionRepository(StrConexion);
        }

        public static ICmVersionprogramaRepository GetCmVersionprogramaRepository()
        {
            return new CmVersionprogramaRepository(StrConexion);
        }

        public static ICmParametroRepository GetCmParametroRepository()
        {
            return new CmParametroRepository(StrConexion);
        }

        public static ICmEquivalenciamodopRepository GetCmEquivalenciamodopRepository()
        {
            return new CmEquivalenciamodopRepository(StrConexion);
        }

        public static ICmGeneracionEmsRepository GetCmGeneracionEmsRepository()
        {
            return new CmGeneracionEmsRepository(StrConexion);
        }

        public static IEmsGeneracionRepository GetEmsGeneracionRepository()
        {
            return new EmsGeneracionRepository(StrConexion);
        }

        public static ICmOperacionregistroRepository GetCmOperacionregistroRepository()
        {
            return new CmOperacionregistroRepository(StrConexion);
        }

        public static IEqCategoriaRepository GetEqCategoriaRepository()
        {
            return new EqCategoriaRepository(StrConexion);
        }

        public static IEqCategoriaDetRepository GetEqCategoriaDetalleRepository()
        {
            return new EqCategoriaDetRepository(StrConexion);
        }

        public static IEqCategoriaEquipoRepository GetEqCategoriaEquipoRepository()
        {
            return new EqCategoriaEquipoRepository(StrConexion);
        }

        public static ICmConjuntoenlaceRepository GetCmConjuntoenlaceRepository()
        {
            return new CmConjuntoenlaceRepository(StrConexion);
        }

        public static ICmCompensacionRepository GetCmCompensacionRepository()
        {
            return new CmCompensacionRepository(StrConexion);
        }

        public static ICmDemandatotalRepository GetCmDemandatotalRepository()
        {
            return new CmDemandatotalRepository(StrConexion);
        }

        public static ICmGeneracionRepository GetCmGeneracionRepository()
        {
            return new CmGeneracionRepository(StrConexion);
        }

        //- Aplicativo PMPO

        public static IMeMensajeRepository GetMeMensajeRepository()
        {
            return new MeMensajeRepository(StrConexion);
        }

        public static IPmpoObraRepository GetPmpoObraRepository()
        {
            return new PmpoObraRepository(StrConexion);
        }

        public static IPmpoObraDetalleRepository GetPmpoObraDetalleRepository()
        {
            return new PmpoObraDetalleRepository(StrConexion);
        }

        public static IPmpoTipoObraRepository GetPmpoTipoObraRepository()
        {
            return new PmpoTipoObraRepository(StrConexion);
        }

        public static IPmpoConfiguracionRepository GetPmpoConfpmRepository()
        {
            return new PmpoConfiguracionRepository(StrConexion);
        }

        public static ISiParametroRepository GetSiParametroRepository()
        {
            return new SiParametroRepository(StrConexion);
        }

        public static ISiParametroValorRepository GetSiParametroValorRepository()
        {
            return new SiParametroValorRepository(StrConexion);
        }
        // compensaciones. DSH -  18/05/2017 : Cambio para atender el requirimiento
        public static IVceTextoReporteRepository GetVceTextoReporteRepository()
        {
            return new VceTextoReporteRepository(StrConexion);
        }

        // compensaciones. DSH -  11/04/2017 : Cambio para atender el requirimiento
        public static IVcePeriodoCalculoRepository GetVcePeriodoCalculoRepository()
        {
            return new VcePeriodoCalculoRepository(StrConexion);
        }

        //- conpensaciones.JDEL - Inicio 17/09/2016: Cambio para atender el requerimiento.

        public static IPeriodoRepository GetTrnPeriodoRepository()
        {
            return new PeriodoRepository(StrConexion);
        }

        public static IVceEnergiaRepository GetVceEnergiaRepository()
        {
            return new VceEnergiaRepository(StrConexion);
        }

        public static IVceHoraOperacionRepository GetVceHoraOperacionRepository()
        {
            return new VceHoraOperacionRepository(StrConexion);
        }

        public static IVceCostoMarginalRepository GetVceCostoMarginalRepository()
        {
            return new VceCostoMarginalRepository(StrConexion);
        }

        public static IVceDatcalculoRepository GetVceDatCalculoRepository()
        {
            return new VceDatcalculoRepository(StrConexion);
        }

        public static IBarraRepository GetTrnBarraRepository()
        {
            return new BarraRepository(StrConexion);
        }

        public static IVcePtomedModopeRepository GetVcePtomedModopeRepository()
        {
            return new VcePtomedModopeRepository(StrConexion);
        }

        public static IVceCompBajaeficRepository GetVceCompBajaeficRepository()
        {
            return new VceCompBajaeficRepository(StrConexion);
        }

        public static IVceCompRegularDetRepository GetVceCompRegularDetRepository()
        {
            return new VceCompRegularDetRepository(StrConexion);
        }

        public static IVceCostoVariableRepository GetVceCostoVariableRepository()
        {
            return new VceCostoVariableRepository(StrConexion);
        }
        //- JDEL Fin
        public static IVceLogCargaCabRepository GetVceLogCargaCabRepository()
        {
            return new VceLogCargaCabRepository(StrConexion);
        }

        public static IVceLogCargaDetRepository GetVceLogCargaDetRepository()
        {
            return new VceLogCargaDetRepository(StrConexion);
        }

        //- compensaciones.HDT - 03/03/2017: Cambio para atender el requerimiento. 
        public static IVceCfgDatcalculoRepository GetVceCfgDatCalculoRepository()
        {
            return new VceCfgDatCalculoRepository(StrConexion);
        }

        //- compensaciones.HDT - 21/03/2017: Cambio para atender el requerimiento. 
        public static IVceArrparIncredGenRepository GetVceArrparIncredGenRepository()
        {
            return new VceArrparIncredGenRepository(StrConexion);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public static IVceArrparGrupoCabRepository GetVceArrparGrupoCaRepository()
        {
            return new VceArrparGrupoCabRepository(StrConexion);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public static IVceArrparTipoOperaRepository GetVceArrparTipoOperaRepository()
        {
            return new VceArrparTipoOperaRepository(StrConexion);
        }

        //- compensaciones.HDT - 29/03/2017: Cambio para atender el requerimiento. 
        public static IVceArrparCompEspRepository GetVceArrparCompEspRepository()
        {
            return new VceArrparCompEspRepository(StrConexion);
        }

        //- compensaciones.HDT - 06/04/2017: Cambio para atender el requerimiento. 
        public static IVceArrparRampaCfgRepository GetVceArrparRampaCfgRepository()
        {
            return new VceArrparRampaCfgRepository(StrConexion);
        }

        public static ICoBandancpRepository GetCoBandancpRepository()
        {
            return new CoBandancpRepository(StrConexion);
        }

        //ESTADISTICAS SGDOC
        public static ISgdEstadisticasRepository GetSgdEstadisticasRepository()
        {
            return new SgdEstadisticasRepository(StrConexion);
        }
        public static IDocTipoRepository GetDocTipoRepository()
        {
            return new DocTipoRepository(StrConexion);
        }

        public static IDocFlujoRepository GetDocFlujoRepository()
        {
            return new DocFlujoRepository(StrConexion);
        }

        public static IMeVerificacionRepository GetMeVerificacionRepository()
        {
            return new MeVerificacionRepository(StrConexion);
        }

        public static IMeVerificacionFormatoRepository GetMeVerificacionFormatoRepository()
        {
            return new MeVerificacionFormatoRepository(StrConexion);
        }

        public static IPmpoReportOsinergRepository GetPmpoReportOsinerg()
        {
            return new PmpoReportOsinergRepository(StrConexion);
        }


        //- NODO ENERGETICO
        public static INrPotenciaconsignaRepository GetNrPotenciaconsignaRepository()
        {
            return new NrPotenciaconsignaRepository(StrConexion);
        }

        public static INrSubmoduloRepository GetNrSubmoduloRepository()
        {
            return new NrSubmoduloRepository(StrConexion);
        }

        public static INrPotenciaconsignaDetalleRepository GetNrPotenciaconsignaDetalleRepository()
        {
            return new NrPotenciaconsignaDetalleRepository(StrConexion);
        }

        public static INrSobrecostoRepository GetNrSobrecostoRepository()
        {
            return new NrSobrecostoRepository(StrConexion);
        }

        public static INrPeriodoResumenRepository GetNrPeriodoResumenRepository()
        {
            return new NrPeriodoResumenRepository(StrConexion);
        }

        public static INrConceptoRepository GetNrConceptoRepository()
        {
            return new NrConceptoRepository(StrConexion);
        }

        public static INrProcesoRepository GetNrProcesoRepository()
        {
            return new NrProcesoRepository(StrConexion);
        }

        public static INrPeriodoRepository GetNrPeriodoRepository()
        {
            return new NrPeriodoRepository(StrConexion);
        }

        #region PR5

        public static IEveEventoEquipoRepository GetEveEventoEquipoRepository()
        {
            return new EveEventoEquipoRepository(StrConexion);
        }

        public static ISiMensajeRepository GetSiMensajeRepository()
        {
            return new SiMensajeRepository(StrConexion);
        }

        public static ISiTipoMensajeRepository GetSiTipoMensajeRepository()
        {
            return new SiTipoMensajeRepository(StrConexion);
        }

        public static ISiLogRepository GetSiLogRepository()
        {
            return new SiLogRepository(StrConexion);
        }

        public static ISiMenureporteRepository GetSiMenureporteRepository()
        {
            return new SiMenureporteRepository(StrConexion);
        }

        public static ISiSolicitudAmpliacionRepository GetSiSolicitudAmpliacionRepository()
        {
            return new SiSolicitudAmpliacionRepository(StrConexion);
        }

        public static IFwModuloRepository GetFwModuloRepository()
        {
            return new FwModuloRepository(StrConexion);
        }

        public static IFwUserrolRepository GetFwUserrolRepository()
        {
            return new FwUserrolRepository(StrConexion);
        }

        public static IFwRolRepository GetFwRolRepository()
        {
            return new FwRolRepository(StrConexion);
        }

        public static IMeRelacionptoRepository GetMeRelacionptoRepository()
        {
            return new MeRelacionptoRepository(StrConexion);
        }

        public static ISiVersionRepository GetSiVersionRepository()
        {
            return new SiVersionRepository(StrConexion);
        }

        public static ISiVersionDetRepository GetSiVersionDetRepository()
        {
            return new SiVersionDetRepository(StrConexion);
        }

        public static ISiMenureporteTipoRepository GetSiMenureporteTipoRepository()
        {
            return new SiMenureporteTipoRepository(StrConexion);
        }

        public static ISiMenuProjectRepository GetSiMenuProjectRepository()
        {
            return new SiMenuProjectRepository(StrConexion);
        }

        public static IMeReporteGraficoRepository GetMeReporteGraficoRepository()
        {
            return new MeReporteGraficoRepository(StrConexion);
        }

        public static IMePtomedcanalRepository GetMePtomedcanalRepository()
        {
            return new MePtomedcanalRepository(StrConexion);
        }

        public static IEveGpsaisladoRepository GetEveGpsaisladoRepository()
        {
            return new EveGpsaisladoRepository(StrConexion);
        }

        public static ISiMenureporteHojaRepository GetSiMenureporteHojaRepository()
        {
            return new SiMenureporteHojaRepository(StrConexion);
        }

        public static ISiVersionDatRepository GetSiVersionDatRepository()
        {
            return new SiVersionDatRepository(StrConexion);
        }

        public static ISiVersionDatdetRepository GetSiVersionDatdetRepository()
        {
            return new SiVersionDatdetRepository(StrConexion);
        }

        public static ISiVersionConceptoRepository GetSiVersionConceptoRepository()
        {
            return new SiVersionConceptoRepository(StrConexion);
        }

        public static IMeDespachoProdgenRepository GetMeDespachoProdgenRepository()
        {
            return new MeDespachoProdgenRepository(StrConexion);
        }

        public static IMeDespachoResumenRepository GetMeDespachoResumenRepository()
        {
            return new MeDespachoResumenRepository(StrConexion);
        }

        #endregion

        #region SIOSEIN

        public static ISiBandejamensajeUserRepository GetSiBandejamensajeUserRepository()
        {
            return new SiBandejamensajeUserRepository(StrConexion);
        }

        public static ISiEnviodetRepository GetSiEnviodetRepository()
        {
            return new SiEnviodetRepository(StrConexion);
        }

        public static ISioCabeceradetRepository GetSioCabeceradetRepository()
        {
            return new SioCabeceradetRepository(StrConexion);
        }

        public static ISioCambioprieRepository GetSioCambioprieRepository()
        {
            return new SioCambioprieRepository(StrConexion);
        }

        public static ISioColumnaprieRepository GetSioColumnaprieRepository()
        {
            return new SioColumnaprieRepository(StrConexion);
        }

        public static ISioDatoprieRepository GetSioDatoprieRepository()
        {
            return new SioDatoprieRepository(StrConexion);
        }

        public static ISioTablaprieRepository GetSioTablaprieRepository()
        {
            return new SioTablaprieRepository(StrConexion);
        }

        public static ISioPrieCompRepository GetSioPrieCompRepository()
        {
            return new SioPrieCompRepository(StrConexion);
        }

        #endregion

        #region Indisponibilidades
        public static IIndManttoRepository GetIndManttoRepository()
        {
            return new IndManttoRepository(StrConexion);
        }

        public static IIndRecalculoRepository GetIndRecalculoRepository()
        {
            return new IndRecalculoRepository(StrConexion);
        }

        public static IIndPeriodoRepository GetIndPeriodoRepository()
        {
            return new IndPeriodoRepository(StrConexion);
        }

        public static IIndCuadroRepository GetIndCuadroRepository()
        {
            return new IndCuadroRepository(StrConexion);
        }

        public static IIndReporteFCRepository GetIndReporteFCRepository()
        {
            return new IndReporteFCRepository(StrConexion);
        }

        public static IIndReporteCalculosRepository GetIndReporteCalculosRepository()
        {
            return new IndReporteCalculosRepository(StrConexion);
        }

        public static IIndReporteInsumosRepository GetIndReporteInsumosRepository()
        {
            return new IndReporteInsumosRepository(StrConexion);
        }

        public static IIndReporteDetRepository GetIndReporteDetRepository()
        {
            return new IndReporteDetRepository(StrConexion);
        }

        public static IIndReporteTotalRepository GetIndReporteTotalRepository()
        {
            return new IndReporteTotalRepository(StrConexion);
        }

        public static IIndReporteRepository GetIndReporteRepository()
        {
            return new IndReporteRepository(StrConexion);
        }

        public static IIndUnidadRepository GetIndUnidadRepository()
        {
            return new IndUnidadRepository(StrConexion);
        }

        public static IIndGaseoductoxcentralRepository GetIndGaseoductoxcentralRepository()
        {
            return new IndGaseoductoxcentralRepository(StrConexion);
        }

        public static IIndInsumoLogRepository GetIndInsumoLogRepository()
        {
            return new IndInsumoLogRepository(StrConexion);
        }

        public static IIndPotlimRepository GetIndPotlimRepository()
        {
            return new IndPotlimRepository(StrConexion);
        }

        public static IIndPotlimUnidadRepository GetIndPotlimUnidadRepository()
        {
            return new IndPotlimUnidadRepository(StrConexion);
        }

        public static IIndEventoRepository GetIndEventoRepository()
        {
            return new IndEventoRepository(StrConexion);
        }

        public static IIndIeodcuadroRepository GetIndIeodcuadroRepository()
        {
            return new IndIeodcuadroRepository(StrConexion);
        }

        public static IIndRelacionRptRepository GetIndRelacionRptRepository()
        {
            return new IndRelacionRptRepository(StrConexion);
        }

        #region IND.PR25.2022
        public static IIndStockCombustibleRepository GetIndStockCombustibleRepository()
        {
            return new IndStockCombustibleRepository(StrConexion);
        }

        public static IIndStkCombustibleDetalleRepository GetIndStkCombustibleDetalleRepository()
        {
            return new IndStkCombustibleDetalleRepository(StrConexion);
        }

        public static IIndHistoricoStockCombustRepository GetIndHistoricoStockCombustRepository()
        {
            return new IndHistoricoStockCombustRepository(StrConexion);
        }

        public static IIndInsumosFactorKRepository GetIndInsumosFactorKRepository()
        {
            return new IndInsumosFactorKRepository(StrConexion);
        }

        public static IIndInsumosFactorKDetalleRepository GetIndInsumosFactorKDetalleRepository()
        {
            return new IndInsumosFactorKDetalleRepository(StrConexion);
        }

        #endregion

        public static IIndRelacionEmpresaRepository GetIIndRelacionEmpresaRepository()
        {
            return new IndRelacionEmpresaRepository(StrConexion);
        }

        #endregion

        #region Rechazo Carga
        public static IRcaCargaEsencialRepository GetRcaCargaEsencialRepository()
        {
            return new RcaCargaEsencialRepository(StrConexion);
        }

        public static IRcaCuadroEjecUsuarioRepository GetRcaCuadroEjecUsuarioRepository()
        {
            return new RcaCuadroEjecUsuarioRepository(StrConexion);
        }

        public static IRcaCuadroProgRepository GetRcaCuadroProgRepository()
        {
            return new RcaCuadroProgRepository(StrConexion);
        }

        public static IRcaCuadroProgUsuarioRepository GetRcaCuadroProgUsuarioRepository()
        {
            return new RcaCuadroProgUsuarioRepository(StrConexion);
        }

        public static IRcaEsquemaUnifilarRepository GetRcaEsquemaUnifilarRepository()
        {
            return new RcaEsquemaUnifilarRepository(StrConexion);
        }

        public static IRcaParamEsquemaRepository GetRcaParamEsquemaRepository()
        {
            return new RcaParamEsquemaRepository(StrConexion);
        }

        public static IRcaProgramaRepository GetRcaProgramaRepository()
        {
            return new RcaProgramaRepository(StrConexion);
        }

        public static IRcaRegistroSvrmRepository GetRcaRegistroSvrmRepository()
        {
            return new RcaRegistroSvrmRepository(StrConexion);
        }
        public static IRcaDemandaUsuarioRepository GetRcaDemandaUsuarioRepository()
        {
            return new RcaDemandaUsuarioRepository(StrConexion);
        }
        public static IRcaCuadroProgDistribRepository GetRcaCuadroProgDistribRepository()
        {
            return new RcaCuadroProgDistribRepository(StrConexion);
        }
        public static IRcaCuadroEjecUsuarioDetRepository GetRcaCuadroEjecUsuarioDetRepository()
        {
            return new RcaCuadroEjecUsuarioDetRepository(StrConexion);
        }
        #endregion

        #region Registro Integrantes

        public static IRiDetalleRevisionRepository GetRiDetalleRevisionRepository()
        {
            return new RiDetalleRevisionRepository(StrConexion);
        }

        public static IRiEtaparevisionRepository GetRiEtaparevisionRepository()
        {
            return new RiEtaparevisionRepository(StrConexion);
        }

        public static IRiRevisionRepository GetRiRevisionRepository()
        {
            return new RiRevisionRepository(StrConexion);
        }

        public static IRiSolicitudRepository GetRiSolicitudRepository()
        {
            return new RiSolicitudRepository(StrConexion);
        }

        public static IRiSolicituddetalleRepository GetRiSolicituddetalleRepository()
        {
            return new RiSolicituddetalleRepository(StrConexion);
        }

        public static IRiTiposolicitudRepository GetRiTiposolicitudRepository()
        {
            return new RiTiposolicitudRepository(StrConexion);
        }

        public static IRiHistoricoRepository GetRiHistoricoRepository()
        {
            return new RiHistoricoRepository(StrConexion);
        }

        public static ISiRepresentanteRepository GetSiRepresentanteRepository()
        {
            return new SiRepresentanteRepository(StrConexion);
        }

        public static ISiTipoComportamientoRepository GetSiTipoComportamientoRepository()
        {
            return new SiTipoComportamientoRepository(StrConexion);
        }

        public static ISiEmpresaRIRepository GetSiEmpresaRIRepository()
        {
            return new SiEmpresaRIRepository(StrConexion);
        }

        #endregion

        public static IEvePruebaunidadRepository GetEvePruebaunidadRepository()
        {
            return new EvePruebaunidadRepository(StrConexion);
        }

        #region Horas de Operación
        public static IEveHoraoperacionEquipoRepository GetEveHoraoperacionEquipoRepository()
        {
            return new EveHoraoperacionEquipoRepository(StrConexion);
        }

        public static IEveHoraoperacionRepository GetEveHoraoperacionRepository()
        {
            return new EveHoraoperacionRepository(StrConexion);
        }

        public static IEveHoUnidadRepository GetEveHoUnidadRepository()
        {
            return new EveHoUnidadRepository(StrConexion);
        }

        public static IEveHoEquiporelRepository GetEveHoEquiporelRepository()
        {
            return new EveHoEquiporelRepository(StrConexion);
        }

        public static ISiAmplazoenvioRepository GetSiAmplazoenvioRepository()
        {
            return new SiAmplazoenvioRepository(StrConexion);
        }

        public static ISiPlazoenvioRepository GetSiPlazoenvioRepository()
        {
            return new SiPlazoenvioRepository(StrConexion);
        }

        #endregion

        #region SIOSEIN2 - NUMERALES
        public static ISiCostomarginalRepository GetSiCostomarginalRepository()
        {
            return new SiCostomarginalRepository(StrConexion);
        }
        public static ISicCostomarginalGraficosRepository GetSicCostomarginalGraficosRepository()
        {
            return new SicCostomarginalGraficosRepository(StrConexion);
        }

        public static ISiCostomarginaltempRepository GetSiCostomarginaltempRepository()
        {
            return new SiCostomarginaltempRepository(StrConexion);
        }

        #endregion

        #region SIOSEIN2 - NUEVA TABLAS

        public static ISpoClasificacionRepository GetSpoClasificacionRepository()
        {
            return new SpoClasificacionRepository(StrConexion);
        }

        public static ISpoConceptoRepository GetSpoConceptoRepository()
        {
            return new SpoConceptoRepository(StrConexion);
        }

        public static ISpoNumcuadroRepository GetSpoNumcuadroRepository()
        {
            return new SpoNumcuadroRepository(StrConexion);
        }

        public static ISpoNumdatcambioRepository GetSpoNumdatcambioRepository()
        {
            return new SpoNumdatcambioRepository(StrConexion);
        }

        public static ISpoNumeralRepository GetSpoNumeralRepository()
        {
            return new SpoNumeralRepository(StrConexion);
        }

        public static ISpoNumeraldatRepository GetSpoNumeraldatRepository()
        {
            return new SpoNumeraldatRepository(StrConexion);
        }

        public static ISpoNumeralGenforzadaRepository GetSpoNumeralGenforzadaRepository()
        {
            return new SpoNumeralGenforzadaRepository(StrConexion);
        }

        public static ISpoNumhistoriaRepository GetSpoNumhistoriaRepository()
        {
            return new SpoNumhistoriaRepository(StrConexion);
        }

        public static ISpoReporteRepository GetSpoReporteRepository()
        {
            return new SpoReporteRepository(StrConexion);
        }

        public static ISpoVerrepnumRepository GetSpoVerrepnumRepository()
        {
            return new SpoVerrepnumRepository(StrConexion);
        }

        public static ISpoVersionnumRepository GetSpoVersionnumRepository()
        {
            return new SpoVersionnumRepository(StrConexion);
        }

        public static ISpoVersionrepRepository GetSpoVersionrepRepository()
        {
            return new SpoVersionrepRepository(StrConexion);
        }

        public static IMapMedicion48Repository GetMapMedicion48Repository()
        {
            return new MapMedicion48Repository(StrConexion);
        }

        public static IMapTipocalculoRepository GetMapTipocalculoRepository()
        {
            return new MapTipocalculoRepository(StrConexion);
        }

        public static IMapVersionRepository GetMapVersionRepository()
        {
            return new MapVersionRepository(StrConexion);
        }

        public static IMapEmpsinrepRepository GetMapEmpsinrepRepository()
        {
            return new MapEmpsinrepRepository(StrConexion);
        }

        public static IMapEmpresaulRepository GetMapEmpresaulRepository()
        {
            return new MapEmpresaulRepository(StrConexion);
        }

        public static ISiNotaRepository GetSiNotaRepository()
        {
            return new SiNotaRepository(StrConexion);
        }

        public static ISiMenureportedetRepository GetSiMenureportedetRepository()
        {
            return new SiMenureportedetRepository(StrConexion);
        }

        public static IIeeRecenergeticoTipoRepository GetIeeRecenergeticoTipoRepository()
        {
            return new IeeRecenergeticoTipoRepository(StrConexion);
        }

        public static IIeeRecenergeticoHistRepository GetIeeRecenergeticoHistRepository()
        {
            return new IeeRecenergeticoHistRepository(StrConexion);
        }

        public static IIeeModoopecmgRepository GetIeeModoopecmgRepository()
        {
            return new IeeModoopecmgRepository(StrConexion);
        }

        public static IIeeBarrazonaRepository GetIeeBarrazonaRepository()
        {
            return new IeeBarrazonaRepository(StrConexion);
        }

        public static IMapDemandaRepository GetMapDemandaRepository()
        {
            return new MapDemandaRepository(StrConexion);
        }

        #endregion

        #region MigracionSGOCOES-GrupoB

        public static ISiActividadRepository GetSiActividadRepository()
        {
            return new SiActividadRepository(StrConexion);
        }

        public static ISiAreaRepository GetSiAreaRepository()
        {
            return new SiAreaRepository(StrConexion);
        }

        public static ISiRolTurnoRepository GetSiRolTurnoRepository()
        {
            return new SiRolTurnoRepository(StrConexion);
        }

        public static IEveObservacionRepository GetEveObservacionRepository()
        {
            return new EveObservacionRepository(StrConexion);
        }
            
        public static IPrAgrupacionRepository GetPrAgrupacionRepository()
        {
            return new PrAgrupacionRepository(StrConexion);
        }

        public static IPrAgrupacionConceptoRepository GetPrAgrupacionConceptoRepository()
        {
            return new PrAgrupacionConceptoRepository(StrConexion);
        }

        public static IPrAreaConceptoRepository GetPrAreaConceptoRepository()
        {
            return new PrAreaConceptoRepository(StrConexion);
        }

        public static IPrAreaConcepUserRepository GetPrAreaConcepUserRepository()
        {
            return new PrAreaConcepUserRepository(StrConexion);
        }

        public static IMeMedicion60Repository GetMeMedicion60Repository()
        {
            return new MeMedicion60Repository(StrConexion);
        }

        public static IEqEquicanalRepository GetEqEquicanalRepository()
        {
            return new EqEquicanalRepository(StrConexion);
        }

        public static IPrDnotasRepository GetPrDnotasRepository()
        {
            return new PrDnotasRepository(StrConexion);
        }

        public static ICPointsRepository GetCPointsRepository()
        {
            return new CPointsRepository(StrConexionSCADA);
        }

        public static IPrHtrabajoEstadoRepository GetPrHtrabajoEstadoRepository()
        {
            return new PrHtrabajoEstadoRepository(StrConexion);
        }

        public static IPrReservaRepository GetPrReservaRepository()
        {
            return new PrReservaRepository(StrConexion);
        }

        #endregion

        #region CONFIGURACION
        public static IFwAreaRepository GetAreaRepository()
        {
            return new FwAreaRepository(StrConexion);
        }
        #endregion
        //cambio Compensaciones 201810
        public static IVceGrupoExcluidoRepository GetVceGrupoExcluidoRepository()
        {
            return new VceGrupoExcluidoRepository(StrConexion);
        }

        public static IVceCfgValidaConceptoRepository GetVceCfgValidaConceptoRepository()
        {
            return new VceCfgValidaConceptoRepository(StrConexion);
        }

        #region FIT - SGOCOES func A

        //SORTEO
        public static ISiListarAreaSorteoRepository GetSiListarAreasSorteoRepository()
        {
            return new SiListarAreaSorteoRepository(StrConexion);
        }
        #endregion

        #region INTERVENCIONES

        public static IInClaseProgRepository GetInClaseProgRepository()
        {
            return new InClaseProgRepository(StrConexion);
        }

        public static IInEstadoRepository GetInEstadoRepository()
        {
            return new InEstadoRepository(StrConexion);
        }

        public static IInIntervencionRepository GetInIntervencionRepository()
        {
            return new InIntervencionRepository(StrConexion);
        }

        public static IInParametroPlazoRepository GetInParametroPlazoRepository()
        {
            return new InParametroPlazoRepository(StrConexion);
        }

        public static IInProgramacionRepository GetInProgramacionRepository()
        {
            return new InProgramacionRepository(StrConexion);
        }

        public static IMeEnvioDetMensajeRepository GetMeEnvioDetMensajeRepository()
        {
            return new MeEnvioDetMensajeRepository(StrConexion);
        }

        public static ISiEmpresaMensajeRepository GetSiEmpresaMensajeRepository()
        {
            return new SiEmpresaMensajeRepository(StrConexion);
        }
        
        public static ISiEmpresamensajedetRepository GetSiEmpresamensajedetRepository()
        {
            return new SiEmpresamensajedetRepository(StrConexion);
        }

        public static IInFactorVersionRepository GetInFactorVersionRepository()
        {
            return new InFactorVersionRepository(StrConexion);
        }

        public static IInFactorVersionDetRepository GetInFactorVersionDetRepository()
        {
            return new InFactorVersionDetRepository(StrConexion);
        }

        public static IInFactorVersionMmayorRepository GetInFactorVersionMmayorRepository()
        {
            return new InFactorVersionMmayorRepository(StrConexion);
        }

        public static IInArchivoRepository GetInArchivoRepository()
        {
            return new InArchivoRepository(StrConexion);
        }

        public static IInFactorRelMmayorArchivoRepository GetInFactorRelMmayorArchivoRepository()
        {
            return new InFactorRelMmayorArchivoRepository(StrConexion);
        }

        public static IInReporteVariableRepository GetInReporteVariableRepository()
        {
            return new InReporteVariableRepository(StrConexion);
        }

        public static IInReporteRepository GetInReporteRepository()
        {
            return new InReporteRepository(StrConexion);
        }

        public static IInSeccionRepository GetInSeccionRepository()
        {
            return new InSeccionRepository(StrConexion);
        }

        public static IInDestinatariomensajeRepository GetInDestinatariomensajeRepository()
        {
            return new InDestinatariomensajeRepository(StrConexion);
        }

        public static IInDestinatariomensajeDetRepository GetInDestinatariomensajeDetRepository()
        {
            return new InDestinatariomensajeDetRepository(StrConexion);
        }

        public static IInIntervencionRelArchivoRepository GetInIntervencionRelArchivoRepository()
        {
            return new InIntervencionRelArchivoRepository(StrConexion);
        }

        public static IInSustentoRepository GetInSustentoRepository()
        {
            return new InSustentoRepository(StrConexion);
        }

        public static IInSustentopltRepository GetInSustentopltRepository()
        {
            return new InSustentopltRepository(StrConexion);
        }

        public static IInSustentopltItemRepository GetInSustentopltItemRepository()
        {
            return new InSustentopltItemRepository(StrConexion);
        }

        public static IInSustentoDetRepository GetInSustentoDetRepository()
        {
            return new InSustentoDetRepository(StrConexion);
        }

        public static IInMensajeRelArchivoRepository GetInMensajeRelArchivoRepository()
        {
            return new InMensajeRelArchivoRepository(StrConexion);
        }

        public static IInSustentoDetRelArchivoRepository GetInSustentoDetRelArchivoRepository()
        {
            return new InSustentoDetRelArchivoRepository(StrConexion);
        }

        #endregion

        #region Rol de Turnos

        public static IRtuActividadRepository GetRtuActividadRepository()
        {
            return new RtuActividadRepository(StrConexion);
        }

        public static IRtuConfiguracionRepository GetRtuConfiguracionRepository()
        {
            return new RtuConfiguracionRepository(StrConexion);
        }

        public static IRtuConfiguracionGrupoRepository GetRtuConfiguracionGrupoRepository()
        {
            return new RtuConfiguracionGrupoRepository(StrConexion);
        }

        public static IRtuConfiguracionPersonaRepository GetRtuConfiguracionPersonaRepository()
        {
            return new RtuConfiguracionPersonaRepository(StrConexion);
        }

        public static IRtuRolturnoRepository GetRtuRolturnoRepository()
        {
            return new RtuRolturnoRepository(StrConexion);
        }

        public static IRtuRolturnoActividadRepository GetRtuRolturnoActividadRepository()
        {
            return new RtuRolturnoActividadRepository(StrConexion);
        }

        public static IRtuRolturnoDetalleRepository GetRtuRolturnoDetalleRepository()
        {
            return new RtuRolturnoDetalleRepository(StrConexion);
        }

        #endregion

        public static IEveAreaSubcausaeventoRepository GetEveAreaSubcausaeventoRepository()
        {
            return new EveAreaSubcausaeventoRepository(StrConexion);
        }

        public static ISiConceptoRepository GetSiConceptoRepository()
        {
            return new SiConceptoRepository(StrConexion);
        }

        #region MonitoreoMME

        public static IEveCongesgdespachoRepository GetEveCongesgdespachoRepository()
        {
            return new EveCongesgdespachoRepository(StrConexion);
        }

        public static IMmmVersionRepository GetMmmVersionRepository()
        {
            return new MmmVersionRepository(StrConexion);
        }

        public static IMmmDatoRepository GetMmmDatoRepository()
        {
            return new MmmDatoRepository(StrConexion);
        }

        public static IMmmCambioversionRepository GetMmmCambioversionRepository()
        {
            return new MmmCambioversionRepository(StrConexion);
        }

        public static IPrGrupoxcnfbarRepository GetPrGrupoxcnfbarRepository()
        {
            return new PrGrupoxcnfbarRepository(StrConexion);
        }

        public static IMmmBandtolRepository GetMmmBandtolRepository()
        {
            return new MmmBandtolRepository(StrConexion);
        }

        public static IMmmIndicadorRepository GetMmmIndicadorRepository()
        {
            return new MmmIndicadorRepository(StrConexion);
        }

        public static IMmmJustificacionRepository GetMmmJustificacionRepository()
        {
            return new MmmJustificacionRepository(StrConexion);
        }

        #endregion

        #region Pronostico de la Demanda
        //ASSETEC.PRODEM3 20220401
        public static IPrnAgrupacionFormulasRepository GetPrnAgrupacionFormulasRepository()
        {
            return new PrnAgrupacionFormulasRepository(StrConexion);
        }

        public static IPrnServiciosAuxiliaresRepository GetServiciosAuxiliaresRepository()
        {
            return new PrnServiciosAuxiliaresRepository(StrConexion);
        }
        //ASSETEC.PRODEM3 20220401

        public static IPrnPronosticoDemandaRepository GetPrnPronosticoDemandaRepository()//Temporal
        {
            return new PrnPronosticoDemandaRepository(StrConexion);
        }

        public static IPrnMedicion48Repository GetPrnMedicion48Repository()
        {
            return new PrnMedicion48Repository(StrConexion);
        }

        public static IPrnMedicioneqRepository GetPrnMedicioneqRepository()
        {
            return new PrnMedicioneqRepository(StrConexion);
        }

        public static IPrnExogenamedicionRepository GetPrnExogenamedicionRepository()
        {
            return new PrnExogenamedicionRepository(StrConexion);
        }

        public static IPrnVariableexogenaRepository GetPrnVariableexogenaRepository()
        {
            return new PrnVariableexogenaRepository(StrConexion);
        }

        public static IPrnAreamedicionRepository GetPrnAreamedicionRepository()
        {
            return new PrnAreamedicionRepository(StrConexion);
        }

        public static IPrnClasificacionRepository GetPrnClasificacionRepository()
        {
            return new PrnClasificacionRepository(StrConexion);
        }

        public static IPrnFormularelRepository GetPrnFormularelRepository()
        {
            return new PrnFormularelRepository(StrConexion);
        }

        //Agregado 20190117
        public static IPrnAgrupacionRepository GetPrnAgrupacionRepository()
        {
            return new PrnAgrupacionRepository(StrConexion);
        }
        //

        //Agregado 20190212
        public static IPrnConfiguracionRepository GetPrnConfiguracionRepository()
        {
            return new PrnConfiguracionRepository(StrConexion);
        }
        //
        //Agregado 20200106
        public static IPrnReduccionRedRepository GetPrnReduccionRedRepository()
        {
            return new PrnReduccionRedRepository(StrConexion);
        }

        public static IPrnVersionRepository GetPrnVersionRepository()
        {
            return new PrnVersionRepository(StrConexion);
        }
        //

        //Agregado 20200319
        public static IPrnConfigbarraRepository GetPrnConfigbarraRepository()
        {
            return new PrnConfigbarraRepository(StrConexion);
        }
        //

        //Agregado 20200424
        public static IPrnMediciongrpRepository GetPrnMediciongrpRepository()
        {
            return new PrnMediciongrpRepository(StrConexion);
        }
        //

        //Agregado 20200520
        public static IPrnPrdTransversalRepository GetPrnPrdTransversalRepository()
        {
            return new PrnPrdTransversalRepository(StrConexion);
        }

        //Agregado 20200617
        public static IPrnPtomedpropRepository GetPrnPtomedpropRepository()
        {
            return new PrnPtomedpropRepository(StrConexion);
        }
        //Agregado 20211111 Estimador
        public static IPrnEstimadorRawRepository GetPrnEstimadorRawRepository()//Temporal
        {
            return new PrnEstimadorRawRepository(StrConexion);
        }
        public static IPrnVariableRepository GetPrnVariableRepository()//Temporal
        {
            return new PrnVariableRepository(StrConexion);
        }
        public static IPrnPtoEstimadorRepository GetPrnPtoEstimadorRepository()//Temporal
        {
            return new PrnPtoEstimadorRepository(StrConexion);
        }
        public static IPrnAsociacionRepository GetPrnAsociacionRepository()
        {
            return new PrnAsociacionRepository(StrConexion);
        }
        public static IPrnConfiguracionFormulaRepository GetPrnConfiguracionFormulaRepository()
        {
            return new PrnConfiguracionFormulaRepository(StrConexion);
        }
        public static IPrnMedicionesRawRepository GetPrnMedicionesRawRepository()
        {
            return new PrnMedicionesRawRepository(StrConexion);
        }
        public static IPrnRelacionTnaRepository GetPrnRelacionTnaRepository()
        {
            return new PrnRelacionTnaRepository(StrConexion);
        }
        //GetPrnVersiongrpReporsitory
        public static IPrnVersiongrpReporsitory GetPrnVersiongrpReporsitory()
        {
            return new PrnVersiongrpRepository(StrConexion);
        }

        // ---------------------------------------------------------------------------------------------------------------
        // ASSETEC 20-04-2022
        // ---------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Metodo Factoria de la entidad PRN_PRDTRANSVERSAL
        /// </summary>
        /// <returns></returns>
        public static IPrnPrdTransversalRepository GetPrnPrdTrasversalRepository()
        {
            return new PrnPrdTransversalRepository(StrConexion);
        }
        // ---------------------------------------------------------------------------------------------------------------
        // ASSETEC 14-07-2022
        // ---------------------------------------------------------------------------------------------------------------
        public static IPrnConfiguracionDiaRepository GetPrnConfiguracionDiaRepository()
        {
            return new PrnConfiguracionDiaRepository(StrConexion);
        }
        // ---------------------------------------------------------------------------------------------------------------
        // ASSETEC 03-10-2022
        // ---------------------------------------------------------------------------------------------------------------
        public static IPrnPerfilBarraRepository GetPrnPerfilBarraRepository()
        {
            return new PrnPerfilBarraRepository(StrConexion);
        }
        #endregion

        #region Corto Plazo

        public static ICpFuentegamsRepository GetCpFuentegamsRepository()
        {
            return new CpFuentegamsRepository(StrConexion);
        }

        public static IEqCircuitoRepository GetEqCircuitoRepository()
        {
            return new EqCircuitoRepository(StrConexion);
        }

        public static IEqCircuitoDetRepository GetEqCircuitoDetRepository()
        {
            return new EqCircuitoDetRepository(StrConexion);
        }

        public static ICpTopologiaRepository GetCptopologiaRepository()
        {
            return new CpTopologiaRepository(StrConexion);
        }

        public static ICpReprogramaRepository GetCpReprogramaRepository()
        {
            return new CpReprogramaRepository(StrConexion);
        }

        public static ICpRecursoRepository GetCpRecursoRepository()
        {
            return new CpRecursoRepository(StrConexion);
        }

        public static ICpRecurptomedRepository GetCpRecurptomedRepository()
        {
            return new CpRecurptomedRepository(StrConexion);
        }

        public static ICpProprecursoRepository GetCpProprecursoRepository()
        {
            return new CpProprecursoRepository(StrConexion);
        }


        public static ICpPropiedadRepository GetCpPropiedadRepository()
        {
            return new CpPropiedadRepository(StrConexion);
        }

        public static ICpDetalleetapaRepository GetCpDetalleetapaRepository()
        {
            return new CpDetalleetapaRepository(StrConexion);
        }

        public static ICpTerminalRepository GetCpTerminalRepository()
        {
            return new TerminalRepository(StrConexion);
        }

        public static ICpParametroRepository GetCpParametroRepository()
        {
            return new CpParametroRepository(StrConexion);
        }

        public static ICpSubrestriccionRepository GetCpSubrestriccionRepository()
        {
            return new CpSubrestriccionRepository(StrConexion);
        }

        public static ICpFcostofRepository GetCpFcostofRepository()
        {
            return new CpFcostofRepository(StrConexion);
        }

        public static ICpDetfcostofRepository GetCpDetfcostofRepository()
        {
            return new CpDetfcostofRepository(StrConexion);
        }

        public static ICpRelacionRepository GetCpRelacionRepository()
        {
            return new CpRelacionRepository(StrConexion);
        }

        public static ICpFuentegamsversionRepository GetCpFuentegamsversionRepository()
        {
            return new CpFuentegamsversionRepository(StrConexion);
        }

        public static ICpMedicion48Repository GetCpMedicion48Repository()
        {
            return new CpMedicion48Repository(StrConexion);
        }

        public static ICpTopologiaRepository GetCpTopologiaRepository()
        {
            return new CpTopologiaRepository(StrConexion);
        }

        public static ICpGruporecursoRepository GetCpGruporecursoRepository()
        {
            return new CpGruporecursoRepository(StrConexion);
        }
        public static ICpSubsrestricdatRepository GetCpSubrestricdatRepository()
        {
            return new CpSubrestricdatRepository(StrConexion);
        }
        #endregion

        #region RDO-Yupana Continuo

        public static ICpArbolContinuoRepository GetCpArbolContinuoRepository()
        {
            return new CpArbolContinuoRepository(StrConexion);
        }

        public static ICpNodoContinuoRepository GetCpNodoContinuoRepository()
        {
            return new CpNodoContinuoRepository(StrConexion);
        }

        public static ICpNodoDetalleRepository GetCpNodoDetalleRepository()
        {
            return new CpNodoDetalleRepository(StrConexion);
        }

        public static ICpNodoConceptoRepository GetCpNodoConceptoRepository()
        {
            return new CpNodoConceptoRepository(StrConexion);
        }

        public static ICpForzadoCabRepository GetCpForzadoCabRepository()
        {
            return new CpForzadoCabRepository(StrConexion);
        }

        public static ICpForzadoDetRepository GetCpForzadoDetRepository()
        {
            return new CpForzadoDetRepository(StrConexion);
        }

        public static ICpYupconCfgRepository GetCpYupconCfgRepository()
        {
            return new CpYupconCfgRepository(StrConexion);
        }

        public static ICpYupconCfgdetRepository GetCpYupconCfgdetRepository()
        {
            return new CpYupconCfgdetRepository(StrConexion);
        }

        public static ICpYupconEnvioRepository GetCpYupconEnvioRepository()
        {
            return new CpYupconEnvioRepository(StrConexion);
        }

        public static ICpYupconM48Repository GetCpYupconM48Repository()
        {
            return new CpYupconM48Repository(StrConexion);
        }

        public static ICpYupconTipoRepository GetCpYupconTipoRepository()
        {
            return new CpYupconTipoRepository(StrConexion);
        }

        #endregion

        #region Req. PMPO

        //- Aplicativo PMPO
        public static IPmoPeriodoRepository GetPmoPeriodoRepository()
        {
            return new PmoPeriodoRepository(StrConexion);
        }

        public static IPmoFormatoRepository GetPmoFormatoRepository()
        {
            return new PmoFormatoRepository(StrConexion);
        }

        public static IPmoDatPmhiTrRepository GetPmoDatPmhiTrRepository()
        {
            return new PmoDatPmhiTrRepository(StrConexion);
        }

        public static IPmoConfIndispEquipoRepository GetPmoConfIndispEquipoRepository()
        {
            return new PmoConfIndispEquipoRepository(StrConexion);
        }

        public static IPmoDatCgndRepository GetPmoDatCgndRepository()
        {
            return new PmoDatCgndRepository(StrConexion);
        }

        public static IPmoDatMgndRepository GetPmoDatMgndRepository()
        {
            return new PmoDatMgndRepository(StrConexion);
        }

        public static IPmoDatDbusRepository GetPmoDatDbusRepository()
        {
            return new PmoDatDbusRepository(StrConexion);
        }
        public static IPmoDatDbfRepository GetPmoDatDbfRepository()
        {
            return new PmoDatDbfRepository(StrConexion);
        }

        public static IPmoDatGndseRepository GetPmoDatGndseRepository()
        {
            return new PmoDatGndseRepository(StrConexion);
        }

        public static IPmoDatGenerateRepository GetPmoDatGenerateRepository()
        {
            return new PmoDatGenerateRepository(StrConexion);
        }

        public static IPmoAnioOperativoRepository GetPmoAnioOperativoRepository()
        {
            return new PmoAnioOperativoRepository(StrConexion);
        }

        public static IPmoFeriadoRepository GetPmoFeriadoRepository()
        {
            return new PmoFeriadoRepository(StrConexion);
        }

        public static IPmoMesRepository GetPmoMesRepository()
        {
            return new PmoMesRepository(StrConexion);
        }

        public static IPmoEstacionhRepository GetPmoEstacionhRepository()
        {
            return new PmoEstacionhRepository(StrConexion);
        }

        public static IPmoPtoxestacionRepository GetPmoPtoxestacionRepository()
        {
            return new PmoPtoxestacionRepository(StrConexion);
        }

        public static IPmoLogRepository GetPmoLogRepository()
        {
            return new PmoLogRepository(StrConexion);
        }

        public static IPmoQnEnvioRepository GetPmoQnEnvioRepository()
        {
            return new PmoQnEnvioRepository(StrConexion);
        }

        public static IPmoQnLecturaRepository GetPmoQnLecturaRepository()
        {
            return new PmoQnLecturaRepository(StrConexion);
        }

        public static IPmoQnConfenvRepository GetPmoQnConfenvRepository()
        {
            return new PmoQnConfenvRepository(StrConexion);
        }

        public static IPmoQnCambioenvioRepository GetPmoQnCambioenvioRepository()
        {
            return new PmoQnCambioenvioRepository(StrConexion);
        }

        public static IPmoQnMedicionRepository GetPmoQnMedicionRepository()
        {
            return new PmoQnMedicionRepository(StrConexion);
        }

        public static IPmoSddpCodigoRepository GetPmoSddpCodigoRepository()
        {
            return new PmoSddpCodigoRepository(StrConexion);
        }

        public static IPmoSddpTipoRepository GetPmoSddpTipoRepository()
        {
            return new PmoSddpTipoRepository(StrConexion);
        }

        public static IMpTopologiaRepository GetMpTopologiaRepository()
        {
            return new MpTopologiaRepository(StrConexion);
        }

        public static IMpRecursoRepository GetMpRecursoRepository()
        {
            return new MpRecursoRepository(StrConexion);
        }

        public static IMpRelRecursoPtoRepository GetMpRelRecursoPtoRepository()
        {
            return new MpRelRecursoPtoRepository(StrConexion);
        }

        public static IMpCategoriaRepository GetMpCategoriaRepository()
        {
            return new MpCategoriaRepository(StrConexion);
        }

        public static IMpProprecursoRepository GetMpProprecursoRepository()
        {
            return new MpProprecursoRepository(StrConexion);
        }

        public static IMpRelacionRepository GetMpRelacionRepository()
        {
            return new MpRelacionRepository(StrConexion);
        }

        public static IMpPropiedadRepository GetMpPropiedadRepository()
        {
            return new MpPropiedadRepository(StrConexion);
        }

        public static IMpTiporelacionRepository GetMpTiporelacionRepository()
        {
            return new MpTiporelacionRepository(StrConexion);
        }

        public static IMpRelRecursoEqRepository GetMpRelRecursoEqRepository()
        {
            return new MpRelRecursoEqRepository(StrConexion);
        }

        public static IMpRelRecursoSddpRepository GetMpRelRecursoSddpRepository()
        {
            return new MpRelRecursoSddpRepository(StrConexion);
        }
        #endregion

        public static IMePlazoptoRepository GetMePlazoptoRepository()
        {
            return new MePlazoptoRepository(StrConexion);
        }
        public static IAudProcesoRepository GetProcesoRepository()
        {
            return new AudProcesoRepository(StrConexion);
        }

        public static IAudRiesgoRepository GetRiesgoRepository()
        {
            return new AudRiesgoRepository(StrConexion);
        }

        public static IAudAuditoriaRepository GetAuditoriaRepository()
        {
            return new AudAuditoriaRepository(StrConexion);
        }

        #region Auditoria
        public static IAudArchivoRepository GetAudArchivoRepository()
        {
            return new AudArchivoRepository(StrConexion);
        }

        public static IAudAuditoriaRepository GetAudAuditoriaRepository()
        {
            return new AudAuditoriaRepository(StrConexion);
        }

        public static IAudAuditoriaprocesoRepository GetAudAuditoriaprocesoRepository()
        {
            return new AudAuditoriaprocesoRepository(StrConexion);
        }

        public static IAudAuditoriaplanificadaRepository GetAudAuditoriaplanificadaRepository()
        {
            return new AudAuditoriaplanificadaRepository(StrConexion);
        }

        public static IAudAudplanificadaProcesoRepository GetAudAudplanificadaProcesoRepository()
        {
            return new AudAudplanificadaProcesoRepository(StrConexion);
        }

        public static IAudElementoRepository GetAudElementoRepository()
        {
            return new AudElementoRepository(StrConexion);
        }

        public static IAudNotificacionRepository GetAudNotificacionRepository()
        {
            return new AudNotificacionRepository(StrConexion);
        }

        public static IAudNotificaciondestRepository GetAudNotificaciondestRepository()
        {
            return new AudNotificaciondestRepository(StrConexion);
        }

        public static IAudPlanauditoriaRepository GetAudPlanauditoriaRepository()
        {
            return new AudPlanauditoriaRepository(StrConexion);
        }

        public static IAudProcesoRepository GetAudProcesoRepository()
        {
            return new AudProcesoRepository(StrConexion);
        }

        public static IAudProgaudiElementoRepository GetAudProgaudiElementoRepository()
        {
            return new AudProgaudiElementoRepository(StrConexion);
        }

        public static IAudProgaudiHallazgosRepository GetAudProgaudiHallazgosRepository()
        {
            return new AudProgaudiHallazgosRepository(StrConexion);
        }

        public static IAudProgaudiInvolucradoRepository GetAudProgaudiInvolucradoRepository()
        {
            return new AudProgaudiInvolucradoRepository(StrConexion);
        }

        public static IAudProgramaauditoriaRepository GetAudProgramaauditoriaRepository()
        {
            return new AudProgramaauditoriaRepository(StrConexion);
        }

        public static IAudRequerimientoInformRepository GetAudRequerimientoInformRepository()
        {
            return new AudRequerimientoInformRepository(StrConexion);
        }

        public static IAudRequerimientoInfoArchivoRepository GetAudRequerimientoInfoArchivoRepository()
        {
            return new AudRequerimientoInfoArchivoRepository(StrConexion);
        }

        public static IAudRiesgoRepository GetAudRiesgoRepository()
        {
            return new AudRiesgoRepository(StrConexion);
        }

        public static IAudTablacodigoRepository GetAudTablacodigoRepository()
        {
            return new AudTablacodigoRepository(StrConexion);
        }

        public static IAudTablacodigoDetalleRepository GetAudTablacodigoDetalleRepository()
        {
            return new AudTablacodigoDetalleRepository(StrConexion);
        }
        #endregion

        #region Estructuras para almacenamiento de información para los aplicativos BI de Producción de energía y Máxima Demanda

        public static IAbiEnergiaxfuentenergiaRepository GetAbiEnergiaxfuentenergiaRepository()
        {
            return new AbiEnergiaxfuentenergiaRepository(StrConexion);
        }

        public static IWbResumenmddetalleRepository GetWbResumenmddetalleRepository()
        {
            return new WbResumenmddetalleRepository(StrConexion);
        }

        public static IWbResumenmdRepository GetWbResumenmdRepository()
        {
            return new WbResumenmdRepository(StrConexion);
        }

        public static IAbiFactorplantaRepository GetAbiFactorplantaRepository()
        {
            return new AbiFactorplantaRepository(StrConexion);
        }

        public static IAbiProdgeneracionRepository GetAbiProdgeneracionRepository()
        {
            return new AbiProdgeneracionRepository(StrConexion);
        }

        public static IAbiPotefecRepository GetAbiPotefecRepository()
        {
            return new AbiPotefecRepository(StrConexion);
        }

        public static IAbiMedidoresResumenRepository GetAbiMedidoresResumenRepository()
        {
            return new AbiMedidoresResumenRepository(StrConexion);
        }

        #endregion

        #region Devolucion
        public static IDaiAportanteRepository GetDaiAportanteRepository()
        {
            return new DaiAportanteRepository(StrConexion);
        }

        public static IDaiCalendariopagoRepository GetDaiCalendariopagoRepository()
        {
            return new DaiCalendariopagoRepository(StrConexion);
        }

        public static IDaiPresupuestoRepository GetDaiPresupuestoRepository()
        {
            return new DaiPresupuestoRepository(StrConexion);
        }

        public static IDaiTablacodigoRepository GetDaiTablacodigoRepository()
        {
            return new DaiTablacodigoRepository(StrConexion);
        }

        public static IDaiTablacodigoDetalleRepository GetDaiTablacodigoDetalleRepository()
        {
            return new DaiTablacodigoDetalleRepository(StrConexion);
        }
        #endregion  

        //Req. Mejoras PR16 09/07/19

        public static IIioSicliOsigFacturaRepository GetIioSicliOsigFacturaRepository()
        {
            return new IioSicliOsigFacturaRepository(StrConexion);
        }

        public static IMeDemandaMercadoLibreRepository GetMeDemandaMercadoLibreRepository()
        {
            return new MeDemandaMercadoLibreRepository(StrConexion);
        }

        #region Titularidad-Instalaciones-Empresas

        public static ISiMigracionRepository GetSiMigracionRepository()
        {
            return new SiMigracionRepository(StrConexion);
        }

        public static ISiTipomigraOperacionRepository GetSiTipomigraoperacionRepository()
        {
            return new SiTipomigraOperacionRepository(StrConexion);
        }

        public static ISiMigraemprOrigenRepository GetSiMigraemprorigenRepository()
        {
            return new SiMigraemprorigenRepository(StrConexion);
        }

        public static ISiEquipoMigrarRepository GetSiEquipomigrarRepository()
        {
            return new SiEquipomigrarRepository(StrConexion);
        }

        public static ISiMigragrupoRepository GetSiMigragrupoRepository()
        {
            return new SiMigragrupoRepository(StrConexion);
        }

        public static ISiMigraptomedicionRepository GetSiMigraptomedicionRepository()
        {
            return new SiMigraptomedicionRepository(StrConexion);
        }

        public static ISiMigraparametroRepository GetSiMigraparametroRepository()
        {
            return new SiMigraparametroRepository(StrConexion);
        }

        public static ISiMigraqueryparametroRepository GetSiMigraqueryparametroRepository()
        {
            return new SiMigraqueryparametroRepository(StrConexion);
        }

        public static ISiMigraquerybaseRepository GetSiMigraquerybaseRepository()
        {
            return new SiMigraquerybaseRepository(StrConexion);
        }

        public static ISiMigraqueryxtipooperacionRepository GetSiMigraqueryxtipooperacionRepository()
        {
            return new SiMigraqueryxtipooperacionRepository(StrConexion);
        }

        public static ISiMigracionlogRepository GetSiMigracionlogRepository()
        {
            return new SiMigracionlogRepository(StrConexion);
        }

        public static ISiLogmigraRepository GetSiLogmigraRepository()
        {
            return new SiLogmigraRepository(StrConexion);
        }

        public static ISiMigralogdbaRepository GetSiMigralogdbaRepository()
        {
            return new SiMigralogdbaRepository(StrConexion);
        }

        public static ISiEmpresadatRepository GetSiEmpresadatRepository()
        {
            return new SiEmpresadatRepository(StrConexion);
        }

        public static ISiHisempptoDataRepository GetSiHisempptoDataRepository()
        {
            return new SiHisempptoDataRepository(StrConexion);
        }

        public static ISiHisempeqRepository GetSiHisempeqRepository()
        {
            return new SiHisempeqRepository(StrConexion);
        }

        public static ISiHisempptoRepository GetSiHisempptoRepository()
        {
            return new SiHisempptoRepository(StrConexion);
        }

        public static ISiHisempgrupoRepository GetSiHisempgrupoRepository()
        {
            return new SiHisempgrupoRepository(StrConexion);
        }

        public static ISiHisempeqDataRepository GetSiHisempeqDataRepository()
        {
            return new SiHisempeqDataRepository(StrConexion);
        }

        public static ISiHisempgrupoDataRepository GetSiHisempgrupoDataRepository()
        {
            return new SiHisempgrupoDataRepository(StrConexion);
        }

        public static ISiMigraqueryplantillaRepository GetSiMigraqueryplantillaRepository()
        {
            return new SiMigraqueryplantillaRepository(StrConexion);
        }

        public static ISiMigraqueryplantparamRepository GetSiMigraqueryplantparamRepository()
        {
            return new SiMigraqueryplantparamRepository(StrConexion);
        }

        public static ISiMigraqueryplanttopRepository GetSiMigraqueryplanttopRepository()
        {
            return new SiMigraqueryplanttopRepository(StrConexion);
        }

        public static ISiHisempentidadRepository GetSiHisempentidadRepository()
        {
            return new SiHisempentidadRepository(StrConexion);
        }

        public static ISiHisempentidadDetRepository GetSiHisempentidadDetRepository()
        {
            return new SiHisempentidadDetRepository(StrConexion);
        }

        public static ISrmRecomendacionRepository GetSrmRecomendacionRepository()
        {
            return new SrmRecomendacionRepository(StrConexion);
        }

        public static ISrmComentarioRepository GetSrmComentarioRepository()
        {
            return new SrmComentarioRepository(StrConexion);
        }

        public static ISrmCriticidadRepository GetSrmCriticidadRepository()
        {
            return new SrmCriticidadRepository(StrConexion);
        }

        public static ISrmEstadoRepository GetSrmEstadoRepository()
        {
            return new SrmEstadoRepository(StrConexion);
        }

        #endregion

        #region GestionEoEpo
        public static IEpoConfiguraRepository GetEpoConfiguraRepository()
        {
            return new EpoConfiguraRepository(StrConexion);
        }

        public static IEpoEstudioEoRepository GetEpoEstudioEoRepository()
        {
            return new EpoEstudioEoRepository(StrConexion);
        }

        public static IEpoEstudioEpoRepository GetEpoEstudioEpoRepository()
        {
            return new EpoEstudioEpoRepository(StrConexion);
        }

        public static IEpoEstudioEstadoRepository GetEpoEstudioEstadoRepository()
        {
            return new EpoEstudioEstadoRepository(StrConexion);
        }

        public static IEpoHistoricoIndicadorRepository GetEpoHistoricoIndicadorRepository()
        {
            return new EpoHistoricoIndicadorRepository(StrConexion);
        }

        public static IEpoHistoricoIndicadorDetRepository GetEpoHistoricoIndicadorDetRepository()
        {
            return new EpoHistoricoIndicadorDetRepository(StrConexion);
        }

        public static IEpoIndicadorRepository GetEpoIndicadorRepository()
        {
            return new EpoIndicadorRepository(StrConexion);
        }

        public static IEpoPeriodoRepository GetEpoPeriodoRepository()
        {
            return new EpoPeriodoRepository(StrConexion);
        }

        public static IEpoRevisionEoRepository GetEpoRevisionEoRepository()
        {
            return new EpoRevisionEoRepository(StrConexion);
        }

        public static IEpoRevisionEpoRepository GetEpoRevisionEpoRepository()
        {
            return new EpoRevisionEpoRepository(StrConexion);
        }

        public static IEpoEstudioTerceroInvEoRepository GetEpoEstudioTerceroInvEoRepository()
        {
            return new EpoEstudioTerceroInvEoRepository(StrConexion);
        }

        public static IEpoEstudioTerceroInvEpoRepository GetEpoEstudioTerceroInvEpoRepository()
        {
            return new EpoEstudioTerceroInvEpoRepository(StrConexion);
        }

        public static IEpoDocDiaNoHabilRepository GetEpoDocDiaNoHabilRepository()
        {
            return new EpoDocDiaNoHabilRepository(StrConexion);
        }
        #endregion

        public static IWbRegistroModplanRepository GetWbRegistroModplanRepository()
        {
            return new WbRegistroModplanRepository(StrConexion);
        }

        public static IWbVersionModplanRepository GetWbVersionModplanRepository()
        {
            return new WbVersionModplanRepository(StrConexion);
        }
        public static IWbArchivoModplanRepository GetWbArchivoModplanRepository()
        {
            return new WbArchivoModplanRepository(StrConexion);
        }

        public static IWbComiteRepository GetWbComiteRepository()
        {
            return new WbComiteRepository(StrConexion);
        }

        public static IWbComiteListaRepository GetWbComiteListaRepository()
        {
            return new WbComiteListaRepository(StrConexion);
        }

        public static IWbComiteListaContactoRepository GetWbComiteListaContactoRepository()
        {
            return new WbComiteListaContactoRepository(StrConexion);
        }

        public static IWbComiteContactoRepository GetWbComiteContactoRepository()
        {
            return new WbComiteContactoRepository(StrConexion);
        }
        public static IWbProcesoRepository GetWbProcesoRepository()
        {
            return new WbProcesoRepository(StrConexion);
        }

        public static IWbProcesoContactoRepository GetWbProcesoContactoRepository()
        {
            return new WbProcesoContactoRepository(StrConexion);
        }
        #region Aplicativo Extranet CTAF

        public static IAfEmpresaRepository GetAfEmpresaRepository()
        {
            return new AfEmpresaRepository(StrConexion);
        }

        public static IAfCondicionesRepository GetAfCondicionesRepository()
        {
            return new AfCondicionesRepository(StrConexion);
        }

        public static IAfEracmfEventoRepository GetAfEracmfEventoRepository()
        {
            return new AfEracmfEventoRepository(StrConexion);
        }

        public static IAfEracmfZonaRepository GetAfEracmfZonaRepository()
        {
            return new AfEracmfZonaRepository(StrConexion);
        }

        public static IAfEventoRepository GetAfEventoRepository()
        {
            return new AfEventoRepository(StrConexion);
        }

        public static IAfHoraCoordRepository GetAfHoraCoordRepository()
        {
            return new AfHoraCoordRepository(StrConexion);
        }

        public static IAfInterrupSuministroRepository GetAfInterrupSuministroRepository()
        {
            return new AfInterrupSuministroRepository(StrConexion);
        }

        public static IAfSolicitudRespRepository GetAfSolicitudRespRepository()
        {
            return new AfSolicitudRespRepository(StrConexion);
        }

        public static IUnitOfWork UnitOfWork()
        {
            return new UnitOfWork(StrConexion);
        }

        #endregion

        #region GMM

        public static IGmmAgenteRepository GetGmmAgentesRepository()
        {
            return new GmmAgenteRepository(StrConexion);
        }

        public static IGmmGarantiaRepository GetGmmGarantiaRepository()
        {
            return new GmmGarantiaRepository(StrConexion);
        }
        public static IGmmEstadoEmpresaRepository GetGmmEstadoEmpresaRepository()
        {
            return new GmmEstadoEmpresaRepository(StrConexion);
        }
        public static IGmmTrienioRepository GetGmmTrienioRepository()
        {
            return new GmmTrienioRepository(StrConexion);
        }
        public static IGmmIncumplimientoRepository GetGmmIncumplimientoRepository()
        {
            return new GmmIncumplimientoRepository(StrConexion);
        }
        public static IGmmDetIncumplimientoRepository GetGmmDetIncumplimientoRepository()
        {
            return new GmmDetIncumplimientoRepository(StrConexion);
        }
        public static IGmmDatInsumoRepository GetGmmDatInsumoRepository()
        {
            return new GmmDatInsumoRepository(StrConexion);
        }
        public static IGmmValEnergiaRepository GetGmmValEnergiaRepository()
        {
            return new GmmValEnergiaRepository(StrConexion);
        }        
        public static IGmmValEnergiaEntregaRepository GetGmmValEnergiaEntregaRepository()
        {
            return new GmmValEnergiaEntregaRepository(StrConexion);
        }
        public static IGmmDatCalculoRepository GetGmmDatCalculoRepository()
        {
            return new GmmDatCalculoRepository(StrConexion);
        }
        #endregion

        //Compensacion Manual
        public static IVceCompMMEDetManualRepository GetVceCompMMEDetManualRepository()
        {
            return new VceCompMMEDetManualRepository(StrConexion);
        }

        public static IFwAccesoModeloRepository GetFwAccesoModeloRepository()
        {
            return new FwAccesoModeloRepository(StrConexion);
        }
        //Importacion Nuevas Tablas Osinergmin
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIioOsigConsumoUlRepository GetOsigConsumoRepository()
        {
            return new IioOsigConsumoUlRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIioOsigFacturaUlRepository GetOsigFacturaRepository()
        {
            return new IioOsigFacturaUlRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIioOsigSuministroUlRepository GetOsigSuministroRepository()
        {
            return new IioOsigSuministroUlRepository(StrConexion);
        }

        #region  Regiones_seguridad

        public static ICmRegionseguridadRepository GetCmRegionseguridadRepository()
        {
            return new CmRegionseguridadRepository(StrConexion);
        }

        public static ICmRegionseguridadDetalleRepository GetCmRegionseguridadDetalleRepository()
        {
            return new CmRegionseguridadDetalleRepository(StrConexion);
        }

        #endregion

        #region Potencia Firme

        public static IPfContratosRepository GetPfContratosRepository()
        {
            return new PfContratosRepository(StrConexion);
        }

        public static IPfCuadroRepository GetPfCuadroRepository()
        {
            return new PfCuadroRepository(StrConexion);
        }

        public static IPfEscenarioRepository GetPfEscenarioRepository()
        {
            return new PfEscenarioRepository(StrConexion);
        }

        public static IPfPeriodoRepository GetPfPeriodoRepository()
        {
            return new PfPeriodoRepository(StrConexion);
        }

        public static IPfPotenciaAdicionalRepository GetPfPotenciaAdicionalRepository()
        {
            return new PfPotenciaAdicionalRepository(StrConexion);
        }

        public static IPfPotenciaGarantizadaRepository GetPfPotenciaGarantizadaRepository()
        {
            return new PfPotenciaGarantizadaRepository(StrConexion);
        }

        public static IPfRecalculoRepository GetPfRecalculoRepository()
        {
            return new PfRecalculoRepository(StrConexion);
        }

        public static IPfRecursoRepository GetPfRecursoRepository()
        {
            return new PfRecursoRepository(StrConexion);
        }

        public static IPfReporteRepository GetPfReporteRepository()
        {
            return new PfReporteRepository(StrConexion);
        }

        public static IPfReporteTotalRepository GetPfReporteTotalRepository()
        {
            return new PfReporteTotalRepository(StrConexion);
        }

        public static IPfVersionRepository GetPfVersionRepository()
        {
            return new PfVersionRepository(StrConexion);
        }

        public static IPfRelacionInsumosRepository GetPfRelacionInsumosRepository()
        {
            return new PfRelacionInsumosRepository(StrConexion);
        }

        public static IPfRelacionIndisponibilidadesRepository GetPfRelacionIndisponibilidadesRepository()
        {
            return new PfRelacionIndisponibilidadesRepository(StrConexion);
        }

        public static IPfFactoresRepository GetPfFactoresRepository()
        {
            return new PfFactoresRepository(StrConexion);
        }

        public static IPfDispcalorutilRepository GetPfDispcalorutilRepository()
        {
            return new PfDispcalorutilRepository(StrConexion);
        }

        public static IPfReporteDetRepository GetPfReporteDetRepository()
        {
            return new PfReporteDetRepository(StrConexion);
        }

        #endregion

        #region Potencia Firme Remunerable
        public static IPfrCuadroRepository GetPfrCuadroRepository()
        {
            return new PfrCuadroRepository(StrConexion);
        }

        public static IPfrRelacionIndisponibilidadRepository GetPfrRelacionIndisponibilidadRepository()
        {
            return new PfrRelacionIndisponibilidadRepository(StrConexion);
        }

        public static IPfrEscenarioRepository GetPfrEscenarioRepository()
        {
            return new PfrEscenarioRepository(StrConexion);
        }

        public static IPfrPeriodoRepository GetPfrPeriodoRepository()
        {
            return new PfrPeriodoRepository(StrConexion);
        }

        public static IPfrRecalculoRepository GetPfrRecalculoRepository()
        {
            return new PfrRecalculoRepository(StrConexion);
        }

        public static IPfrRelacionPotenciaFirmeRepository GetPfrRelacionPotenciaFirmeRepository()
        {
            return new PfrRelacionPotenciaFirmeRepository(StrConexion);
        }

        public static IPfrReporteRepository GetPfrReporteRepository()
        {
            return new PfrReporteRepository(StrConexion);
        }

        public static IPfrReporteTotalRepository GetPfrReporteTotalRepository()
        {
            return new PfrReporteTotalRepository(StrConexion);
        }

        public static IPfrResultadosGamsRepository GetPfrResultadosGamsRepository()
        {
            return new PfrResultadosGamsRepository(StrConexion);
        }

        public static IPfrEntidadRepository GetPfrEntidadRepository()
        {
            return new PfrEntidadRepository(StrConexion);
        }

        public static IPfrEntidadDatRepository GetPfrEntidadDatRepository()
        {
            return new PfrEntidadDatRepository(StrConexion);
        }

        public static IPfrConceptoRepository GetPfrConceptoRepository()
        {
            return new PfrConceptoRepository(StrConexion);
        }

        public static IPfrTipoRepository GetPfrTipoRepository()
        {
            return new PfrTipoRepository(StrConexion);
        }

        #endregion

        #region Cálculo Consumo Combustible

        public static ICccReporteRepository GetCccReporteRepository()
        {
            return new CccReporteRepository(StrConexion);
        }

        public static ICccVersionRepository GetCccVersionRepository()
        {
            return new CccVersionRepository(StrConexion);
        }

        public static ICccVcomRepository GetCccVcomRepository()
        {
            return new CccVcomRepository(StrConexion);
        }

        public static ISiFactorconversionRepository GetSiFactorconversionRepository()
        {
            return new SiFactorconversionRepository(StrConexion);
        }

        public static IRepVcomRepository GetRepVcomRepository()
        {
            return new RepVcomRepository(StrConexion);
        }

        #endregion

        #region Mejoras Costos de Oportunidad - Movisoft

        public static ICoPeriodoRepository GetCoPeriodoRepository()
        {
            return new CoPeriodoRepository(StrConexion);
        }

        public static ICoProcesocalculoRepository GetCoProcesocalculoRepository()
        {
            return new CoProcesocalculoRepository(StrConexion);
        }

        public static ICoRaejecutadaRepository GetCoRaejecutadaRepository()
        {
            return new CoRaejecutadaRepository(StrConexion);
        }

        public static ICoTipoinformacionRepository GetCoTipoinformacionRepository()
        {
            return new CoTipoinformacionRepository(StrConexion);
        }

        public static ICoUrsEspecialRepository GetCoUrsEspecialRepository()
        {
            return new CoUrsEspecialRepository(StrConexion);
        }

        public static ICoUrsEspecialbaseRepository GetCoUrsEspecialbaseRepository()
        {
            return new CoUrsEspecialbaseRepository(StrConexion);
        }

        public static ICoVersionbaseRepository GetCoVersionbaseRepository()
        {
            return new CoVersionbaseRepository(StrConexion);
        }

        public static ICoVersionRepository GetCoVersionRepository()
        {
            return new CoVersionRepository(StrConexion);
        }

        public static ICoMedicion48Repository GetCoMedicion48Repository()
        {
            return new CoMedicion48Repository(StrConexion);
        }      

        public static ICoEnvioliquidacionRepository GetCoEnvioliquidacionRepository()
        {
            return new CoEnvioliquidacionRepository(StrConexion);
        }

        public static ICoRaejecutadadetRepository GetCoRaejecutadadetRepository()
        {
            return new CoRaejecutadadetRepository(StrConexion);
        }

        public static IEpoZonaRepository GetEpoZonaRepository()
        {
            return new EpoZonaRepository(StrConexion);
        }

        public static IEpoPuntoConexionRepository GetEpoPuntoConexionRepository()
        {
            return new EpoPuntoConexionRepository(StrConexion);
        }

        #region RSF_PR22
        public static ICoConfiguracionUrsRepository GetCoConfiguracionUrsRepository()
        {
            return new CoConfiguracionUrsRepository(StrConexion);
        }

        public static ICoConfiguracionDetRepository GetCoConfiguracionDetRepository()
        {
            return new CoConfiguracionDetRepository(StrConexion);
        }

        public static ICoTipoDatoRepository GetCoTipoDatoRepository()
        {
            return new CoTipoDatoRepository(StrConexion);
        }

        public static ICoConfiguracionSenialRepository GetCoConfiguracionSenialRepository()
        {
            return new CoConfiguracionSenialRepository(StrConexion);
        }

        public static ICoPeriodoProgRepository GetCoPeriodoProgRepository()
        {
            return new CoPeriodoProgRepository(StrConexion);
        }

        public static ICoProcesoDiarioRepository GetCoProcesoDiarioRepository()
        {
            return new CoProcesoDiarioRepository(StrConexion);
        }

        public static ICoProcesoErrorRepository GetCoProcesoErrorRepository()
        {
            return new CoProcesoErrorRepository(StrConexion);
        }

        public static ICoFactorUtilizacionRepository GetCoFactorUtilizacionRepository()
        {
            return new CoFactorUtilizacionRepository(StrConexion);
        }

        public static ICoMedicion60Repository GetCoMedicion60Repository()
        {
            return new CoMedicion60Repository(StrConexion);
        }
        #endregion
        #endregion

        #region Mejoras RDO
        public static IRdoCumplimientoRepository GetRdoCumplimientoRepository()
        {
            return new RdoCumplimientoRepository(StrConexion);
        }
        public static IRdoRepository GetRdoRepository()
        {
            return new RdoRepository(StrConexion);
        }
        #endregion

        #region Mejoras CMgN
        public static ICmUmbralComparacionRepository GetCmUmbralComparacionRepository()
        {
            return new CmUmbralComparacionRepository(StrConexion);
        }

        public static ICmCongestionCalculoRepository GetCmCongestionCalculoRepository()
        {
            return new CmCongestionCalculoRepository(StrConexion);
        }

        public static ICmCostoIncrementalRepository GetCmCostoIncrementalRepository()
        {
            return new CmCostoIncrementalRepository(StrConexion);
        }

        public static ICmBarraRelacionRepository GetCmBarraRelacionRepository()
        {
            return new CmBarraRelacionRepository(StrConexion);
        }

        public static ICmPeriodoRepository GetCmPeriodoRepository()
        {
            return new CmPeriodoRepository(StrConexion);
        }

        public static ICmUmbralreporteRepository GetCmUmbralreporteRepository()
        {
            return new CmUmbralreporteRepository(StrConexion);
        }

        public static ICmEquipobarraRepository GetCmEquipobarraRepository()
        {
            return new CmEquipobarraRepository(StrConexion);
        }

        public static ICmEquipobarraDetRepository GetCmEquipobarraDetRepository()
        {
            return new CmEquipobarraDetRepository(StrConexion);
        }

        public static ICmFpmdetalleRepository GetCmFpmdetalleRepository()
        {
            return new CmFpmdetalleRepository(StrConexion);
        }

        public static ICmFactorperdidaRepository GetCmFactorperdidaRepository()
        {
            return new CmFactorperdidaRepository(StrConexion);
        }

        public static ICmReporteRepository GetCmReporteRepository()
        {
            return new CmReporteRepository(StrConexion);
        }

        public static ICmReportedetalleRepository GetCmReportedetalleRepository()
        {
            return new CmReportedetalleRepository(StrConexion);
        }

        public static ICmBarraRelacionDetRepository GetCmBarraRelacionDetRepository()
        {
            return new CmBarraRelacionDetRepository(StrConexion);
        }
        
        public static ICmModeloAgrupacionRepository GetCmModeloAgrupacionRepository()
        {
            return new CmModeloAgrupacionRepository(StrConexion);
        }

        public static ICmModeloComponenteRepository GetCmModeloComponenteRepository()
        {
            return new CmModeloComponenteRepository(StrConexion);
        }

        public static ICmModeloConfiguracionRepository GetCmModeloConfiguracionRepository()
        {
            return new CmModeloConfiguracionRepository(StrConexion);
        }

        public static ICmModeloEmbalseRepository GetCmModeloEmbalseRepository()
        {
            return new CmModeloEmbalseRepository(StrConexion);
        }

        public static ICmVolumenDetalleRepository GetCmVolumenDetalleRepository()
        {
            return new CmVolumenDetalleRepository(StrConexion);
        }

        public static ICmVolumenCalculoRepository GetCmVolumenCalculoRepository()
        {
            return new CmVolumenCalculoRepository(StrConexion);
        }

        public static ICmVolumenInsensibilidadRepository GetCmVolumenInsensibilidadRepository()
        {
            return new CmVolumenInsensibilidadRepository(StrConexion);
        }

        #endregion

        #region Regiones de Seguridad

        public static ISegCoordenadaRepository GetSegCoordenadaRepository()
        {
            return new SegCoordenadaRepository(StrConexion);
        }

        public static ISegRegionRepository GetSegRegionRepository()
        {
            return new SegRegionRepository(StrConexion);
        }

        public static ISegRegionequipoRepository GetSegRegionequipoRepository()
        {
            return new SegRegionequipoRepository(StrConexion);
        }

        public static ISegRegiongrupoRepository GetSegRegiongrupoRepository()
        {
            return new SegRegiongrupoRepository(StrConexion);
        }

        public static ISegZonaRepository GetSegZonaRepository()
        {
            return new SegZonaRepository(StrConexion);
        }

        #endregion

        #region Ticket 2022-004245

        public static ICmGeneradorBarraemsRepository GetCmGeneradorBarraemsRepository()
        {
            return new CmGeneradorBarraemsRepository(StrConexion);
        }
        public static ICmGeneradorPotenciagenRepository GetCmGeneradorPotenciagenRepository()
        {
            return new CmGeneradorPotenciagenRepository(StrConexion);
        }

        #endregion

        #region IND.PR25.2022
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndCabeceraRepository GetIndCabeceraRepository()
        {
            return new IndCabeceraRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndCapacidadRepository GetIndCapacidadRepository()
        {
            return new IndCapacidadRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndCapacidadDiaRepository GetIndCapacidadDiaRepository()
        {
            return new IndCapacidadDiaRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndCrdSugadRepository GetIndCrdSugadRepository()
        {
            return new IndCrdSugadRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndCapacidadTransporteRepository GetIndCapacidadTransporteRepository()
        {
            return new IndCapacidadTransporteRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IIndTransporteDetalleRepository GetIndTransporteDetalleRepository()
        {
            return new IndTransporteDetalleRepository(StrConexion);
        }
        #endregion

        #region Mejoras CTAF
        public static IMeEnvioEveEventoRepository GetMeEnvioEveEventoRepository()
        {
            return new MeEnvioEveEventoRepository(StrConexion);
        }
        public static IEveInformesScoRepository GetEveInformesScoRepository()
        {
            return new EveInformesScoRepository(StrConexion);
        }
        #endregion

        #region RSF_PR22 Iteracion 2

        public static IRpfEnvioRepository GetRpfEnvioRepository()
        {
            return new RpfEnvioRepository(StrConexion);
        }

        public static IRpfMedicion60Repository GetRpfMedicion60Repository()
        {
            return new RpfMedicion60Repository(StrConexion);
        }
        public static ICoConfiguracionGenRepository GetCoConfiguracionGenRepository()
        {
            return new CoConfiguracionGenRepository(StrConexion);
        }

        #endregion

        #region FichaTecnica

        public static IFtFichaTecnicaRepository GetFtFichaTecnicaRepository()
        {
            return new FtFichaTecnicaRepository(StrConexion);
        }

        public static IFtFictecDetRepository GetFtFictecDetRepository()
        {
            return new FtFictecDetRepository(StrConexion);
        }

        public static IFtFictecItemRepository GetFtFictecItemRepository()
        {
            return new FtFictecItemRepository(StrConexion);
        }

        public static IFtFictecXTipoEquipoRepository GetFtFictecXTipoEquipoRepository()
        {
            return new FtFictecXTipoEquipoRepository(StrConexion);
        }

        public static IFtFictecNotaRepository GetFtFictecNotaRepository()
        {
            return new FtFictecNotaRepository(StrConexion);
        }

        public static IFtFictecPropRepository GetFtFictecPropRepository()
        {
            return new FtFictecPropRepository(StrConexion);
        }

        public static IFtFictecItemNotaRepository GetFtFictecItemNotaRepository()
        {
            return new FtFictecItemNotaRepository(StrConexion);
        }

        public static IPrGrupoeqRepository GetPrGrupoeqRepository()
        {
            return new PrGrupoeqRepository(StrConexion);
        }

        public static IFtFictecVisualentidadRepository GetFtFictecVisualentidadRepository()
        {
            return new FtFictecVisualentidadRepository(StrConexion);
        }

        public static IFtExtFormatoRepository GetFtExtFormatoRepository()
        {
            return new FtExtFormatoRepository(StrConexion);
        }

        public static IFtExtEnvioAreaRepository GetFtExtEnvioAreaRepository()
        {
            return new FtExtEnvioAreaRepository(StrConexion);
        }

        public static IFtExtEnvioRevareaRepository GetFtExtEnvioRevareaRepository()
        {
            return new FtExtEnvioRevareaRepository(StrConexion);
        }

        public static IFtExtEnvioReldatorevareaRepository GetFtExtEnvioReldatorevareaRepository()
        {
            return new FtExtEnvioReldatorevareaRepository(StrConexion);
        }

        public static IFtExtEnvioRelreqrevareaRepository GetFtExtEnvioRelreqrevareaRepository()
        {
            return new FtExtEnvioRelreqrevareaRepository(StrConexion);
        }

        public static IFtExtEnvioRelrevareaarchivoRepository GetFtExtEnvioRelrevareaarchivoRepository()
        {
            return new FtExtEnvioRelrevareaarchivoRepository(StrConexion);
        }

        public static IFtExtEnvioRelrevarchivoRepository GetFtExtEnvioRelrevarchivoRepository()
        {
            return new FtExtEnvioRelrevarchivoRepository(StrConexion);
        }

        public static IFtExtItemcfgRepository GetFtExtItemcfgRepository()
        {
            return new FtExtItemcfgRepository(StrConexion);
        }

        public static IFtExtEventoRepository GetFtExtEventoRepository()
        {
            return new FtExtEventoRepository(StrConexion);
        }

        public static IFtExtEventoReqRepository GetFtExtEventoReqRepository()
        {
            return new FtExtEventoReqRepository(StrConexion);
        }

        public static IFtExtReleqempltRepository GetFtExtReleqempltRepository()
        {
            return new FtExtReleqempltRepository(StrConexion);
        }

        public static IFtExtReleqpryRepository GetFtExtReleqpryRepository()
        {
            return new FtExtReleqpryRepository(StrConexion);
        }

        public static IFtExtRelempetapaRepository GetFtExtRelempetapaRepository()
        {
            return new FtExtRelempetapaRepository(StrConexion);
        }

        public static IFtExtEtempdetpryRepository GetFtExtEtempdetpryRepository()
        {
            return new FtExtEtempdetpryRepository(StrConexion);
        }

        public static IFtExtEtempdetpryeqRepository GetFtExtEtempdetpryeqRepository()
        {
            return new FtExtEtempdetpryeqRepository(StrConexion);
        }

        public static IFtExtEtempdeteqRepository GetFtExtEtempdeteqRepository()
        {
            return new FtExtEtempdeteqRepository(StrConexion);
        }

        public static IFtExtEtapaRepository GetFtExtEtapaRepository()
        {
            return new FtExtEtapaRepository(StrConexion);
        }

        public static IFtExtProyectoRepository GetFtExtProyectoRepository()
        {
            return new FtExtProyectoRepository(StrConexion);
        }

        public static IFtExtEnvioRepository GetFtExtEnvioRepository()
        {
            return new FtExtEnvioRepository(StrConexion);
        }

        public static IFtExtEnvioReleeqrevRepository GetFtExtEnvioReleeqrevRepository()
        {
            return new FtExtEnvioReleeqrevRepository(StrConexion);
        }

        public static IFtExtRelpltcorretapaRepository GetFtExtRelpltcorretapaRepository()
        {
            return new FtExtRelpltcorretapaRepository(StrConexion);
        }

        public static IFtExtEnvioArchivoRepository GetFtExtEnvioArchivoRepository()
        {
            return new FtExtEnvioArchivoRepository(StrConexion);
        }

        public static IFtExtEnvioDatoRepository GetFtExtEnvioDatoRepository()
        {
            return new FtExtEnvioDatoRepository(StrConexion);
        }

        public static IFtExtEnvioEqRepository GetFtExtEnvioEqRepository()
        {
            return new FtExtEnvioEqRepository(StrConexion);
        }

        public static IFtExtEnvioLogRepository GetFtExtEnvioLogRepository()
        {
            return new FtExtEnvioLogRepository(StrConexion);
        }

        public static IFtExtEnvioReldatoarchivoRepository GetFtExtEnvioReldatoarchivoRepository()
        {
            return new FtExtEnvioReldatoarchivoRepository(StrConexion);
        }

        public static IFtExtEnvioRelreqarchivoRepository GetFtExtEnvioRelreqarchivoRepository()
        {
            return new FtExtEnvioRelreqarchivoRepository(StrConexion);
        }

        public static IFtExtEnvioReqRepository GetFtExtEnvioReqRepository()
        {
            return new FtExtEnvioReqRepository(StrConexion);
        }

        public static IFtExtEnvioVersionRepository GetFtExtEnvioVersionRepository()
        {
            return new FtExtEnvioVersionRepository(StrConexion);
        }

        public static IFtExtCorreoareaRepository GetFtExtCorreoareaRepository()
        {
            return new FtExtCorreoareaRepository(StrConexion);
        }

        public static IFtExtCorreoareadetRepository GetFtExtCorreoareadetRepository()
        {
            return new FtExtCorreoareadetRepository(StrConexion);
        }

        public static IFtExtEnvioReldatorevRepository GetFtExtEnvioReldatorevRepository()
        {
            return new FtExtEnvioReldatorevRepository(StrConexion);
        }

        public static IFtExtEnvioRelreqrevRepository GetFtExtEnvioRelreqrevRepository()
        {
            return new FtExtEnvioRelreqrevRepository(StrConexion);
        }

        public static IFtExtEnvioRevisionRepository GetFtExtEnvioRevisionRepository()
        {
            return new FtExtEnvioRevisionRepository(StrConexion);
        }

        public static IFtExtRelAreareqRepository GetFtExtRelAreareqRepository()
        {
            return new FtExtRelAreareqRepository(StrConexion);
        }

        public static IFtExtRelareaitemcfgRepository GetFtExtRelareaitemcfgRepository()
        {
            return new FtExtRelareaitemcfgRepository(StrConexion);
        }

        public static IFwUserRepository GetFwUserRepository()
        {
            return new FwUserRepository(StrConexion);
        }

        public static IIntDirectorioRepository GetIntDirectorioRepository()
        {
            return new IntDirectorioRepository(StrConexion);
        }

        #endregion

        #region Ticket 6964
        public static ICoDatoSenialRepository GetCoDatoSenialRepository()
        {
            return new CoDatoSenialRepository(StrConexion);
        }
        #endregion

        #region DemandaPO Iteracción 1
        /// <summary>
        /// Factory para los metodos del objeto DpoEstimadorRawRepository
        /// </summary>
        /// <returns>DpoEstimadorRawRepository</returns>
        public static IDpoEstimadorRawRepository GetDpoEstimadorRawRepository()
        {
            return new DpoEstimadorRawRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoDemandaScoRepository
        /// </summary>
        /// <returns>DpoDemandaScoRepository</returns>
        public static IDpoDemandaScoRepository GetDpoDemandaScoRepository()
        {
            return new DpoDemandaScoRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoFeriadosRepository
        /// </summary>
        /// <returns>DpoFeriadosRepository</returns>
        public static IDpoFeriadosRepository GetDpoFeriadosRepository()
        {
            return new DpoFeriadosRepository(StrConexion);
        }
        #endregion

        #region DemandaPO Iteracion 2
        /// <summary>
        /// Factory para los metodos del objeto DpoRelacionPtoBarraRepository
        /// </summary>
        /// <returns>DpoRelacionPtoBarraRepository</returns>
        public static IDpoRelacionPtoBarraRepository GetDpoRelacionPtoBarraRepository()
        {
            return new DpoRelacionPtoBarraRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoDatos96Repository
        /// </summary>
        /// <returns>DpoDatos96Repository</returns>
        public static IDpoDatos96Repository GetDpoDatos96Repository()
        {
            return new DpoDatos96Repository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoBarraRepository
        /// </summary>
        /// <returns>DpoBarraRepository</returns>
        public static IDpoBarraRepository GetDpoBarraRepository()
        {
            return new DpoBarraRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoTrafoBarraRepository
        /// </summary>
        /// <returns>DpoTrafoBarraRepository</returns>
        public static IDpoTrafoBarraRepository GetDpoTrafoBarraRepository()
        {
            return new DpoTrafoBarraRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoSubestacionRepository
        /// </summary>
        /// <returns>DpoSubestacionRepository</returns>
        public static IDpoSubestacionRepository GetDpoSubestacionRepository()
        {
            return new DpoSubestacionRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoTransformadorRepository
        /// </summary>
        /// <returns>DpoTransformadorRepository</returns>
        public static IDpoTransformadorRepository GetDpoTransformadorRepository()
        {
            return new DpoTransformadorRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoRelSplFormulaRepository
        /// </summary>
        /// <returns>DpoRelSplFormulaRepository</returns>
        public static IDpoRelSPlFormulaRepository GetDpoRelSplFormulaRepository()
        {
            return new DpoRelSplFormulaRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoBarraSplRepository
        /// </summary>
        /// <returns>DpoBarraSplRepository</returns>
        public static IDpoBarraSplRepository GetDpoBarraSplRepository()
        {
            return new DpoBarraSplRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoVersionRelacionRepository
        /// </summary>
        /// <returns>DpoVersionRelacionRepository</returns>
        public static IDpoVersionRelacionRepository GetDpoVersionRelacionRepository()
        {
            return new DpoVersionRelacionRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDpoMedicion96Repository GetDpoMedicion96Repository()
        {
            return new DpoMedicion96Repository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDpoConfiguracionRepository GetDpoConfiguracionRepository()
        {
            return new DpoConfiguracionRepository(StrConexion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IDpoEstadoVersionRepository GetDpoEstadoVersionRepository()
        {
            return new DpoEstadoVersionRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoCasoRepository
        /// </summary>
        /// <returns>DpoCasoRepository</returns>
        public static IDpoCasoRepository GetDpoCasoRepository()
        {
            return new DpoCasoRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoFuncionRepository
        /// </summary>
        /// <returns>DpoFuncionRepository</returns>
        public static IDpoFuncionRepository GetDpoFuncionRepository()
        {
            return new DpoFuncionRepository(StrConexion);
        }

        /// <summary>
        /// Factory para los metodos del objeto DpoCasoDetalleRepository
        /// </summary>
        /// <returns>DpoCasoDetalleRepository</returns>
        public static IDpoCasoDetalleRepository GetDpoCasoDetalleRepository()
        {
            return new DpoCasoDetalleRepository(StrConexion);
        }
        #endregion

        #region REQ 2023-000143
        public static IHtCentralCfgRepository GetHtCentralCfgRepository()
        {
            return new HtCentralCfgRepository(StrConexion);
        }

        public static IHtCentralCfgdetRepository GetHtCentralCfgdetRepository()
        {
            return new HtCentralCfgdetRepository(StrConexion);
        }
        #endregion

        #region Calculo de resarcimientos

        public static IReCausaInterrupcionRepository GetReCausaInterrupcionRepository()
        {
            return new ReCausaInterrupcionRepository(StrConexion);
        }

        public static IReEventoPeriodoRepository GetReEventoPeriodoRepository()
        {
            return new ReEventoPeriodoRepository(StrConexion);
        }

        public static IReIndicadorPeriodoRepository GetReIndicadorPeriodoRepository()
        {
            return new ReIndicadorPeriodoRepository(StrConexion);
        }

        public static IReIngresoTransmisionRepository GetReIngresoTransmisionRepository()
        {
            return new ReIngresoTransmisionRepository(StrConexion);
        }

        public static IReMaestroEtapaRepository GetReMaestroEtapaRepository()
        {
            return new ReMaestroEtapaRepository(StrConexion);
        }

        public static IReNivelTensionRepository GetReNivelTensionRepository()
        {
            return new ReNivelTensionRepository(StrConexion);
        }

        public static IRePeriodoRepository GetRePeriodoRepository()
        {
            return new RePeriodoRepository(StrConexion);
        }

        public static IRePeriodoEtapaRepository GetRePeriodoEtapaRepository()
        {
            return new RePeriodoEtapaRepository(StrConexion);
        }

        public static IRePtoentregaPeriodoRepository GetRePtoentregaPeriodoRepository()
        {
            return new RePtoentregaPeriodoRepository(StrConexion);
        }

        public static IRePuntoEntregaRepository GetRePuntoEntregaRepository()
        {
            return new RePuntoEntregaRepository(StrConexion);
        }

        public static IReTipoInterrupcionRepository GetReTipoInterrupcionRepository()
        {
            return new ReTipoInterrupcionRepository(StrConexion);
        }

        public static IReToleranciaPeriodoRepository GetReToleranciaPeriodoRepository()
        {
            return new ReToleranciaPeriodoRepository(StrConexion);
        }

        public static IReEnvioRepository GetReEnvioRepository()
        {
            return new ReEnvioRepository(StrConexion);
        }

        public static IReDeclaracionSuministroRepository GetReDeclaracionSuministroRepository()
        {
            return new ReDeclaracionSuministroRepository(StrConexion);
        }

        public static IReInterrupcionSuministroRepository GetReInterrupcionSuministroRepository()
        {
            return new ReInterrupcionSuministroRepository(StrConexion);
        }

        public static IReInterrupcionSuministroDetRepository GetReInterrupcionSuministroDetRepository()
        {
            return new ReInterrupcionSuministroDetRepository(StrConexion);
        }

        public static IReRechazoCargaRepository GetReRechazoCargaRepository()
        {
            return new ReRechazoCargaRepository(StrConexion);
        }

        public static IReEventoLogenvioRepository GetReEventoLogenvioRepository()
        {
            return new ReEventoLogenvioRepository(StrConexion);
        }

        public static IReEventoMedicionRepository GetReEventoMedicionRepository()
        {
            return new ReEventoMedicionRepository(StrConexion);
        }

        public static IReEventoProductoRepository GetReEventoProductoRepository()
        {
            return new ReEventoProductoRepository(StrConexion);
        }

        public static IReEventoSuministradorRepository GetReEventoSuministradorRepository()
        {
            return new ReEventoSuministradorRepository(StrConexion);
        }

        public static IReInterrupcionAccesoRepository GetReInterrupcionAccesoRepository()
        {
            return new ReInterrupcionAccesoRepository(StrConexion);
        }

        public static IReLogcorreoRepository GetReLogcorreoRepository()
        {
            return new ReLogcorreoRepository(StrConexion);
        }

        public static IReTipocorreoRepository GetReTipocorreoRepository()
        {
            return new ReTipocorreoRepository(StrConexion);
        }

        public static IReInterrupcionInsumoRepository GetReInterrupcionInsumoRepository()
        {
            return new ReInterrupcionInsumoRepository(StrConexion);
        }
        public static IRePtoentregaSuministradorRepository GetRePtoentregaSuministradorRepository()
        {
            return new RePtoentregaSuministradorRepository(StrConexion);
        }

        #endregion

        #region Informes SGI
        public static IMeRfriaUnidadrestricRepository GetMeRfriaUnidadrestricRepository()
        {
            return new MeRfriaUnidadrestricRepository(StrConexion);
        }

        public static IMeInformeInterconexionRepository GetMeInformeInterconexionRepository()
        {
            return new MeInformeInterconexionRepository(StrConexion);
        }
        public static IMeInformeAntecedenteRepository GetMeInformeAntecedenteRepository()
        {
            return new MeInformeAntecedenteRepository(StrConexion);
        }
        public static ITrnBarraAreaRepository GetTrnBarraAreaRepository()
        {
            return new TrnBarraAreaRepository(StrConexion);
        }

        public static IMeEnvcorreoConfRepository GetMeEnvcorreoConfRepository()
        {
            return new MeEnvcorreoConfRepository(StrConexion);
        }

        public static IMeEnvcorreoFormatoRepository GetMeEnvcorreoFormatoRepository()
        {
            return new MeEnvcorreoFormatoRepository(StrConexion);
        }

        public static ISiMenureporteGraficoRepository GetSiMenureporteGraficoRepository()
        {
            return new SiMenureporteGraficoRepository(StrConexion);
        }

        #endregion

        #region COES Storage

        public static IWbBlobRepository GetWbBlobRepository()
        {
            return new WbBlobRepository(StrConexion);
        }

        public static IWbBlobconfigRepository GetWbBlobconfigRepository()
        {
            return new WbBlobconfigRepository(StrConexion);
        }

        public static IWbBlobfuenteRepository GetWbBlobfuenteRepository()
        {
            return new WbBlobfuenteRepository(StrConexion);
        }

        #endregion

        public static IEveintdescargaRepository GetEveintdescargaRepository()
        {
            return new EveintdescargaRepository(StrConexion);
        }
        public static IEveCondPreviaRepository GetEveCondPreviaRepository()
        {
            return new EveCondPreviaRepository(StrConexion);
        }
        public static IEveHistoricoScadaRepository GetEveHistoricoScadaRepository()
        {
            return new EveHistoricoScadaRepository(StrConexion);
        }
        public static IEveAnalisisEventoRepository GetEveAnalisisEventoRepository()
        {
            return new EveAnalisisEventoRepository(StrConexion);
        }

        public static IEveRecomobservRepository GetEveRecomobservRepository()
        {
            return new EveRecomobservRepository(StrConexion);
        }
        public static IEveTiposNumeralRepository GetEveTiposNumeralRepository()
        {
            return new EveTiposNumeralRepository(StrConexion);
        }
        /// <summary>
        /// Permite conectar a interfaz de CrEvento
        /// </summary>
        public static ICrEventoRepository GetCrEventoRepository()
        {
            return new CrEventoRepository(StrConexion);
        }
        /// <summary>
        /// Permite conectar a interfaz de CrEtapaEvento
        /// </summary>
        public static ICrEtapaEventoRepository GetCrEtapaEventoRepository()
        {
            return new CrEtapaEventoRepository(StrConexion);
        }
        /// <summary>
        /// Permite conectar a interfaz de CrEmpresaResponsable
        /// </summary>
        public static ICrEmpresaResponsableRepository GetCrEmpresaResponsableRepository()
        {
            return new CrEmpresaResponsableRepository(StrConexion);
        }
        /// <summary>
        /// Permite conectar a interfaz de CrEmpresaSolicitante
        /// </summary>
        public static ICrEmpresaSolicitanteRepository GetCrEmpresaSolicitanteRepository()
        {
            return new CrEmpresaSolicitanteRepository(StrConexion);
        }
        /// <summary>
        /// Permite conectar a interfaz de CrEmpresaCriterio
        /// </summary>
        public static ICrEtapaCriterioRepository GetCrEtapaCriterioRepository()
        {
            return new CrEtapaCriterioRepository(StrConexion);
        }

        public static IEqRelacionTnaRepository GetEqRelacionTnaRepository()
        {
            return new EqRelacionTnaRepository(StrConexion);
        }

        /// <summary>
        /// Permite conectar a interfaz de ExtRatio
        /// </summary>
        public static IExtRatioRepository GetExtRatioRepository() 
        { 
            return new ExtRatioRepository(StrConexion);
        }


        #region GESPROTEC - 05112024
        public static IEprProyectoActEqpRepository GetEprProyectoActEqpRepository()
        {
            return new EprProyectoActEqpRepository(StrConexion);
        }

        public static IEprAreaRepository GetEprAreaRepository()
        {
            return new EprAreaRepository(StrConexion);
        }

        public static IEprEquipoRepository GetEprEquipoRepository()
        {
            return new EprEquipoRepository(StrConexion);
        }

        public static IEprSubestacionRepository GetEprSubestacionRepository()
        {
            return new EprSubestacionRepository(StrConexion);
        }

        /// <summary>
        /// Implementacion para la Carga Masiva
        /// </summary>
        /// <returns></returns>
        public static IEprCargaMasivaRepository GetEprCargaMasivaRepository()
        {
            return new EprCargaMasivaRepository(StrConexion);
        }


        public static IEprPropCatalogoDataRepository GetEprPropCatalogoDataRepository()
        {
            return new EprPropCatalogoDataRepository(StrConexion);
        }

        public static IEprRepLimitCapRepository GetEprRepLimitCapRepository()
        {
            return new EprRepLimitCapRepository(StrConexion);
        }

        #endregion

        #region GESPROTEC - 18022025
        /// <summary>
        /// implementacion para la obtencion de formulas para calculos
        /// </summary>
        /// <returns></returns>
        public static IEprCalculosRepository GetEprCalculosRepository()
        {
            return new EprCalculosRepository(StrConexion);
        }
        #endregion

        #region DemandaPO - Proceso Pronostico Vegetativo 20240902
        public static IDpoProcesoPronosticoRepository GetDpoProcesoPronosticoRepository()
        {
            return new DpoProcesoPronosticoRepository(StrConexion);
        }
        #endregion

    }
}