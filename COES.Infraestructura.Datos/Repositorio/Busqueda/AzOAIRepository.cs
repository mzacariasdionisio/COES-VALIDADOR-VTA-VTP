using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using OpenAI.Chat;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class AzOAIRepository : IAzOAIRepository
    {
        private readonly string endpoint = ConfigurationManager.AppSettings["AzureOpenAI:Endpoint"];
        private readonly string apikey = ConfigurationManager.AppSettings["AzureOpenAI:ApiKey"];
        private readonly string modelo = ConfigurationManager.AppSettings["AzureOpenAI:Model"];
        private readonly string _searchEndpoint = ConfigurationManager.AppSettings["AzureSearch:Endpoint"];
        private readonly string _searchIndex = ConfigurationManager.AppSettings["AzureSearch:IndexName"];
        private readonly string _searchKey = ConfigurationManager.AppSettings["AzureSearch:ApiKey"];
        private readonly ChatClient _chatClient;

        public AzOAIRepository()
        {
            Uri uri = new Uri(endpoint);
            ApiKeyCredential credential = new ApiKeyCredential(apikey);
            AzureOpenAIClient azureClient = new AzureOpenAIClient(uri, credential);
            _chatClient = azureClient.GetChatClient(modelo);
        }

        public async Task<RespuestaChatDTO> BusquedaConversacional(List<ChatHistoryDTO> historial)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<ChatMessage> historialMensajes = new List<ChatMessage>();
            string promptBase = "Eres un asistente de IA especializado en ayudar a las personas a encontrar información precisa y fundamentada. " +
                "Responde siempre en español, traduciendo los contenidos si es necesario para garantizar una comprensión clara. " +
                "Tu tarea es analizar y basar tus respuestas exclusivamente en el contexto. " +
                "De cada documento enfócate en las secciones " +
                "análisis del recurso, análisis de la impugnación, análisis de la reconsideración o análisis de la apelación. " +
                "Dame la siguiente información de ser encontrada en el documento con el siguiente peso: petitorio (10%), análisis del recurso (80%) y decisión (10%)." +
                "Proporciona respuestas detalladas, bien estructuradas y fundamentadas, citando los documentos del contexto utilizados en cada afirmación. " +
                "No inventes información ni proporciones respuestas basadas en tu entrenamiento previo ni información externa. ";

            historialMensajes.Add(new SystemChatMessage(promptBase));

            foreach (var m in historial)
            {
                if (m.Role.ToLower() == "user")
                {
                    historialMensajes.Add(new UserChatMessage(m.Content));
                }
                else
                {
                    historialMensajes.Add(new AssistantChatMessage(m.Content));
                }
            }

            ChatCompletionOptions options = new ChatCompletionOptions
            {
                Temperature = (float)0.2,
                ResponseFormat = ChatResponseFormat.CreateTextFormat(),
                MaxOutputTokenCount = 3000,
                //FrequencyPenalty = (float)0.2,
                //PresencePenalty = (float)0.7
            };
#pragma warning disable AOAI001 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.

            options.AddDataSource(new AzureSearchChatDataSource()
            {
                Endpoint = new Uri(_searchEndpoint),
                IndexName = _searchIndex,
                Authentication = DataSourceAuthentication.FromApiKey(_searchKey),
                TopNDocuments = 3,
                FieldMappings = new DataSourceFieldMappings()
                {
                    ContentFieldNames = { "content", "NombreArchivoConExtension" },
                    ContentFieldSeparator = "\n",
                    UrlFieldName = "RutaSharePointOnline",
                    TitleFieldName = "NombreArchivoConExtension"
                },
                InScope = true,
                Strictness = 4,
                Filter = "NombreArchivoConExtension ne null and metadata_storage_size ne null" +
                $" and search.ismatchscoring('coes-d, coes/d, coes-p, coes/p', 'NombreArchivoConExtension')"
            });

            try
            {
                var completionUpdates = await _chatClient.CompleteChatAsync(historialMensajes, options);
                var result = completionUpdates.Value;
                string text = result.Content[0].Text;
                var messageContext = result.GetMessageContext();

#pragma warning restore AOAI001

                var chatResponseDto = new RespuestaChatDTO
                {
                    Contenido = text,
                    Referencias = new List<ReferenciaChatDTO>()
                };

                var citaciones = new List<ReferenciaChatDTO>();
                if (messageContext.Citations.Any())
                {
                    foreach (var citation in messageContext.Citations)
                    {
                        citaciones.Add(new ReferenciaChatDTO
                        {
                            Titulo = citation.Title,
                            Url = citation.Url,
                            Content = citation.Content
                        });
                    }
                    chatResponseDto.Referencias = citaciones;
                }
                return chatResponseDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new RespuestaChatDTO
                {
                    Contenido = "Sucedió un error, por favor consulta nuevamente. ",
                };
            }
        }
    }
}
