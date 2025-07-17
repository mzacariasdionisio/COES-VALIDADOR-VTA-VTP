using COES.Dominio.DTO.Busqueda;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IBCDCalificacionBusquedaRepository
    {
        void AddCalificacion(BCDCalificacionBusquedaDTO calificacion);
        BCDCalificacionBusquedaDTO BuscarCalificacionPorIdBusqueda(int Id_search_q);
        void RemoveCalificacion(BCDCalificacionBusquedaDTO calificacion);
        void UpdateCalificacion(BCDCalificacionBusquedaDTO calificacion);
    }
}
