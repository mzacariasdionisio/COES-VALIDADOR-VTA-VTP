using Azure;
using Azure.Data.Tables;
using System;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class AzDataTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string metadata_storage_path { get; set; }
        public string TipoDocumento { get; set; }
        public string RutaSharepointOnline { get; set; }
        public string modificado_por { get; set; }
        public string UsuarioAccesoTotal { get; set; }
        public string PalabrasClave { get; set; }
        public string NombreArchivoConExtension { get; set; }
        public string Author { get; set; }
        public string Keyconcepts { get; set; }
        public string Created { get; set; }
        public string Expediente { get; set; }
    }
}
