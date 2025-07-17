using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Busqueda
{
    public class AzSearchService : IAzSearchServicio
    {
        private readonly IApiMLRepository _wbMLApiRepository;
        private readonly IAzSearchRepository _azureSearchService;
        private readonly IAzDataTablesRepository _wbAzDataTableRepositorio;
        private readonly IBCDBusquedasRepository _bCDBusquedasRepository;

        public AzSearchService()
        {
            _wbMLApiRepository = new ApiMLRepository();
            _azureSearchService = new AzSearchRepository();
            _wbAzDataTableRepositorio = new AzDataTablesRepository();
            _bCDBusquedasRepository = new BCDBusquedasRepository();
        }

        #region Métodos del servicio de Azure Search

        /// <summary>
        /// Permite buscar documentos
        /// </summary>
        /// <param name="busqueda"></param>
        /// <param name="Usuario"></param>
        /// <returns></returns>
        public async Task<List<WbBusquedasDTO>> BuscarDocumentosAsync(BCDBusquedasDTO busqueda, string Usuario)
        {
            Task<List<WbBusquedasDTO>> resultadosML = _wbMLApiRepository.BuscarDocumentos(busqueda, Usuario);
            if (resultadosML.Result.Any())
            {
                return resultadosML.Result;
            }
            return await _azureSearchService.BuscarDocumentosAsync(busqueda, Usuario).ConfigureAwait(false);
        }

        /// <summary>
        /// Permite restablecer el indexador
        /// </summary>
        /// <returns></returns>
        public void RestablecerIndexadorTabla()
        {
            _azureSearchService.RestablecerIndexadorTabla();
        }

        /// <summary>
        /// Permite sugerir texto
        /// </summary>
        /// <param name="highlights"></param>
        /// <param name="fuzzy"></param>
        /// <param name="term"></param>
        /// <param name="searchField"></param>
        /// <returns></returns>
        public async Task<List<string>> Sugerir(bool highlights, bool fuzzy, string term, string searchField)
        {
            var resultados = await _azureSearchService.Sugerir(highlights, fuzzy, term, searchField).ConfigureAwait(false);
            if (resultados.Count() < 1)
            {
                resultados = _bCDBusquedasRepository.QuizaQuisoDecir(term);
            }
            return resultados;
        }

        #endregion

        #region Métodos del servicio de Azure Datatables

        public async Task<RespuestaDTO<string>> ActualizarPalabrasClaveAsync(string rowKey, string palabrasClave)
        {
            AzDataTableDTO registro = await _wbAzDataTableRepositorio.ObtenerRegistroAsync(rowKey);
            if (registro == null)
            {
                return new RespuestaDTO<string>(false, "No se encontró el registro. ");
            }
            else
            {
                registro.PalabrasClave = palabrasClave;

                if (_wbAzDataTableRepositorio.ActualizarRegistro(registro))
                {
                    return new RespuestaDTO<string>(true, "Registro actualizado. ");
                }
                else
                {
                    return new RespuestaDTO<string>(false, "Falló al actualizar el registro. ");
                }
            }
        }

        public async Task<List<string>> ObtenerTiposDocumento()
        {
            return await _wbAzDataTableRepositorio.ObtenerTiposDocumento();
        }

        #endregion
    }
}
