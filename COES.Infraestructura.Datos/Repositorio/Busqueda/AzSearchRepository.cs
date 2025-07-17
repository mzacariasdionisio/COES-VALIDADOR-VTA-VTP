using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;
using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class AzSearchRepository : IAzSearchRepository
    {
        private readonly SearchClient _searchClient;
        private readonly SearchIndexerClient _searchIndexerClient;
        private readonly SearchIndexClient _searchIndexClient;
        private readonly string _indexerTableName = ConfigurationManager.AppSettings["AzureSearch:IndexerTableName"];
        private readonly string _indexerBlobName = ConfigurationManager.AppSettings["AzureSearch:IndexerBlobName"];
        private readonly string _indexName = ConfigurationManager.AppSettings["AzureSearch:IndexName"];

        public AzSearchRepository()
        {
            string endpoint = ConfigurationManager.AppSettings["AzureSearch:Endpoint"];
            string apiKey = ConfigurationManager.AppSettings["AzureSearch:ApiKey"];
            Uri uri = new Uri(endpoint);

            var credential = new AzureKeyCredential(apiKey);
            _searchClient = new SearchClient(uri, _indexName, credential);
            _searchIndexerClient = new SearchIndexerClient(uri, credential);
            _searchIndexClient = new SearchIndexClient(uri, credential);
        }

        public async Task<List<WbBusquedasDTO>> BuscarDocumentosAsync(BCDBusquedasDTO busqueda, string Usuario)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (busqueda.Search_text == null) { busqueda.Search_text = "*"; }
            else { busqueda.Search_text = $" {busqueda.Search_text} "; }

            string startDateIso = busqueda.Search_start_date.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string endDateIso = busqueda.Search_end_date.ToString("yyyy-MM-ddTHH:mm:ssZ");

            string filtro = "NombreArchivoConExtension ne null" +
                " and metadata_storage_size ne null" +
                $" and Created ge {startDateIso}" +
                $" and Created le {endDateIso}" +
                $" and search.ismatchscoring('coes-d, coes/d, coes-p, coes/p', 'NombreArchivoConExtension')";

            if (busqueda.Tipo_documento != null)
            {
                filtro += $" and TipoDocumento eq '{busqueda.Tipo_documento}'";
            }

            if (busqueda.Key_concepts != null)
            {
                filtro += $" and search.ismatch('{busqueda.Key_concepts}','Keyconcepts')";
            }

            if (busqueda.Key_words != null)
            {
                filtro += $" and search.ismatch('{busqueda.Key_words}','PalabrasClave')";
            }
            if (busqueda.Exclude_words != null)
            {
                filtro += $" and not search.ismatch('{busqueda.Exclude_words}','PalabrasClave')";
            }

            var options = new SearchOptions
            {
                IncludeTotalCount = false,
                Size = busqueda.Result_number,
                SearchFields = { "content" },
                Filter = filtro,
                QueryType = Azure.Search.Documents.Models.SearchQueryType.Simple,
                SearchMode = Azure.Search.Documents.Models.SearchMode.All,
                HighlightFields = { "content-5" },
                HighlightPreTag = "<mark>",
                HighlightPostTag = "</mark>",
            };

            var response = await _searchClient.SearchAsync<WbBusquedasDTO>(busqueda.Search_text, options);
            var resultados = new List<WbBusquedasDTO>();

            double? maxScore = response.Value.GetResults().Max(r => r.Score);

            foreach (var result in response.Value.GetResults())
            {
                float percentageScore = maxScore > 0 ? (float)(result.Score / maxScore) * 100 : 0;
                result.Document.score = (float)Math.Round(percentageScore, 2);
                string highlights = "";

                if (result.Highlights != null && result.Highlights.ContainsKey("content"))
                {
                    highlights = string.Join("\n\n", result.Highlights["content"].Take(5));
                }

                result.Document.highlights = highlights ?? "";

                // Agregar documento solo si pertenece al usuario o no se indica
                string usuario = result.Document.UsuarioAccesoTotal?.ToString().ToLower();
                if (string.IsNullOrEmpty(usuario) || usuario == Usuario.ToLower())
                {
                    resultados.Add(result.Document);
                }
            }
            resultados = resultados.OrderByDescending(r => r.score).ToList();

            return resultados;
        }

        public void RestablecerIndexadorTabla()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // var indice = DefinirIndice();

            //_searchIndexClient.DeleteIndex(indexName: _indexName);
            //_searchIndexClient.CreateIndex(index: indice);
            //_searchIndexerClient.ResetIndexer(indexerName: _indexerTableName);
            //_searchIndexerClient.ResetIndexer(indexerName: _indexerBlobName);
            _searchIndexerClient.RunIndexer(indexerName: _indexerTableName);
            _searchIndexerClient.RunIndexer(indexerName: _indexerBlobName);
        }

        private SearchIndex DefinirIndice()
        {
            var fields = new List<SearchField>
                {
                    new SearchField("content", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        IsFacetable = false, IsSortable = false, IsKey = false,
                        AnalyzerName = "mapeoEtiquetas"
                    },
                    new SearchField("content_suggester", SearchFieldDataType.String)
                    {
                        IsFilterable = false, IsSortable = false,
                        IsFacetable = false, AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("metadata_storage_size", SearchFieldDataType.Int64)
                    {
                        IsSearchable = false, IsFilterable = true,
                        IsSortable = true, IsFacetable = true, IsKey = false
                    },
                    new SearchField("metadata_storage_content_md5", SearchFieldDataType.String)
                    {
                        IsSearchable = false, IsFilterable = false
                    },
                    new SearchField("metadata_storage_name", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = true,
                        IsSortable = true, IsFacetable = true, AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("metadata_storage_path", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        IsKey = true, AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("metadata_creation_date", SearchFieldDataType.DateTimeOffset)
                    {
                        IsSearchable = false, IsFilterable = true,
                        IsSortable = true, IsFacetable = true
                    },
                    new SearchField("metadata_author", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = true,
                        IsSortable = true, IsFacetable = true, AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("document_type", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = true,
                        IsSortable = true, IsFacetable = true, AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("Autor", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        AnalyzerName = LexicalAnalyzerName.EnMicrosoft
                    },
                    new SearchField("TipoDocumento", SearchFieldDataType.String)
                    {
                        IsSearchable = false, IsFilterable = true,
                        IsSortable = true, IsFacetable = true
                    },
                    new SearchField("RutaSharePointOnline", SearchFieldDataType.String)
                    {
                        IsSearchable = false, IsFilterable = false
                    },
                    new SearchField("Timestamp", SearchFieldDataType.DateTimeOffset)
                    {
                        IsSearchable = false, IsFilterable = true,
                        IsSortable = true, IsFacetable = true
                    },
                    new SearchField("NombreArchivoConExtension", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = true,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("PalabrasClave", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("Keyconcepts", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("UsuarioAccesoTotal", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("RowKey", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = true,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("Expediente", SearchFieldDataType.String)
                    {
                        IsSearchable = true, IsFilterable = false,
                        AnalyzerName = LexicalAnalyzerName.StandardLucene
                    },
                    new SearchField("Created", SearchFieldDataType.DateTimeOffset)
                    {
                        IsSearchable = false, IsFilterable = true,
                        IsSortable = true, IsFacetable = true
                    }
            };

            SearchIndex indice = new SearchIndex(_indexName) { Fields = fields };

            indice.Analyzers.Add(
                new CustomAnalyzer("mapeoEtiquetas", LexicalTokenizerName.Standard)
                {
                    TokenFilters = { TokenFilterName.Lowercase },
                    CharFilters = { "html_strip", "newline_strip" }
                }
            );
            indice.CharFilters.Add(new PatternReplaceCharFilter("newline_strip", @"(\n\s*)+", " "));
            indice.Suggesters.Add(
                new SearchSuggester(
                    "NombreArchivoConExtensionYContent",
                    sourceFields: new[] {
                        "NombreArchivoConExtension",
                        "content_suggester",
                        "PalabrasClave",
                        "Keyconcepts"
                    }
                )
            );

            // Agregar perfil de puntuacion y asignarle predeterminado en caso de usarlo

            return indice;
        }

        public async Task<List<string>> Sugerir(bool highlights, bool fuzzy, string term, string searchField)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            List<string> suggestions = new List<string>();
            var options = new SuggestOptions()
            {
                UseFuzzyMatching = fuzzy,
                Size = 8,
                SearchFields = { searchField },
            };

            if (highlights)
            {
                options.HighlightPreTag = "<b>";
                options.HighlightPostTag = "</b>";
            }
            try
            {
                var suggestResult = await _searchClient.SuggestAsync<SearchDocument>(term, "NombreArchivoConExtensionYContent", options).ConfigureAwait(false);
                suggestions = suggestResult.Value.Results
                    .SelectMany(x => x.Text.Split(','))
                    .Select(text => text.Trim())
                    .Where(text => text.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Distinct()
                    .ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return suggestions;
        }
    }
}
