using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ICodigoRetiroDetalleRepository : IRepositoryBase
    {
        List<CodigoRetiroDetalleDTO> ListarCodigoRetiroDetalle(System.Int32 id);
    }
}
