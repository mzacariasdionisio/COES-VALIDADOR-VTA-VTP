using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class TensionGenerController : FormatoController
    {
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(TensionGenerController));
        private static string NameController = MethodBase.GetCurrentMethod().DeclaringType.Name;

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

        #region METODOS TENSION DE GENERACION

        /// <summary>
        /// Index de Tension de generación
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoTension);
            return View(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            var formato = base.servFormato.ListMeFormatos().Find(x => x.Formatcodi == idFormato);

            var modelHoja = new FormatoModel();
            base.IndexFormato(modelHoja, idFormato);

            modelHoja.IdHoja = idHoja;
            modelHoja.Titulo = formato.Formatnombre;
            modelHoja.IdFormato = idFormato;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Tensión de generación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, int verUltimoEnvio)
        {
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(ConstantesHard.IdFormatoTension, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelTension(idEmpresa, idEnvio, fecha, verUltimoEnvio);
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
        public FormatoModel BuildHojaExcelTension(int idEmpresa, int idEnvio, string fecha, int verUltimoEnvio)
        {
            FormatoModel model = new FormatoModel();
            model.ValidaHorasOperacion = true;
            model.ValidaMantenimiento = true;
            model.ValidaRestricOperativa = true;
            model.UtilizaScada = true;
            model.UtilizaFiltroCentral = true;

            base.BuildHojaExcel(model, idEmpresa, idEnvio, fecha, ConstantesHard.IdFormatoTension, verUltimoEnvio);

            model.TituloGrafico = ConstantesIEOD.TituloGraficoTensionGener;

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
        public JsonResult GrabarExcelWeb(string[][] data, int idEmpresa, string fecha, int idFormato)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaExcelData = data;
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.IdFormato = idFormato;

            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }
        /// Mejoras IEOD -- Modifico Funcion
        /// <summary>
        /// Permite generar el formato en formato excel de Tensión de generación
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(string[][] data, int idEmpresa, string dia, int idEnvio)
        {
            string ruta = string.Empty;
            try
            {
                idEnvio = idEnvio > 0 ? idEnvio : -1;
                this.MatrizExcel = data;
                FormatoModel model = BuildHojaExcelTension(idEmpresa, idEnvio, dia, ConstantesFormato.NoVerUltimoEnvio);
                model.NombreArchivoExcel = ConstantesIEOD.IdccG;
                model.IdEnvio = idEnvio;
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
                ruta += "," + model.NombreArchivoExcel;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }

        #endregion

    }
}
