using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Scada;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Models;
using COES.MVC.Extranet.Areas.Medidores.Models;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.General;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using COES.Servicios.Aplicacion.Mediciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing.Chart;
using System.Net;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.ServicioRPF;
using System.Configuration;
using COES.Servicios.Aplicacion.Mediciones;
using COES.MVC.Extranet.SeguridadServicio;

namespace COES.MVC.Extranet.Areas.Medidores.Controllers
{
    public class EnvioController : FormatoController
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "EnvioController";
        private readonly List<PeriodoDato> LsPeriodo = new List<PeriodoDato>();
        private readonly List<FuenteInformacion> LsFuente1 = new List<FuenteInformacion>();
        private readonly List<FuenteInformacion> LsFuente2 = new List<FuenteInformacion>();
        SeguridadServicioClient seguridad = new SeguridadServicioClient();
        int IdAplicacion = Convert.ToInt32(ConfigurationManager.AppSettings[DatosConfiguracion.IdAplicacionExtranet]);

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                Log.Error(NameController, objErr);
            }
            catch (Exception ex)
            {
                Log.Fatal(NameController, ex);
                throw;
            }
        }

        public EnvioController()
        {
            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoTodos, Valor = ConstantesMedidores.DescPeriodoTodos });
            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoHp, Valor = ConstantesMedidores.DescPeriodoHp });
            LsPeriodo.Add(new PeriodoDato { Codigo = ConstantesMedidores.PeriodoHfp, Valor = ConstantesMedidores.DescPeriodoHfp });

            FuenteInformacion f1 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteMedidores,
                Valor = ConstantesMedidores.DescFuenteMedidores,
                Nombre = ConstantesMedidores.DescNombreMedidores,
                Leyenda = ConstantesMedidores.DescLeyendaMedidores,
                Titulo = ConstantesMedidores.DescTituloMedidores
            };
            FuenteInformacion f2 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteCaudalTurbinado,
                Valor = ConstantesMedidores.DescFuenteCaudalTurbinado,
                Nombre = ConstantesMedidores.DescNombreCaudalTurbinado,
                Leyenda = ConstantesMedidores.DescLeyendaCaudalTurbinado,
                Titulo = ConstantesMedidores.DescTituloCaudalTurbinado
            };
            FuenteInformacion f3 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteDespachoDiario,
                Valor = ConstantesMedidores.DescFuenteDespachoDiario,
                Nombre = ConstantesMedidores.DescNombreDespachoDiario,
                Leyenda = ConstantesMedidores.DescLeyendaDespachoDiario,
                Titulo = ConstantesMedidores.DescTituloDespachoDiario
            };
            FuenteInformacion f4 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteDatosScada,
                Valor = ConstantesMedidores.DescFuenteDatosScada,
                Nombre = ConstantesMedidores.DescNombreDatosScada,
                Leyenda = ConstantesMedidores.DescLeyendaDatosScada,
                Titulo = ConstantesMedidores.DescTituloDatosScada
            };
            FuenteInformacion f5 = new FuenteInformacion
            {
                Codigo = ConstantesMedidores.IdFuenteRPF,
                Valor = ConstantesMedidores.DescFuenteRPF,
                Nombre = ConstantesMedidores.DescNombreRPF,
                Leyenda = ConstantesMedidores.DescLeyendaRPF,
                Titulo = ConstantesMedidores.DescTituloRPF
            };

            LsFuente1.Add(f1);

            LsFuente2.Add(f3);
            LsFuente2.Add(f5);
        }

        //
        // GET: /Medidores/Envio/
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();

            MedidorGeneracionModel model = new MedidorGeneracionModel();
            base.IndexFormato(model, ConstantesMedidores.IdFormatoMedidorGeneracion);

            List<MeHojaDTO> listaHoja = base.servFormato.ListMeHoja();
            model.TituloCargaCentralPotActiva = listaHoja.Where(x => x.Hojacodi == ConstantesMedidores.IdHojaCargaCentralPotActiva).FirstOrDefault().Hojanombre;
            model.TituloCargaCentralPotReactiva = listaHoja.Where(x => x.Hojacodi == ConstantesMedidores.IdHojaCargaCentralPotReactiva).FirstOrDefault().Hojanombre;
            model.TituloCargaServAuxPotReactiva = listaHoja.Where(x => x.Hojacodi == ConstantesMedidores.IdHojaCargaServAuxPotReactiva).FirstOrDefault().Hojanombre;

            model.IdFormatoCargaCentralPotActiva = ConstantesMedidores.IdFormatoCargaCentralPotActiva;
            model.IdFormatoCargaCentralPotReactiva = ConstantesMedidores.IdFormatoCargaCentralPotReactiva;
            model.IdFormatoCargaServAuxPotReactiva = ConstantesMedidores.IdFormatoCargaServAuxPotActiva;

            model.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            return View(model);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Medidor de Generación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato, int verUltimoEnvio)
        {
            base.ValidarSesionUsuario();
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelMedidorGeneracion(idEmpresa, idEnvio, fecha, mes, idFormato, verUltimoEnvio);
                //FormatoModel obj = serializer.DeserializeObject(jsModel.);
                var resultado = Json(jsModel);
                resultado.MaxJsonLength = int.MaxValue;
                return resultado;
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Medidor de Generación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelMedidorGeneracion(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato, int verUltimoEnvio)
        {
            MedidorGeneracionModel model = new MedidorGeneracionModel();
            model.UtilizaFiltroCentral = true;
            model.ValidaMedidorGeneracion = true;
            model.ValidaHorasOperacion = true;
            model.ValidaMantenimiento = false;
            model.ValidaRestricOperativa = false;
            model.ValidaEventos = false;
            model.UtilizaScada = false;
            model.UtilizaFiltroCentral = true;

            model.Mes = mes;
            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            var listaInf = model.ListaHojaPto.GroupBy(x => new { x.Tipoinfocodi, x.Tipoinfoabrev }).Select(
                    grp => new SiTipoinformacionDTO { Tipoinfocodi = grp.Key.Tipoinfocodi, Tipoinfoabrev = grp.Key.Tipoinfoabrev }).ToList();
            model.ListaTipoInformacion = listaInf;

            model.ListaFuente1 = LsFuente1;
            model.ListaFuente2 = LsFuente2.OrderByDescending(x => x.Codigo).ToList();

            DateTime fechaProceso = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaProceso;
            DateTime fechaFin = fechaProceso.AddMonths(1).AddDays(-1);
            model.ListaHOP = base.servHO.ListarReporteHOP(idEmpresa, fechaIni, fechaFin);

            return model;
        }

        /// <summary>
        /// Devuelve Vista Parcial para cada hoja de Medidor de Generación
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            //List<MeHojaDTO> listaHoja = base.servFormato.GetByCriteriaMeHoja(ConstantesMedidores.IdFormatoMedidorGeneracion);
            //var hoja = listaHoja.Where(x => x.Hojacodi == hojaNum).FirstOrDefault();
            var formato = base.servFormato.ListMeFormatos().Where(x => x.Formatcodi == idFormato).FirstOrDefault();

            var modelHoja = new MedidorGeneracionModel();
            base.IndexFormato(modelHoja, idFormato);

            modelHoja.OpAccesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, base.UserName);

            modelHoja.IdHoja = idHoja;
            modelHoja.Titulo = formato.Formatnombre;
            modelHoja.IdFormato = idFormato;
            modelHoja.DiaMes = DateTime.Now.Day * -1;
            modelHoja.Mes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1).ToString("MM yyyy");

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato 
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato, List<MeJustificacionDTO> listaJustificacion)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaExcelData = data;
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.Semana = semana;
            model.Mes = mes;
            model.IdFormato = idFormato;
            model.ListaJustificacion = listaJustificacion;

            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }

        /// <summary>
        /// Permite generar el formato en un archivo Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato)
        {
            string ruta = string.Empty;
            try
            {
                int idEnvio = -1;
                this.MatrizExcel = data;
                FormatoModel model = BuildHojaExcelMedidorGeneracion(idEmpresa, idEnvio, fecha, mes, idFormato, ConstantesFormato.NoVerUltimoEnvio);
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        /// <summary>
        /// Gráficos en Carga de Datos para usuario COES
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarGraficoCargaDatoUsuarioAgentes(string[][] data, int idEmpresa, int idFormato, string mes, int periodo, int tipoDato, int idCentral, int fuente2)
        {
            DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
            DateTime fechaIni = fechaPeriodo;
            DateTime fechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);
            GraficoMedidorGeneracion g = GenerarGrafico(data, idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, ConstantesMedidores.IdFuenteMedidores, fuente2, ConstantesMedidores.UsuarioAgentes);
            return Json(g);
        }

        /// <summary>
        /// Generar archivo excel del gráfico
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="mes"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="fuente2"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ExportarExcelGraficoCargaDatoUsuarioAgentes(string[][] data, int idEmpresa, int idFormato, string mes, int periodo, int tipoDato, int idCentral, int fuente2)
        {
            string ruta = string.Empty;
            string[] datos = new string[2];
            try
            {
                DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
                DateTime fechaIni = fechaPeriodo;
                DateTime fechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);
                GraficoMedidorGeneracion g = GenerarGrafico(data, idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, ConstantesMedidores.IdFuenteMedidores, fuente2, ConstantesMedidores.UsuarioAgentes);

                ruta = GenerarExcelGrafico(g);
                datos[0] = ruta;
                datos[1] = g.Nombre;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                datos[0] = "-1";
                datos[1] = "";
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        /// <summary>
        /// Permite descargar el formato de carga para usuario COES
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelGraficoCargaDato()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #region Validación Horas de Operación

        /// <summary>
        /// Validación de log HOP
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="mes"></param>
        /// <param name="idFormato"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarLogHOP(string[][] data, int idEmpresa, string mes, int idFormato)
        {
            string[] datos = new string[2];
            try
            {
                var opAccesoEmpresa = seguridad.ValidarPermisoOpcion(this.IdAplicacion, (int)base.IdOpcion, Acciones.AccesoEmpresa, base.UserName);
                if (!opAccesoEmpresa) throw new ArgumentException(Constantes.MensajePermisoNoValido);

                DateTime fechaConsulta = DateTime.Now;

                DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
                DateTime fechaIni = fechaPeriodo;
                DateTime fechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);

                //formato y cabecera
                var formato = this.servFormato.GetByIdMeFormato(idFormato);
                var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
                formato.Formatcols = cabecera.Cabcolumnas;
                formato.Formatrows = cabecera.Cabfilas;
                formato.Formatheaderrow = cabecera.Cabcampodef;

                //lista hoja
                var listaHojaPto1 = this.servFormato.GetListaPtos(fechaFin, 0, idEmpresa, idFormato, cabecera.Cabquery);
                List<MeMedicion96DTO> listaData = ObtenerDatos96(data, listaHojaPto1, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);

                //Realizar validación
                var empresa = base.servIEOD.GetByIdEmpresa(idEmpresa);
                List<LogErrorHOPvsMedidores> listaLog = (new ReporteMedidoresAppServicio()).ListarValidacionHopVsMedidores(fechaPeriodo, idEmpresa, listaData);
                List<ReporteHoraoperacionDTO> listaHOP = base.servHO.ListarReporteHOP(idEmpresa, fechaIni, fechaFin);

                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string ruta = (new ReporteMedidoresAppServicio()).GenerarExcelLogErroresHOP(listaLog, base.UserEmail, fechaConsulta, pathLogo, empresa.Emprnomb, mes, listaHOP);

                datos[0] = ruta;
                datos[1] = "LogErrores_HOP_" + empresa.Emprnomb + "_" + mes;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                datos[0] = "-1";
                datos[1] = "";
            }

            var jsonResult = Json(datos);
            return jsonResult;
        }

        [HttpPost]
        public JsonResult ExportarExcelReporteHOP(int idEmpresa, string mes)
        {
            string[] datos = new string[2];
            try
            {
                DateTime fechaPeriodo = EPDate.GetFechaIniPeriodo(5, mes, "", "", "");
                DateTime fechaIni = fechaPeriodo;
                DateTime fechaFin = fechaPeriodo.AddMonths(1).AddDays(-1);

                var empresa = base.servIEOD.GetByIdEmpresa(idEmpresa);
                var listaHOP = base.servHO.ListarReporteHOP(idEmpresa, fechaIni, fechaFin);

                string pathLogo = AppDomain.CurrentDomain.BaseDirectory + RutaDirectorio.PathLogo;
                string ruta = (new ReporteMedidoresAppServicio()).GenerarExcelReporteHOP(empresa.Emprnomb, mes, listaHOP, pathLogo);

                datos[0] = ruta;
                datos[1] = "Reporte HOP_" + empresa.Emprabrev + "_" + mes;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                datos[0] = "-1";
                datos[1] = "";
            }
            var jsonResult = Json(datos);
            return jsonResult;
        }

        /// <summary>
        /// Permite descargar el reporte de HOP
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarExcelReporteHOP()
        {
            string strArchivoTemporal = Request["archivo"];
            string strArchivoNombre = Request["nombre"];
            byte[] buffer = null;

            if (System.IO.File.Exists(strArchivoTemporal))
            {
                buffer = System.IO.File.ReadAllBytes(strArchivoTemporal);
                System.IO.File.Delete(strArchivoTemporal);
            }

            string strNombreArchivo = string.Format("{0}.xlsx", strArchivoNombre);

            return File(buffer, System.Net.Mime.MediaTypeNames.Application.Octet, strNombreArchivo);
        }

        #endregion

        #region métodos

        /// <summary>
        /// Método para generar gráfico en web y excel
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="fechaIni"></param>
        /// <param name="fechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="fuente1"></param>
        /// <param name="fuente2"></param>
        /// <param name="resolucion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private GraficoMedidorGeneracion GenerarGrafico(string[][] data, int idEmpresa, int idFormato, DateTime fechaIni, DateTime fechaFin, int periodo, int tipoDato, int idCentral, int fuente1, int fuente2, int usuario)
        {
            List<PuntoGraficoMedidorGeneracion> listaGraf = new List<PuntoGraficoMedidorGeneracion>();
            //variables
            int tipoinfocodi = -1;
            int tipoGrafico = 0;
            string nombreFuente1 = string.Empty, nombreFuente2 = string.Empty;
            string valorFuente1 = string.Empty, valorFuente2 = string.Empty;
            string tituloFuente1 = string.Empty, tituloFuente2 = string.Empty;
            string leyendaFuente1 = string.Empty, leyendaFuente2 = string.Empty;
            string descCentral = this.servFormato.GetByIdEqequipo(idCentral).Equinomb;

            //resolucion
            int resolucion = 0;
            switch (tipoDato)
            {
                case 2:
                    resolucion = ParametrosFormato.ResolucionMediaHora;
                    break;
                case 3:
                    resolucion = ParametrosFormato.ResolucionHora;
                    break;
                case 1:
                default:
                    resolucion = ParametrosFormato.ResolucionCuartoHora;
                    break;
            }
            //HP, desde 7:30pm a 11:15pm 
            int hIni = 0;
            int hFin = 0;
            int horaIniHP = 0;
            int horaFinHP = 0;
            switch (resolucion)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    hIni = 1;
                    hFin = 96;
                    horaIniHP = 78;
                    horaFinHP = 93;
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    hIni = 1;
                    hFin = 48;
                    horaIniHP = 24;
                    horaFinHP = 46;
                    break;
                case ParametrosFormato.ResolucionHora:
                    hIni = 1;
                    hFin = 24;
                    horaIniHP = 20; //7:30pm se redondea a 8pm
                    horaFinHP = 23;
                    break;
            }

            //Fuente1
            List<PuntoFuenteInformacion> listaFuente1 = new List<PuntoFuenteInformacion>();
            nombreFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Nombre;
            valorFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Valor;
            tituloFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Titulo;
            leyendaFuente1 = LsFuente1.Where(x => x.Codigo == fuente1).First().Leyenda;
            switch (fuente1)
            {
                case ConstantesMedidores.IdFuenteMedidores:
                    listaFuente1 = GetListaFuenteMedidores(data, idEmpresa, idFormato, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion, ref tipoinfocodi);
                    break;
            }

            //fuente2
            List<PuntoFuenteInformacion> listaFuente2 = new List<PuntoFuenteInformacion>();
            nombreFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Nombre;
            valorFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Valor;
            tituloFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Titulo;
            leyendaFuente2 = LsFuente2.Where(x => x.Codigo == fuente2).First().Leyenda;
            switch (fuente2)
            {
                case ConstantesMedidores.IdFuenteDespachoDiario:
                    tipoGrafico = ConstantesMedidores.GraficoIgualMedida;
                    listaFuente2 = GetListaFuenteDespachoDiario(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion, tipoinfocodi);
                    break;
                case ConstantesMedidores.IdFuenteRPF:
                    tipoGrafico = ConstantesMedidores.GraficoIgualMedida;
                    listaFuente2 = GetListaFuenteRPF(idEmpresa, fechaIni, fechaFin, periodo, tipoDato, idCentral, resolucion);
                    break;
            }

            //Generar data
            for (var day = fechaIni.Date; day.Date <= fechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaFuente1.Where(x => x.FechaFiltro.Date == day).ToList();
                var listaDataFuente2Dia = listaFuente2.Where(x => x.FechaFiltro.Date == day).ToList();

                for (int i = hIni; i <= hFin; i++)
                {
                    //fuente1
                    PuntoFuenteInformacion f1 = listaDataFuente1Dia.Where(x => x.NumeroTiempo == i).First();
                    decimal? valor1 = f1.ValorFuente;

                    decimal? valor2 = 0;
                    //fuente2
                    if (listaDataFuente2Dia.Count > 0) { 
                        PuntoFuenteInformacion f2 = listaDataFuente2Dia.Where(x => x.NumeroTiempo == i).First();
                        valor2 = f2.ValorFuente;
                    }

                    //fecha
                    DateTime fechaPunto = day;
                    switch (resolucion)
                    {
                        case ParametrosFormato.ResolucionCuartoHora:
                            int horas = (i) / 4;
                            int minutos = ((i) % 4) * 15;
                            fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                            break;
                        case ParametrosFormato.ResolucionMediaHora:
                            horas = (i) / 2;
                            minutos = ((i) % 2) * 30;
                            fechaPunto = day.AddHours(horas).AddMinutes(minutos);
                            break;
                        case ParametrosFormato.ResolucionHora:
                            fechaPunto = day.AddHours(i);
                            break;
                    }

                    PuntoGraficoMedidorGeneracion gr = new PuntoGraficoMedidorGeneracion();
                    gr.Fecha = fechaPunto;
                    gr.FechaString = gr.Fecha.ToString(ConstantesBase.FormatoFechaHora);
                    gr.ValorFuente1 = valor1;
                    gr.ValorFuente2 = valor2;

                    switch (periodo)
                    {
                        case ConstantesMedidores.PeriodoTodos:
                            listaGraf.Add(gr);
                            break;
                        case ConstantesMedidores.PeriodoHp:
                            if (i >= horaIniHP && i <= horaFinHP)
                            {
                                listaGraf.Add(gr);
                            }
                            break;
                        case ConstantesMedidores.PeriodoHfp:
                            if (i < horaIniHP || i > horaFinHP)
                            {
                                listaGraf.Add(gr);
                            }
                            break;
                    }
                }
            }

            //crear objeto grafico
            GraficoMedidorGeneracion g = new GraficoMedidorGeneracion();
            g.TipoUsuario = usuario;
            g.ListaPunto = listaGraf;
            g.TituloGrafico = "Gráfico comparativo " + nombreFuente1 + " vs " + nombreFuente2;
            g.TituloFuente1 = tituloFuente1;
            g.TituloFuente2 = tituloFuente2;
            g.LeyendaFuente1 = leyendaFuente1;
            g.LeyendaFuente2 = leyendaFuente2;
            g.Nombre = string.Format("{0}_{1:HHmmss}.xlsx", g.TituloGrafico, DateTime.Now);
            g.FechaInicio = fechaIni.ToString(ConstantesBase.FormatoFechaHora);
            g.FechaFin = fechaFin.ToString(ConstantesBase.FormatoFechaHora);
            g.ValorFuente1 = valorFuente1;
            g.ValorFuente2 = valorFuente2;
            g.DescPeriodo = LsPeriodo.Where(x => x.Codigo == periodo).First().Valor;
            g.DescCentral = descCentral;
            g.TipoGrafico = tipoGrafico;

            return g;
        }

        /// <summary>
        /// ListaFuenteMedidores
        /// </summary>
        /// <param name="data"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="idFormato"></param>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteMedidores(string[][] data, int idEmpresa, int idFormato, DateTime dfechaIni, DateTime dfechaFin, int periodo, int tipoDato, int idCentral, int resolucion, ref int tipoinfocodi)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            //formato y cabecera
            var formato = this.servFormato.GetByIdMeFormato(idFormato);
            var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formato.Cabcodi).FirstOrDefault();
            formato.Formatcols = cabecera.Cabcolumnas;
            formato.Formatrows = cabecera.Cabfilas;
            formato.Formatheaderrow = cabecera.Cabcampodef;

            //lista hoja
            var listaHojaPto1 = this.servFormato.GetListaPtos(dfechaFin, 0, idEmpresa, idFormato, cabecera.Cabquery);
            var hojaPtoFuente1 = listaHojaPto1.Where(x => x.Equipadre == idCentral).ToList();
            List<MeMedicion96DTO> listaDataFuente = new List<MeMedicion96DTO>();
            List<MeMedicion96DTO> listaData = ObtenerDatos96(data, listaHojaPto1, formato.Formatcheckblanco, (int)formato.Formatrows, formato.Lectcodi);

            foreach (var hoj in hojaPtoFuente1)
            {
                listaDataFuente.AddRange(listaData.Where(x => x.Ptomedicodi == hoj.Ptomedicodi).ToList());
            }

            if (listaDataFuente.Count > 0)
            {
                tipoinfocodi = listaDataFuente[0].Tipoinfocodi;
            }

            //data temporal
            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaDataFuente.Where(x => x.Medifecha == day).ToList();
                for (int i = 1; i <= 96; i++)
                {
                    int horas = i / 4;
                    int minutos = (i % 4) * 15;
                    decimal? valor1 = 0;
                    foreach (var m96 in listaDataFuente1Dia)
                    {
                        decimal? valorOrigen = (decimal?)m96.GetType().GetProperty("H" + (i).ToString()).GetValue(m96, null);
                        valor1 += valorOrigen.GetValueOrDefault(0);
                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                    gr.ValorFuente = valor1;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionCuartoHora, resolucion, tipoDato);

            return lista;
        }
        /// <summary>
        /// ListaFuenteDespachoDiario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <param name="tipoinfocodi"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteDespachoDiario(int idEmpresa, DateTime dfechaIni, DateTime dfechaFin, int periodo, int tipoDato, int idCentral, int resolucion, int tipoinfocodi)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            //formato y cabecera
            var formatoDespachoDiario = this.servFormato.GetByIdMeFormato(ConstantesMedidores.IdFormatoDespachoEjecutadoDiario);
            var cabecera = this.servFormato.GetListMeCabecera().Where(x => x.Cabcodi == formatoDespachoDiario.Cabcodi).FirstOrDefault();
            var listaHojaPto = this.servFormato.GetListaPtos(dfechaFin, 0, idEmpresa, formatoDespachoDiario.Formatcodi, cabecera.Cabquery);
            var hojaPtoFuente = listaHojaPto.Where(x => x.Equipadre == idCentral && x.Tipoinfocodi == tipoinfocodi).ToList();
            List<MeMedicion48DTO> listaDataFuente = new List<MeMedicion48DTO>();
            foreach (var hoja in hojaPtoFuente)
            {
                listaDataFuente.AddRange(this.servFormato.GetByCriteriaMeMedicion48(dfechaIni, dfechaFin, formatoDespachoDiario.Lectcodi, hoja.Tipoinfocodi, hoja.Ptomedicodi));
            }

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                var listaDataFuente1Dia = listaDataFuente.Where(x => x.Medifecha == day).ToList();
                for (int i = 1; i <= 48; i++)
                {
                    int horas = i / 4;
                    int minutos = (i % 2) * 30;
                    decimal? valor1 = 0;
                    foreach (var m48 in listaDataFuente1Dia)
                    {
                        decimal? valorOrigen = (decimal?)m48.GetType().GetProperty("H" + (i).ToString()).GetValue(m48, null);
                        valor1 += valorOrigen.GetValueOrDefault(0);
                    }

                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                    gr.FechaFiltro = day;
                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                    gr.ValorFuente = valor1;
                    gr.NumeroTiempo = i;

                    lista.Add(gr);
                }
            }

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, ParametrosFormato.ResolucionMediaHora, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// ListaFuenteRPF
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="periodo"></param>
        /// <param name="tipoDato"></param>
        /// <param name="idCentral"></param>
        /// <param name="resolucion"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFuenteRPF(int idEmpresa, DateTime dfechaIni, DateTime dfechaFin, int periodo, int tipoDato, int idCentral, int resolucion)
        {
            List<PuntoFuenteInformacion> lista = new List<PuntoFuenteInformacion>();

            //obtencion de datos
            List<int> puntosRpf = this.servFormato.ListPuntosRPF(idEmpresa, idCentral, dfechaIni);
            string rpf = string.Join<int>(Constantes.CaracterComa.ToString(), puntosRpf);

            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
            {
                
                List<ServicioCloud.Medicion> listRPF = new List<ServicioCloud.Medicion>();
                if (rpf != string.Empty)
                {
                    //listRPF = (new RpfAppServicio()).ObtenerDatosComparacionRangoResolucion(day, rpf, resolucion).ToList();
                    //listRPF = (new ServicioCloud.ServicioCloudClient()).ObtenerDatosComparacionRangoResolucion(day, rpf, resolucion).ToList();
                    listRPF = (new ServicioCloud.ServicioCloudClient()).DescargarEnvio(puntosRpf.ToArray(), day).ToList();
                }

                foreach (int punto in puntosRpf)
                {
                    //var lista = listRPF.Where(x => x.PTOMEDICODI == punto && x.TIPOINFOCODI == 1).OrderBy(x => x.FECHAHORA).ToList();
                    List<ServicioCloud.Medicion> list = listRPF.Where(x => x.PTOMEDICODI == punto && x.TIPOINFOCODI == ConstantesAppServicio.TipoinfocodiMW).OrderBy(x => x.FECHAHORA).ToList();

                    if(list.Count > 0)
                    {

                    

                        switch (resolucion)
                        {
                            case ParametrosFormato.ResolucionCuartoHora:
                                int m_ini = 0;
                                int m_fin = 0;
                                for (int i = 1; i <= 96; i++)
                                {
                                    int horas = i / 4;
                                    int minutos = (i % 4) * 15;
                                    decimal? valor1 = 0;
                                    m_ini = m_fin;
                                    m_fin = m_ini + 15;

                                    //if (listRPF.Count == 96) valor1 = listRPF[i - 1].H0;
                                    for (int m = m_ini; m < m_fin; m++)
                                    {
                                        for (int k = 0; k <= 59; k++)
                                        {
                                            valor1 = valor1 + (decimal)list[m].GetType().GetProperty("H" + k.ToString()).GetValue(list[m], null);
                                        }

                                    }

                                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                    gr.FechaFiltro = day;
                                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                                    gr.ValorFuente = valor1 / 900;
                                    gr.NumeroTiempo = i;

                                    lista.Add(gr);
                                }
                                break;
                            case ParametrosFormato.ResolucionMediaHora:
                                m_ini = 0;
                                m_fin = 0;
                                for (int i = 1; i <= 48; i++)
                                {
                                    int horas = i / 2;
                                    int minutos = (i % 2) * 30;
                                    decimal? valor1 = 0;
                                    m_ini = m_fin;
                                    m_fin = m_ini + 30;

                                    //if (listRPF.Count == 48) valor1 = listRPF[i - 1].H0;
                                    for (int m = m_ini; m < m_fin; m++)
                                    {
                                        for (int k = 0; k <= 59; k++)
                                        {
                                            valor1 = valor1 + (decimal)list[m].GetType().GetProperty("H" + k.ToString()).GetValue(list[m], null);
                                        }

                                    }

                                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                    gr.FechaFiltro = day;
                                    gr.Fecha = day.AddHours(horas).AddMinutes(minutos);
                                    gr.ValorFuente = valor1/1800;
                                    gr.NumeroTiempo = i;

                                    lista.Add(gr);
                                }

                                break;
                            case ParametrosFormato.ResolucionHora:
                                m_ini = 0;
                                m_fin = 0;
                                for (int i = 1; i <= 24; i++)
                                {
                                    int horas = i;
                                    decimal? valor1 = 0;
                                    m_ini = m_fin;
                                    m_fin = m_ini + 60;

                                    //if (listRPF.Count == 24) valor1 = listRPF[i - 1].H0;
                                    for (int m = m_ini; m < m_fin; m++)
                                    {
                                        for (int k = 0; k <= 59; k++)
                                        {
                                            valor1 = valor1 + (decimal)list[m].GetType().GetProperty("H" + k.ToString()).GetValue(list[m], null);
                                        }

                                    }

                                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                    gr.FechaFiltro = day;
                                    gr.Fecha = day.AddHours(horas);
                                    gr.ValorFuente = valor1/3600;
                                    gr.NumeroTiempo = i;

                                    lista.Add(gr);
                                }
                                break;
                        }
                    }
                }
            }

            lista = lista.GroupBy(x => new { x.Fecha, x.FechaFiltro, x.NumeroTiempo }).Select(y => new PuntoFuenteInformacion
            {
                FechaFiltro = y.Key.FechaFiltro,
                Fecha = y.Key.Fecha,
                NumeroTiempo = y.Key.NumeroTiempo,
                ValorFuente = y.Sum(h => h.ValorFuente)
                }).ToList();

            //obtener data segun resolucion de destino
            lista = GetListaFinalPorResolucion(dfechaIni, dfechaFin, lista, resolucion, resolucion, tipoDato);

            return lista;
        }

        /// <summary>
        /// Lista FinalPorResolucion
        /// </summary>
        /// <param name="dfechaIni"></param>
        /// <param name="dfechaFin"></param>
        /// <param name="lista"></param>
        /// <param name="resolucionOrigen"></param>
        /// <param name="resolucionDestino"></param>
        /// <param name="tipoDato"></param>
        /// <returns></returns>
        private List<PuntoFuenteInformacion> GetListaFinalPorResolucion(DateTime dfechaIni, DateTime dfechaFin, List<PuntoFuenteInformacion> lista, int resolucionOrigen, int resolucionDestino, int tipoDato)
        {
            List<PuntoFuenteInformacion> listaFinal = new List<PuntoFuenteInformacion>();
            int equivalencia = 0;
            if (tipoDato != resolucionDestino)
            {
                equivalencia = ConstantesMedidores.DatoPromedio;
            }

            //datos para el grafico
            switch (resolucionDestino)
            {
                case ParametrosFormato.ResolucionCuartoHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 96
                            listaFinal = lista;
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 96

                            for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                            {
                                var listaDay = lista.Where(x => x.FechaFiltro.Date == day);

                                for (int i = 1; i <= 96; i += 2)
                                {
                                    var numeroTiempo = i / 2 + 1;
                                    var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                    PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                    gr.FechaFiltro = reg.FechaFiltro;
                                    gr.Fecha = reg.Fecha;
                                    gr.ValorFuente = reg.ValorFuente;
                                    gr.NumeroTiempo = i;
                                    listaFinal.Add(gr);

                                    PuntoFuenteInformacion gr2 = new PuntoFuenteInformacion();
                                    gr2.FechaFiltro = reg.FechaFiltro;
                                    gr2.Fecha = reg.Fecha;
                                    gr2.ValorFuente = reg.ValorFuente;
                                    gr2.NumeroTiempo = i + 1;
                                    listaFinal.Add(gr2);
                                }
                            }
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionMediaHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora: //De 96 a 48
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 48; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 2 + 1;
                                            var numeroTiempoFin = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 48
                            listaFinal = lista;
                            break;
                        case ParametrosFormato.ResolucionHora:
                            //TODO
                            break;
                    }
                    break;
                case ParametrosFormato.ResolucionHora:
                    switch (resolucionOrigen)
                    {
                        case ParametrosFormato.ResolucionCuartoHora://De 96 a 24
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempo = i * 4;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = reg.ValorFuente;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 4 + 1;
                                            var numeroTiempoFin = i * 4;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionMediaHora: //De 48 a 24
                            switch (equivalencia)
                            {
                                case ConstantesMedidores.DatoHorario:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempo = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempo).FirstOrDefault();

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = reg.ValorFuente;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }

                                    break;
                                case ConstantesMedidores.DatoPromedio:
                                    for (var day = dfechaIni.Date; day.Date <= dfechaFin.Date; day = day.AddDays(1))
                                    {
                                        var listaDay = lista.Where(x => x.FechaFiltro.Date == day);
                                        for (int i = 1; i <= 24; i++)
                                        {
                                            var numeroTiempoIni = (i - 1) * 2 + 1;
                                            var numeroTiempoFin = i * 2;
                                            var reg = listaDay.Where(x => x.NumeroTiempo == numeroTiempoFin).FirstOrDefault();
                                            var listaProm = listaDay.Where(x => x.NumeroTiempo >= numeroTiempoIni && x.NumeroTiempo <= numeroTiempoFin).ToList();
                                            decimal? resultado = 0;
                                            if (listaProm.Count > 0)
                                            {
                                                foreach (var t in listaProm)
                                                {
                                                    resultado += t.ValorFuente.GetValueOrDefault(0);
                                                }
                                                resultado = resultado / listaProm.Count;
                                            }

                                            PuntoFuenteInformacion gr = new PuntoFuenteInformacion();
                                            gr.FechaFiltro = reg.FechaFiltro;
                                            gr.Fecha = reg.Fecha;
                                            gr.ValorFuente = resultado;
                                            gr.NumeroTiempo = i;
                                            listaFinal.Add(gr);
                                        }
                                    }
                                    break;
                            }
                            break;
                        case ParametrosFormato.ResolucionHora: //De 24 a 24
                            listaFinal = lista;
                            break;
                    }
                    break;
            }

            return listaFinal;
        }

        /// <summary>
        /// Generar Excel Grafico
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private string GenerarExcelGrafico(GraficoMedidorGeneracion g)
        {
            string rutaArchivoExcel = string.Empty;
            //generacion de excel
            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = xlPackage.Workbook.Worksheets.Add("Gráfico");
                ws.TabColor = Color.Orange;

                #region cabecera excel
                //Filtros
                int row = 7;
                int rowIniCentral = row;
                int rowIniFechaDesde = rowIniCentral + 1;
                int rowIniFechaHasta = rowIniFechaDesde + 1;
                int rowIniPeriodo = rowIniFechaHasta + 1;
                int colIniFechaDesde = 1;
                int colIniFechaHasta = colIniFechaDesde;
                int colIniPeriodo = colIniFechaHasta;

                ws.Cells[rowIniCentral, colIniFechaDesde].Value = "Central";
                ws.Cells[rowIniCentral, colIniFechaDesde + 1].Value = g.DescCentral;
                ws.Cells[rowIniFechaDesde, colIniFechaDesde].Value = "Fecha Desde";
                ws.Cells[rowIniFechaDesde, colIniFechaDesde + 1].Value = g.FechaInicio;
                ws.Cells[rowIniFechaHasta, colIniFechaHasta].Value = "Fecha Hasta";
                ws.Cells[rowIniFechaHasta, colIniFechaHasta + 1].Value = g.FechaFin;
                ws.Cells[rowIniPeriodo, colIniPeriodo].Value = "Período";
                ws.Cells[rowIniPeriodo, colIniPeriodo + 1].Value = g.DescPeriodo;

                using (var range = ws.Cells[row, colIniFechaDesde, rowIniPeriodo, colIniPeriodo + 1])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }

                //columnas
                row = rowIniPeriodo + 2;
                var rowIniCabecera = row;
                var colIniFuente1 = 2;
                var colIniFuente2 = colIniFuente1 + 1;

                ws.Cells[rowIniCabecera, colIniFuente1].Value = g.ValorFuente1;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[rowIniCabecera, colIniFuente2].Value = g.ValorFuente2;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells[rowIniCabecera, colIniFuente2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[rowIniCabecera, colIniFuente2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                using (ExcelRange r = ws.Cells[rowIniCabecera, colIniFuente1, rowIniCabecera, colIniFuente2])
                {
                    r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#2980B9"));
                    r.Style.Font.Color.SetColor(Color.White);
                    r.Style.WrapText = true;
                }
                #endregion

                #region cuerpo excel
                row += 1;
                var rowIniFecha = row;
                var rowFinFecha = rowIniFecha + g.ListaPunto.Count - 1;
                var colIniFecha = 1;

                foreach (var punto in g.ListaPunto)
                {
                    //Fecha
                    ws.Cells[row, colIniFecha].Value = punto.FechaString;

                    //Fuente1
                    ws.Cells[row, colIniFuente1].Value = punto.ValorFuente1;

                    //Fuente2
                    ws.Cells[row, colIniFuente2].Value = punto.ValorFuente2;

                    row++;
                }

                using (ExcelRange range = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFuente2])
                {
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Color.SetColor(Color.Black);
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Color.SetColor(Color.Black);
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Top.Color.SetColor(Color.Black);
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Color.SetColor(Color.Black);
                }


                //ancho de columnas
                ws.Column(colIniFecha).Width = 16;
                ws.Column(colIniFuente1).Width = 20;
                ws.Column(colIniFuente2).Width = 20;

                #endregion

                #region chart
                var chart1 = ws.Drawings.AddChart("Chart1", eChartType.LineMarkers) as ExcelLineChart;
                chart1.SetPosition(5, 0, colIniFuente2 + 3, 0);
                chart1.SetSize(1200, 600);
                chart1.Title.Text = g.TituloGrafico;
                chart1.DataLabel.ShowLeaderLines = false;
                chart1.YAxis.Title.Text = g.TituloFuente1;
                chart1.Legend.Position = eLegendPosition.Bottom;

                var ran0 = ws.Cells[rowIniFecha, colIniFecha, rowFinFecha, colIniFecha];
                var ran1 = ws.Cells[rowIniFecha, colIniFuente1, rowFinFecha, colIniFuente1];
                var ran2 = ws.Cells[rowIniFecha, colIniFuente2, rowFinFecha, colIniFuente2];

                //Fuente1
                var serie = (ExcelChartSerie)chart1.Series.Add(ran1, ran0);
                serie.Header = g.LeyendaFuente1;

                switch (g.TipoUsuario)
                {
                    case ConstantesMedidores.UsuarioCOES:
                        var chart2 = chart1.PlotArea.ChartTypes.Add(eChartType.Line);

                        //Fuente2
                        var serie2 = (ExcelChartSerie)chart2.Series.Add(ran2, ran0);
                        serie2.Header = g.LeyendaFuente2;
                        chart2.UseSecondaryAxis = true;
                        chart2.YAxis.Title.Text = g.TituloFuente2;

                        break;
                    case ConstantesMedidores.UsuarioAgentes:
                        var serie3 = (ExcelChartSerie)chart1.Series.Add(ran2, ran0);
                        serie3.Header = g.LeyendaFuente2;

                        break;
                }
                #endregion

                #region Logo
                HttpWebRequest request = (HttpWebRequest)System.Net.HttpWebRequest.Create(ConfigurationManager.AppSettings["LogoCoes"].ToString());
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.Drawing.Image img = System.Drawing.Image.FromStream(response.GetResponseStream());
                ExcelPicture picture = ws.Drawings.AddPicture("Imagen", img);
                picture.From.Column = 0;
                picture.From.Row = 1;
                picture.To.Column = 1;
                picture.To.Row = 2;
                picture.SetSize(120, 60);
                #endregion

                rutaArchivoExcel = System.IO.Path.GetTempFileName();
                xlPackage.SaveAs(new FileInfo(rutaArchivoExcel));
            }
            return rutaArchivoExcel;
        }

        #endregion
    }
}
