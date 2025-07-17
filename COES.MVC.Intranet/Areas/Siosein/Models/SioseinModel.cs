using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COES.MVC.Intranet.Areas.Siosein.Models
{
    public class SioseinModel
    {
        public SioCabeceradetDTO EntCabeceradet { get; set; }
        public string PeriodoSelect { get; set; }
        public List<SioTablaprieDTO> ListaTablaPrie { get; set; }
        public string Menu { get; set; }
        public string NroReporte { get; set; }
        public string MesActual { get; set; }
        public string TituloWeb { get; set; }
        public int Repcodi { get; set; }
        public int Total { get; set; }
        public List<string> Resultados { get; set; }
        public string Url { get; set; }

        public SioseinModel()
        {
            EntCabeceradet = new SioCabeceradetDTO();
        }

        //Costo total operacion
        public decimal CostoTotalOperacion;

        public List<SioTablaprieDTO> ListaTablasPrie { get; set; }
        public SioTablaprieDTO TablaPrie { get; set; }
        public List<FwAreaDTO> ListaAreas { get; set; }
        public List<SioColumnaprieDTO> ListaCampos { get; set; }
        public List<ListaSelect> ListaEmpresas { get; set; }
        public MeFormatoDTO Formato { get; set; }
        public string FechaEnvio { get; set; }
        public string Resultado { get; set; }
        public List<ListaSelect> Lista1 { get; set; }

        //Difusion Tabla Prie03 
        public List<ListaSelect> Tensiones { get; set; }
        public List<ListaSelect> AreasOperativas { get; set; }
        public List<ListaSelect> Barras { get; set; }
        public List<ListaSelect> Rangos { get; set; }

        //Difusion Tabla Prie04
        public List<PrGrupoDTO> ModoOpe { get; set; }
        public List<SiFuenteenergiaDTO> TipoCombustible { get; set; }

        //Index Tabla Prie 17
        public List<ListaSelect> ListaComponentes { get; set; }
        public List<InInterconexionDTO> ListaInterconexion { get; set; }
        public int IdPtomedicion { get; set; }
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public GraficoWeb Grafico { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string PeriodoDet { get; set; }
        public List<MeMedicion96DTO> ListaMemedicion96 { get; set; }
        public List<string> FechasCategoria { get; set; }

        //Index Tabla Prie 26
        public List<SiFuenteenergiaDTO> ListaRecursoEnergetico { get; set; }
        public List<SiTipogeneracionDTO> ListaTipoGeneracion { get; set; }
        public List<ListaSelect> CentralesLista { get; set; }

        public List<string> Categoria { get; set; }
        public List<decimal> Serie1 { get; set; }
        public List<decimal> Serie2 { get; set; }
        public List<decimal> Serie3 { get; set; }
        public List<decimal> Serie4 { get; set; }
        public List<decimal> Serie5 { get; set; }
        public List<decimal> Serie6 { get; set; }
        public List<decimal> Serie7 { get; set; }
        public List<decimal> Serie8 { get; set; }
        public List<decimal> Serie9 { get; set; }
        public string NomGrafico { get; set; }
        public List<ListaGenerica> SeriesPie { get; set; }
        public List<ListaGenerica> SeriesBarr { get; set; }



        public class ListaGenerica
        {
            public decimal Decimal1 { get; set; }
            public string String1 { get; set; }
            public string String2 { get; set; }
            public List<decimal> Series { get; set; }
            public List<SubSeries> SubSerie { get; set; }
        }
        public class SubSeries
        {
            public decimal Decimal1 { get; set; }
            public string String1 { get; set; }
            public string String2 { get; set; }
        }

        public string Mes { get; set; }
        public int IdEnvio { get; set; }
        public string Fecha { get; set; }
        public int IdParametro { get; set; }
        public string SheetName { get; set; }

        //Reporte Tab
        public List<string> TipoEmpresas { get; set; }
        public List<EveEventoDTO> ListaEveEvento { get; set; }

        //Reporte30
        public List<EqEquipoDTO> ListaEquipos { get; set; }

        public int NRegistros { get; set; }
        public string Titulo { get; set; }
        public string Anho { get; set; }

        #region remisiones
        [Display(Name = "Entidad")]
        public string Nombre { get; set; }
        [Display(Name = "No Registros COES")]
        public int NroRegistrosCoes { get; set; }
        [Display(Name = "No Registros Osinergmin")]
        public int NroRegistrosOsinergmin { get; set; }
        [Display(Name = "Selec.")]
        public bool EstaSeleccionado { get; set; }
        [Display(Name = "Cod. Ent")]
        public List<IioTablaSyncDTO> ListarEntidades { get; set; }
        public int CantidadErrores { get; set; }
        public string Mensaje2 { get; set; }

        public List<DataFileCargados> ListaDocumentos { get; set; }
        #endregion

        public int CountCarga09 { get; set; }

        #region SIOSEIN2 - NUMERALES
        public List<Dominio.DTO.Transferencias.BarraDTO> CabeceraCm { get; set; }
        public List<SiCostomarginalDTO> ListaCM { get; set; }
        public DateTime Fecha_ { get; set; }

        public int ResultadoInt { get; set; }
        public string Resultado2 { get; set; }
        public string Resultado3 { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public string DiasMes { get; set; }
        public bool MostrarCarga { get; set; }
        public string DiasNoCargados { get; set; }
        #endregion

        public List<GraficoWeb> Graficos { get; set; }

        public int Tpriecodi { get; set; }
        public string Tprieabrev { get; set; }
        public string TituloTabla { get; set; }
        public bool TienePermisoAdmin { get; set; }
        public List<SioCabeceradetDTO> ListaHistorialVerificacion { get; set; }
        public List<IioControlCargaDTO> ListaHistorialRemision { get; set; }

        ///Fuente de Datos
        public int Idnumeral { get; set; }
        public int Tiporeporte { get; set; }

        ///Filtros
        public List<SiVersionDTO> ListaVersion { get; set; }
        public int Verscodi { get; set; }
        public FechasPR5 ObjFecha { get; set; }

        public List<AbiProdgeneracionDTO> ListaDetalleProduccion { get; set; }
        public List<InfSGIFilaResumenInterc> ListaDetalleInterconexion { get; set; }

        //Graficos en Reporte
        public List<MeReporteDTO> ListaGraficosReporte { get; set; }
    }

    /// <summary>
    /// MODEL para excel web COMP
    /// </summary>
    public class SioseinTblCOMPModel
    {
        public string Mes { get; set; }

        public List<SioPrieCompDTO> ListaData { get; set; }

        public string Resultado { get; set; }
        public string StrMensaje { get; set; }
        public string Detalle { get; set; }
        public int NRegistros { get; set; }

        public string UsuarioEnvio { get; set; }
        public string FechaEnvio { get; set; }
    }

    /// <summary>
    /// Objeto para almacenar las series de duración de carga
    /// </summary>
    public class SerieDuracionCarga
    {
        public string SerieName { get; set; }
        public List<decimal> ListaValores { get; set; }
        public List<DateTime> ListaValores2 { get; set; }
        public string SerieColor { get; set; }

        public string SerieType { get; set; }

        public int SerieYaxis { get; set; }
        public decimal Valor { get; set; }
    }

    public class DataFileCargados
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string Icono { get; set; }
        public string Extension { get; set; }
        public string LastWriteTime { get; set; }
        public int IdEnvio { get; set; }
        public string Tabla { get; set; }
    }

}