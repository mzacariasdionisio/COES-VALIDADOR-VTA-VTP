using System;
using System.Collections.Generic;
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
        static HttpServiceHelper()
        {
            _httpClient = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan // Lo dejamos infinito, controlaremos con token
            };
        }

        public static async Task<string> SendAsync(HttpMethod method, string url, HttpContent content = null, int timeoutSeconds = 30)
        {
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds)))
            using (var request = new HttpRequestMessage(method, url))
            {
                if (content != null)
                    request.Content = content;

                try
                {
                    var response = await _httpClient.SendAsync(request, cts.Token);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException ex)
                {
                    if (!cts.Token.IsCancellationRequested)
                        throw new TimeoutException($"Tiempo de espera agotado para {url}", ex);

                    throw; // Fue cancelado manualmente
                }
            }

        }
    }
