using COES.Framework.Base.Tools;
using COES.WebService.FileServer.Contratos;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Services;
using System.Xml.Linq;

namespace COES.WebService.FileServer.Servicios
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class FileServerServicio : IFileServerServicio
    {

        public static string YupanaDirectory = ConfigurationManager.AppSettings["PathArchivosYupana"];
        private static readonly ILog log = log4net.LogManager.GetLogger(typeof(FileServerServicio));

        /// <summary>
        /// Devuelve un listado de archivos 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public List<FileDataRequest> ListarArhivosWS(string relativePath)
        {
            List<FileDataRequest> salida = new List<FileDataRequest>();

            List<FileData> lstArchivos = COES.Framework.Base.Tools.FileServer.ListarArhivos("Modelos\\Yupana\\" + relativePath, YupanaDirectory);

            foreach (var archivo in lstArchivos)
            {
                FileDataRequest obj = new FileDataRequest();
                obj.FileUrl = archivo.FileUrl;
                obj.FileName = archivo.FileName;
                obj.FileType = archivo.FileType;
                obj.FileSize = archivo.FileSize;
                obj.Icono = archivo.Icono;
                obj.Extension = archivo.Extension;
                obj.FechaModificacion = archivo.FechaModificacion;
                obj.LastWriteTime = archivo.LastWriteTime;

                salida.Add(obj);
            }

            return salida;
        }

        /// <summary>
        /// Devuelve una secuencia de bytes del archivo a descargar
        /// </summary>
        /// <param name="nombreCarpeta"></param>
        /// <param name="nombreRecursoSeleccionado"></param>
        /// <returns></returns>
        public Stream ObtenerBytesArchivoADescargar(string nombreCarpeta, string nombreRecursoSeleccionado)
        {
            Stream salida = new MemoryStream();

            if (nombreRecursoSeleccionado != null && nombreRecursoSeleccionado != "")
            {
                var nombre = nombreRecursoSeleccionado;
                try
                {
                    // crear y Obtener path carpeta actual
                    string nombreCarpetaActual = nombreCarpeta;


                    string pathBaseVideosTutoriales = nombreCarpetaActual + "\\" + nombre;
                    Stream stream = COES.Framework.Base.Tools.FileServer.DownloadToStream(pathBaseVideosTutoriales, YupanaDirectory);

                    if (stream != null)
                    {
                        salida = stream;
                    }
                    else
                    {
                        log.Info("Download - No se encontro el archivo: " + nombreCarpetaActual);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            return salida;
        }
    }
}
