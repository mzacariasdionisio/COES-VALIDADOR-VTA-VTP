using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda;
using System;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Busqueda
{
    public class WbRecomendacionServicio : IWbRecomendacionServicio
    {
        private readonly IAzSearchRepository _azSearchRepository;
        private readonly IAzDataTablesRepository _azDataTablesRepository;
        private readonly IBCDResultadosRecomendadosRepository _resultadosRecomendadosRepository;
        private readonly IBCDBusquedasRepository _bCDBusquedasRepository;
        private readonly IDocumentosAbiertosRepository _documentosAbiertosRepository;
        private readonly IResultadosRelacionadosRepository _resultadosRelacionadosRepository;

        public WbRecomendacionServicio()
        {
            _azSearchRepository = new AzSearchRepository();
            _azDataTablesRepository = new AzDataTablesRepository();
            _resultadosRecomendadosRepository = new BCDResultadosRecomendadosRepository();
            _bCDBusquedasRepository = new BCDBusquedasRepository();
            _documentosAbiertosRepository = new DocumentosAbiertosRepository();
            _resultadosRelacionadosRepository = new ResultadosRelacionadosRepository();
        }

        /// <summary>
        /// Permite agregar una recomendación
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <param name="idSearchRr"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public RespuestaDTO<string> SaveRecomendacion(WbBusquedasDTO recomendacion, int idSearchRr, string usuario)
        {
            try
            {
                BCDResultadosRecomendadosDTO resultado = MapearBusquedaAResultadoDTO(recomendacion, idSearchRr, usuario);
                BCDResultadosRecomendadosDTO registro = _resultadosRecomendadosRepository.BuscarRecomendacion(resultado);

                if (registro == null)
                {
                    _resultadosRecomendadosRepository.AddRecomendacion(resultado);

                    BCDBusquedasDTO busqueda = _bCDBusquedasRepository.BusquedaPorId(resultado.Id_search_rr);
                    string mensaje = "Registro agregado correctamente. ";
                    try
                    {
                        var guardarFraseTask = Task.Run(() =>
                            _azDataTablesRepository.GuardarFraseComoConceptoClaveAsync(resultado.Index_rowkey, busqueda.Search_text)
                            );
                        mensaje += "Guardando conceptos clave... ";
                        var restablecerIndexadorTask = Task.Run(() =>
                        _azSearchRepository.RestablecerIndexadorTabla()
                        );

                        mensaje += "Restableciendo índice... ";

                        Task.WhenAll(guardarFraseTask, restablecerIndexadorTask)
                            .ContinueWith(task =>
                            {
                                if (task.IsFaulted)
                                {
                                    Console.WriteLine("Ocurrió un error en las tareas en segundo plano.");
                                    foreach (var ex in task.Exception.InnerExceptions)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            });
                    }
                    catch (Exception e)
                    {
                        mensaje += "Sucedió un error:" + e.Message;
                        Console.WriteLine(e.Message);
                    }
                    return new RespuestaDTO<string>(true, mensaje, null);
                }

                _resultadosRecomendadosRepository.DeleteRecomendacion(registro);
                return new RespuestaDTO<string>(true, "Recomendación removida. ", null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new RespuestaDTO<string>(false, "Sucedió un error: " + e.Message, null);
            }
        }

        /// <summary>
        /// Permite guardar o eliminar un registro relacionado
        /// </summary>
        /// <param name="resultado"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO<string>> SaveRelacion(WbBusquedasDTO resultadoDTO, int idSearchRr, string usuario)
        {
            return await Task.Run(() =>
            {
                BCDResultadosRecomendadosDTO resultado = MapearBusquedaAResultadoDTO(resultadoDTO, idSearchRr, usuario);

                BCDResultadosRecomendadosDTO registro = _resultadosRelacionadosRepository.BuscarRelacionado(resultado);
                if (registro != null)
                {
                    _resultadosRelacionadosRepository.RemoveRelacionado(registro);
                    return new RespuestaDTO<string>(true, "Relacionamiento eliminado", null);
                }
                try
                {
                    _resultadosRelacionadosRepository.AddRelacionado(resultado);
                    return new RespuestaDTO<string>(true, "Relacionamiento almacenado", null);
                }
                catch (Exception ex)
                {
                    return new RespuestaDTO<string>(false, "Sucedió un error: " + ex.Message, null);
                }
            });
        }

        /// <summary>
        /// Permite guardar un documento que ha sido abierto
        /// </summary>
        /// <param name="resultadoDTO"></param>
        /// <param name="idSearchRr"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO<string>> GuardarDocumentoAbierto(WbBusquedasDTO resultadoDTO, int idSearchRr, string usuario)
        {
            BCDResultadosRecomendadosDTO result = MapearBusquedaAResultadoDTO(resultadoDTO, idSearchRr, usuario);

            return await Task.Run(() =>
            {
                try
                {
                    BCDResultadosRecomendadosDTO registro = _documentosAbiertosRepository.BuscarDocumentoAbierto(result);

                    if (registro != null)
                    {
                        return new RespuestaDTO<string>(false, "Documento ya registrado anteriormente", null);
                    }

                    _documentosAbiertosRepository.AddDocumentoAbierto(result);
                    return new RespuestaDTO<string>(true, "Registro almacenado", null);
                }
                catch (Exception ex)
                {
                    return new RespuestaDTO<string>(false, "Sucedió un error: " + ex.Message, null);
                }
            });
        }

        /// <summary>
        /// Permite mapear WbBusquedasDTO, idSearchRr con usuario a BCDResultadosRecomendadosDTO
        /// </summary>
        /// <param name="resultadoDTO"></param>
        /// <param name="idSearchRr"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private BCDResultadosRecomendadosDTO MapearBusquedaAResultadoDTO(WbBusquedasDTO resultadoDTO, int idSearchRr, string usuario)
        {
            BCDResultadosRecomendadosDTO resultadoMapeado = new BCDResultadosRecomendadosDTO
            {
                Id_search_rr = idSearchRr,
                Index_rowkey = resultadoDTO.RowKey,
                Document_path = resultadoDTO.RutaSharePointOnline,
                Document_name = resultadoDTO.NombreArchivoConExtension,
                Confidence = (decimal?)resultadoDTO.score,
                Recommend_by = usuario
            };

            return resultadoMapeado;
        }
    }
}
