using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ICodigoRetiroEquivalenciaDetalleRepository
    {
        int Save(CodigoRetiroRelacionDetalleDTO entity);
        int Delete(int id);
        List<CodigoRetiroRelacionDetalleDTO> ListarRelacionCodigoRetiros(List<int> idArray);
        List<CodigoRetiroRelacionDetalleDTO> ListarRelacionDetalle(List<int> idArray);
        List<CodigoRetiroRelacionDetalleDTO> GetById(int id);
        CodigoRetiroRelacionDetalleDTO GetRelacionDetallePorVTEA(int coresCodVTEA);

        bool ExisteVTEA(int id);
        bool ExisteVTP(int id);

    }

}
