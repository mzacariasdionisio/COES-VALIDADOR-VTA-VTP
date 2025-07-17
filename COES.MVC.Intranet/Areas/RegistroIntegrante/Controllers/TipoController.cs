using COES.MVC.Intranet.Controllers;
using COES.MVC.Intranet.Areas.RegistroIntegrante.Models;
using COES.Servicios.Aplicacion.RegistroIntegrantes;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COES.Dominio.DTO.Sic;
using COES.MVC.Intranet.Helper;
using COES.MVC.Intranet.SeguridadServicio;
using System.IO;
using COES.Framework.Base.Tools;

namespace COES.MVC.Intranet.Areas.RegistroIntegrante.Controllers
{
    public class TipoController : BaseController
    {
        /// <summary>
        /// Instanciamiento de Log4net
        /// </summary>  
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(TipoController));

        /// <summary>
        /// Instancia de la clase RepresentanteAppServicio
        /// </summary>
        private TipoAppServicio appTipo = new TipoAppServicio();

        public TipoController()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                Exception objErr = filterContext.Exception;
                log.Error("TipoController", objErr);
            }
            catch (Exception ex)
            {
                log.Fatal("TipoController", ex);
                throw;
            }
        }

        /// <summary>
        /// Permite retornar la vista de personas de contactos
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public ActionResult Index(int emprcodi)
        {
            ViewBag.emprcodi = emprcodi;
            return View();
        }

        /// <summary>
        /// Permite pintar la lista de contactos
        /// </summary>
        /// <param name="emprcodi">codigo de empresa</param>
        /// <returns></returns>
        public PartialViewResult Listado(int emprcodi)
        {                      
            var DatosTipo = this.appTipo.GetByEmpresaSiTipo(emprcodi).ToList();

            TipoModel modelTipo = new TipoModel();
            modelTipo.ListaTipos = DatosTipo;

            return PartialView(modelTipo);
        }

        /// <summary>
        /// Permite cargar los datos del contacto seleccionado
        /// </summary>
        /// <param name="idTipo">Codigo de Tipo</param>
        [HttpPost]
        public PartialViewResult Edicion(int idTipo)
        {
            TipoModel model = new TipoModel();
            var DatosTipo = this.appTipo.GetByIdSiTipo(idTipo);
         
            model.Tipocodi = DatosTipo.Tipocodi;
            model.Tipoemprcodi = DatosTipo.Tipoemprcodi;
            model.Tipoprincipal = DatosTipo.Tipoprincipal;
            model.Tipotipagente = DatosTipo.Tipotipagente;
            model.Tipodocsustentatorio = DatosTipo.Tipodocsustentatorio;
            model.Tipoarcdigitalizado = DatosTipo.Tipoarcdigitalizado;
            model.Tipoarcdigitalizadofilename = DatosTipo.Tipoarcdigitalizadofilename;
            model.Tipopotenciainstalada = DatosTipo.Tipopotenciainstalada;
            model.Tiponrocentrales = DatosTipo.Tiponrocentrales;
            model.Tipolineatrans500 = DatosTipo.Tipolineatrans500;
            model.Tipolineatrans220 = DatosTipo.Tipolineatrans220;
            model.Tipolineatrans138 = DatosTipo.Tipolineatrans138;
            model.Tipolineatrans500km = DatosTipo.Tipolineatrans500km;
            model.Tipolineatrans220km = DatosTipo.Tipolineatrans220km;
            model.Tipolineatrans138km = DatosTipo.Tipolineatrans138km;
            model.Tipototallineastransmision = DatosTipo.Tipototallineastransmision;
            model.Tipomaxdemandacoincidente = DatosTipo.Tipomaxdemandacoincidente;
            model.Tipomaxdemandacontratada = DatosTipo.Tipomaxdemandacontratada;
            model.Tiponumsuministrador = DatosTipo.Tiponumsuministrador;

            return PartialView(model);
        }      

        /// <summary>
        /// Permite bajar el archivo del reporte generado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult Download(string url, string nombre)
        {
            try
            {
                Stream stream = FileServer.DownloadToStream(url, string.Empty);
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
                Stream stream = FileServer.DownloadToStream(url, string.Empty);
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