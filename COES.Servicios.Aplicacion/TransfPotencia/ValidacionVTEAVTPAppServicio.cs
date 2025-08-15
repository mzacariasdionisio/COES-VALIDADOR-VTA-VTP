
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Helper;
using DevExpress.Office.Utils;
using Google.Api.Gax.ResourceNames;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        //string urlBase = "http://10.100.210.3:8001";
        string urlBase = "";
        //string urlBaseValidador = "http://10.100.210.3:8002";
        string urlBaseValidador = "";
        const string HttpMethodTrnperiodo = "sme/trnperiodo";
        const string HttpMethodVtpVersions = "sme/vtp_versions";
        const string HttpMethodVteaVersions = "sme/vtea_versions";
        const string HttpMethodVtpValidacion = "funcion/vtp_validation";
        const string HttpMethodVtp = "funcion/vtp";
        const string HttpMethodVtpVtea = "funcion/vtp_vtea";

        const string HttpMethodVteaValidation = "funcion/vtea_validation";
        const string HttpMethodVtea = "funcion/vtea";

        public ValidacionVTEAVTPAppServicio(){
            urlBase = ConfigurationManager.AppSettings["SmeApiRestCombo"];
            urlBaseValidador = ConfigurationManager.AppSettings["SmeApiRestProceso"];
        }


        private static readonly ILog Logger = LogManager.GetLogger(typeof(ValidacionVTEAVTPAppServicio));

        /// <summary>
        /// Obtiene datos del servicio Trnperiodo
        /// </summary>
        public async Task<TrnPeriodoDTO> ObtenerSmeTrnPeriodo(string folderUpload,
            string pathfile,
            string folderSave)
        {
            TrnPeriodoDTO trnPeriodoDTO = new TrnPeriodoDTO(); ;
            try
            {
                string urlMetodo = string.Format("{0}/{1}", urlBase, HttpMethodTrnperiodo);
                var response =await HttpServiceHelper.SendAsync(HttpMethod.Get, urlMetodo);
                List<TablePeriodoDTO> periodos = JsonConvert.DeserializeObject<List<TablePeriodoDTO>>(response);
                trnPeriodoDTO.Resultado = 0;
                trnPeriodoDTO.Periodos = periodos;
                return trnPeriodoDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                trnPeriodoDTO.Resultado = -1;
                trnPeriodoDTO.Mensaje = ex.Message.ToString();
                return trnPeriodoDTO;
                //throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
        public async Task<VtpVersionDTO> ObtenerSmeVtpVersions(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            VtpVersionDTO vtpVersionDTO = new VtpVersionDTO();  
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
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);
                List<TableVersionVtpDTO> versiones = JsonConvert.DeserializeObject<List<TableVersionVtpDTO>>(json);
                vtpVersionDTO.Resultado = 0;
                vtpVersionDTO.Versiones = versiones;
                return vtpVersionDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpVersionDTO.Resultado = -1;
                vtpVersionDTO.Mensaje = ex.Message.ToString();
                return vtpVersionDTO;
                //throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtea Versions
        /// </summary>
        public async Task<VteaVersionDTO> ObtenerSmeVteaVersions(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            VteaVersionDTO vteaVersionDTO = new VteaVersionDTO();
            try
            {
                string urlMetodo = $"{urlBase}/{HttpMethodVteaVersions}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);
                List<TableVersionVteaDTO> versiones = JsonConvert.DeserializeObject<List<TableVersionVteaDTO>>(json);
                vteaVersionDTO.Resultado = 0;
                vteaVersionDTO.Versiones = versiones;
                return vteaVersionDTO;
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaVersionDTO.Resultado = -1;
                vteaVersionDTO.Mensaje = ex.Message.ToString();
                return vteaVersionDTO;
                //throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Validar
        /// </summary>
        public async Task<VtpValidacionDTO> FuncionVtpValidar(string perinombre, string recpotnombre, 
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            VtpValidacionDTO vtpValidacionDTO = new VtpValidacionDTO();
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
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);
                return JsonConvert.DeserializeObject<VtpValidacionDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpValidacionDTO.Resultado = -1;
                vtpValidacionDTO.Mensaje = ex.Message.ToString();
                return vtpValidacionDTO;
                //throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
        public async Task<VteaDTO> FuncionVtea(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            VteaDTO vteaDTO = new VteaDTO();    
            try
            {
                string urlMetodo = $"{urlBaseValidador}/{HttpMethodVtea}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);

                return JsonConvert.DeserializeObject<VteaDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaDTO.Resultado = -1;
                vteaDTO.Mensaje = ex.Message.ToString();
                return vteaDTO;

            }
        }


        /// <summary>
        /// Obtiene datos del servicio Vtp Versions
        /// </summary>
       /* public async Task<VteaValidadorDTO> FuncionVteaValidador(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave)
        {
            VteaValidadorDTO vteaValidadorDTO = new VteaValidadorDTO(); 
            try
            {
                string urlMetodo = $"{urlBaseValidador}/{HttpMethodVtea}/";

                var parametros = new
                {
                    perinombre,
                    recpotnombre
                };
                string jsonBody = JsonConvert.SerializeObject(parametros);

                // Aquí se configura correctamente el tipo MIME: application/json
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);

                return JsonConvert.DeserializeObject<VteaValidadorDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vteaValidadorDTO.Resultado = -1;
                vteaValidadorDTO.Mensaje =ex.Message.ToString();
                return vteaValidadorDTO;

            }
        }*/

        /// <summary>
        /// Obtiene datos del servicio funcion/vtp_vtp
        /// </summary>
        public async Task<VtpDTO> FuncionVtp(string perinombre, string recpotnombre,
            string folderUpload,
            string pathfile,
            string folderSave
            )
        {
            VtpDTO vtpDTO = new VtpDTO();
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

                //var response = await httpClient.PostAsync(urlMetodo, content);

                //if (!response.IsSuccessStatusCode)
                //    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                //string json = await response.Content.ReadAsStringAsync();
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post, urlMetodo, content);
                //RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<VtpDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpDTO.Resultado = -1;
                vtpDTO.Mensaje = ex.Message.ToString();
                return vtpDTO;
                //throw new Exception(ex.Message, ex);
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
            VtpVteaDTO vtpVteaDTO = new VtpVteaDTO();
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

                //var response = await httpClient.PostAsync(urlMetodo, content);

                //if (!response.IsSuccessStatusCode)
                //    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                //string json = await response.Content.ReadAsStringAsync();
                string json = await HttpServiceHelper.SendAsync(HttpMethod.Post,urlMetodo, content);
                //RegistrarLogTransaccionTxt("POST", urlMetodo, jsonBody, response, folderUpload, pathfile, folderSave);
                return JsonConvert.DeserializeObject<VtpVteaDTO>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                vtpVteaDTO.Resultado = -1;
                vtpVteaDTO.Mensaje = ex.Message.ToString();
                return vtpVteaDTO;
                //throw new Exception(ex.Message, ex);
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
