using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Globalization;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.Controllers;
using COES.Framework.Base.Core;
using System.Configuration;
using COES.Dominio.DTO.Scada;
using COES.Servicios.Aplicacion.TiempoReal;
using COES.MVC.Intranet.Areas.TiempoReal.Models;
using COES.MVC.Intranet.Areas.TiempoReal.Helper;
using COES.Servicios.Aplicacion.TiempoReal.Helper;
//using COES.Servicios.Aplicacion.Sp7;
using log4net;
using COES.Servicios.Aplicacion.Helper;

namespace COES.MVC.Intranet.Areas.TiempoReal.Controllers
{
    public class CircularSp7Controller : BaseController
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(CircularSp7Controller));
        //ConectorHISServicio servHIS = new ConectorHISServicio();
        ScadaSp7AppServicio servScadaSp7= new ScadaSp7AppServicio();
        TiempoRealAppServicio servTiempoReal = new TiempoRealAppServicio();

        public List<DatosSP7DTO> ListaTrCircularSp7
        {
            get
            {
                return (Session[ConstantesTiempoReal.ListaCircular] != null)
                    ? (List<DatosSP7DTO>)Session[ConstantesTiempoReal.ListaCircular]
                    : new List<DatosSP7DTO>();
            }
            set { Session[ConstantesTiempoReal.ListaCircular] = value; }
        }

        public ActionResult Index()
        {
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();
            model.ListaTrZonaSp7 = servScadaSp7.ListTrZonaSp7s();
            model.ListaTrCalidadSp7 = servTiempoReal.ListTrCalidadSp7s();

            model.Fecha = DateTime.Now.ToString(Constantes.FormatoFechaYYYYMMDD);

            model.HoraFin = DateTime.Now.AddMinutes(-30).ToString(Constantes.FormatoHora);
            model.HoraIni = "00:00";
            
            return View(model);
        }
        
        /// <summary>
        /// Permite obtener una lista filtrada por Zona (Ubicación)
        /// </summary>
        /// <param name="zonaCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListaCanalPorZona(int zonaCodi)
        {
            List<TrCanalSp7DTO> listCanales = new List<TrCanalSp7DTO>();
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();
            model.ListaTrCanalSp7 = servScadaSp7.GetByZonaTrCanalSp7(zonaCodi);

            foreach (TrCanalSp7DTO canal in model.ListaTrCanalSp7)
            {
                bool bExisteCanal = listCanales.Exists(x => x.Canalcodi == canal.Canalcodi);

                if(bExisteCanal == false)
                    listCanales.Add(canal);
            }
            return Json(listCanales);

        }

        /// <summary>
        /// Permite copiar la Grafica obtenida al model
        /// </summary>
        /// <param name="canalCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaGrafica(int canalCodi)
        {
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();

            model.ListaTrCircularSp7Grafica = ListaTrCircularSp7;

            return PartialView(model);
        }

        /// <summary>
        /// Permite copiar la Grafica obtenida al model
        /// </summary>
        /// <param name="canalCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ListaGraficaHistorial(List<int> canalCodi)
        {

            List<TrCanalSp7DTO> listCanales = new List<TrCanalSp7DTO>();
            foreach (int pCanalCodi in canalCodi)
            {
                var objCanal = servScadaSp7.GetByIdTrCanalSp7(pCanalCodi);
                listCanales.Add(objCanal);
            }
            ViewBag.listCanalCodi = listCanales;
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();

            model.ListaTrCircularSp7Grafica = ListaTrCircularSp7;

            return PartialView(model);
        }

        /// <summary>
        /// Crea el gráfico de Frecuencia
        /// </summary>
        /// <param name="canalCodi">Código de canal</param>
        /// <param name="fechaHoraIni">Fecha hora inicial</param>
        /// <param name="fechaHoraFin">Fecha hora final</param>
        /// <param name="canalNombre">Nombre de GPS</param>
        /// <param name="filtro">Filtro</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grafico(List<int> canalCodi, string fechaHoraIni, string fechaHoraFin, string canalNombre,
            string filtro)
        {
            //int canalCodi = 0;
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();
            try
            {
                DateTime fechaIni = DateTime.ParseExact(fechaHoraIni, Constantes.FormatoFechaHora,
                    CultureInfo.InvariantCulture);
                DateTime fechaFin = DateTime.ParseExact(fechaHoraFin, Constantes.FormatoFechaHora,
                    CultureInfo.InvariantCulture);
                //model = GraficoDetalle(canalCodi, fechaIni, fechaFin, canalNombre, filtro);
                model = GraficoDetalle(canalCodi, fechaIni, fechaFin, canalNombre, filtro);
            }
            catch(Exception ex)
            {
                log.Error("Error en Grafico() - " + ex.Message, ex);
            }
            var jsonResult = Json(model);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        /// <summary>
        /// Listado de datos de frecuencia a exportar
        /// </summary>
        /// <param name="canalNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Exportar(List<int> listCanalCodi, string canalNombre, string fechaIni, string fechaFin, string filtro)
        {
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();

            int result = 1;

            try
            {
                DateTime fechaIniFormat = DateTime.ParseExact(fechaIni, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                DateTime fechaFinFormat = DateTime.ParseExact(fechaFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                //GraficoDetalle(listCanalCodi, fechaIniFormat, fechaFinFormat, canalNombre, filtro);

                
                                  
                 model.ListaTrCircularSp7Grafica = ListaTrCircularSp7;
                 ExcelDocument.GenerarArchivoCircularSP7MultiplesCanales(listCanalCodi, model.ListaTrCircularSp7Grafica, canalNombre, fechaIni, fechaFin);
                 result = 1;
                
                
                
                   
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }

            return Json(result);
        }

        /// <summary>
        /// Listado de datos de frecuencia a exportar
        /// </summary>
        /// <param name="canalNombre">Nombre de GPS</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarCSV(List<int> listCanalCodi, string canalNombre, string fechaIni, string fechaFin, string filtro)
        {
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();

            int result = 1;

            try
            {
                DateTime fechaIniFormat = DateTime.ParseExact(fechaIni, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                DateTime fechaFinFormat = DateTime.ParseExact(fechaFin, Constantes.FormatoFechaHora, CultureInfo.InvariantCulture);
                ExcelDocument.GenerarArchivoCircularSP7MultiplesCanalesCSV(listCanalCodi, null, canalNombre, fechaIni, fechaFin);
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
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
                          NombreArchivo.ReporteCircular;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteCircular);
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarCSV()
        {
            string file = ConfigurationManager.AppSettings[RutaDirectorio.ArchivoReporteCircularZip];
            return File(file, Constantes.FileZip, "data.zip");
        }

        /// <summary>
        /// Permite exportar el reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarDirectorioCSV()
        {
            string file = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel +
                          NombreArchivo.ReporteCircular;
            return File(file, Constantes.AppExcel, NombreArchivo.ReporteCircular);
        }

        public List<DatosSP7DTO> ConvertirDto(List<TrCircularSp7DTO> listaIni)
        {
            List<DatosSP7DTO> lista = new List<DatosSP7DTO>();
            try
            {
                foreach(TrCircularSp7DTO item in listaIni)
                {
                    DatosSP7DTO entity = new DatosSP7DTO();
                    entity.Canalcodi = item.Canalcodi;
                    entity.Fecha = item.Canalfechahora;
                    entity.FechaSistema = item.Canalfhsist;
                    entity.Valor = (decimal)item.Canalvalor;
                    entity.Calidad = (int)item.Canalcalidad;
                    entity.CalidadTexto = item.GetCanalcalidadDescripcion() ?? string.Empty;

                    lista.Add(entity);
                }

            }
            catch(Exception ex)
            {
                log.Error("Error ConvertirDto-"+ ex.Message, ex);
            }


            return lista;
        }

        /// <summary>
        /// Genera el gráfico de tabla circular de Siemens
        /// </summary>
        /// <param name="canalCodi">Código de canal</param>
        /// <param name="fechaIni">Fecha inicial</param>
        /// <param name="fechaFin">Fecha final</param>
        /// <param name="canalNombre">Nombre de canal</param>
        /// <param name="filtro">T: Todas</param>
        ///                      V: Valido
        ///                      C: Congelado
        ///                      I: Indeterminado
        ///                      N: No válido
        /// <returns></returns>
        public BusquedaTrCircularSp7Model GraficoDetalle(List<int> listCanalCodi, DateTime fechaIni, DateTime fechaFin,
            string canalNombre, string filtro)
        {
            BusquedaTrCircularSp7Model model = new BusquedaTrCircularSp7Model();
            GraficoWeb grafico = new GraficoWeb();
            DateTime fechaIniAux = fechaIni;
            DateTime fechaFinAux = fechaFin;
            List<DatosSP7DTO> listCanalCodiRegistros = new List<DatosSP7DTO>();

            model.Grafico = grafico;

            model.Grafico.Series = new List<RegistroSerie>();
            model.Grafico.SerieDataS = new DatosSerie[listCanalCodi.Count][];
            model.Grafico.SeriesType = new List<string>();
            model.Grafico.SeriesName = new List<string>();
            model.Grafico.YAxixTitle = new List<string>();

            int posListCanalCodi = 0;

            foreach (int canalCodi in listCanalCodi)
            {
                var objCanal = servScadaSp7.GetByIdTrCanalSp7(canalCodi);
                canalNombre = objCanal.Canalnomb;
                string tipoDato = canalNombre.Substring(canalNombre.Length - 1, 1);


                //model.ListaTrCircularSp7Grafica = servHIS.ObtenerDatosHistoricos(fechaIni, fechaFin, canalCodi.ToString(), tipoDato).ToList();

                List<TrCircularSp7DTO> listaCircularSp7 = servTiempoReal.BuscarOperacionesRangoCircularSp7(canalCodi.ToString(), fechaIni, fechaFin, -1, -1).ToList();

                model.ListaTrCircularSp7Grafica = ConvertirDto(listaCircularSp7);



                List<DatosSP7DTO> listaTrCircularSp7GraficaFiltrada = new List<DatosSP7DTO>();

                //asignando calidad
                foreach (var item in model.ListaTrCircularSp7Grafica)
                {
                    if(filtro == "T")
                    {
                        listaTrCircularSp7GraficaFiltrada.Add(item);
                        continue;
                    }

                    if(filtro == item.Calidad.ToString())
                    {
                        listaTrCircularSp7GraficaFiltrada.Add(item);
                    }
                    
                    /*
                    if ((item.Calidad >= 0 && item.Calidad <= 15) || (item.Calidad >= 64 && item.Calidad <= 79) ||
                        (item.Calidad >= 128 && item.Calidad <= 143) || (item.Calidad >= 192 && item.Calidad <= 207))
                    {
                        if (filtro == "T" || filtro == "V")
                            listaTrCircularSp7GraficaFiltrada.Add(item);
                        continue;
                    }
                    else
                    {
                        //CONGELADO
                        if ((item.Calidad >= 16 && item.Calidad <= 31) || (item.Calidad >= 80 && item.Calidad <= 95) ||
                            (item.Calidad >= 144 && item.Calidad <= 159) || (item.Calidad >= 208 && item.Calidad <= 223))
                        {
                            if (filtro == "T" || filtro == "C")
                                listaTrCircularSp7GraficaFiltrada.Add(item);
                            continue;
                        }
                        else
                        {
                            //INDETERMINADO
                            if ((item.Calidad >= 32 && item.Calidad <= 47) || (item.Calidad >= 96 && item.Calidad <= 111) ||
                                (item.Calidad >= 160 && item.Calidad <= 175) || (item.Calidad >= 224 && item.Calidad <= 239))
                            {
                                if (filtro == "T" || filtro == "I")
                                    listaTrCircularSp7GraficaFiltrada.Add(item);
                                continue;
                            }
                            else
                            {
                                //NO VALIDO
                                if ((item.Calidad >= 48 && item.Calidad <= 63) ||
                                    (item.Calidad >= 112 && item.Calidad <= 127) ||
                                    (item.Calidad >= 176 && item.Calidad <= 191) ||
                                    (item.Calidad >= 240 && item.Calidad <= 255))
                                {
                                    if (filtro == "T" || filtro == "N")
                                        listaTrCircularSp7GraficaFiltrada.Add(item);
                                    continue;
                                }
                            }
                        }
                    }*/

                }

                //Considerar datos filtrados
                model.ListaTrCircularSp7Grafica = listaTrCircularSp7GraficaFiltrada;

                listCanalCodiRegistros.AddRange(model.ListaTrCircularSp7Grafica);
                //ListaTrCircularSp7.AddRange(model.ListaTrCircularSp7Grafica);
                //ListaTrCircularSp7 = model.ListaTrCircularSp7Grafica;

                //model.Grafico.SerieDataS = new DatosSerie[1][];
                //model.Grafico.SerieDataS.Add(new DatosSerie());
                model.Grafico.Series.Add(new RegistroSerie());


                model.Grafico.Series[posListCanalCodi].Name = canalNombre;
                model.Grafico.Series[posListCanalCodi].Type = "line";
                model.Grafico.Series[posListCanalCodi].Color = "#3498DB";
                model.Grafico.Series[posListCanalCodi].YAxisTitle = "MW";
                model.Grafico.SerieDataS[posListCanalCodi] = new DatosSerie[model.ListaTrCircularSp7Grafica.Count];

                model.Grafico.TitleText = @"titulo - texto";
                model.Grafico.YAxixTitle.Add("  ");

                if (model.ListaTrCircularSp7Grafica.Count > 0)
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

                    for (var i = 0; i < model.ListaTrCircularSp7Grafica.Count; i++)
                    {
                        DateTime fecha = (DateTime)model.ListaTrCircularSp7Grafica[i].FechaSistema;

                        var registro = model.ListaTrCircularSp7Grafica[i];

                        decimal? valor = 0;

                        valor = (decimal?)registro.GetType().GetProperty("Valor").GetValue(registro, null);
                        if (valor == null)
                            valor = 0;
                        if (i == 0)
                            model.Grafico.XAxisCategories.Add(registro.FechaSistema.ToString(Constantes.FormatoFechaHora));
                        var serie = new DatosSerie();
                        serie.X = registro.FechaSistema;
                        serie.Y = valor;

                        try
                        {
                            model.Grafico.SerieDataS[posListCanalCodi][indiceB] = serie;
                        }
                        catch(Exception ex)
                        {

                        }

                        indiceB++;
                    }
                }
                else
                {
                    //model.Grafico.Series[posListCanalCodi].Name = "SIN DATOS";
                }
                posListCanalCodi++;
            }

            ListaTrCircularSp7 = listCanalCodiRegistros;

            return model;
        }
    }
}