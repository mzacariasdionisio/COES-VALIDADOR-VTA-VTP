using COES.Dominio.DTO.Busqueda;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IWbBusquedaServicio
    {
        int AlmacenarBusqueda(BCDBusquedasDTO busquedaDTO);
        Task<RespuestaDTO<string>> CalificarBusqueda(string idBusqueda, string calificacion);
        Task<RespuestaDTO<string>> GuardarSeleccionRelacionados(string idBusqueda, bool seleccion);
        Task<RespuestaDTO<string>> GuardarTopResultados(List<WbBusquedasDTO> resultados, int idSearch, string userName);
        Task<List<BCDBusquedasDTO>> ObtenerBusquedas(DateTime start_date, DateTime end_date);
    }
}
