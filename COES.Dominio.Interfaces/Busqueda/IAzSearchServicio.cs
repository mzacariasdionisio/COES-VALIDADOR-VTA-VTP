using COES.Dominio.DTO.Busqueda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IAzSearchServicio
    {
        Task<RespuestaDTO<string>> ActualizarPalabrasClaveAsync(string rowKey, string palabrasClave);
        Task<List<WbBusquedasDTO>> BuscarDocumentosAsync(BCDBusquedasDTO busqueda, string Usuario);
        Task<List<string>> ObtenerTiposDocumento();
        void RestablecerIndexadorTabla();
        Task<List<string>> Sugerir(bool highlights, bool fuzzy, string term, string searchField);
    }
}
