using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CentralSolarReporteDTO
    {
        public List<SolHojaADTO> listaSolarA { get; set; }
        public List<SolHojaBDTO> listaSolarB { get; set; }
        public List<DetSolHojaCDTO> listaSolarC { get; set; }

        public CentralSolarReporteDTO()
        {
            listaSolarA = new List<SolHojaADTO>();
            listaSolarB = new List<SolHojaBDTO>();
            listaSolarC = new List<DetSolHojaCDTO>();
        }
    }
}