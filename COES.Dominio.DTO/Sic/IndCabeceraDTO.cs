using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCabeceraDTO
    {
        public int Indcbrcodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ipericodi { get; set; }
        public int Indcbrtipo { get; set; }
        public string Indcbrusucreacion { get; set; }
        public DateTime Indcbrfeccreacion { get; set; }
    }
}
