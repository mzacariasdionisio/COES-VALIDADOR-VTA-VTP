using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.DTO.Transferencias
{
    public class CostoMarginalGraficoValoresDTO
    {
        public int BarrCodi { get; set; }
        public string FechaColumna { get; set; }
        public decimal? CMGRTotal { get; set; }
        public decimal? CMGRCongestion { get; set; }
        public decimal? CMGREnergia { get; set; }
        public decimal? CMGRPromedio { get; set; }
        public DateTime FechaIntervalo { get; set; }
    }
}
