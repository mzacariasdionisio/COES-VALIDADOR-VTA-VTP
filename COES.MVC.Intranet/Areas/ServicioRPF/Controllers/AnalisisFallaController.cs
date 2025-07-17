using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Helper;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.ServicioCloud;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class AnalisisFallaController : Controller
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AnalisisNormalController));

        /// <summary>
        /// Lista de datos cargados
        /// </summary>
        public List<RegistroRPF> ListaDatos
        {
            get
            {
                return (Session[DatosSesion.ListaDatosFalla] != null) ?
                    (List<RegistroRPF>)Session[DatosSesion.ListaDatosFalla] : new List<RegistroRPF>();
            }
            set { Session[DatosSesion.ListaDatosFalla] = value; }
        }

        /// <summary>
        /// Lista de frecuencias para el análisis
        /// </summary>
        public List<decimal> ListaFrecuencia
        {
            get
            {
                return (Session[DatosSesion.ListaFrecuenciaFalla] != null) ?
                    (List<decimal>)Session[DatosSesion.ListaFrecuenciaFalla] : new List<decimal>();
            }
            set { Session[DatosSesion.ListaFrecuenciaFalla] = value; }
        }

        /// <summary>
        /// Lista las frecuencias de la SE de San Juan
        /// </summary>
        public List<decimal> ListaFrecuenciaTotal
        {
            get
            {
                return (Session[DatosSesion.ListaFrecuenciaFallaTotal] != null) ?
                    (List<decimal>)Session[DatosSesion.ListaFrecuenciaFallaTotal] : new List<decimal>();
            }
            set { Session[DatosSesion.ListaFrecuenciaFallaTotal] = value; }
        }

        /// <summary>
        /// Almacena los valores de la potencia
        /// </summary>
        public List<ServicioRpfDTO> ListaPotencia
        {
            get
            {
                return (Session[DatosSesion.ListaPotenciaFalla] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaPotenciaFalla] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaPotenciaFalla] = value; }
        }

        /// <summary>
        /// Permite determinar que caso de evaluación se realizará
        /// </summary>
        public string IndicadorCaso
        {
            get
            {
                return (Session[DatosSesion.IndicadorEvaluacion] != null) ?
                    Session[DatosSesion.IndicadorEvaluacion].ToString() : Constantes.SI;
            }
            set { Session[DatosSesion.IndicadorEvaluacion] = value; }
        }

        /// <summary>
        /// Número de segundos
        /// </summary>
        public int Segundos
        {
            get
            {
                return (Session[DatosSesion.NumeroSegundos] != null) ?
                        int.Parse(Session[DatosSesion.NumeroSegundos].ToString()) : 0;
            }
            set
            {
                Session[DatosSesion.NumeroSegundos] = value;
            }
        }

        /// <summary>
        /// Fecha del proceso
        /// </summary>
        public DateTime FechaProceso
        {
            get
            {
                return (Session[DatosSesion.FechaProceso] != null) ?
                    Convert.ToDateTime(Session[DatosSesion.FechaProceso]) : DateTime.Now;
            }
            set
            {
                Session[DatosSesion.FechaProceso] = value;
            }
        }

        /// <summary>
        /// Instancias de servicio en la nube
        /// </summary>
        ServicioCloudClient servicioCloud = new ServicioCloudClient();

        /// <summary>
        /// Instancia de objeto para acceso a datos
        /// </summary>
        RpfAppServicio logic = new RpfAppServicio();

        /// <summary>
        /// Permite mostrar la página de inicio
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AnalisisFallaModel model = new AnalisisFallaModel();
            model.FechaProceso = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);
            model.ListaUnidades = this.logic.ObtenerUnidadesCarga(DateTime.Now.AddDays(-1));

            return View(model);
        }

        /// <summary>
        /// Permite validar los datos ingresados
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="hora"></param>
        /// <param name="ptoMediCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Validacion(string fecha, string hora, string unidades, decimal reserva)
        {
            AnalisisFallaModel model = new AnalisisFallaModel();

            DateTime date = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string[] horas = hora.Split(Constantes.CaracterDosPuntos);
            int hour = int.Parse(horas[0]);
            int minute = int.Parse(horas[1]);
            int second = int.Parse(horas[2]);

            this.ListaDatos = null;
            this.ListaFrecuencia = null;
            this.ListaFrecuenciaTotal = null;

            model.IndicadorExistenciaDatos = Constantes.NO;
            model.IndicadorExistenciaPotencia = Constantes.NO;
            model.IndicadorExistenciaRPF = Constantes.NO;
            model.ValidacionFrecuencia = false;
            model.ValidacionGeneral = false;
            model.ValidacionPotencia = false;

            DateTime fechaProceso = date.AddHours(hour).AddMinutes(minute).AddSeconds(second);
            List<decimal> frecuencias = this.logic.ObtenerFrecuenciasSanJuan(fechaProceso);
            this.ListaFrecuenciaTotal = this.logic.ObtenerFrecuenciaSanJuanTotal(fechaProceso);

            model.ListaFrecuencias = frecuencias;

            List<RegistroRPF> list = this.servicioCloud.ObtenerDatosFallas(fechaProceso).ToList();

            if (list.Count > 0)
            {
                decimal? reservaPrimaria = reserva;
                decimal? potencia = null;
                model.IndicadorExistenciaDatos = Constantes.SI;

                int segundos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.SegundosPosteriores);

                this.ListaDatos = list;
                this.ListaFrecuencia = frecuencias;
                this.FechaProceso = fechaProceso;

                List<int> puntos = unidades.Split(Constantes.CaracterComa).Select(int.Parse).ToList();
                decimal potenciaSuma = 0;
                foreach (int punto in puntos)
                {
                    List<RegistroRPF> datos = list.Where(x => x.PTOMEDICODI == punto).ToList();
                    if (datos.Count > 0)
                    {
                        potenciaSuma = potenciaSuma + datos[10].POTENCIA - datos[10 + segundos].POTENCIA;
                    }
                }

                if (potenciaSuma != 0)
                {
                    potencia = potenciaSuma;
                    model.IndicadorExistenciaPotencia = Constantes.SI;
                    model.PotenciaDesconectada = (potenciaSuma).ToString();
                }

                if (reservaPrimaria != null)
                {
                    model.IndicadorExistenciaRPF = Constantes.SI;
                    model.ReservaPrimaria = ((decimal)reservaPrimaria).ToString();
                }

                if (reservaPrimaria != null && potencia != null)
                {
                    bool flagPotencia = true;
                    bool flagFrecuencia = true;

                    if (potencia < reservaPrimaria)
                    {
                        flagPotencia = false;
                    }
                    foreach (decimal frecuencia in frecuencias)
                    {
                        if (frecuencia < 60)
                        {
                            flagFrecuencia = false;
                            break;
                        }
                    }

                    model.ValidacionPotencia = flagPotencia;
                    model.ValidacionFrecuencia = flagFrecuencia;
                    model.ValidacionGeneral = flagFrecuencia & flagPotencia;

                    if (flagFrecuencia & flagPotencia) this.IndicadorCaso = Constantes.SI;
                    else this.IndicadorCaso = Constantes.NO;
                }
            }

            return PartialView(model);
        }

        /// <summary>
        /// Permite hacer una revalidacion de los datos
        /// </summary>
        /// <param name="potencia"></param>
        /// <param name="reserva"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ReValidacion(string potencia, string reserva)
        {
            AnalisisFallaModel model = new AnalisisFallaModel();

            decimal potenciaDesconectada = decimal.Parse(potencia);
            decimal reservaPrimaria = decimal.Parse(reserva);

            bool flagPotencia = true;
            bool flagFrecuencia = true;

            if (potenciaDesconectada < reservaPrimaria)
            {
                flagPotencia = false;
            }
            foreach (decimal frecuencia in this.ListaFrecuencia)
            {
                if (frecuencia < 60)
                {
                    flagFrecuencia = false;
                    break;
                }
            }

            model.ValidacionPotencia = flagPotencia;
            model.ValidacionFrecuencia = flagFrecuencia;
            model.ValidacionGeneral = flagFrecuencia & flagPotencia;

            if (flagFrecuencia & flagPotencia) this.IndicadorCaso = Constantes.SI;
            else this.IndicadorCaso = Constantes.NO;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra la pantalla para la evaluación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Evaluacion(int segundos)
        {
            AnalisisFallaModel model = new AnalisisFallaModel();
            model.ListaUnidades = this.logic.ObtenerUnidadesCarga(DateTime.Now);
            this.Segundos = segundos;

            return PartialView(model);
        }

        /// <summary>
        /// Muestra la vista para carga de la potencia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargaPotencia()
        {
            AnalisisFallaModel model = new AnalisisFallaModel();
            model.ListaPotencia = this.ListaPotencia;

            return PartialView(model);
        }

        /// <summary>
        /// Permite cargar el archivo de potencia
        /// </summary>
        /// <returns></returns>
        public ActionResult Upload()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga;

                if (Request.Files.Count == 1)
                {
                    var file = Request.Files[0];
                    string fileName = path + Constantes.ArchivoPotencia;

                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    file.SaveAs(fileName);
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Permite cargar la potencia desde excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DatosPotencia()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + Constantes.RutaCarga + Constantes.ArchivoPotencia;
                List<ServicioRpfDTO> list = (new RpfHelper()).LeerDesdeFormato(path);
                this.ListaPotencia = list;
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite graficar los datos
        /// </summary>
        /// <param name="ptoMediCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Grafico(int ptoMediCodi)
        {
            SerieGraficoRpf grafico = new SerieGraficoRpf();
            try
            {
                if (this.ListaDatos != null)
                {
                    List<RegistroRPF> list = this.ListaDatos.Where(x => x.PTOMEDICODI == ptoMediCodi).ToList();
                    if (list != null)
                    {
                        if (list.Count >= 60)
                        {
                            if (this.ListaPotencia != null)
                            {
                                ServicioRpfDTO itemPotencia = this.ListaPotencia.Where(x => x.PTOMEDICODI == ptoMediCodi).FirstOrDefault();

                                if (itemPotencia != null)
                                {
                                    decimal potenciaMax = itemPotencia.POTENCIAMAX;
                                    decimal frec30Seg = list[40].FRECUENCIA;
                                    //decimal potenciaGenerada = list[10].POTENCIA;

                                    decimal potenciaGenerada = (list[10].POTENCIA + list[9].POTENCIA + list[8].POTENCIA + list[7].POTENCIA + list[6].POTENCIA) / 5M;


                                    decimal frecFalla = 0;
                                    int nroSegundos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.SegundosFrecuenciaFalla);
                                    for (int i = 0; i < nroSegundos; i++)
                                    {
                                        frecFalla = frecFalla + list[9 - i].FRECUENCIA;
                                    }
                                    frecFalla = frecFalla / nroSegundos;

                                    if (frecFalla > 0)
                                    {
                                        int indice = this.CalcularIndiceSalida();
                                        decimal ra = (new ConfiguracionRPF()).ObtenerValorRA(potenciaMax, frec30Seg, frecFalla, potenciaGenerada);

                                        List<decimal> frecuencias = new List<decimal>();
                                        if (this.ListaFrecuencia.Count == 10)
                                        {
                                            frecuencias.AddRange(this.ListaFrecuencia);
                                        }
                                        else
                                        {
                                            for (int i = 0; i < 10; i++) { frecuencias.Add(0); }
                                        }
                                        frecuencias.AddRange(this.ListaFrecuenciaTotal);

                                        bool isTV = (new RpfHelper()).IsTurboVapor(ptoMediCodi);
                                        if (isTV && indice > 60) indice = 60;

                                        grafico = (new RpfHelper()).ObtenerGraficoFalla(ra, list, indice, frecuencias);
                                    }
                                    else
                                    {
                                        grafico.Indicador = -7;
                                    }
                                }
                                else
                                {
                                    grafico.Indicador = -6;
                                }
                            }
                            else
                            {
                                grafico.Indicador = -5;
                            }
                        }
                        else
                        {
                            grafico.Indicador = -4;
                        }
                    }
                    else
                    {
                        grafico.Indicador = -2;
                    }
                }
                else
                {
                    grafico.Indicador = -3;
                }
            }
            catch
            {
                grafico.Indicador = -1;
            }

            return Json(grafico);
        }

        /// <summary>
        /// Permite calcular el índice para la salida
        /// </summary>
        /// <returns></returns>
        private int CalcularIndiceSalida()
        {
            if (this.IndicadorCaso == Constantes.SI)
            {
                decimal frecUmbral = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.FrecuenciaUmbralFalla);

                int indice = 0;

                if (this.ListaFrecuenciaTotal.Count > 0)
                {
                    for (int i = 0; i < this.ListaFrecuenciaTotal.Count - 1; i++)
                    {
                        indice++;
                        if (this.ListaFrecuenciaTotal[i + 1] > this.ListaFrecuenciaTotal[i])
                        {
                            break;
                        }
                    }
                }

                int index = indice;

                for (int i = indice; i < this.ListaFrecuenciaTotal.Count; i++)
                {
                    if (this.ListaFrecuenciaTotal[i] >= frecUmbral)
                    {
                        index = i;
                        break;
                    }
                }

                return index;
            }
            else
            {
                return this.Segundos;
            }
        }

        /// <summary>
        /// Permite mostrar el reporte de cumplimiento en pantalla
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte()
        {
            AnalisisFallaModel model = new AnalisisFallaModel();

            List<ServicioRpfDTO> listConfiguracion = this.logic.ObtenerUnidadesCarga(DateTime.Now);
            List<RegistroRPF> listDatos = this.ListaDatos;
            List<ServicioRpfDTO> listPotencias = this.ListaPotencia;
            int indice = this.CalcularIndiceSalida();
            List<ServicioRpfDTO> listReporte = (new RpfHelper()).ObtenerReporteFalla(listConfiguracion, listDatos, listPotencias, indice);

            model.ListaReporte = listReporte;
            return PartialView(model);
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarReporte()
        {
            int indicador = 1;

            try
            {
                List<ServicioRpfDTO> listConfiguracion = this.logic.ObtenerUnidadesCarga(DateTime.Now);
                List<RegistroRPF> listDatos = this.ListaDatos;
                List<ServicioRpfDTO> listPotencias = this.ListaPotencia;
                int indice = this.CalcularIndiceSalida();
                List<ServicioRpfDTO> listReporte = (new RpfHelper()).ObtenerReporteFalla(listConfiguracion, listDatos, listPotencias, indice);

                ExcelDocument.GenerarReporteCumplimientoFalla(listReporte);

                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                Logger.Error(Constantes.LogError, ex);
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Exportar()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteCumplimientoRPFFalla;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteCumplimientoRPFFalla);
        }

        /// <summary>
        /// Permite generar el reporte de cumplimiento en formato Word
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarWord(string fecha)
        {
            int indicador = 1;

            try
            {
                DateTime date = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<ServicioRpfDTO> listConfiguracion = this.logic.ObtenerUnidadesCarga(date);
                List<RegistroRPF> listDatos = this.ListaDatos;
                List<ServicioRpfDTO> listPotencias = this.ListaPotencia;
                int indice = this.CalcularIndiceSalida();

                List<decimal> frecuencias = new List<decimal>();
                if (this.ListaFrecuencia.Count == 10)
                {
                    frecuencias.AddRange(this.ListaFrecuencia);
                }
                else
                {
                    for (int i = 0; i < 10; i++) { frecuencias.Add(0); }
                }
                frecuencias.AddRange(this.ListaFrecuenciaTotal);

                List<ServicioRpfDTO> list = (new RpfHelper()).ObtenerReporteWordRPF(listConfiguracion, listDatos, listPotencias, indice, frecuencias);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF];

                List<ServicioRpfDTO> listReporte = list.Where(x => x.INDICADORCARGA == Constantes.SI).ToList();

                WordDocument.GenerarReporteCumplimientoFalla(listReporte, path, date, this.FechaProceso);

                indicador = 1;
            }
            catch (Exception ex)
            {
                indicador = -1;
                Logger.Error(Constantes.LogError, ex);
            }

            return Json(indicador);
        }

        /// <summary>
        /// Permite abrir el archivo del reporte  ante fallas en Word generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarWord()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteRPFFallaWord;
            return File(fullPath, Constantes.AppWord, Constantes.NombreReporteRPFFallaWord);
        }

        /// <summary>
        /// Permite mostrar la pantala de configuracion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Configuracion()
        {
            ConfiguracionRpfModel model = new ConfiguracionRpfModel();

            model.ListaParametro = (new ConfiguracionRPF()).ListarParametros(ValoresRPF.AnalisisFalla);

            return PartialView(model);
        }

        /// <summary>
        /// Permite grabar los parametros de configuración del análisis
        /// </summary>
        /// <param name="estatismo"></param>
        /// <param name="rPrimaria"></param>
        /// <param name="frecNominal"></param>
        /// <param name="porCumplimiento"></param>
        /// <param name="varPotencia"></param>
        /// <param name="varFrecuencia"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarParametro(decimal estatismo, decimal rPrimaria, decimal frecNominal,
            decimal porCumplimiento, decimal frecUmbral, decimal segundos, decimal segFalla)
        {
            try
            {
                List<ParametroRpfDTO> entitys = new List<ParametroRpfDTO>();

                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.EstatismoFalla, PARAMVALUE = estatismo.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.RPrimariaFalla, PARAMVALUE = rPrimaria.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaNominalFalla, PARAMVALUE = frecNominal.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.PorcentajeCumplimientoFalla, PARAMVALUE = porCumplimiento.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaUmbralFalla, PARAMVALUE = frecUmbral.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.SegundosPosteriores, PARAMVALUE = segundos.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.SegundosFrecuenciaFalla, PARAMVALUE = segFalla.ToString() });

                (new ConfiguracionRPF()).GrabarParametro(entitys, ValoresRPF.AnalisisFalla);

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }
    }
}
