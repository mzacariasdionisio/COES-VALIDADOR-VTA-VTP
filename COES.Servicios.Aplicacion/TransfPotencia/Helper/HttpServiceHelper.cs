using DocumentFormat.OpenXml.Wordprocessing;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.TransfPotencia.Helper
{
    /// <summary>
    /// Clase para consumir servicios REST SME y FUNCTION
    /// </summary>
    public static class HttpServiceHelper
    {
        private static readonly HttpClient _httpClient;
        private static readonly ILog _log = LogManager.GetLogger(typeof(HttpServiceHelper));
        private static int _timeoutSeconds;

        /// <summary>
        /// Constructor para consumir servicios REST SME y FUNCTION
        /// </summary>
        static HttpServiceHelper()
        {
            string configValue = ConfigurationManager.AppSettings["SmeConnectionTimeout"];

            if (string.IsNullOrWhiteSpace(configValue) || !int.TryParse(configValue, out _timeoutSeconds))
            {
                // Valor por defecto si la clave no existe o no es numérica
                _timeoutSeconds = 30;
            }
            else {
                _timeoutSeconds = Convert.ToInt32(configValue);
            }

            _httpClient = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan // Lo dejamos infinito, controlaremos con token
            };
        }

        /// <summary>
        /// Metodo para consumir servicios REST SME y FUNCTION
        /// </summary>
        public static async Task<string> SendAsync(HttpMethod method, string url, HttpContent content = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_timeoutSeconds)))
            using (var request = new HttpRequestMessage(method, url))
            {
                _log.InfoFormat("[Rest Api MSE] [HTTP {0}] URL: {1}", method, url);
                if (content != null)
                {
                    request.Content = content;
                    string body = await content.ReadAsStringAsync();
                    _log.InfoFormat($"[Rest Api MSE] Request Body: {0}", body);
                }

                try
                {
                    var response = await _httpClient.SendAsync(request, cts.Token);
                    response.EnsureSuccessStatusCode();

                    _log.InfoFormat("[Rest Api MSE] [HTTP {0}] StatusCode: {1} {2}", method, response.StatusCode, response.ReasonPhrase);
                    _log.InfoFormat("[Rest Api MSE] Response Body: {0}", response);

                    return await response.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException ex)
                {
                    if (!cts.Token.IsCancellationRequested)
                    {
                        _log.Error($"[Rest Api MSE] Timeout después de {_timeoutSeconds}s para URL: {url}", ex);
                        throw new TimeoutException($"[Rest Api MSE] Tiempo de espera agotado para {url}", ex);
                    }
                    _log.Error($"[Rest Api MSE] Error general en solicitud a {url}", ex);
                    throw new OperationCanceledException($"La solicitud a {url} fue cancelada manualmente.", ex, cts.Token);
                }
            }

        }
    }
}
