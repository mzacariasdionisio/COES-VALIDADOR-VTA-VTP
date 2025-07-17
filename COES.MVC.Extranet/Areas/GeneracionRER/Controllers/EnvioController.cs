using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.GeneracionRER.Controllers
{
    public class EnvioController : FormatoController
    {
        #region Declaracion de variables de Sesión

        private static readonly ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().Name);
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

        /// <summary>
        /// Excepciones ocurridas en el controlador
        /// </summary>
        /// <param name="filterContext"></param>
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

        #endregion

        // GET: GeneracionRER/Envio
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            //if (this.IdModulo == null) return base.RedirectToHomeDefault();

            var listaFmt = this.servFormato.ListarFormatosGeneracionRER();

            FormatoModel model = this.GenerarValoresDefecto(listaFmt.First().Formatcodi);
            model.ListaFormato = listaFmt;
            model.IdFormato = listaFmt.First().Formatcodi;

            return View(model);
        }

        /// <summary>
        /// Hojas segun el formato
        /// </summary>
        /// <param name="formatcodi"></param>
        /// <returns></returns>
        public JsonResult CargarFormato(int formatcodi)
        {
            FormatoModel model = this.GenerarValoresDefecto(formatcodi);
            return Json(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            var formato = base.servFormato.ListMeFormatos().Find(x => x.Formatcodi == idFormato);

            var modelHoja = this.GenerarValoresDefecto(idFormato);

            modelHoja.IdAplicativo = ConstantesFormatoMedicion.AplicativoProgRER;
            modelHoja.IdHoja = idHoja;
            modelHoja.Titulo = formato.Formatnombre;
            modelHoja.Periodo = formato.Formatperiodo ?? 0;
            modelHoja.IdFormato = idFormato;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Despacho diario
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, int verUltimoEnvio)
        {
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelGeneracionRER(idEmpresa, idEnvio, fecha, semana, mes, idFormato, verUltimoEnvio);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve el model con informacion de Despacho diario
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelGeneracionRER(int idEmpresa, int idEnvio, string fecha, string semana, string mes, int idFormato, int verUltimoEnvio)
        {
            FormatoModel model = new FormatoModel();
            model.ValidaHorasOperacion = false;
            model.ValidaMantenimiento = false;
            model.ValidaRestricOperativa = false;
            model.ValidaEventos = false;
            model.UtilizaScada = false;
            model.UtilizaFiltroCentral = true;
            model.ValidaTiempoReal = false;
            model.MostrarDataBDSinEnvioPrevio = true;

            model.Semana = semana;
            model.Mes = mes;

            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            model.TituloGrafico = ConstantesIEOD.TituloGraficoGeneracionRER;

            return model;
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato Despacho diario
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaExcelData = data;
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.Semana = semana;
            model.Mes = mes;
            model.IdFormato = idFormato;

            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }
        /// Mejoras IEOD -- Modificó Función
        /// <summary>
        /// Permite generar el formato en formato excel de Despacho diario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(string[][] data, int idEmpresa, string fecha, string semana, string mes, int idFormato, int idEnvio)
        {
            string ruta = string.Empty;
            try
            {
                idEnvio = idEnvio > 0 ? idEnvio : -1;
                this.MatrizExcel = data;
                FormatoModel model = BuildHojaExcelGeneracionRER(idEmpresa, idEnvio, fecha, semana, mes, idFormato, ConstantesFormato.NoVerUltimoEnvio);
                model.IdEnvio = idEnvio;
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
        /// Generar valor por defecto
        /// </summary>
        /// <param name="model"></param>
        private FormatoModel GenerarValoresDefecto(int formatcodi)
        {
            FormatoModel model = new FormatoModel();
            //lista de empresas
            base.IndexFormato(model, formatcodi);

            //fechas Formato Diario
            DateTime fechaActual = DateTime.Now.AddDays(1);

            var formato = base.servFormato.ListMeFormatos().Find(x => x.Formatcodi == formatcodi);
            model.Formato = formato;
            model.Formato.FechaProceso = fechaActual.Date;
            FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            if ((DateTime.Now >= formato.FechaPlazoIni) && (DateTime.Now <= formato.FechaPlazo)) { }
            else
            {
                fechaActual = fechaActual.AddDays(1);
            }

            model.Fecha = fechaActual.ToString(ConstantesAppServicio.FormatoFecha);
            model.Dia = fechaActual.ToString(Constantes.FormatoFecha);

            //fechas Formato Semanal            
            model.Formato.FechaProceso = EPDate.f_fechainiciosemana(fechaActual.AddDays(7));
            FormatoMedicionAppServicio.GetSizeFormato(model.Formato);
            if ((DateTime.Now >= formato.FechaPlazoIni) && (DateTime.Now <= formato.FechaPlazo))
            {
                fechaActual = fechaActual.AddDays(7);
            }
            else
            {
                fechaActual = fechaActual.AddDays(14);
            }

            Tuple<int, int> tupla = EPDate.f_numerosemana_y_anho(fechaActual);

            model.NroSemana = tupla.Item1;

            List<GenericoDTO> entitys = new List<GenericoDTO>();

            int nsemanas = EPDate.TotalSemanasEnAnho(tupla.Item2, FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + tupla.Item2;
                reg.String2 = i == tupla.Item1 ? "selected" : "";
                entitys.Add(reg);

            }
            model.Anho = tupla.Item2.ToString();
            model.ListaSemanas2 = entitys;

            //Hojas
            model.ListaMeHoja = base.servFormato.GetByCriteriaMeHoja(formatcodi);
            model.ListaMeHojaPadre = this.servFormato.ListHojaPadre(formatcodi);

            return model;
        }

        /// <summary>
        /// Lista de Semana por Año
        /// </summary>
        /// <param name="idAnho"></param>
        /// <returns></returns>
        public PartialViewResult CargarSemanas(string idAnho)
        {
            FormatoModel model = new FormatoModel();
            List<GenericoDTO> entitys = new List<GenericoDTO>();
            if (idAnho == "0")
            {
                idAnho = DateTime.Now.Year.ToString();
            }
            DateTime dfecha = new DateTime(Int32.Parse(idAnho), 12, 31);
            int nsemanas = EPDate.TotalSemanasEnAnho(Int32.Parse(idAnho), FirstDayOfWeek.Saturday);

            for (int i = 1; i <= nsemanas; i++)
            {
                GenericoDTO reg = new GenericoDTO();
                reg.Entero1 = i;
                reg.String1 = "Sem" + i + "-" + idAnho;
                entitys.Add(reg);

            }
            model.ListaSemanas2 = entitys;
            return PartialView(model);
        }

    }
}