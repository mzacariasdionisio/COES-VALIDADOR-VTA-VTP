using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamTransmisionProyectoRepository
    {


        List<TransmisionProyectoDTO> GetTransmisionProyecto(int id);

        bool SaveTransmisionProyecto(TransmisionProyectoDTO transmisionProy);

        bool DeleteTransmisionProyectoById(int id, string usuario);

        int GetLastTransmisionProyectoId();

        TransmisionProyectoDTO GetTransmisionProyectoById(int id);

        bool UpdateTransmisionProyecto(TransmisionProyectoDTO transmisionProy);
        
        List<TransmisionProyectoDTO> GetTransmisionProyectoByPeriodo(int id);

        List<TransmisionProyectoDTO> GetTransmisionProyectoByPeriodoFilter(string pericodi, string empresa, string estado);

        bool UpdateProyEstadoById(int id, string proyestado);

        bool UpdateProyEstadoByIdProy(int id, string proyestado, string proyestadoini);

        bool UpdateProyFechaEnvioObsById(int id);
    }
}
