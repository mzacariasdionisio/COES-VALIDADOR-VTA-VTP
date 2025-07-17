using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class MapEmpresaulDTO
    {
        public int Empulcodi { get; set; }
        public DateTime Empulfecha { get; set; }
        public Decimal Empuldesv { get; set; }
        public Decimal Empulprog { get; set; }
        public Decimal Empulejec { get; set; }
        public int Tipoccodi { get; set; }
        public int Vermcodi {get; set;}
        public int Emprcodi { get; set; }
        public int Ptomedicodi { get; set; }

        public int Equicodi { get; set; }
        public string Equiabrev { get; set; }
        public string Areanomb { get; set; }
        public decimal? Equitension { get; set; }
        public string Emprnomb { get; set; }
        public string Tipocdesc { get; set; }
        public int Barrcodi { get; set; }
    }
}
