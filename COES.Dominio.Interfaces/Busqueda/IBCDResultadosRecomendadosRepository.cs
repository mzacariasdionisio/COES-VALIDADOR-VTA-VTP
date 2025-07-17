using COES.Dominio.DTO.Busqueda;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IBCDResultadosRecomendadosRepository
    {
        bool AddRecomendacion(BCDResultadosRecomendadosDTO resultado);
        BCDResultadosRecomendadosDTO BuscarRecomendacion(BCDResultadosRecomendadosDTO resultado);
        void DeleteRecomendacion(BCDResultadosRecomendadosDTO resultado);
    }
}
