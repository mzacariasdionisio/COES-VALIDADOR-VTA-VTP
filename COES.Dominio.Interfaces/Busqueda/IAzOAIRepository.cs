using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IAzOAIRepository
    {
        Task<RespuestaChatDTO> BusquedaConversacional(List<ChatHistoryDTO> historial);
    }
}
