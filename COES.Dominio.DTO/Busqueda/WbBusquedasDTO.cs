using Newtonsoft.Json;

namespace COES.Dominio.DTO.Busqueda
{
    public class WbBusquedasDTO
    {
        [JsonProperty("@search.score")]
        public float score { get; set; }
        public string content { get; set; }
        public string highlights { get; set; }
        public long metadata_storage_size { get; set; }
        public string metadata_storage_content_md5 { get; set; }
        public string metadata_storage_name { get; set; }
        public string metadata_storage_path { get; set; }
        public string metadata_creation_date { get; set; }
        public string metadata_author { get; set; }
        public string document_type { get; set; }
        public string Autor { get; set; }
        public string TipoDocumento { get; set; }
        public string RutaSharePointOnline { get; set; }
        public string Timestamp { get; set; }
        public string NombreArchivoConExtension { get; set; }
        public string PalabrasClave { get; set; }
        public string Keyconcepts { get; set; }
        public string UsuarioAccesoTotal { get; set; }
        public string RowKey { get; set; }

    }
}
