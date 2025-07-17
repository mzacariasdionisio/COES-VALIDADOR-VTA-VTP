using Azure;
using Azure.Data.Tables;
using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Busqueda
{
    public class AzDataTablesRepository : IAzDataTablesRepository
    {
        private readonly TableClient _tableClient;
        private readonly string tablePartitionKey = ConfigurationManager.AppSettings["AzureStorage:TablePartitionKey"];
        private readonly BDMappers _bDMappers = new BDMappers();

        public AzDataTablesRepository()
        {
            string connectionString = ConfigurationManager.AppSettings["AzureStorage:ConnectionString"];
            string tableName = ConfigurationManager.AppSettings["AzureStorage:TableName"];
            _tableClient = new TableClient(connectionString, tableName);
        }

        public async Task<List<string>> ObtenerTiposDocumento()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            return _tableClient.Query<AzDataTableEntity>()
                .Select(t => t.TipoDocumento)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToList();
        }

        public async Task<AzDataTableDTO> ObtenerRegistroAsync(string rowKey)
        {
            AzDataTableEntity registro = await _tableClient.GetEntityAsync<AzDataTableEntity>(tablePartitionKey, rowKey);
            return _bDMappers.DataTableEntidad2AzDataTableDTO(registro);
        }

        public bool ActualizarRegistro(AzDataTableDTO dto)
        {
            try
            {
                AzDataTableEntity registro = _tableClient.GetEntityAsync<AzDataTableEntity>(tablePartitionKey, dto.RowKey).Result;
                registro.PalabrasClave = dto.PalabrasClave;
                _tableClient.UpdateEntity(registro, ETag.All, TableUpdateMode.Replace);
                return true;
            }
            catch { return false; }
        }

        public async Task GuardarFraseComoConceptoClaveAsync(string rowKey, string conceptoClave)
        {
            var registro = await ObtenerRegistroAsync(rowKey);
            if (registro != null)
            {
                registro.Keyconcepts = conceptoClave;
                ActualizarRegistro(registro);
            }
        }
    }
}
