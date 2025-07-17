using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamTipoproyectoRepository
    {

        List<TipoProyectoDTO> GetTipoProyecto();
    }
}
