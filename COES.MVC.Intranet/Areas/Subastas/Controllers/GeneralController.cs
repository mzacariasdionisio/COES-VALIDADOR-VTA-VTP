using COES.MVC.Intranet.Areas.Subastas.Models;
using COES.MVC.Intranet.Controllers;
using COES.Servicios.Aplicacion.Helper;
using COES.Servicios.Aplicacion.Subastas;
using System.Web.Mvc;
using COES.Framework.Base.Tools;
using System.Configuration;
using System;
using COES.MVC.Intranet.Helper;

namespace COES.MVC.Intranet.Areas.Subastas.Controllers
{
    public class GeneralController : BaseController
    {
        public ActionResult Index()
        {
            if (!base.IsValidSesionView()) return base.RedirectToLogin();
            if (base.IdOpcion != ConstantesSubasta.MenuOpcionCodeSubastas) return base.RedirectToHomeDefault();

            ProcesoModel model = new ProcesoModel();
            model.TienePermiso = base.VerificarAccesoAccion(Acciones.Grabar, base.UserName);

            return View(model);
        }

        /// <summary>
        /// Descargar manual usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual FileResult DescargarManualUsuario()
        {
            string modulo = ConstantesSubasta.ModuloManualUsuario;
            string nombreArchivo = ConstantesSubasta.ArchivoManualUsuarioIntranet;
            string pathDestino = modulo + ConstantesSubasta.FolderRaizOfertasRSFModuloManual;
            string pathAlternativo = ConfigurationManager.AppSettings["FileSystemPortal"];

            try
            {
                if (FileServer.VerificarExistenciaFile(pathDestino, nombreArchivo, pathAlternativo))
                {
                    byte[] buffer = FileServer.DownloadToArrayByte(pathDestino + "\\" + nombreArchivo, pathAlternativo);
                    return File(buffer, Constantes.AppPdf, nombreArchivo);
                }
                else
                {
                    throw new ArgumentException("No se pudo descargar el archivo del servidor.");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("ERROR: ", ex);
            }
        }
    }
}