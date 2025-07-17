using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class ProDemandaReporteDTO
    {
        public List<FormatoD1ADTO> listaProDemandaA { get; set; }
        public List<FormatoD1BDTO> listaProDemandaB { get; set; }
        public List<FormatoD1CDTO> listaProDemandaC { get; set; }
        public List<FormatoD1DDTO> listaProDemandaD { get; set; }
        public ProDemandaReporteDTO()
        {
            listaProDemandaA = new List<FormatoD1ADTO>();
            listaProDemandaB = new List<FormatoD1BDTO>();
            listaProDemandaC = new List<FormatoD1CDTO>();
            listaProDemandaD = new List<FormatoD1DDTO>();
        }
    }
}
