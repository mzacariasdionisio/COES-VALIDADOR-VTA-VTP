using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class PrnConfiguracionDiaDTO
    {
        public int Cnfdiacodi { get; set; }
        public DateTime Cnfdiafecha { get; set; }
        public string Cnfdiaferiado { get; set; }
        public string Cnfdiaatipico { get; set; }
        public string Cnfdiaveda { get; set; }
    }
}
