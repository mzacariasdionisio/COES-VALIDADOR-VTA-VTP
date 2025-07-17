using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.Interconexiones.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Interconexiones;
using COES.Servicios.Aplicacion.Interconexiones.Helper;
using COES.Servicios.Aplicacion.Intervenciones.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.Interconexiones.Controllers
{
    public class InformeController : BaseController
    {
        private InformeInterconexionAppServicio servicio = new InformeInterconexionAppServicio();
        private static readonly ILog Log = log4net.LogManager.GetLogger(typeof(InformeController));
        private static string NameController = "ReportesController";

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

        /// <summary>
        /// Muestra la ventana principal
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            InformeModel model = new InformeModel();
            model.Anio = DateTime.Now.Year;
            model.ListaSemana = this.servicio.ObtenerSemanasPorAnio(model.Anio);
            return View(model);
        }

        /// <summary>
        /// Permite obtener las semanas operativas
        /// </summary>
        /// <param name="anio"></param>
        /// <returns></returns>
        public JsonResult ObtenerSemanas(int anio)
        {
            return Json(this.servicio.ObtenerSemanasPorAnio(anio));
        }

        /// <summary>
        /// Permite generar un nuevo informe
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GenerarVersion(int anio, int semana)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + ConstantesInterconexiones.PathArchivos;   
            return Json(this.servicio.GenerarVersionInforme(anio, semana, path, base.UserName));
        }

        /// <summary>
        /// Permite descargar el archivo del informe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarArchivo(int id)
        {
            string fileName = string.Format(ConstantesInterconexiones.NombreArhivoInformeInterconexion, id);         
            Stream stream = FileServer.DownloadToStream(ConstantesInterconexiones.FolderIntervenciones + fileName,
                string.Empty);
            return File(stream, ConstantesInterconexiones.ExtensionInforme, fileName);
        }

        [HttpPost]
        public JsonResult VisualiarArchivo(int id)
        {
            InformeModel model = new InformeModel();

            try
            {
                string subcarpetaDestino = ConstantesIntervencionesAppServicio.RutaReportes;
                string directorioDestino = AppDomain.CurrentDomain.BaseDirectory + subcarpetaDestino;
                string fileName = string.Format(ConstantesInterconexiones.NombreArhivoInformeInterconexion, id);
                Stream stream = FileServer.DownloadToStream(ConstantesInterconexiones.FolderIntervenciones + fileName,string.Empty);
                FileServer.UploadFromStream(stream, string.Empty, fileName, directorioDestino);
                string url = subcarpetaDestino + fileName;
                model.Resultado = url;
                model.Detalle = fileName;
            }
            catch (Exception ex)
            {
                Log.Error(NameController, ex);
                model.Resultado = "-1";
                model.Mensaje = ex.Message;
                model.Detalle = ex.StackTrace;
            }

            return Json(model);
        }

        /// <summary>
        /// Permite mostrar el listado de versiones
        /// </summary>
        /// <param name="anio"></param>
        /// <param name="semana"></param>
        /// <returns></returns>
        public PartialViewResult Lista(int anio, int semana)
        {
            InformeModel model = new InformeModel();
            model.ListaVersion = this.servicio.ConsultarVersiones(anio, semana);
            return PartialView(model);
        }

        /// <summary>
        /// Muestra la ventana de antecedentes
        /// </summary>
        /// <returns></returns>
        public ActionResult Antecedentes()
        {
            return View();
        }

        /// <summary>
        /// Muestra la lista de antecedentes
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ListaAntecedentes()
        {
            AntecedentesModel model = new AntecedentesModel();
            model.ListaAntecedentes = this.servicio.ObtenerAntecedentes();
            return PartialView(model);
        }

        /// <summary>
        /// Pantalla de edicion de antecendentes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EditarAntecedentes(int id)
        {
            AntecedentesModel model = new AntecedentesModel();

            if (id == 0)            
                model.Entidad = new MeInformeAntecedenteDTO();            
            else            
                model.Entidad = this.servicio.ObtenerAntecedentePorId(id);            

            return Json(model);
        }

        /// <summary>
        /// Grabado de antecedentes
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult GrabarAntecedente(AntecedentesModel model)
        {
            MeInformeAntecedenteDTO entity = new MeInformeAntecedenteDTO();
            entity.Infantcodi = model.Codigo;
            entity.Infantorden = -1;
            entity.Intantcontenido = model.Contenido;
            entity.Intantestado = ConstantesAppServicio.Activo;
            entity.Intantusucreacion = base.UserName;
            entity.Intantfeccreacion = DateTime.Now;
            return Json(this.servicio.GrabarAntecedente(entity));
        }

        /// <summary>
        /// Eliminado de antecedentes
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult EliminarAntecedente(int id)
        {
            return Json(this.servicio.EliminarAntecedente(id));
        }

    }
}
