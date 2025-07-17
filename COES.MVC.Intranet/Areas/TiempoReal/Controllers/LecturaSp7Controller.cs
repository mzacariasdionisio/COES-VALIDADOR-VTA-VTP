using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.MVC.Intranet.Helper;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Controllers;
using COES.Framework.Base.Core;
using System.Configuration;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.Servicios.Aplicacion.Mediciones;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
using COES.MVC.Intranet.Areas.TiempoReal.Helper;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class LecturaSp7Controller : BaseController
    {
        ScadaSp7AppServicio servScadaSp7 = new ScadaSp7AppServicio();
        GpsAppServicio servGps = new GpsAppServicio();
        
        public List<FLecturaSp7DTO> ListaFLecturaSp7
        {
            get
            {
                return (Session[ConstantesTiempoReal.ListaFrecuencia] != null)
                    ? (List<FLecturaSp7DTO>)Session[ConstantesTiempoReal.ListaFrecuencia]
                    : new List<FLecturaSp7DTO>();
            }
            set { Session[ConstantesTiempoReal.ListaFrecuencia] = value; }
        }

        public ActionResult Index()
        {

            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();
            model.ListaMeGps = servGps.ListMeGpss();

            model.FechaFin = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFechaFull);
            model.FechaIni = DateTime.Now.AddDays(-1).AddSeconds(-3 * 60).ToString(Constantes.FormatoFechaFull);

            return View(model);
        }



        [HttpPost]
        public PartialViewResult ListaGrafica(int gpsCodi)
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();
            model.ListaFLecturaSp7Grafica = ListaFLecturaSp7;

            return PartialView(model);

        }
        


        /// <summary>
        /// Obtiene un listado según un listado de opciones        
        /// </summary>
        /// <param name="opcion">1: Maxima Frecuencia</param>
        ///                      2: Minima Frecuencia
        ///                      3: súbitas
        ///                      4: sostenidas
        /// <param name="gpsCodi">Código de GPS</param>
        /// <param name="fechaIni">Fecha Inicial</param>
        /// <param name="fechaFin">Fecha Final</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <returns></returns>
        [HttpPost]
        public BusquedaFLecturaSp7Model Listado(int opcion, string transgresion, int gpsCodi, DateTime fechaIni,
            DateTime fechaFin, string gpsNombre)
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();

            switch (opcion)
            {
                case 1: //maxima frecuencia
                    model.ListaFLecturaSp7Grafica =
                        servScadaSp7.ObtenerMaximaFrecuencia(gpsCodi, fechaIni, fechaFin).ToList();
                    ListaFLecturaSp7 = model.ListaFLecturaSp7Grafica;
                    break;

                case 2: //minima frecuencia
                    model.ListaFLecturaSp7Grafica =
                        servScadaSp7.ObtenerMinimaFrecuencia(gpsCodi, fechaIni, fechaFin).ToList();
                    ListaFLecturaSp7 = model.ListaFLecturaSp7Grafica;
                    break;
                case 3: //frecuencia subita
                    model.ListaFLecturaSp7Grafica =
                        servScadaSp7.ObtenerSubitaFrecuencia(gpsCodi, transgresion, fechaIni, fechaFin).ToList();
                    ListaFLecturaSp7 = model.ListaFLecturaSp7Grafica;
                    break;
                case 4: //frecuencia sostenida
                    model.ListaFLecturaSp7Grafica =
                        servScadaSp7.ObtenerSostenidaFrecuencia(gpsCodi, transgresion, fechaIni, fechaFin).ToList();
                    ListaFLecturaSp7 = model.ListaFLecturaSp7Grafica;
                    break;

            }


            if (opcion == 1)
            {

            }

            return model;
        }
        


        /// <summary>
        /// Crea el gráfico de Frecuencia
        /// </summary>
        /// <param name="gpsCodi">Código de GPS</param>
        /// <param name="fechaHoraIni">Fecha hora inicial</param>
        /// <param name="fechaHoraFin">Fecha hora final</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GraficoFrecuencia(int gpsCodi, string fechaHoraIni, string fechaHoraFin, string gpsNombre)
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();

            DateTime fechaIni = DateTime.ParseExact(fechaHoraIni, Constantes.FormatoFechaHora,
                CultureInfo.InvariantCulture);
            DateTime fechaFin = DateTime.ParseExact(fechaHoraFin, Constantes.FormatoFechaHora,
                CultureInfo.InvariantCulture);


            model = GraficoFrecuenciaDetalle(gpsCodi, fechaIni, fechaFin, gpsNombre);
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        /// <summary>
        /// Listado de datos de frecuencia a exportar
        /// </summary>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarFrecuencia(string gpsNombre, string fechaIni, string fechaFin)
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();

            int result = 1;

            try
            {

                if (ListaFLecturaSp7.Count > 0)
                {

                    model.ListaFLecturaSp7Grafica = ListaFLecturaSp7;
                    ExcelDocument.GenerarArchivoFrecuenciaSP7(model.ListaFLecturaSp7Grafica, gpsNombre, fechaIni,
                        fechaFin);

                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            catch
            {
                result = -1;
            }

            return Json(result);

        }



        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Descargar()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel +
                          NombreArchivo.ReporteFrecuencia;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteFrecuencia);

        }



        /// <summary>
        /// Genera el gráfico de Frecuencia
        /// </summary>
        /// <param name="gpsCodi">Código de GPS</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="gpsNombre">Nombre de GPS</param>
        /// <returns></returns>
        public BusquedaFLecturaSp7Model GraficoFrecuenciaDetalle(int gpsCodi, DateTime fechaIni, DateTime fechaFin,
            string gpsNombre)
        {
            BusquedaFLecturaSp7Model model = new BusquedaFLecturaSp7Model();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            int factor = 1;
            model.Grafico = grafico;

            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();

            model.ListaFLecturaSp7Grafica = servScadaSp7.BuscarOperaciones(gpsCodi, fechaIni, fechaFin).ToList();
            ListaFLecturaSp7 = model.ListaFLecturaSp7Grafica;

            model.Grafico.SerieDataS = new DatosSerie[1][];
            model.Grafico.Series.Add(new RegistroSerie());



            model.Grafico.Series[0].Name = gpsNombre;
            model.Grafico.Series[0].Type = "line";
            model.Grafico.Series[0].Color = "#3498DB";
            model.Grafico.Series[0].YAxisTitle = "MW";
            model.Grafico.SerieDataS[0] = new DatosSerie[model.ListaFLecturaSp7Grafica.Count];

            model.Grafico.TitleText = @"titulo - texto";
            model.Grafico.YAxixTitle.Add("Hz");


            if (model.ListaFLecturaSp7Grafica.Count > 0)
            {
                model.FechaIni = fechaIni.ToString(Constantes.FormatoFecha);
                model.FechaFin = fechaFin.ToString(Constantes.FormatoFecha);
                model.Grafico.XAxisTitle = "Dia:Horas";

                // titulo el reporte
                model.SheetName = "GRAFICO";
                model.Grafico.YaxixTitle = "(MWh)";
                model.Grafico.XAxisCategories = new List<string>();
                model.Grafico.SeriesType = new List<string>();
                model.Grafico.SeriesYAxis = new List<int>();

                // Obtener Lista de intervalos categoria del grafico   
                model.Grafico.SeriesYAxis.Add(0);

                int indiceB = 0;

                for (var i = 0; i < model.ListaFLecturaSp7Grafica.Count; i++)
                {
                    DateTime fecha = (DateTime) model.ListaFLecturaSp7Grafica[i].Fechahora;

                    var registro = model.ListaFLecturaSp7Grafica[i];

                    decimal? valor = 0;
                    valor = (decimal?) registro.GetType().GetProperty("H" + (0).ToString()).GetValue(registro, null);
                    if (valor == null)
                        valor = 0;
                    if (i == 0)
                        model.Grafico.XAxisCategories.Add(registro.Fechahora.ToString(Constantes.FormatoFechaHora));
                    var serie = new DatosSerie();
                    serie.X = registro.Fechahora;
                    serie.Y = valor;

                    model.Grafico.SerieDataS[0][indiceB] = serie;


                    indiceB++;


                }

            }
            else
            {
                model.Grafico.Series[0].Name = "SIN DATOS";
            }


            return model;
        }





    }
}
