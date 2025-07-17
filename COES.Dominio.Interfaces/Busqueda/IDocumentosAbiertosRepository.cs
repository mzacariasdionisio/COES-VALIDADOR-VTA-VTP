using COES.Dominio.DTO.Busqueda;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IDocumentosAbiertosRepository
    {
        void AddDocumentoAbierto(BCDResultadosRecomendadosDTO result);
        BCDResultadosRecomendadosDTO BuscarDocumentoAbierto(BCDResultadosRecomendadosDTO result);
    }
}
