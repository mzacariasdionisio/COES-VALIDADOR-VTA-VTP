using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDatacatalogoRepository
    {
        List<DataCatalogoDTO> GetParametria(int catcodi);
        List<DataSubestacionDTO> GetParamSubestacion();
        List<DataCatalogoDTO> GetParametriaAll();
    }
}
