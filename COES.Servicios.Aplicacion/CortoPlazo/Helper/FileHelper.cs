using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.CortoPlazo.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// Permite obtener el nombre de archivo PSSE
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static bool ObtenerArchivoRaw(string filePsse, string pathTrabajo, string pathRaiz, DateTime fechaProceso, int indicadorPSSE)
        {
            string fileResult = string.Empty;
            //- Definimos la origen de los archivos .raw
            string pathExportacion = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathExportacionPSSE];

            if (indicadorPSSE == 0)
            {
                //- Leemos de la carpeta respectiva

                if (!(DateTime.Now.Year == fechaProceso.Year && DateTime.Now.Month == fechaProceso.Month))
                {
                    pathExportacion = pathExportacion + fechaProceso.Month.ToString().PadLeft(2, '0') + fechaProceso.Year.ToString() + @"\";
                }                

                //- Obtenemos los archivos .raw en base al rango de fechas
                List<FileData> list = FileServer.ObtenerArchivosPorCriterio(string.Empty, ConstantesCortoPlazo.ExtensionRaw,
                    fechaProceso.AddMinutes(-30), fechaProceso, pathExportacion);

                if (list.Count > 0)
                {
                    string file = list[0].FileName;
                    //- Copiando y renombrando los archivos utilizados
                    FileServer.CopiarFileAlter(pathExportacion, pathTrabajo, file, pathRaiz);
                    FileServer.RenameBlob(pathTrabajo, file, filePsse, pathRaiz);

                    return true;
                }
            }
            else 
            {
                pathExportacion = pathExportacion + @"Reproceso\";
                string fileName = "psse_reproceso.raw";
                FileServer.CopiarFileAlter(pathExportacion, pathTrabajo, fileName, pathRaiz);
                FileServer.RenameBlob(pathTrabajo, fileName, filePsse, pathRaiz);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Permite obtener el nombre de archivo PSSE
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static bool ObtenerArchivoRawGeneracion(string filePsse, string pathTrabajo, string pathRaiz, DateTime fechaProceso)
        {
            string fileResult = string.Empty;
            //- Definimos la origen de los archivos .raw
            string pathExportacion = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathExportacionPSSE];

            //- Obtenemos los archivos .raw en base al rango de fechas
            List<FileData> list = FileServer.ObtenerArchivosPorCriterio(string.Empty, ConstantesCortoPlazo.ExtensionRaw,
                fechaProceso.AddMinutes(-5), fechaProceso, pathExportacion);

            if (list.Count > 0)
            {
                if (FileServer.VerificarExistenciaFile(pathTrabajo, filePsse, pathRaiz))
                {
                    FileServer.DeleteBlob(filePsse, pathRaiz);
                }

                string file = list[0].FileName;
                //- Copiando y renombrando los archivos utilizados
                FileServer.CopiarFileAlter(pathExportacion, pathTrabajo, file, pathRaiz);
                FileServer.RenameBlob(pathTrabajo, file, filePsse, pathRaiz);

                return true;
            }

            return false;
        }


        /// <summary>
        /// Verificamos la existencia del archivo RAW
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static int VerificarExistenciaRaw(DateTime fechaProceso, out string pathRuta, out string filename)
        {
            int resultado = 0; //- No existe archivo RAW
            string pathExportacion = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathExportacionPSSE];
            pathRuta = pathExportacion;
            filename = string.Empty;

            List<FileData> list = FileServer.ObtenerArchivosPorCriterio(string.Empty, ConstantesCortoPlazo.ExtensionRaw,
                    fechaProceso.AddMinutes(-10), fechaProceso, pathExportacion);

            if (list.Count > 0)
            {
                filename = list[0].FileName;
                resultado = 1; //- Existe archivo raw
            }

            return resultado;
        }

        /// <summary>
        /// Obteniene la relación de códigos y los nombres correctos
        /// </summary>
        /// <param name="file">datos.raw</param>
        /// <returns></returns>
        public static List<NombreCodigoBarra> ObtenerBarras(string fileName, string path)
        {
            List<NombreCodigoBarra> list = new List<NombreCodigoBarra>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line = string.Empty;
                int contador = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (contador >= 3)
                    {
                        if (line == ConstantesCortoPlazo.IniEPPSCODE) break;

                        string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();
                        NombreCodigoBarra item = new NombreCodigoBarra();
                        item.CodBarra = arreglo[0].Trim();
                        item.NombBarra = arreglo[1].Replace("'", "").Trim();

                        string tension = arreglo[2];
                        decimal valor = 0;
                        if (tension != null)
                        {
                            valor = (decimal.TryParse(tension, out valor)) ? valor : 0;
                        }

                        item.Tension = valor.ToString(ConstantesCortoPlazo.FormatoNumero, CultureInfo.InvariantCulture);

                        item.TipoTension = Convert.ToDouble(arreglo[3]); //- Linea agregada

                        item.VoltajePU = Convert.ToDouble(arreglo[7]);
                        item.Angulo = Convert.ToDouble(arreglo[8]);

                        //- if voltaje PU  = 1 y angulo = 0, debe quitarse


                        list.Add(item);
                    }
                    contador++;
                }
            }

            return list;
        }

        /// <summary>
        /// Permite obtener las lineas del archivo ODMS
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="relacionBarra">relacion de Barra</param>
        /// <returns></returns>
        public static List<NombreCodigoLinea> ObtenerLineas(string fileName, string path, List<NombreCodigoBarra> relacionBarra)
        {
            List<NombreCodigoLinea> list = new List<NombreCodigoLinea>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line = string.Empty;

                List<string[]> listaBarra = ObtenerDatosEPPS(fileName, path, ConstantesCortoPlazo.IniDatosCongestion, ConstantesCortoPlazo.FinDatosCongestion);


                foreach (string[] arreglo in listaBarra)// while ((line = reader.ReadLine()) != null)
                {
                    //string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV);
                    NombreCodigoLinea item = new NombreCodigoLinea();
                    item.CodBarra1 = arreglo[0].Trim();
                    item.CodBarra2 = arreglo[1].Trim();
                    item.NombLinea = arreglo[2].Replace("'", "").Trim();
                    item.Rps = Convert.ToDouble(arreglo[3].Trim());
                    item.Xps = Convert.ToDouble(arreglo[4].Trim());
                    item.Bsh = Convert.ToDouble(arreglo[5].Trim());
                    item.GshP = Convert.ToDouble(arreglo[9].Trim());
                    item.BshP = Convert.ToDouble(arreglo[10].Trim());
                    item.GshS = Convert.ToDouble(arreglo[11].Trim());
                    item.BshS = Convert.ToDouble(arreglo[12].Trim());
                    item.BitEstado = Convert.ToInt32(arreglo[13].Trim());
                    item.Pot = Convert.ToDouble(arreglo[7].Trim());
                    
                    if(arreglo.Length > 15)
                    {
                        item.L = Convert.ToDouble(arreglo[15]);
                    }

                    //obtenienfo la tensionPU y ángulo de la barra 1 y 2

                    if (item.BitEstado == 1)
                    {
                        item.VoltajePU1 = relacionBarra.Where(x => x.CodBarra == item.CodBarra1).ToList()[0].VoltajePU;
                        item.Angulo1 = relacionBarra.Where(x => x.CodBarra == item.CodBarra1).ToList()[0].Angulo;

                        item.VoltajePU2 = relacionBarra.Where(x => x.CodBarra == item.CodBarra2).ToList()[0].VoltajePU;
                        item.Angulo2 = relacionBarra.Where(x => x.CodBarra == item.CodBarra2).ToList()[0].Angulo;

                        list.Add(item);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Permite obtener el listado para la creación de trafos
        /// </summary>
        /// <param name="fileName">nombre de archivo</param>
        /// <param name="relacionBarra"></param>
        /// <returns></returns>
        public static List<TrafoEms> ObtenerListadoTrafo(string fileName, string path, List<NombreCodigoBarra> relacionBarra)
        {
            try
            {
                List<TrafoEms> list = new List<TrafoEms>();

                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    List<string[]> listaBarra = ObtenerDatosEPPS(fileName, path, ConstantesCortoPlazo.FinDatosCongestion, ConstantesCortoPlazo.FinDatosTransformador);

                    //agregando datos al ultimo registro porque se requieren 5 filas al final
                    //listaBarra.Add(listaBarra[0]);

                    //for (int indice = 0; indice < listaBarra.Count; indice++)
                    int indice = 0;

                    while (indice < listaBarra.Count)
                    {
                        string[] arreglo = listaBarra[indice];

                        double[,] datoTrafo = new double[5, 20];
                        string nombre4 = listaBarra[indice][3].Replace("'", "").Trim();

                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < listaBarra[i + indice].Length; j++)
                            {
                                try
                                {
                                    string cad1 = listaBarra[i + indice][j];

                                    if (cad1.IndexOf("'") < 0)
                                    {
                                        datoTrafo[i, j] = Convert.ToDouble(Decimal.Parse(listaBarra[i + indice][j], NumberStyles.Any, CultureInfo.InvariantCulture));
                                    }
                                    else
                                    {
                                        datoTrafo[i, j] = 0;
                                    }
                                }
                                catch
                                {
                                    datoTrafo[i, j] = 0;
                                }
                            }
                        }

                        if (datoTrafo[0, 11] == 1)//-Linea agregada
                        {

                            double nivelTension1 = Convert.ToDouble(relacionBarra.Where(x => x.CodBarra == arreglo[0].Trim()).ToList()[0].Tension);
                            double nivelTension2 = Convert.ToDouble(relacionBarra.Where(x => x.CodBarra == arreglo[1].Trim()).ToList()[0].Tension);

                            double tension1 = relacionBarra.Where(x => x.CodBarra == arreglo[0].Trim()).ToList()[0].VoltajePU;
                            double angulo1 = relacionBarra.Where(x => x.CodBarra == arreglo[0].Trim()).ToList()[0].Angulo;

                            double tension2 = relacionBarra.Where(x => x.CodBarra == arreglo[1].Trim()).ToList()[0].VoltajePU;
                            double angulo2 = relacionBarra.Where(x => x.CodBarra == arreglo[1].Trim()).ToList()[0].Angulo;

                            double nivelTension3 = arreglo[2].Trim() == "0" ? 0 : Convert.ToDouble(relacionBarra.Where(x => x.CodBarra == arreglo[2].Trim()).ToList()[0].Tension);
                            double tension3 = arreglo[2].Trim() == "0" ? 0 : relacionBarra.Where(x => x.CodBarra == arreglo[2].Trim()).ToList()[0].VoltajePU;
                            double angulo3 = arreglo[2].Trim() == "0" ? 0 : relacionBarra.Where(x => x.CodBarra == arreglo[2].Trim()).ToList()[0].Angulo;


                            TrafoEms trafo = new TrafoEms(datoTrafo, nombre4, nivelTension1, nivelTension2, tension1, angulo1, tension2, angulo2, tension3, angulo3, nivelTension3, string.Empty);

                            list.Add(trafo);

                        }

                        bool esTrafo2D = (arreglo[2].Trim() == "0");

                        if (esTrafo2D)
                        {
                            //se procesan 3 filas más
                            indice += 4;
                        }
                        else
                        {
                            //se procesan 4 filas más
                            indice += 5;
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener estructura de datos desde el archivo
        /// de separador1 a separador2
        /// </summary>
        /// <param name="fileName">datos.raw</param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosEPPS(string fileName, string path, string separador1, string separador2)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == separador1) flag = true;
                    if (line == separador2) break;
                    if (flag)
                    {
                        list.Add(line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray());
                    }
                }
            }

            if (list.Count > 0) list.RemoveAt(0);
            return list;
        }

        /// <summary>
        /// Obtener estructura de datos desde el archivo
        /// 1 Barra, 2 ID, 3 Potencia Máxima, 14 indicador operacion
        /// </summary>
        /// <param name="fileName">datos.raw</param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosEPPS(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ConstantesCortoPlazo.IniEPPS) flag = true;
                    if (line == ConstantesCortoPlazo.FinEPPS) break;
                    if (flag)
                    {
                        list.Add(line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray());
                    }
                }
            }

            if (list.Count > 0) list.RemoveAt(0);
            return list;
        }

        /// <summary>
        /// Obtener estructura de datos de las centrales hidraulicas para la potencia maxima
        /// 1: Periodo, 2-n nombre de centrales
        /// </summary>
        /// <param name="fileName">oppchgcp.csv</param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosCentralesHidraulicas(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(ConstantesCortoPlazo.TextoInicioCH)) flag = true;

                    if (flag)
                    {
                        string[] linea = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();
                        for (int i = 0; i < linea.Count(); i++)
                        {
                            linea[i] = linea[i].Trim();
                        }

                        list.Add(linea);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Lectura de equivalencias entre barras y centrales
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string[]> ObtenerEquivalenciaBarraCentral(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line;
                int count = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    if (count > 0)
                    {
                        try
                        {
                            List<string> str = new List<string>();

                            string barra = line.Trim().Substring(8, 13);
                            string central = line.Trim().Substring(35, 24);

                            if (!string.IsNullOrEmpty(central))
                            {
                                if (!string.IsNullOrEmpty(central.Trim()))
                                {
                                    str.Add(barra.Trim());
                                    str.Add(central.Trim());
                                    list.Add(str.ToArray());
                                }
                            }
                        }
                        catch
                        {
                            //string li = line;
                        }
                    }

                    count++;
                }
            }

            return list;
        }

        /// <summary>
        /// Permite obtener la equivalencia
        /// </summary>
        /// <param name="codigoncp"></param>
        /// <returns></returns>
        public static string ObtenerEquivalenciaCentral(string codigoncp, List<string[]> equivalencias)
        {
            string result = string.Empty;

            foreach (string[] item in equivalencias)
            {
                if (codigoncp.Contains("ñ")) codigoncp = codigoncp.Replace("ñ", "�");

                if (item[1].Trim().ToLower() == codigoncp.Trim().ToLower())
                {
                    result = item[0];
                }
            }

            return result;
        }


        /// <summary>
        /// Pemite obtener los datos para las potencias hidraulicas 
        /// </summary>
        /// <param name="fileName">cHidroPE.dat</param>
        /// <returns></returns>
        public static List<string[]> ObtenerPotenciasHidraulico(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line;
                int count = 0;
                List<string> str = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (count > 0)
                    {
                        string cad = line.Substring(0, 91);
                                                
                        //1.268   1.602 

                        //1: codigo, 2: Nombre, 3: VMin, 4:VMax
                        string codi = cad.Substring(0, 5).Trim();
                        string nombre = cad.Substring(5, 14).Trim();
                        string vmin = cad.Substring(75, 8).Trim();
                        string vmax = cad.Substring(83, 8).Trim();
                        
                        string[] item = { codi, nombre, vmin, vmax};

                        list.Add(item);
                    }

                    count++;
                }
            }

            return list;
        }


        /// <summary>
        /// Pemite obtener los datos para las potencias hidraulicas 
        /// </summary>
        /// <param name="fileName">cHidroPE.dat</param>
        /// <returns></returns>
        public static decimal ObtenerMaximoCostosIncrementales(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();
            decimal maximo = -10000;
            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line;
                int count = 0;
                List<string> str = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (count > 3)
                    {
                        string[] registro = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                        if (registro.Length > 3)
                        {
                            string dato = registro[3];
                            decimal valor = 0;

                            if (decimal.TryParse(dato, out valor))
                            {
                                if (valor > maximo)
                                {
                                    maximo = valor;
                                }
                            }
                        }

                    }

                    count++;
                }
            }

            return maximo;
        }

        /// <summary>
        /// Permite obtener los datos de demanda en barras
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosDemanda(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ConstantesCortoPlazo.IniDemandaBarra)
                        flag = true;
                    if (line == ConstantesCortoPlazo.FinDemandaBarra) break;
                    if (flag)
                    {
                        string[] registro = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                        if (registro.Length > 2)
                        {

                            if (registro[2] == ConstantesCortoPlazo.CampoActivo)
                            {
                                string[] demanda = new string[4];
                                demanda[0] = registro[0].Trim();  //codigo
                                demanda[1] = registro[5].Trim();  //carga1 barraPc
                                demanda[2] = registro[6].Trim();  //carga2 barraQc
                                demanda[3] = registro[2].Trim();  //connectado
                                list.Add(demanda);
                            }
                        }
                    }
                }
            }

            //if (list.Count > 0) list.RemoveAt(0);
            return list;

        }


        /// <summary>
        /// Permite obtener los datos de shunt
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosShunt(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ConstantesCortoPlazo.FinDemandaBarra)
                        flag = true;
                    if (line == ConstantesCortoPlazo.IniEPPS) break;
                    if (flag)
                    {
                        string[] registro = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                        if (registro.Length > 2)
                        {
                            if (registro[2] == ConstantesCortoPlazo.CampoActivo)
                            {
                                string[] shunt = new string[4];
                                shunt[0] = registro[0];  //codigo
                                shunt[1] = registro[4]; //carga resistiva I
                                shunt[2] = registro[3]; //carga adicional R
                                shunt[3] = registro[2]; //conn
                                list.Add(shunt);
                            }
                        }
                    }
                }
            }

            //if (list.Count > 0) list.RemoveAt(0);
            return list;

        }


        /// <summary>
        /// Permite obtener los datos de shunt
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<string[]> ObtenerDatosShuntDinamicos(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                bool flag = false;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ConstantesCortoPlazo.InicioShuntDinamico)
                        flag = true;
                    if (line == ConstantesCortoPlazo.FinShuntDinamico) break;
                    if (flag)
                    {
                        string[] registro = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                        if (registro.Length > 2)
                        {
                            if (registro[3] == ConstantesCortoPlazo.CampoActivo)
                            {
                                string[] shunt = new string[6];
                                shunt[0] = registro[0]; //codigo
                                shunt[1] = registro[9]; //valor                                

                                if (registro[1] != "2")
                                {
                                    shunt[2] = registro[9];
                                    shunt[3] = registro[9];
                                }
                                else
                                {
                                    shunt[2] = registro[11];
                                    shunt[3] = (registro.Count() > 13) ? registro[13] : registro[9]; //max
                                }


                                shunt[4] = registro[3]; //conn
                                shunt[5] = registro[1];
                                list.Add(shunt);
                            }
                        }
                    }
                }
            }

            //if (list.Count > 0) list.RemoveAt(0);
            return list;

        }

        /// <summary>
        /// Permite generar el achivo requerido con los datos
        /// </summary>
        /// <param name="consumo">Lista de datos del consumo.</param>
        /// <param name="congestion">Lista de datos de la congestión.</param>
        /// <param name="generacionForzada">Lista de datos de generación forzada.</param>
        /// <param name="fileName">Nombre del archivo que se generará.</param>
        public static void GenerarArchivoResultado(List<RegistroGenerado> consumo, List<PrCongestionDTO> congestion,
            List<PrCongestionDTO> congestionConjunta, List<PrGenforzadaDTO> generacionForzada, string pathTrabajo, string fileName)
        {
            string texto = string.Empty;

            using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, pathTrabajo))
            {
                //Inicio de pintado de datos de consumo
                file.WriteLine(ConstantesCortoPlazo.HeaderConsumo);

                foreach (RegistroGenerado item in consumo)
                {
                    texto =
                        item.BarraNombre.PadRight(12, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.Tension.PadRight(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.GenerID.PadLeft(3, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.Tipo.PadLeft(3, ' ') + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Potencia.ToString()) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMaxima.ToString()) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.PotenciaMinima.ToString()) + ConstantesCortoPlazo.CaracterTab +
                        item.IndOpe.ToString().PadLeft(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValor(item.Costo1.ToString()) + ConstantesCortoPlazo.CaracterTab +
                        item.IndNcv.PadLeft(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.Cod.ToString().PadLeft(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Pmax1) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Ci1) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Pmax2) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Ci2) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Pmax3) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Ci3) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Pmax4) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Ci4) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Pmax5) + ConstantesCortoPlazo.CaracterTab +
                        UtilCortoPlazo.FormatearValorAdicional(item.Ci5);

                    file.WriteLine(texto);
                }

                //Inicio de pintado de los datos de congestión de las barras
                file.WriteLine(ConstantesCortoPlazo.InicioDatosCongestion);

                foreach (PrCongestionDTO item in congestion)
                {
                    texto =
                        item.Nodobarra1.PadRight(12, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.Nodobarra2.PadRight(12, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.NombLinea.PadRight(3, ' ') + ConstantesCortoPlazo.CaracterTab +
                        item.Flujo.ToString(ConstantesCortoPlazo.FormatoDecimal).PadRight(8, ' ');

                    file.WriteLine(texto);
                }

                //Incio de pintado de los datos de congestión de conjunto de líneas
                file.WriteLine(ConstantesCortoPlazo.InicioDatosCongestionGrupo);

                foreach (PrCongestionDTO itemCongestion in congestionConjunta)
                {
                    texto =
                        itemCongestion.Flujo.ToString(ConstantesCortoPlazo.FormatoDecimal).PadRight(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                        itemCongestion.ListaItems.Count + ConstantesCortoPlazo.CaracterTab;

                    foreach (PrCongestionitemDTO item in itemCongestion.ListaItems)
                    {
                        texto = texto +
                            item.Nombarra1.Trim().PadRight(12, ' ') + ConstantesCortoPlazo.CaracterTab +
                            item.Nombarra2.Trim().PadRight(12, ' ') + ConstantesCortoPlazo.CaracterTab +
                            item.NombLinea.ToString().PadLeft(3, ' ') + ConstantesCortoPlazo.CaracterTab;
                    }

                    file.WriteLine(texto);
                }

                //Inicio de pintado de los datos de generación forzada
                file.WriteLine(ConstantesCortoPlazo.InicioGeneracionForzada);

                int contador = 0;
                try
                {

                    foreach (PrGenforzadaDTO item in generacionForzada)
                    {
                        if (string.IsNullOrEmpty(item.Subcausacmg)) item.Subcausacmg = string.Empty;
                        contador++;

                        if (contador > 0)
                        {

                            texto = ConstantesCortoPlazo.CaracterIgual +
                                item.Suma.ToString(ConstantesCortoPlazo.FormatoDecimal).PadRight(8, ' ') + ConstantesCortoPlazo.CaracterTab +
                                item.Subcausacmg.PadRight(5, ' ') + ConstantesCortoPlazo.CaracterTab +
                                item.Cantidad + ConstantesCortoPlazo.CaracterTab;

                            if (item.ListaItems != null)
                            {
                                foreach (PrgenforzadaitemDTO generador in item.ListaItems)
                                {
                                    texto = texto +
                                        generador.Nombarra.PadRight(12, ' ') + ConstantesCortoPlazo.CaracterSeperacionDoble +
                                        generador.Tension.PadRight(6, ' ') + ConstantesCortoPlazo.CaracterSeperacionDoble +
                                        generador.Idgenerador + ConstantesCortoPlazo.CaracterTab;
                                }
                            }
                            file.WriteLine(texto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    int k = contador;
                }

                file.WriteLine(ConstantesCortoPlazo.FinArchivo);
            }
        }

        /// <summary>
        /// Permite generar el archivo de entrada GAMS
        /// </summary>
        public static bool GenerarArchivoEntradaGams(string fileName, string path, string texto)
        {
            try
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    {
                        file.Write(texto);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite generar el archivo de entrada GAMS
        /// </summary>
        public static bool GenerarArchivoGams(string fileName, string path, string texto)
        {
            try
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    {
                        file.Write(texto);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite generar el archivo de entrada GAMS
        /// </summary>
        public static bool GenerarArchivoLog(string fileName, string path, string texto)
        {
            try
            {
                if (!string.IsNullOrEmpty(texto))
                {
                    using (System.IO.StreamWriter file = FileServer.OpenWriterFile(fileName, path))
                    {
                        file.Write(texto);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Permite leer los costos marginales del archivo de resultado
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<ResultadoGams> ObtenerResultadoGams(string fileName, string path)
        {
            List<ResultadoGams> list = new List<ResultadoGams>();

            try
            {
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == ConstantesCortoPlazo.InicioResultadoGams)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            contador = contador + 1;
                        }

                        if (contador > 3)
                        {
                            if (line != string.Empty)
                            {
                                string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                if (arreglo.Length == 4)
                                {
                                    ResultadoGams item = new ResultadoGams();
                                    item.Nombarra = arreglo[0].Replace("\"", "").Trim();

                                    decimal energia = 0;
                                    decimal congestion = 0;
                                    decimal total = 0;

                                    if (decimal.TryParse(arreglo[1], out energia)) { }
                                    if (decimal.TryParse(arreglo[2], out congestion)) { }
                                    if (decimal.TryParse(arreglo[3], out total)) { }

                                    item.Energia = energia;
                                    item.Congestion = congestion;
                                    item.Total = total;

                                    list.Add(item);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
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
        /// Permite leer la estructua de congestiones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="path">Ruta donde estan ubicados los archivos NCP</param>
        /// <returns></returns>
        public List<RelacionCongestion> ObtenerDatosCongestion(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();
            List<RelacionCongestion> result = new List<RelacionCongestion>();
            List<RelacionCongestion> final = new List<RelacionCongestion>();

            //- Leemos la estructura del archivo

            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                if (reader != null)
                {
                    bool flag = false;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(ConstantesCortoPlazo.TextoInicioCH)) flag = true;

                        if (flag)
                        {
                            list.Add(line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray());
                        }
                    }
                }
            }

            //- Obtenemos los tramos donde existe congestión
            string[] cabecera = list[0];

            int longitud = list.Count() - 1;
            int restante = 48 - longitud;

            for (int i = 1; i < list.Count(); i++)
            {
                string periodo = list[i][0];

                if (list[i].Count() > 3)
                {
                    for (int j = 3; j < list[i].Count(); j++)
                    {
                        decimal valor = 0;

                        if (decimal.TryParse(list[i][j], out valor))
                        {
                            //- Indica si existe congestion programada
                            if (valor != 0)
                            {
                                RelacionCongestion item = new RelacionCongestion
                                {
                                    Periodo = int.Parse(periodo) + restante,
                                    CodigoNCP = cabecera[j]
                                };

                                result.Add(item);
                            }
                        }
                    }
                }
            }

            //- Obtenemos el rango de horas con congestión
            List<string> codigos = result.Select(x => x.CodigoNCP).Distinct().ToList();

            List<RelacionCongestion> horas = new List<RelacionCongestion>();
            for (int i = 1; i <= 48; i++)
                horas.Add(new RelacionCongestion { Periodo = i });

            foreach (string codigo in codigos)
            {
                List<RelacionCongestion> periodos = result.Where(x => x.CodigoNCP == codigo).OrderBy(x => x.Periodo).ToList();

                foreach (RelacionCongestion item in horas)
                {
                    if (periodos.Where(x => x.Periodo == item.Periodo).Count() > 0)
                        item.Indicador = ConstantesAppServicio.SI;
                    else
                        item.Indicador = ConstantesAppServicio.NO;
                }

                //- Por cada codigo NCP vemos los tramos congestionados

                int periodoInicio = 1;
                int periodoFin = 1;
                bool flag = true;
                bool flagBloque = false;
                for (int k = 1; k <= 48; k++)
                {
                    if (horas[k - 1].Indicador == ConstantesAppServicio.SI)
                    {
                        if (flag)
                        {
                            periodoInicio = k;
                        }
                        periodoFin = k;
                        flag = false;
                        flagBloque = true;

                        if (k == 48)
                        {
                            RelacionCongestion congestion = new RelacionCongestion
                            {
                                CodigoNCP = codigo,
                                PeriodoInicio = periodoInicio,
                                PeriodoFin = periodoFin
                            };

                            final.Add(congestion);
                        }
                    }
                    else
                    {
                        if (flagBloque)
                        {
                            //llenamos inicio y fin
                            flag = true;
                            flagBloque = false;

                            RelacionCongestion congestion = new RelacionCongestion
                            {
                                CodigoNCP = codigo,
                                PeriodoInicio = periodoInicio,
                                PeriodoFin = periodoFin
                            };

                            final.Add(congestion);
                        }
                    }
                }

            }

            return final;
        }

        /// <summary>
        /// Permite leer la estructua de congestiones
        /// </summary>
        /// <param name="fileName">Nombre del archivo</param>
        /// <param name="path">Ruta donde estan ubicados los archivos NCP</param>
        /// <returns></returns>
        public List<RelacionModoOperacion> ObtenerDatoModosOperacion(string fileName, string path)
        {
            List<string[]> list = new List<string[]>();
            List<RelacionModoOperacion> result = new List<RelacionModoOperacion>();
            List<RelacionModoOperacion> final = new List<RelacionModoOperacion>();

            //- Leemos la estructura del archivo

            using (StreamReader reader = FileServerScada.OpenReaderFile(fileName, path))
            {
                if (reader != null)
                {
                    bool flag = false;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(ConstantesCortoPlazo.TextoInicioCH)) flag = true;

                        if (flag)
                        {
                            list.Add(line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray());
                        }
                    }
                }
            }

            //- Obtenemos los tramos donde existe congestión
            string[] cabecera = list[0];

            for (int i = 1; i < list.Count(); i++)
            {
                string periodo = list[i][0];

                if (list[i].Count() > 3)
                {
                    for (int j = 3; j < list[i].Count(); j++)
                    {
                        decimal valor = 0;

                        if (decimal.TryParse(list[i][j], out valor))
                        {
                            //- Indica si existe congestion programada
                            if (valor != 0)
                            {
                                RelacionModoOperacion item = new RelacionModoOperacion
                                {
                                    Periodo = int.Parse(periodo),
                                    CodigoNCP = cabecera[j]
                                };

                                result.Add(item);
                            }
                        }
                    }
                }
            }

            //- Obtenemos el rango de horas con congestión
            List<string> codigos = result.Select(x => x.CodigoNCP).Distinct().ToList();

            List<RelacionModoOperacion> horas = new List<RelacionModoOperacion>();
            for (int i = 1; i <= 48; i++)
                horas.Add(new RelacionModoOperacion { Periodo = i });

            foreach (string codigo in codigos)
            {
                List<RelacionModoOperacion> periodos = result.Where(x => x.CodigoNCP == codigo).OrderBy(x => x.Periodo).ToList();

                foreach (RelacionModoOperacion item in horas)
                {
                    if (periodos.Where(x => x.Periodo == item.Periodo).Count() > 0)
                        item.Indicador = ConstantesAppServicio.SI;
                    else
                        item.Indicador = ConstantesAppServicio.NO;
                }

                //- Por cada codigo NCP vemos los tramos congestionados

                int periodoInicio = 1;
                int periodoFin = 1;
                bool flag = true;
                bool flagBloque = false;
                for (int k = 1; k <= 48; k++)
                {
                    if (horas[k - 1].Indicador == ConstantesAppServicio.SI)
                    {
                        if (flag)
                        {
                            periodoInicio = k;
                        }
                        periodoFin = k;
                        flag = false;
                        flagBloque = true;

                        if (k == 48)
                        {
                            RelacionModoOperacion congestion = new RelacionModoOperacion
                            {
                                CodigoNCP = codigo,
                                PeriodoInicio = periodoInicio,
                                PeriodoFin = periodoFin
                            };
                            final.Add(congestion);
                        }
                    }
                    else
                    {
                        if (flagBloque)
                        {
                            //llenamos inicio y fin
                            flag = true;
                            flagBloque = false;

                            RelacionModoOperacion congestion = new RelacionModoOperacion
                            {
                                CodigoNCP = codigo,
                                PeriodoInicio = periodoInicio,
                                PeriodoFin = periodoFin
                            };
                            final.Add(congestion);
                        }
                    }
                }

            }

            return final;
        }
        
        /// <summary>
        /// Permite obtener la carpeta de trabajo PSSE
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static string ObtenerArchivoNCP(DateTime fechaProceso)
        {
            //- Definimos la carpeta de trabajo temporal
            string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];

            //- Definimos la carpeta de trabajo de CMgN
            string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

            //- Verificamos que exista la carpeta para los archivos NCP
            string folderNCP = @"\" + fechaProceso.Year + @"\" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";
            FileServer.CreateFolder(string.Empty, folderNCP, pathTrabajo);

            //- Verificamos la existencia de los archivos
            bool flagFile = true;

            //- Listamos los archivos necesarios para el proceso
            List<string> files = ObtenerFilesNCP();

            foreach (var file in files)
            {
                if (!FileServerScada.VerificarExistenciaFile(folderNCP, file, pathTrabajo))
                {
                    flagFile = false;
                }
            }

            if (!flagFile)
            {
                //- Eliminamos el contenido de la carpeta temporal
                DirectoryInfo di = new DirectoryInfo(pathTemporal);

                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo si in di.GetDirectories())
                {
                    si.Delete(true);
                }

                //- Determinamos la ruta del archivo comprimido NCP

                //string pathRootNCP = @"\\fs\Areas\SPR\2-DESPACHO\1-PDO\";
                string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";

                //string pathFuenteNCP = fechaProceso.Year +
                //    @"\Sem" + EPDate.f_numerosemana(fechaProceso).ToString().PadLeft(2, '0') + fechaProceso.Year.ToString().Substring(2, 2) +
                //    @"\PDO_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                //    @"\5-Final\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                //    @".zip";

                string pathFuenteNCP = @"Operación\Programa de Operación\Programa Diario\" + fechaProceso.Year + @"\" + fechaProceso.Month.ToString().PadLeft(2, '0') + @"_" +
                   COES.Base.Tools.Util.ObtenerNombreMes(fechaProceso.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProceso.Day.ToString().PadLeft(2, '0') +
                    @"\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                    @".zip";

                //- Extraemos en una nueva carpeta el contenido del ZIP
                FileServer.DescomprimirZip(pathFuenteNCP, pathTemporal, pathRootNCP);

                //- Definimos la ruta de origen donde se copiaran los archivos
                string pathSource = pathTemporal + "NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";

                //- Copiamos los archivos necesarios
                foreach (var file in files)
                {
                    FileServer.CopiarFileAlter(pathSource, folderNCP, file, pathTrabajo);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Permite obtener la carpeta de trabajo PSSE
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static string ObtenerArchivoNCPrograma(DateTime fechaProceso)
        {
            //- Definimos la carpeta de trabajo temporal
            string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];

            //- Definimos la carpeta de trabajo de CMgN
            string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

            DateTime fechaProcesoOrigen;

            if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
            {
                fechaProcesoOrigen = (new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day)).AddDays(-1).AddHours(23).AddMinutes(59);
                //fechaProcesoOrigen = fechaProceso;
            }
            else
            {
                fechaProcesoOrigen = fechaProceso;
            }

            //- Verificamos que exista la carpeta para los archivos NCP
            string folderNCP = @"\" + fechaProceso.Year + @"\" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";
            FileServerScada.CreateFolder(string.Empty, folderNCP, pathTrabajo);

            //- Verificamos la existencia de los archivos
            bool flagFile = true;

            //- Listamos los archivos necesarios para el proceso
            List<string> files = ObtenerFilesNCP();

            //foreach (var file in files)
            //{
            //    if (!FileServer.VerificarExistenciaFile(folderNCP, file, pathTrabajo))
            //    {
            //        flagFile = false;
            //    }
            //}

            if (true)
            {
                //- Eliminamos el contenido de la carpeta temporal
                DirectoryInfo di = new DirectoryInfo(pathTemporal);

                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo si in di.GetDirectories())
                {
                    si.Delete(true);
                }

                //- Determinamos la ruta del archivo comprimido NCP

                //string pathRootNCP = @"\\fs\Areas\SPR\2-DESPACHO\1-PDO\";
                string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";

                //string pathFuenteNCP = fechaProcesoOrigen.Year +
                //    @"\Sem" + EPDate.f_numerosemana(fechaProcesoOrigen).ToString().PadLeft(2, '0') + fechaProcesoOrigen.Year.ToString().Substring(2, 2) +
                //    @"\PDO_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') +
                //    @"\5-Final\NCP_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') +
                //    @".zip";

                string pathFuenteNCP = @"Operación\Programa de Operación\Programa Diario\" + fechaProcesoOrigen.Year + @"\" + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') + @"_" +
                   COES.Base.Tools.Util.ObtenerNombreMes(fechaProcesoOrigen.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') +
                    @"\NCP_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') +
                    @".zip";                

                //- Extraemos en una nueva carpeta el contenido del ZIP
                FileServer.DescomprimirZip(pathFuenteNCP, pathTemporal, pathRootNCP);

                //- Definimos la ruta de origen donde se copiaran los archivos
                string pathSource = pathTemporal + "NCP_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') + @"\";

                //- Copiamos los archivos necesarios
                foreach (var file in files)
                {
                    FileServer.CopiarFileAlterFinal(pathSource, folderNCP, file, pathTrabajo);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Permitirá obtener los costos marginales programados
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static string[][] ObtenerCostosMarginalesProgramados(DateTime fechaProceso)
        {
            string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];
            
            //- Eliminamos el contenido de la carpeta temporal
            DirectoryInfo di = new DirectoryInfo(pathTemporal);

            foreach (FileInfo fi in di.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo si in di.GetDirectories())
            {
                si.Delete(true);
            }

            //- Establecemos ruta de donde se obtendrá los archivos NCP
            //string pathRootNCP = @"\\fs\Areas\SPR\2-DESPACHO\1-PDO\";
            string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";
            Tuple<int, int> semana_anho = EPDate.f_numerosemana_y_anho(fechaProceso);

            //string pathFuenteNCP = semana_anho.Item2 +
            //            @"\Sem" + semana_anho.Item1.ToString().PadLeft(2, '0') + semana_anho.Item2.ToString().Substring(2, 2) +
            //            @"\PDO_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
            //            @"\5-Final\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
            //            @".zip";

            string pathFuenteNCP = @"Operación\Programa de Operación\Programa Diario\" + fechaProceso.Year + @"\" + fechaProceso.Month.ToString().PadLeft(2, '0') + @"_" +
                  COES.Base.Tools.Util.ObtenerNombreMes(fechaProceso.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProceso.Day.ToString().PadLeft(2, '0') +
                   @"\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                   @".zip";

            //- Extraemos en una nueva carpeta el contenido del ZIP
            FileServer.DescomprimirZip(pathFuenteNCP, pathTemporal, pathRootNCP);

            string file = "cmgbuscp.csv";
            string url = "NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";

            if (FileServer.VerificarExistenciaFile(url, file, pathTemporal))
            {
                List<string[]> list = ObtenerDatosCentralesHidraulicas(url + file, pathTemporal);
                return list.ToArray();
            }                       

            return null;
        }

        /// <summary>
        /// Permitirá obtener los costos marginales programados
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static string[][] ObtenerRelacionBarraGenerador(DateTime fechaProceso)
        {
            string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];

            //- Eliminamos el contenido de la carpeta temporal
            DirectoryInfo di = new DirectoryInfo(pathTemporal);

            foreach (FileInfo fi in di.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo si in di.GetDirectories())
            {
                si.Delete(true);
            }

            //- Establecemos ruta de donde se obtendrá los archivos NCP
            //string pathRootNCP = @"\\fs\Areas\SPR\2-DESPACHO\1-PDO\";

            string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";

            //string pathFuenteNCP = fechaProceso.Year +
            //            @"\Sem" + EPDate.f_numerosemana(fechaProceso).ToString().PadLeft(2, '0') + fechaProceso.Year.ToString().Substring(2, 2) +
            //            @"\PDO_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
            //            @"\5-Final\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
            //            @".zip";

            string pathFuenteNCP = @"Operación\Programa de Operación\Programa Diario\" + fechaProceso.Year + @"\" + fechaProceso.Month.ToString().PadLeft(2, '0') + @"_" +
                   COES.Base.Tools.Util.ObtenerNombreMes(fechaProceso.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProceso.Day.ToString().PadLeft(2, '0') +
                    @"\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                    @".zip";

            //- Extraemos en una nueva carpeta el contenido del ZIP
            FileServer.DescomprimirZip(pathFuenteNCP, pathTemporal, pathRootNCP);

            string file = ConstantesCortoPlazo.FileEquivBarraCentral;
            string url = "NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";

            if (FileServer.VerificarExistenciaFile(url, file, pathTemporal))
            {                
                List<string[]> equivalenciaBarraCentral = FileHelper.ObtenerEquivalenciaBarraCentral(url + file,pathTemporal);
                return equivalenciaBarraCentral.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Establece la carpeta de trabajo de la corrida
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="pathRaiz"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public static string EstablecerCarpetaTrabajo(DateTime fechaProceso, string pathRaiz, int correlativo, out bool flag, bool flagMD, CpTopologiaDTO topologiaFinal)
        {         
            flag = true;
            //- Seteamos la carpeta correspondiente al dia
            string pathDia = fechaProceso.Year + @"\" +
                      fechaProceso.Day.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) +
                      fechaProceso.Month.ToString().PadLeft(2, Convert.ToChar(ConstantesCortoPlazo.CaracterCero)) + @"\";

            //- Ruta de los archivos de la corrdida
            string pathCorrida = pathDia + ConstantesCortoPlazo.FolderCorrida + correlativo + @"\";

            //- Creamos la carpeta de trabajo de la corrida
            FileServer.CreateFolder(pathDia, ConstantesCortoPlazo.FolderCorrida + correlativo, pathRaiz);


            if (!flagMD)
            {
                //- Listamos los archivos necesarios para el proceso
                List<string> filesNCP = ObtenerFilesNCP();

                //- Verificamos la existencia del archivo en NCP
                if (!FileServer.VerificarExistenciaFile(pathDia, filesNCP[0], pathRaiz))
                {
                    flag = false;
                }

                if (flag)
                {
                    //- Copiamos los archivos NCP respectivos
                    foreach (string fileNcp in filesNCP)
                    {
                        FileServer.CopiarFile(pathDia, pathCorrida, fileNcp, pathRaiz);
                    }
                }
            }
            else
            {
                //- En caso no exista el programa o reprograma
                if (topologiaFinal == null)
                {
                    flag = false;
                }
            }


            //- Seteamos la carpeta correspondiente a la corrida
            string pathTrabajo = pathCorrida;

            return pathTrabajo;
        }

        /// <summary>
        /// Permite obtener los reprogramas en caso que existan
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <param name="pathRaiz"></param>
        public static void VerificarExistenciaReprograma(DateTime fechaProceso, string pathRaiz, bool indicadorNCP, out string letraReprograma, 
            string rutaNCP, bool flagMD, out CpTopologiaDTO topologiaFinal, int idEscenario, int periodo)
        {

            if (!flagMD)
            {
                //- Definimos la carpeta de trabajo temporal
                string pathTemporal = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathTempCostosMarginales];

                //- Definimos la carpeta de trabajo de CMgN
                string pathTrabajo = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathCostosMarginales];

                //- Verificamos que exista la carpeta para los archivos NCP

                DateTime fechaProcesoOrigen;

                if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
                    fechaProcesoOrigen = (new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day)).AddDays(-1).AddHours(23).AddMinutes(59);
                else
                    fechaProcesoOrigen = fechaProceso;


                string folderNCP = @"\" + fechaProceso.Year + @"\" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') + @"\";
                FileServerScada.CreateFolder(string.Empty, folderNCP, pathTrabajo);

                if (!indicadorNCP)
                {
                    //- Verificamos que exista reprograma en la ultima media hora
                    string folderReprograma = @"Operación\Programa de Operación\Reprograma Diario Operación\" + fechaProcesoOrigen.Year + @"\" + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') + @"_" +
                        COES.Base.Tools.Util.ObtenerNombreMes(fechaProcesoOrigen.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0');

                    //- Lista de directorios de reprogramas
                    string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";
                    List<FileData> listDirectories = FileServerScada.ObtenerFolderPorCriterio(folderReprograma, fechaProceso.AddHours(-24), fechaProceso, pathRootNCP);

                    if (listDirectories.Count() > 0)
                    {
                        //- Eliminamos el contenido de la carpeta temporal
                        DirectoryInfo di = new DirectoryInfo(pathTemporal);

                        foreach (FileInfo fi in di.GetFiles())
                        {
                            fi.Delete();
                        }

                        foreach (DirectoryInfo si in di.GetDirectories())
                        {
                            si.Delete(true);
                        }

                        //- Determinamos la ruta del archivo comprimido NCP                              

                        string folderName = listDirectories[0].FileName;
                        string reprograma = folderName[folderName.Length - 1].ToString();
                        string pathFuenteNCP = folderReprograma + @"\" + folderName + @"\NCP_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') +
                            fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') + reprograma + ".zip";

                        //- Verificamos que el reprograma del envio de correo coincida con lo cargado en el portal
                        //if (reprograma == letraReprograma)
                        //{
                        //- Extraemos en una nueva carpeta el contenido del ZIP
                        FileServer.DescomprimirZip(pathFuenteNCP, pathTemporal, pathRootNCP);

                        //- Definimos la ruta de origen donde se copiaran los archivos
                        string pathSource = pathTemporal + "NCP_" + fechaProcesoOrigen.Day.ToString().PadLeft(2, '0') + fechaProcesoOrigen.Month.ToString().PadLeft(2, '0') + reprograma + @"\";

                        //- Archivos necesarios
                        List<string> files = ObtenerFilesNCP();

                        //- Copiamos los archivos necesarios
                        foreach (var file in files)
                        {
                            FileServer.CopiarFileAlterFinal(pathSource, folderNCP, file, pathTrabajo);
                        }
                        //}

                        letraReprograma = "RDO " + reprograma;

                    }
                    else
                    {
                        ObtenerArchivoNCPrograma(fechaProceso);
                        letraReprograma = "PDO";
                    }
                }
                else
                {
                    //- Definimos la ruta desde donde copiaran los archivos a reemplazar
                    string pathSource = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathModificacionNCP];

                    if (!string.IsNullOrEmpty(rutaNCP))
                    {
                        pathSource = ConfigurationManager.AppSettings[ConstantesCortoPlazo.PathModificacionAlternaNCP] + rutaNCP + @"\";
                    }

                    //- Archivos necesarios
                    List<string> files = ObtenerFileSimplificadosNCP();

                    //- Copiamos los archivos necesarios
                    foreach (var file in files)
                    {
                        FileServer.CopiarFileAlterFinal(pathSource, folderNCP, file, pathTrabajo);
                    }

                    letraReprograma = "MANUAL";
                }

                topologiaFinal = null;
            }
            else
            {
                if (idEscenario == 0)
                {
                    DateTime fechaProcesoOrigen;

                    if (fechaProceso.Hour == 0 && fechaProceso.Minute == 0)
                        fechaProcesoOrigen = (new DateTime(fechaProceso.Year, fechaProceso.Month, fechaProceso.Day)).AddDays(-1).AddHours(23).AddMinutes(59);
                    else
                        fechaProcesoOrigen = fechaProceso;

                    topologiaFinal = (new McpAppServicio()).ObtenerTopologiaFinalPorFecha(fechaProcesoOrigen, ConstantesCortoPlazo.TopologiaDiario, periodo);
                    letraReprograma = topologiaFinal.Topcodi.ToString();
                }
                else
                {
                    topologiaFinal = (new McpAppServicio()).ObtenerPorTopologiaPorId(idEscenario);
                    letraReprograma = topologiaFinal.Topcodi.ToString();
                }
            }
        }

        /// <summary>
        /// Validamos existencia de Programa o Reprograma
        /// </summary>
        /// <param name="fechaProceso"></param>
        /// <returns></returns>
        public static int ValidarExistenciaNCP(DateTime fechaProceso)
        {
            int resultado = 0; //- No se encontro archivos
            try
            {
                //- Verificamos que exista reprograma en la ultima media hora
                string folderReprograma = @"Operación\Programa de Operación\Reprograma Diario Operación\" + fechaProceso.Year + @"\" + fechaProceso.Month.ToString().PadLeft(2, '0') + @"_" +
                    COES.Base.Tools.Util.ObtenerNombreMes(fechaProceso.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProceso.Day.ToString().PadLeft(2, '0');

                //- Lista de directorios de reprogramas
                string pathRootNCP = @"\\coes.org.pe\archivosapp\web\";
                List<FileData> listDirectories = FileServer.ObtenerFolderPorCriterio(folderReprograma, fechaProceso.AddHours(-24), fechaProceso, pathRootNCP);

                if (listDirectories.Count() > 0)
                {
                    //- Determinamos si las carpetas tienen la estructura requerida
                    string folderName = listDirectories[0].FileName;
                    string reprograma = folderName[folderName.Length - 1].ToString();
                    string pathFuenteNCP = folderReprograma + @"\" + folderName + @"\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') +
                        fechaProceso.Month.ToString().PadLeft(2, '0') + reprograma + ".zip";

                    if (FileServer.VerificarExistenciaFile(string.Empty, pathFuenteNCP, pathRootNCP))
                    {
                        resultado = 3; //-Existe Reprograma                
                    }
                    else
                    {
                        resultado = 4; //-Reprograma no está en el formato correcto
                    }
                }
                else
                {
                    //pathRootNCP = @"\\fs\Areas\SPR\2-DESPACHO\1-PDO\";
                    pathRootNCP = @"\\coes.org.pe\archivosapp\web\";

                    //string pathFuenteNCP = fechaProceso.Year +
                    //    @"\Sem" + EPDate.f_numerosemana(fechaProceso).ToString().PadLeft(2, '0') + fechaProceso.Year.ToString().Substring(2, 2) +
                    //    @"\PDO_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                    //    @"\5-Final\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                    //    @".zip";

                    string pathFuenteNCP = @"Operación\Programa de Operación\Programa Diario\" + fechaProceso.Year + @"\" + fechaProceso.Month.ToString().PadLeft(2, '0') + @"_" +
                   COES.Base.Tools.Util.ObtenerNombreMes(fechaProceso.Month) + @"\" + ConstantesCortoPlazo.TextoDia + fechaProceso.Day.ToString().PadLeft(2, '0') +
                    @"\NCP_" + fechaProceso.Day.ToString().PadLeft(2, '0') + fechaProceso.Month.ToString().PadLeft(2, '0') +
                    @".zip";

                    if (FileServer.VerificarExistenciaFile(string.Empty, pathFuenteNCP, pathRootNCP))
                    {
                        resultado = 1; //-Existe programa                
                    }
                    else
                    {
                        resultado = 2; //-No existe programa                
                    }
                }
            }
            catch(Exception ex)
            {
                resultado = -1;
            }

            resultado = 1; // cambio

            return resultado;
        }

        /// <summary>
        /// Lista de archivos requeridos
        /// </summary>
        /// <returns></returns>
        public static List<string> ObtenerFilesNCP()
        {
            return new List<string>{
                "cmgbuscp.csv",
                "oppchgcp.csv",
                "chidroPE.dat",
                //"ctermiPE.dat",
                "cmgsumcp.csv",
                "cmgcircp.csv",
                "volfincp.csv",
                "gertercp.csv",
                "dbus.dat",
                "cmgdemcp.csv",
                "cmgtercp.csv"
            };
        }

        /// <summary>
        /// Lista de archivos requeridos
        /// </summary>
        /// <returns></returns>
        public static List<string> ObtenerFileSimplificadosNCP()
        {
            return new List<string>{ 
                "cmgbuscp.csv",
                //"oppchgcp.csv",
                "chidroPE.dat",
                //"ctermiPE.dat",
                //"cmgsumcp.csv",
                //"cmgcircp.csv",
                "volfincp.csv",
                //"gertercp.csv",
                "dbus.dat"
                
            };
        }



        #region FIT - VALORIZACION

        /// <summary>
        /// Permite leer los costos marginales del archivo de resultado de analisis
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="relacion"></param>
        /// <returns></returns>
        public static ResultadoGamsAnalisis ObtenerResultadoGamsAnalisis(string fileName, string path, List<EqRelacionDTO> relacion)
        {

            ResultadoGamsAnalisis resultado = new ResultadoGamsAnalisis();

            resultado.Resumen = new ResultadoResumen();
            resultado.ListaCompensacionTermica = new List<ResultadoCompensacionesTermicas>();
            resultado.ListaGeneracionTermica = new List<ResultadoGeneracionTermica>();
            resultado.ListaGeneracionHidraulica = new List<ResultadoGeneracionHidraulica>();
            resultado.ListaCongestion = new List<ResultadoCongestion>();
            resultado.ListaCongestionConjunta = new List<ResultadoCongestion>();
            resultado.ListaCongestionRegionArriba = new List<ResultadoCongestion>();
            resultado.ListaCongestionRegionAbajo = new List<ResultadoCongestion>();

            try
            {
                //Resumen
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == ConstantesCortoPlazo.InicioResultadoGamsResumen)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            contador = contador + 1;
                        }

                        if (contador == 3)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 2)
                                    {

                                        decimal generacionTermica = 0;

                                        if (decimal.TryParse(arreglo[1], out generacionTermica)) { }

                                        resultado.Resumen.GeneracionTermica = generacionTermica;

                                    }

                                }
                            }
                        }

                        if (contador == 4)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 2)
                                    {

                                        decimal generacionHidraulica = 0;

                                        if (decimal.TryParse(arreglo[1], out generacionHidraulica)) { }

                                        resultado.Resumen.GeneracionHidraulica = generacionHidraulica;

                                    }
                                }
                            }
                        }


                        if (contador == 5)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 2)
                                    {

                                        decimal demandaTotal = 0;

                                        if (decimal.TryParse(arreglo[1], out demandaTotal)) { }

                                        resultado.Resumen.GeneracionDemandaTotal = demandaTotal;

                                    }
                                }
                            }

                            break;
                        }
                    }
                }

                //Generación Termica
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == ConstantesCortoPlazo.InicioResultadoGamsGeneracionTermica)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            contador = contador + 1;
                        }

                        if (contador > 3)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 4)
                                    {
                                        ResultadoGeneracionTermica item = new ResultadoGeneracionTermica();
                                        item.Termica = arreglo[0].Replace("\"", "").Trim();

                                        string nombreBarra = "";
                                        string idGenerador = "";
                                        string nomTna = item.Termica;

                                        string[] split = item.Termica.Split(' ');
                                        if (split.Length == 2)
                                        {
                                            nombreBarra = split[0].Trim();
                                            idGenerador = split[1].Trim();
                                        }

                                        var itemRelacion = relacion.Where(x => x.Nombretna == nomTna).FirstOrDefault();
                                        int? equicodi = null;
                                        if (itemRelacion != null)
                                            equicodi = itemRelacion.Equicodi;

                                        if (equicodi.HasValue)
                                        {
                                            item.CodigoEquipo = equicodi.Value;

                                            decimal pgMW = 0;

                                            if (decimal.TryParse(arreglo[1], out pgMW)) { }

                                            item.PgMW = pgMW;

                                            resultado.ListaGeneracionTermica.Add(item);
                                        }


                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }


                //Generación Hidraulica
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == ConstantesCortoPlazo.InicioResultadoGamsGeneracionHidraulica)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            contador = contador + 1;
                        }

                        if (contador > 3)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 3)
                                    {
                                        ResultadoGeneracionHidraulica item = new ResultadoGeneracionHidraulica();
                                        item.Hidraulica = arreglo[0].Replace("\"", "").Trim();

                                        string nombreBarra = "";
                                        string idGenerador = "";
                                        string nomTna = item.Hidraulica;

                                        string[] split = item.Hidraulica.Split(' ');
                                        if (split.Length == 2)
                                        {
                                            nombreBarra = split[0].Trim();
                                            idGenerador = split[1].Trim();
                                        }

                                        var itemRelacion = relacion.Where(x => x.Nombretna == nomTna).FirstOrDefault();
                                        int? equicodi = null;
                                        if (itemRelacion != null)
                                            equicodi = itemRelacion.Equicodi;

                                        if (equicodi.HasValue)
                                        {
                                            item.CodigoEquipo = equicodi.Value;

                                            decimal phMW = 0;

                                            if (decimal.TryParse(arreglo[1], out phMW)) { }

                                            item.PhMW = phMW;

                                            resultado.ListaGeneracionHidraulica.Add(item);
                                        }


                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }


                // Compensacion Termica
                using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
                {
                    string line = string.Empty;
                    int contador = 0;
                    bool flag = false;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == ConstantesCortoPlazo.InicioResultadoGamsCompensacionTermica)
                        {
                            flag = true;
                        }

                        if (flag)
                        {
                            contador = contador + 1;
                        }

                        if (contador > 3)
                        {
                            if (line != null)
                            {
                                if (line != string.Empty)
                                {
                                    string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                    if (arreglo.Length == 6)
                                    {

                                        ResultadoCompensacionesTermicas item = new ResultadoCompensacionesTermicas();
                                        item.Termica = arreglo[0].Replace("\"", "").Trim();
                                        item.Calificacion = arreglo[1].Replace("\"", "").Trim();

                                        string nombreBarra = "";
                                        string idGenerador = "";
                                        string nomTna = item.Termica;

                                        string[] split = item.Termica.Split(' ');
                                        if (split.Length == 2)
                                        {
                                            nombreBarra = split[0].Trim();
                                            idGenerador = split[1].Trim();
                                        }

                                        var itemRelacion = relacion.Where(x => x.Nombretna == nomTna).FirstOrDefault();
                                        int? equicodi = null;
                                        if (itemRelacion != null)
                                            equicodi = itemRelacion.Equicodi;

                                        if (equicodi.HasValue)
                                        {
                                            item.CodigoEquipo = equicodi.Value;

                                            decimal compensacion = 0;

                                            if (decimal.TryParse(arreglo[5], out compensacion)) { }

                                            item.Compensacion = compensacion;

                                            resultado.ListaCompensacionTermica.Add(item);
                                        }



                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                //- Congestion en enlaces
                resultado.ListaCongestion = ObtenerDatosCongestion(fileName, path, 0);
                resultado.ListaCongestionConjunta = ObtenerDatosCongestion(fileName, path, 1);
                resultado.ListaCongestionRegionArriba = ObtenerDatosCongestion(fileName, path, 2);
                resultado.ListaCongestionRegionAbajo = ObtenerDatosCongestion(fileName, path, 3);


                //- Congestion en conjunto de lineas

                //- Congestión en regiones de seguridad

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return resultado;
        }

        /// <summary>
        /// Permite obtener los datos de congestiones
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="opcion"></param>
        /// <returns></returns>
        public static List<ResultadoCongestion> ObtenerDatosCongestion(string fileName, string path, int opcion) 
        {
            List<ResultadoCongestion> result = new List<ResultadoCongestion>();
            using (StreamReader reader = FileServer.OpenReaderFile(fileName, path))
            {
                string line = string.Empty;
                int contador = 0;
                bool flag = false;

                string ini = string.Empty;

                switch (opcion)
                    {
                    case 0:
                        ini = ConstantesCortoPlazo.InicioResultadoGamsCongestion;
                        break;
                    case 1:
                        ini = ConstantesCortoPlazo.InicioResultadoGamsCongestionConjunto;
                        break;
                    case 2:
                        ini = ConstantesCortoPlazo.InicioResultadoGamsRegionSeguridadArriba;
                        break;
                    case 3:
                        ini = ConstantesCortoPlazo.InicioResultadoGamsRegionSeguridadAbajo;
                        break;
                    default:
                        break;
                }

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ini)
                    {
                        flag = true;
                    }

                    if (flag)
                    {
                        contador = contador + 1;
                    }

                    if (contador > 3)
                    {
                        if (line != null)
                        {
                            if (line != string.Empty)
                            {
                                string[] arreglo = line.Split(ConstantesCortoPlazo.CaracterSeparacionCSV).Select(x => x.Trim()).ToArray();

                                if ((arreglo.Length == 5 && (opcion == 0 || opcion == 1) ) || (arreglo.Length == 7 && (opcion == 2 || opcion == 3)))
                                {
                                    ResultadoCongestion item = new ResultadoCongestion();
                                    item.NombreCongestion = arreglo[0];
                                    item.Tipo = opcion;
                                    decimal limite = 0;
                                    decimal envio = 0;
                                    decimal recepcion = 0;
                                    decimal congestion = 0;
                                    decimal genlimite = 0;
                                    decimal generacion = 0;


                                    if (decimal.TryParse(arreglo[1], out limite)) { }
                                    if (decimal.TryParse(arreglo[2], out envio)) { }
                                    if (decimal.TryParse(arreglo[3], out recepcion)) { }
                                    if (decimal.TryParse(arreglo[4], out congestion)) { }

                                    if(opcion == 2 || opcion == 3)
                                    {
                                        if (decimal.TryParse(arreglo[5], out genlimite)) { }
                                        if (decimal.TryParse(arreglo[6], out generacion)) { }
                                    }

                                    item.Limite = limite;
                                    item.Envio = envio;
                                    item.Recepcion = recepcion;
                                    item.Congestion = congestion;
                                    item.GenLimite = genlimite;
                                    item.Generacion = generacion;

                                    result.Add(item);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}
