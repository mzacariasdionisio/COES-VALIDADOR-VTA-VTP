using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IAzDataTablesRepository
    {
        bool ActualizarRegistro(AzDataTableDTO dto);
        Task GuardarFraseComoConceptoClaveAsync(string rowKey, string conceptoClave);
        Task<AzDataTableDTO> ObtenerRegistroAsync(string rowKey);
        Task<List<string>> ObtenerTiposDocumento();
    }
}
