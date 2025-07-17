using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class IndCapacidadTransporteDTO
    {
        public int Cpctnscodi { get; set; }
        public int Emprcodi { get; set; }
        public int Ipericodi { get; set; }
        public string Cpctnsusucreacion { get; set; }
        public DateTime Cpctnsfeccreacion { get; set; }
        public string Cpctnsusumodificacion { get; set; }
        public DateTime Cpctnsfecmodificacion { get; set; }
    }
}
