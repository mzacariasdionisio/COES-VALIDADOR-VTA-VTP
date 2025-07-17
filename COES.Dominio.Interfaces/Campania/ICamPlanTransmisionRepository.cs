using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamPlanTransmisionRepository
    {


        List<PlanTransmisionDTO> GetPlanTransmision();

        bool SavePlanTransmision(PlanTransmisionDTO planTransmision);

        bool DeletePlanTransmisionById(int id, string usuario);

        int GetLastPlanTransmisionId();

        PlanTransmisionDTO GetPlanTransmisionById(int id);

        bool UpdatePlanTransmision(PlanTransmisionDTO planTransmision);

        List<PlanTransmisionDTO> GetPlanTransmisionByFilters(int planTransmision);

        List<PlanTransmisionDTO> GetPlanTransmisionByEstado(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl);

        List<PlanTransmisionDTO> GetPlanTransmisionByEstadoEmpresa(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl);

        bool DesactivatePlanById(int id, string vigencia);

        bool ActivatePlanById(int id, string vigencia);

        bool UpdatePlanEstadoById(int id, string planestao);

        bool UpdatePlanEstadoEnviarById(int id, string planestao, string correo);

        List<PlanTransmisionDTO> GetPlanTransmisionByEstadoVigente(string empresa, string estado, string periodo, string tipoproyecto, string subtipoproyecto, string observados, string estadoExcl);

        List<PlanTransmisionDTO> GetPlanTransmisionByVigente(string empresa, string estado, string periodo, string estadoExcl);
        
        bool UpdateProyRegById(int id);
    }
}
