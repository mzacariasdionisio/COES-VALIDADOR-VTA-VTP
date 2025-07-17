using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace COES.Framework.Base.Core
{
    /// <summary>
    /// Clase base model
    /// </summary>
    public class Paginacion
    {
        public bool IndicadorPagina { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public int NroPagina { get; set; }
        public string[] CantidadRegistros { get; set; }
    }

    /// <summary>
    /// Model para el manejo de grillas tipo excel
    /// </summary>
    public class GridExcel
    {
        public string[] Headers { get; set; }
        public int[] Widths { get; set; }
        public object[] Columnas { get; set; }
        public string[][] Data { get; set; }

        public const string TipoTexto = "text";
        public const string TipoNumerico = "numeric";
        public const string TipoFecha = "date";
        public const string TipoLista = "dropdown";
        public const string TipoCheck = "checkbox";
    }

    /// <summary>
    /// Clase utilizada para el graficado de datos
    /// </summary>
    public class Graficador
    {
        public string Titulo { get; set; }
        public List<string> SerieX { get; set; }
        public object[] Series { get; set; }
    }

    /// <summary>
    /// Clase para manejo de items de serie
    /// </summary>
    public class GraficadorItem
    {
        public string Texto { get; set; }
        public decimal Valor { get; set; }
    }

    public class GraficoWeb
    {
        /// <summary>
        /// Titulo del grafico
        /// </summary>
        //public string titleText { get; set; }
        public string TitleText { get; set; }
        /// <summary>
        /// Lista de categorias eje x.
        /// </summary>
        public List<string> XAxisCategories { get; set; }
        /// <summary>
        /// Titulo eje x
        /// </summary>
        public string XAxisTitle { get; set; }
        public decimal? YaxixMax { get; set; }
        public decimal? YaxixMin { get; set; }
        /// <summary>
        /// titulo eje y
        /// </summary>
        public string YaxixTitle { get; set; }
        public List<string> YAxixTitle { get; set; }
        public List<int> SeriesYAxis { get; set; }
        /// <summary>
        /// nombre de las series del gráfico
        /// </summary>
        //public List<string> seriesName { get; set; }
        public List<string> SeriesName { get; set; }
        //public List<string> seriesType { get; set; }
        public List<string> SeriesType { get; set; }
        /// <summary>
        /// valores de datos de la serie
        /// </summary>
        public decimal?[][] SeriesData { get; set; }

        public bool[][] SeriesDataVisible { get; set; }

        public DatosSerie[][] SerieDataS { get; set; }

        public List<RegistroSerie> Series { get; set; }

        public string YaxixTitle2 { get; set; }

        public string Subtitle { get; set; }

        public List<string> ListaNota { get; set; }

        public int IdGrafico { get; set; }

        public string NameGrafico { get; set; }

        #region siosein2
        public decimal?[] YaxixTickPositions { get; set; }

        public List<PlotBands> PlotBands { get; set; } = new List<PlotBands>();

        public DatosSerie[] SerieData { get; set; }

        public Categorias[] Categorias { get; set; }

        public string YaxixLabelsFormat { get; set; }

        public bool YAxisStackLabels { get; set; } = true;

        public string Type { get; set; }

        public bool Shadow { get; set; } = true;

        public string TooltipValueSuffix { get; set; } = string.Empty;

        public string TooltipValuePrefix { get; set; } = string.Empty;

        public int TooltipValueDecimals { get; set; } = 2;

        public string SeriesInnerSize { get; set; }

        public int? XAxisLabelsRotation { get; set; }

        public bool PlotOptionsDataLabels { get; set; } = true;

        public int PlotOptionsDataLabelsDigit { get; set; } = 2;

        public string TooltipPointFormat { get; set; }

        public string PlotOptionsFormat { get; set; }

        public string LegendLayout { get; set; } = "vertical";

        public string LegendAlign { get; set; } = "right";

        public string LegendVerticalAlign { get; set; } = "middle";

        public List<string> YAxisLabelsFormat { get; set; }

        #endregion

        #region SIOSEIN
        public List<RegistroSerie> Drilldown { get; set; }
        public decimal? TickInterval { get; set; }
        #endregion
    }

    public class RegistroSerie
    {
        public string Name;
        public string Type;
        public string Color;
        public int YAxis;
        public int To;
        public int From;
        public string YAxisTitle;
        public List<DatosSerie> Data;
        public int TipoPto;
        public decimal? Acumulado { get; set; }
        public decimal? Porcentaje { get; set; }
        public int Codigo { get; set; }
        public string Center { get; set; }
        public bool NotShowInLegend { get; set; }
        public int ZIndex { get; set; }
        public string DashStyle { get; set; }
        public string Id { get; set; }
    }

    public class DatosSerie
    {
        public DateTime X { get; set; }
        public decimal? Y { get; set; }
        public decimal? Z { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }

        #region siosein2
        public string Drilldown { get; set; }
        public decimal?[] Data { get; set; }
        public int YAxis { get; set; }
        public string TooltipValueSuffix { get; set; } = string.Empty;
        public string TooltipValuePrefix { get; set; } = string.Empty;
        public string Type { get; set; }
        public int PointWidth { get; set; }
        public int BorderWidth { get; set; }
        public bool MarkerEnabled { get; set; }
        #endregion
    }

    public class PlotBands
    {
        public double From { get; set; }
        public double To { get; set; }
        public string Color { get; set; }
        public string Thickness { get; set; }
    }

    public class Categorias
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string[] Categories { get; set; }
    }

    public class TablaReporte : ICloneable
    {
        public int ReptiCodiTabla { get; set; }
        public string Titulo { get; set; }
        public CabeceraReporte Cabecera { get; set; }
        public List<RegistroReporte> ListaRegistros { get; set; }
        public PieReporte Leyenda { get; set; }

        public List<CabeceraReporteColumna> CabeceraColumnas { get; set; }
        public string TipoFuente { get; set; }
        public int TamLetra { get; set; }
        public string Color { get; set; }
        public decimal AltoFilaCab { get; set; }
        public bool EsMayuscula { get; set; }
        public string ColorERACMF { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public TablaReporte()
        {
            Cabecera = new CabeceraReporte();
            ListaRegistros = new List<RegistroReporte>();
            CabeceraColumnas = new List<CabeceraReporteColumna>();
            Leyenda = new PieReporte();
        }

        //PR05
        public List<ItemMenuNumeral> ListaItem { get; set; }
    }

    public class ItemMenuNumeral
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string NombreExcel { get; set; }
    }

    public class CabeceraReporte
    {
        public string[,] CabeceraData { get; set; }
    }

    public class CabeceraReporteColumna
    {
        public string NombreColumna { get; set; }
        public decimal AnchoColumna { get; set; }
    }

    public class RegistroReporte
    {
        public string Nombre { get; set; }
        public string Nombre2 { get; set; }
        public List<decimal?> ListaData { get; set; }
        public List<string> ListaFiltroData { get; set; }
        public List<string> ListaPropiedades { get; set; }
        public List<CeldaReporte> ListaCelda { get; set; }
        public bool EsFilaResumen { get; set; }
        public decimal AltoFila { get; set; }
        public bool EsAgrupado { get; set; }
        public string codigo { get; set; }
        public int Emprcodi { get; set; }
        public int CodigoFila { get; set; }
        public string ColorFila { get; set; }
        public bool EsFilaVisible { get; set; }

        public decimal? ValorAnioActual { get; set; }
    }

    public class CeldaReporte
    {
        public decimal? Valor { get; set; }
        public int Fila { get; set; }
        public string Texto { get; set; }
        public bool EsTexto { get; set; }
        public bool EsTextoFecha { get; set; }
        public bool TieneFormatoFechaExcel { get; set; }
        public bool EsNumero { get; set; }
        public bool TieneTextoNegrita { get; set; }
        public bool TieneTextoPorcentaje { get; set; }
        public bool TieneTextoCentrado { get; set; }
        public bool TieneTextoIzquierdo { get; set; }
        public bool TieneTextoDerecho { get; set; }
        public bool TieneBorder { get; set; }
        public bool TieneColor { get; set; }
        public bool TieneTextoColor { get; set; }
        public bool TieneAgrupacion { get; set; }

        //Aplicativo Extranet CTAF
        public bool TieneFormatoNumeroEspecial { get; set; }
        public bool EsNumeroTruncado { get; set; }
        public bool EsNumeroRedondeado { get; set; }
        public int DigitosParteDecimal { get; set; }

        public string TituloReporte { get; set; }
        #region Cambios Subastas 09/2020
        public string ColorTexto { get; set; }
        public string ColorFondo { get; set; }
        #endregion

        public bool TieneColorERACMF { get; set; }

        public CeldaReporte()
        {
        }

        #region Cambios Subastas 09/2020
        
        public CeldaReporte(string texto, string colorTexto, string colorFondo)
        {
            this.Texto = texto;
            this.ColorTexto = colorTexto;
            this.ColorFondo = colorFondo;
        }
        #endregion

        public CeldaReporte(decimal? valor, bool tieneTextoPorcentaje, bool tieneTextoNegrita)
        {
            this.Valor = valor;
            this.TieneTextoPorcentaje = tieneTextoPorcentaje;
            this.TieneTextoNegrita = tieneTextoNegrita;
            this.EsNumero = true;
        }
        public CeldaReporte(string texto, bool tieneTextoNegrita)
        {
            this.Texto = texto;
            this.TieneTextoNegrita = tieneTextoNegrita;
            this.EsTexto = true;
        }
        public CeldaReporte(string texto, bool tieneTextoNegrita, bool tieneTextoCentrado, bool tieneTextoIzquierdo)
        {
            this.Texto = texto;
            this.TieneTextoNegrita = tieneTextoNegrita;
            this.TieneTextoCentrado = tieneTextoCentrado;
            this.TieneTextoIzquierdo = tieneTextoIzquierdo;
            this.EsTexto = true;
        }

        public CeldaReporte(int valor, bool tieneTextoNegrita, bool tieneTextoCentrado, bool tieneTextoIzquierdo)
        {
            this.Fila = valor;
            this.TieneTextoNegrita = tieneTextoNegrita;
            this.TieneTextoCentrado = tieneTextoCentrado;
            this.TieneTextoIzquierdo = tieneTextoIzquierdo;
            this.EsTexto = true;
        }
        public CeldaReporte(string texto, bool tieneTextoNegrita, bool tieneTextoCentrado, bool tieneTextoIzquierdo, bool tieneTextoDerecho)
        {
            this.Texto = texto;
            this.TieneTextoNegrita = tieneTextoNegrita;
            this.TieneTextoCentrado = tieneTextoCentrado;
            this.TieneTextoIzquierdo = tieneTextoIzquierdo;
            this.TieneTextoDerecho = tieneTextoDerecho;
            this.EsTexto = true;
        }
    }

    public class PieReporte
    {
        public List<string> ListaDescripcion { get; set; }
    }

    #region Highchart

    public class GHGraficoHighchart
    {
        public GHChart chart { get; set; }
        public GHTitle title { get; set; }
        public GHSubtitle subtitle { get; set; }
        public GHXAxis[] xAxis { get; set; }
        public GHYAxis yAxis { get; set; }
        public GHTooltip tooltip { get; set; }
        public GHLegend legend { get; set; }
        public GHPlotOptions plotOptions { get; set; }
        public GHSerie[] series { get; set; }
    }

    public class GHSerie
    {
        public string name { get; set; }
        public string type { get; set; }
        public int yAxis { get; set; }
        public decimal?[] data { get; set; }
        public string color { get; set; }
    }

    public class GHChart
    {
        public string zoomType { get; set; }
        public string type { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public bool shadow { get; set; }
    }

    public class GHTitle
    {
        public string text { get; set; }
        public bool enabled { get; set; }
    }

    public class GHSubtitle
    {
        public GHTitle title { get; set; }
        public bool floating { get; set; }
        public string align { get; set; }
        public string text { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }

    public class GHXAxis
    {
        public string[] categories { get; set; }
        public bool crosshair { get; set; }
        public int tickInterval { get; set; }
        public int lineWidth { get; set; }
        public GHLabels labels { get; set; }
        public GHTitle title { get; set; }
    }

    public class GHYAxis
    {
        public GHTitle title { get; set; }
        public GHLabels labels { get; set; }
    }

    public class GHLabels
    {
        public string format { get; set; }
        public int rotation { get; set; }
    }

    public class GHTooltip
    {
        public bool shared { get; set; }
    }

    public class GHLegend
    {
        public bool enabled { get; set; }
        public string align { get; set; }
        public string verticalAlign { get; set; }
        public string layout { get; set; }
    }

    public class GHPlotOptions
    {
        public GHSpline spline { get; set; }
        public GHArea area { get; set; }

    }
    public class GHArea
    {
        public string stacking { get; set; }
        public string lineColor { get; set; }
        public int lineWidth { get; set; }
        public GHMarker marker { get; set; }
    }
    public class GHSpline
    {
        public int lineWidth { get; set; }
        public GHStates states { get; set; }
        public GHMarker marker { get; set; }
    }
    public class GHStates
    {
        public GHHover hover { get; set; }
    }
    public class GHHover
    {
        public int lineWidth { get; set; }
    }
    public class GHMarker
    {
        public bool enabled { get; set; }
        public int lineWidth { get; set; }
        public string lineColor { get; set; }
    }

    #endregion
}
