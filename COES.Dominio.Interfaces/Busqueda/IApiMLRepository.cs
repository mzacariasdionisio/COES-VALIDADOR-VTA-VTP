using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IApiMLRepository
    {
        Task<List<WbBusquedasDTO>> BuscarDocumentos(BCDBusquedasDTO busqueda, string Usuario);
    }
}
