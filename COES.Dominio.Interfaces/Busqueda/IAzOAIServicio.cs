using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IAzOAIServicio
    {
        Task<RespuestaChatDTO> BusquedaConversacional(List<ChatHistoryDTO> historial, string UserName, string busqueda);
    }
}
