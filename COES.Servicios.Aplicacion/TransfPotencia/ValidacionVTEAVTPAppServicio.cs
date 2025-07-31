
using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using Google.Api.Gax.ResourceNames;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{

    public class ValidacionVTEAVTPAppServicio
    {
        private static readonly HttpClient httpClient = new HttpClient();

        string urlBase = "http://10.100.210.3:8001";
        string urlBaseValidador = "http://10.100.210.3:8002";
        const string HttpMethodTrnperiodo = "sme/trnperiodo";
        const string HttpMethodVtpVersions = "sme/vtp_versions";
        const string HttpMethodVtpValidacion = "funcion/vtp_validation";
        const string HttpMethodVtp = "funcion/vtp";
        const string HttpMethodVtpVtea = "funcion/vtp_vtea";


        private static readonly ILog Logger = LogManager.GetLogger(typeof(ValidacionVTEAVTPAppServicio));

        /// <summary>
        /// Obtiene datos del servicio Trnperiodo
        /// </summary>
        public async Task<List<TrnPeriodoDTO>> ObtenerSmeTrnPeriodo(string folderUpload,
            string pathfile,
            string folderSave)
        {
            try
            {
                string urlMetodo = string.Format("{0}/{1}", urlBase, HttpMethodTrnperiodo);
                var response = await httpClient.GetAsync(urlMetodo);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                var json = await response.Content.ReadAsStringAsync();
                RegistrarLogTransaccionTxt("GET", urlMetodo, "", response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<List<TrnPeriodoDTO>>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
        public async Task<List<VteVersionDTO>> ObtenerSmeVtpVersions(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            try
            {
                string urlMetodo = $"{urlBase}/{HttpMethodVtpVersions}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(urlMetodo, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                string json = await response.Content.ReadAsStringAsync();
                RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<List<VteVersionDTO>>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
        public async Task<VtpValidacionDTO> FuncionVtpValidar(string perinombre, string recpotnombre, 
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            try
            {
                string urlMetodo = $"{urlBaseValidador}/{HttpMethodVtpValidacion}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(urlMetodo, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                string json = await response.Content.ReadAsStringAsync();
                RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<VtpValidacionDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio funcion/vtp_vtp
        /// </summary>
        public async Task<VtpDTO> FuncionVtp(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave
            )
        {
            try
            {
                string urlMetodo = $"{urlBaseValidador}/{HttpMethodVtp}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(urlMetodo, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                string json = await response.Content.ReadAsStringAsync();
                RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<VtpDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio funcion/vtp_vtp
        /// </summary>
        public async Task<VtpVteaDTO> FuncionVtpVtea(string perinombre, string recanombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave
            )
        {
            try
            {
                string urlMetodo = $"{urlBaseValidador}/{HttpMethodVtpVtea}/";

                var parametros = new
                {
                    perinombre,
                    recanombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(urlMetodo, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                string json = await response.Content.ReadAsStringAsync();
                RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<VtpVteaDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }


        private void RegistrarLogTransaccionTxt(
            string metodoHttp,
            string url,
            string requestBody,
            HttpResponseMessage response,
            string folderUpload,
            string pathfile,
            string folderSave
        )
        {

            string fileName = $"Transacciones-{DateTime.Now:yyyy-MM-dd}.txt";
            string ruta = folderUpload;
            string filePath = ruta + fileName;
            string ruta2 = string.Format("{0}\\{1}", pathfile, folderSave);
            string path = FileServer.GetDirectory() + ruta2;
            string pathFilename = string.Format("{0}\\{1}", path, fileName);

            //string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(path);

            

            // Cabecera (solo si el archivo no existe)
            if (!File.Exists(pathFilename))
            {
                string header = "FechaHora | Metodo | URL | Parametros | BodyRequest | StatusCode";
                File.WriteAllText(pathFilename, header + Environment.NewLine);
            }

            // Línea de log
            string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {metodoHttp} | {url} | " +
                             $"{requestBody} | " +
                             $"{(int)response.StatusCode} {response.ReasonPhrase}";

            File.AppendAllText(pathFilename, logLine + Environment.NewLine);
        }

    }
}
