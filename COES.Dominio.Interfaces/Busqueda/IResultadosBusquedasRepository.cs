using COES.Dominio.DTO.Busqueda;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IResultadosBusquedasRepository
    {
        void GuardarTopResultados(List<BCDResultadosRecomendadosDTO> resultados);
    }
}
