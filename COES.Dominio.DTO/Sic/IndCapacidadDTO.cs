using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCapacidadDTO
    {
        public int Indcpccodi { get; set; }
        public int Indcbrcodi { get; set; }
        public int Equicodicentral { get; set; }
        public int Equicodiunidad { get; set; }
        public int Grupocodi { get; set; }
        public int Famcodi { get; set; }
        public DateTime Indcpcfecinicio { get; set; }
        public DateTime Indcpcfecfin { get; set; }
        public int Indcpctipo { get; set; }
        public string Indcpcusucreacion { get; set; }
        public DateTime Indcpcfeccreacion { get; set; }
        public string Indcpcusumodificacion { get; set; }
        public DateTime Indcpcfecmodificacion { get; set; }


        //Additional
        public int Ipericodi { get; set; }
        public int Emprcodi { get; set; }
        public int Indcbrtipo { get; set; }
    }
}
