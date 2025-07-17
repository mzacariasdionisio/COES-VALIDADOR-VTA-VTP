using COES.Framework.Base.Tools;
using GAMS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Yupana.Helper
{
    public class FileHelper
    {
        public static string UserLogin = ConfigurationManager.AppSettings["UserFS"];
        public static string Domain = ConfigurationManager.AppSettings["DomainFS"];
        public static string Password = ConfigurationManager.AppSettings["PasswordFS"];
        /// <summary>
        /// Permite generar el archivo de entrada GAMS
        /// </summary>
        public static bool GenerarArchivo(string fileName, string path, string texto)
        {
            try
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    // Reemplazar por FilerServer
                    //using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    using (System.IO.StreamWriter file = OpenWriterFile(fileName, path))
                    {
                        file.Write(texto);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
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
            string LocalDirectory = "";// GetDirectory();
            if (!string.IsNullOrEmpty(pathAlternativo)) LocalDirectory = pathAlternativo;

            using (new Impersonator(UserLogin, Domain, Password, LocalDirectory))
            {
                StreamWriter stream = new StreamWriter(LocalDirectory + sourceBlobName);

                return stream;
            }
        }

        public static bool GenerarArchivoByte(string fileName, string path, byte[] archivo)
        {
            try
            {
                string archEnc = path +  fileName;
                using (var file = new FileStream(archEnc, FileMode.Create))
                {
                    file.Write(archivo, 0, archivo.Count());
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite crear un directorio en un carpeta determinada
        /// </summary>
        /// <param name="url"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool CreateFolder2(string url, string folderName)
        {
            using (new Impersonator(UserLogin, Domain, Password, ""))
            {
                if (!string.IsNullOrEmpty(url)) url = url + "/";
                string path = url + folderName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Permite copiar un archivo
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="destino"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static int CopiarFile(string origen, string destino, string fileOrigen,string filedestino)
        {
            try
            {

                using (new Impersonator(UserLogin, Domain, Password, ""))
                {
                    string pathOrigen =  origen + fileOrigen;
                    string pathDestino = destino + filedestino;

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

        public static string LeerLogGamsDivergeConverge(string path,string fileName)
        {
            string sLine = "";
            string salida = string.Empty;
            string contenido = string.Empty;
            string archivo = path + fileName;
            using (new Impersonator(UserLogin, Domain, Password, ""))
            {
                using (var fs = new FileStream(archivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader fileSeek = new StreamReader(fs, Encoding.Default))
                {
                    while ((sLine = fileSeek.ReadLine()) != null)
                    {
                        if (sLine.Contains(ConstantesYupana.GamsConverge))
                        {
                            salida = ConstantesYupana.Converge;
                            break;
                        }
                        if (sLine.Contains(ConstantesYupana.GamsDiverge))
                        {
                            salida = ConstantesYupana.Diverge;
                            break;
                        }
                    }
                }
            }
            return salida;
        }

        public static void RunGams(ref GAMSJob resultado, GAMSDatabase resultDB,string path, string fileName)
        {
            using (new Impersonator(UserLogin, Domain, Password, ""))
            {
                using (TextWriter writer = File.CreateText(path +  fileName))
                    resultado.Run(output: writer, databases: resultDB);
            }
        }

    }
}
