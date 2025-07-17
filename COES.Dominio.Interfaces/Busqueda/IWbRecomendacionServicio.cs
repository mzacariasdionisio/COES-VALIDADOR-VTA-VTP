using COES.Dominio.DTO.Busqueda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Busqueda
{
    public interface IWbRecomendacionServicio
    {
        Task<RespuestaDTO<string>> GuardarDocumentoAbierto(WbBusquedasDTO resultadoDTO, int idSearchRr, string usuario);
        RespuestaDTO<string> SaveRecomendacion(WbBusquedasDTO recomendacion, int idSearchRr, string usuario);
        Task<RespuestaDTO<string>> SaveRelacion(WbBusquedasDTO resultadoDTO, int idSearchRr, string usuario);
    }
}
