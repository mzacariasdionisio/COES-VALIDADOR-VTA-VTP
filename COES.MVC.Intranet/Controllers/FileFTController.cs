using COES.Framework.Base.Tools;
using COES.MVC.Intranet.Helper;
using COES.Servicios.Aplicacion.Equipamiento;
using COES.Servicios.Aplicacion.Equipamiento.Helper;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace COES.MVC.Intranet.Controllers
{
    public class FileFTController : BaseController
    {
        /// <summary>
        /// Descarga de archivos confidenciales del fileapp. La validación de la sesión del usuario ya no se hace en el "fileapp" sino en intranet
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarSustentoConfidencial(string url, string idapp)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                //solo permitir la descarga del archivo cuando se realiza un nuevo envio temporal. O cuando el usuario COES tiene permiso confidencial y usa la URL desde su navegador
                bool esAdminFT = ConstantesFichaTecnica.ClaveOcultaPermiteDescargaConfidencial == idapp || base.TienePermisosConfidencialFT(Acciones.Confidencial);

                if (esAdminFT)
                {
                    //sea confidencial o no, como es usuario con rol admin entonces se descargará el archivo
                    ObtenerArchivoSustentoConfidencialFT(url, out Stream stream, out string extension, out string fileName);

                    //validar existencia de archivo
                    if (stream == null)
                    {
                        //descargar archivo que no existe en el fileapp
                        string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoNoDisponible;
                        return Redirect(urlArchivoSinPermiso);
                    }
                    else
                    {
                        //descargar archivo del fileapp
                        return File(stream, extension, fileName);
                    }
                }
                else
                {
                    //descargar .txt cuando no tiene permisos
                    string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                    return Redirect(urlArchivoSinPermiso);
                }

            }
            else
            {
                //si no tiene sesión entonces redirigirlo al Login y luego vuelva a descargar el archivo
                string urlNavegador = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
                return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler, new { area = string.Empty, originalUrl = urlNavegador });
            }
        }

        private void ObtenerArchivoSustentoConfidencialFT(string url, out Stream stream, out string extension, out string fileName)
        {
            stream = null;
            extension = "";
            fileName = "";

            try
            {

                //datos de archivo
                int indexOf = url.LastIndexOf('/');

                fileName = url;
                if (indexOf >= 0)
                {
                    fileName = url.Substring(indexOf + 1, url.Length - indexOf - 1);
                }
                indexOf = fileName.LastIndexOf('.');
                extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

                //acceder al file server del fileapp
                string pathAlternativo = ConfigurationManager.AppSettings[ConstantesFichaTecnica.KeyFileServerFileAppFichaTecnica].ToString();

                stream = FileServer.DownloadToStream(url, pathAlternativo);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Descarga de archivos confidenciales cargados por los agentes en Extranet y aprobados en Intranet.
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult DescargarConfidencial(string archivo, string idapp)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                //solo permitir la descarga del archivo cuando se realiza un nuevo envio temporal. O cuando el usuario COES tiene permiso confidencial y usa la URL desde su navegador
                bool esAdminFT = ConstantesFichaTecnica.ClaveOcultaPermiteDescargaConfidencial == idapp || base.TienePermisosConfidencialFT(Acciones.Confidencial);

                if (esAdminFT)
                {
                    //sea confidencial o no, como es usuario con rol admin entonces se descargará el archivo
                    ObtenerArchivoConfidencialFT(archivo, out Stream stream, out string extension, out string fileName);

                    //validar existencia de archivo
                    if (stream == null)
                    {
                        //descargar archivo que no existe en el fileapp
                        string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoNoDisponible;
                        return Redirect(urlArchivoSinPermiso);
                    }
                    else
                    {
                        //descargar archivo del fileapp
                        return File(stream, extension, fileName);
                    }
                }
                else
                {
                    //descargar .txt cuando no tiene permisos
                    string urlArchivoSinPermiso = FichaTecnicaAppServicio.GetUrlFileappFichaTecnica() + "Content/" + ConstantesFichaTecnica.ArchivoConfidencialSinPermiso;
                    return Redirect(urlArchivoSinPermiso);
                }

            }
            else
            {
                //si no tiene sesión entonces redirigirlo al Login y luego vuelva a descargar el archivo
                string urlNavegador = System.Web.HttpContext.Current.Request.Url.PathAndQuery;
                return RedirectToAction(Constantes.LoginAction, Constantes.DefaultControler, new { area = string.Empty, originalUrl = urlNavegador });
            }
        }

        private void ObtenerArchivoConfidencialFT(string archivo, out Stream stream, out string extension, out string fileName)
        {
            stream = null;
            extension = "";
            fileName = "";

            try
            {
                //datos de archivo
                fileName = archivo;
                int indexOf = fileName.LastIndexOf('.');
                extension = fileName.Substring(indexOf + 1, fileName.Length - indexOf - 1);

                var arr = fileName.Split('_');
                string subCarpetaEnvio = "envio_" + arr[0].Replace("env", "");
                string subCarpetaEqGr = arr[1];

                //acceder al file server de la Extranet de ficha técnica
                string pathAlternativo = FileServer.GetDirectory();

                string pathDestino = FichaTecnicaAppServicio.GetPathPrincipalExtranetAprobado() + ConstantesFichaTecnica.SubcarpetaConfidencial + "/"+ subCarpetaEnvio + "/"+ subCarpetaEqGr;
                stream = FileServer.DownloadToStream(fileName, pathDestino + "\\");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
