
using COES.Dominio.DTO.ValidacionVTEAVTP;
using COES.Servicios.Aplicacion.Helper;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        const string HttpMethodTrnperiodo = "sme/trnperiodo";
        const string HttpMethodVtpVersions = "sme/vtp_versions";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(ValidacionVTEAVTPAppServicio));

        /// <summary>
        /// Obtiene datos del servicio Trnperiodo
        /// </summary>
        public async Task<List<TrnPeriodoDTO>> ObtenerSmeTrnPeriodo()
        {
            try
            {
                string urlMetodo = string.Format("{0}/{1}", urlBase, HttpMethodTrnperiodo);
                var response = await httpClient.GetAsync(urlMetodo);
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Error al llamar al servicio: " + response.StatusCode);

                var json = await response.Content.ReadAsStringAsync();
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
        public async Task<List<VteVersionDTO>> ObtenerSmeVtpVersions(string perinombre, string recpotnombre)
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

                return JsonConvert.DeserializeObject<List<VteVersionDTO>>(json);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                throw new Exception(ex.Message, ex);
            }
        }

    }
}
