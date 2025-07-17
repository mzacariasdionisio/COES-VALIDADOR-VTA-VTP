using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IIndTransporteDetalleRepository
    {
        int Save(IndTransporteDetalleDTO entity);
        void DeleteByCapacidadTransporte(int cpctnscodi);
        List<IndTransporteDetalleDTO> ListTransporteDetalle(int cpctnscodi);
        List<IndTransporteDetalleDTO> ReporteIncumplimientoByPeriodo(int ipericodi);
    }
}
