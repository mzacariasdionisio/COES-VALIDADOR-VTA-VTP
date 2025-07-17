using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamRespObsRepository
    {

        List<RespuestaObsDTO> GetRespuestaObsByObs(int observacion);

        bool SaveRespuestaObs(RespuestaObsDTO respuestaObsDTO);

        bool DeleteRespuestaObsById(int id, string usuario);

        int GetLastRespuestaObsId();

        RespuestaObsDTO GetRespuestaObsById(int id);

        bool UpdateRespuestaObs(RespuestaObsDTO RespuestaObsDTO);

    }
}
