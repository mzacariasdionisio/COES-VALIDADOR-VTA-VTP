using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
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
        /// Permite retornar la vista empresa
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public ActionResult Index(int emprcodi)
        {          
            ViewBag.emprcodi = emprcodi;
            return View();            
        }

        /// <summary>
        /// Permite pintar la lista de representantes
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public PartialViewResult Listado(int emprcodi)
        {
            
            var DatosRepresentante = this.appRepresentante.GetByEmpresaSiRepresentante(emprcodi).Where(x => x.Rptetipo.ToString() == ConstantesRegistroIntegrantes.RepresentanteTipoLegal).ToList();
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
                model.FechaVigenciaPoder = DatosRepresentante.Rptefechavigenciapoder;

                if (DatosRepresentante.Rptefechavigenciapoder != null)
                {
                    model.FechaVigencia = ((DateTime)DatosRepresentante.Rptefechavigenciapoder).ToString(Constantes.FormatoFecha);
                }

                model.CargoEmpresa = DatosRepresentante.Rptecargoempresa;
                model.Telefono = DatosRepresentante.Rptetelefono;
                model.TelefonoMovil = DatosRepresentante.Rptetelfmovil;
                model.CorreoElectronico = DatosRepresentante.Rptecorreoelectronico;
                model.TipoRepresentanteLegal = DatosRepresentante.Rptetiprepresentantelegal;
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
        public JsonResult ActualizarGestionModificacion(
            int idRepresentante, 
            string tipoRepresentante,
            string dni,
            string nombre,
            string apellido,            
            string cargo,
            string telefono, 
            string telefonoMovil, 
            string fechaVigenciaPoder,
            string correo)
        {
            DateTime? fecha = null;
            
            if (!string.IsNullOrEmpty(fechaVigenciaPoder))
            {
                fecha = DateTime.ParseExact(fechaVigenciaPoder, Constantes.FormatoFecha, CultureInfo.InvariantCulture);
            }

            try
            {
                appRepresentante.ActualizarRepresentanteGestionModificacion(idRepresentante, tipoRepresentante,
                    dni, nombre, apellido, cargo, telefono, telefonoMovil, fecha, base.UserName, correo);
                return Json(1);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Json(-1);
            }
        }

        [HttpPost]
        public JsonResult DarBajaRepresentante(int idRepresentante)
        {
            try
            {
                appRepresentante.DarBajaRepresentante(idRepresentante, base.UserName);
                return Json(1);
            }
            catch
            {
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
                Stream stream = FileServer.DownloadToStream(ConstantesRegistroIntegrantes.FolderRI + url,  Path);
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