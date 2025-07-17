using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamObservacionRepository
    {
        List<ObservacionDTO> GetObservacionByProyCodi(int proyCodi);

        int SaveObservacion(ObservacionDTO Observacion);

        bool DeleteObservacionById(int id, string usuario);

        int GetLastObservacionId();

        ObservacionDTO GetObservacionById(int id);

        bool UpdateObservacion(ObservacionDTO ObservacionDTO);

        bool EnviarObservacionByProyecto(int idProyecto);

        bool GetObservacionByPlanCodi(int planCodi);

        bool UpdateObservacionByProyecto(int idProyecto, string estado);
    }
}
