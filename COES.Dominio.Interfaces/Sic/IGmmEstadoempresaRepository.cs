using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IGmmEstadoEmpresaRepository
    {
        int Save(GmmEstadoEmpresaDTO entity);
    }
}
