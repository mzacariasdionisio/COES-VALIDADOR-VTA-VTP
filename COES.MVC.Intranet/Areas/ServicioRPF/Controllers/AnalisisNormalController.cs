using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Areas.ServicioRPF.Helper;
using COES.MVC.Intranet.Areas.ServicioRPF.Models;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.ServicioCloud;
using COES.Servicios.Aplicacion.ServicioRPF;
using log4net;
using COES.Servicios.Aplicacion.Eventos;
using COES.MVC.Intranet.Controllers;

namespace COES.MVC.Intranet.Areas.ServicioRPF.Controllers
{
    public class AnalisisNormalController : BaseController
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AnalisisNormalController));

        /// <summary>
        /// Instancias de servicio en la nube
        /// </summary>
        ServicioCloudClient servicioCloud = new ServicioCloudClient();

        /// <summary>
        /// Instancia de objeto para acceso a datos
        /// </summary>
        RpfAppServicio logic = new RpfAppServicio();

        /// <summary>
        /// Configuracion del analisis
        /// </summary>
        ConfiguracionRPF configuracion = new ConfiguracionRPF();

        /// <summary>
        /// Almacena los datos a procesar
        /// </summary>
        public List<RegistroRPF> ListaDatos
        {
            get
            {
                return (Session[DatosSesion.ListaEvaluacion] != null) ?
                    (List<RegistroRPF>)Session[DatosSesion.ListaEvaluacion] : new List<RegistroRPF>();
            }
            set { Session[DatosSesion.ListaEvaluacion] = value; }
        }

        /// <summary>
        /// Lista para almacenar los datos de configuracion
        /// </summary>
        public List<ServicioRpfDTO> ListaConfiguracion
        {
            get
            {
                return (Session[DatosSesion.ListaConfiguracion] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaConfiguracion] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaConfiguracion] = value; }
        }

        /// <summary>
        /// Lista de unidades sin rangos
        /// </summary>
        public List<ServicioRpfDTO> ListaRangoNoEncontrado
        {
            get
            {
                return (Session[DatosSesion.ListaRangoNoEncontrado] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaRangoNoEncontrado] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaRangoNoEncontrado] = value; }
        }

        /// <summary>
        /// Lista de unidades que no cargaron
        /// </summary>
        public List<ServicioRpfDTO> ListaNoCargaron
        {
            get
            {
                return (Session[DatosSesion.ListaNoCargaron] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaNoCargaron] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaNoCargaron] = value; }
        }

        /// <summary>
        /// Lista de datos que son incorrectos potencia o frecuencia en 0
        /// </summary>
        public List<ServicioRpfDTO> ListaIncorrecto
        {
            get
            {
                return (Session[DatosSesion.ListaIncorrecto] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaIncorrecto] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaIncorrecto] = value; }
        }

        /// <summary>
        /// Almacena los valores de la potencia
        /// </summary>
        public List<ServicioRpfDTO> ListaPotencia
        {
            get
            {
                return (Session[DatosSesion.ListaPotencia] != null) ?
                    (List<ServicioRpfDTO>)Session[DatosSesion.ListaPotencia] : new List<ServicioRpfDTO>();
            }
            set { Session[DatosSesion.ListaPotencia] = value; }
        }

        /// <summary>
        /// Permite mostrar la lista que no se incluirá en el segundo punto
        /// </summary>
        public List<RegistroRPF> ListaNoIncluida
        {
            get
            {
                return (Session[DatosSesion.ListaNoIncluir] != null) ?
                    (List<RegistroRPF>)Session[DatosSesion.ListaNoIncluir] : new List<RegistroRPF>();
            }
            set { Session[DatosSesion.ListaNoIncluir] = value; }
        }

        /// <summary>
        /// Permite obtener la lista del gráfico
        /// </summary>
        public List<RegistroRPF> ListaGrafico
        {
            get
            {
                return (Session[DatosSesion.ListaGrafico] != null) ?
                    (List<RegistroRPF>)Session[DatosSesion.ListaGrafico] : new List<RegistroRPF>();
            }
            set { Session[DatosSesion.ListaGrafico] = value; }
        }

        /// <summary>
        /// Carga inicial de la pagina
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            AnalisisModel model = new AnalisisModel();
            model.FechaProceso = DateTime.Now.ToString(Constantes.FormatoFecha);
            return View(model);
        }

        /// <summary>
        /// Obtencion del rango a evaluar
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult SeleccionRango(string fecha)
        {
            AnalisisModel model = new AnalisisModel();

            DateTime fechaProceso = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            List<ServicioRpfDTO> listConfiguracion = this.logic.ObtenerUnidadesCarga(fechaProceso);
            List<ServicioRpfDTO> list = listConfiguracion.Where(x => this.ListaPotencia.Any(y => x.PTOMEDICODI == y.PTOMEDICODI)).ToList();

            List<int> ids = (from puntos in list select puntos.PTOMEDICODI).Distinct().ToList();

            decimal ajuste = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.VariacionPotencia);
            int intentos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.NumeroIntentos);
            int minimo = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.MinimoExlusion);
            int nroDatos = (int)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.NumeroDatosNormal);

            int[] idsCargaron = null;
            int[] idsNoEncontrados = null;
            int[] idsPotenciaCero = null;
            int[] idsFrecuenciaCero = null;

            List<RegistroRPF> resultado = this.servicioCloud.ObtenerDatosAnalisis(fechaProceso, ajuste, intentos, minimo, nroDatos, out idsCargaron,
                out idsNoEncontrados, out idsPotenciaCero, out idsFrecuenciaCero).ToList();

            //Lista que no cargaron
            List<ServicioRpfDTO> listNoCargaron = list.Where(x => !idsCargaron.Any(y => x.PTOMEDICODI == y)).ToList();
            model.ListaNoCargaron = listNoCargaron;
            this.ListaNoCargaron = listNoCargaron;

            //Lista no encontrados rango
            List<ServicioRpfDTO> listNoEncontrado = list.Where(x => idsNoEncontrados.Any(y => x.PTOMEDICODI == y)).ToList();
            model.ListaNoEncontrados = listNoEncontrado;
            this.ListaRangoNoEncontrado = listNoEncontrado;

            //potencia o frecuencia en cero
            List<int> idsPotenciaError = new List<int>(idsPotenciaCero);
            List<int> idsIncorrecto = idsPotenciaCero.Union(idsFrecuenciaCero).ToList();
            List<ServicioRpfDTO> listIncorrecto = list.Where(x => idsIncorrecto.Any(y => x.PTOMEDICODI == y)).ToList();

            foreach (ServicioRpfDTO item in listIncorrecto)
            {
                if (idsPotenciaError.Contains(item.PTOMEDICODI))
                {
                    item.INDICADORPOTENCIA = Constantes.CaracterCero;
                }
                if (idsFrecuenciaCero.Contains(item.PTOMEDICODI))
                {
                    item.INDICADORFRECUENCIA = Constantes.CaracterCero;
                }
            }

            model.ListIncorrecto = listIncorrecto;
            this.ListaIncorrecto = listIncorrecto;

            //Lista que si tiene datos
            List<int> idsOk = (from item in resultado select item.PTOMEDICODI).Distinct().ToList();
            List<ServicioRpfDTO> listOk = list.Where(x => idsOk.Any(y => x.PTOMEDICODI == y)).ToList();

            foreach (ServicioRpfDTO item in listOk)
            {
                RegistroRPF entity = resultado.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).First();
                item.HORAINICIO = entity.HORAINICIO.ToString(Constantes.FormatoHora);
                item.HORAFIN = entity.HORAFIN.ToString(Constantes.FormatoHora);
            }
            model.ListaOK = listOk.OrderBy(x => x.HORAINICIO).ToList();

            this.ListaDatos = resultado;
            this.ListaConfiguracion = list;

            return PartialView(model);
        }

        /// <summary>
        /// Seleccionar y exluir unidades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult SeleccionUnidad()
        {
            AnalisisModel model = new AnalisisModel();
            List<RegistroRPF> list = this.ListaDatos;

            double frecuencia = this.configuracion.ObtenerFiltroEvaluacion();

            List<RegistroRPF> listIncluir = list.Where(x => x.FRECUENCIA > (decimal)frecuencia).ToList();
            List<RegistroRPF> listNoIncluir = list.Where(x => x.FRECUENCIA <= (decimal)frecuencia).ToList();
            List<ServicioRpfDTO> listConfiguracion = this.ListaConfiguracion;
            List<int> idsNoIncluir = (from item in listNoIncluir select item.PTOMEDICODI).Distinct().ToList();
            List<ServicioRpfDTO> listaExcluir = listConfiguracion.Where(x => idsNoIncluir.Any(y => y == x.PTOMEDICODI)).ToList();
            List<ServicioRpfDTO> listGrafico = listConfiguracion.Where(x => listIncluir.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();

            foreach (ServicioRpfDTO item in listaExcluir)
            {
                item.CONTADOR = listNoIncluir.Where(x => x.PTOMEDICODI == item.PTOMEDICODI).Count();
            }

            this.ListaNoIncluida = listNoIncluir;
            model.ListaNoIncluida = listaExcluir;
            model.ListaGrafico = listGrafico;
            this.ListaGrafico = listIncluir;

            return PartialView(model);
        }

        /// <summary>
        /// Mostrar los registros que se han excluido
        /// </summary>
        /// <param name="ptoMediCodi"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Excluidos(int ptoMediCodi)
        {
            AnalisisModel model = new AnalisisModel();
            List<RegistroRPF> list = this.ListaNoIncluida;
            model.ListaDatoExcluido = list.Where(x => x.PTOMEDICODI == ptoMediCodi).ToList();
            return PartialView(model);
        }

        /// <summary>
        /// Muestra la vista para carga de la potencia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult CargaPotencia()
        {
            AnalisisModel model = new AnalisisModel();
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
        /// Permite verificar si existen potencias cargadas
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult VerificarArchivoPotencia()
        {
            try
            {
                if (this.ListaPotencia != null)
                {
                    if (this.ListaPotencia.Count > 0)
                    {
                        return Json(1);
                    }
                }
                return Json(0);
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
            SerieGraficoRpf grafico = null;
            try
            {
                decimal banda = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.TamanioBanda);

                List<RegistroRPF> list = this.ListaGrafico.Where(x => x.PTOMEDICODI ==
                    ptoMediCodi).OrderBy(y => y.FECHAHORA).ToList();
                ServicioRpfDTO itemPotencia = this.ListaPotencia.Where(x => x.PTOMEDICODI == ptoMediCodi).FirstOrDefault();

                grafico = (new RpfHelper()).ObtenerGrafico(banda, list, itemPotencia);
            }
            catch
            {
                grafico = new SerieGraficoRpf();
                grafico.Indicador = -1;
            }

            return Json(grafico);
        }

        /// <summary>
        /// Permite mostrar el reporte de cumplimiento en pantalla
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Reporte()
        {
            decimal banda = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.TamanioBanda);

            AnalisisModel model = new AnalisisModel();

            List<RegistroRPF> listGrafico = this.ListaGrafico;
            List<ServicioRpfDTO> listConfiguracion = this.ListaConfiguracion;
            List<ServicioRpfDTO> list = listConfiguracion.Where(x => listGrafico.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();
            List<ServicioRpfDTO> listPotencia = this.ListaPotencia;
            List<ServicioRpfDTO> listReporte = (new RpfHelper()).ObtenerReporte(list, listGrafico, listPotencia, banda);
            List<ServicioRpfDTO> listNoCarga = listConfiguracion.Where(x => !listGrafico.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();

            foreach (ServicioRpfDTO item in listNoCarga)
            {
                item.PORCENTAJE = 0;
                item.INDCUMPLIMIENTO = Constantes.TextoNO;
            }

            listReporte.AddRange(listNoCarga);

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
                decimal banda = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.TamanioBanda);

                List<RegistroRPF> listGrafico = this.ListaGrafico;
                List<ServicioRpfDTO> listConfiguracion = this.ListaConfiguracion;
                List<ServicioRpfDTO> list = listConfiguracion.Where(x => listGrafico.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();
                List<ServicioRpfDTO> listPotencia = this.ListaPotencia;

                List<ServicioRpfDTO> listReporte = (new RpfHelper()).ObtenerReporte(list, listGrafico, listPotencia, banda);

                List<ServicioRpfDTO> listNoCarga = listConfiguracion.Where(x => !listGrafico.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();
                foreach (ServicioRpfDTO item in listNoCarga)
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoNO;
                }

                listReporte.AddRange(listNoCarga);


                ExcelDocument.GenerarReporteCumplimiento(listReporte);

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
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteCumplimientoRPF;
            return File(fullPath, Constantes.AppExcel, Constantes.NombreReporteCumplimientoRPF);
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
                decimal banda = (decimal)(new ConfiguracionRPF()).ObtenerParametro(ValoresRPF.TamanioBanda);
                DateTime date = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);

                List<RegistroRPF> listGrafico = this.ListaGrafico;
                List<ServicioRpfDTO> listConfiguracion = this.ListaConfiguracion;
                List<ServicioRpfDTO> list = listConfiguracion.Where(x => listGrafico.Any(y => y.PTOMEDICODI == x.PTOMEDICODI)).ToList();
                List<ServicioRpfDTO> listPotencia = this.ListaPotencia;

                List<ServicioRpfDTO> listReporte = (new RpfHelper()).ObtenerReporteWord(list, listGrafico, listPotencia, banda);

                List<ServicioRpfDTO> listRangoNoEncontrado = this.ListaRangoNoEncontrado;
                List<ServicioRpfDTO> listNoCargaron = this.ListaNoCargaron;
                List<ServicioRpfDTO> listIncorrecto = this.ListaIncorrecto;

                int[] idsRsf = { 58, 59, 61, 360, 361, 362, 423, 429, 438, 466 };
                List<ServicioRpfDTO> listRsf = listReporte.Where(x => idsRsf.Any(y => y == x.PTOMEDICODI)).ToList();

                foreach (ServicioRpfDTO item in listRsf)
                {
                    DateTime horaInicio = DateTime.ParseExact(item.HORAINICIO, Constantes.FormatoHora, CultureInfo.InvariantCulture);
                    DateTime horaFin = DateTime.ParseExact(item.HORAFIN, Constantes.FormatoHora, CultureInfo.InvariantCulture);
                    DateTime fechaInicio = date.AddHours(horaInicio.Hour).AddMinutes(horaInicio.Minute).AddSeconds(horaInicio.Second);
                    DateTime fechaFin = date.AddHours(horaFin.Hour).AddMinutes(horaFin.Minute).AddSeconds(horaInicio.Second);
                    bool flag = (new HoraOperacionAppServicio()).ExisteRegistro(item.PTOMEDICODI, fechaInicio, fechaFin);

                    if (flag)
                    {
                        foreach (ServicioRpfDTO rpf in listReporte)
                        {
                            if (rpf.PTOMEDICODI == item.PTOMEDICODI)
                            {
                                rpf.INDCUMPLIMIENTO = Constantes.TextoRSFA;
                                break;
                            }
                        }
                    }
                }

                List<int> ids = (from item in listReporte select item.PTOMEDICODI).ToList();
                ids.AddRange((from item in listRangoNoEncontrado select item.PTOMEDICODI).ToList());
                ids.AddRange((from item in listNoCargaron select item.PTOMEDICODI).ToList());
                ids.AddRange((from item in listIncorrecto select item.PTOMEDICODI).ToList());

                List<ServicioRpfDTO> listTotal = this.logic.ObtenerUnidadesCarga(date);
                List<ServicioRpfDTO> listNoSospechosos = listTotal.Where(x => !ids.Any(y => y == x.PTOMEDICODI)).ToList();

                foreach (ServicioRpfDTO item in listRangoNoEncontrado)
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoNoRango;
                    item.HORAINICIO = string.Empty;
                    item.HORAFIN = string.Empty;
                    item.ListaSerie = null;
                }

                foreach (ServicioRpfDTO item in listIncorrecto)
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoFPIncorrecto;
                    item.HORAINICIO = string.Empty;
                    item.HORAFIN = string.Empty;
                    item.ListaSerie = null;
                }

                foreach (ServicioRpfDTO item in listNoCargaron)
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoNoEnvio;
                    item.HORAINICIO = string.Empty;
                    item.HORAFIN = string.Empty;
                    item.ListaSerie = null;
                }

                foreach (ServicioRpfDTO item in listNoSospechosos)
                {
                    item.PORCENTAJE = 0;
                    item.INDCUMPLIMIENTO = Constantes.TextoNoSospechoso;
                    item.HORAINICIO = string.Empty;
                    item.HORAFIN = string.Empty;
                    item.ListaSerie = null;
                }

                listReporte.AddRange(listRangoNoEncontrado);
                listReporte.AddRange(listIncorrecto);
                listReporte.AddRange(listNoCargaron);
                listReporte.AddRange(listNoSospechosos);

                string path = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF];
                WordDocument.GenerarReporteCumplimiento(listReporte, path, date);

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
        /// Permite abrir el archivo del reporte en Word generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult ExportarWord()
        {
            string fullPath = ConfigurationManager.AppSettings[RutaDirectorio.ReporteServicioRPF] + Constantes.NombreReporteRPFWord;
            return File(fullPath, Constantes.AppWord, Constantes.NombreReporteRPFWord);
        }

        /// <summary>
        /// Permite mostrar la pantala de configuracion
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Configuracion()
        {
            ConfiguracionRpfModel model = new ConfiguracionRpfModel();
            model.ListaHistorico = (new ConfiguracionRPF()).ListaHistoricoParametro(ValoresRPF.RPrimaria);

            if (model.ListaHistorico != null)
            {
                if (model.ListaHistorico.Count > 0)
                {
                    decimal valorActual = 0;
                    bool flag = false;
                    foreach (ParametroDetRpfDTO item in model.ListaHistorico)
                    {
                        if (item.Paramvigencia.Subtract(DateTime.Now).TotalMinutes < 0)
                        {
                            valorActual = item.Paramvalor;
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        int id = ValoresRPF.RPrimaria;
                        (new ConfiguracionRPF()).ActualizaParametro(id, valorActual);
                    }
                }
            }

            model.ListaParametro = (new ConfiguracionRPF()).ListarParametros(ValoresRPF.AnalisisNormal);

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
        public JsonResult GrabarParametro(decimal estatismo, decimal rPrimaria, decimal frecNominal, decimal porCumplimiento,
            decimal varPotencia, decimal varFrecuencia, decimal intentos, decimal cantidad, decimal banda, int nroDatos, decimal? rPrimariaNew, string vigencia)
        {
            try
            {
                List<ParametroRpfDTO> entitys = new List<ParametroRpfDTO>();

                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.Estatismo, PARAMVALUE = estatismo.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.RPrimaria, PARAMVALUE = rPrimaria.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.FrecuenciaNominal, PARAMVALUE = frecNominal.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.PorcentajeEvaluacion, PARAMVALUE = porCumplimiento.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.VariacionPotencia, PARAMVALUE = varPotencia.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.VariacionFrecuencia, PARAMVALUE = varFrecuencia.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.NumeroIntentos, PARAMVALUE = intentos.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.MinimoExlusion, PARAMVALUE = cantidad.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.TamanioBanda, PARAMVALUE = banda.ToString() });
                entitys.Add(new ParametroRpfDTO { PARAMRPFCODI = ValoresRPF.NumeroDatosNormal, PARAMVALUE = nroDatos.ToString() });

                (new ConfiguracionRPF()).GrabarParametro(entitys, ValoresRPF.AnalisisNormal);

                if (!string.IsNullOrEmpty(vigencia))
                {
                    if (rPrimariaNew != null)
                    {
                        DateTime fechaVigencia = DateTime.ParseExact(vigencia, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
                        (new ConfiguracionRPF()).GrabarHistoricoParametro(ValoresRPF.RPrimaria, (decimal)rPrimariaNew, fechaVigencia, base.UserName);
                    }
                }

                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite mostrar el historico de un parametro
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult Historico()
        {
            ConfiguracionRpfModel model = new ConfiguracionRpfModel();
            model.ListaHistorico = (new ConfiguracionRPF()).ListaHistoricoParametro(ValoresRPF.RPrimaria);

            return PartialView(model);
        }
    }
}
