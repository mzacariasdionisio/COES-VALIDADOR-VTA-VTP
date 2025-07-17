using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Extranet.Areas.RegistroIntegrante.Models;
using COES.MVC.Extranet.Controllers;
using COES.MVC.Extranet.Helper;
using COES.MVC.Extranet.SeguridadServicio;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Extranet.Areas.RegistroIntegrante.Controllers
{
    public class RepresentanteController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(RepresentanteController));

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private RepresentanteAppServicio appRepresentante = new RepresentanteAppServicio();

        public RepresentanteController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("RepresentanteController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("RepresentanteController", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite retornar la vista de representantes
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {         
            return View();            
        }

        /// <summary>
        /// Permite pintar la lista de representantes
        /// </summary>        
        /// <returns></returns>
        public PartialViewResult Listado()
        {
            int idEmpresa = base.EmpresaId;            

            var DatosRepresentante = this.appRepresentante.GetByEmpresaSiRepresentante(idEmpresa).
                Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).ToList();

            RepresentanteModel modelRepresentante = new RepresentanteModel();           
            modelRepresentante.ListaRepresentantes = DatosRepresentante;

            return PartialView(modelRepresentante);
        }

        /// <summary>
        /// Permite CARGAR los datos del representante legal seleccionado
        /// </summary>
        /// <param name="idRepresentante">Codigo de Representante</param>
        [HttpPost]
        public PartialViewResult Edicion(int idRepresentante)
        {
            RepresentanteModel model    = new RepresentanteModel();
            var DatosRepresentante      = this.appRepresentante.GetByIdSiRepresentante(idRepresentante);
            if (DatosRepresentante.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal)
            {
                model.Documento = DatosRepresentante.Rptedocidentidad;
                model.DocumentoAdjunto = DatosRepresentante.Rptedocidentidadadj;
                model.DocumentoAdjuntoFileName = DatosRepresentante.Rptedocidentidadadjfilename;

                model.Nombres = DatosRepresentante.Rptenombres;
                model.Apellidos = DatosRepresentante.Rpteapellidos;

                model.VigenciaPoderAdjunto = DatosRepresentante.Rptevigenciapoder;
                model.VigenciaPoderAdjuntoFileName = DatosRepresentante.Rptevigenciapoderfilename;

                model.CargoEmpresa = DatosRepresentante.Rptecargoempresa;
                model.Telefono = DatosRepresentante.Rptetelefono;
                model.TelefonoMovil = DatosRepresentante.Rptetelfmovil;
                model.CorreoElectronico = DatosRepresentante.Rptecorreoelectronico;                
            }

            return PartialView(model);
        }

        /// <summary>
        ///  Permite ACTUALIZAR los datos del Representante Legal de la gestion de modificacion
        /// </summary>
        /// <param name="idRepresentante">codigo del representante</param>
        /// <param name="telefono">telefono</param>
        /// <param name="telefonoMovil"> telefono movil</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ActualizarGestionModificacion(int idRepresentante, string telefono, string telefonoMovil)
        {
            try
            {
                appRepresentante.ActualizarRepresentanteGestionModificacion(idRepresentante, telefono, telefonoMovil);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        /// <summary>
        /// Permite abrir el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url, string nombre)
        {
            try
            {
                string Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, Path);
                int indexOf = nombre.LastIndexOf('.');
                string extension = nombre.Substring(indexOf + 1, nombre.Length - indexOf - 1);

                if (stream != null)
                    return File(stream, extension, nombre);
                else
                {
                    log.Info("Download - No se encontro el archivo: " + url);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// Permite visualizar el archivo generado
        /// </summary>
        /// <returns></returns>
        public FileStreamResult ver(string url)
        {
            try
            {
                string Path = ConstantesRegistroIntegrantes.FolderUploadRutaCompleta;
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url, Path);
                FileStream fs = stream as FileStream;

                if (stream != null)
                    return File(fs, "application/pdf");
                else
                {
                    log.Info("Ver - No se encontro el archivo: " + url);
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

    }
}