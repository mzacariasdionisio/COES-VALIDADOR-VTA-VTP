using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Campania
{
    public class CentralHidroReporteDTO
    {
        public List<RegHojaADTO> listaHidroA { get; set; }
        public List<RegHojaBDTO> listaHidroB { get; set; }
        public List<DetRegHojaCDTO> listaHidroC { get; set; }
        public List<RegHojaDDTO> listaFichaD { get; set; }


        public CentralHidroReporteDTO()
        {
            listaHidroA = new List<RegHojaADTO>();
            listaHidroB = new List<RegHojaBDTO>();
            listaHidroC = new List<DetRegHojaCDTO>();
            listaFichaD = new List<RegHojaDDTO>();
        }
    }
}
