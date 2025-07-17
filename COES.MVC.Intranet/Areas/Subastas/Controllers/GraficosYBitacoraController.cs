using COES.Framework.Base.Core;
using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class GraficosYBitacoraController : BaseController
    {

        SubastasAppServicio servicio = new SubastasAppServicio();

        #region Declaración de variables

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error("Error", objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal("Error", ex);
                throw;
            }
        }


        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        #endregion


        #region Principal

        /// <summary>
        /// Pantalla principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            OfertaModel model = new OfertaModel();

            try
            {
                model.ListaEmpresaUsuarios = this.servicio.ListEmpresaSmaUserEmpresa(-1);
                model.FechaInicial = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
                model.FechaFinal = DateTime.Now.AddDays(1).ToString(ConstantesAppServicio.FormatoFecha);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return View(model);
        }

        #endregion

        #region Gráficos
        /// <summary>
        /// Busca la informacion para el grafico de potencia ofertada y reserva de RSF
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="tipoGrafico"></param>
        /// <param name="idsUrs"></param>
        /// <returns></returns>
        public JsonResult ConsultarGraficoOferta(string fechaOferta, string tipoOferta, string tipoGrafico, string idsUrs)
        {

            OfertaModel model = new OfertaModel();

            try
            {
                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                List<Dato48> listaPO = servicio.ObtenerDataGraficoPotenciaOfertada(fechaDeOferta, tipoOferta, tipoGrafico, idsUrs, out bool HayInfoPotencia);
                Dato48 listaReserva = servicio.ObtenerDataGraficoReservaRSF(fechaDeOferta, idsUrs, tipoGrafico);

                model.ExisteReserva = listaReserva.ConInformacion;
                model.Grafico = this.GraficoGeneracionSEIN(listaPO, fechaOferta, listaReserva, tipoOferta, tipoGrafico);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        /// <summary>
        /// Genera la informacion del grafico a mostrar
        /// </summary>
        /// <param name="listaDataPO"></param>
        /// <param name="fechaOferta"></param>
        /// <param name="listaDataReserva"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="tipoGrafico"></param>
        /// <returns></returns>
        public GraficoWeb GraficoGeneracionSEIN(List<Dato48> listaDataPO, string fechaOferta, Dato48 dataReserva, string tipoOferta, string tipoGrafico)
        {
            GraficoWeb Grafico = new GraficoWeb();
            switch (tipoOferta)
            {
                case ConstantesSubasta.FuenteExtranet:
                    switch (tipoGrafico)
                    {
                        case ConstantesSubasta.GraficoUrsSubir:
                            Grafico.TitleText = "Oferta Diaria (Mercado de Ajustes de RSF) por URS a Subir __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoUrsBajar:
                            Grafico.TitleText = "Oferta Diaria (Mercado de Ajustes de RSF) por URS a Bajar __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoTotalSubir:
                            Grafico.TitleText = "Oferta Diaria (Mercado de Ajustes de RSF) Total a Subir __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoTotalBajar:
                            Grafico.TitleText = "Oferta Diaria (Mercado de Ajustes de RSF) Total a Bajar __ " + fechaOferta;
                            break;
                    }
                    break;
                case ConstantesSubasta.FuenteTodos:
                    switch (tipoGrafico)
                    {
                        case ConstantesSubasta.GraficoUrsSubir:
                            Grafico.TitleText = "Oferta Diaria más Ofertas por Defecto Activadas por URS a Subir __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoUrsBajar:
                            Grafico.TitleText = "Oferta Diaria más Ofertas por Defecto Activadas por URS a Bajar __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoTotalSubir:
                            Grafico.TitleText = "Oferta Diaria más Ofertas por Defecto Activadas Total a Subir __ " + fechaOferta;
                            break;

                        case ConstantesSubasta.GraficoTotalBajar:
                            Grafico.TitleText = "Oferta Diaria más Ofertas por Defecto Activadas Total a Bajar __ " + fechaOferta;
                            break;
                    }
                    break;
            }

            Grafico.SeriesType = new List<string>();
            Grafico.SeriesName = new List<string>();
            Grafico.YAxixTitle = new List<string>();
            Grafico.SerieDataS = new DatosSerie[listaDataPO.Count][];

            //Eje X
            Grafico.XAxisCategories = new List<string>();
            DateTime horas = new DateTime(2013, 9, 15, 0, 0, 0);
            for (int h = 1; h <= 48; h++)
            {
                if (h == 48)
                {
                    horas = horas.AddMinutes(29);
                }
                else
                {
                    horas = horas.AddMinutes(30);
                }

                Grafico.XAxisCategories.Add(horas.ToString(ConstantesAppServicio.FormatoOnlyHora));
            }

            int numDatos = dataReserva.ConInformacion ? (listaDataPO.Count() + 1) : listaDataPO.Count();
            //Data potencia Ofertada
            Grafico.Series = new List<RegistroSerie>();
            Grafico.SeriesData = new decimal?[numDatos][];
            for (int i = 0; i < listaDataPO.Count; i++)
            {
                var pto = listaDataPO[i];
                Grafico.Series.Add(new RegistroSerie());
                Grafico.Series[i].Name = pto.Etiqueta;
                Grafico.Series[i].Type = "area";
                Grafico.Series[i].YAxis = 0;

                Grafico.SeriesData[i] = new decimal?[48];
                for (int h = 1; h <= 48; h++)
                {
                    Grafico.SeriesData[i][h - 1] = (decimal?)pto.GetType().GetProperty(ConstantesSubasta.CaracterV + h).GetValue(pto, null);
                }
            }

            //Data reserva deRSF
            if (dataReserva.ConInformacion)
            {
                var ptoReserva = dataReserva;
                Grafico.Series.Add(new RegistroSerie());
                Grafico.Series[listaDataPO.Count].Name = ptoReserva.Etiqueta;
                Grafico.Series[listaDataPO.Count].Type = "line";
                Grafico.Series[listaDataPO.Count].YAxis = 0;

                Grafico.SeriesData[listaDataPO.Count] = new decimal?[48];
                for (int h = 1; h <= 48; h++)
                {
                    Grafico.SeriesData[listaDataPO.Count][h - 1] = (decimal?)ptoReserva.GetType().GetProperty(ConstantesSubasta.CaracterV + h).GetValue(ptoReserva, null);
                }

            }

            return Grafico;
        }

        /// <summary>
        /// Actualiza la lista de URS segun filtro (solo muestra si dicha urs tiene informacion de potencia ofertada)
        /// </summary>
        /// <param name="fechaOferta"></param>
        /// <param name="tipoOferta"></param>
        /// <param name="tipoGrafico"></param>
        /// <returns></returns>
        public JsonResult ActualizarUrsSegunFiltro(string fechaOferta, string tipoOferta, string tipoGrafico)
        {
            OfertaModel model = new OfertaModel();

            try
            {
                DateTime fechaDeOferta = DateTime.ParseExact(fechaOferta, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                model.ListaUrsModoOperacion = servicio.ObtenerUrsConPotenciaOfertada(fechaDeOferta, tipoOferta, tipoGrafico);
                model.Resultado = 1;
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
                Log.Error(NameController, ex);
            }
            return Json(model);
        }

        #endregion

        #region Bitácora

        /// <summary>
        /// Devuelve el listado de la bitacora para el filtro ingresado
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult ListarBitacora(string fechaInicial, string fechaFinal)
        {
            OfertaModel model = new OfertaModel();
            try
            {
                DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
                DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

                int dias = Convert.ToInt32((fecFinal - fecInicial).TotalDays);
                if (dias >= 367)
                {
                    model.ListaBitacora = new List<Bitacora>();
                    model.Resultado = 2;
                }
                else
                {
                    model.ListaBitacora = this.servicio.CargarListarBitacora(fecInicial, fecFinal);
                    model.Resultado = 1;
                }
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Genera el archivo excel del reporte bitacora
        /// </summary>
        /// <param name="fechaInicial"></param>
        /// <param name="fechaFinal"></param>
        /// <returns></returns>
        public JsonResult GenerarExcelBitacora(string fechaInicial, string fechaFinal)
        {
            var model = new OfertaModel();
            DateTime fecInicial = DateTime.ParseExact(fechaInicial, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);
            DateTime fecFinal = DateTime.ParseExact(fechaFinal, ConstantesAppServicio.FormatoFecha, CultureInfo.InvariantCulture);

            try
            {
                base.ValidarSesionJsonResult();

                int dias = Convert.ToInt32((fecFinal - fecInicial).TotalDays);
                if (dias >= 367)
                {
                    model.Resultado = 2;
                }
                else
                {
                    string ruta = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel;
                    string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                    string nameFile = servicio.GenerarArchivoExcelBitacora(ruta, pathLogo, fecInicial, fecFinal, out bool existeDatos);

                    //model.FlagTieneDatos = existeDatos;
                    model.NombreArchivo = nameFile;
                    model.Resultado = 1;
                }
            }
            catch (Exception ex)
            {
                model.Resultado = -1;
                model.Mensaje = ex.Message;
                Log.Error(NameController, ex);
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite descargar el archivo Excel al explorador
        /// </summary>
        /// <param name="file">Nombre del archivo</param>
        /// <returns>Archivo</returns>
        public virtual ActionResult AbrirArchivo(string file)
        {
            base.ValidarSesionJsonResult();
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesAppServicio.PathArchivoExcel + file;
            return File(path, Constantes.AppExcel, file);
        }

        #endregion
    }
}