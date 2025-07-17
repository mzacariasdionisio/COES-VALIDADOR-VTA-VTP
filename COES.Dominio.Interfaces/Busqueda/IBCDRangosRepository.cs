using COES.Dominio.DTO.Busqueda;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IBCDRangosRepository
    {
        BCDRangosDTO BuscarRango(string calificacion);
    }
}
