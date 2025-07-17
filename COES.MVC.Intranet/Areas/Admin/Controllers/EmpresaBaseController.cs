
using System;
using System.IO;
using System.Web.Mvc;
using System.IO.Compression;

namespace COES.MVC.Intranet.Areas.Admin.Controllers
{
    /// <summary>
    /// Clase controladora padre para PMPO
    /// </summary>
    public class EmpresaBaseController : COES.MVC.Intranet.Controllers.BaseController
    {
        #region Json

        /// <summary>
        /// Reemplza el método Json
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public new Helper.JsonDataContractActionResult Json(object data)
        {
            return new Helper.JsonDataContractActionResult(data);
        }

        #endregion

        #region DownloadFile

        /// <summary>
        /// Descarga de archivo
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public FilePathResult DownloadFile(string path, string name)
        {
            string fileName = (string.IsNullOrEmpty(name) ? Path.GetFileName(path) : name);

            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        #endregion

        #region DownloadDirectory

        /// <summary>
        /// Descarga de directorio
        /// </summary>
        /// <param name="path"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public FilePathResult DownloadDirectory(string path, string file)
        {
            string zipPathName,
                zipPath;

            if (Directory.Exists(path))
            {

                zipPathName = string.Format("{0}.zip", Path.GetFileName(!string.IsNullOrEmpty(file) ? file : path));
                zipPath = string.Format("{0}\\{1}", Path.GetTempPath(), Path.GetRandomFileName());

                ZipFile.CreateFromDirectory(path, zipPath, CompressionLevel.Optimal, false);

            }
            else
            {
                throw new Exception("Directorio no existe");
            }

            return File(zipPath, System.Net.Mime.MediaTypeNames.Application.Octet, zipPathName);
        }

        #endregion
    }
}