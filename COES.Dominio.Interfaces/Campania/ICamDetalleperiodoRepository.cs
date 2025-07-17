
using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamDetalleperiodoRepository
    {
        bool SaveDetalle(DetallePeriodoDTO fichPro);
        bool DeleteDetalleById(int id, string usuario);
        int GetDetalleId();
        List<DetallePeriodoDTO> GetDetalleByPericodi(int pericodi, string inddel);

        List<int> GetDetalleHojaByPericodi(int pericodi, string inddel);

    }
}
