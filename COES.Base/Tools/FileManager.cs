using COES.Base.Core;
using COES.Base.Tools;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace COES.Framework.Base.Tools
{
    /// <summary>
    /// Clase para manejo de archivos
    /// </summary>
    public class FileServer
    {
        public static string UserLogin = ConfigurationManager.AppSettings["UserFS"];
        public static string Domain = ConfigurationManager.AppSettings["DomainFS"];
        public static string Password = ConfigurationManager.AppSettings["PasswordFS"];


        /// <summary>
        /// Directorio base por defecto
        /// </summary>
        public static string GetDirectory()
        {
            return ConfigurationManager.AppSettings["LocalDirectory"];
        }

        /// <summary>
        /// Permite crear un archivo desde un stram
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="targetBlobName"></param>
        /// <returns></returns>
        public static bool UploadFromStream(Stream stream, string url, string targetBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;
            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string path = LocalDirectory + url;
                if (Directory.Exists(path))
                {
                    string file = path + targetBlobName;
                    if (!File.Exists(file))
                    {
                        using (var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        File.Delete(file);
                        using (var fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Permite cargar desde un directorio
        /// </summary>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <param name="targetBlobName"></param>
        /// <returns></returns>
        public static bool UploadFromFileDirectory(string path, string url, string targetBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;
            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string directorio = LocalDirectory + url;
                if (Directory.Exists(directorio))
                {
                    string file = directorio + targetBlobName;
                    if (!File.Exists(file) && File.Exists(path))
                    {
                        File.Copy(path, file);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Permite descargar en array de bytes
        /// </summary>
        /// <param name="sourceBlobName"></param>
        /// <returns></returns>
        public static byte[] DownloadToArrayByte(string sourceBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                byte[] array = null;
                string path = LocalDirectory + sourceBlobName;

                if (File.Exists(path))
                {
                    array = File.ReadAllBytes(path);
                }

                return array;
            }
        }

        /// <summary>
        /// Permite descargar un archivo como un stream
        /// </summary>
        /// <param name="sourceBlobName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream DownloadToStream(string sourceBlobName, string pathAlternativo)
        {

            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                Stream stream = new MemoryStream();
                string path = LocalDirectory + sourceBlobName;

                if (File.Exists(path))
                {
                    stream = File.OpenRead(path);
                }

                return stream;
            }
        }

        /// <summary>
        /// Permite descargar los archivos como un zip
        /// </summary>
        /// <param name="sourcesBlob"></param>
        /// <returns></returns>
        public static int DownloadAsZip(List<string> sourcesBlob, string filename, string pathAlternativo)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    foreach (string sourceBlobName in sourcesBlob)
                    {
                        int indexOf = sourceBlobName.LastIndexOf('/');
                        string fileName = string.Empty;
                        if (indexOf >= 0)
                        {
                            fileName = sourceBlobName.Substring(indexOf + 1, sourceBlobName.Length - indexOf - 1);
                        }
                        else
                        {
                            fileName = sourceBlobName;
                        }

                        Stream stream = DownloadToStream(sourceBlobName, pathAlternativo);
                        zip.AddEntry(fileName, stream);
                    }

                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    zip.Save(filename);
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }



        /// <summary>
        /// Permite descargar los archivos como un zip
        /// </summary>
        /// <param name="sourcesBlob"></param>
        /// <returns></returns>
        public static int DownloadAsZipRenombadro(List<string> sourcesBlob, List<string> names, string filename, string pathAlternativo)
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    int index = 0;

                    try
                    {
                        foreach (string sourceBlobName in sourcesBlob)
                        {
                            int indexOf = sourceBlobName.LastIndexOf('/');
                            string fileName = string.Empty;
                            if (indexOf >= 0)
                            {
                                fileName = sourceBlobName.Substring(indexOf + 1, sourceBlobName.Length - indexOf - 1);
                            }
                            else
                            {
                                fileName = sourceBlobName;
                            }

                            Stream stream = DownloadToStream(sourceBlobName, pathAlternativo);
                            zip.AddEntry(names[index], stream);

                            index++;
                        }

                    }
                    catch (Exception ex)
                    {
                        string s = index.ToString();
                    }
                    if (File.Exists(filename))
                    {
                        File.Delete(filename);
                    }

                    zip.Save(filename);
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite renombrar un elemento
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="newBlobName"></param>
        /// <returns></returns>
        public static bool RenameBlob(string url, string blobName, string newBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (!string.IsNullOrEmpty(url)) url = url + "/";

                string pathAnterior = LocalDirectory + url + blobName;

                if (File.Exists(pathAnterior))
                {
                    string pathNuevo = LocalDirectory + url + newBlobName;

                    if (!File.Exists(pathNuevo))
                    {
                        File.Move(pathAnterior, pathNuevo);
                        return true;
                    }
                }

                return true;
            }
        }

        public static bool CopyBlob(string url, string blobName, string newBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (!string.IsNullOrEmpty(url)) url = url + "/";

                string pathAnterior = LocalDirectory + url + blobName;

                if (File.Exists(pathAnterior))
                {
                    string pathNuevo = LocalDirectory + url + newBlobName;

                    if (!File.Exists(pathNuevo))
                    {
                        File.Copy(pathAnterior, pathNuevo);
                        return true;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Permite validar la existencia de un archivo
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public static bool VerificarExistenciaFile(string url, string blobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (!string.IsNullOrEmpty(url)) url = url + "/";

                if (File.Exists(LocalDirectory + url + blobName))
                {
                    return true;
                }

                return false;
            }
        }


        /// <summary>
        /// Permite eliminar un elemento
        /// </summary>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public static void DeleteBlob(string blobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string filename = LocalDirectory + blobName;

                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }

        /// <summary>
        /// Permite crear un directorio en un carpeta determinada
        /// </summary>
        /// <param name="url"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool CreateFolder(string url, string folderName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (!string.IsNullOrEmpty(url)) url = url + "/";
                string path = LocalDirectory + url + folderName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Permite eliminacion recursiva del folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public bool DeleteFolder(string url)
        {
            string LocalDirectory = GetDirectory();
            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string path = LocalDirectory + url;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }
                return false;
            }
        }

        #region Movisoft - 18685
        /// <summary>
        /// Permite eliminacion recursiva del folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool DeleteFolderAlter(string url, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string path = LocalDirectory + url;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    return true;
                }
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Permite obtener el cursor del archivo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static StreamReader OpenReaderFile(string sourceBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                StreamReader stream = null;
                string path = LocalDirectory + sourceBlobName;

                if (File.Exists(path))
                {
                    stream = new StreamReader(LocalDirectory + sourceBlobName);
                }

                return stream;
            }
        }

        /// <summary>
        /// Permite obtener el cursor del archivo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static StreamWriter OpenWriterFile(string sourceBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                StreamWriter stream = new StreamWriter(LocalDirectory + sourceBlobName);

                return stream;
            }
        }

        /// <summary>
        /// Permite obtener el cursor del archivo
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static StreamWriter OpenWriterFileEncoding(string sourceBlobName, string pathAlternativo, int encoding)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                StreamWriter stream = new StreamWriter(LocalDirectory + sourceBlobName, true, Encoding.GetEncoding(encoding));

                return stream;
            }
        }

        public static bool RenameFolderAlter(string url, string folderName, string folderAnterior, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (folderName.ToLower() != folderAnterior.ToLower())
                {
                    if (!string.IsNullOrEmpty(url)) url = url + "/";

                    string pathAnterior = LocalDirectory + url + folderAnterior;
                    if (Directory.Exists(pathAnterior))
                    {
                        string pathNuevo = LocalDirectory + url + folderName;

                        if (!Directory.Exists(pathNuevo))
                        {
                            Directory.Move(pathAnterior, pathNuevo);
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Permite obtener los archivos en base al criterio seleccionado
        /// </summary>
        /// <param name="url"></param>
        /// <param name="extension"></param>
        /// <param name="fecha"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static List<FileData> ObtenerArchivosPorCriterio(string url, string extension, DateTime fechaIncio, DateTime fechaFin, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                List<FileData> entitys = new List<FileData>();
                DirectoryInfo directoryInfo = new DirectoryInfo(LocalDirectory + url);

                if (directoryInfo.Exists)
                {
                    FileInfo[] files = directoryInfo.GetFiles().Where(x => x.Extension == extension &&
                        x.LastWriteTime >= fechaIncio && x.LastWriteTime <= fechaFin).OrderByDescending(x => x.LastWriteTime).ToArray()
                        ;
                    foreach (FileInfo file in files)
                    {
                        FileData entity = new FileData();
                        entity.FileName = file.Name;
                        entity.Extension = file.Extension;
                        entity.LastWriteTime = file.LastWriteTime.ToString(ConstantesBase.FormatoFechaExtendido);
                        entitys.Add(entity);
                    }
                }
                return entitys;
            }
        }

        /// <summary>
        /// Permite buscar los folder cargados durante la ultima hora
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static List<FileData> ObtenerFolderPorCriterio(string url, DateTime fechaInicio, DateTime fechaFin, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                List<FileData> entitys = new List<FileData>();
                DirectoryInfo directoryInfo = new DirectoryInfo(LocalDirectory + url);

                if (directoryInfo.Exists)
                {

                    DirectoryInfo[] directories = directoryInfo.GetDirectories().Where(x => x.LastWriteTime >= fechaInicio && x.LastWriteTime <=
                        fechaFin).OrderByDescending(x => x.LastWriteTime).ToArray();

                    foreach (DirectoryInfo file in directories)
                    {
                        FileData entity = new FileData();
                        entity.FileName = file.Name;
                        entity.LastWriteTime = file.LastWriteTime.ToString(ConstantesBase.FormatoFechaExtendido);
                        entitys.Add(entity);
                    }
                }
                return entitys;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFile(string origen, string destino, string file, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen + file;
                    string pathDestino = LocalDirectory + destino + file;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo y lo renombra
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <param name="pathAlternativo"></param>
        /// <param name="fileRename"></param>
        /// <returns></returns>
        public static int CopiarFileRename(string origen, string destino, string file, string pathAlternativo, string fileRename)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen + file;
                    string pathDestino = LocalDirectory + destino + fileRename;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int MoveFile(string url, string origen, string destino, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + url + origen;
                    string pathDestino = LocalDirectory + url + destino;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Move(pathOrigen, pathDestino);
                        }
                        else
                        {
                            File.Delete(pathDestino);
                            File.Move(pathOrigen, pathDestino);
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarArchivo(string url, string origen, string destino, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + url + origen;
                    string pathDestino = LocalDirectory + url + destino;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }                        
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFileAlter(string origen, string destino, string file, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = origen + file; // LocalDirectory + origen + file;
                    string pathDestino = LocalDirectory + destino + file;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFileDirectory(string path, string fileOrigen, string fileDestino, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory  + path + fileOrigen; 
                    string pathDestino = LocalDirectory + path + fileDestino;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFileAlterFinal(string origen, string destino, string file, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = origen + file; // LocalDirectory + origen + file;
                    string pathDestino = LocalDirectory + destino + file;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                            return 1;
                        }
                        else
                        {
                            File.Delete(pathDestino);
                            File.Copy(pathOrigen, pathDestino);
                            return 1;
                        }
                    }
                    else
                    {
                        if (File.Exists(pathDestino))
                        {
                            File.Delete(pathDestino);
                        }
                    }
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int DescargarCopia(string origen, string destino, string file, string pathAlternativo, Boolean eliminarDestino = true)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen + file;
                    string pathDestino = destino + file;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Copy(pathOrigen, pathDestino);
                        }
                        else
                        {
                            File.Delete(pathDestino);
                            File.Copy(pathOrigen, pathDestino);
                            return 1;
                        }
                    }
                    else
                    {
                        if (File.Exists(pathDestino) && eliminarDestino)
                        {
                            File.Delete(pathDestino);
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite descomprimir un directorio
        /// </summary>
        /// <param name="pathSource"></param>
        /// <param name="pathDestino"></param>
        /// <param name="pathAlternativo"></param>
        public static void DescomprimirZip(string pathSource, string pathDestino, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(pathAlternativo + pathSource, pathDestino);                
            }
        }

        /// <summary>
        /// Permite descomprimir un directorio
        /// </summary>
        /// <param name="pathSource"></param>
        /// <param name="pathDestino"></param>
        /// <param name="pathAlternativo"></param>
        public static void DescomprimirZipAlter(string pathSource, string pathDestino, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                System.IO.Compression.ZipFile.ExtractToDirectory(pathAlternativo + pathSource, pathAlternativo + pathDestino);
            }
        }

        /// <summary>
        /// Permite descomprimir un directorio
        /// </summary>
        /// <param name="pathSource"></param>
        /// <param name="pathDestino"></param>
        /// <param name="pathAlternativo"></param>
        public static void ComprimirDirectorio(string path, string folderSource, string fileZip, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string folderAComprimir = LocalDirectory + path + "/" + folderSource;
                string archivoComprido = LocalDirectory + path + "/" + fileZip;

                System.IO.Compression.ZipFile.CreateFromDirectory(folderAComprimir, archivoComprido);
            }
        }

        /// <summary>
        /// Permite cortar archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public int CortarFile(string origen, string destino, string file)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen + file;
                    string pathDestino = LocalDirectory + destino + file;

                    if (File.Exists(pathOrigen))
                    {
                        if (!File.Exists(pathDestino))
                        {
                            File.Move(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar el contenido de una carpeta en otra
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="pathAlternativo"></param>
        /// <returns></returns>
        public static int CopiarDirectoryAlter(string origen, string destino, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string pathOrigen = LocalDirectory + origen;
                string pathDestino = LocalDirectory + destino;

                if (Directory.Exists(pathOrigen))
                {
                    DirectoryInfo diSource = new DirectoryInfo(pathOrigen);
                    DirectoryInfo diTarget = new DirectoryInfo(pathDestino);

                    CopyAll(diSource, diTarget);
                }

                return 1;
            }
        }

        /// <summary>
        /// Copia una carpeta y contenido de forma recursiva
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            string LocalDirectory = GetDirectory();
            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (Directory.Exists(target.FullName) == false)
                {
                    Directory.CreateDirectory(target.FullName);
                }

                foreach (FileInfo fi in source.GetFiles())
                {
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }

                foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
        }

        /// <summary>
        /// Permite obtener los blobs dentro de un directorio con datos adicionales
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static List<FileData> ListarArhivos(string relativePath, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                List<FileData> entitys = new List<FileData>();
                DirectoryInfo directoryInfo = new DirectoryInfo(LocalDirectory + "\\" + relativePath);

                if (directoryInfo.Exists)
                {
                    foreach (FileInfo file in directoryInfo.GetFiles().OrderBy(x => x.Name))
                    {
                        FileData entity = new FileData();
                        entity.FileName = file.Name;
                        entity.Extension = file.Extension;
                        entity.FileSize = ((decimal)(file.Length / (decimal)(1024))).ToString("#,###.##") + " KB";
                        entity.FileType = ConstantesBase.TipoFile;
                        entity.FileUrl = relativePath + "/" + file.Name;
                        entity.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, file.Extension);
                        entity.FileUrl = entity.FileUrl.Replace('\\', '/');
                        entity.FechaModificacion = file.LastWriteTime.ToString(ConstantesBase.FormatFechaFull);
                        entity.LastWriteTime = file.LastWriteTime.ToString(ConstantesBase.FormatoFechaExtendido);
                        entitys.Add(entity);
                    }
                    foreach (DirectoryInfo directory in directoryInfo.GetDirectories().OrderBy(x => x.Name))
                    {
                        FileData entity = new FileData();
                        entity.FileName = directory.Name;
                        entity.FileType = ConstantesBase.TipoFolder;
                        entity.FileUrl = directory.FullName.Replace(LocalDirectory, string.Empty) + "/";
                        entity.FileUrl = entity.FileUrl.Replace('\\', '/');
                        entity.Icono = Util.ObtenerIcono(ConstantesBase.TipoFolder, string.Empty);
                        entity.FechaModificacion = directory.LastWriteTime.ToString(ConstantesBase.FormatFechaFull);
                        entitys.Add(entity);
                    }
                }
                return entitys;
            }
        }

        /// <summary>
        /// Permite obtener en breadcrumb de la libreria
        /// </summary>
        /// <param name="urlBase"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static List<BreadCrumb> ObtenerBreadCrumb(string urlBase, string url)
        {
            List<BreadCrumb> entitys = new List<BreadCrumb>();

            BreadCrumb entity = new BreadCrumb();
            entity.Name = ConstantesBase.FolderMain;
            entity.Url = urlBase;
            entitys.Add(entity);

            if (urlBase != url)
            {
                string enlace = (!string.IsNullOrEmpty(urlBase)) ? url.Replace(urlBase, string.Empty) : url;
                string[] split = enlace.Split(ConstantesBase.CaracterSlash);

                string urlBred = urlBase;

                foreach (string part in split)
                {
                    urlBred = urlBred + part + ConstantesBase.CaracterSlash.ToString();
                    BreadCrumb item = new BreadCrumb();
                    item.Name = part;
                    item.Url = urlBred;

                    if (!string.IsNullOrEmpty(part))
                    {
                        entitys.Add(item);
                    }
                }
            }

            return entitys;
        }

        /// <summary>
        /// Permite validar la existencia de un directorio
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public static bool VerificarExistenciaDirectorio(string url, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                if (Directory.Exists(LocalDirectory + url))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Permite validar la existencia de un directorio
        /// </summary>
        /// <param name="url"></param>
        /// <param name="blobName"></param>
        /// <returns></returns>
        public static bool VerificarLaExistenciaDirectorio(string url)
        {
            using (new Impersonator(UserLogin, Domain, Password, url))
            {
                if (Directory.Exists(url))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Crea un archivo zip que contiene una lista de archivos especificados
        /// </summary>
        /// <param name="relativeFilePaths">Lista de rutas relativas de archivos a comprimir</param>
        /// <param name="destinationArchiveFileName">Ruta completa del archivo zip a crear</param>
        /// <param name="localDirectoryAlternative">Ruta raíz alternativa (opcional)</param>
        public static void CreateZipFromFiles(List<string> relativeFilePaths, string destinationArchiveFileName, string localDirectoryAlternative = null)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(localDirectoryAlternative)) LocalDirectory = localDirectoryAlternative;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    foreach (var relativePath in relativeFilePaths)
                    {
                        string fullPath = Path.Combine(LocalDirectory, relativePath);
                        if (File.Exists(fullPath))
                        {                           
                            zip.AddFile(fullPath, "");
                        }
                    }
                    string file = Path.Combine(LocalDirectory, destinationArchiveFileName);
                    zip.Save(file);
                }
            }
        }

        #region Pronostico de la demanda Etapa 3
        /// <summary>
        /// Permite obtener un archivo especifico
        /// </summary>
        /// <param name="ruta">ruta del directorio</param>
        /// <param name="nombreArchivo">nombre del archivo a obtener</param>
        /// <returns></returns>
        public static FileData ObtenerArchivoEspecifico(string ruta, string nombreArchivo)
        {
            using (new Impersonator(UserLogin, Domain, Password, ruta))
            {
                FileData entity = new FileData();
                FileInfo archivoInfo = new FileInfo(ruta + nombreArchivo);

                if (archivoInfo.Exists)
                {
                    entity.FileName = archivoInfo.Name;
                    entity.Extension = archivoInfo.Extension;
                    entity.FileSize = ((decimal)(archivoInfo.Length / (decimal)(1024))).ToString("#,###.##") + " KB";
                    entity.FileType = ConstantesBase.TipoFile;
                    entity.FileUrl = ruta + "/" + archivoInfo.Name;
                    entity.Icono = Util.ObtenerIcono(ConstantesBase.TipoFile, archivoInfo.Extension);
                    entity.FileUrl = entity.FileUrl.Replace('\\', '/');
                    entity.FechaModificacion = archivoInfo.LastWriteTime.ToString(ConstantesBase.FormatFechaFull);
                    entity.LastWriteTime = archivoInfo.LastWriteTime.ToString(ConstantesBase.FormatoFechaExtendido);
                }
                return entity;
            }
        }

        #endregion

        #region INTERVENCIONES
        /// <summary>
        /// Permite copiar el contenido de una carpeta en otra por directorio
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <returns></returns>
        public int CopiarDirectory(string origen, string destino)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen;
                    string pathDestino = LocalDirectory + destino;

                    if (Directory.Exists(pathOrigen))
                    {
                        if (!Directory.Exists(pathDestino))
                        {
                            DirectoryInfo diSource = new DirectoryInfo(pathOrigen);
                            DirectoryInfo diTarget = new DirectoryInfo(pathDestino);

                            CopyAll(diSource, diTarget);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite cortar un directorio y pegarlo en otra ubicacion por por directorio
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <returns></returns>
        public int CortarDirectory(string origen, string destino)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    string pathOrigen = LocalDirectory + origen;
                    string pathDestino = LocalDirectory + destino;

                    if (Directory.Exists(pathOrigen))
                    {
                        if (!Directory.Exists(pathDestino))
                        {
                            Directory.Move(pathOrigen, pathDestino);
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    return 1;
                }
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFileAlterFinalOrigen(string origen, string destino, string file, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string pathOrigen = LocalDirectory + origen + file; // LocalDirectory + origen + file;
                string pathDestino = destino + file;

                if (File.Exists(pathOrigen))
                {
                    if (!File.Exists(pathDestino))
                    {
                        File.Copy(pathOrigen, pathDestino);
                        return 1;
                    }
                    else
                    {
                        File.Delete(pathDestino);
                        File.Copy(pathOrigen, pathDestino);
                        return 1;
                    }
                }
                else
                {
                    if (File.Exists(pathDestino))
                    {
                        File.Delete(pathDestino);
                        return -1;
                    }
                }
                return -1;
            }
        }

        /// <summary>
        /// Permite descargar los archivos como un zip
        /// </summary>
        /// <param name="sourcesBlob"></param>
        /// <returns></returns>
        public static int DownloadAsZipCustom(List<string> sourcesBlob, string pathFiles, string filename, string pathAlternativo)
        {
            try
            {
                string LocalDirectory = GetDirectory();
                if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

                using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
                {
                    using (ZipFile zip = new ZipFile())
                    {
                        foreach (string sourceBlobName in sourcesBlob)
                        {
                            string fileName = Path.GetFileName(sourceBlobName);
                            Stream stream = DownloadToStream(sourceBlobName, pathAlternativo);

                            if (stream != null)
                            {
                                if (stream.CanSeek) stream.Position = 0;
                                zip.AddEntry(fileName, stream);
                            }
                        }

                        if (File.Exists(LocalDirectory + pathFiles + filename))
                        {
                            File.Delete(LocalDirectory + pathFiles + filename);
                        }

                        zip.Save(LocalDirectory + pathFiles + filename);
                        return 1;
                    }
                }
            }
            catch
            {
                return -1;
            }
        }
        /// <summary>
        /// Permite renombrar un elemento y copiar de una ruta a otra ruta
        /// </summary>
        /// <param name="origen"></param> //Ruta de archivo de donde se copia (origen)
        /// <param name="blobName"></param> //Nombre actual de archivo
        /// <param name="newBlobName"></param> //Nombre nuevo de archivo
        /// <param name="pathAlternativo"></param> //File server a dónde se copia (destino)
        /// <returns></returns>
        public static bool RenameBlobCopyFile(string url, string blobName, string newBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, pathAlternativo))
            {           
                string pathAnterior = url + blobName;

                if (File.Exists(pathAnterior))
                {
                    string pathNuevo = LocalDirectory + newBlobName;

                    if (File.Exists(pathNuevo))
                    {
                        File.Delete(pathNuevo);
                    }
                    if (!File.Exists(LocalDirectory))
                       Directory.CreateDirectory(LocalDirectory);

                    File.Copy(pathAnterior, pathNuevo);
                    return true;
                    

                }
                
                return true;
            }
        }
        /// <summary>
        /// Permite renombrar un elemento y copiar de una ruta a otra ruta
        /// </summary>
        /// <param name="origen"></param> //Ruta de archivo de donde se copia (origen)
        /// <param name="blobName"></param> //Nombre actual de archivo
        /// <param name="newBlobName"></param> //Nombre nuevo de archivo
        /// <param name="pathAlternativo"></param> //File server a dónde se copia (destino)
        /// <returns></returns>
        public static bool RenameBlobCopyFile2(string url, string blobName, string newBlobName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, pathAlternativo))
            {
                string pathAnterior = url + blobName;

                if (File.Exists(pathAnterior))
                {
                    string pathNuevo = LocalDirectory + newBlobName;
                    if (File.Exists(pathNuevo))
                    {
                        File.Delete(pathNuevo);
                        if (!File.Exists(LocalDirectory))
                            Directory.CreateDirectory(LocalDirectory);
                    }
                        
                    File.Copy(pathAnterior, pathNuevo);
                    return true;
 

                }

                return true;
            }
        }

        public static int RenameFileCopyFile(string origen, string destino, string fileName, string newFileName, string pathAlternativo)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                string pathOrigen = LocalDirectory + origen + fileName;
                string pathDestino = LocalDirectory + destino + newFileName;

                if (File.Exists(pathOrigen))
                {
                    if (!File.Exists(pathDestino))
                    {
                        File.Copy(pathOrigen, pathDestino);
                    }
                    else
                    {
                        File.Delete(pathDestino);
                        File.Copy(pathOrigen, pathDestino);
                        return 1;
                    }
                }
                else
                {
                    if (File.Exists(pathDestino))
                    {
                        File.Delete(pathDestino);
                    }
                }
                return 1;
            }
        }

        /// <summary>
        /// Eliminar archivos que están en la carpeta reporte Cada vez que se ingrese al módulo luego de reiniciado el servidor
        /// </summary>
        /// <param name="carpetaLocalReporte"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void EliminarArchivosTemporalesCarpetaReporte(string carpetaLocalReporte, List<string> listaArchivoExcluir)
        {
            try
            {
                //COES.MVC.Extranet/Areas/modulo/Reporte
                if (!string.IsNullOrEmpty(carpetaLocalReporte))
                {
                    var listaDocumentos = FileServer.ListarArhivos(null, carpetaLocalReporte);

                    if (listaDocumentos.Any())
                    {
                        foreach (var item in listaDocumentos)
                        {
                            //eliminar los archivos a excepción de algunos
                            if (!listaArchivoExcluir.Contains(item.FileName))
                            {
                                FileServer.DeleteBlob(item.FileName, carpetaLocalReporte);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //No se pudo eliminar el archivo del servidor.
            }
        }

        #endregion

        #region Potencia Firme Remunerable

        /// <summary>
        /// Crea un archivo zip que contienen los archivos y directorios del directorio especificado
        /// </summary>
        /// <param name="sourceDirectoryName">Ruta de directorio acceder</param>
        /// <param name="destinationArchiveFileName">Ruta directorio donde se creará</param>
        /// <param name="localDirectoryAlternative">Ruta raiz directorio alternativo</param>
        public static void CreateZipFromDirectory(string sourceDirectoryName, string destinationArchiveFileName, string localDirectoryAlternative = null)
        {
            string LocalDirectory = GetDirectory();
            if (!string.IsNullOrEmpty(localDirectoryAlternative)) LocalDirectory = localDirectoryAlternative;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                System.IO.Compression.ZipFile.CreateFromDirectory(LocalDirectory + sourceDirectoryName, destinationArchiveFileName);
            }
        }
        #endregion

        #region Reportes Frecuencia
        /// <summary>
        /// Leer el contenido de un archivo
        /// </summary>
        /// <param name="sourceDirectoryName">Ruta de Directorio acceder</param>
        /// <param name="sourceFileName">Ruta Completa de Archivo a acceder</param>
        public static string ReadFileFromDirectory(string sourceDirectoryName, string sourceFileName)
        {
            string text = string.Empty;
            using (new Impersonator(UserLogin, Domain, Password, sourceDirectoryName))
            {
                if (Directory.Exists(sourceDirectoryName))
                {
                    if (File.Exists(sourceFileName))
                    {
                        text = System.IO.File.ReadAllText(sourceFileName);                       
                    }

                }

            }
            return text;
        }
        #endregion

    }

    /// <summary>
    /// Clase para manejo de permisos
    /// </summary>
    public class Impersonator : IDisposable
    {
        #region Public methods.
        // ------------------------------------------------------------------

        /// <summary>
        /// Constructor. Starts the impersonation with the given credentials.
        /// Please note that the account that instantiates the Impersonator class
        /// needs to have the 'Act as part of operating system' privilege set.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        public Impersonator(
            string userName,
            string domainName,
            string password,
            string path)
        {
            ImpersonateValidUser(userName, domainName, password, path);
        }

        // ------------------------------------------------------------------
        #endregion

        #region IDisposable member.
        // ------------------------------------------------------------------

        public void Dispose()
        {
            UndoImpersonation();
        }

        // ------------------------------------------------------------------
        #endregion

        #region P/Invoke.
        // ------------------------------------------------------------------

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(
            string lpszUserName,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(
            IntPtr hToken,
            int impersonationLevel,
            ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(
            IntPtr handle);

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        // ------------------------------------------------------------------
        #endregion

        #region Private member.
        // ------------------------------------------------------------------

        /// <summary>
        /// Does the actual impersonation.
        /// </summary>
        /// <param name="userName">The name of the user to act as.</param>
        /// <param name="domainName">The domain name of the user to act as.</param>
        /// <param name="password">The password of the user to act as.</param>
        private void ImpersonateValidUser(
            string userName,
            string domain,
            string password,
            string path)
        {
            if (path.ToLower().Contains(":") == false)
            {
                WindowsIdentity tempWindowsIdentity = null;
                IntPtr token = IntPtr.Zero;
                IntPtr tokenDuplicate = IntPtr.Zero;

                try
                {
                    if (RevertToSelf())
                    {
                        if (LogonUser(
                            userName,
                            domain,
                            password,
                            LOGON32_LOGON_INTERACTIVE,
                            LOGON32_PROVIDER_DEFAULT,
                            ref token) != 0)
                        {
                            if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                            {
                                tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                                impersonationContext = tempWindowsIdentity.Impersonate();
                            }
                            else
                            {
                                throw new Win32Exception(Marshal.GetLastWin32Error());
                            }
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                finally
                {
                    if (token != IntPtr.Zero)
                    {
                        CloseHandle(token);
                    }
                    if (tokenDuplicate != IntPtr.Zero)
                    {
                        CloseHandle(tokenDuplicate);
                    }
                }
            }
        }

        /// <summary>
        /// Reverts the impersonation.
        /// </summary>
        private void UndoImpersonation()
        {
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
            }
        }

        private WindowsImpersonationContext impersonationContext = null;

        // ------------------------------------------------------------------
        #endregion
    }

    public class FileData
    {
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string Icono { get; set; }
        public string Extension { get; set; }
        public string FechaModificacion { get; set; }
        public string LastWriteTime { get; set; }
        public bool EsTemporal { get; set; }
    }

    /// <summary>
    /// Clase para el manejo
    /// </summary>
    public class BreadCrumb
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }
}
