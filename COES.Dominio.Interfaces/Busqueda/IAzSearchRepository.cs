using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IAzSearchRepository
    {
        Task<List<WbBusquedasDTO>> BuscarDocumentosAsync(BCDBusquedasDTO busqueda, string Usuario);
        void RestablecerIndexadorTabla();
        Task<List<string>> Sugerir(bool highlights, bool fuzzy, string term, string searchField);
    }
}
