using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDesviacionRepository
    {
        int Save(DesviacionDTO entity);
        List<DesviacionDTO> ListarDesviacion(DateTime fecha);
        void Delete(DateTime fecha);
    }
}
