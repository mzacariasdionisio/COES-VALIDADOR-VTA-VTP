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
    public class HttpServiceHelper
    {
        private static readonly HttpClient _httpClient;
        private static readonly ILog _log = LogManager.GetLogger(typeof(HttpServiceHelper));
        private static int _timeoutSeconds;
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

        public static async Task<string> SendAsync(HttpMethod method, string url, HttpContent content = null)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_timeoutSeconds)))
            using (var request = new HttpRequestMessage(method, url))
            {
                _log.Info($"[Rest Api MSE] [HTTP {method}] URL: {url}");
                if (content != null)
                {
                    request.Content = content;
                    string body = await content.ReadAsStringAsync();
                    _log.Info($"[Rest Api MSE] Request Body: {body}");
                }

                try
                {
                    var response = await _httpClient.SendAsync(request, cts.Token);
                    response.EnsureSuccessStatusCode();

                    _log.Info($"[Rest Api MSE] [HTTP {method}] StatusCode: {(int)response.StatusCode} {response.ReasonPhrase}");
                    _log.Info($"[Rest Api MSE] Response Body: {response}");

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
                    throw; // Fue cancelado manualmente
                }
            }

        }
    }
}
