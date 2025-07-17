using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IND.Models
{
    public class IndisponibilidadesModel
    {
        public string Mensaje { get; set; }
        public string Mensaje2 { get; set; }
        public string Mensaje3 { get; set; }
        public string Detalle { get; set; }
        public string Resultado { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public List<string> ListaNota { get; set; }
        public bool FlagContinuar { get; set; }

        //Interfaz Mantenimientos
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int FiltroFamilia { get; set; }

        public bool UsarLayoutModulo { get; set; } = true;

        public bool TienePermiso { get; set; }
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool TienePermisoGuardar { get; set; }
        public bool AccesoEditar { get; internal set; }

        public string TipoMenu { get; set; }
        public string Fecha { get; internal set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public List<GenericoDTO> ListaAnio { get; set; }
        public List<GenericoDTO> ListaMes { get; set; }
        public List<GenericoDTO> ListaHoraIni { get; set; }
        public List<GenericoDTO> ListaHoraFin { get; set; }
        public int AnioActual { get; set; }
        public int MesActual { get; set; }
        public string AnioIni { get; set; }
        public string AnioFin { get; set; }
        public string MesIni { get; set; }
        public string MesFin { get; set; }
        public string Horizonte { get; set; }

        public List<IndCuadroDTO> ListaCuadro { get; set; }
        public IndPeriodoDTO IndPeriodo { get; set; }
        public IndRecalculoDTO IndRecalculo { get; set; }
        public List<IndPeriodoDTO> ListaPeriodo { get; set; }
        public IndReporteDTO IndReporte { get; set; }
        public List<IndReporteDTO> ListaReporte { get; set; }
        public List<IndRecalculoDTO> ListaRecalculo { get; set; }
        public string TipoRecalculo { get; set; }

        public int IdCuadro { get; set; }
        public int IdPeriodo { get; set; }
        public int IdRecalculo { get; set; }
        public int IdReporte { get; set; }
        public int IdReporte2 { get; set; }
        public int IdReporte3 { get; set; }
        public int Famcodi { get; set; }
        public IndCuadroDTO Cuadro { get; set; }
        public int UsarAplicativo { get; set; }

        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<EqEquipoDTO> ListaCentral { get; set; }
        public List<EqEquipoDTO> ListaModo { get; set; }
        public List<EqEquipoDTO> ListaGasoducto { get; set; }
        public List<EqFamiliaDTO> ListaFamilia { get; set; }
        public List<AreaDTO> ListaArea { get; set; }
        public List<EqEquipoDTO> ListaEquipoFiltro { get; set; }
        public List<SiTipoempresaDTO> ListaTipoEmpresa { get; set; }
        public List<ListaSelect> ListaModoOperacion { get; set; }

        public HandsonModel HandsonFF { get; set; }
        public HandsonModel HandsonFP { get; set; }
        public HandsonModel HandsonDisp { get; set; }
        public HandsonModel HandsonK { get; set; }
        public HandsonModel HandsonPa { get; set; }
        public HandsonModel HandsonTotal { get; set; }
        public HandsonModel HandsonFort { get; set; }
        public HandsonModel HandsonProg { get; set; }
        public HandsonModel HandsonCargaHistorica { get; set; }

        public string RptHtmlCambios { get; set; }
        public string RptHtml1 { get; set; }
        public string RptHtml2 { get; set; }
        public string RptHtml3 { get; set; }
        public string NombreArchivoUpload { get; set; }

        public List<IndReporteTotalDTO> ListaTotTermo { get; set; }
        public List<IndReporteTotalDTO> ListaTotHidro { get; set; }

        public List<IndManttoDTO> ListaIndMantto { get; set; }
        public List<IndEventoDTO> ListaIndEvento { get; set; }
        public List<IndIeodcuadroDTO> ListaIndIeodcuadro { get; set; }
        public List<ResultadoValidacionAplicativo> ListaVerificacion { get; set; }

        public List<EveEvenclaseDTO> ListaTipoMantenimiento { get; set; }
        public List<EveManttoDTO> ListaManttos { get; set; }
        public List<ReporteManttoDTO> ListarReporteManttos { get; set; }
        public List<TipoEventoDTO> ListaTipoEvento { get; set; }
        public List<SubCausaEventoDTO> ListaCausaEvento { get; set; }
        public EventoDTO Evento { get; set; }
        public EveIeodcuadroDTO Ieodcuadro { get; set; }
        public int IdMantto { get; set; }

        public int Estado { get; set; }

        public List<IndGaseoductoxcentralDTO> ListadoRelacionGaseoducto { get; set; }
        public List<PrGrupodatDTO> ListaParametros { get; internal set; }
        public string Anexo { get; internal set; }
        public string Concepto { get; internal set; }

        //IND.PR25.2022
        public List<EqEquipoDTO> ListaUnidad { get; set; }
        public List<PrGrupoDTO> ListaGrupo { get; set; }
        public List<IndRelacionEmpresaDTO> ListaUnidadNombre { get; set; }
        public HandsonModel HandsonStkCmtDet { get; set; }
        public HandsonModel HandsonInfkdt { get; set; }
        public int NumeroDias { get; set; }
        public IndRelacionEmpresaDTO RelacionEmpresa { get; set; }
        public IndInsumosFactorKDTO InsumosFactorK { get; set; }
        //


        //Assetec(RAC) Indisponibilidad de Empresa
        public bool bEditar { get; set; }
        public bool bNuevo { get; set; }
        public bool bEliminar { get; set; }
        public bool bGrabar { get; set; }
        public IndRelacionEmpresaDTO Entidad { get; set; }
        public int Relempcodi { get; set; }
        public List<IndRelacionEmpresaDTO> ListadoIdnRelacionEmpresa { get; set; }
        public List<IndRelacionEmpresaDTO> ListaCentral2 { get; set; }
        public List<IndRelacionEmpresaDTO> ListaGaseoducto { get; set; }
        public List<TipoTecnologia> ListaTipoTecnologia { get; set;  }
    }

    public class BusquedaMantenimientoModel
    {
        public List<EveEvenclaseDTO> ListaTipoMantenimiento { get; set; }
        public int IdFamilia { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoMantenimiento { get; set; }
        public int IdTipoEmpresa { get; set; }
        public int Equicodi { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string Empresa { get; set; }
        public string Familia { get; set; }
        public string Ubicacion { get; set; }
        public string Equinomb { get; set; }
        public string FechaActual { get; set; }
        public string FechaSiguiente { get; set; }
        public string Descripcion { get; set; }

        public string Tipoindisp { get; set; }
        public decimal? Pr { get; set; }
        public string Asocproc { get; set; }
        public string Comentario { get; set; }

        public int TipoAccionFormulario { get; set; }
        public int FuenteDatos { get; set; }
        public int ParametroDefecto { get; set; }

        public int Indmancodi { get; set; }
        public int? Manttocodi { get; set; }
        public int NroDiaReplicar { get; set; }

        public string GrupoCogeneracion { get; set; }
        public List<GenericoDTO> ListaTipoindispPr25 { get; set; }
    }

    public class TipoTecnologia
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
    }
}