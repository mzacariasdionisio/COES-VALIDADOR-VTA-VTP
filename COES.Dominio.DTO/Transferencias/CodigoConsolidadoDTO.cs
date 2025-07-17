using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CodigoConsolidadoDTO
    {
        public int Codcncodi { get; set; }
        public int Emprcodi { get; set; }
        public string Empresa { get; set; }
        public int Clicodi { get; set; }
        public string Cliente { get; set; }
        public int Barrcodi { get; set; }
        public string Barra { get; set; }
        public string Codcncodivtp { get; set; }
        public int Codcnpotegre { get; set; }
        public int Tipusucodi { get; set; }
        public int Tipconcodi { get; set; }
        public string Tipusunombre { get; set; }
        public string Tipconnombre { get; set; }
    }
}
