using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Busqueda
{
    public class AzOAIServicio : IAzOAIServicio
    {
        private readonly AzOAIRepository _azOAIRepository;
        private readonly BCDBusquedasRepository _bCDBusquedasRepository;


        public AzOAIServicio()
        {
            _azOAIRepository = new AzOAIRepository();
            _bCDBusquedasRepository = new BCDBusquedasRepository();
        }

        /// <summary>
        /// Permite buscar documentos
        /// </summary>
        /// <param name="historial"></param>
        /// <param name="UserName"></param>
        /// <param name="busqueda"></param>
        /// <returns></returns>
        public async Task<RespuestaChatDTO> BusquedaConversacional(List<ChatHistoryDTO> historial, string UserName, string busqueda)
        {
            var resultados = await _azOAIRepository.BusquedaConversacional(historial);

            BCDBusquedasDTO nuevaBusqueda = new BCDBusquedasDTO
            {
                Search_text = busqueda,
                Search_type = false,
                Search_user = UserName,
            };

            await Task.Run(async () =>
            {
                int idCreada = _bCDBusquedasRepository.Add(nuevaBusqueda);
                return idCreada;
            });
            return resultados;
        }
    }
}
