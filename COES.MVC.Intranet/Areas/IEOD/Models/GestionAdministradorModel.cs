using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class GestionAdministradorModel
    {
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Mes { get; set; }
        public string MesInicio { get; set; }
        public string MesFin { get; set; }
        public List<MeEstadoenvioDTO> ListaEstadoEnvio { get; set; }
        public List<MeFormatoDTO> ListaFormato { get; set; }

        public List<MeEnvioDTO> ListaEnvio { get; set; }
        public List<MeCambioenvioDTO> ListaCambioEnvio { get; set; }
        public List<MeValidacionDTO> ListaValidacion { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public string Resultado { get; set; }
        public string NombreFortmato { get; set; }

        public int Columnas { get; set; }
        public int Resolucion { get; set; }

        public string Fecha { get; set; }
        public string FechaPlazo { get; set; }
        public int HoraPlazo { get; set; }
        public int DiaMes { get; set; }
    }

    public class BusquedaIEODModel
    {
        public List<EqEquipoDTO> ListaEquipo { get; set; }
        public List<EqFamiliaDTO> ListaTipoEquipo { get; set; }
        public List<MePerfilRuleDTO> ListaFormulas { get; set; }
        public List<MeReporptomedDTO> ListaAreaOperativa { get; set; }
        public List<EqAreaDTO> ListaSubEstacion { get; set; }
        public List<PrGrupoDTO> ListaModo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqEquipoDTO> ListaUnidades { get; set; }
        public List<EstadoModel> listaEstadoSistemaA { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresas { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<EqAreaDTO> ListaUbicacion { get; set; }
        public List<EqEquipoDTO> ListaAreas { get; set; }
        public List<FamiliaDTO> ListaFamilias { get; set; }
        public List<EveEvenclaseDTO> ListaTipoMantenimiento { get; set; }
        public List<PrGrupoDTO> ListaGrupoCentral { get; set; }
        public List<EqFamiliaDTO> ListaTipoCentral { get; set; }
        public List<MeMedicion48DTO> ListaTipoCentrales { get; set; }
        public List<MeMedicion48DTO> ListaTipoRecurso { get; set; }
        public List<MeMedicion48DTO> ListaCombustibles { get; set; }
        public List<EveCausaeventoDTO> ListaCausa { get; set; }
        public List<SiFuenteenergiaDTO> ListaTipoCombustibles { get; set; }
        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }
        public List<PrTipogrupoDTO> ListaClasificacion { get; set; }
        public List<MeMedicion48DTO> ListaPotenciaxTipoRecurso { get; set; }
        public List<MeGpsDTO> ListaGps { get; set; }
        public List<SiTipogeneracionDTO> ListTipogeneracion { get; set; }

        //filtros
        public List<SiVersionDTO> ListaVersion { get; set; }
        public int Verscodi { get; set; }

        public DateTime FechaPeriodo { get; set; }
        public string Anho { get; set; }
        public string AnhoIni { get; set; }
        public string AnhoFin { get; set; }
        public string Mes { get; set; }
        public string Semana { get; set; }
        public string SemanaIni { get; set; }
        public string SemanaFin { get; set; }
        public string Dia { get; set; }
        public string Fecha { get; set; }
        public int NroSemana { get; set; }
        public List<GenericoDTO> ListaSemanas2 { get; set; }
        public List<TipoInformacionPR5> ListaSemanas { get; set; }
        public List<TipoInformacionPR5> ListaSemanasIni { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }

        public int TensionBarra { get; set; }
        public int Formatcodi { get; set; }

        //Paginado
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }

        public string Menu { get; set; }

        public string FiltroFechaDesc { get; set; }
        public List<ListaSelect> Reptiprepcodi { get; set; }
        public List<SiMenureporteDTO> ListMenuCate { get; set; }
        public List<SiMenureporteDTO> ListMenuReporte { get; set; }
        public int CountMenu { get; set; }

        #region AMPLIACION
        public List<MeFormatoDTO> ListaFormato { get; set; }
        public List<MeLecturaDTO> ListaLectura { get; set; }
        public string StrFormatCodi { get; set; }
        public string StrFormatPeriodo { get; set; }
        public int IdModulo { get; set; }
        public List<MeAmpliacionfechaDTO> ListaAmpliacion { get; set; }
        public string FechaPlazo { get; set; }
        public int CodigoApp { get; set; }
        #endregion
        public string MagnitudRPF { get; set; }

        public List<ModoOperacionPR5> ListModosOpeGrupos { get; set; }

        public List<MeTipopuntomedicionDTO> ListaTipoPtoMedicion { get; set; }
        public List<EqEquipoDTO> ListaCuenca { get; set; }

        public int Reporcodi { get; set; }
        public bool TieneFiltroDobleFecha { get; set; }
        public string TituloWeb { get; set; }

        public List<TipoDashboardIEOD> ListaTipoDashboard { get; set; }
        public string Url { get; set; }

        public List<TipoInformacion2> ListaTipoAgente { get; set; }
        public List<TipoInformacion> ListaCentralIntegrante { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<EqCategoriaDetDTO> ListaYacimientos { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public int Idnumeral { get; set; }
        public int Tiporeporte { get; set; }

        //Hojas
        public List<SiMenureporteHojaDTO> ListaHojaExcel { get; set; }

        //Graficos en Reporte
        public List<MeReporteDTO> ListaGraficosReporte { get; set; }
    }

    public class ParametroModel
    {
        public ParametroMagnitudRPF MagnitudRPF { get; set; }
        public List<ParametroMagnitudRPF> ListaMagnitudRPF { get; set; }
        public List<EstadoParametro> ListaEstado { get; set; }
        public string IdPeriodoAvenida { get; set; }
        public string IdPeriodoEstiaje { get; set; }
        public int Anho { get; set; }

        //parametros aplicativo
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public SiParametroDTO ParametroUmbral { get; set; }
        public int Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }

        //Umabral
        public decimal? ValorUmbral { get; set; }
        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }
    }

    public class UnidadesRFriaModel
    { 
        public List<MeRfriaUnidadrestricDTO> ListadoUnidades { get; set; }
        public MeRfriaUnidadrestricDTO Entidad { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<PrGrupoDTO> ListaCentral { get; set; }
        public string Fecha { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public int Codigo { get; set; }
        public int Grupocodi { get; set; }
        public string Observacion { get; set; }
        public int Emprcodi { get; set; }
        public int Centralcodi { get; set; }
    }
}