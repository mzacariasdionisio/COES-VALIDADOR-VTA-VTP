using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnDemandaDTO
    {
        public int Demcodi { get; set; }
        public int Emprcodi { get; set; }
        public decimal Valormaximo { get; set; }
        public string Periododemanda { get; set; }
        public string Lastuser { get; set; }
        public DateTime? Lastdate { get; set; }
    }
}
