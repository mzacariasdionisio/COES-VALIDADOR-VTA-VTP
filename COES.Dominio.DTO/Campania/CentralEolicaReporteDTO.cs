using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CentralEolicaReporteDTO
    {
        public List<RegHojaEolADTO> listaEolicaA { get; set; }
        public List<RegHojaEolBDTO> listaEolicaB { get; set; }
        public List<DetRegHojaEolCDTO> listaEolicaC { get; set; }

        public CentralEolicaReporteDTO()
        {
            listaEolicaA = new List<RegHojaEolADTO>();
            listaEolicaB = new List<RegHojaEolBDTO>();
            listaEolicaC = new List<DetRegHojaEolCDTO>();
        }
    }
}
