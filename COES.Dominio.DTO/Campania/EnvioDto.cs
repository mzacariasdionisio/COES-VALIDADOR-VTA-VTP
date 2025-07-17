using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class EnvioDto
    {

        public PlanTransmisionDTO PlanTransmision { get; set; }
        public TransmisionProyectoDTO TransmisionProyectoDTO { get; set; }
        public List<TransmisionProyectoDTO> ListaTrsmProyecto { get; set; }
        public string Comentarios { get; set; }

        public string Correos { get; set; }



    }
}
