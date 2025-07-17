using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCapacidadDiaDTO
    {
        public int Cpcdiacodi { get; set; }
        public int Indcpccodi { get; set; }
        public DateTime Cpcdiafecha { get; set; }
        public decimal? Cpcdiavalor { get; set; }
        public string Cpcdiausucreacion { get; set; }
        public DateTime Cpcdiafeccreacion { get; set; }

        //Additional
        public int Indcbrcodi { get; set; }
        public int Ipericodi { get; set; }
        public int Emprcodi { get; set; }
        public int Famcodi { get; set; }
        public int Indcbrtipo { get; set; }
        public int Equicodicentral { get; set; }
        public int Equicodiunidad { get; set; }
        public int Grupocodi { get; set; }
        public int Indcpctipo { get; set; }

    }
}
