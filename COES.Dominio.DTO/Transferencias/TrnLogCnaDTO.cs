using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class TrnLogCnaDTO
    {
        public int Logcnacodi { get; set; }
        public int Emprcodi { get; set; }
        public DateTime? FechaProceso { get; set; }
        public int Cantcna { get; set; }
        public string Emprrazsocial { get; set; }
    }
}
