using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Servicios.Aplicacion.ReportesFrecuencia;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Models;
using COES.MVC.Intranet.Areas.ReportesFrecuencia.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Net;
using OfficeOpenXml.Drawing;
using System.Configuration;
using COES.MVC.Intranet.SeguridadServicio;
using COES.MVC.Intranet.Areas.PMPO.Controllers;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Web.UI.DataVisualization.Charting;

namespace COES.MVC.Intranet.Areas.ReportesFrecuencia.Controllers
{
    public class ImagenFrecuenciaController : BaseController
    {
        /// <summary>
        /// Instancia del Web Service de seguridad
        /// </summary>
        SeguridadServicio.SeguridadServicioClient seguridad = new SeguridadServicio.SeguridadServicioClient();
        double frecMinima = 0;
        double frecMaxima = 0;

        public ActionResult Index()
        {
            //base.ValidarSesionUsuario();
            //if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteFrecuenciaModel model = new ReporteFrecuenciaModel();
            //model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            model.IdGPS = Convert.ToInt32(Request.Params["IdGPS"]);
            model.FechaInicial = Convert.ToDateTime(Request.Params["FechaInicial"]);
            model.FechaFinal = Convert.ToDateTime(Request.Params["FechaFinal"]);
            model.Etapas = Request.Params["Etapas"];

            return View(model);
        }

        public ActionResult Imagen()
        {
            //base.ValidarSesionUsuario();
            //if (!base.IsValidSesionView()) return base.RedirectToLogin();
            ReporteFrecuenciaModel model = new ReporteFrecuenciaModel();
            //model.bNuevo = (new Funcion()).ValidarPermisoNuevo(Session[DatosSesion.SesionIdOpcion], User.Identity.Name);

            //model.bNuevo = true;
            //model.bEditar = true;
            //model.bGrabar = true;
            //model.IdGPS = Convert.ToInt32(Request.Params["IdGPS"]);
            //model.FechaInicial = Convert.ToDateTime(Request.Params["FechaInicial"]);
            //model.FechaFinal = Convert.ToDateTime(Request.Params["FechaFinal"]);

            return View(model);
        }

        public List<ReporteFrecuenciaDescargaDTO> obtenerData(List<ReporteFrecuenciaDescargaDTO> resultado) 
        {
            List<ReporteFrecuenciaDescargaDTO> resultadoFinal = new List<ReporteFrecuenciaDescargaDTO>();
            int Contador = 0;
            List<double> listFrecMinuto = new List<double>();
            foreach (ReporteFrecuenciaDescargaDTO result in resultado)
            {
                if (Contador==0)
                {
                    resultadoFinal.Add(result);
                } else
                {
                    int difSegs = 0;
                    difSegs = Contador % 60;
                    if (difSegs == 0)
                    {
                        ReporteFrecuenciaDescargaDTO puntoFrecMin = new ReporteFrecuenciaDescargaDTO();
                        puntoFrecMin.Frecuencia = listFrecMinuto.Min().ToString();
                        resultadoFinal.Add(puntoFrecMin);

                        ReporteFrecuenciaDescargaDTO puntoFrecMax = new ReporteFrecuenciaDescargaDTO();
                        puntoFrecMax.Frecuencia = listFrecMinuto.Max().ToString();
                        resultadoFinal.Add(puntoFrecMax);

                        //List<double> listFrecMinuto = new List<double>();
                        listFrecMinuto.Clear();
                    }
                    else 
                    {
                        listFrecMinuto.Add(Convert.ToDouble(result.Frecuencia));

                    } 

                }
                Contador++;
            }
            return resultadoFinal;

        }

        public ActionResult GetImg(string IdGPS, string FechaInicial, string FechaFinal, string Etapas)
        {

            ReporteFrecuenciaParam param = new ReporteFrecuenciaParam();
            param.FechaInicial = DateTime.Parse(FechaInicial);
            param.FechaFinal = DateTime.Parse(FechaFinal);
            param.IdGPS = Convert.ToInt32(IdGPS);

            List<ReporteFrecuenciaDescargaDTO> resultado = new ReporteFrecuenciaAppServicio().ObtenerFrecuencia(param);

            List<ReporteFrecuenciaDescargaDTO> resultadoFinal = new List<ReporteFrecuenciaDescargaDTO>();

            if (resultado.Count>7200)
            {
                resultadoFinal = obtenerData(resultado);
            } else
            {
                resultadoFinal = resultado;
            }

            string strFechaInicio = param.FechaInicial.ToString(Constantes.FormatoFecha);

            Chart chart = new Chart();
            chart.Width = 1200;
            chart.Height = 680;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(strFechaInicio));

            
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(System.Web.UI.DataVisualization.Charting.SeriesChartType.Spline, resultadoFinal));


            
            
            if (!string.IsNullOrEmpty(Etapas))
            {
                List<double> listFrec = new List<double>();
                listFrec.Add(this.frecMinima);
                listFrec.Add(this.frecMaxima);
                string[] arrayEtapa =  Etapas.Split('|');
                foreach(string etapa in arrayEtapa) {
                    if (!string.IsNullOrEmpty(etapa))
                    {
                        string[] arrayEtapaValor = etapa.Split(':');
                        //string strNombreEtapa = arrayEtapaValor[0];
                        string strValorUmbral = arrayEtapaValor[1];
                        listFrec.Add(Convert.ToDouble(strValorUmbral));
                    }                    
                }
                this.frecMinima = listFrec.Min();
                this.frecMaxima = listFrec.Max();
            }

            chart.ChartAreas.Add(CreateChartArea(resultadoFinal.Count));


            if (!string.IsNullOrEmpty(Etapas))
            {
                List<double> listFrec = new List<double>();
                //listFrec.Add(this.frecMinima);
                string[] arrayEtapa = Etapas.Split('|');
                foreach (string etapa in arrayEtapa)
                {
                    if (!string.IsNullOrEmpty(etapa))
                    {
                        string[] arrayEtapaValor = etapa.Split(':');
                        string strNombreEtapa = arrayEtapaValor[0];
                        string strValorUmbral = arrayEtapaValor[1];

                        StripLine stripline = new StripLine();
                        stripline.Interval = 0;
                        stripline.IntervalOffset = Convert.ToDouble(strValorUmbral);
                        listFrec.Add(Convert.ToDouble(strValorUmbral));
                        //stripline.Text = strNombreEtapa;
                        stripline.Text = "";
                        stripline.StripWidth = 0.002;
                        stripline.BackColor = Color.Blue;
                        stripline.TextLineAlignment = StringAlignment.Far;
                        stripline.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
                        stripline.ForeColor = Color.Black;
                        stripline.BorderDashStyle = ChartDashStyle.DashDotDot;
                        //stripline.TextOrientation = TextOrientation.Stacked;
                        //stripline.BackGradientStyle = GradientStyle.Center;
                        //stripline.ToolTip = "prueba";
                        //stripline.StripWidthType = DateTimeIntervalType.Auto;
                        //stripline.BackHatchStyle = ChartHatchStyle.DarkHorizontal;
                        chart.ChartAreas[0].AxisY.StripLines.Add(stripline);


                        //ChartArea ca = chart.ChartAreas[0];
                        //TextAnnotation ta = new TextAnnotation();
                        //inicio
                        ChartArea ca = chart.ChartAreas[0];
                        TextAnnotation ta = new TextAnnotation();
                        ta.Text = strNombreEtapa;
                        ta.ForeColor = Color.Blue;

                        ta.AxisY = ca.AxisY;
                        ta.AxisX = ca.AxisX;
                        ta.Y = Convert.ToDouble(strValorUmbral);
                        int numResultado = resultadoFinal.Count;
                        int numX = Convert.ToInt32(0.8 * numResultado);
                        ta.AnchorOffsetX = 1;
                        ta.AnchorX = 1;
                        ta.AxisX.Minimum = 0;
                        ta.AxisX.Maximum = numResultado;
                        ta.X = numX;  // pick a value that fits with your y-axis!
                        ta.ShadowColor = Color.White;
                        ta.BackColor = Color.Black;
                        ta.LineColor = Color.Blue;
                        ta.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);

                        ta.Alignment = ContentAlignment.TopCenter;
                        //ta.Width = 600;
                        chart.Annotations.Add(ta);

                        


                    }
                }
                //this.frecMinima = listFrec.Min();
                //this.frecMaxima = listFrec.Max();
            }


            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public ActionResult GetImagen(string Etapas)
        {

            List<ReporteFrecuenciaDescargaDTO> resultado = new List<ReporteFrecuenciaDescargaDTO>();
            DateTime fecha = new DateTime(2024,1,1);
            //double frecInicio = 59.84;
            int segMaximo = 30;
            int numInicio = 10;
            for (var i = 1; i <= segMaximo; i++)
            {
                ReporteFrecuenciaDescargaDTO res = new ReporteFrecuenciaDescargaDTO();
                res.FechaHora = fecha.AddSeconds(i).ToString();
                //DateTime hora = fecha.AddSeconds(i).ToString().Substring(9);
                res.Hora = fecha.AddSeconds(i).ToString().Substring(numInicio);
                Random rnd = new Random();
#pragma warning disable SCS0005 // Weak random number generator.
                int frecRandom = rnd.Next(5980, 6040);
#pragma warning restore SCS0005 // Weak random number generator.
                double decFrec = (double) frecRandom / 100;
                res.Frecuencia = Convert.ToString(decFrec);
                //frecInicio = frecInicio + (i / 10);
                resultado.Add(res);

            }
           
            Chart chart = new Chart();
            chart.Width = 1200;
            chart.Height = 680;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(""));
            //chart.Legends.Add(CreateLegend());
            chart.Series.Add(CreateSeries(System.Web.UI.DataVisualization.Charting.SeriesChartType.Spline, resultado));
            chart.ChartAreas.Add(CreateChartArea(resultado.Count));

            chart.Series.Add(CreateSeries2(System.Web.UI.DataVisualization.Charting.SeriesChartType.Line, resultado));
            chart.ChartAreas.Add(CreateChartArea2(resultado.Count));

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public Title CreateTitle(string FechaInicio)
        {
            Title title = new Title();
            title.Text = "Frecuencia " + FechaInicio;
            //title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.ShadowColor = Color.White;
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.FromArgb(26, 59, 105);
            return title;
        }

        public Legend CreateLegend()
        {
            Legend legend = new Legend();
            legend.Title = "Frecuencia";
            legend.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            legend.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            legend.ShadowOffset = 3;
            legend.ForeColor = Color.FromArgb(26, 59, 105);
            return legend;
        }

        public Series CreateSeries(SeriesChartType chartType, List<ReporteFrecuenciaDescargaDTO> resultado)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = "Frecuencia";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = Color.Red;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 1;

            DataPoint point;

            int numResul = resultado.Count;
            int numRegistros = 30;
            int numInicio = 1;
            int numRegSep = 1;
            if (numResul>= numRegistros)
            {
                numRegSep = numResul / numRegistros;
            }

            List<double> listFrec = new List<double>();
            foreach (ReporteFrecuenciaDescargaDTO result in resultado)
            {
                point = new DataPoint();
                point.AxisLabel = ".";
                if (string.IsNullOrEmpty(result.Frecuencia))
                {
                    point.YValues = new double[] { double.Parse("0") };
                }
                else if (result.Frecuencia == "null")
                {
                    point.YValues = new double[] { double.Parse("0") };
                }
                else
                {
                    point.YValues = new double[] { double.Parse(result.Frecuencia) };
                    listFrec.Add(double.Parse(result.Frecuencia));
                }              
                seriesDetail.Points.Add(point);
                numInicio = numInicio + 1;               
            }
            if (listFrec.Count>1)
            {
                this.frecMinima = listFrec.Min();
                this.frecMaxima = listFrec.Max();
            }
            seriesDetail.ChartArea = "Result Chart";
            return seriesDetail;
        }

        public Series CreateSeries2(SeriesChartType chartType, List<ReporteFrecuenciaDescargaDTO> resultado)
        {
            Series seriesDetail = new Series();
            seriesDetail.Name = "Frecuencia2";
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = Color.FromArgb(198, 99, 99);
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;

            DataPoint point;
            point = new DataPoint();
            point.XValue = 59.98;
            point.YValues = new double[] { double.Parse(Convert.ToString(1)) };
            seriesDetail.Points.Add(point);

            point = new DataPoint();
            point.XValue = 60.12;
            point.YValues = new double[] { double.Parse(Convert.ToString(2)) };
            seriesDetail.Points.Add(point);

            seriesDetail.ChartArea = "Result Chart2";
            return seriesDetail;
        }

        public ChartArea CreateChartArea(int numResultados)
        {
            ChartArea chartArea = new ChartArea();

            //Inicio
            /*StripLine stripline1 = new StripLine();
            stripline1.Interval = 0;
            stripline1.IntervalOffset = Convert.ToDouble("59.85");
            //listFrec.Add(Convert.ToDouble(strValorUmbral));
            stripline1.Text = "Prueba 12345.";
            stripline1.StripWidth = 0.202;
            stripline1.BackColor = Color.Black;
            stripline1.TextLineAlignment = StringAlignment.Center;
            stripline1.Font = new Font("Trebuchet MS", 12F, FontStyle.Bold);
            stripline1.ForeColor = Color.Black;
            stripline1.BorderDashStyle = ChartDashStyle.DashDotDot;
            //stripline.TextOrientation = TextOrientation.Stacked;
            chartArea.AxisY.StripLines.Add(stripline1);*/
            //Fin

            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.White;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            //chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisX.LineColor = Color.Transparent;
            chartArea.AxisX.Enabled = AxisEnabled.False;
            //chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.Black;
            chartArea.AxisX.MajorGrid.LineColor = Color.Transparent;
            chartArea.AxisX.Interval = 1;
            chartArea.AxisY.Minimum = this.frecMinima - 0.01;
            chartArea.AxisY.Maximum = this.frecMaxima + 0.01;
            double dblDifFrecMaxMin = this.frecMaxima - this.frecMinima;
            double dblInterval = dblDifFrecMaxMin / 10;
            dblInterval = Math.Round((Double)dblInterval, 3);
            chartArea.AxisY.Interval = dblInterval;

            chartArea.AxisX.Minimum = 1;
            chartArea.AxisX.Maximum = numResultados;


            

            return chartArea;
        }

        public ChartArea CreateChartArea2(int numResultados)
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart2";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        12F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        12F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(32, 32, 32, 32);
            chartArea.AxisX.LineColor = Color.FromArgb(32, 32, 32, 32);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(32, 32, 32, 32);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(32, 32, 32, 32);
            chartArea.AxisX.Interval = 1;
            //chartArea.AxisX.MajorGrid.Interval = 5;
            //chartArea.AxisX.MajorTickMark.Interval = 1;
            chartArea.AxisY.Minimum = 50.88;
            chartArea.AxisX.Minimum = 1;
            chartArea.AxisX.Maximum = 2;

            return chartArea;
        }

        public ChartArea CreateChartAreaResultado()
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "Result Chart";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            //chartArea.AxisX.MajorGrid.Interval = 5;
            //chartArea.AxisX.MajorTickMark.Interval = 1;
            chartArea.AxisY.Minimum = 59.80;
            chartArea.AxisX.Minimum = 1;
            chartArea.AxisX.Maximum = 10;
            return chartArea;
        }




    }
}

