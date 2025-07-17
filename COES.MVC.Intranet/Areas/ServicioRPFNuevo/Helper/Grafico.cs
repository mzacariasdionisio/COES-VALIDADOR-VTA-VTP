using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace COES.MVC.Intranet.Areas.ServicioRPFNuevo.Helper
{
    public class Grafico
    {
        /// <summary>
        /// Permite generar el gráfico
        /// </summary>
        /// <param name="listSerie"></param>
        /// <param name="listSuperior"></param>
        /// <param name="listInferior"></param>
        /// <param name="listPuntos"></param>
        /// <param name="titulo"></param>
        /// <param name="xTitulo"></param>
        /// <param name="yTitulo"></param>
        public void GenerarGrafico(List<ServicioRpfSerie> listSerie, List<ServicioRpfSerie> listSuperior, List<ServicioRpfSerie> listInferior,
            List<ServicioRpfSerie> listPuntos, string path)
        {
            Chart chart = new Chart();
            chart.Width = 480;
            chart.Height = 370;
            chart.BackColor = ColorTranslator.FromHtml("#F6F6F6");
            chart.BorderlineDashStyle = ChartDashStyle.Solid;           
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = ColorTranslator.FromHtml("#E0E0E0");
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;     
            chart.Series.Add(this.CrearSerie(listSerie, SeriesChartType.Line, "SeriePrincipal", ColorTranslator.FromHtml("#4BB2C5"), ChartDashStyle.Solid));
            chart.Series.Add(this.CrearSerie(listSuperior, SeriesChartType.Line, "SerieSuperior", ColorTranslator.FromHtml("#EAA228"), ChartDashStyle.Dash));
            chart.Series.Add(this.CrearSerie(listInferior, SeriesChartType.Line, "SerieInferior", ColorTranslator.FromHtml("#C5B47F"), ChartDashStyle.Dash));
            chart.Series.Add(this.CrearSerie(listPuntos, SeriesChartType.Point, "SeriePuntos", ColorTranslator.FromHtml("#679D81"), ChartDashStyle.NotSet));

            //chart.Legends.Add(this.CrearLeyenda("Análisis Normal"));

            double maximoSerie = (double)listSerie.Max(x => x.Frecuencia);
            double minimoSerie = (double)listSerie.Min(x => x.Frecuencia);
            double maximoSuperior = (double)listSuperior.Max(x => x.Frecuencia);
            double minimoSuperior = (double)listSuperior.Min(x => x.Frecuencia);
            double maximoInferior = (double)listInferior.Max(x => x.Frecuencia);
            double minimoInferior = (double)listInferior.Min(x => x.Frecuencia);
            double maximoPuntos = (double)listPuntos.Max(x => x.Frecuencia);
            double minimoPuntos = (double)listPuntos.Min(x => x.Frecuencia);

            double[] maximos = { maximoSerie, maximoSuperior, maximoInferior, maximoPuntos };
            double[] minimos = { minimoSerie, minimoSuperior, minimoInferior, minimoPuntos };
            
            chart.ChartAreas.Add(this.CrearArea(maximos.Max(), minimos.Min()));
            chart.ImageType = ChartImageType.Jpeg;
            
            try
            {
                if (File.Exists(path + Constantes.NombreChartRPF))
                {
                    File.Delete(path + Constantes.NombreChartRPF);
                }

                chart.SaveImage(path + Constantes.NombreChartRPF, ChartImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
              

        /// <summary>
        /// Permite generar el gráfico
        /// </summary>
        /// <param name="listSerie"></param>
        /// <param name="listSuperior"></param>
        /// <param name="listInferior"></param>
        /// <param name="listPuntos"></param>
        /// <param name="titulo"></param>
        /// <param name="xTitulo"></param>
        /// <param name="yTitulo"></param>
        /// 
        public void GenerarGraficoFalla(List<ServicioRpfSerie> listSerie, List<ServicioRpfSerie> listPotencia, List<ServicioRpfSerie> listFrecuencia ,
            string path, int indice, DateTime fechaProceso, decimal valorRA, List<ServicioRpfSerie> frecSanJuan)
        {
            Chart chart = new Chart();
            chart.Width = 560;
            chart.Height = 370;
            chart.BackColor = ColorTranslator.FromHtml("#F6F6F6");
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = ColorTranslator.FromHtml("#E0E0E0");
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.None;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Series.Add(this.CrearSerieFalla(listSerie, SeriesChartType.Area, "RA = " + valorRA.ToString("0.0000"), 
                ColorTranslator.FromHtml("#C6D9F1"),
                ChartDashStyle.Solid, AxisType.Primary, fechaProceso));
            chart.Series.Add(this.CrearSerieFalla(listPotencia, SeriesChartType.Line, "Potencia", ColorTranslator.FromHtml("#FF4040"), 
                ChartDashStyle.Solid, AxisType.Primary, fechaProceso));
            chart.Series.Add(this.CrearSerieFalla(listFrecuencia, SeriesChartType.Line, "Frecuencia", ColorTranslator.FromHtml("#905EB5"), 
                ChartDashStyle.Solid, AxisType.Secondary, fechaProceso));
            chart.Series.Add(this.CrearSerieFalla(frecSanJuan, SeriesChartType.Line, "Frecuencia San Juan", ColorTranslator.FromHtml("#33CC00"),
                ChartDashStyle.Solid, AxisType.Secondary, fechaProceso));

            chart.Legends.Add(this.CrearLeyenda("Análisis de Fallas"));

            double maximoSerie = (double)listSerie.Max(x => x.Valor);
            double minimoSerie = (double)listSerie.Min(x => x.Valor);

            double maximoPuntos = (double)listPotencia.Max(x => x.Valor);
            double minimoPuntos = (double)listPotencia.Min(x => x.Valor);

            double maximoFrecNormal = (double)listFrecuencia.Max(x => x.Valor);
            double minimoFrecNormal = (double)listFrecuencia.Min(x => x.Valor);
            double maximoFrecSanJuan = (double)frecSanJuan.Max(x => x.Valor);
            double minimoFrecSanJuan = (double)frecSanJuan.Min(x => x.Valor);


            double[] maximos = { maximoSerie, maximoPuntos };
            double[] minimos = { minimoSerie, minimoPuntos };
            double[] maximosFrec = {maximoFrecNormal, maximoFrecSanJuan};
            double[] minimosFrec = { minimoFrecNormal, minimoFrecSanJuan };

            chart.ChartAreas.Add(this.CrearAreaFalla(maximos.Max(), minimos.Min(), maximosFrec.Max(), minimosFrec.Min()));
            chart.ImageType = ChartImageType.Jpeg;

            try
            {
                if (File.Exists(path + indice + Constantes.NombreChartFallaRPF))
                {
                    File.Delete(path + indice + Constantes.NombreChartFallaRPF);
                }

                chart.SaveImage(path + indice + Constantes.NombreChartFallaRPF, ChartImageFormat.Jpeg);   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Permite agregar un titulo al gráfico
        /// </summary>
        /// <param name="titulo"></param>
        /// <returns></returns>
        private Title CrearTitulo(string titulo)
        {
            Title title = new Title();
            title.Text = titulo;
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);

            return title;
        }

        /// <summary>
        /// Permite agregar una leyenda al gráfico
        /// </summary>
        /// <returns></returns>
        private Legend CrearLeyenda(string titulo)
        {
            Legend legend = new Legend();
            legend.Name = titulo;            
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.ForeColor = Color.Black;
            legend.BackColor = Color.Transparent;
            legend.Font = new Font(new FontFamily("Trebuchet MS"), 8);
            legend.LegendStyle = LegendStyle.Row;

            return legend;
        }

        /// <summary>
        /// Permite crear las series de un gráfico
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="chartType"></param>
        /// <param name="serieName"></param>
        /// <param name="color"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private Series CrearSerie(List<ServicioRpfSerie> lista, SeriesChartType chartType, string serieName, System.Drawing.Color color, ChartDashStyle tipo)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = serieName;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            seriesDetail.BorderDashStyle = tipo;
            seriesDetail.YValueType = ChartValueType.Double;
            seriesDetail.XValueType = ChartValueType.Double;      
            
                                 
            foreach (ServicioRpfSerie result in lista)
            {
                seriesDetail.Points.AddXY((double)result.Potencia, (double)result.Frecuencia);               
            }

            seriesDetail.ChartArea = "Cumplimiento RPF";

            return seriesDetail;
        }


        /// <summary>
        /// Permite crear las series de un gráfico
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="chartType"></param>
        /// <param name="serieName"></param>
        /// <param name="color"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        private Series CrearSerieFalla(List<ServicioRpfSerie> lista, SeriesChartType chartType, string serieName, System.Drawing.Color color, ChartDashStyle tipo, AxisType yaxis, DateTime fecha)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = serieName;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 1;
            
            seriesDetail.BorderDashStyle = tipo;
            seriesDetail.YValueType = ChartValueType.Double;
            seriesDetail.XValueType = ChartValueType.DateTime;      
            seriesDetail.YAxisType = yaxis;
                      
            foreach (ServicioRpfSerie result in lista)
            {               
                seriesDetail.Points.AddXY(fecha.AddSeconds((int)result.Segundo), (double)result.Valor);   
            }

            seriesDetail.ChartArea = "Cumplimiento RPF";

            return seriesDetail;
        }

        /// <summary>
        /// Permite crear el layout del gráfico
        /// </summary>
        /// <returns></returns>
        private ChartArea CrearArea(double maximo, double minimo)
        {
            ChartArea chartArea = new ChartArea();

            chartArea.Name = "Cumplimiento RPF";
            chartArea.BackColor = Color.White;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = ColorTranslator.FromHtml("#DFDFDF");
            chartArea.AxisX.LineColor = ColorTranslator.FromHtml("#DFDFDF");
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);           
            chartArea.AxisY.Maximum = maximo;
            chartArea.AxisY.Minimum = minimo;                        
            chartArea.AxisX.LabelStyle.Format = "#,###.00";
            chartArea.AxisY.LabelStyle.Format = "#,###.00";                        
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;       

            return chartArea;
        }

        /// <summary>
        /// Permite crear el layout del gráfico
        /// </summary>
        /// <returns></returns>
        private ChartArea CrearAreaFalla(double maximo, double minimo, double maximoy2, double minimoy2)
        {
            ChartArea chartArea = new ChartArea();

            chartArea.Name = "Cumplimiento RPF";
            chartArea.BackColor = Color.White;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisY2.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY2.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = ColorTranslator.FromHtml("#DFDFDF");
            chartArea.AxisX.LineColor = ColorTranslator.FromHtml("#DFDFDF");
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Title = "Segundos";
            chartArea.AxisY.Title = "Potencia (MW)";
            chartArea.AxisY2.Title = "Frecuencia (HZ)";

            chartArea.AxisY2.Enabled = AxisEnabled.True;
            chartArea.AxisY2.IsStartedFromZero = chartArea.AxisY.IsStartedFromZero;
            chartArea.AxisY2.MajorGrid.Enabled = false;            
            chartArea.AxisY2.LineColor = Color.Transparent;

            if (minimo == maximo && maximo == 0) maximo = 20;
            if (minimo == maximo) maximo = maximo + 20;

            chartArea.AxisY.Maximum = maximo;
            chartArea.AxisY.Minimum = minimo;

            if (maximoy2 == minimoy2) maximoy2 = minimoy2 + 10;

            chartArea.AxisY2.Maximum = maximoy2;
            chartArea.AxisY2.Minimum = minimoy2;
            
            chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
            chartArea.AxisX.LabelStyle.Angle = -90;
            chartArea.AxisY.LabelStyle.Format = "#,###.00";
            chartArea.AxisY2.LabelStyle.Format = "#,###.00";
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartArea.AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            
            return chartArea;
        }        
    }
}