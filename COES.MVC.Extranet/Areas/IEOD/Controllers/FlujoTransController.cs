using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.IEOD;
using log4net;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class FlujoTransController : FormatoController
    {
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(FlujoTransController));
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

        #region METODOS FLUJO DE LOS TRANSMISORES

        /// <summary>
        /// Index de Flujo de los transmisores
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            FormatoModel model = new FormatoModel();
            base.IndexFormato(model, ConstantesHard.IdFormatoFlujoTrans);
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
        /// Metodo llamado desde cliente web para consultar el formato excel web de flujo de los transmisores
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, int verUltimoEnvio)
        {
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(ConstantesHard.IdFormatoFlujoTrans, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelFlujoTrans(idEmpresa, idEnvio, fecha, verUltimoEnvio);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }
        }

        /// <summary>
        /// Devuelve el model con informacion del flujo de los transmisores
        /// </summary>sic
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public FormatoModel BuildHojaExcelFlujoTrans(int idEmpresa, int idEnvio, string fecha, int verUltimoEnvio)
        {
            FormatoModel model = new FormatoModel();
            model.UtilizaScada = true;
            model.UtilizaFlujoTrans = true;
            model.ValidaMantenimiento = true;

            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, ConstantesHard.IdFormatoFlujoTrans, verUltimoEnvio);

            var listaFam = model.ListaHojaPto
                .GroupBy(x => new { x.Famcodi, x.Famabrev })
                .Select(grp => new EqFamiliaDTO { Famcodi = grp.Key.Famcodi, Famabrev = grp.Key.Famabrev })
                .OrderBy(x => x.Famabrev).ToList();
            model.ListaFamilia = listaFam;

            model.ListaAreaOperativa = model.ListaHojaPto.Select(y => y.AreaOperativa).Distinct().OrderBy(x => x).ToList();
            model.ListaFamilia = listaFam;

            var listaSub = model.ListaHojaPto
                .GroupBy(x => new { x.Areacodi, x.Areanomb })
                .Select(grp => new EqAreaDTO { Areacodi = grp.Key.Areacodi, Areanomb = grp.Key.Areanomb }).OrderBy(x => x.Areanomb).ToList();
            model.ListaSubestacion = listaSub;
            model.ListaCausaJustificacion = base.servFormato.GetListaJustificacion();

            return model;
        }

        /// <summary>
        /// Graba los datos enviados por el agente del formato flujo de los transmisores
        /// </summary>
        /// <param name="dataExcel"></param>
        /// <param name="idFormato"></param>
        /// <param name="idEmpresa"></param>
        /// <param name="fecha"></param>
        /// <param name="semana"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GrabarExcelWeb(string[][] data, int idEmpresa, string fecha)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.Handson.ListaExcelData = data;
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.IdFormato = ConstantesHard.IdFormatoFlujoTrans;

            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }

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
                FormatoModel model = BuildHojaExcelFlujoTrans(idEmpresa, idEnvio, dia, ConstantesFormato.NoVerUltimoEnvio);
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
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
