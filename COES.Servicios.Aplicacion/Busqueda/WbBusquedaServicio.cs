using COES.Dominio.DTO.Busqueda;
using COES.Dominio.Interfaces.Busqueda;
using COES.Infraestructura.Datos.Repositorio.Busqueda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COES.Servicios.Aplicacion.Busqueda
{
    public class WbBusquedaServicio : IWbBusquedaServicio
    {
        private readonly IBCDBusquedasRepository _bCDBusquedasRepository;
        private readonly IBCDCalificacionBusquedaRepository _bCDCalificacionBusquedaRepository;
        private readonly IBCDRangosRepository _bCDRangosRepository;
        private readonly IResultadosBusquedasRepository _resultadosBusquedasRepository;

        public WbBusquedaServicio()
        {
            _bCDBusquedasRepository = new BCDBusquedasRepository();
            _bCDCalificacionBusquedaRepository = new BCDCalificacionBusquedaRepository();
            _bCDRangosRepository = new BCDRangosRepository();
            _resultadosBusquedasRepository = new ResultadosBusquedasRepository();
        }

        /// <summary>
        /// Permite guardar una busqueda
        /// </summary>
        /// <param name="busquedaDTO"></param>
        /// <returns></returns>
        public int AlmacenarBusqueda(BCDBusquedasDTO busquedaDTO)
        {
            return _bCDBusquedasRepository.Add(busquedaDTO);
        }

        /// <summary>
        /// Permite guardar la utilidad de los resultados de una busqueda
        /// </summary>
        /// <param name="idBusqueda"></param>
        /// <param name="calificacion"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO<string>> CalificarBusqueda(string idBusqueda, string calificacion)
        {
            if (!int.TryParse(idBusqueda, out int idSearch))
            {
                return new RespuestaDTO<string>(false, "El ID de búsqueda no es válido", null);
            }

            try
            {
                string message = "";
                bool existeBusqueda = _bCDBusquedasRepository.BuscarBusqueda(idSearch);
                BCDRangosDTO Rango = _bCDRangosRepository.BuscarRango(calificacion);

                if (!existeBusqueda)
                {
                    return new RespuestaDTO<string>(false, "La búsqueda no existe", null);
                }

                if (Rango == null)
                {
                    return new RespuestaDTO<string>(false, "El rango no es válido", null);
                }

                int rango = Rango.Id_range;

                BCDCalificacionBusquedaDTO calificacionExistente = _bCDCalificacionBusquedaRepository.BuscarCalificacionPorIdBusqueda(idSearch);

                // Si no existe calificación, se agrega
                if (calificacionExistente == null)
                {
                    BCDCalificacionBusquedaDTO registro = new BCDCalificacionBusquedaDTO
                    {
                        Id_range_q = rango,
                        Id_search_q = idSearch
                    };
                    _bCDCalificacionBusquedaRepository.AddCalificacion(registro);
                    message = "Calificación agregada correctamente";
                }
                // Si el rango es igual, se elimina el registro
                else if (calificacionExistente.Id_range_q == rango)
                {
                    _bCDCalificacionBusquedaRepository.RemoveCalificacion(calificacionExistente);
                    message = "Calificación removida correctamente";
                }
                // Si el rango es diferente, se actualiza
                else
                {
                    calificacionExistente.Id_range_q = rango;
                    _bCDCalificacionBusquedaRepository.UpdateCalificacion(calificacionExistente);
                    message = "Calificación actualizada correctamente";
                }

                return new RespuestaDTO<string>(true, message, null);
            }
            catch (Exception e)
            {
                return new RespuestaDTO<string>(false, "Sucedió un error: " + e.Message, null);
            }
        }

        /// <summary>
        /// Permite guardar un top de resultados
        /// </summary>
        /// <param name="resultados"></param>
        /// <param name="idSearch"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO<string>> GuardarTopResultados(List<WbBusquedasDTO> resultados, int idSearch, string userName)
        {
            if (resultados == null || !resultados.Any())
            {
                return new RespuestaDTO<string>(false, "No hay resultados para guardar", null);
            }
            resultados = resultados.Take(10).ToList();

            return await Task.Run(() =>
            {
                try
                {
                    var registros = resultados.Select(resultado => new BCDResultadosRecomendadosDTO
                    {
                        Id_document = 0,
                        Id_search_rr = idSearch,
                        Index_rowkey = resultado.RowKey,
                        Document_path = resultado.RutaSharePointOnline,
                        Document_name = resultado.metadata_storage_name,
                        Confidence = (decimal?)resultado.score,
                        Recommend_by = userName
                    }).ToList();

                    _resultadosBusquedasRepository.GuardarTopResultados(registros);
                    return new RespuestaDTO<string>(true, "Top guardado correctamente", null);
                }
                catch (Exception ex)
                {
                    return new RespuestaDTO<string>(false, "Sucedió un error: " + ex.Message, null);
                }
            });
        }

        /// <summary>
        /// Permite guardar la relación entre los resultados de una busqueda
        /// </summary>
        /// <param name="idBusqueda"></param>
        /// <param name="seleccion"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO<string>> GuardarSeleccionRelacionados(string idBusqueda, bool seleccion)
        {
            if (!int.TryParse(idBusqueda, out int idSearch))
            {
                return new RespuestaDTO<string>(false, "El ID de búsqueda no es válido", null);
            }

            return await Task.Run(() =>
            {
                try
                {
                    BCDBusquedasDTO registro = _bCDBusquedasRepository.BusquedaPorId(idSearch);

                    if (registro == null)
                    {
                        return new RespuestaDTO<string>(false, "No se encontró la búsqueda relacionada", null);
                    }

                    registro.Search_relation = seleccion;

                    _bCDBusquedasRepository.UpdateBusqueda(registro);

                    string mensaje = seleccion ? "Relacionamiento activo" : "Relacionamiento inactivo";
                    return new RespuestaDTO<string>(true, mensaje, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new RespuestaDTO<string>(false, "Sucedió un error: " + ex.Message, null);
                }
            });
        }

        /// <summary>
        /// Permite listar las busquedas entre un periodo de tiempo
        /// </summary>
        /// <param name="start_date"></param>
        /// <param name="end_date"></param>
        /// <returns></returns>
        public async Task<List<BCDBusquedasDTO>> ObtenerBusquedas(DateTime start_date, DateTime end_date)
        {
            return _bCDBusquedasRepository.ObtenerBusquedas(start_date, end_date);
        }
    }
}
