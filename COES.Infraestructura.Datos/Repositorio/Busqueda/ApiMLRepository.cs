using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class ApiMLRepository : IApiMLRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _mlApiUrl = ConfigurationManager.AppSettings["MLService:Url"];

        public ApiMLRepository()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri(_mlApiUrl),
                Timeout = TimeSpan.FromMinutes(2)
            };
        }

        public async Task<List<WbBusquedasDTO>> BuscarDocumentos(BCDBusquedasDTO busqueda, string Usuario)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var listadoFinal = new List<WbBusquedasDTO>();
            var queryParams = new Dictionary<string, string>
            {
                ["search_text"] = busqueda.Search_text,
                ["result_number"] = busqueda.Result_number.ToString(),
                ["key_words"] = busqueda.Key_words,
                ["key_concepts"] = busqueda.Key_concepts,
                ["exclude_words"] = busqueda.Exclude_words,
                ["search_start_date"] = busqueda.Search_start_date.ToString("yyyy-MM-dd"),
                ["search_end_date"] = busqueda.Search_end_date.ToString("yyyy-MM-dd"),
                ["tipo_documento"] = busqueda.Tipo_documento
            };
            var queryString = string.Join("&", queryParams
                .Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value)}"));

            var response = _httpClient.PostAsync($"/documentos/recomendados?{queryString}", null).Result;

            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<JObject>(res);
                var docEncontrados = value["value"].ToObject<List<WbBusquedasDTO>>();
                if (docEncontrados.Any())
                {
                    double? maxScore = docEncontrados.Max(r => r.score);

                    foreach (var result in docEncontrados)
                    {
                        float percentageScore = maxScore > 0 ? (float)(result.score / maxScore) * 100 : 0;
                        result.score = (float)Math.Round(percentageScore, 2);
                        string highlights = "";

                        if (result.highlights != "")
                        {
                            highlights = string.Join("\n\n", result.highlights);
                        }

                        result.highlights = highlights;

                        // Agregar documento solo si pertenece al usuario o no se indica
                        string usuario = result.UsuarioAccesoTotal?.ToString().ToLower();
                        if (string.IsNullOrEmpty(usuario) || usuario == Usuario.ToLower())
                        {
                            listadoFinal.Add(result);
                        }
                    }
                    listadoFinal = listadoFinal.OrderByDescending(r => r.score).ToList();
                }
            }
            return listadoFinal;
        }
    }
}
