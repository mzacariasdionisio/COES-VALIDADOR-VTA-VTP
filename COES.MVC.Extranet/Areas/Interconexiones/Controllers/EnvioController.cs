using COES.Dominio.DTO.Sic;
using COES.MVC.Extranet.Areas.Interconexiones.Helper;
using COES.MVC.Extranet.Areas.Interconexiones.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.Models;
using COES.Servicios.Aplicacion.FormatoMedicion;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.Interconexiones.Controllers
{
    public class EnvioController : FormatoController
    {
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(EnvioController));
        private static string NameController = "EnvioController";

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
        }

        //
        // GET: /Interconexiones/Envio/
        public ActionResult Index()
        {
            base.ValidarSesionUsuario();

            InterconexionesModel model = new InterconexionesModel();
            base.IndexFormato(model, ConstantesInterconexiones.IdFormato);

            model.ListaEmpresas = model.ListaEmpresas.Where(x => x.Emprcodi == ConstantesInterconexiones.IdEmpresaInterconexion).ToList();
            if (model.ListaEmpresas.Count == 1)
            {
                model.IdEmpresa = model.ListaEmpresas[0].Emprcodi;
            }

            model.Dia = DateTime.Now.AddDays(-1).ToString(Constantes.FormatoFecha);

            //Hoja Medidores
            List<MeHojaDTO> listaHoja = base.servFormato.GetByCriteriaMeHoja(ConstantesInterconexiones.IdFormato);
            MeHojaDTO hojaPadre = listaHoja.Find(x => x.Hojacodi == ConstantesInterconexiones.IdHoja);
            MeHojaDTO hojaMedidorPrincipal = listaHoja.Find(x => x.Hojacodi == ConstantesInterconexiones.IdHojaMedidorPrincipal);
            MeHojaDTO hojaMedidorSecundario = listaHoja.Find(x => x.Hojacodi == ConstantesInterconexiones.IdHojaMedidorSecundario);

            model.IdHoja = hojaPadre.Hojacodi;
            model.TituloMedidorPrincipal = hojaMedidorPrincipal.Hojanombre;
            model.TituloMedidorSecundario = hojaMedidorSecundario.Hojanombre;
            model.HojaMedidorPrincipal = hojaMedidorPrincipal.Hojacodi;
            model.HojaMedidorSecundario = hojaMedidorSecundario.Hojacodi;

            return View(model);
        }

        /// <summary>
        /// Devuelve Vista Parcial para cada hoja
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ViewHojaCargaDatos(int idHoja, int idFormato)
        {
            InterconexionesModel modelHoja = new InterconexionesModel();
            MeHojaDTO hoja = base.servFormato.GetByIdMeHoja(idHoja);

            modelHoja.IdHoja = hoja.Hojacodi;
            modelHoja.NombreHoja = hoja.Hojanombre;

            return PartialView(modelHoja);
        }

        /// <summary>
        /// Metodo llamado desde cliente web para consultar el formato excel web de Medidor de Generación
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idEnvio"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MostrarGrilla(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato, string listaHoja, int verUltimoEnvio, int? idHoja = 0)
        {
            base.ValidarSesionUsuario();
            List<MeHojaptomedDTO> entitys = servFormato.ObtenerPtosXFormato(idFormato, idEmpresa);
            if (entitys.Count > 0)
            {
                FormatoModel jsModel = BuildHojaExcelInterconexion(idEmpresa, idEnvio, fecha, mes, idFormato, false, verUltimoEnvio, idHoja.Value);
                return Json(jsModel);
            }
            else
            {
                return Json(-1);
            }
        }

        public FormatoModel BuildHojaExcelInterconexion(int idEmpresa, int idEnvio, string fecha, string mes, int idFormato, bool opGrabar, int verUltimoEnvio, int idHoja)
        {
            InterconexionesModel model = new InterconexionesModel();
            model.Mes = mes;
            model.UtilizaHoja = true;
            model.OpGrabar = opGrabar;
            model.IdHojaPadre = idHoja;

            BuildHojaExcel(model, idEmpresa, idEnvio, fecha, idFormato, verUltimoEnvio);

            return model;
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
        public JsonResult GrabarExcelWeb(int idEmpresa, string fecha, string semana, string mes, int idFormato, List<int> listaHoja, List<string[][]> listaData, int? idHoja = 0)
        {
            FormatoModel model = new FormatoModel();
            model.Handson = new HandsonModel();
            model.IdEmpresa = idEmpresa;
            model.Fecha = fecha;
            model.Semana = semana;
            model.Mes = mes;
            model.IdHojaPadre = idHoja.Value;
            model.IdFormato = idFormato;

            model.UtilizaHoja = true;
            model.OpGrabar = true;
            model.ListaHoja = listaHoja;
            model.ListaData = listaData;

            FormatoResultado modelResultado = GrabarExcelWeb(model);
            return Json(modelResultado);
        }

        /// <summary>
        /// Permite generar el formato en un archivo Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GenerarFormato(int idEmpresa, string fecha, string semana, string mes, int idFormato, List<int> listaHoja, List<string[][]> listaData, int? idHoja = 0)
        {
            string ruta = string.Empty;
            try
            {
                int idEnvio = -1;
                this.ListaHoja = listaHoja;
                this.ListaMatrizExcel = listaData;
                FormatoModel model = BuildHojaExcelInterconexion(idEmpresa, idEnvio, fecha, mes, idFormato, true, ConstantesFormato.NoVerUltimoEnvio, idHoja.Value);
                ruta = ToolsFormato.GenerarFileExcelFormato(model);
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                ruta = "-1";
            }
            return ruta;
        }
    }
}
