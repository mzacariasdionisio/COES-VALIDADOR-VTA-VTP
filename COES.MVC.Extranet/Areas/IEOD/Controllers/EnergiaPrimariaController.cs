using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.IEOD;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace COES.MVC.Extranet.Areas.IEOD.Controllers
{
    public class EnergiaPrimariaController : FormatoController
    {
        #region Declaración de variables

        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnergiaPrimariaController));
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

        #region METODOS ENERGIA PRIMARIA

        public ActionResult Index()
        {
            if (!base.IsValidSesion) return base.RedirectToLogin();
            if (this.IdModulo == null) return base.RedirectToHomeDefault();

            var listaFormato = this.servFormato.ListarFormatosEnergiaPrimaria();
            var listaHoja = this.servFormato.ListarHojaEnergiaPrimaria();
            List<MeHojaDTO> listaHojaEnergPrimaria = new List<MeHojaDTO>();
            foreach (var fmt in listaFormato)
            {
                var listaEmp = base.ListarEmpresaByFormatoYUsuario(Acciones.AccesoEmpresa, User.Identity.Name, fmt.Formatcodi);
                if (listaEmp.Where(x => x.Emprcodi > 0).Count() > 0)
                {
                    listaHojaEnergPrimaria.AddRange(listaHoja.Where(x => x.Formatcodi == fmt.Formatcodi).ToList());
                }
            }

            FormatoModel model = new FormatoModel();
            model.ListaMeHoja = listaHojaEnergPrimaria;

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            StringBuilder json = new StringBuilder();
            serializer.Serialize(model.ListaMeHoja, json);
            model.ListaMeHojaJson = json.ToString();

            return View(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial para cada hoja
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            FormatoModel modelHoja = new FormatoModel();
            base.IndexFormato(modelHoja, idFormato);

            MeHojaDTO hoja = base.servFormato.GetByIdMeHoja(idHoja);

            modelHoja.IdHoja = hoja.Hojacodi;
            modelHoja.NombreHoja = hoja.Hojanombre;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Energia Primaria
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, int idFormato, int verUltimoEnvio)
        {
            base.ValidarSesionUsuario();
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelEnergiaPrimaria(idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);
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
        public FormatoModel BuildHojaExcelEnergiaPrimaria(int idEmpresa, int idEnvio, string fecha, int idFormato, int verUltimoEnvio)
        {
            FormatoModel model = new FormatoModel();
            model.UtilizaFiltroCentral = true;
            model.UtilizaScada = true;

            int idFormatoHistorico = idFormato;
            ConfigurarFEnergPrimariaSegunFecha(model, fecha, idFormato, out idFormatoHistorico);
            model.IdFormato = idFormato;
            model.IdFormatoNuevo = idFormatoHistorico;

            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            model.IdFormato = idFormato;
            model.TituloGrafico = ConstantesIEOD.TituloGraficoEnergiaPrimaria;

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

            int idFormatoHistorico = idFormato;
            ConfigurarFEnergPrimariaSegunFecha(model, fecha, idFormato, out idFormatoHistorico);

            model.IdFormato = idFormato;
            model.IdFormatoNuevo = idFormatoHistorico;
            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }

        /// <summary>
        /// Permite generar el formato en formato excel de Despacho diario
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(string[][] data, int idEmpresa, string dia, int idFormato, int idEnvio)
        {
            string ruta = string.Empty;
            try
            {
                idEnvio = idEnvio > 0 ? idEnvio : -1;
                this.MatrizExcel = data;
                FormatoModel model = BuildHojaExcelEnergiaPrimaria(idEmpresa, idEnvio, dia, idFormato, ConstantesFormato.NoVerUltimoEnvio);
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
        /// Configuracion del Formato de Energia Primaria, segun la fecha de corte tomará la data historico o la nueva
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fecha"></param>
        /// <param name="idFormato"></param>
        /// <param name="idFormatoHistorico"></param>
        private void ConfigurarFEnergPrimariaSegunFecha(FormatoModel model, string fecha, int idFormato, out int idFormatoHistorico)
        {
            DateTime dfecha = DateTime.ParseExact(fecha, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            string fechaEnergSolar = System.Configuration.ConfigurationManager.AppSettings[ConstantesIEOD.KeyFechaIniProcesoFEnergSolar];
            DateTime dfechaEnergSolar = DateTime.ParseExact(fechaEnergSolar, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            idFormatoHistorico = idFormato;
            if (ConstantesHard.IdFormatoEnergiaPrimaria != idFormato)
            {
                if (dfecha.Date < dfechaEnergSolar)
                {
                    model.IdFormatoNuevo = idFormato;
                    idFormatoHistorico = ConstantesHard.IdFormatoEnergiaPrimaria;
                }
            }
        }

        #endregion
    }
}
