using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CentralTermicaReporteDTO
    {
        public List<RegHojaCCTTADTO> listaTermicaA { get; set; }
        public List<RegHojaCCTTBDTO> listaTermicaB { get; set; }
        public List<Det1RegHojaCCTTCDTO> listaTermicaC { get; set; }
        public List<Det2RegHojaCCTTCDTO> listaTermicaC2 { get; set; }

        public CentralTermicaReporteDTO()
        {
            listaTermicaA = new List<RegHojaCCTTADTO>();
            listaTermicaB = new List<RegHojaCCTTBDTO>();
            listaTermicaC = new List<Det1RegHojaCCTTCDTO>();
            listaTermicaC2 = new List<Det2RegHojaCCTTCDTO>();
        }
    }
}