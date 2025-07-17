using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.IEOD;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IEOD.Models
{
    public class HorasOperacionModel
    {
        public List<EqFamiliaDTO> ListaTipoCentral { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<AreaDTO> ListaArea { get; set; }

        public string Fecha { get; set; }
        public string FechaFin { get; set; }
        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }

        public int FlagCentralRsvFriaToRegistrarUnidad { get; set; }
        public string EtiquetaFiltro { get; set; }
        public string EtiquetaFiltroGrupoModo { get; set; }
        public string ActFiltroCtral { get; set; }
        public List<UnidadesExtra> MatrizunidadesExtra { get; set; }

        public int IdEmpresa { get; set; }
        public int IdTipoCentral { get; set; }
        public int IdCentralSelect { get; set; }
        public string FechaIni { get; set; }
        public string Hophorordarranq { get; set; }
        public string HoraIni { get; set; }
        public string Hophorparada { get; set; }
        public string HoraFin { get; set; }
        public string Hopdesc { get; set; }
        public string Hopobs { get; set; }
        public int TipoModOp { get; set; }
        public int IdTipoOperSelect { get; set; }
        public int IdMotOpForzadaSelect { get; set; }
        public int IdEnvio { get; set; }
        public int OpfueraServ { get; set; }
        public int OpCompOrdArranq { get; set; }
        public int OpCompOrdParad { get; set; }
        public int OpSistAislado { get; set; }
        public int OpLimTransm { get; set; }
        public int OpArranqBlackStart { get; set; }
        public int OpEnsayope { get; set; }
        public int OpEnsayopmin { get; set; }

        public EveHoraoperacionDTO HoraOperacion { get; set; }
        public int Hopcodi { get; set; }

        public List<EqEquipoDTO> ListaLineasCongestion { get; set; }

        public string Descripcion { get; set; }
        public bool TieneAlertaDetectada { get; set; }
        public string ValorParamIdEmpresaSeleccione { get { return ConstantesHorasOperacion.ParamEmpresaSeleccione; } }
        public string ValorParamIdCentralSeleccione { get { return ConstantesHorasOperacion.ParamCentralSeleccione; } }
        public string ValorParamIdModoSeleccione { get { return ConstantesHorasOperacion.ParamModoSeleccione; } }

        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int FiltroFamilia { get; set; }

        public string UsuarioModificacion { get; set; }
        public string FechaModificacion { get; set; }

        public string HoBitacoraJson { get; set; }
        public string BitacoraHophoriniFecha { get; set; }
        public string BitacoraHophoriniHora { get; set; }
        public string BitacoraHophorfinFecha { get; set; }
        public string BitacoraHophorfinHora { get; set; }
        public string BitacoraDescripcion { get; set; }
        public string BitacoraComentario { get; set; }
        public string BitacoraDetalle { get; set; }
        public int? BitacoraIdSubCausaEvento { get; set; }
        public int? BitacoraIdEvento { get; set; }
        public int? BitacoraIdTipoEvento { get; set; }
        public int? BitacoraIdEquipo { get; set; }
        public int? BitacoraIdEmpresa { get; set; }
        public int? BitacoraIdTipoOperacion { get; set; }
        public int BitacoraHayCambios { get; set; }

        public List<EveHoEquiporelDTO> ListaDesglose { get; set; }

        public List<EveHoraoperacionDTO> ListaHorasOperacionCostoIncremental { get; set; }
        public List<ValidacionHoraOperacion> ListaValCruce { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionCostoIncremental { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacionIntervencion { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionIntervencion { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
    }

    public class HOReporteModel
    {
        public bool TienePermisoAdministrador { get; set; }

        public int IdEmpresa { get; set; }
        public int IdTipoCentral { get; set; }
        public int IdCentralSelect { get; set; }
        public int IdEquipoOrIdModo { get; set; }
        public int IdEquipo { get; set; }
        public int IdGrupoModo { get; set; }

        public string HoraMinutoConsulta { get; set; }
        public string HoraMinutoActual { get; set; }
        public string HoraFinDefecto { get; set; }
        public string FechaListado { get; set; }
        public string FechaAnterior { get; set; }
        public string Fecha { get; set; }
        public string FechaSiguiente { get; set; }
        public string FechaIni { get; set; }
        public string HoraIni { get; set; }
        public string FechaFin { get; set; }
        public string HoraFin { get; set; }
        public string Fechahorordarranq { get; set; }
        public string Hophorordarranq { get; set; }
        public string FechaHophorparada { get; set; }
        public string Hophorparada { get; set; }
        public List<string> ListaFechaArranque { get; set; }

        public List<EveSubcausaeventoDTO> ListaTipoOperacion { get; set; }
        public List<MotivoOperacionForzada> ListaMotOpForzada { get; set; }
        public List<TipoDesgloseHoraOperacion> ListaTipoDesglose { get; set; }

        public List<EqEquipoDTO> ListaCentrales { get; set; }
        public List<PrGrupoDTO> ListaModosOperacion { get; set; }
        public List<PrGrupoDTO> ListaModosOperacionCI { get; set; }
        public List<PrGrupoDTO> ListaModosOperacionCT { get; set; }
        public List<PrGrupoDTO> ListaModosOperacionProgramada { get; set; }

        public List<HOPAlerta> ListaAlerta { get; set; }

        public List<EqEquipoDTO> ListaUnidades { get; set; }
        public List<PrGrupoDTO> ListaUnidXModoOP { get; set; }
        public List<SiEmpresaDTO> ListaEmpresas { get; set; }
        public List<MeEnvioDTO> ListaEnvios { get; set; }
        public List<MeEnviodetDTO> ListaEnviodet { get; set; }

        public EveHoraoperacionDTO HoraOperacion { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacion { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacionAnt { get; set; }

        public List<HorasProgramadasDTO> ListaHorasProgramadas { get; set; }

        public int TotalValInterFS { get; set; }

        public List<EqEquipoDTO> ListaUnidadesNoRegistradasEMS { get; set; }
        public List<EqEquipoDTO> ListaUnidadesNoRegistradasScada { get; set; }
        public List<ResultadoValidacionAplicativo> ListaHOPUnidadesNoRegistradasEMS { get; set; }
        public List<ResultadoValidacionAplicativo> ListaHOPUnidadesNoRegistradasScada { get; set; }

        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionEms { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionScada { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionIntervencion { get; set; }
        public List<ResultadoValidacionAplicativo> ListaValidacionHorasOperacionCostoIncremental { get; set; }

        public List<EveHoraoperacionDTO> ListaHorasOperacionIntervencion { get; set; }
        public List<EveHoraoperacionDTO> ListaHorasOperacionCostoIncremental { get; set; }

        public decimal ValorUmbral { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string Reporte { get; set; }
        public string NombreArchivo { get; set; }
    }

    public class ColorModel
    {
        public List<EqEquipoDTO> ListadoEquipamiento { get; set; }
    }
}