using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace COES.Dominio.Interfaces.ReportesFrecuencia
{
    public interface IEquipoGPSRepository
    {
        EquipoGPSDTO GetById(int GPSCodi);
        List<EquipoGPSDTO> GetListaEquipoGPS();
        List<EquipoGPSDTO> GetListaEquipoGPSPorFiltro(int codEquipo, string IndOficial);
        int GetUltimoCodigoGenerado();
        int GetNumeroRegistrosPorEquipo(int GPSCodi);
        int ValidarNombreEquipoGPS(EquipoGPSDTO entity);
        EquipoGPSDTO SaveUpdate(EquipoGPSDTO entity);
        EquipoGPSDTO Eliminar(EquipoGPSDTO entity);
    }
}
