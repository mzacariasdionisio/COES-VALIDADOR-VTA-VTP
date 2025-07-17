using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.PotenciaFirme;
using GAMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper.EntidadesAdicionales;

namespace COES.Servicios.Aplicacion.PotenciaFirmeRemunerable.Helper
{
    public class FileHelper
    {
        public static string UserLogin = "admin";
        public static string Domain = "";
        public static string Password = "JavaScripto2020";
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
                    using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    //using (System.IO.StreamWriter file = OpenWriterFile(fileName, path))
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
        public static StreamWriter OpenWriterFile2(string sourceBlobName, string pathAlternativo)
        {
        string LocalDirectory = "";// GetDirectory();
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
        public static StreamReader OpenReaderFile2(string sourceBlobName, string pathAlternativo)
        {
            string LocalDirectory = "";//GetDirectory();
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
        /// Permite crear un directorio en un carpeta determinada
        /// </summary>
        /// <param name="url"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool CreateFolder2(string url, string folderName, string pathAlternativo)
        {
            string LocalDirectory = "";// GetDirectory();
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
        /// Establece la carpeta de trabajo de la corrida
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="pathRaiz"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public static string EstablecerCarpetaTrabajo(DateTime fechaProceso, string pathRaiz,string version ,int correlativo)
        {
            //- Seteamos la carpeta correspondiente al dia
            string pathMes = fechaProceso.Year + @"\" +
                      fechaProceso.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesPotenciaFirmeRemunerable.CaracterCero)) + @"\";

            //- Ruta de los archivos de la corrdida
            string pathCorrida = pathMes + version + @"\" + ConstantesPotenciaFirmeRemunerable.FolderEscenario + correlativo + @"\";

            //- Creamos la carpeta de trabajo de la corrida -- reemplazar por FileServer
            //CreateFolder(pathMes + version + @"\", ConstantesPotenciaFirmeRemunerable.FolderEscenario + correlativo, pathRaiz);
            FileServer.CreateFolder(pathMes + version + @"\", ConstantesPotenciaFirmeRemunerable.FolderEscenario + correlativo, pathRaiz);


            //- Seteamos la carpeta correspondiente a la corrida
            string pathTrabajo = pathCorrida;

            return pathTrabajo;
        }

        /// <summary>
        /// Permite leer los costos marginales del archivo de resultado
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<SalidaGams> ObtenerResultadoGams(string fileName,int idEscenario, string path)
        { 
            List<SalidaGams> list = new List<SalidaGams>();

            try
            {
                // Reemplazar por FileServer
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                //using (StreamReader reader = OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = true;
                    int seccion = 0;
                    int i = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        i++;
                        if (!string.IsNullOrEmpty(line))
                        {
                            contador = 0;
                            switch (line)
                            {
                                case ConstantesPotenciaFirmeRemunerable.InicioResultadoGamsPotGenerada:
                                    flag = true;
                                    seccion = 1;
                                    break;

                                case ConstantesPotenciaFirmeRemunerable.InicioResultadoGamsCongSimple:
                                    seccion = 2;
                                    flag = true;
                                    break;
                                case ConstantesPotenciaFirmeRemunerable.InicioResultadoGamsCongCompuesta:
                                    seccion = 3;
                                    flag = true;
                                    break;
                                default:
                                    if (seccion > 0) flag = false;
                                    break;
                            }
                        }
                        else
                        {
                            seccion = 0;
                            contador++;
                            if (contador > 1) break;
                        }
                        if (!flag)
                        {
                            string[] arreglo = line.Split(ConstantesPotenciaFirmeRemunerable.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();
                            SalidaGams item = new SalidaGams();
                            decimal total = 0;
                            switch (seccion)
                            {
                                case 1:

                                    break;
                                case 2:
                                    item.Id = arreglo[0].Replace("\"", "").Trim();
                                    item.Escenariocodi = idEscenario;
                                    if (decimal.TryParse(arreglo[2], out total)) { }
                                    item.Valor = total;
                                    item.Tipo = (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion;
                                    list.Add(item);
                                    break;
                                case 3:
                                    item.Id = arreglo[0].Replace("\"", "").Trim();
                                    item.Escenariocodi = idEscenario;
                                    if (decimal.TryParse(arreglo[2], out total)) { }
                                    item.Valor = total;
                                    item.Tipo = (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Congestion;
                                    list.Add(item);

                                    break;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return list;
        }

        /// <summary>
        /// Permite leer los Voltajes del archivo de resultado GDX
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<SalidaGams> ObtenerResultadoGamsV(GAMSJob modelo, string path,int idEscenario)
        {
            List<SalidaGams> list = new List<SalidaGams>();
            GAMSDatabase db2 = modelo.Workspace.AddDatabaseFromGDX(path + ConstantesPotenciaFirmeRemunerable.ArchivoGdx);
           
            //Salidas V
            GAMSVariable listaVariable = db2.GetVariable("V"); 
            foreach (GAMSVariableRecord rec in listaVariable)
            {
                SalidaGams item = new SalidaGams();
                item.Id = rec.Key(0);
                item.Escenariocodi = idEscenario;
                item.Valor = (decimal)rec.Level;
                item.Tipo = (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.V;
                list.Add(item);
            }

            //Salidas PG
            GAMSVariable listaVariablePg = db2.GetVariable("Pg");
            foreach (GAMSVariableRecord rec in listaVariablePg)
            {
                SalidaGams item = new SalidaGams();
                item.Id = rec.Key(0);
                item.Escenariocodi = idEscenario;
                item.Valor = (decimal)rec.Level * ConstantesPotenciaFirmeRemunerable.PBase; 
                item.Tipo = (int)ConstantesPotenciaFirmeRemunerable.SalidasGams.Pg;
                list.Add(item);
            }

            return list;
        }

    }
}
