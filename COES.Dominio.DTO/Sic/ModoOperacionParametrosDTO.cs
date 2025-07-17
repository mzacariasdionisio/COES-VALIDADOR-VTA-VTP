using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Sic
{
    public class ModoOperacionParametrosDTO
    {
        public int Grupocodi { get; set; }
        public string Gruponomb { get; set; }
        public string Combustible { get; set; }
        public decimal? PotenciaMinima { get; set; }
        public decimal? PotenciaEfectiva { get; set; }
        public decimal? A { get; set; }
        public decimal? B { get; set; }
        public decimal? C { get; set; }
        public decimal? CeCalor { get; set; }
        public decimal? Lhv { get; set; }
        public decimal? RendimientoNominal { get; set; }

    }
}
