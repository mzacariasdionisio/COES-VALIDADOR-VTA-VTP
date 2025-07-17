using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnContadorCorreosCnaDTO
    {
        public int Contcnacodi { get; set; }
        public int Emprcodi { get; set; }
        public int Cantcorreos { get; set; }
        public DateTime? Lastdate { get; set; }
        public string Lastuser { get; set; }
    }
}
